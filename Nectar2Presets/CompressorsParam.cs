using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Nerseth
{
	
	public class CompressorsParam
	{
		// ATTRIBUTES
		[XmlAttribute("ElementID")]
		public string ElementID { get; set; }
		
		[XmlAttribute("ParamID")]
		public string ParamID { get; set; }
		
		[XmlAttribute("Value")]
		public decimal Value  { get; set; }
		
		// CONSTRUCTOR
		public CompressorsParam()
		{}
	}
}
