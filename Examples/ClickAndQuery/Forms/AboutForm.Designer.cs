namespace ClickAndQuery.Forms
{
	partial class AboutForm
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
			this.components = new System.ComponentModel.Container();
			this._bumphLabel = new System.Windows.Forms.Label();
			this._linksBox = new System.Windows.Forms.GroupBox();
			this._projectSiteLinkLabel = new System.Windows.Forms.LinkLabel();
			this._companySiteLinkLabel = new System.Windows.Forms.LinkLabel();
			this._projectSiteLabel = new System.Windows.Forms.Label();
			this._companySiteLabel = new System.Windows.Forms.Label();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon( this.components );
			this._closeButton = new System.Windows.Forms.Button();
			this._linksBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// _bumphLabel
			// 
			this._bumphLabel.AutoSize = true;
			this._bumphLabel.Location = new System.Drawing.Point( 12, 22 );
			this._bumphLabel.Name = "_bumphLabel";
			this._bumphLabel.Size = new System.Drawing.Size( 163, 13 );
			this._bumphLabel.TabIndex = 0;
			this._bumphLabel.Text = "Lee Alexander\'s. Click and Query";
			// 
			// _linksBox
			// 
			this._linksBox.Controls.Add( this._projectSiteLinkLabel );
			this._linksBox.Controls.Add( this._companySiteLinkLabel );
			this._linksBox.Controls.Add( this._projectSiteLabel );
			this._linksBox.Controls.Add( this._companySiteLabel );
			this._linksBox.Location = new System.Drawing.Point( 15, 53 );
			this._linksBox.Name = "_linksBox";
			this._linksBox.Size = new System.Drawing.Size( 200, 77 );
			this._linksBox.TabIndex = 1;
			this._linksBox.TabStop = false;
			this._linksBox.Text = "Links";
			// 
			// _projectSiteLinkLabel
			// 
			this._projectSiteLinkLabel.AutoSize = true;
			this._projectSiteLinkLabel.Location = new System.Drawing.Point( 86, 53 );
			this._projectSiteLinkLabel.Name = "_projectSiteLinkLabel";
			this._projectSiteLinkLabel.Size = new System.Drawing.Size( 52, 13 );
			this._projectSiteLinkLabel.TabIndex = 3;
			this._projectSiteLinkLabel.TabStop = true;
			this._projectSiteLinkLabel.Text = "CodePlex";
			this._projectSiteLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this._projectSiteLinkLabel_LinkClicked );
			// 
			// _companySiteLinkLabel
			// 
			this._companySiteLinkLabel.AutoSize = true;
			this._companySiteLinkLabel.Location = new System.Drawing.Point( 86, 26 );
			this._companySiteLinkLabel.Name = "_companySiteLinkLabel";
			this._companySiteLinkLabel.Size = new System.Drawing.Size( 113, 13 );
			this._companySiteLinkLabel.TabIndex = 1;
			this._companySiteLinkLabel.TabStop = true;
			this._companySiteLinkLabel.Text = "BinaryComponents Ltd";
			this._companySiteLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this._companySiteLinkLabel_LinkClicked );
			// 
			// _projectSiteLabel
			// 
			this._projectSiteLabel.AutoSize = true;
			this._projectSiteLabel.Location = new System.Drawing.Point( 6, 53 );
			this._projectSiteLabel.Name = "_projectSiteLabel";
			this._projectSiteLabel.Size = new System.Drawing.Size( 64, 13 );
			this._projectSiteLabel.TabIndex = 2;
			this._projectSiteLabel.Text = "Project Site:";
			// 
			// _companySiteLabel
			// 
			this._companySiteLabel.AutoSize = true;
			this._companySiteLabel.Location = new System.Drawing.Point( 6, 26 );
			this._companySiteLabel.Name = "_companySiteLabel";
			this._companySiteLabel.Size = new System.Drawing.Size( 75, 13 );
			this._companySiteLabel.TabIndex = 0;
			this._companySiteLabel.Text = "Company Site:";
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Text = "notifyIcon1";
			this.notifyIcon1.Visible = true;
			// 
			// _closeButton
			// 
			this._closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._closeButton.Location = new System.Drawing.Point( 140, 136 );
			this._closeButton.Name = "_closeButton";
			this._closeButton.Size = new System.Drawing.Size( 75, 23 );
			this._closeButton.TabIndex = 2;
			this._closeButton.Text = "&Close";
			this._closeButton.UseVisualStyleBackColor = true;
			this._closeButton.Click += new System.EventHandler( this._closeButton_Click );
			// 
			// AboutForm
			// 
			this.AcceptButton = this._closeButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._closeButton;
			this.ClientSize = new System.Drawing.Size( 225, 165 );
			this.Controls.Add( this._closeButton );
			this.Controls.Add( this._linksBox );
			this.Controls.Add( this._bumphLabel );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			this._linksBox.ResumeLayout( false );
			this._linksBox.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _bumphLabel;
		private System.Windows.Forms.GroupBox _linksBox;
		private System.Windows.Forms.LinkLabel _projectSiteLinkLabel;
		private System.Windows.Forms.LinkLabel _companySiteLinkLabel;
		private System.Windows.Forms.Label _projectSiteLabel;
		private System.Windows.Forms.Label _companySiteLabel;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.Button _closeButton;
	}
}