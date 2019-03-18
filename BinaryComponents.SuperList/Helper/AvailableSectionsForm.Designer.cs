/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

namespace BinaryComponents.SuperList.Helper
{
	partial class AvailableSectionsForm
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
			this._panel = new System.Windows.Forms.Panel();
			this._availableSectionsControl = new BinaryComponents.SuperList.Helper.AvailableSectionsControl();
			this._label = new System.Windows.Forms.Label();
			this._panel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _panel
			// 
			this._panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this._panel.AutoScroll = true;
			this._panel.Controls.Add( this._availableSectionsControl );
			this._panel.Location = new System.Drawing.Point( 0, 44 );
			this._panel.Name = "_panel";
			this._panel.Size = new System.Drawing.Size( 164, 114 );
			this._panel.TabIndex = 0;
			// 
			// _availableSectionsControl
			// 
			this._availableSectionsControl.AllowDrop = true;
			this._availableSectionsControl.ColumnList = null;
			this._availableSectionsControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._availableSectionsControl.Location = new System.Drawing.Point( 0, 0 );
			this._availableSectionsControl.Name = "_availableSectionsControl";
			this._availableSectionsControl.SectionFactory = sectionFactory1;
			this._availableSectionsControl.Size = new System.Drawing.Size( 164, 0 );
			this._availableSectionsControl.TabIndex = 1;
			// 
			// _label
			// 
			this._label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this._label.Location = new System.Drawing.Point( 3, 1 );
			this._label.Name = "_label";
			this._label.Size = new System.Drawing.Size( 161, 47 );
			this._label.TabIndex = 1;
			this._label.Text = "Drag and drop the columns below onto either the header or group box on the list:";
			// 
			// AvailableSectionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 163, 158 );
			this.Controls.Add( this._label );
			this.Controls.Add( this._panel );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AvailableSectionsForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Column Chooser";
			this._panel.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Panel _panel;
		private AvailableSectionsControl _availableSectionsControl;
		private System.Windows.Forms.Label _label;

	}
}