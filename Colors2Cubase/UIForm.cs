﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using System.Windows.Forms.VisualStyles;
using System.Xml;
using System.Xml.Linq;

using CommonUtils;

namespace Colors2Cubase
{
	/// <summary>
	/// Description of UIForm.
	/// </summary>
	public partial class UIForm : Form
	{
		List<Color> colors = new List<Color>();

		public UIForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
		}
		
		void btnBrowseClick(object sender, EventArgs e)
		{
			// try to find Cubase's Defaults.xml file
			string fileName = "Defaults.xml";
			
			// E.g. C:\Users\perivar\AppData\Roaming\Steinberg\Cubase 7.5_64\Defaults.xml
			string userAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string cubaseConfigDir = Path.Combine(userAppDataFolder, "Steinberg");
			if (Directory.Exists(cubaseConfigDir))
			{
				var files = IOUtils.GetFilesRecursive(cubaseConfigDir, fileName);
				if (files.Count() == 1) {
					openFileDialog.InitialDirectory = new FileInfo(files.FirstOrDefault()).DirectoryName;
				} else {
					// choose the config file that was changes last
					var fileInfos = files.Select(p => new FileInfo(p)).OrderByDescending(t => t.LastWriteTime).ToList();
					openFileDialog.InitialDirectory = fileInfos.FirstOrDefault().DirectoryName;
				}
			}
			
			openFileDialog.FileName = fileName;
			openFileDialog.Filter = "Xml Files (*.xml)|*.xml|All files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				txtFilePath.Text = openFileDialog.FileName;
			}
		}

		void txtInputTextChanged(object sender, EventArgs e)
		{
			// parse the text
			string textToParse= txtInput.Text.Trim();
			
			// reset the color list
			colors.Clear();
			
			// match RGB codes in three digit comma separated groups
			// e.g.
			// "[[88,101,121],"
			// "[112,219,69],"
			// "[146,72,63]]"
			// rgb(83,114,110)
			string patternRGB = @"(\d+),(\d+),(\d+)"; // \G doesn't work?!
			foreach (Match match in Regex.Matches(textToParse, patternRGB)) {
				int r = int.Parse(match.Groups[1].Value);
				int g = int.Parse(match.Groups[2].Value);
				int b = int.Parse(match.Groups[3].Value);
				if (r < 256 && g < 256 && b < 256) {
					colors.Add(Color.FromArgb(r, g, b));
				}
			}

			// match HTML type RGB codes (called Hex codes, such as #FFCC66)
			// e.g.
			// ["#53726E",
			// "#80DE41",
			// #CF5E3D
			string patternHexCode = @"#(?:[0-9a-fA-F]{3}){1,2}";
			foreach (Match match in Regex.Matches(textToParse, patternHexCode)) {
				string htmlColor = match.Groups[0].Value;
				Color col = ColorTranslator.FromHtml(htmlColor);
				colors.Add(col);
			}
			
			// match Cubase Uint type color xml fragments
			// e.g.
			// value="4291172633"
			string patternUintColorCode = @"value=""(\d{10})""";
			foreach (Match match in Regex.Matches(textToParse, patternUintColorCode)) {
				string uintColor = match.Groups[1].Value;
				uint c = uint.Parse(uintColor);
				Color col = UintToColor(c);
				colors.Add(col);
			}

			DrawColors(colors, pictureBox1.Width, pictureBox1.Height);
		}
		
		void btnChangeCubaseClick(object sender, EventArgs e)
		{
			string fileName = txtFilePath.Text;
			
			// first check that we are able to find the right node in the
			// Defaults.xml file
			XElement colorListNode = ReadColorListFromCubaseConfig(fileName);
			if (colorListNode != null) {
				
				// build up the xml fragment that is required
				// <list name="Set" type="list">
				// 	<item>
				// 		<string name="Name" value="Color 1" wide="true"/>
				// 		<int name="Color" value="4284797946"/>
				// 	</item>
				// </list>
				
				//XmlDocument xmlNewDoc = CreateXmlUsingXmlElements();
				//xmlNewDoc.Save("XmlUsingXmlElements.xml");
				
				XElement replaceWithXml = CreateXmlUsingXElements();
				//replaceWithXml.Save("XmlUsingXElements.xml");
				
				// first make a backup of the config file
				IOUtils.MakeBackupOfFile(fileName);
				
				// replace node
				// http://stackoverflow.com/questions/5820143/how-can-i-update-replace-an-element-of-an-xelement-from-a-string
				// Replace Xml Node with Raw Xml in .Net http://omegacoder.com/?p=103
				colorListNode.ReplaceWith(replaceWithXml);
				
				// Save XML and disable the UTF-8 BOM bytes at the top of the Xml document (EF BB BF)
				// which is actually discouraged by the Unicode standard
				XmlUtils.SaveXDocument(replaceWithXml.Document, fileName, true);
				
				MessageBox.Show("Succesfully update the Cubase configuration file!", "Succesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		/// <summary>
		/// A more modern way of building up XML hiearchies using XElement nesting.
		/// </summary>
		/// <seealso cref="http://www.intertech.com/Blog/create-an-xml-document-with-linq-to-xml/" />
		/// <returns>a XElement tree</returns>
		private XElement CreateXmlUsingXElements() {
			
			var counter = 1;
			XElement xElement = new XElement("list",
			                                 new XAttribute("name", "Set"),
			                                 new XAttribute("type", "list"),
			                                 from c in colors
			                                 select new XElement("item",
			                                                     new XElement("string",
			                                                                  new XAttribute("name", "Name"),
			                                                                  new XAttribute("value", String.Format("Color {0}", counter++)),
			                                                                  new XAttribute("wide", "true")),
			                                                     new XElement("int",
			                                                                  new XAttribute("name", "Color"),
			                                                                  new XAttribute("value", ColorToUint(c)))
			                                                    )
			                                );

			return xElement;
		}
		
		/// <summary>
		/// A more traditional way (read Old) of building up XML hiearchies.
		/// This meethod uses the "old" style XmlElement CreateElement and SetAttribute Methods
		/// </summary>
		/// <returns>XmlDocument</returns>
		private XmlDocument CreateXmlUsingXmlElements() {
			
			// Create the XmlDocument with default content
			XmlDocument xmlDoc = new XmlDocument();
			
			XmlElement listNode = xmlDoc.CreateElement("list");
			listNode.SetAttribute("name", "Set");
			listNode.SetAttribute("type", "list");
			xmlDoc.AppendChild(listNode);

			int colorCount = 1;
			foreach (var c in colors) {
				XmlElement itemNode = xmlDoc.CreateElement("item");
				listNode.AppendChild(itemNode);
				
				XmlElement stringNode = xmlDoc.CreateElement("string");
				stringNode.SetAttribute("name", "Name");
				stringNode.SetAttribute("value", String.Format("Color {0}", colorCount));
				stringNode.SetAttribute("wide", "true");
				
				XmlElement intNode = xmlDoc.CreateElement("int");
				intNode.SetAttribute("name", "Color");
				intNode.SetAttribute("value", "" + ColorToUint(c));
				
				itemNode.AppendChild(stringNode);
				itemNode.AppendChild(intNode);
				
				colorCount++;
			}
			
			return xmlDoc;
		}
		
		/// <summary>
		/// Use LINQ to XML for finding a specific node (with children) in a Xml file
		/// </summary>
		/// <param name="fileName">filepath to xml document</param>
		/// <returns>A XElement with the CubaseColorList xml</returns>
		private XElement ReadColorListFromCubaseConfig(string fileName) {
			
			if (!fileName.Equals("")) {
				if (File.Exists(fileName)) {

					XDocument xDoc = XDocument.Load(fileName, LoadOptions.PreserveWhitespace);
					
					// find  <obj class="UColorSet" name="Event Colors" ID="?????">
					// return IEnumerable<XElement>
					var pUColorSetNode = from xn in xDoc.Descendants("obj")
						where (string) xn.Attribute("class") == "UColorSet"
						&& (string) xn.Attribute("name") == "Event Colors"
						select xn;
					
					// find direct child list node <list name="Set" type="list">
					// return IEnumerable<XElement>
					var pEventColorsSetNode = from xn in pUColorSetNode.Elements("list")
						where (string) xn.Attribute("name") == "Set"
						&& (string) xn.Attribute("type") == "list"
						select xn;
					
					if (pEventColorsSetNode.Count() == 0) {
						MessageBox.Show("Failed reading the configuration file. Maybe Cubase has changed the format. Please contact the developer.",
						                "Failed reading configuration file",
						                MessageBoxButtons.OK,
						                MessageBoxIcon.Error);
						return null;
					}
					
					return pEventColorsSetNode.FirstOrDefault();
					
				} else {
					MessageBox.Show("Failed reading the selected file", "Error reading file", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} else {
				MessageBox.Show("You must select the Cubase Default.xml config file first", "No file selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return null;
		}
		
		private uint ColorToUint(Color color) {
			// 4278190080 in base 16 is FF000000
			// ((R<<16) | (G<<8) | (B))
			//uint colorValue = ((uint) color.R) | ((uint) (color.G << 8)) | ((uint) (color.B << 16)) | 0xFF000000;
			uint colorValue = ((uint) color.B) | ((uint) (color.G << 8)) | ((uint) (color.R << 16)) | 0xFF000000;
			return colorValue;
		}

		private Color UintToColor(uint colorValue) {
			return ColorUtils.UIntToColor(colorValue);
		}
		
		private void DrawColors(List<Color> colors, int width, int height, int columns = 5) {
			
			var bmp = new Bitmap(width, height);
			using (var g = Graphics.FromImage(bmp))
			{
				int count = colors.Count;
				int rows = (count - 1) / columns + 1;
				
				int frameWidth = width / columns;
				int frameHeight = height / rows;
				
				int rowCount = 1;
				int x = 0;
				int y = 0;
				foreach (var c in colors) {
					using(Brush brush = new SolidBrush(c))
					{
						g.FillRectangle(brush, x, y, frameWidth, frameHeight);
						x += frameWidth;
						
						if (rowCount > 0 && rowCount % columns == 0) {
							rowCount = 0;
							x = 0;
							y += frameHeight;
						}
						
						rowCount++;
					}
				}
			}
			pictureBox1.Image = bmp;
		}
		
		void TxtInputKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control & e.KeyCode == Keys.A)
			{
				txtInput.SelectAll();
			}
			else if (e.Control & e.KeyCode == Keys.Back)
			{
				SendKeys.SendWait("^+{LEFT}{BACKSPACE}");
			}
		}
		
		void BtnReadColorConfigListClick(object sender, EventArgs e)
		{
			string fileName = txtFilePath.Text;

			XElement colorListNode = ReadColorListFromCubaseConfig(fileName);
			if (colorListNode != null) {
				txtInput.Text = colorListNode.ToString();
				MessageBox.Show("Succesfully read the color list from the Cubase configuration file!", "Succesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
	}
}
