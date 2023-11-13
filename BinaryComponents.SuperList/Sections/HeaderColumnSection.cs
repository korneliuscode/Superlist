/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using BinaryComponents.SuperList.Helper;

namespace BinaryComponents.SuperList.Sections
{
	public class HeaderColumnSection : Section
	{
		public enum DisplayMode
		{
			Header,
			Customise
		} ;

		public HeaderColumnSection( ISectionHost host, DisplayMode displayMode, Column column )
			: base( host )
		{
			Debug.Assert( column != null );
			CanDrag = true;
			_displayMode = displayMode;
			_column = column;
			column.DataChanged += column_DataChanged;
		}

		public override void Dispose()
		{
			base.Dispose();
			if( _column != null )
			{
				_column.DataChanged -= column_DataChanged;
				_column = null;
			}
		}

		public override bool CanDropInVoid
		{
			get
			{
				return Parent.AllowColumnsToBeDroppedInVoid;
			}
		}

		public Column Column
		{
			get
			{
				return _column;
			}
		}

		public new HeaderColumnSectionContainer Parent
		{
			get
			{
				return (HeaderColumnSectionContainer)base.Parent;
			}
		}


		public bool LeftMouseButtonPressed
		{
			get
			{
				return _leftMouseButtonPressed;
			}
		}

		public DisplayMode Mode
		{
			get
			{
				return _displayMode;
			}
		}

		public int ArrowWidth
		{
			get
			{
				return _arrowWidth;
			}
		}

		public int ArrowSpaceXMargin
		{
			get
			{
				return _arrowSpaceXMargin;
			}
		}

		public override void DroppedInVoid()
		{
			ListControl listControl = Host as ListControl;
			if( listControl != null && !listControl.ShowCustomizeSection )
			{
				return;
			}

			Parent.Columns.Remove( Column );
		}

		public override void Paint( GraphicsSettings gs, Rectangle clipRect )
		{
			using( SolidBrush brush = new SolidBrush( Host.TextColor ) )
			{
				Rectangle rc = Rectangle;
				int offset = 0;

				if( Parent.LayoutController != null )
				{
					offset = Parent.LayoutController.CurrentHorizontalScrollPosition;
					rc.X -= offset;
				}

				if( _displayMode == DisplayMode.Header )
				{
					if( VisualStyleRenderer.IsSupported )
					{
						VisualStyleRenderer renderer = GetRenderer();

						renderer.DrawBackground( gs.Graphics, rc );
					}
					else
					{
						gs.Graphics.FillRectangle( SystemBrushes.Control, rc );

						if( _leftMouseButtonPressed )
						{
							ControlPaint.DrawBorder3D( gs.Graphics, rc, Border3DStyle.Sunken );
						}
						else
						{
							ControlPaint.DrawBorder3D( gs.Graphics, rc, Border3DStyle.Raised );
						}
					}
				}
				else
				{
					DrawBox( gs.Graphics, rc );
				}

				DrawIcon( gs.Graphics, ref rc );

				const int textMargin = 2;

				rc.X += textMargin;
				rc.Width -= textMargin;

				if( Parent.LayoutController != null )
				{
					if( _column.ShowHeaderSortArrow )
					{
						rc.Width -= _scaledArrowWidth + _scaledArrowSpaceXMargin * 2;
					}
				}

				rc.Y -= 2;

				DrawCaption( gs, rc );

				DrawSortArrow( gs, rc );
			}
		}

		protected virtual void DrawCaption( GraphicsSettings gs, Rectangle rc )
		{
			TextRendererEx.DrawText( gs.Graphics,
															_column.Caption,
															SystemFonts.MenuFont,
															rc,
															Color.Black,
															GetTextFormatFlags() );
		}

		protected virtual void DrawBox( Graphics g, Rectangle rc )
		{
			ControlPaint.DrawBorder3D( g, rc.Left, rc.Top, rc.Width, rc.Height - 1, Border3DStyle.RaisedInner, Border3DSide.All );
		}

