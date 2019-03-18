namespace ClickAndQuery.Forms
{
	partial class ConnectionDetailsForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && (components != null) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ConnectionDetailsForm ) );
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._serverNameLabel = new System.Windows.Forms.Label();
			this._serverNameTextbox = new System.Windows.Forms.TextBox();
			this._authenticationLabel = new System.Windows.Forms.Label();
			this._authenticationList = new System.Windows.Forms.ComboBox();
			this._usernameTextbox = new System.Windows.Forms.TextBox();
			this._usernameLabel = new System.Windows.Forms.Label();
			this._passwordTextbox = new System.Windows.Forms.TextBox();
			this._passwordLabel = new System.Windows.Forms.Label();
			this._databaseTextbox = new System.Windows.Forms.TextBox();
			this._databaseLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Location = new System.Drawing.Point( 122, 148 );
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size( 75, 23 );
			this._okButton.TabIndex = 10;
			this._okButton.Text = "OK";
			this._okButton.UseVisualStyleBackColor = true;
			this._okButton.Click += new System.EventHandler( this._oktButton_Click );
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point( 207, 148 );
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this._cancelButton.TabIndex = 11;
			this._cancelButton.Text = "&Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			this._cancelButton.Click += new System.EventHandler( this._cancelButton_Click );
			// 
			// _serverNameLabel
			// 
			this._serverNameLabel.AutoSize = true;
			this._serverNameLabel.Location = new System.Drawing.Point( 12, 22 );
			this._serverNameLabel.Name = "_serverNameLabel";
			this._serverNameLabel.Size = new System.Drawing.Size( 70, 13 );
			this._serverNameLabel.TabIndex = 0;
			this._serverNameLabel.Text = "&Server name:";
			// 
			// _serverNameTextbox
			// 
			this._serverNameTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._serverNameTextbox.Location = new System.Drawing.Point( 125, 19 );
			this._serverNameTextbox.Name = "_serverNameTextbox";
			this._serverNameTextbox.Size = new System.Drawing.Size( 153, 20 );
			this._serverNameTextbox.TabIndex = 1;
			// 
			// _authenticationLabel
			// 
			this._authenticationLabel.AutoSize = true;
			this._authenticationLabel.Location = new System.Drawing.Point( 12, 69 );
			this._authenticationLabel.Name = "_authenticationLabel";
			this._authenticationLabel.Size = new System.Drawing.Size( 78, 13 );
			this._authenticationLabel.TabIndex = 4;
			this._authenticationLabel.Text = "&Authentication:";
			// 
			// _authenticationList
			// 
			this._authenticationList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._authenticationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._authenticationList.FormattingEnabled = true;
			this._authenticationList.Items.AddRange( new object[] {
            "Windows Authentication",
            "SQL Server Authentication"} );
			this._authenticationList.Location = new System.Drawing.Point( 125, 69 );
			this._authenticationList.Name = "_authenticationList";
			this._authenticationList.Size = new System.Drawing.Size( 153, 21 );
			this._authenticationList.TabIndex = 5;
			this._authenticationList.SelectedIndexChanged += new System.EventHandler( this._authenticationList_SelectedIndexChanged );
			// 
			// _usernameTextbox
			// 
			this._usernameTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._usernameTextbox.Location = new System.Drawing.Point( 136, 96 );
			this._usernameTextbox.Name = "_usernameTextbox";
			this._usernameTextbox.Size = new System.Drawing.Size( 142, 20 );
			this._usernameTextbox.TabIndex = 7;
			// 
			// _usernameLabel
			// 
			this._usernameLabel.AutoSize = true;
			this._usernameLabel.Location = new System.Drawing.Point( 29, 99 );
			this._usernameLabel.Name = "_usernameLabel";
			this._usernameLabel.Size = new System.Drawing.Size( 61, 13 );
			this._usernameLabel.TabIndex = 6;
			this._usernameLabel.Text = "&User name:";
			// 
			// _passwordTextbox
			// 
			this._passwordTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._passwordTextbox.Location = new System.Drawing.Point( 136, 122 );
			this._passwordTextbox.Name = "_passwordTextbox";
			this._passwordTextbox.Size = new System.Drawing.Size( 142, 20 );
			this._passwordTextbox.TabIndex = 9;
			this._passwordTextbox.UseSystemPasswordChar = true;
			// 
			// _passwordLabel
			// 
			this._passwordLabel.AutoSize = true;
			this._passwordLabel.Location = new System.Drawing.Point( 29, 125 );
			this._passwordLabel.Name = "_passwordLabel";
			this._passwordLabel.Size = new System.Drawing.Size( 56, 13 );
			this._passwordLabel.TabIndex = 8;
			this._passwordLabel.Text = "&Password:";
			// 
			// _databaseTextbox
			// 
			this._databaseTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._databaseTextbox.Location = new System.Drawing.Point( 125, 44 );
			this._databaseTextbox.Name = "_databaseTextbox";
			this._databaseTextbox.Size = new System.Drawing.Size( 153, 20 );
			this._databaseTextbox.TabIndex = 3;
			// 
			// _databaseLabel
			// 
			this._databaseLabel.AutoSize = true;
			this._databaseLabel.Location = new System.Drawing.Point( 12, 47 );
			this._databaseLabel.Name = "_databaseLabel";
			this._databaseLabel.Size = new System.Drawing.Size( 56, 13 );
			this._databaseLabel.TabIndex = 2;
			this._databaseLabel.Text = "&Database:";
			// 
			// ConnectionDetailsForm
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size( 291, 183 );
			this.Controls.Add( this._databaseTextbox );
			this.Controls.Add( this._databaseLabel );
			this.Controls.Add( this._passwordTextbox );
			this.Controls.Add( this._passwordLabel );
			this.Controls.Add( this._usernameTextbox );
			this.Controls.Add( this._usernameLabel );
			this.Controls.Add( this._authenticationList );
			this.Controls.Add( this._authenticationLabel );
			this.Controls.Add( this._serverNameTextbox );
			this.Controls.Add( this._serverNameLabel );
			this.Controls.Add( this._cancelButton );
			this.Controls.Add( this._okButton );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConnectionDetailsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Connection Details";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Label _serverNameLabel;
		private System.Windows.Forms.TextBox _serverNameTextbox;
		private System.Windows.Forms.Label _authenticationLabel;
		private System.Windows.Forms.ComboBox _authenticationList;
		private System.Windows.Forms.TextBox _usernameTextbox;
		private System.Windows.Forms.Label _usernameLabel;
		private System.Windows.Forms.TextBox _passwordTextbox;
		private System.Windows.Forms.Label _passwordLabel;
		private System.Windows.Forms.TextBox _databaseTextbox;
		private System.Windows.Forms.Label _databaseLabel;
	}
}