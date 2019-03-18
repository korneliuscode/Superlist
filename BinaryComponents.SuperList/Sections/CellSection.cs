/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using BinaryComponents.WinFormsUtility.Drawing;

namespace BinaryComponents.SuperList.Sections
{
	public class CellSection : Section
	{
		public CellSection( ISectionHost host, HeaderColumnSection hcs, object item )
			: base( host )
		{
			_hcs = hcs;
			_item = item;
		}

		public override void Layout( GraphicsSettings gs, Size maximumSize )
		{
			const int margin = 2;
			SizeF size;
			if( HeaderColumnSection.Column.WrapText )
			{
				size = GdiPlusEx.MeasureString( gs.Graphics, DisplayItem.ToString(), Font, maximumSize.Width - 5 );
			}
			else
			{
				size = gs.Graphics.MeasureString( DisplayItem.ToString(), Font );
			}
			if( size.Height < MinimumHeight )
			{
				size.Height = MinimumHeight;
			}
			Size = new Size( maximumSize.Width, (int)size.Height + margin * 2 );
		}

		public virtual Size GetIdealSize( GraphicsSettings gs )
		{
			const int margin = 2;
			SizeF size = gs.Graphics.MeasureString( DisplayItem.ToString(), Font );
			return new Size( (int)size.Width + 4, (int)size.Height + margin * 2 );
		}

		public override void Paint( GraphicsSettings gs, Rectangle clipRect )
		{
			Rectangle rc = DrawRectangle;

			GdiPlusEx.TextSplitting textSplitting =
					(HeaderColumnSection.Column.WrapText) ? GdiPlusEx.TextSplitting.MultiLine : GdiPlusEx.TextSplitting.SingleLineEllipsis;

			GdiPlusEx.DrawString
					( gs.Graphics,
					 DisplayItemString,
					 Font,
					 (Host.FocusedSection == ListSection && RowSection.IsSelected) ? SystemColors.HighlightText : Color,
					 rc,
					 GdiExAlignment, GdiExVerticalAlignment,
					 textSplitting, GdiPlusEx.Ampersands.Display );
		}


		public HeaderColumnSection HeaderColumnSection
		{
			get
			{
				return _hcs;
			}
			set
			{
				_hcs = value;
			}
		}

		public virtual object Item
		{
			get
			{
				return _item;
			}
			set
			{
				_item = value;
			}
		}

		protected virtual object DisplayItem
		{
			get
			{
				object o = _hcs.Column.ColumnItemAccessor( _item );
				if( o == null )
				{
					o = "(null)";
				}
				return o;
			}
		}

		protected Rectangle DrawRectangle
		{
			get
			{
				Rectangle rcScrollAdjusted = HostBasedRectangle;
				Rectangle rc = new Rectangle( rcScrollAdjusted.X + 5,
																		 rcScrollAdjusted.Y + 2,
																		 rcScrollAdjusted.Width - 5,
																		 rcScrollAdjusted.Height - 2 );
				return rc;
			}
		}

		protected Color Color
		{
			get
			{
				if( HeaderColumnSection.Column.ColumnItemFontColorAccessor == null )
				{
					return SystemColors.MenuText;
				}
				else
				{
					return HeaderColumnSection.Column.ColumnItemFontColorAccessor( Item );
				}
			}
		}

		protected string DisplayItemString
		{
			get
			{
				if( HeaderColumnSection.Column.ColumnItemFormattedAccessor == null )
				{
					return DisplayItem.ToString();
				}
				else
				{
					return HeaderColumnSection.Column.ColumnItemFormattedAccessor( Item ).ToString();
				}
			}
		}

		protected virtual Font Font
		{
			get
			{
				return Host.Font;
			}
		}

		protected GdiPlusEx.Alignment GdiExAlignment
		{
			get
			{
				switch( _hcs.Column.Alignment )
				{
					case Alignment.Left:
						return GdiPlusEx.Alignment.Left;
					case Alignment.Center:
						return GdiPlusEx.Alignment.Center;
					case Alignment.Right:
						return GdiPlusEx.Alignment.Right;
					default:
						throw new NotSupportedException();
				}
			}
		}

		protected GdiPlusEx.VAlignment GdiExVerticalAlignment
		{
			get
			{
				switch( _hcs.Column.VerticalAlignment )
				{
					case VerticalAlignment.Bottom:
						return GdiPlusEx.VAlignment.Bottom;
					case VerticalAlignment.Center:
						return GdiPlusEx.VAlignment.Center;
					case VerticalAlignment.Top:
						return GdiPlusEx.VAlignment.Top;
					default:
						throw new NotSupportedException();
				}
			}
		}

		protected internal RowSection RowSection
		{
			get
			{
				return (RowSection)Parent;
			}
		}

		protected ListSection ListSection
		{
			get
			{
				return (ListSection)Parent.Parent;
			}
		}

		private HeaderColumnSection _hcs;
		private object _item;
	}
}