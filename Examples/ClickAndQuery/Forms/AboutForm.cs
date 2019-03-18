using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ClickAndQuery.Forms
{
	public partial class AboutForm : Form
	{
		public AboutForm()
		{
			InitializeComponent();
		}

		private void _companySiteLinkLabel_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
		{
			System.Diagnostics.Process.Start( "http://www.BinaryComponents.com" );
		}

		private void _projectSiteLinkLabel_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
		{
			System.Diagnostics.Process.Start( "http://www.codeplex.com/Superlist" );
		}

		private void _closeButton_Click( object sender, EventArgs e )
		{
			this.Close();
		}
	}
}