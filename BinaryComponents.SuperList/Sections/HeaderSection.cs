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
using System.Windows.Forms.VisualStyles;
using BinaryComponents.Utility.Collections;

namespace BinaryComponents.SuperList.Sections
{
    public class HeaderSection : HeaderColumnSectionContainer
    {
        public HeaderSection(ISectionHost hostControl, EventingList<Column> columns)
            : base(hostControl, columns)
        {
            Columns.DataChanged += Columns_DataChanged;
        }

        private void Columns_DataChanged(object sender, EventingList<Column>.EventInfo e)
        {
            Host.LazyLayout(Parent);
        }

        public override void SetColumnSortOrder(Column column, SortOrder sortOrder)
        {
            foreach (Column c in Columns)
            {
                if (c != column)
                {
                    c.SortOrder = SortOrder.None;
                }
            }
            column.SortOrder = sortOrder;
            Invalidate();
        }

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    Host.LazyLayout(Parent);
                }
            }
        }

        public override void Layout(GraphicsSettings settings, Size maximumSize)
        {
            int width = 0;
            int height = 0;

            SyncSectionsToColumns(HeaderColumnSection.DisplayMode.Header);

            Point pt = new Point(Location.X, Location.Y);

            int reservedSpace = LayoutController == null ? 0 : LayoutController.ReservedNearSpace;

            bool first = true;

            foreach (Section hcs in Children)
            {
                hcs.Location = pt;
                hcs.Layout(settings, new Size(maximumSize.Width - pt.X, _isVisible ? maximumSize.Height - pt.Y : 0));
                if (first)
                {
                    hcs.Size = new Size(hcs.Size.Width + reservedSpace, hcs.Size.Height);
                    first = false;
                }
                pt = new Point(hcs.Rectangle.Right, pt.Y);
                width += hcs.Rectangle.Width;
                if (hcs.Size.Height > height)
                {
                    height = hcs.Size.Height;
                }
            }
            if (height == 0 && _isVisible)
            {
                if (VisualStyleRenderer.IsSupported)
                {
                    VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.Header.Item.Normal);

                    height = renderer.GetPartSize(settings.Graphics, ThemeSizeType.True).Height;
                }
                else
                {
                    height = SystemFonts.DialogFont.Height + 6;
                }
            }
            _idealWidth = width;
            if (height < MinimumHeight)
            {
                height = MinimumHeight;
            }
            Size = new Size(Math.Max(maximumSize.Width, _idealWidth), height);
            if (LayoutController != null)
            {
                LayoutController.HeaderLayedOut();
            }
        }

        public int IdealWidth
        {
            get
            {
                return _idealWidth;
            }
        }

        public override void PaintBackground(GraphicsSettings gs, Rectangle clipRect)
        {
            Rectangle rc = Rectangle;

            rc.Width += 2; // hide end vert bars

            if (VisualStyleRenderer.IsSupported)
            {
                VisualStyleRenderer vsr = new VisualStyleRenderer(VisualStyleElement.Header.Item.Normal);

                vsr.DrawBackground(gs.Graphics, rc);
            }
            else
            {
                gs.Graphics.FillRectangle(SystemBrushes.Control, rc);
                ControlPaint.DrawBorder3D(gs.Graphics, rc, Border3DStyle.Raised);
            }

            base.Paint(gs, clipRect);
        }

        private bool _isVisible = true;
        private int _idealWidth;
    }
}