		protected virtual void DrawSortArrow( GraphicsSettings gs, Rectangle rc )
		{
			if( !_column.ShowHeaderSortArrow )
			{
				return;
			}

			if( rc.Width <= 0 )
			{
				return;
			}


			int offset = 0;

			if( Parent.LayoutController != null )
			{
				offset = Parent.LayoutController.CurrentHorizontalScrollPosition;
			}
			//
			// Draw sort arrows
			int halfArrowWidth = _scaledArrowWidth / 2;
			int right = Rectangle.Right - offset;
			Rectangle rcArrow = new Rectangle( right - _scaledArrowWidth - _scaledArrowSpaceXMargin * 2,
																				Rectangle.Y + (Rectangle.Height - _scaledArrowWidth) / 2,
																				_scaledArrowWidth,
																				_scaledArrowWidth
					);

			SortOrder sortOrder = Parent.GetColumnSortOrder( Column );

			switch( sortOrder )
			{
				case SortOrder.Ascending:
					{
						int xTop = rcArrow.Left + rcArrow.Width / 2 + 1;
						int yTop = rcArrow.Top + (rcArrow.Height - halfArrowWidth) / 2 - 1;
						int xLeft = xTop - halfArrowWidth;
						int yLeft = yTop + halfArrowWidth + 1;
						int xRight = xTop + halfArrowWidth + 1;
						int yRight = yTop + halfArrowWidth + 1;

						gs.Graphics.FillPolygon( SystemBrushes.ControlDark, new Point[]
                                                                               {
                                                                                   new Point(xTop, yTop),
                                                                                   new Point(xLeft, yLeft),
                                                                                   new Point(xRight, yRight)
                                                                               } );
					}
					break;
				case SortOrder.Descending:
					{
						int xBottom = rcArrow.Left + rcArrow.Width / 2 + 1;
						int xLeft = xBottom - halfArrowWidth + 1;
						int yLeft = rcArrow.Top + (rcArrow.Height - halfArrowWidth) / 2;
						int xRight = xBottom + halfArrowWidth;
						int yRight = rcArrow.Top + (rcArrow.Height - halfArrowWidth) / 2;
						int yBottom = yRight + halfArrowWidth;

						gs.Graphics.FillPolygon( SystemBrushes.ControlDark, new Point[]
                                                                               {
                                                                                   new Point(xLeft, yLeft),
                                                                                   new Point(xBottom, yBottom),
                                                                                   new Point(xRight, yRight)
                                                                               } );
					}
					break;
			}
		}

		private void DrawIcon( Graphics g, ref Rectangle rc )
		{
			if( _column.HeaderIcon == null )
			{
				return;
			}
			if( rc.Width < _column.HeaderIcon.Size.Width )
			{
				return;
			}
			if( rc.Height < _column.HeaderIcon.Size.Height )
			{
				return;
			}
			g.DrawIcon( _column.HeaderIcon, rc.Left, rc.Top );
			rc.Offset( _column.HeaderIcon.Size.Width, 0 );
			rc.Width -= _column.HeaderIcon.Size.Width;
		}

		protected virtual TextFormatFlags GetTextFormatFlags()
		{
			return TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
		}

		public override void Layout( GraphicsSettings gs, Size maximumSize )
		{
			const int widthPadding = 10;
			int headerWidth;
			int height;

			float dpiScaleFactor = gs.Graphics.DpiX / 96.0f; // Assuming 96 DPI as the standard scaling factor.
			_scaledArrowWidth = (int)(_arrowWidth * dpiScaleFactor); // Dynamically scale the arrow width based on DPI.
			_scaledArrowSpaceXMargin = (int)(_arrowSpaceXMargin * dpiScaleFactor);

			if ( VisualStyleRenderer.IsSupported )
			{
				VisualStyleRenderer renderer = GetRenderer();

				height = renderer.GetPartSize( gs.Graphics, ThemeSizeType.True ).Height;
			}
			else
			{
				height = SystemFonts.DialogFont.Height + 6;
			}
			if( height > maximumSize.Height )
			{
				height = maximumSize.Height;
			}

			switch( _displayMode )
			{
				case DisplayMode.Header:
					headerWidth = _column.Width;
					break;
				case DisplayMode.Customise:
					headerWidth = TextRenderer.MeasureText( _column.Caption, SystemFonts.MenuFont ).Width + widthPadding + _scaledArrowWidth + _scaledArrowSpaceXMargin * 2;
					break;
				default:
					throw new NotSupportedException();
			}
			if( height < MinimumHeight )
			{
				height = MinimumHeight;
			}
			Size = new Size( headerWidth, height );
		}

		public override void MouseEnter()
		{
			base.MouseEnter();
			Invalidate();
		}

		public override void MouseLeave()
		{
			Host.Cursor = Cursors.Default;
			base.MouseLeave();
			Invalidate();
		}

		public override void MouseDown( MouseEventArgs e )
		{
			base.MouseDown( e );

			if( e.Button == MouseButtons.Left )
			{
				_leftMouseButtonPressed = true;
			}
			Invalidate();
		}

		private bool PointInChangeWidthHotSpot( Point pt )
		{
			Rectangle rc = HostBasedRectangle;

			rc.X = rc.Right - _hotSpotWidth / 2;
			rc.Width = _hotSpotWidth;

			return rc.Contains( pt );
		}

