using System;
using System.Collections.Generic;
using System.Text;

namespace ClickAndQuery.Data
{
	public enum AuthenticationStyle { Integrated, Server };
	public class Document
	{
		public string Query = string.Empty;
		public ConnectionDetails ConnectionDetails = new ConnectionDetails();
		public string SuperlistState;
	}
}
