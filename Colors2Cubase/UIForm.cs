using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

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
			if (!fileName.Equals("")) {
				if (File.Exists(fileName)) {
					
					// build up the xml document that is required
					// <list name="Set" type="list">
					// 	<item>
					// 		<string name="Name" value="Color 1" wide="true"/>
					// 		<int name="Color" value="4284797946"/>
					// 	</item>
					// </list>
					
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
					
					// first make a backup of the config file
					MakeBackupOfFile(fileName);
					
					xmlDoc.Save("test.xml");
				}
			} else {
				MessageBox.Show("You must select the Cubase Default.xml config file first", "No file selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		private void MakeBackupOfFile(string destinationFileName) {
			if (File.Exists(destinationFileName)) {
				
				string destinationBackupFileName = destinationFileName + ".bak";
				
				if (File.Exists(destinationBackupFileName)) {
					// the backup file already exists ?!
				} else {
					File.Copy(destinationFileName, destinationBackupFileName);
				}
			}
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
	}
}
