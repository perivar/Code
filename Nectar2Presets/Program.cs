using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using Nerseth;
using CommonUtils;

namespace Nectar2Preset
{
	class Program
	{
		public static void Main(string[] args)
		{
			//string filePath = @"C:\Users\perivar.nerseth\OneDrive\Audio\Presets\Izotope\Nectar 2\Presets\TEST.xml";
			string filePath = @"C:\Users\perivar.nerseth\OneDrive\Audio\Presets\Izotope\Nectar 2\Presets\Pop\Airy Lead (Harmonized).xml";
			
			XmlDocument xmldoc = new XmlDocument();
			xmldoc.Load(filePath);
			
			string presetName = StringUtils.GetStringAfterSearchWord(filePath, "Nectar 2\\Presets\\");
			presetName = presetName.Substring(0, presetName.Length - 4); // remove file suffix
			Nectar2Preset nectar2Preset = new Nectar2Preset(presetName);
			nectar2Preset.ReadPreset(xmldoc);

			string outputPresetInfo = @"..\..\PresetInfo.txt";
			TextWriter tw = new StreamWriter(outputPresetInfo);
			tw.WriteLine(nectar2Preset);
			tw.Close();

			// generate code
			//string outputfilenameMethods = @"..\..\ReadWriteMethods.txt";
			//generateCode(xmldoc, outputfilenameMethods);
		}

		private static void generateCode(XmlDocument xmldoc, string outputfilenameMethods) {
			XElement xe = xmldoc.GetXElement();

			// variables to hold the generated code
			StringBuilder defineAttributes = new StringBuilder();
			StringBuilder initAttributes = new StringBuilder();
			StringBuilder readMethod = new StringBuilder();
			StringBuilder toStringMethod = new StringBuilder();
			
			// check if we are parsing a preset file with the right version
			string presetVersion = xmldoc.SelectSingleNode("/Nectar/@PresetVer").Value;
			if (presetVersion != null && presetVersion.Equals("2")) {
				
				// parse all param nodes
				var xParams = (from a in xe.Descendants().Elements("Param")
				               select new
				               {
				               	Parent = a.Parent.Name,
				               	ElementID = a.Attribute("ElementID").Value,
				               	ParamID = a.Attribute("ParamID").Value,
				               	Value = NumberUtils.DecimalTryParseOrZero(a.Attribute("Value").Value)
				               }).ToList();

				// build generated code
				defineAttributes.AppendLine("#region Define the attributes");
				initAttributes.AppendLine("#region Initialize the attributes");
				readMethod.AppendLine("#region Read attributes from Xml");
				readMethod.AppendLine("public void ReadPreset(XmlDocument xmldoc) {");
				toStringMethod.AppendLine("#region toString Method");
				toStringMethod.AppendLine("public override string ToString() {\n\tStringBuilder sb = new StringBuilder();");
				
				bool isEnum = false;
				foreach(var xParamEntry in xParams) {
					string name = String.Format("{0} {1}", xParamEntry.ElementID, xParamEntry.ParamID);
					
					// Dynamics1Attack
					string attributeName = String.Format("{0}{1}", StringUtils.ToPascalCase(xParamEntry.ElementID), StringUtils.ToPascalCase(xParamEntry.ParamID));
					
					// dynamics1Attack
					string variableName = String.Format("{0}{1}", StringUtils.ToCamelCase(xParamEntry.ElementID), StringUtils.ToPascalCase(xParamEntry.ParamID));
					
					// code to define the attributes
					// [XmlAttribute("Dynamics1Attack")]
					// public decimal Dynamics1Attack  { get; set; }
					defineAttributes.AppendFormat("public decimal {0} {{ get; set; }}\n", attributeName);
					
					// code to initialize the attributes
					// Dynamics1Attack = 3.57761979m;
					initAttributes.AppendFormat(CultureInfo.InvariantCulture, "{0} = {1:0.############################}m;\n", attributeName, xParamEntry.Value);
					
					string nodeName = String.Format("{0}Node", variableName);
					string xPath = String.Format("/Nectar/{0}/Param[@ElementID='{1}' and @ParamID='{2}']", xParamEntry.Parent, xParamEntry.ElementID, xParamEntry.ParamID);
					
					// code to generate for finding one node using xpath and attribute values
					//XmlNode comp1AttackNode  = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 1' and @ParamID='Attack']");
					//if (comp1AttackNode != null) {
					// read the attribute value
					//	comp1AttackValue = DecimalTryParseOrZero(comp1AttackNode.SelectSingleNode("@Value").Value);
					//}
					readMethod.AppendFormat("XmlNode {0} = xmldoc.SelectSingleNode(\"{1}\");\n", nodeName, xPath);
					readMethod.AppendFormat("if ({0} != null) {{\n", nodeName);
					readMethod.AppendFormat("\t// read the {0} attribute value\n", variableName);
					readMethod.AppendFormat("\t{0} = NumberUtils.DecimalTryParseOrZero({1}.SelectSingleNode(\"@Value\").Value);\n", attributeName, nodeName);
					readMethod.AppendFormat("}}\n\n");
					
					// code to generate for outputting one variable
					//sb.Append("Dynamics 1 Attack:".PadRight(40)).AppendFormat("{0:0.############################}\n", Dynamics1Attack);
					//sb.Append("Dynamics 1 Mode:".PadRight(40)).AppendFormat("{0}\n", Dynamics1Mode);
					if (isEnum) {
						toStringMethod.AppendLine(string.Format("\tsb.Append(\"{0}:\".PadRight(40)).AppendFormat(\"{{0}}\\n\", {1});", name, attributeName));
					} else {
						toStringMethod.AppendLine(string.Format("\tsb.Append(\"{0}:\".PadRight(40)).AppendFormat(\"{{0:0.############################}}\\n\", {1});", name, attributeName));
					}
				}
				defineAttributes.AppendLine("#endregion");
				initAttributes.AppendLine("#endregion");
				readMethod.AppendLine("}");
				readMethod.AppendLine("#endregion");
				toStringMethod.AppendLine("\treturn sb.ToString();\n}");
				toStringMethod.AppendLine("#endregion");

				TextWriter tw = new StreamWriter(outputfilenameMethods);
				tw.WriteLine(defineAttributes.ToString());
				
				tw.WriteLine("// add this to the constructor");
				tw.WriteLine(initAttributes.ToString());
				tw.WriteLine();
				
				tw.WriteLine(readMethod.ToString());
				tw.WriteLine(toStringMethod.ToString());
				
				tw.Close();
			}
		}
		
