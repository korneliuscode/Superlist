/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using BinaryComponents.Utility.Collections;

namespace BinaryComponents.SuperList.Sections
{
    public class SectionContainer : Section
    {
        public SectionContainer(ISectionHost hostControl)
            : base(hostControl)
        {
        }

        public override void Paint(GraphicsSettings gs, Rectangle clipRect)
        {
            foreach (Section s in Children)
            {
                if (clipRect.IntersectsWith(s.HostBasedRectangle))
                {
                    s.Paint(gs, clipRect);
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            DeleteChildren();
        }

        public void DeleteChildren()
        {
            foreach (Section s in Children)
            {
                s.Dispose();
            }
            Children.Clear();
        }

        public void DeleteRange(int start, int count)
        {
            int counter = count;
            for (int i = start; counter-- > 0; i++)
            {
                Children[i].Dispose();
            }
            Children.RemoveRange(start, count);
        }


        public override void PaintBackground(GraphicsSettings gs, Rectangle clipRect)
        {
            foreach (Section s in Children)
            {
                if (clipRect.IntersectsWith(s.Rectangle))
                {
                    s.PaintBackground(gs, clipRect);
                }
            }
        }

        public override void Layout(GraphicsSettings gs, Size maximumSize)
        {
            base.Layout(gs, maximumSize);
            Point pt = Location;

            //
            // Default handling is to arrange down.
            foreach (Section s in Children)
            {
                s.Location = pt;
                s.Layout(gs, maximumSize);
                pt.Y = s.Rectangle.Bottom;
                maximumSize.Height -= s.Size.Height;
            }
        }

        public override Point Location
        {
            get
            {
                return base.Location;
            }
            set
            {
                Point oldLocation = Location;
                base.Location = value;

                int deltaX = Location.X - oldLocation.X;
                int deltaY = Location.Y - oldLocation.Y;
                foreach (Section s in Children)
                {
                    s.Location = new Point(s.Location.X + deltaX, s.Location.Y + deltaY);
                }
            }
        }

        public override Section SectionFromPoint(Point pt)
        {
            pt.X += GetAbsoluteScrollCoordinates().X;
            pt.Y += GetAbsoluteScrollCoordinates().Y;
            foreach (Section s in Children)
            {
                Section sectionFromPoint = s.SectionFromPoint(pt);
                if (sectionFromPoint != null)
                {
                    return sectionFromPoint;
                }
            }
            return base.SectionFromPoint(pt);
        }

        public EventingList<Section> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new EventingList<Section>();
                    _children.DataChanged += ChildrenDataChanged;
                }
                return _children;
            }
        }

        private void ChildrenDataChanged(object sender, EventingList<Section>.EventInfo e)
        {
            switch (e.EventType)
            {
                case EventingList<Section>.EventType.Deleted:
                    foreach (Section s in e.Items)
                    {
                        s.Parent = null;
                    }
                    break;
                case EventingList<Section>.EventType.Added:
                    foreach (Section s in e.Items)
                    {
                        s.Parent = this;
                    }
                    break;
            }
        }

        private EventingList<Section> _children;
    }
}