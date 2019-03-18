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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BinaryComponents.SuperList;
using ListControl = BinaryComponents.SuperList.ListControl;

namespace SuperListTest
{
    public partial class SelectItemForm : Form
    {
        public SelectItemForm(ListControl listControl)
        {
            InitializeComponent();
            _listControl.Columns.AddRange(listControl.Columns.ToArray());
            _listControl.Items.AddRange(listControl.Items.ToArray());
        }

        public object[] SelectedItems;

        private void _selectButton_Click(object sender, EventArgs e)
        {
            if (_listControl.SelectedItems.Count > 0)
            {
                List<object> items = new List<object>();
                SelectedItems = new object[_listControl.SelectedItems.Count];
                foreach (RowIdentifier ri in _listControl.SelectedItems)
                {
                    items.AddRange(ri.Items);
                }
                SelectedItems = items.ToArray();
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
            Close();
        }
    }
}