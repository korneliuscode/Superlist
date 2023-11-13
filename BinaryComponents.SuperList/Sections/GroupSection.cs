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
using BinaryComponents.Utility.Assemblies;
using BinaryComponents.WinFormsUtility.Drawing;

namespace BinaryComponents.SuperList.Sections
{
	public class GroupSection : RowSection
	{
		public GroupSection( ListControl listControl, RowIdentifier ri, HeaderSection headerSection, int position, int groupIndentWidth )
			: base( listControl, ri, headerSection, position )
		{
			Debug.Assert( ri is ListSection.GroupIdentifier );
			_groupIndentWidth = groupIndentWidth;
		}

		public override void Layout( GraphicsSettings gs, Size maximumSize )
		{
			SizeF size = gs.Graphics.MeasureString( Text, Font );
			if( size.Height < MinimumHeight )
			{
				size.Height = MinimumHeight;
			}
			Size = new Size( HeaderSection.Rectangle.Width, (int)size.Height + _margin + _separatorLineHeight );
		}

		public override void Paint(GraphicsSettings gs, Rectangle clipRect)
		{
			float dpiScaleX = gs.Graphics.DpiX / 96.0f; // Assuming 96 DPI as the standard scaling factor.
														// Adjust the _groupIndentWidth, _margin, and _separatorLineHeight for DPI scaling
			_scaledGroupIndentWidth = (int)(_groupIndentWidth * dpiScaleX);
			_scaledMargin = (int)(_margin * dpiScaleX);

			Rectangle rcText;
			GetDrawRectangles(out rcText, out _buttonRectangle, dpiScaleX);
			Rectangle rc = HostBasedRectangle;

			//
			// Fill indent area if any
			if (_groupIndentWidth > 0)
			{
				Rectangle rcIndent = new Rectangle(rc.X, rc.Y, _scaledGroupIndentWidth, rc.Height);

				PaintIndentArea(gs.Graphics, rcIndent);
			}

			gs.Graphics.DrawIcon(DrawIcon, _buttonRectangle);

			GdiPlusEx.DrawString
					(gs.Graphics, Text, Font, (Host.FocusedSection == ListSection && IsSelected) ? SystemColors.HighlightText : ListControl.GroupSectionForeColor, rcText
					 , GdiPlusEx.Alignment.Left, ListControl.GroupSectionVerticalAlignment, GdiPlusEx.TextSplitting.SingleLineEllipsis, GdiPlusEx.Ampersands.Display);

			Rectangle rcLine = rc;
			rcLine.X += _scaledGroupIndentWidth;
			PaintSeparatorLine(gs.Graphics, rcLine);
		}


		public void GetDrawRectangles(out Rectangle textRectangle, out Rectangle buttonRectangle, float dpiScaleX)
		{
			int spacing = (int)(4 * dpiScaleX);
			Rectangle rc = HostBasedRectangle;

			rc.X += _scaledGroupIndentWidth + spacing;
			rc.Height -= _scaledMargin + _separatorLineHeight;
			rc.Y += _scaledMargin;

			int iconWidth = (int)(DrawIcon.Width * dpiScaleX); // Scale icon width
			int iconHeight = (int)(DrawIcon.Height * dpiScaleX); // Scale icon height


			buttonRectangle = new Rectangle(rc.X, rc.Y, iconWidth, iconHeight);
			rc.X += iconWidth + spacing;
			textRectangle = new Rectangle(rc.X, rc.Y - 1, rc.Width, rc.Height);
			switch (ListControl.GroupSectionVerticalAlignment)
			{
				case GdiPlusEx.VAlignment.Bottom:
					buttonRectangle.Y = textRectangle.Bottom - iconHeight;
					break;
				case GdiPlusEx.VAlignment.Center:
					buttonRectangle.Y = (textRectangle.Bottom - textRectangle.Height / 2) - iconHeight / 2;
					break;
			}
		}

		private Icon DrawIcon
		{
			get
			{
				Icon drawIcon = null;
				switch( GroupState )
				{
					case ListSection.GroupState.Collapsed:
						drawIcon = _resources.ExpandIcon;
						break;
					case ListSection.GroupState.Expanded:
						drawIcon = _resources.CollapseIcon;
						break;
				}
				return drawIcon;
			}
		}

