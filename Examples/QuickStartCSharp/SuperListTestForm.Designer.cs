/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
//
// (c) 2006 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

namespace SuperListTest
{
	partial class SuperListTestForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( SuperListTestForm ) );
			BinaryComponents.SuperList.Sections.SectionFactory sectionFactory1 = new BinaryComponents.SuperList.Sections.SectionFactory();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this._fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this._fileExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.startTimedAdditionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._ensureItemVisibleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._dirtyItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._selectItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._sizeColumnsToFitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allowDragDrop = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleGroupAreaVisibilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleHeaderVisibilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleRowPaintingOverrideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.emailLikePreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.emailLikePreviewToolStripMenuItemAll = new System.Windows.Forms.ToolStripMenuItem();
			this.emailLikePreviewToolStripMenuItemSelectedOnly = new System.Windows.Forms.ToolStripMenuItem();
			this.emailLikePreviewToolStripMenuItemNone = new System.Windows.Forms.ToolStripMenuItem();
			this._imageList = new System.Windows.Forms.ImageList( this.components );
			this._superList = new BinaryComponents.SuperList.ListControl();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._fileMenuItem,
            this.actionsToolStripMenuItem,
            this.customizeToolStripMenuItem} );
			this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size( 615, 24 );
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// _fileMenuItem
			// 
			this._fileMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.loadConfigToolStripMenuItem,
            this.saveConfigToolStripMenuItem,
            this.toolStripSeparator2,
            this._fileExitMenuItem} );
			this._fileMenuItem.Name = "_fileMenuItem";
			this._fileMenuItem.Size = new System.Drawing.Size( 37, 20 );
			this._fileMenuItem.Text = "File";
			// 
			// loadConfigToolStripMenuItem
			// 
			this.loadConfigToolStripMenuItem.Name = "loadConfigToolStripMenuItem";
			this.loadConfigToolStripMenuItem.Size = new System.Drawing.Size( 148, 22 );
			this.loadConfigToolStripMenuItem.Text = "Load Config...";
			this.loadConfigToolStripMenuItem.Click += new System.EventHandler( this.loadConfigToolStripMenuItem_Click_1 );
			// 
			// saveConfigToolStripMenuItem
			// 
			this.saveConfigToolStripMenuItem.Name = "saveConfigToolStripMenuItem";
			this.saveConfigToolStripMenuItem.Size = new System.Drawing.Size( 148, 22 );
			this.saveConfigToolStripMenuItem.Text = "Save Config...";
			this.saveConfigToolStripMenuItem.Click += new System.EventHandler( this.saveConfigToolStripMenuItem_Click_1 );
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size( 145, 6 );
			// 
			// _fileExitMenuItem
			// 
			this._fileExitMenuItem.Name = "_fileExitMenuItem";
			this._fileExitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this._fileExitMenuItem.Size = new System.Drawing.Size( 148, 22 );
			this._fileExitMenuItem.Text = "E&xit";
			this._fileExitMenuItem.Click += new System.EventHandler( this._fileExitMenuItem_Click );
			// 
			// actionsToolStripMenuItem
			// 
			this.actionsToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.startTimedAdditionsToolStripMenuItem,
            this.clearListToolStripMenuItem,
            this.selectAllToolStripMenuItem,
            this.toolStripSeparator1,
            this._ensureItemVisibleToolStripMenuItem,
            this._dirtyItemsToolStripMenuItem,
            this._selectItemsToolStripMenuItem,
            this.clearSelectionToolStripMenuItem,
            this._sizeColumnsToFitToolStripMenuItem} );
			this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
			this.actionsToolStripMenuItem.Size = new System.Drawing.Size( 59, 20 );
			this.actionsToolStripMenuItem.Text = "Actions";
			// 
			// startTimedAdditionsToolStripMenuItem
			// 
			this.startTimedAdditionsToolStripMenuItem.Name = "startTimedAdditionsToolStripMenuItem";
			this.startTimedAdditionsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.startTimedAdditionsToolStripMenuItem.Size = new System.Drawing.Size( 245, 22 );
			this.startTimedAdditionsToolStripMenuItem.Text = "Toggle Timed Additions";
			this.startTimedAdditionsToolStripMenuItem.Click += new System.EventHandler( this.startTimedAdditionsToolStripMenuItem_Click );
			// 
			// clearListToolStripMenuItem
			// 
			this.clearListToolStripMenuItem.Name = "clearListToolStripMenuItem";
			this.clearListToolStripMenuItem.Size = new System.Drawing.Size( 245, 22 );
			this.clearListToolStripMenuItem.Text = "Clear List";
			this.clearListToolStripMenuItem.Click += new System.EventHandler( this.clearListToolStripMenuItem_Click );
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size( 245, 22 );
			this.selectAllToolStripMenuItem.Text = "Select All";
			this.selectAllToolStripMenuItem.Click += new System.EventHandler( this.selectAllToolStripMenuItem_Click );
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 242, 6 );
			// 
			// _ensureItemVisibleToolStripMenuItem
			// 
			this._ensureItemVisibleToolStripMenuItem.Name = "_ensureItemVisibleToolStripMenuItem";
			this._ensureItemVisibleToolStripMenuItem.Size = new System.Drawing.Size( 245, 22 );
			this._ensureItemVisibleToolStripMenuItem.Text = "Ensure Item Visible...";
			this._ensureItemVisibleToolStripMenuItem.Click += new System.EventHandler( this._ensureItemVisibleToolStripMenuItem_Click );
			// 
			// _dirtyItemsToolStripMenuItem
			// 
			this._dirtyItemsToolStripMenuItem.Name = "_dirtyItemsToolStripMenuItem";
			this._dirtyItemsToolStripMenuItem.Size = new System.Drawing.Size( 245, 22 );
			this._dirtyItemsToolStripMenuItem.Text = "Dirty Items...";
			this._dirtyItemsToolStripMenuItem.Click += new System.EventHandler( this._dirtyItemsToolStripMenuItem_Click );
			// 
			// _selectItemsToolStripMenuItem
			// 
			this._selectItemsToolStripMenuItem.Name = "_selectItemsToolStripMenuItem";
			this._selectItemsToolStripMenuItem.Size = new System.Drawing.Size( 245, 22 );
			this._selectItemsToolStripMenuItem.Text = "Select Items...";
			this._selectItemsToolStripMenuItem.Click += new System.EventHandler( this.selectItemsToolStripMenuItem_Click );
			// 
			// clearSelectionToolStripMenuItem
			// 
			this.clearSelectionToolStripMenuItem.Name = "clearSelectionToolStripMenuItem";
			this.clearSelectionToolStripMenuItem.Size = new System.Drawing.Size( 245, 22 );
			this.clearSelectionToolStripMenuItem.Text = "Clear Selection";
			this.clearSelectionToolStripMenuItem.Click += new System.EventHandler( this.clearSelectionToolStripMenuItem_Click );
			// 
			// _sizeColumnsToFitToolStripMenuItem
			// 
			this._sizeColumnsToFitToolStripMenuItem.Name = "_sizeColumnsToFitToolStripMenuItem";
			this._sizeColumnsToFitToolStripMenuItem.Size = new System.Drawing.Size( 245, 22 );
			this._sizeColumnsToFitToolStripMenuItem.Text = "Size Columns to Fit";
			this._sizeColumnsToFitToolStripMenuItem.Click += new System.EventHandler( this._sizeColumnsToFitToolStripMenuItem_Click );
			// 
			// customizeToolStripMenuItem
			// 
			this.customizeToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.allowDragDrop,
            this.toggleGroupAreaVisibilityToolStripMenuItem,
            this.toggleHeaderVisibilityToolStripMenuItem,
            this.toggleRowPaintingOverrideToolStripMenuItem,
            this.emailLikePreviewToolStripMenuItem} );
			this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
			this.customizeToolStripMenuItem.Size = new System.Drawing.Size( 75, 20 );
			this.customizeToolStripMenuItem.Text = "Customize";
			// 
			// allowDragDrop
			// 
			this.allowDragDrop.CheckOnClick = true;
			this.allowDragDrop.Name = "allowDragDrop";
			this.allowDragDrop.Size = new System.Drawing.Size( 232, 22 );
			this.allowDragDrop.Text = "AllowDragDrop";
			this.allowDragDrop.CheckStateChanged += new System.EventHandler( this.allowDragDrop_CheckStateChanged );
			// 
			// toggleGroupAreaVisibilityToolStripMenuItem
			// 
			this.toggleGroupAreaVisibilityToolStripMenuItem.Name = "toggleGroupAreaVisibilityToolStripMenuItem";
			this.toggleGroupAreaVisibilityToolStripMenuItem.Size = new System.Drawing.Size( 232, 22 );
			this.toggleGroupAreaVisibilityToolStripMenuItem.Text = "Toggle Group Area Visibility";
			this.toggleGroupAreaVisibilityToolStripMenuItem.Click += new System.EventHandler( this.toggleCustomAreaVisibilityToolStripMenuItem_Click );
			// 
			// toggleHeaderVisibilityToolStripMenuItem
			// 
			this.toggleHeaderVisibilityToolStripMenuItem.Name = "toggleHeaderVisibilityToolStripMenuItem";
			this.toggleHeaderVisibilityToolStripMenuItem.Size = new System.Drawing.Size( 232, 22 );
			this.toggleHeaderVisibilityToolStripMenuItem.Text = "Toggle Header Visibility";
			this.toggleHeaderVisibilityToolStripMenuItem.Click += new System.EventHandler( this.toggleHeaderVisibilityToolStripMenuItem_Click );
			// 
			// toggleRowPaintingOverrideToolStripMenuItem
			// 
			this.toggleRowPaintingOverrideToolStripMenuItem.Name = "toggleRowPaintingOverrideToolStripMenuItem";
			this.toggleRowPaintingOverrideToolStripMenuItem.Size = new System.Drawing.Size( 232, 22 );
			this.toggleRowPaintingOverrideToolStripMenuItem.Text = "Toggle Row Painting Override";
			this.toggleRowPaintingOverrideToolStripMenuItem.Click += new System.EventHandler( this.toggleRowPaintingOverrideToolStripMenuItem_Click );
			// 
			// emailLikePreviewToolStripMenuItem
			// 
			this.emailLikePreviewToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.emailLikePreviewToolStripMenuItemAll,
            this.emailLikePreviewToolStripMenuItemSelectedOnly,
            this.emailLikePreviewToolStripMenuItemNone} );
			this.emailLikePreviewToolStripMenuItem.Name = "emailLikePreviewToolStripMenuItem";
			this.emailLikePreviewToolStripMenuItem.Size = new System.Drawing.Size( 232, 22 );
			this.emailLikePreviewToolStripMenuItem.Text = "Email Preview Style";
			// 
			// emailLikePreviewToolStripMenuItemAll
			// 
			this.emailLikePreviewToolStripMenuItemAll.Name = "emailLikePreviewToolStripMenuItemAll";
			this.emailLikePreviewToolStripMenuItemAll.Size = new System.Drawing.Size( 146, 22 );
			this.emailLikePreviewToolStripMenuItemAll.Text = "All Rows";
			this.emailLikePreviewToolStripMenuItemAll.Click += new System.EventHandler( this.emailLikePreviewToolStripMenuItemAll_Click );
			// 
			// emailLikePreviewToolStripMenuItemSelectedOnly
			// 
			this.emailLikePreviewToolStripMenuItemSelectedOnly.Name = "emailLikePreviewToolStripMenuItemSelectedOnly";
			this.emailLikePreviewToolStripMenuItemSelectedOnly.Size = new System.Drawing.Size( 146, 22 );
			this.emailLikePreviewToolStripMenuItemSelectedOnly.Text = "Selected Only";
			this.emailLikePreviewToolStripMenuItemSelectedOnly.Click += new System.EventHandler( this.emailLikePreviewToolStripMenuItemSelectedOnly_Click );
			// 
			// emailLikePreviewToolStripMenuItemNone
			// 
			this.emailLikePreviewToolStripMenuItemNone.Name = "emailLikePreviewToolStripMenuItemNone";
			this.emailLikePreviewToolStripMenuItemNone.Size = new System.Drawing.Size( 146, 22 );
			this.emailLikePreviewToolStripMenuItemNone.Text = "None";
			this.emailLikePreviewToolStripMenuItemNone.Click += new System.EventHandler( this.emailLikePreviewToolStripMenuItemNone_Click );
			// 
			// _imageList
			// 
			this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject( "_imageList.ImageStream" )));
			this._imageList.TransparentColor = System.Drawing.Color.Transparent;
			this._imageList.Images.SetKeyName( 0, "Closed.png" );
			this._imageList.Images.SetKeyName( 1, "Open.png" );
			// 
			// _superList
			// 
			this._superList.AllowDrop = true;
			this._superList.AllowRowDragDrop = false;
			this._superList.AllowSorting = true;
			this._superList.AlternatingRowColor = System.Drawing.Color.Empty;
			this._superList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this._superList.Dock = System.Windows.Forms.DockStyle.Fill;
			this._superList.FocusedItem = null;
			this._superList.GroupSectionFont = null;
			this._superList.GroupSectionForeColor = System.Drawing.SystemColors.WindowText;
			this._superList.GroupSectionTextPlural = "Items";
			this._superList.GroupSectionTextSingular = "Item";
			this._superList.GroupSectionVerticalAlignment = BinaryComponents.WinFormsUtility.Drawing.GdiPlusEx.VAlignment.Top;
			this._superList.IndentColor = System.Drawing.Color.LightGoldenrodYellow;
			this._superList.Location = new System.Drawing.Point( 0, 24 );
			this._superList.MultiSelect = true;
			this._superList.Name = "_superList";
			this._superList.SectionFactory = sectionFactory1;
			this._superList.SeparatorColor = System.Drawing.Color.FromArgb( ((int)(((byte)(123)))), ((int)(((byte)(164)))), ((int)(((byte)(224)))) );
			this._superList.ShowCustomizeSection = true;
			this._superList.ShowHeaderSection = true;
			this._superList.Size = new System.Drawing.Size( 615, 471 );
			this._superList.TabIndex = 1;
			this._superList.KeyDown += new System.Windows.Forms.KeyEventHandler( this._superList_KeyDown );
			// 
			// SuperListTestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 615, 495 );
			this.Controls.Add( this._superList );
			this.Controls.Add( this.menuStrip1 );
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "SuperListTestForm";
			this.Text = "SuperList Test";
			this.menuStrip1.ResumeLayout( false );
			this.menuStrip1.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem _fileMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _fileExitMenuItem;
		private BinaryComponents.SuperList.ListControl _superList;
		private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem startTimedAdditionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearListToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem _ensureItemVisibleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _dirtyItemsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _selectItemsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toggleGroupAreaVisibilityToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toggleHeaderVisibilityToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearSelectionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadConfigToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveConfigToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem toggleRowPaintingOverrideToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _sizeColumnsToFitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem emailLikePreviewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem emailLikePreviewToolStripMenuItemAll;
		private System.Windows.Forms.ToolStripMenuItem emailLikePreviewToolStripMenuItemSelectedOnly;
		private System.Windows.Forms.ToolStripMenuItem emailLikePreviewToolStripMenuItemNone;
		private System.Windows.Forms.ToolStripMenuItem allowDragDrop;
		private System.Windows.Forms.ImageList _imageList;
	}
}

