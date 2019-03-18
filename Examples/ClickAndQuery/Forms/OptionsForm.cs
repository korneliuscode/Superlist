using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ClickAndQuery.Forms
{
	public partial class OptionsForm : Form
	{
		public OptionsForm()
		{
			InitializeComponent();
			_runQueryOnOpenCheckbox.Checked = Program.RunQueryOnOpen;
		}

		private void _okButton_Click( object sender, EventArgs e )
		{
			Program.RunQueryOnOpen = _runQueryOnOpenCheckbox.Checked;
			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void _cancelButton_Click( object sender, EventArgs e )
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}