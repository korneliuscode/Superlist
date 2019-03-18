using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClickAndQuery.Utility;

namespace ClickAndQuery.Forms
{
	public partial class AquireParameterValuesForm : Form
	{
		public AquireParameterValuesForm( List<TaggedItems.Tag> tags )
		{
			InitializeComponent();

			new SelfRegisteringWatch( new FileControlPreferences.FormWatch( this ) );

			foreach( TaggedItems.Tag tag in tags )
			{
				int rowIndex = _dataGrid.Rows.Add( new object[] { tag.Name, tag.Value } );
				_dataGrid.Rows[rowIndex].Tag = tag;
			}

			Application.Idle += new EventHandler( Application_Idle );
		}

		void Application_Idle( object sender, EventArgs e )
		{
			if( _dataGrid.Rows.Count > 0 )
			{
				_dataGrid.CurrentCell = _dataGrid.Rows[0].Cells[1];
				_dataGrid.BeginEdit( true );
				_dataGrid.EditingControl.Focus();
			}
			Application.Idle -= new EventHandler( Application_Idle );
		}



		private void _okButton_Click( object sender, EventArgs e )
		{
			foreach( DataGridViewRow row in _dataGrid.Rows )
			{
				string s = (string)row.Cells[1].Value;
				if( string.IsNullOrEmpty( s ) )
				{
					MessageBox.Show( string.Format( "No values have been given for tag '{0}'", row.Cells[0].Value ) );
					return; 
				}
				((TaggedItems.Tag)row.Tag).Value = s;
			}
			DialogResult = DialogResult.OK;
			this.Close();
		}


		private void _dataGrid_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Enter )
			{
				_okButton_Click( sender, e );
			}
		}
	}
}