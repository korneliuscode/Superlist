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
	partial class SelectItemForm
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
			this._listControl = new BinaryComponents.SuperList.ListControl();
			this._selectButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _listControl
			// 
			this._listControl.AllowDrop = true;
			this._listControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this._listControl.Location = new System.Drawing.Point( -2, -1 );
			this._listControl.MultiSelect = true;
			this._listControl.Name = "_listControl";
			this._listControl.Size = new System.Drawing.Size( 361, 367 );
			this._listControl.TabIndex = 0;
			// 
			// _selectButton
			// 
			this._selectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._selectButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._selectButton.Location = new System.Drawing.Point( 377, 22 );
			this._selectButton.Name = "_selectButton";
			this._selectButton.Size = new System.Drawing.Size( 75, 23 );
			this._selectButton.TabIndex = 1;
			this._selectButton.Text = "Select";
			this._selectButton.UseVisualStyleBackColor = true;
			this._selectButton.Click += new System.EventHandler( this._selectButton_Click );
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point( 377, 61 );
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this._cancelButton.TabIndex = 2;
			this._cancelButton.Text = "Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			// 
			// SelectItemForm
			// 
			this.AcceptButton = this._selectButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size( 464, 368 );
			this.ControlBox = false;
			this.Controls.Add( this._cancelButton );
			this.Controls.Add( this._selectButton );
			this.Controls.Add( this._listControl );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectItemForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SelectItemForm";
			this.ResumeLayout( false );

		}

		#endregion

		private BinaryComponents.SuperList.ListControl _listControl;
		private System.Windows.Forms.Button _selectButton;
		private System.Windows.Forms.Button _cancelButton;
	}
}