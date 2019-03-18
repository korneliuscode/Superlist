namespace ClickAndQuery.Controls
{
	partial class ResultsControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			BinaryComponents.SuperList.Sections.SectionFactory sectionFactory1 = new BinaryComponents.SuperList.Sections.SectionFactory();
			this._listControl = new BinaryComponents.SuperList.ListControl();
			this.SuspendLayout();
			// 
			// _listControl
			// 
			this._listControl.AllowDrop = true;
			this._listControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._listControl.FocusedItem = null;
			this._listControl.Location = new System.Drawing.Point( 0, 0 );
			this._listControl.MultiSelect = true;
			this._listControl.Name = "_listControl";
			this._listControl.SectionFactory = sectionFactory1;
			this._listControl.ShowCustomizeSection = true;
			this._listControl.ShowHeaderSection = true;
			this._listControl.Size = new System.Drawing.Size( 364, 345 );
			this._listControl.TabIndex = 0;
			// 
			// ResultsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this._listControl );
			this.Name = "ResultsControl";
			this.Size = new System.Drawing.Size( 364, 345 );
			this.ResumeLayout( false );

		}

		#endregion

		private BinaryComponents.SuperList.ListControl _listControl;
	}
}
