using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Nerseth
{
	
	public class Compressors
	{
		// ATTRIBUTES
		[XmlAttribute("Enabled")]
		public int Enabled  { get; set; }
		
		// ELEMENTS
		[XmlElement("Param")]
		public List<CompressorsParam> CompressorsParam { get; set; }
		
		// CONSTRUCTOR
		public Compressors()
		{}
	}
}
