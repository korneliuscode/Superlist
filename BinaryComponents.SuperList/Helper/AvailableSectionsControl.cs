/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System.Drawing;
using BinaryComponents.SuperList.Sections;
using BinaryComponents.Utility.Collections;

namespace BinaryComponents.SuperList.Helper
{
    internal class AvailableSectionsControl : SectionContainerControl
    {
        public AvailableSectionsControl()
        {
            InitializeComponent();
        }


        public ColumnList ColumnList
        {
            get
            {
                return _columnList;
            }
            set
            {
                if (_columnList != value)
                {
                    if (_columnList != null)
                    {
                        _columnList.DataChanged -= columnList_DataChanged;
                        _availableColumns.Clear();
                    }

                    _columnList = value;

                    if (_columnList != null)
                    {
                        foreach (Column column in _columnList)
                        {
                            _availableColumns.Add(column);
                        }
                        _columnList.DataChanged += columnList_DataChanged;
                    }
                }
            }
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_columnList != null)
            {
                _columnList.DataChanged -= columnList_DataChanged;
                _columnList = null;
            }
        }

        private void columnList_DataChanged(object sender, EventingList<Column>.EventInfo e)
        {
            switch (e.EventType)
            {
                case EventingList<Column>.EventType.Deleted:
                    foreach (Column c in e.Items)
                    {
                        _availableColumns.Remove(c);
                    }
                    break;
                case EventingList<Column>.EventType.Added:
                    foreach (Column c in e.Items)
                    {
                        _availableColumns.Add(c);
                    }
                    break;
                default:
                    break;
            }
        }

        public override void LayoutControl()
        {
            StopLazyUpdateTimer();
            Canvas.Location = Point.Empty;
            using (Graphics grfx = CreateGraphics())
            {
                Canvas.Layout(new Section.GraphicsSettings(grfx), new Size(ClientRectangle.Width, 1024));
            }
            Invalidate();

            Height = Canvas.Rectangle.Height;
        }

        protected override SectionContainer CreateCanvas()
        {
            return new AvailableColumnsSection(this, _availableColumns);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            ResumeLayout(false);
        }

        private EventingList<Column> _availableColumns = new EventingList<Column>();
        private ColumnList _columnList;
    }
}