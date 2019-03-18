/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using BinaryComponents.Utility.Collections;
using System.Collections.Generic;

namespace BinaryComponents.SuperList.Sections
{
	public class SectionFactory
	{
		public SectionFactory()
		{
			CellSectionImage.RegisterMapping( this );
		}
		public virtual ListSection CreateListSection( ListControl listControl )
		{
			return new ListSection( listControl );
		}

		public virtual HeaderSection CreateHeaderSection( ISectionHost hostControl, EventingList<Column> columns )
		{
			return new HeaderSection( hostControl, columns );
		}

		public virtual HeaderColumnSection CreateHeaderColumnSection( ISectionHost host, HeaderColumnSection.DisplayMode displayMode, Column column )
		{
			return new HeaderColumnSection( host, displayMode, column );
		}

		public virtual GroupSection CreateGroupSection( ListControl listControl, RowIdentifier ri, HeaderSection headerSection, int position, int groupIndentWidth )
		{
			return new GroupSection( listControl, ri, headerSection, position, groupIndentWidth );
		}

		public virtual RowSection CreateRowSection( ListControl listControl, RowIdentifier rowIdentifier, HeaderSection headerSection, int position )
		{
			return new RowSection( listControl, rowIdentifier, headerSection, position );
		}

		public delegate CellSection CellSectionCreator( ISectionHost host, HeaderColumnSection hcs, object item );
		/// <summary>
		/// Allows you to map a cell values type given by a column item accessor to a Cell type. We use this for registering
		/// Image and Checkbox handling Cells.
		/// </summary>
		/// <param name="valueType">A cell values type given by a column item accessor</param>
		/// <param name="creator">Delegate that gets called to created a mapped CellSection</param>
		public void RegisterCellSection( Type valueType,  CellSectionCreator creator )
		{
			if( _cellSectionFactoryMap.ContainsKey( valueType ) )
			{
				_cellSectionFactoryMap.Remove( valueType );
			}
			_cellSectionFactoryMap.Add( valueType, creator );
		}


		public virtual CellSection CreateCellSection( ISectionHost host, HeaderColumnSection hcs, object item )
		{
			object cellValue = item == null ? null : hcs.Column.ColumnItemAccessor( item );
			if( cellValue != null )
			{
				Type type = cellValue.GetType();
				foreach( KeyValuePair<Type, CellSectionCreator> kv in _cellSectionFactoryMap )
				{
					if( type == kv.Key || type.IsSubclassOf( kv.Key ) )
					{
						return kv.Value( host, hcs, item );
					}
				}
			}
			return new CellSection( host, hcs, item );
		}

		public virtual Section CreateCustomiseListSection( ListControl listControl )
		{
			return new CustomiseListSection( listControl );
		}

		public virtual Section CreateCustomiseGroupingSection( ISectionHost hostControl, EventingList<Column> columns )
		{
			return new CustomiseGroupingSection( hostControl, columns );
		}

		public virtual OptionsToolbarSection CreateOptionsToolbarSection( ListControl listControl )
		{
			return new ToolStripOptionsToolbarSection( listControl );
		}

		public virtual HeaderColumnSection CreateAvailableColumnSection( ISectionHost host, Column column )
		{
			return new AvailableColumnsSection.AvailableColumnSection( host, column );
		}

		private Dictionary<Type, CellSectionCreator> _cellSectionFactoryMap = new Dictionary<Type, CellSectionCreator>();
	}
}