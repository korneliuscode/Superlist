/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using BinaryComponents.SuperList.Helper;
using BinaryComponents.Utility.Assemblies;
using BinaryComponents.Utility.Collections;

namespace BinaryComponents.SuperList.Sections
{
	public class HeaderColumnSectionContainer : SectionContainer
	{
		public HeaderColumnSectionContainer( ISectionHost hostControl, EventingList<Column> columns )
			: base( hostControl )
		{
			_columns = columns;
		}


		public override void Dispose()
		{
			base.Dispose();
			if( _insertionArrows != null )
			{
				_insertionArrows.Dispose();
				_insertionArrows = null;
			}
		}

		public interface ILayoutController
		{
			int ReservedNearSpace
			{
				get;
			}

			void HeaderLayedOut();

			int CurrentHorizontalScrollPosition
			{
				get;
				set;
			}
		}

		public virtual SortOrder GetColumnSortOrder( Column column )
		{
			return column.SortOrder;
		}

		public virtual void SetColumnSortOrder( Column column, SortOrder sortOrder )
		{
			column.SortOrder = sortOrder;
		}

		public virtual bool AllowColumnsToBeDroppedInVoid
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Called by the list to reserve space for the grouping rows.
		/// </summary>
		public ILayoutController LayoutController
		{
			get
			{
				return _layoutController;
			}
			set
			{
				_layoutController = value;
			}
		}

		private static HeaderColumnSection GetHeaderColumnSectionFromDragData( IDataObject dataObject )
		{
			string[] formats = dataObject.GetFormats();

			foreach( string format in formats )
			{
				object[] items = dataObject.GetData( format ) as object[];
				if( items.Length == 1 )
				{
					HeaderColumnSection headerColumnSection = items[0] as HeaderColumnSection;

					if( headerColumnSection != null )
					{
						return headerColumnSection;
					}
				}
			}
			return null;
		}

		public override void Drop( DragEventArgs eventArgs, Point dropPoint )
		{
			HeaderColumnSection headerColumnSection = GetHeaderColumnSectionFromDragData( eventArgs.Data );

			HeaderColumnSection before;
			HeaderColumnSection after;

			GetDropBoundsSections( ToWorldPoint( dropPoint ), out before, out after );

			int insertionIndex = 0;

			if( headerColumnSection.Parent != this || headerColumnSection.Parent.Columns.IndexOf( headerColumnSection.Column ) != insertionIndex - 1 )
			{
				if( headerColumnSection.Parent.ShouldRemoveColumnOnDrop( headerColumnSection.Column ) )
				{
					headerColumnSection.Parent.Columns.Remove( headerColumnSection.Column );
				}
				if( before != null )
				{
					insertionIndex = Columns.IndexOf( before.Column ) + 1;
				}

				int columnIndexFound = Columns.IndexOf( headerColumnSection.Column );

				if( columnIndexFound != -1 )
				{
					Columns.RemoveAt( columnIndexFound );
					if( insertionIndex > columnIndexFound )
					{
						insertionIndex--;
					}
				}
				Columns.Insert( insertionIndex, headerColumnSection.Column );
			}
		}

		public virtual bool ShouldRemoveColumnOnDrop( Column column )
		{
			return column.MoveBehaviour == Column.MoveToGroupBehaviour.Move;
		}


		public override void DraggingOver( Point pt, IDataObject objectToDrop )
		{
			if( CanDrop( objectToDrop, CanDropQueryContext.Ancestors ) && GetHeaderColumnSectionFromDragData( objectToDrop ) != null )
			{
				PositionDropWindows( ToWorldPoint( pt ) );
			}
		}

		public override void DragLeave()
		{
			ShowingMouseDropPoint = false;
		}

		public override bool CanDrop( IDataObject objectToDrop, CanDropQueryContext context )
		{
			bool canDrop = GetHeaderColumnSectionFromDragData( objectToDrop ) != null;

			if( canDrop )
			{
				ShowingMouseDropPoint = true;
			}
			return canDrop;
		}

		public EventingList<Column> Columns
		{
			get
			{
				return _columns;
			}
		}

		protected virtual HeaderColumnSection CreateHeaderColumnSection( HeaderColumnSection.DisplayMode displayModeToCreate, Column column )
		{
			return Host.SectionFactory.CreateHeaderColumnSection( Host, displayModeToCreate, column );
		}

		protected void SyncSectionsToColumns( HeaderColumnSection.DisplayMode displayModeToCreate )
		{
			List<HeaderColumnSection> newHeaderList = new List<HeaderColumnSection>();
			Dictionary<Column, HeaderColumnSection> columnToHeaderColumnSection = new Dictionary<Column, HeaderColumnSection>();

			foreach( Section s in Children )
			{
				HeaderColumnSection headerColumnSection = s as HeaderColumnSection;

				if( headerColumnSection != null )
				{
					columnToHeaderColumnSection[headerColumnSection.Column] = headerColumnSection;
				}
			}

			//
			// Add  additions
			foreach( Column column in _columns )
			{
				HeaderColumnSection headerColumnSection;

				if( !columnToHeaderColumnSection.TryGetValue( column, out headerColumnSection ) )
				{
					headerColumnSection = CreateHeaderColumnSection( displayModeToCreate, column );
				}
				newHeaderList.Add( headerColumnSection );
			}

			//
			// Remove any ones that doesn't exists any more
			foreach( Section s in Children )
			{
				HeaderColumnSection hcs = s as HeaderColumnSection;

				if( hcs != null )
				{
					if( newHeaderList.IndexOf( hcs ) == -1 )
					{
						hcs.Dispose();
					}
				}
			}

			Children.Clear();
			Children.AddRange( newHeaderList.ToArray() );
		}

