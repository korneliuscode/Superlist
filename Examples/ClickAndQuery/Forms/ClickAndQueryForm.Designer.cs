namespace ClickAndQuery.Forms
{
	partial class ClickAndQueryForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ClickAndQueryForm ) );
			this._menuStrip = new System.Windows.Forms.MenuStrip();
			this._fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._recentFilesSeparator = new System.Windows.Forms.ToolStripSeparator();
			this._exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._sizeColumnsToFitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._queryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._connectionDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._splitContainer = new System.Windows.Forms.SplitContainer();
			this._queryTextbox = new System.Windows.Forms.TextBox();
			this._resultsControl = new ClickAndQuery.Controls.ResultsControl();
			this._toolbarStrip = new System.Windows.Forms.ToolStrip();
			this._newToolStripButton = new System.Windows.Forms.ToolStripButton();
			this._openToolStripButton = new System.Windows.Forms.ToolStripButton();
			this._saveToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this._copyToClipboardToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this._executeQueryToolStripButton = new System.Windows.Forms.ToolStripButton();
			this._cancelQueryToolStripButton = new System.Windows.Forms.ToolStripButton();
			this._connectionDetailsToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this._optionsToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this._aboutToolStripButton = new System.Windows.Forms.ToolStripButton();
			this._menuStrip.SuspendLayout();
			this._splitContainer.Panel1.SuspendLayout();
			this._splitContainer.Panel2.SuspendLayout();
			this._splitContainer.SuspendLayout();
			this._toolbarStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// _menuStrip
			// 
			this._menuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._fileToolStripMenuItem,
            this._editToolStripMenuItem,
            this._viewToolStripMenuItem,
            this._queryToolStripMenuItem,
            this._toolsToolStripMenuItem,
            this._helpToolStripMenuItem} );
			this._menuStrip.Location = new System.Drawing.Point( 0, 0 );
			this._menuStrip.Name = "_menuStrip";
			this._menuStrip.Size = new System.Drawing.Size( 562, 24 );
			this._menuStrip.TabIndex = 0;
			this._menuStrip.Text = "menuStrip1";
			// 
			// _fileToolStripMenuItem
			// 
			this._fileToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._newToolStripMenuItem,
            this._openToolStripMenuItem,
            this._saveToolStripMenuItem,
            this._saveAsToolStripMenuItem,
            this._recentFilesSeparator,
            this._exitToolStripMenuItem} );
			this._fileToolStripMenuItem.Name = "_fileToolStripMenuItem";
			this._fileToolStripMenuItem.Size = new System.Drawing.Size( 35, 20 );
			this._fileToolStripMenuItem.Text = "&File";
			// 
			// _newToolStripMenuItem
			// 
			this._newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject( "_newToolStripMenuItem.Image" )));
			this._newToolStripMenuItem.Name = "_newToolStripMenuItem";
			this._newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this._newToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this._newToolStripMenuItem.Text = "&New";
			// 
			// _openToolStripMenuItem
			// 
			this._openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject( "_openToolStripMenuItem.Image" )));
			this._openToolStripMenuItem.Name = "_openToolStripMenuItem";
			this._openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this._openToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this._openToolStripMenuItem.Text = "&Open";
			// 
			// _saveToolStripMenuItem
			// 
			this._saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject( "_saveToolStripMenuItem.Image" )));
			this._saveToolStripMenuItem.Name = "_saveToolStripMenuItem";
			this._saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this._saveToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this._saveToolStripMenuItem.Text = "&Save";
			// 
			// _saveAsToolStripMenuItem
			// 
			this._saveAsToolStripMenuItem.Name = "_saveAsToolStripMenuItem";
			this._saveAsToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this._saveAsToolStripMenuItem.Text = "Save &As";
			// 
			// _recentFilesSeparator
			// 
			this._recentFilesSeparator.Name = "_recentFilesSeparator";
			this._recentFilesSeparator.Size = new System.Drawing.Size( 149, 6 );
			// 
			// _exitToolStripMenuItem
			// 
			this._exitToolStripMenuItem.Name = "_exitToolStripMenuItem";
			this._exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this._exitToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this._exitToolStripMenuItem.Text = "E&xit";
			// 
			// _editToolStripMenuItem
			// 
			this._editToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._copyToolStripMenuItem} );
			this._editToolStripMenuItem.Name = "_editToolStripMenuItem";
			this._editToolStripMenuItem.Size = new System.Drawing.Size( 37, 20 );
			this._editToolStripMenuItem.Text = "&Edit";
			// 
			// _copyToolStripMenuItem
			// 
			this._copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject( "_copyToolStripMenuItem.Image" )));
			this._copyToolStripMenuItem.Name = "_copyToolStripMenuItem";
			this._copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this._copyToolStripMenuItem.Size = new System.Drawing.Size( 149, 22 );
			this._copyToolStripMenuItem.Text = "&Copy";
			// 
			// _viewToolStripMenuItem
			// 
			this._viewToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._sizeColumnsToFitToolStripMenuItem} );
			this._viewToolStripMenuItem.Name = "_viewToolStripMenuItem";
			this._viewToolStripMenuItem.Size = new System.Drawing.Size( 41, 20 );
			this._viewToolStripMenuItem.Text = "&View";
			// 
			// _sizeColumnsToFitToolStripMenuItem
			// 
			this._sizeColumnsToFitToolStripMenuItem.Name = "_sizeColumnsToFitToolStripMenuItem";
			this._sizeColumnsToFitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
			this._sizeColumnsToFitToolStripMenuItem.Size = new System.Drawing.Size( 209, 22 );
			this._sizeColumnsToFitToolStripMenuItem.Text = "Size Columns to Fit";
			// 
			// _queryToolStripMenuItem
			// 
			this._queryToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._executeToolStripMenuItem,
            this._cancelToolStripMenuItem,
            this.toolStripSeparator1,
            this._connectionDetailsToolStripMenuItem} );
			this._queryToolStripMenuItem.Name = "_queryToolStripMenuItem";
			this._queryToolStripMenuItem.Size = new System.Drawing.Size( 49, 20 );
			this._queryToolStripMenuItem.Text = "&Query";
			// 
			// _executeToolStripMenuItem
			// 
			this._executeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject( "_executeToolStripMenuItem.Image" )));
			this._executeToolStripMenuItem.Name = "_executeToolStripMenuItem";
			this._executeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this._executeToolStripMenuItem.Size = new System.Drawing.Size( 221, 22 );
			this._executeToolStripMenuItem.Text = "&Execute";
			// 
			// _cancelToolStripMenuItem
			// 
			this._cancelToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject( "_cancelToolStripMenuItem.Image" )));
			this._cancelToolStripMenuItem.Name = "_cancelToolStripMenuItem";
			this._cancelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Pause)));
			this._cancelToolStripMenuItem.Size = new System.Drawing.Size( 221, 22 );
			this._cancelToolStripMenuItem.Text = "&Cancel";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 218, 6 );
			// 
			// _connectionDetailsToolStripMenuItem
			// 
			this._connectionDetailsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject( "_connectionDetailsToolStripMenuItem.Image" )));
			this._connectionDetailsToolStripMenuItem.Name = "_connectionDetailsToolStripMenuItem";
			this._connectionDetailsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
			this._connectionDetailsToolStripMenuItem.Size = new System.Drawing.Size( 221, 22 );
			this._connectionDetailsToolStripMenuItem.Text = "Connection Details...";
			// 
			// _toolsToolStripMenuItem
			// 
			this._toolsToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._optionsToolStripMenuItem} );
			this._toolsToolStripMenuItem.Name = "_toolsToolStripMenuItem";
			this._toolsToolStripMenuItem.Size = new System.Drawing.Size( 44, 20 );
			this._toolsToolStripMenuItem.Text = "&Tools";
			// 
			// _optionsToolStripMenuItem
			// 
			this._optionsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject( "_optionsToolStripMenuItem.Image" )));
			this._optionsToolStripMenuItem.Name = "_optionsToolStripMenuItem";
			this._optionsToolStripMenuItem.Size = new System.Drawing.Size( 134, 22 );
			this._optionsToolStripMenuItem.Text = "&Options...";
			// 
			// _helpToolStripMenuItem
			// 
			this._helpToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._aboutToolStripMenuItem} );
			this._helpToolStripMenuItem.Name = "_helpToolStripMenuItem";
			this._helpToolStripMenuItem.Size = new System.Drawing.Size( 40, 20 );
			this._helpToolStripMenuItem.Text = "&Help";
			// 
			// _aboutToolStripMenuItem
			// 
			this._aboutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject( "_aboutToolStripMenuItem.Image" )));
			this._aboutToolStripMenuItem.Name = "_aboutToolStripMenuItem";
			this._aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this._aboutToolStripMenuItem.Size = new System.Drawing.Size( 145, 22 );
			this._aboutToolStripMenuItem.Text = "&About...";
			// 
			// _splitContainer
			// 
			this._splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this._splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this._splitContainer.Location = new System.Drawing.Point( 0, 52 );
			this._splitContainer.Name = "_splitContainer";
			this._splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// _splitContainer.Panel1
			// 
			this._splitContainer.Panel1.Controls.Add( this._queryTextbox );
			// 
			// _splitContainer.Panel2
			// 
			this._splitContainer.Panel2.Controls.Add( this._resultsControl );
			this._splitContainer.Size = new System.Drawing.Size( 562, 253 );
			this._splitContainer.SplitterDistance = 97;
			this._splitContainer.TabIndex = 1;
			// 
			// _queryTextbox
			// 
			this._queryTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._queryTextbox.Location = new System.Drawing.Point( 0, 0 );
			this._queryTextbox.Multiline = true;
			this._queryTextbox.Name = "_queryTextbox";
			this._queryTextbox.Size = new System.Drawing.Size( 562, 97 );
			this._queryTextbox.TabIndex = 0;
			this._queryTextbox.TextChanged += new System.EventHandler( this._queryTextbox_TextChanged );
			// 
			// _resultsControl
			// 
			this._resultsControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._resultsControl.LastQueryError = null;
			this._resultsControl.Location = new System.Drawing.Point( 0, 0 );
			this._resultsControl.Name = "_resultsControl";
			this._resultsControl.Size = new System.Drawing.Size( 562, 152 );
			this._resultsControl.TabIndex = 0;
			// 
			// _toolbarStrip
			// 
			this._toolbarStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._newToolStripButton,
            this._openToolStripButton,
            this._saveToolStripButton,
            this.toolStripSeparator2,
            this._copyToClipboardToolStripButton,
            this.toolStripSeparator3,
            this._executeQueryToolStripButton,
            this._cancelQueryToolStripButton,
            this._connectionDetailsToolStripButton,
            this.toolStripSeparator4,
            this._optionsToolStripButton,
            this.toolStripSeparator5,
            this._aboutToolStripButton} );
			this._toolbarStrip.Location = new System.Drawing.Point( 0, 24 );
			this._toolbarStrip.Name = "_toolbarStrip";
			this._toolbarStrip.Size = new System.Drawing.Size( 562, 25 );
			this._toolbarStrip.TabIndex = 2;
			// 
			// _newToolStripButton
			// 
			this._newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject( "_newToolStripButton.Image" )));
			this._newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._newToolStripButton.Name = "_newToolStripButton";
			this._newToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this._newToolStripButton.Tag = "_newToolStripMenuItem";
			this._newToolStripButton.Text = "New";
			// 
			// _openToolStripButton
			// 
			this._openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject( "_openToolStripButton.Image" )));
			this._openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._openToolStripButton.Name = "_openToolStripButton";
			this._openToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this._openToolStripButton.Tag = "_openToolStripMenuItem";
			this._openToolStripButton.Text = "Open";
			// 
			// _saveToolStripButton
			// 
			this._saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject( "_saveToolStripButton.Image" )));
			this._saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._saveToolStripButton.Name = "_saveToolStripButton";
			this._saveToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this._saveToolStripButton.Tag = "_saveToolStripMenuItem";
			this._saveToolStripButton.Text = "Save";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size( 6, 25 );
			// 
			// _copyToClipboardToolStripButton
			// 
			this._copyToClipboardToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._copyToClipboardToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject( "_copyToClipboardToolStripButton.Image" )));
			this._copyToClipboardToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._copyToClipboardToolStripButton.Name = "_copyToClipboardToolStripButton";
			this._copyToClipboardToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this._copyToClipboardToolStripButton.Tag = "_saveToolStripMenuItem";
			this._copyToClipboardToolStripButton.Text = "Copy to Clipboard";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size( 6, 25 );
			// 
			// _executeQueryToolStripButton
			// 
			this._executeQueryToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._executeQueryToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject( "_executeQueryToolStripButton.Image" )));
			this._executeQueryToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._executeQueryToolStripButton.Name = "_executeQueryToolStripButton";
			this._executeQueryToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this._executeQueryToolStripButton.Tag = "_executeToolStripMenuItem";
			this._executeQueryToolStripButton.Text = "Execute Query";
			// 
			// _cancelQueryToolStripButton
			// 
			this._cancelQueryToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._cancelQueryToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject( "_cancelQueryToolStripButton.Image" )));
			this._cancelQueryToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._cancelQueryToolStripButton.Name = "_cancelQueryToolStripButton";
			this._cancelQueryToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this._cancelQueryToolStripButton.Tag = "_cancelToolStripMenuItem";
			this._cancelQueryToolStripButton.Text = "Cancel Query";
			// 
			// _connectionDetailsToolStripButton
			// 
			this._connectionDetailsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._connectionDetailsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject( "_connectionDetailsToolStripButton.Image" )));
			this._connectionDetailsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._connectionDetailsToolStripButton.Name = "_connectionDetailsToolStripButton";
			this._connectionDetailsToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this._connectionDetailsToolStripButton.Tag = "_connectionDetailsToolStripMenuItem";
			this._connectionDetailsToolStripButton.Text = "Connection Details";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size( 6, 25 );
			// 
			// _optionsToolStripButton
			// 
			this._optionsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._optionsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject( "_optionsToolStripButton.Image" )));
			this._optionsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._optionsToolStripButton.Name = "_optionsToolStripButton";
			this._optionsToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this._optionsToolStripButton.Tag = "_optionsToolStripMenuItem";
			this._optionsToolStripButton.Text = "Options";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size( 6, 25 );
			// 
			// _aboutToolStripButton
			// 
			this._aboutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._aboutToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject( "_aboutToolStripButton.Image" )));
			this._aboutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._aboutToolStripButton.Name = "_aboutToolStripButton";
			this._aboutToolStripButton.Size = new System.Drawing.Size( 23, 22 );
			this._aboutToolStripButton.Tag = "_aboutToolStripMenuItem";
			this._aboutToolStripButton.Text = "About";
			// 
			// ClickAndQueryForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 562, 305 );
			this.Controls.Add( this._toolbarStrip );
			this.Controls.Add( this._splitContainer );
			this.Controls.Add( this._menuStrip );
			this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
			this.MainMenuStrip = this._menuStrip;
			this.Name = "ClickAndQueryForm";
			this.Text = "ClickAndQuery";
			this._menuStrip.ResumeLayout( false );
			this._menuStrip.PerformLayout();
			this._splitContainer.Panel1.ResumeLayout( false );
			this._splitContainer.Panel1.PerformLayout();
			this._splitContainer.Panel2.ResumeLayout( false );
			this._splitContainer.ResumeLayout( false );
			this._toolbarStrip.ResumeLayout( false );
			this._toolbarStrip.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip _menuStrip;
		private System.Windows.Forms.ToolStripMenuItem _fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator _recentFilesSeparator;
		private System.Windows.Forms.ToolStripMenuItem _exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _queryToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _executeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _connectionDetailsToolStripMenuItem;
		private System.Windows.Forms.SplitContainer _splitContainer;
		private System.Windows.Forms.TextBox _queryTextbox;
		private Controls.ResultsControl _resultsControl;
		private System.Windows.Forms.ToolStripMenuItem _editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _cancelToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem _toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _sizeColumnsToFitToolStripMenuItem;
		private System.Windows.Forms.ToolStrip _toolbarStrip;
		private System.Windows.Forms.ToolStripButton _newToolStripButton;
		private System.Windows.Forms.ToolStripButton _openToolStripButton;
		private System.Windows.Forms.ToolStripButton _saveToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton _copyToClipboardToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton _executeQueryToolStripButton;
		private System.Windows.Forms.ToolStripButton _cancelQueryToolStripButton;
		private System.Windows.Forms.ToolStripButton _connectionDetailsToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripButton _optionsToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripButton _aboutToolStripButton;
	}
}

