using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace BinaryComponents.SuperList.Sections
{
	public class CellSectionImage : CellSection
	{
		/// <summary>
		/// Image handling CellSectio registration
		/// </summary>
		/// <param name="factory"></param>
		public static void RegisterMapping( SectionFactory factory )
		{
			factory.RegisterCellSection(
				typeof( System.Drawing.Image ),
				delegate( ISectionHost host, HeaderColumnSection hcs, object item )
				{
					return new CellSectionImage( host, hcs, item );
				}
			);
		}

		public CellSectionImage( ISectionHost host, HeaderColumnSection hcs, object item )
			: base( host, hcs, item )
		{
		}

		public override void Layout( Section.GraphicsSettings gs, Size maximumSize )
		{
			if( this.MinimumHeight < Image.Height )
			{
				this.MinimumHeight = Image.Height;
			}
			base.Layout( gs, maximumSize );
		}

		public override Size GetIdealSize( GraphicsSettings gs )
		{
			return new Size( Image.Width + 2, Image.Height + 2 );
		}

		public override void Paint( Section.GraphicsSettings gs, Rectangle clipRect )
		{
			Rectangle rc = DrawRectangle;
			Image image = Image;
			gs.Graphics.DrawImage( Image, new Point( rc.Left + (rc.Width - image.Width ) / 2, rc.Top + (rc.Height - image.Height ) / 2 ) );
		}

		public Image Image
		{
			get
			{
				return (Image)HeaderColumnSection.Column.ColumnItemAccessor( Item );
			}
		}
	}
}
