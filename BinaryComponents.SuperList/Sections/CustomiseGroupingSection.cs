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
using BinaryComponents.Utility.Collections;

namespace BinaryComponents.SuperList.Sections
{
    public class CustomiseGroupingSection : HeaderColumnSectionContainer
    {
        public CustomiseGroupingSection(ISectionHost hostControl, EventingList<Column> columns)
            : base(hostControl, columns)
        {
            Columns.DataChanged += GroupedItems_DataChanged;
        }

        public override void Layout(GraphicsSettings gs, Size maximumSize)
        {
            SyncSectionsToColumns(HeaderColumnSection.DisplayMode.Customise);

            const int verticalMargin = 5;
            const int columnItemSeparation = 7;

            Point pt = new Point(Location.X + columnItemSeparation, Location.Y + verticalMargin);
            int bottom = 0;
            if (Children.Count == 0)
            {
                SizeF messageSize = gs.Graphics.MeasureString(_emptyGroupMessage, Host.Font);
                _minimumWidth = (int) messageSize.Width + _paintEmptyMessageMargin*2;
            }
            else
            {
                foreach (HeaderColumnSection hcs in Children)
                {
                    hcs.Location = pt;
                    hcs.Layout(gs, maximumSize);
                    pt.X += hcs.Size.Width + columnItemSeparation;
                    pt.Y += hcs.Size.Height/2;
                    bottom = hcs.Rectangle.Bottom;
                    _minimumWidth = hcs.Rectangle.Right - Rectangle.Left;
                }
            }
            bottom += verticalMargin;
            Size = new Size(maximumSize.Width, Math.Max(30, bottom));
        }

        public int MinimumWidth
        {
            get
            {
                return _minimumWidth;
            }
        }

        public override SortOrder GetColumnSortOrder(Column column)
        {
            return column.GroupSortOrder;
        }

        public override void SetColumnSortOrder(Column column, SortOrder sortOrder)
        {
            column.GroupSortOrder = sortOrder;
        }

        public override bool ShouldRemoveColumnOnDrop(Column column)
        {
            return true;
        }


        public override void Paint(GraphicsSettings gs, Rectangle clipRect)
        {
            base.Paint(gs, clipRect);

            if (Children.Count == 0)
            {
                SizeF messageSize = gs.Graphics.MeasureString(_emptyGroupMessage, Host.Font);
                RectangleF rcDraw = new RectangleF(0, 0, messageSize.Width + _paintEmptyMessageMargin, messageSize.Height + _paintEmptyMessageMargin);

                rcDraw.Offset(5, (Rectangle.Height - rcDraw.Height)/2);

                StringFormat sf = new StringFormat(gs.DefaultStringFormat);
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                gs.Graphics.FillRectangle(SystemBrushes.ButtonFace, rcDraw);
                gs.Graphics.DrawString(_emptyGroupMessage, Host.Font, Brushes.Gray, rcDraw, sf);
            }
            else
            {
                for (int i = 1; i < Children.Count; i++)
                {
                    Rectangle rcPrior = Children[i - 1].Rectangle;
                    Rectangle rcHcs = Children[i].Rectangle;

                    Point[] linkPoints = new Point[]
                        {
                            new Point(rcPrior.Right - 4, rcPrior.Bottom - 2),
                            new Point(rcPrior.Right - 4, rcHcs.Top + rcHcs.Height/2),
                            new Point(rcHcs.Left - 1, rcHcs.Top + rcHcs.Height/2),
                        };
                    gs.Graphics.DrawLines(Pens.Black, linkPoints);
                }
            }
        }

        private void GroupedItems_DataChanged(object sender, EventingList<Column>.EventInfo e)
        {
            Host.LazyLayout(null);
        }

        private string _emptyGroupMessage = "Drag a column header here to group by that column";
        private int _paintEmptyMessageMargin = 4;
        private int _minimumWidth;
    }
}