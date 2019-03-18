using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClickAndQuery.Utility;

namespace ClickAndQuery.Forms
{
	public partial class ConnectionDetailsForm : Form
	{
		public ConnectionDetailsForm( Data.ConnectionDetails connectionDetails )
		{
			InitializeComponent();

			_connectionDetails = connectionDetails;
			_serverNameTextbox.Text = connectionDetails.Server;
			_databaseTextbox.Text = connectionDetails.Database;
			switch( connectionDetails.AuthenticationStyle )
			{
				case ClickAndQuery.Data.AuthenticationStyle.Integrated:
					_authenticationList.SelectedIndex = 0;
					break;
				case ClickAndQuery.Data.AuthenticationStyle.Server:
					_authenticationList.SelectedIndex = 1;
					_usernameTextbox.Text = connectionDetails.User;
					_passwordTextbox.Text = connectionDetails.Pwd;
					break;
				default:
					throw new NotImplementedException();
			}

		}

		private void _oktButton_Click( object sender, EventArgs e )
		{
			Data.ConnectionDetails connectionDetails = new Data.ConnectionDetails();
			connectionDetails.Server = _serverNameTextbox.Text;
			connectionDetails.Database = _databaseTextbox.Text;
			if( _authenticationList.SelectedIndex == 0 )
			{
				connectionDetails.AuthenticationStyle = ClickAndQuery.Data.AuthenticationStyle.Integrated;
			}
			else
			{
				connectionDetails.AuthenticationStyle = ClickAndQuery.Data.AuthenticationStyle.Server;
				connectionDetails.User = _usernameTextbox.Text;
				connectionDetails.Pwd = _passwordTextbox.Text;
			}
			try
			{
				string [] sources = { connectionDetails.Server, connectionDetails.User, connectionDetails.Pwd };
				bool connectionHasTags = TaggedItems.Extract( TaggedItems.ExtractPolicy.CoalesceSameNameTags, sources ).Count > 0;

				if( !connectionHasTags )
				{
					connectionDetails.Connect().Dispose(); // test connection.
				}

				//
				// Transfer details over to real data source object.
				_connectionDetails.Server = connectionDetails.Server;
				_connectionDetails.Database = connectionDetails.Database;
				_connectionDetails.AuthenticationStyle = connectionDetails.AuthenticationStyle;
				_connectionDetails.User = connectionDetails.User;
				_connectionDetails.Pwd = connectionDetails.Pwd;

				DialogResult = DialogResult.OK;
				this.Close();
			}
			catch( Exception exception )
			{
				MessageBox.Show( exception.Message );
			}
		}

		private Data.ConnectionDetails _connectionDetails;

		private void _cancelButton_Click( object sender, EventArgs e )
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void _authenticationList_SelectedIndexChanged( object sender, EventArgs e )
		{
			bool allowManualCredentials = _authenticationList.SelectedIndex == 1;
			_usernameTextbox.Enabled = allowManualCredentials;
			_passwordTextbox.Enabled = allowManualCredentials;
		}
	}
}