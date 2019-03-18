/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Windows.Forms;

namespace BinaryComponents.SuperList.Sections
{
	/// <summary>
	/// Represents a visible item within the SuperList
	/// </summary>
	public abstract class Section : IDisposable
	{
		public Section( ISectionHost hostControl )
		{
			if( hostControl == null )
			{
				throw new ArgumentNullException( "hostControl" );
			}

			_host = hostControl;
		}

		public virtual void Paint( GraphicsSettings gs, Rectangle clipRect )
		{
		}

		public virtual void PaintBackground( GraphicsSettings gs, Rectangle clipRect )
		{
		}

		public ISectionHost Host
		{
			get
			{
				return _host;
			}
		}

		protected int MinimumHeight
		{
			get
			{
				return _minimumHeight;
			}
			set
			{
				_minimumHeight = value;
			}
		}

		public bool IsAncestorOf( Section sectionToTest )
		{
			for( Section s = sectionToTest.Parent; s != null; s = s.Parent )
			{
				if( s == this )
				{
					return true;
				}
			}
			return false;
		}

		public T  GetAncestor<T>( bool includeThis ) where T: Section
		{
			for( Section s = includeThis ? this : this.Parent; s != null; s = s.Parent )
			{
				if( s is T )
				{
					return (T)s;
				}
			}
			return null;
		}

		public SectionContainer Parent
		{
			get
			{
				return _parent;
			}
			internal set
			{
				_parent = value;
			}
		}

		public virtual void Dispose()
		{
			_disposed = true;
		}

		public virtual void GotFocus()
		{
			if( !_disposed )
			{
				Invalidate();
			}
		}

		public virtual void LostFocus()
		{
			if( !_disposed )
			{
				Invalidate();
			}
		}

		/// <summary>
		/// returns the actual rectangle for the section for drawing.
		/// </summary>
		/// <returns></returns>
		public Rectangle HostBasedRectangle
		{
			get
			{
				return ToHostRectangle( Rectangle );
			}
		}

		public Rectangle ToHostRectangle( Rectangle rc )
		{
			return new Rectangle( ToHostPoint( rc.Location ), rc.Size );
		}

		public Rectangle ToWorldRectangle( Rectangle rc )
		{
			return new Rectangle( ToWorldPoint( rc.Location ), rc.Size );
		}

		public Point ToWorldPoint( Point pt )
		{
			return new Point( pt.X + GetAbsoluteScrollCoordinates().X, pt.Y + GetAbsoluteScrollCoordinates().Y );
		}

		public Point ToHostPoint( Point pt )
		{
			return new Point( pt.X - GetAbsoluteScrollCoordinates().X, pt.Y - GetAbsoluteScrollCoordinates().Y );
		}

		public Point GetAbsoluteScrollCoordinates()
		{
			int x = 0;
			int y = 0;
			for( Section s = Parent; s != null; s = s.Parent )
			{
				Point ptAdjusted = s.GetScrollCoordinates();
				x += ptAdjusted.X;
				y += ptAdjusted.Y;
			}
			return new Point( x, y );
		}

		public virtual Point GetScrollCoordinates()
		{
			return Point.Empty;
		}

		#region Geometry

		public virtual Point Location
		{
			get
			{
				return _location;
			}
			set
			{
				_location = value;
			}
		}

		public Size Size
		{
			get
			{
				return _size;
			}
			set
			{
				_size = value;
			}
		}

		public virtual Section SectionFromPoint( Point pt )
		{
			if( Rectangle.Contains( pt ) )
			{
				return this;
			}
			return null;
		}

		public void Invalidate()
		{
			Host.Invalidate( this );
		}

		public Rectangle Rectangle
		{
			get
			{
				return new Rectangle( Location, Size );
			}
		}

		#region GraphicsSettings class

		public class GraphicsSettings
		{
			public GraphicsSettings( Graphics grfx )
			{
				if( grfx == null )
				{
					throw new ArgumentNullException( "grfx" );
				}

				_grfx = grfx;
			}

			public Graphics Graphics
			{
				get
				{
					return _grfx;
				}
			}

			public StringFormat DefaultStringFormat
			{
				get
				{
					if( _format == null )
					{
						_format = new StringFormat( StringFormat.GenericDefault );
						_format.Trimming = StringTrimming.EllipsisCharacter;
						_format.LineAlignment = StringAlignment.Near | StringAlignment.Center;
						_format.Alignment = StringAlignment.Near;
						_format.FormatFlags = StringFormatFlags.NoWrap;
					}
					return _format;
				}
			}

			private Graphics _grfx;
			private StringFormat _format;
		}

		#endregion

		/// <summary>
		/// lays out the visual items within this section. Note the location will
		/// already be set by the parent.
		/// </summary>
		/// <param name="gs"></param>
		/// <param name="maximumSize"></param>
		public virtual void Layout( GraphicsSettings gs, Size maximumSize )
		{
			_size = maximumSize;
		}

		#endregion

		#region Mouse Handling

		private bool _mouseDowned = false;

		public virtual void CancelMouseCapture()
		{
		}

