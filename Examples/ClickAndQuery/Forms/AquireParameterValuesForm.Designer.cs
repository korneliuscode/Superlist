namespace ClickAndQuery.Forms
{
	partial class AquireParameterValuesForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( AquireParameterValuesForm ) );
			this._dataGrid = new System.Windows.Forms.DataGridView();
			this._nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this._dataGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// _dataGrid
			// 
			this._dataGrid.AllowUserToAddRows = false;
			this._dataGrid.AllowUserToDeleteRows = false;
			this._dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this._dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._dataGrid.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this._nameColumn,
            this._valueColumn} );
			this._dataGrid.Location = new System.Drawing.Point( 0, 0 );
			this._dataGrid.MultiSelect = false;
			this._dataGrid.Name = "_dataGrid";
			this._dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this._dataGrid.Size = new System.Drawing.Size( 318, 179 );
			this._dataGrid.TabIndex = 0;
			this._dataGrid.KeyDown += new System.Windows.Forms.KeyEventHandler( this._dataGrid_KeyDown );
			// 
			// _nameColumn
			// 
			this._nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this._nameColumn.HeaderText = "Name";
			this._nameColumn.Name = "_nameColumn";
			this._nameColumn.ReadOnly = true;
			this._nameColumn.Width = 61;
			// 
			// _valueColumn
			// 
			this._valueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this._valueColumn.HeaderText = "Value";
			this._valueColumn.Name = "_valueColumn";
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.Location = new System.Drawing.Point( 150, 191 );
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size( 75, 23 );
			this._okButton.TabIndex = 1;
			this._okButton.Text = "&OK";
			this._okButton.UseVisualStyleBackColor = true;
			this._okButton.Click += new System.EventHandler( this._okButton_Click );
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point( 235, 191 );
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this._cancelButton.TabIndex = 2;
			this._cancelButton.Text = "&Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			// 
			// AquireParameterValuesForm
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size( 319, 220 );
			this.Controls.Add( this._cancelButton );
			this.Controls.Add( this._okButton );
			this.Controls.Add( this._dataGrid );
			this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AquireParameterValuesForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Enter Parameter Values for Query";
			((System.ComponentModel.ISupportInitialize)(this._dataGrid)).EndInit();
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.DataGridView _dataGrid;
		private System.Windows.Forms.DataGridViewTextBoxColumn _nameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn _valueColumn;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;
	}
}