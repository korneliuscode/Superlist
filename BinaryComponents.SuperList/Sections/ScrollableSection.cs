/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BinaryComponents.SuperList.Sections
{
    public class ScrollableSection : SectionContainer
    {
        public ScrollableSection(ISectionHost host)
            : base(host)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_vScrollbar != null)
            {
                Host.ControlCollection.Remove(_vScrollbar);
                _vScrollbar.Dispose();
                _vScrollbar = null;
            }
            if (_hScrollbar != null)
            {
                Host.ControlCollection.Remove(_hScrollbar);
                _hScrollbar.Dispose();
                _hScrollbar = null;
            }
        }

        public override Point GetScrollCoordinates()
        {
            Point pt = Point.Empty;
            if (HorizontalScrollbarVisible)
            {
                pt.X += HScrollbar.Value;
            }
            if (VerticalScrollbarVisible)
            {
                pt.Y += VScrollbar.Value;
            }
            return pt;
        }

        public void UpdateScrollInfo()
        {
            if (HorizontalScrollbarVisible)
            {
                SetHScrollInfo();
                PositionHorizontalScrollbar();
            }
            if (VerticalScrollbarVisible)
            {
                SetVScrollInfo();
                PositionVerticalScrollbar();
            }
        }

        protected bool HorizontalScrollbarVisible
        {
            get
            {
                return HScrollbar != null && HScrollbar.Visible;
            }
            set
            {
                if (value != HorizontalScrollbarVisible)
                {
                    if (HScrollbar == null)
                    {
                        _hScrollbar = new NonSelectableHScrollBar();

                        _hScrollbar.Visible = false;
                        _hScrollbar.ValueChanged += HScrollbar_ValueChanged;
                        Host.ControlCollection.Add(HScrollbar);
                    }
                    if (value)
                    {
                        SetHScrollInfo();
                        PositionHorizontalScrollbar();
                    }
                    HScrollbar.Visible = value;
                }
            }
        }

        protected bool VerticalScrollbarVisible
        {
            get
            {
                return VScrollbar != null && VScrollbar.Visible;
            }
            set
            {
                if (value != VerticalScrollbarVisible)
                {
                    if (VScrollbar == null)
                    {
                        _vScrollbar = new NonSelectableVScrollBar();

                        _vScrollbar.Visible = false;
                        _vScrollbar.ValueChanged += VScrollbar_ValueChanged;
                        Host.ControlCollection.Add(VScrollbar);
                    }
                    if (value)
                    {
                        SetVScrollInfo();
                        PositionVerticalScrollbar();
                    }
                    VScrollbar.Visible = value;
                }
            }
        }

        protected virtual void OnVScrollValueChanged(int value)
        {
            Invalidate();
        }

        protected virtual void SetVScrollInfo()
        {
        }

        protected virtual void PositionVerticalScrollbar()
        {
            Rectangle workingRectangle = WorkingRectangle;
            int hScrollSpacing = HorizontalScrollbarVisible ? HScrollbar.Bounds.Height : 0;
            VScrollbar.Bounds = new Rectangle(
                workingRectangle.Right,
                workingRectangle.Top,
                VScrollbar.Bounds.Width,
                workingRectangle.Height);
        }

        protected virtual void OnHScrollValueChanged(int value)
        {
            Invalidate();
        }

        protected virtual void SetHScrollInfo()
        {
        }

        protected virtual void PositionHorizontalScrollbar()
        {
            int vScrollSpacing = VerticalScrollbarVisible ? VScrollbar.Bounds.Width : 0;
            HScrollbar.Bounds = new Rectangle(
                Rectangle.X,
                Rectangle.Bottom - HScrollbar.Bounds.Height,
                Rectangle.Width - vScrollSpacing,
                HScrollbar.Bounds.Height
                );
        }

        protected Rectangle WorkingRectangle
        {
            get
            {
                Rectangle rc = Rectangle;
                if (VerticalScrollbarVisible)
                {
                    rc.Width -= VScrollbar.Width;
                }
                if (HorizontalScrollbarVisible)
                {
                    rc.Height -= HScrollbar.Height;
                }
                if (ExcludeFirstChildrenFromVScroll > 0)
                {
                    int bottom = 0;
                    int count = ExcludeFirstChildrenFromVScroll;
                    foreach (Section s in Children)
                    {
                        if (count-- == 0)
                        {
                            break;
                        }
                        if (s.Rectangle.Bottom > bottom)
                        {
                            bottom = s.Rectangle.Bottom;
                        }
                    }
                    int height = bottom - Rectangle.Top;
                    rc.Y += height;
                    rc.Height -= height;
                }
                return rc;
            }
        }

        public override void Paint(GraphicsSettings gs, Rectangle clipRect)
        {
            for (int i = 0; i < ExcludeFirstChildrenFromVScroll; i++)
            {
                Children[i].Paint(gs, clipRect);
            }

            GraphicsContainer container = gs.Graphics.BeginContainer();
            try
            {
                using (Region clipRegion = new Region(clipRect))
                {
                    clipRegion.Intersect(WorkingRectangle);
                    gs.Graphics.Clip = clipRegion;
                    for (int i = ExcludeFirstChildrenFromVScroll; i < Children.Count; i++)
                    {
                        Section s = Children[i];
                        if (clipRect.IntersectsWith(s.HostBasedRectangle))
                        {
                            s.Paint(gs, clipRect);
                        }
                    }
                }
            }
            finally
            {
                gs.Graphics.EndContainer(container);
            }
        }

        public VScrollBar VScrollbar
        {
            get
            {
                return _vScrollbar;
            }
        }

        public HScrollBar HScrollbar
        {
            get
            {
                return _hScrollbar;
            }
        }

        private void VScrollbar_ValueChanged(object sender, EventArgs e)
        {
            OnVScrollValueChanged(_vScrollbar.Value);
        }

        private void HScrollbar_ValueChanged(object sender, EventArgs e)
        {
            OnHScrollValueChanged(_hScrollbar.Value);
        }

        protected int ExcludeFirstChildrenFromVScroll
        {
            get
            {
                return _excludeFirstChildrenFromVScroll;
            }
            set
            {
                _excludeFirstChildrenFromVScroll = value;
            }
        }

        #region NonSelectableHScrollBar

        internal sealed class NonSelectableHScrollBar : HScrollBar
        {
            internal NonSelectableHScrollBar()
            {
                SetStyle(ControlStyles.Selectable, false);
            }
        }

        #endregion

        #region NonSelectableVScrollBar

        internal sealed class NonSelectableVScrollBar : VScrollBar
        {
            internal NonSelectableVScrollBar()
            {
                SetStyle(ControlStyles.Selectable, false);
            }
        }

        #endregion

        private int _excludeFirstChildrenFromVScroll = 0;
        private NonSelectableVScrollBar _vScrollbar = null;
        private NonSelectableHScrollBar _hScrollbar = null;
    }
}