		public override bool MouseDoubleClick( Point pt )
		{
      if (!IsPointOverButton(pt))
      {
        GroupState = GroupState == ListSection.GroupState.Expanded ? ListSection.GroupState.Collapsed : ListSection.GroupState.Expanded;
        return true;
      }
      return false;
		}

		public override void MouseClick( MouseEventArgs e )
		{
			if (!IsPointOverButton(new Point(e.X, e.Y))) // stop selection if over + - button
			{
				base.MouseClick( e );
			}
		}

    private bool IsPointOverButton( Point pt )
    {
      return _buttonRectangle.Contains( pt );
    }

		public override void MouseDown( MouseEventArgs e )
		{
      if (IsPointOverButton(new Point(e.X, e.Y)) && e.Button == MouseButtons.Left)
			{
				GroupState = GroupState == ListSection.GroupState.Expanded ? ListSection.GroupState.Collapsed : ListSection.GroupState.Expanded;
			}
			else
			{
				base.MouseDown( e );
			}
		}


		protected override int IndentWidth
		{
			get
			{
				return _groupIndentWidth;
			}
		}

		protected virtual string Text
		{
			get
			{
				int descendentCount = RowIdentifier.Items.Length;
				return string.Format( "{0} ({1} {2})",
														 RowIdentifier.LastColumn.GroupItemAccessor( Item ),
														 descendentCount,
														 descendentCount == 1 ? ListControl.GroupSectionTextSingular : ListControl.GroupSectionTextPlural
						);
			}
		}

		protected virtual Font Font
		{
			get
			{
				return ListControl.GroupSectionFont == null ? Host.Font : ListControl.GroupSectionFont;
			}
		}

		protected override void PaintSeparatorLine( Graphics g, Rectangle rc )
		{
			using( Pen pen = new Pen( SeperatorColor, _separatorLineHeight ) )
			{
				g.DrawLine( pen, new Point( rc.Left, rc.Bottom - _separatorLineHeight + 1 ), new Point( rc.Right, rc.Bottom - _separatorLineHeight + 1 ) );
			}
		}

		protected ListSection.GroupState GroupState
		{
			get
			{
				return ListSection.GetGroupState( RowIdentifier );
			}
			set
			{
				ListSection.SetGroupState( RowIdentifier, value, true );
			}
		}

		#region Resources

		/// <summary>
		/// Class that shares out the insertion window resources
		/// </summary>
		private class Resources : IDisposable
		{
			public Resources()
			{
				_referencesCount++;
			}

			public void Dispose()
			{
				if( !_disposed )
				{
					if( --_referencesCount == 0 )
					{
						if( _collapseIcon != null )
						{
							_collapseIcon.Dispose();
							_collapseIcon = null;
						}
						if( _expandIcon != null )
						{
							_expandIcon.Dispose();
							_expandIcon = null;
						}
					}
					Debug.Assert( _referencesCount >= 0 );
					_disposed = true;
				}
			}

			public Icon CollapseIcon
			{
				get
				{
					if( _collapseIcon == null )
					{
						_collapseIcon = _resources.GetIcon( "CollapseGroup.ico" );
					}
					return _collapseIcon;
				}
			}

			public Icon ExpandIcon
			{
				get
				{
					if( _expandIcon == null )
					{
						_expandIcon = _resources.GetIcon( "ExpandGroup.ico" );
					}
					return _expandIcon;
				}
			}

			private bool _disposed = false;
			private static int _referencesCount = 0;
			private static Icon _expandIcon = null;
			private static Icon _collapseIcon = null;
			private ManifestResources _resources = new ManifestResources( "BinaryComponents.SuperList.Resources" );
		}

		#endregion

		private const int _separatorLineHeight = 2;
		private const int _margin = 7;
		private Rectangle _buttonRectangle;
		private int _groupIndentWidth;
		private int _scaledGroupIndentWidth;
		private int _scaledMargin;
		private static Resources _resources = new Resources();
	}
}