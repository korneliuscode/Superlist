/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BinaryComponents.SuperList.Sections
{
	public class RowSection : SectionContainer
	{
		public RowSection( ListControl listControl, RowIdentifier rowIdentifier, HeaderSection headerSection, int position )
			: base( listControl )
		{
			_headerSection = headerSection;
			_rowIdentifier = rowIdentifier;
			_position = position;
			_seperatorColor = listControl.SeparatorColor;
			_indentColor = listControl.IndentColor;
		}

		public override Section[] GetExpandedDragList()
		{
			List<Section> sections = new List<Section>();
			foreach( Section s in ListSection.Children )
			{
				RowSection rowSection = s as RowSection;
				if( rowSection != null && rowSection.IsSelected )
				{
					sections.Add( rowSection );
				}
			}
			return sections.ToArray();
		}

		public override object []GetDragObjects()
		{
			return ListSection.SelectedItems.ToArray();
		}

		public override void DragLeave()
		{
			base.DragLeave();
			this.Invalidate();
		}

		public override void DraggingOver( Point pt, System.Windows.Forms.IDataObject dataObject )
		{
			this.Invalidate();
		}

		public override bool CanDrag
		{
			get
			{
				return ListSection.AllowRowDragDrop;
			}
			set
			{
				throw new NotSupportedException( "Use the ListSection.AllowRowDragDrop property instead" );
			}
		}

		//public override bool CanDrop( System.Windows.Forms.IDataObject objectToDrop, CanDropQueryContext context )
		//{
		//  if( ListSection.AllowRowDragDrop )
		//  {
		//    return true;
		//  }

		//  return base.CanDrop( objectToDrop, context );
		//}

		public Object Item
		{
			get
			{
				return _rowIdentifier.Items[0];
			}
		}

		public int Position
		{
			get
			{
				return _position;
			}
		}

		public RowIdentifier RowIdentifier
		{
			get
			{
				return _rowIdentifier;
			}
		}

		public HeaderSection HeaderSection
		{
			get
			{
				return _headerSection;
			}
		}

		public override void Layout( GraphicsSettings gs, Size maximumSize )
		{
			DeleteChildren();

			int bottom = Location.Y;

			foreach( Section s in _headerSection.Children )
			{
				HeaderColumnSection hcs = s as HeaderColumnSection;

				if( hcs != null )
				{
					CellSection cellSection = Host.SectionFactory.CreateCellSection( Host, hcs, Item );

					Children.Add( cellSection );

					//
					//	We position the cell aligned to its corresponding HeaderColumnSection. We nudge it to right here
					//	based on any difference between the column size and the headers columns actual size. The only time there
					//	will be a different currently is when we have multiple groups reserving initial space on the first column.
					cellSection.Location = new Point( hcs.Location.X + hcs.Rectangle.Width - hcs.Column.Width, Location.Y );
					cellSection.Layout( gs, new Size( hcs.Column.Width, maximumSize.Height ) );
					bottom = Math.Max( bottom, cellSection.Rectangle.Bottom );
				}
			}
			int newHeigt = MinimumHeight;
			if( bottom - Rectangle.Top > MinimumHeight )
			{
				newHeigt = bottom - Rectangle.Top;
			}
			Size = new Size( HeaderSection.Rectangle.Width, newHeigt );
		}

		public void PaintSelection( GraphicsSettings gs )
		{
			System.Diagnostics.Debug.WriteLine( this.Host.CurrentSectionDraggedOver );
			if( IsSelected )
			{
				gs.Graphics.FillRectangle( Host.FocusedSection == ListSection ? SystemBrushes.Highlight : SystemBrushes.ButtonFace, Rectangle );
			}
			else if( this.Host.CurrentSectionDraggedOver == this )
			{
				Color h = SystemColors.Highlight;
				Color c = Color.FromArgb( h.A / 3, h.R, h.G, h.B );
				using( SolidBrush brush = new SolidBrush( c ) )
				{
					gs.Graphics.FillRectangle( brush, Rectangle );
				}
			}
			else if( ListControl.AlternatingRowColor != Color.Empty && Position % 2 == 0 && RowIdentifier.GroupColumns.Length == 0 )
			{
				gs.Graphics.FillRectangle( new SolidBrush( ListControl.AlternatingRowColor ), Rectangle );
			}
		}

		public override void PaintBackground( GraphicsSettings gs, Rectangle clipRect )
		{
			PaintSelection( gs );
			_drawnSelected = IsSelected;

			if( IsFocused )
			{
				Rectangle rc = HostBasedRectangle;
				int indent = IndentWidth;
				Rectangle focusRect = new Rectangle( rc.X + indent, rc.Y, rc.Width - indent, Rectangle.Height );

				focusRect.Width -= 1;
				if( this is GroupSection )
				{
					focusRect.Height -= _separatorLineHeight;
				}
				focusRect.Height -= 2;

				using( Pen pen = new Pen( SystemColors.ControlDark ) )
				{
					pen.DashStyle = DashStyle.Dot;
					gs.Graphics.DrawRectangle( pen, focusRect );
				}
			}
		}

		public bool DrawnSelected
		{
			get
			{
				return _drawnSelected;
			}
		}

		public virtual bool NeedsLayoutOnSelection
		{
			get
			{
				return false;
			}
		}

		public bool IsSelected
		{
			get
			{
				return ListSection.SelectedItems.IsSelected( RowIdentifier );
			}
		}

		public bool IsFocused
		{
			get
			{
				return ListSection.HasFocus( RowIdentifier );
			}
		}


		protected virtual void PaintSeparatorLine( Graphics g, Rectangle rc )
		{
			using( Pen pen = new Pen( Color.FromArgb( 70, _seperatorColor.R, _seperatorColor.G, _seperatorColor.B ), _separatorLineHeight ) )
			{
				g.DrawLine( pen, new Point( rc.Left, rc.Bottom - _separatorLineHeight ), new Point( rc.Right, rc.Bottom - _separatorLineHeight ) );
			}
		}

		public override void Paint( GraphicsSettings gs, Rectangle clipRect )
		{
			//
			// Fill indent area
			if( Children.Count > 0 )
			{
				Rectangle rc = HostBasedRectangle;
				Rectangle rcIndent = new Rectangle( rc.X, Rectangle.Y, Children[0].HostBasedRectangle.X - rc.X, rc.Height );

				PaintIndentArea( gs.Graphics, rcIndent );
			}

			base.Paint( gs, clipRect );

			Rectangle rcLine = Rectangle;
			rcLine.X += IndentWidth;
			PaintSeparatorLine( gs.Graphics, rcLine );
		}

		protected virtual int IndentWidth
		{
			get
			{
				return Children.Count > 0 ? Children[0].Rectangle.X - Rectangle.X : 0;
			}
		}

		protected ListSection ListSection
		{
			get
			{
				return (ListSection)Parent;
			}
		}

		protected ListControl ListControl
		{
			get
			{
				return (ListControl)Host;
			}
		}

		protected virtual void PaintIndentArea( Graphics g, Rectangle rcIndent )
		{
			g.FillRectangle( new SolidBrush( _indentColor ), rcIndent );
		}

		public CellSection CellSectionFromPoint( Point pt )
		{
			pt.X += GetAbsoluteScrollCoordinates().X;
			pt.Y += GetAbsoluteScrollCoordinates().Y;
			foreach( Section s in Children )
			{
				CellSection cs = s as CellSection;
				if( cs != null )
				{
					Section sectionFromPoint = cs.SectionFromPoint( pt );
					if( sectionFromPoint != null )
					{
						return cs;
					}
				}
			}
			return null;
		}

		public Color SeperatorColor
		{
			get
			{
				return _seperatorColor;
			}
			set
			{
				_seperatorColor = value;
			}
		}

		public Color IndentColor
		{
			get
			{
				return _indentColor;
			}
			set
			{
				_indentColor = value;
			}
		}

		private const int _separatorLineHeight = 1;
		private readonly int _position;
		private readonly RowIdentifier _rowIdentifier;
		private readonly HeaderSection _headerSection;
		private bool _drawnSelected = false;
		private Color _seperatorColor;
		private Color _indentColor;
	}
}