using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Nerseth
{
	
	public class Reverb
	{
		// ATTRIBUTES
		[XmlAttribute("Enabled")]
		public int Enabled  { get; set; }
		
		// ELEMENTS
		[XmlElement("Param")]
		public List<ReverbParam> ReverbParam { get; set; }
		
		// CONSTRUCTOR
		public Reverb()
		{}
	}
}