		public override void KeyDown( KeyEventArgs e )
		{
			base.KeyDown( e );

			if( e.KeyCode == Keys.Escape && Host.SectionWithMouseCapture == this )
			{
				CancelMouseCapture();
			}
		}

		public override Section SectionFromPoint( Point pt )
		{
			Rectangle rc = Rectangle;

			if( _displayMode == DisplayMode.Header )
			{
				rc.Width += _hotSpotWidth / 2;
			}
			if( rc.Contains( pt ) )
			{
				return this;
			}
			return null;
		}


		public override void MouseMove( Point pt, MouseEventArgs e )
		{
			if( _displayMode == DisplayMode.Header && (Host.SectionWithMouseCapture == this || PointInChangeWidthHotSpot( pt )) )
			{
				#region Fixed Column Header Width

				if( Column.IsFixedWidth )
				{
					Host.Cursor = Cursors.Default;
					base.MouseMove( pt, e );
					return;
				}

				#endregion

				Host.Cursor = Cursors.VSplit;
				if( Host.SectionWithMouseCapture == this )
				{
					int newWidth = pt.X - HostBasedRectangle.Left - _reservedColumnSpace;

					if( newWidth < _hotSpotWidth / 2 )
					{
						newWidth = Math.Max( _hotSpotWidth / 2, _reservedColumnSpace );
					}
					Column.Width = newWidth;
				}
				else if( _leftMouseButtonPressed && e.Button == MouseButtons.Left )
				{
					_oldWidth = Column.Width;
					_reservedColumnSpace = Rectangle.Width - Column.Width;
					Host.StartMouseCapture( this );
				}
			}
			else
			{
				Host.Cursor = Cursors.Default;
				base.MouseMove( pt, e );
			}
		}

		public override bool MouseDoubleClick( Point pt )
		{
			if( _displayMode == DisplayMode.Header && PointInChangeWidthHotSpot( pt ) )
			{
				ListSection listSection = ListSection;
				if( listSection != null )
				{
					listSection.SizeColumnsToFit( Column );
				}
			}
			return true;
		}

		private ListSection ListSection
		{
			get
			{
				for( Section parent = Parent; parent != null; parent = parent.Parent )
				{
					ListSection listSection = parent as ListSection;
					if( listSection != null )
					{
						return listSection;
					}
				}
				return null;
			}
		}

		public override void MouseClick( MouseEventArgs e )
		{
			base.MouseClick( e );

			ListControl listControl = Host as ListControl;
			if( listControl != null && !listControl.AllowSorting )
			{
				return;
			}

			Point pt = new Point( e.X, e.Y );
			if( !PointInChangeWidthHotSpot( pt ) )
			{
				switch( Parent.GetColumnSortOrder( Column ) )
				{
					case SortOrder.Ascending:
						Parent.SetColumnSortOrder( Column, SortOrder.Descending );
						break;

					case SortOrder.None:
					case SortOrder.Descending:
						Parent.SetColumnSortOrder( Column, SortOrder.Ascending );
						break;
				}
			}
		}

		public override void MouseUp( MouseEventArgs e )
		{
			base.MouseUp( e );

			if( !Host.IsInDragOperation )
			{
				if( Host.SectionWithMouseCapture == this )
				{
					Host.EndMouseCapture();
				}
			}
			_leftMouseButtonPressed = false;
			Invalidate();
		}

		public override void CancelMouseCapture()
		{
			_leftMouseButtonPressed = false;
			Host.Cursor = Cursors.Default;
			base.CancelMouseCapture();
			Column.Width = _oldWidth;
			Host.EndMouseCapture();
		}

		private VisualStyleRenderer GetRenderer()
		{
			VisualStyleElement item;

			if( _leftMouseButtonPressed )
			{
				item = VisualStyleElement.Header.Item.Pressed;
			}
			else
			{
				if( Host.SectionMouseOver == this )
				{
					item = VisualStyleElement.Header.Item.Hot;
				}
				else
				{
					item = VisualStyleElement.Header.Item.Normal;
				}
			}

			VisualStyleRenderer renderer = new VisualStyleRenderer( item );

			return renderer;
		}

		private void column_DataChanged( object sender, Column.ColumnDataChangedEventArgs eventArgs )
		{
			if( eventArgs.WhatChanged == Column.WhatPropertyChanged.Width )
			{
				Host.LazyLayout( null );
			}
		}

		private int _reservedColumnSpace = 0;
		private bool _leftMouseButtonPressed = false;
		private int _oldWidth;
		private const int _arrowSpaceXMargin = 2;
		private const int _arrowWidth = 10;
		private int _scaledArrowWidth;
		private int _scaledArrowSpaceXMargin;
		private DisplayMode _displayMode;
		private Column _column;
		private const int _hotSpotWidth = 20;
	}
}