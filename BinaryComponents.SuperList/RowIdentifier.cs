/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;

namespace BinaryComponents.SuperList
{
	public abstract class RowIdentifier
	{
		public Column LastColumn
		{
			get
			{
				return GroupColumns.Length > 0 ? GroupColumns[GroupColumns.Length - 1] : null;
			}
		}

		public abstract Column[] GroupColumns
		{
			get;
		}

		public abstract object[] GroupValues
		{
			get;
		}

		public object LastGroupValue
		{
			get
			{
				return GroupValues.Length == 0 ? null : GroupValues[GroupValues.Length - 1];
			}
		}

		public abstract object[] Items
		{
			get;
		}

		public static bool operator ==( RowIdentifier lhs, RowIdentifier rhs )
		{
			if( ReferenceEquals( lhs, rhs ) )
			{
				return true;
			}
			if( ReferenceEquals( lhs, null ) )
			{
				return false;
			}
			if( ReferenceEquals( rhs, null ) )
			{
				return false;
			}

			return lhs.Equals( rhs );
		}

		public static bool operator !=( RowIdentifier lhs, RowIdentifier rhs )
		{
			return !(lhs == rhs);
		}

		public override bool Equals( object obj )
		{
			throw new NotImplementedException();
		}

		public override int GetHashCode()
		{
			throw new NotImplementedException();
		}
	}
}