using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Nerseth
{
	
	public class Delay
	{
		// ATTRIBUTES
		[XmlAttribute("Enabled")]
		public int Enabled  { get; set; }
		
		// ELEMENTS
		[XmlElement("Param")]
		public List<DelayParam> DelayParam { get; set; }
		
		// CONSTRUCTOR
		public Delay()
		{}
	}
}
