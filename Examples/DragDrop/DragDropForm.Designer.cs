namespace DragDrop
{
	partial class DragDropForm
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
			BinaryComponents.SuperList.Sections.SectionFactory sectionFactory1 = new BinaryComponents.SuperList.Sections.SectionFactory();
			BinaryComponents.SuperList.Sections.SectionFactory sectionFactory2 = new BinaryComponents.SuperList.Sections.SectionFactory();
			this._descriptionLabel = new System.Windows.Forms.Label();
			this._splitContainer = new System.Windows.Forms.SplitContainer();
			this._listControl1 = new BinaryComponents.SuperList.ListControl();
			this._listControl2 = new BinaryComponents.SuperList.ListControl();
			this._splitContainer.Panel1.SuspendLayout();
			this._splitContainer.Panel2.SuspendLayout();
			this._splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// _descriptionLabel
			// 
			this._descriptionLabel.AutoSize = true;
			this._descriptionLabel.Location = new System.Drawing.Point( 207, 9 );
			this._descriptionLabel.Name = "_descriptionLabel";
			this._descriptionLabel.Size = new System.Drawing.Size( 139, 13 );
			this._descriptionLabel.TabIndex = 1;
			this._descriptionLabel.Text = "Drag and drop between lists";
			// 
			// _splitContainer
			// 
			this._splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this._splitContainer.Location = new System.Drawing.Point( 3, 25 );
			this._splitContainer.Name = "_splitContainer";
			// 
			// _splitContainer.Panel1
			// 
			this._splitContainer.Panel1.Controls.Add( this._listControl1 );
			// 
			// _splitContainer.Panel2
			// 
			this._splitContainer.Panel2.Controls.Add( this._listControl2 );
			this._splitContainer.Size = new System.Drawing.Size( 578, 355 );
			this._splitContainer.SplitterDistance = 289;
			this._splitContainer.TabIndex = 3;
			// 
			// _listControl1
			// 
			this._listControl1.AllowDrop = true;
			this._listControl1.AllowRowDragDrop = false;
			this._listControl1.AllowSorting = true;
			this._listControl1.AlternatingRowColor = System.Drawing.Color.Empty;
			this._listControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this._listControl1.FocusedItem = null;
			this._listControl1.GroupSectionFont = null;
			this._listControl1.GroupSectionForeColor = System.Drawing.SystemColors.WindowText;
			this._listControl1.GroupSectionTextPlural = "Items";
			this._listControl1.GroupSectionTextSingular = "Item";
			this._listControl1.GroupSectionVerticalAlignment = BinaryComponents.WinFormsUtility.Drawing.GdiPlusEx.VAlignment.Top;
			this._listControl1.IndentColor = System.Drawing.Color.LightGoldenrodYellow;
			this._listControl1.Location = new System.Drawing.Point( 0, 0 );
			this._listControl1.MultiSelect = true;
			this._listControl1.Name = "_listControl1";
			this._listControl1.SectionFactory = sectionFactory1;
			this._listControl1.SeparatorColor = System.Drawing.Color.FromArgb( ((int)(((byte)(123)))), ((int)(((byte)(164)))), ((int)(((byte)(224)))) );
			this._listControl1.ShowCustomizeSection = true;
			this._listControl1.ShowHeaderSection = true;
			this._listControl1.Size = new System.Drawing.Size( 289, 355 );
			this._listControl1.TabIndex = 1;
			// 
			// _listControl2
			// 
			this._listControl2.AllowDrop = true;
			this._listControl2.AllowRowDragDrop = false;
			this._listControl2.AllowSorting = true;
			this._listControl2.AlternatingRowColor = System.Drawing.Color.Empty;
			this._listControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this._listControl2.FocusedItem = null;
			this._listControl2.GroupSectionFont = null;
			this._listControl2.GroupSectionForeColor = System.Drawing.SystemColors.WindowText;
			this._listControl2.GroupSectionTextPlural = "Items";
			this._listControl2.GroupSectionTextSingular = "Item";
			this._listControl2.GroupSectionVerticalAlignment = BinaryComponents.WinFormsUtility.Drawing.GdiPlusEx.VAlignment.Top;
			this._listControl2.IndentColor = System.Drawing.Color.LightGoldenrodYellow;
			this._listControl2.Location = new System.Drawing.Point( 0, 0 );
			this._listControl2.MultiSelect = true;
			this._listControl2.Name = "_listControl2";
			this._listControl2.SectionFactory = sectionFactory2;
			this._listControl2.SeparatorColor = System.Drawing.Color.FromArgb( ((int)(((byte)(123)))), ((int)(((byte)(164)))), ((int)(((byte)(224)))) );
			this._listControl2.ShowCustomizeSection = true;
			this._listControl2.ShowHeaderSection = true;
			this._listControl2.Size = new System.Drawing.Size( 285, 355 );
			this._listControl2.TabIndex = 3;
			// 
			// DragDropForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 579, 379 );
			this.Controls.Add( this._splitContainer );
			this.Controls.Add( this._descriptionLabel );
			this.Name = "DragDropForm";
			this.Text = "Drag & Drop Example";
			this._splitContainer.Panel1.ResumeLayout( false );
			this._splitContainer.Panel2.ResumeLayout( false );
			this._splitContainer.ResumeLayout( false );
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _descriptionLabel;
		private System.Windows.Forms.SplitContainer _splitContainer;
		private BinaryComponents.SuperList.ListControl _listControl1;
		private BinaryComponents.SuperList.ListControl _listControl2;
	}
}