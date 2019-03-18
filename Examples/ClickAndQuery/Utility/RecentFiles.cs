using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ClickAndQuery
{
	/// <summary>
	/// Handles the recent files feature.
	/// </summary>
	public class RecentFiles
	{
		public delegate void LoadRecentFileHandler( FileInfo fileToLoad );

		public RecentFiles( ToolStripMenuItem menuItemParent, ToolStripItem addAfter, LoadRecentFileHandler loadRecentFileHandler )
		{
			_loadRecentFileHandler = loadRecentFileHandler;
			_menuItemParent = menuItemParent;
			_addAfter = addAfter;
			_menuItemParent.Owner.HandleDestroyed += new EventHandler( menuStrip_HandleDestroyed );

			//
			// Fill list.
			for( int i = 0; i < _maxEntries; i++ )
			{
				string file = Program.FileControlPreferences.GetValue( "MRU", i.ToString() );
				if( file == null )
				{
					break;
				}
				else
				{
					Add( file );
				}
			}
		}

		void menuStrip_HandleDestroyed( object sender, EventArgs e )
		{
			//
			// Save our settings out.
			int i = 0;
			List<ToolStripMenuItem> recentFilesMenus = RecentFilesMenus;
			recentFilesMenus.Reverse();
			foreach( ToolStripMenuItem item in recentFilesMenus )
			{
				Program.FileControlPreferences.SetValue( "MRU", i++.ToString(), ((FileInfo)item.Tag).FullName );
			}
		}

		public void Add( string file )
		{
			int recentFilesCount = RecentFilesMenus.Count;
			ToolStripMenuItem itemToAdd = null;
			//
			//	Check to see if it's already found.
			foreach( ToolStripMenuItem item in RecentFilesMenus )
			{
				if( ((FileInfo)item.Tag).FullName == file )
				{
					itemToAdd = item;
					_menuItemParent.DropDownItems.Remove( itemToAdd ); // removed so we can add to the top
					break;
				}
			}
			if( itemToAdd == null )
			{
				itemToAdd = CreateMenuItem( new FileInfo( file ) );

				//
				// Remove any excess recent files
				int count = 0;
				foreach( ToolStripMenuItem menuItem in RecentFilesMenus )
				{
					if( ++count >= _maxEntries )
					{
						_menuItemParent.DropDownItems.Remove( menuItem );
					}
				}
			}
			int index = _menuItemParent.DropDownItems.IndexOf( _addAfter ) + 1;
			_menuItemParent.DropDownItems.Insert( index++, itemToAdd );

			if( recentFilesCount == 0 )
			{
				_menuItemParent.DropDownItems.Insert( index, new ToolStripSeparator() );
			}
		}

		private void FileClicked( object sender, EventArgs ea )
		{
			_loadRecentFileHandler( (FileInfo)((ToolStripMenuItem)sender).Tag );
		}

		private List<ToolStripMenuItem> RecentFilesMenus
		{
			get
			{
				List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();
				foreach( ToolStripItem item in _menuItemParent.DropDownItems )
				{
					if( IsRecentMenuItem( item ) )
					{
						items.Add( (ToolStripMenuItem)item );
					}
				}
				return items;
			}
		}

		private static bool IsRecentMenuItem( ToolStripItem item )
		{
			return item.Tag is FileInfo;
		}

		private ToolStripMenuItem CreateMenuItem( FileInfo fileInfo )
		{
			string fileName = fileInfo.Name;
			int delim = fileName.LastIndexOf( '.' );
			if( delim != -1 )
			{
				fileName = fileName.Substring( 0, delim );
			}
			ToolStripMenuItem menuItem =  new ToolStripMenuItem( fileName, null,  FileClicked );
			menuItem.Tag = fileInfo;
			return menuItem;
		}


		private ToolStripItem _addAfter;
		private const int _maxEntries = 6;
		private ToolStripMenuItem _menuItemParent;
		private LoadRecentFileHandler _loadRecentFileHandler;
	}
}