		public bool ShowingMouseDropPoint
		{
			get
			{
				return _showingDropPoint;
			}
			set
			{
				if( _showingDropPoint != value )
				{
					if( _showingDropPoint )
					{
						_insertionArrows.UpArrowWindow.Visible = false;
						_insertionArrows.DownArrowWindow.Visible = false;
					}
					_showingDropPoint = value;
				}
			}
		}

		protected void GetDropBoundsSections( Point pt, out HeaderColumnSection before, out HeaderColumnSection after )
		{
			before = null;
			after = null;

			foreach( HeaderColumnSection s in Children )
			{
				if( pt.X > s.Rectangle.Right && (before == null || s.Rectangle.Right > before.Rectangle.Right) )
				{
					before = s;
				}
				if( pt.X < s.Rectangle.Right && (after == null || s.Rectangle.Right < after.Rectangle.Right) )
				{
					after = s;
				}
			}
		}

		private void PositionDropWindows( Point pt )
		{
			//
			// Find the before and after sections from the point given
			HeaderColumnSection before;
			HeaderColumnSection after;

			GetDropBoundsSections( pt, out before, out after );

			//
			// Calculate the x position of the insertion arrows
			int xPos;

			if( before != null && after != null )
			{
				xPos = before.HostBasedRectangle.Right + (after.HostBasedRectangle.Left - before.HostBasedRectangle.Right) / 2;
			}
			else if( before == null )
			{
				xPos = HostBasedRectangle.Left;
				if( after != null )
				{
					xPos += (after.HostBasedRectangle.Left - HostBasedRectangle.Left) / 2;
				}
			}
			else
			{
				xPos = before.HostBasedRectangle.Right;
			}

			//
			// Calculate the y position of the insertion arrows
			int top;
			int bottom;

			if( after != null )
			{
				top = after.Rectangle.Top;
				bottom = after.Rectangle.Bottom;
			}
			else if( before != null )
			{
				top = before.Rectangle.Top;
				bottom = before.Rectangle.Bottom;
			}
			else
			{
				top = Rectangle.Top;
				bottom = Rectangle.Bottom;
			}

			//
			// Now we have all the info actually position them.
			ImageWindow downArrowWindow = _insertionArrows.DownArrowWindow;
			ImageWindow upArrowWindow = _insertionArrows.UpArrowWindow;
			Point downArrowPoint = new Point( xPos - downArrowWindow.Width / 2, top - downArrowWindow.Height );

			if( LayoutController != null )
			{
				if( xPos < 0 )
				{
					LayoutController.CurrentHorizontalScrollPosition = xPos + GetAbsoluteScrollCoordinates().X;
				}
			}

			downArrowWindow.Location = Host.PointToScreen(
					downArrowPoint
					);

			Point upArrowPoint = new Point( xPos - upArrowWindow.Width / 2, bottom );
			upArrowWindow.Location = Host.PointToScreen( upArrowPoint );

			//
			// Finally make them 
			if( !upArrowWindow.Visible )
			{
				upArrowWindow.Visible = true;
			}
			if( !downArrowWindow.Visible )
			{
				downArrowWindow.Visible = true;
			}
		}

		#region InsertArrowResources

		/// <summary>
		/// Class that shares out the insertion window resources
		/// </summary>
		private class InsertArrowResources : IDisposable
		{
			public InsertArrowResources()
			{
				_referencesCount++;
			}

			public void Dispose()
			{
				if( !_disposed )
				{
					if( --_referencesCount == 0 )
					{
						if( _upArrowWindow != null )
						{
							_upArrowWindow.Close();
							_upArrowWindow = null;
						}
						if( _downArrowWindow != null )
						{
							_downArrowWindow.Close();
							_downArrowWindow = null;
						}
					}
					Debug.Assert( _referencesCount >= 0 );
					_disposed = true;
				}
			}

			public ImageWindow UpArrowWindow
			{
				get
				{
					if( _upArrowWindow == null )
					{
						_upArrowWindow = new ImageWindow( _resources.GetIcon( "UpArrow.ico" ) );
					}
					return _upArrowWindow;
				}
			}

			public ImageWindow DownArrowWindow
			{
				get
				{
					if( _downArrowWindow == null )
					{
						_downArrowWindow = new ImageWindow( _resources.GetIcon( "DownArrow.ico" ) );
					}
					return _downArrowWindow;
				}
			}

			private bool _disposed = false;
			private static int _referencesCount = 0;
			private static ImageWindow _upArrowWindow = null;
			private static ImageWindow _downArrowWindow = null;
			private ManifestResources _resources = new ManifestResources( "BinaryComponents.SuperList.Resources" );
		}

		#endregion

		private InsertArrowResources _insertionArrows = new InsertArrowResources();
		private bool _showingDropPoint = false;
		private readonly EventingList<Column> _columns;
		private ILayoutController _layoutController = null;
	}
}