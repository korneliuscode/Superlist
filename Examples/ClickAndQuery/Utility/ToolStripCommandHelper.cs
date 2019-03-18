using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace ClickAndQuery.Utility
{
	public static class ToolStripCommandHelper
	{
		public static void SetEnabledProperties( ToolStrip toolStrip )
		{
			foreach( ToolStripItem item in GetToolStripItemList( toolStrip ) )
			{
				Command cmd = item.Tag as Command;
				if( cmd != null )
				{
					item.Enabled = cmd.IsEnabled;
				}
			}
		}

		/// <summary>
		/// Given two tool ToolStrips we bind commands from one to the other based on
		/// the names of the 'fromStrip' sub-items and the name (if any) of the tag
		/// on the 'toStrip'
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void BindToolStripItems( ToolStrip fromStrip, ToolStrip toStrip )
		{
			Dictionary<string, ToolStripItem> fromItems = new Dictionary<string, ToolStripItem>();
			foreach( ToolStripItem item in GetToolStripItemList( fromStrip ) )
			{
				if( item.Tag is Command )
				{
					fromItems[item.Name] = item;
				}
			}

			foreach( ToolStripItem item in GetToolStripItemList( toStrip ) )
			{
				string tagName = item.Tag as string;
				if( tagName != null )
				{
					ToolStripItem itemFrom;
					if( fromItems.TryGetValue( tagName, out itemFrom ) )
					{
						((Command)itemFrom.Tag).Bind( item );
					}
				}
			}
		}

		private static List<ToolStripItem> GetToolStripItemList( ToolStrip toolStrip )
		{
			List<ToolStripItem> list = new List<ToolStripItem>();
			Stack<ToolStripItemCollection> itemStack = new Stack<ToolStripItemCollection>();
			itemStack.Push( toolStrip.Items );
			while( itemStack.Count > 0 )
			{
				ToolStripItemCollection items = itemStack.Pop();
				foreach( ToolStripItem item in items )
				{
					ToolStripMenuItem menuItem = item as ToolStripMenuItem;
					if( menuItem != null && menuItem.DropDownItems.Count > 0 )
					{
						itemStack.Push( menuItem.DropDownItems );
					}
					list.Add( item );
				}

			}
			return list;
		}
	}
}
