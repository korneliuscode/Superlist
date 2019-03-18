/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;

namespace BinaryComponents.SuperList.Sections
{
    public class CustomiseListSection : SectionContainer
    {
        public CustomiseListSection(ListControl listControl)
            : base(listControl)
        {
            _customiseGroupingSection = listControl.SectionFactory.CreateCustomiseGroupingSection(listControl, listControl.Columns.GroupedItems);
            _optionsToolbarSection = listControl.SectionFactory.CreateOptionsToolbarSection(listControl);
            Children.Add(_customiseGroupingSection);
            Children.Add(_optionsToolbarSection);
        }

        public override void Layout(GraphicsSettings gs, Size maximumSize)
        {
            CustomiseGroupingSection groupSection = (CustomiseGroupingSection) _customiseGroupingSection;
            OptionsToolbarSection optionsSection = (OptionsToolbarSection) _optionsToolbarSection;
            ListSection listSection = ListSection;

            groupSection.Location = Location;
            groupSection.Layout(gs, maximumSize);

            optionsSection.Layout(gs,
                                  new Size(Math.Max(optionsSection.MinimumWidth, maximumSize.Width - groupSection.MinimumWidth),
                                           groupSection.Size.Height));

            groupSection.Size = new Size(maximumSize.Width - optionsSection.Size.Width, groupSection.Size.Height);
            optionsSection.Location = new Point(groupSection.Rectangle.Right, Location.Y);
            optionsSection.Visible = true;

            Size = new Size(maximumSize.Width, optionsSection.Size.Height);
        }

        public override void PaintBackground(GraphicsSettings gs, Rectangle clipRect)
        {
            gs.Graphics.FillRectangle(SystemBrushes.ControlDark, Rectangle);
            base.PaintBackground(gs, clipRect);
        }

        public ListSection ListSection
        {
            get
            {
                return ((ListControl) Host).ListSection;
            }
        }

        private Section _customiseGroupingSection;
        private Section _optionsToolbarSection;
    }
}