		private static void ParseXml() {
			XElement xe = XElement.Load(@"C:\Users\perivar.nerseth\OneDrive\Audio\Presets\Izotope\Nectar 2\Presets\TEST.xml");
			
			// Compressors:
			// Dynamics 1 and Dynamics 2
			// Bypass
			// Attack
			// Gain
			// Mode
			// RMS Detection
			// Ratio
			// Release
			// Threshold
			Compressors comp = new Compressors();
			var xComp = (from a in xe.Element("Compressors").Elements("Param")
			             //where a.Attribute("ElementID").Value.Equals("Dynamics 1")
			             select new CompressorsParam
			             {
			             	ElementID = a.Attribute("ElementID").Value,
			             	ParamID = a.Attribute("ParamID").Value,
			             	Value = NumberUtils.DecimalTryParseOrZero(a.Attribute("Value").Value)
			             }).ToList();
			comp.CompressorsParam = xComp;
			
			// Delay
			Delay del = new Delay();
			var xDel = (from a in xe.Element("Delay").Elements("Param")
			            select new DelayParam
			            {
			            	ElementID = a.Attribute("ElementID").Value,
			            	ParamID = a.Attribute("ParamID").Value,
			            	Value = NumberUtils.DecimalTryParseOrZero(a.Attribute("Value").Value)
			            }).ToList();
			del.DelayParam = xDel;
			
			// Reverb
			Reverb rev = new Reverb();
			var xReverb = (from a in xe.Element("Reverb").Elements("Param")
			               select new ReverbParam
			               {
			               	ElementID = a.Attribute("ElementID").Value,
			               	ParamID = a.Attribute("ParamID").Value,
			               	Value = NumberUtils.DecimalTryParseOrZero(a.Attribute("Value").Value)
			               }).ToList();
			rev.ReverbParam = xReverb;
			
			// EQ
			EQ eq = new EQ();
			// Band 2 Enable
			// Band 2 Frequency
			// Band 2 Gain
			// Band 2 Shape
			// Band 0 - 7
			var xEQ = (from a in xe.Element("EQ").Elements("Param")
			           where a.Attribute("ParamID").Value.StartsWith("Band 4")
			           select new EQParam
			           {
			           	ElementID = a.Attribute("ElementID").Value,
			           	ParamID = a.Attribute("ParamID").Value,
			           	Value = NumberUtils.DecimalTryParseOrZero(a.Attribute("Value").Value)
			           }).ToList();
			eq.EQParam = xEQ;
		}
		
	}
}