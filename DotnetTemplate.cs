using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet_new_GUI
{
	public class DotnetTemplate
	{
		public string Template { get; set; }
		public string ShortName { get; set; }
		public string Languages { get; set; }
		public string Tags { get; set; }

		public DotnetTemplate()
		{
			Template = "Template";
			ShortName = "ShortName";
			Languages = "Languages";
			Tags = "Tags";
		}
	}
}
