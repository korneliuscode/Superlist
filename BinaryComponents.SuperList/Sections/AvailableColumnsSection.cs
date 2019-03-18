/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System.Drawing;
using System.Windows.Forms;
using BinaryComponents.Utility.Collections;

namespace BinaryComponents.SuperList.Sections
{
	public class AvailableColumnsSection : HeaderColumnSectionContainer
	{
		public AvailableColumnsSection( ISectionHost host, EventingList<Column> columns )
			: base( host, columns )
		{
			columns.DataChanged += columns_DataChanged;
		}

		public override bool CanDrop( IDataObject dataObject, CanDropQueryContext context )
		{
			return false;
		}

		public override bool AllowColumnsToBeDroppedInVoid
		{
			get
			{
				return false;
			}
		}

		public override bool ShouldRemoveColumnOnDrop( Column column )
		{
			return false;
		}

		public override void Layout( GraphicsSettings gs, Size maximumSize )
		{
			base.Layout( gs, maximumSize );
			Size = new Size( Size.Width, Children.Count == 0 ? 0 : Children[Children.Count - 1].Rectangle.Bottom - Rectangle.Top );
		}

		protected override HeaderColumnSection CreateHeaderColumnSection( HeaderColumnSection.DisplayMode displayModeToCreate, Column column )
		{
			return Host.SectionFactory.CreateAvailableColumnSection( Host, column );
		}

		public class AvailableColumnSection : HeaderColumnSection
		{
			public AvailableColumnSection( ISectionHost host, Column column )
				: base( host, DisplayMode.Customise, column )
			{
			}

			public override void MouseUp( MouseEventArgs e )
			{
			}

			protected override TextFormatFlags GetTextFormatFlags()
			{
				return TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis;
			}

			public override void Layout( GraphicsSettings gs, Size maximumSize )
			{
				base.Layout( gs, maximumSize );
				Size = new Size( maximumSize.Width, Size.Height );
			}

			protected override void DrawSortArrow( GraphicsSettings gs, Rectangle rc )
			{
				// do nothing.
			}
		}

		private void columns_DataChanged( object sender, EventingList<Column>.EventInfo e )
		{
			SyncSectionsToColumns( HeaderColumnSection.DisplayMode.Customise );
			Host.LazyLayout( null );
		}
	}
}