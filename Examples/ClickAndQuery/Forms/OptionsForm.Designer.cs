namespace ClickAndQuery.Forms
{
	partial class OptionsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( OptionsForm ) );
			this._runQueryOnOpenCheckbox = new System.Windows.Forms.CheckBox();
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _runQueryOnOpenCheckbox
			// 
			this._runQueryOnOpenCheckbox.AutoSize = true;
			this._runQueryOnOpenCheckbox.Location = new System.Drawing.Point( 12, 16 );
			this._runQueryOnOpenCheckbox.Name = "_runQueryOnOpenCheckbox";
			this._runQueryOnOpenCheckbox.Size = new System.Drawing.Size( 143, 17 );
			this._runQueryOnOpenCheckbox.TabIndex = 0;
			this._runQueryOnOpenCheckbox.Text = "Run query when opened";
			this._runQueryOnOpenCheckbox.UseVisualStyleBackColor = true;
			// 
			// _okButton
			// 
			this._okButton.Location = new System.Drawing.Point( 202, 12 );
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size( 75, 23 );
			this._okButton.TabIndex = 1;
			this._okButton.Text = "OK";
			this._okButton.UseVisualStyleBackColor = true;
			this._okButton.Click += new System.EventHandler( this._okButton_Click );
			// 
			// _cancelButton
			// 
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point( 202, 41 );
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this._cancelButton.TabIndex = 2;
			this._cancelButton.Text = "Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			this._cancelButton.Click += new System.EventHandler( this._cancelButton_Click );
			// 
			// OptionsForm
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size( 289, 76 );
			this.Controls.Add( this._cancelButton );
			this.Controls.Add( this._okButton );
			this.Controls.Add( this._runQueryOnOpenCheckbox );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox _runQueryOnOpenCheckbox;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;
	}
}