		public virtual void MouseDown( MouseEventArgs e )
		{
			_mouseDowned = true;
			_mouseDownPosition = e.Location;
			if( Parent != null )
			{
				Parent.MouseDown( e );
			}
		}

		public virtual bool MouseDoubleClick( Point pt )
		{
			if( Parent != null )
			{
				return Parent.MouseDoubleClick( pt );
			}
			return false;
		}

		public virtual void MouseClick( MouseEventArgs e )
		{
			if( Parent != null )
			{
				Parent.MouseClick( e );
			}
		}


		public virtual void MouseUp( MouseEventArgs e )
		{
			_mouseDowned = false;
			if( Parent != null )
			{
				Parent.MouseUp( e );
			}
		}

		public virtual void MouseMove( Point pt, MouseEventArgs e )
		{
			if( e.Button == MouseButtons.Left && _mouseDowned )
			{
				Section dragObject = null;
				for( Section s = this; s != null; s = s.Parent )
				{
					if( s.CanDrag )
					{
						dragObject = s;
						break;
					}
				}
				if( dragObject != null )
				{
					Rectangle allowed = new Rectangle
							( _mouseDownPosition.X - SystemInformation.DoubleClickSize.Width / 2, _mouseDownPosition.Y - SystemInformation.DoubleClickSize.Height / 2
							 , SystemInformation.DoubleClickSize.Width, SystemInformation.DoubleClickSize.Height );

					if( !allowed.Contains( e.Location ) )
					{
						Host.DoDragDropOperation( dragObject );
					}
				}
			}
		}

		public virtual void MouseEnter()
		{
		}

		public virtual void MouseLeave()
		{
			_mouseDowned = false;
		}

		public virtual void MouseWheel( MouseEventArgs e )
		{
			if( Parent != null )
			{
				Parent.MouseWheel( e );
			}
		}

		#endregion

		#region Drag drop

		public virtual Section[] GetExpandedDragList()
		{
			return new Section[] { this };
		}

		public virtual bool CanDrag
		{
			get
			{
				return _canDrag;
			}
			set
			{
				_canDrag = value;
			}
		}

		/// <summary>
		/// Get called when a drag operation is about to be started. What's returned
		/// here gets put into the DoDragDrop methods data parameter. We override this
		/// in RowSection so that all selected items are included.
		/// </summary>
		/// <param name="sectionsToDrag"></param>
		/// <returns></returns>
		public virtual object[] GetDragObjects()
		{
			return new object[] { this };
		}

		/// <summary>
		/// This object has been dropped outside of any acceptable drop point
		/// </summary>
		public virtual void DroppedInVoid()
		{
		}

		/// <summary>
		/// Drag operation on this object has been cancelled.
		/// </summary>
		public virtual void CancelDrag()
		{
		}

		public enum CanDropQueryContext { Ancestors, Section };

		/// <summary>
		/// Can the specified object be dropped on this section.
		/// </summary>
		public virtual bool CanDrop( IDataObject objectToDrop, CanDropQueryContext context )
		{
			if( context == CanDropQueryContext.Section )
			{
				return false;
			}
			else
			{
				return Parent == null ? false : Parent.CanDrop( objectToDrop, context );
			}
		}

		/// <summary>
		/// If the drop point is not over a droppable point do we allow 
		/// the section to be dropped into the 'void'
		/// </summary>
		public virtual bool CanDropInVoid
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Called when a descendent section is dragged over. We use
		/// this in the ListSection to move the scrollbar if dragged
		/// item stays over last or first row for a set period ot time.
		/// </summary>
		/// <param name="s"></param>
		public virtual void DescendentDraggedOver( Section s )
		{
		}

		/// <summary>
		/// Called when an object that is droppable is being dragged over this section.
		/// </summary>
		/// <param name="dataObject"></param>
		public virtual void DraggingOver( Point pt, IDataObject dataObject )
		{
			if( Parent != null )
			{
				Parent.DraggingOver( pt, dataObject );
			}
		}

		/// <summary>
		/// Called when an that was droppable left this sections area.
		/// </summary>
		public virtual void DragLeave()
		{
			if( Parent != null )
			{
				Parent.DragLeave();
			}
		}

		/// <summary>
		/// Object has been dropped on us.
		/// </summary>
		public virtual void Drop( DragEventArgs eventArgs, Point dropPoint )
		{
			if( Parent != null )
			{
				Parent.Drop( eventArgs, dropPoint );
			}
		}

		#endregion

		#region Keyboard Handling

		public virtual void KeyDown( KeyEventArgs e )
		{
			if( Parent != null )
			{
				Parent.KeyDown( e );
			}
		}

		#endregion

		protected static RectangleF ToRectangleF( Rectangle rc )
		{
			return new RectangleF( rc.X, rc.Y, rc.Width, rc.Height );
		}

		private bool _disposed = false;
		private SectionContainer _parent;
		private ISectionHost _host;
		private Point _location = Point.Empty;
		private Size _size = Size.Empty;
		private Point _mouseDownPosition;
		private int _minimumHeight = 0;
		private bool _canDrag = false;
	}
}