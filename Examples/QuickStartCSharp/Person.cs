/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
//
// (c) 2006 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;

namespace SuperListTest
{
	public class Person : IComparable
	{
		public Person( bool opened, string surname, string firstname, string phone, string city, string state, string description, DateTime? date )
		{
			this.Open = opened;
			Surname = surname;
			Firstname = firstname;
			Phone = phone;
			City = city;
			State = state;
			Description = description;


			if( date == null )
			{
				date = DateTime.Now + new TimeSpan( _randomizer.Next( 0, 1000 * 60 * 60 * 24 * 7 ) );
			}
			Date = date.Value;
		}

		public Person( bool opened, string surname, string firstname, string phone, string city, string state, string description )
			: this( opened, surname, firstname, phone, city, state, description, null )
		{
		}

		private static int _nextId = 1;
		private int _id = _nextId++;


		private static Random _randomizer = new Random( 1 );

		public override string ToString()
		{
			return _id + " " + Firstname + " " + Surname;
		}

		public bool Open;
		public string Surname;
		public string Firstname;
		public string Phone;
		public string City;
		public string State;
		public DateTime Date;
		public string Description;

		public static Person[] GetData()
		{
			Person[] persons = new Person[]
                {
                    new Person( true, "Alexander", "Lee", "408 496-7223", "Menlo Park", "CA", "Great bloke and all round nice guy :-)", DateTime.Now),
                    new Person( true, "O'Leary", "Fay", "408 286-2428", "San Jose", "CA", string.Empty, DateTime.Now + new TimeSpan(0, 1, 0, 0)),
                    new Person( true, "Alexander", "Sam", "415 986-7020", "Oakland", "CA", string.Empty, DateTime.Now + new TimeSpan(0, 1, 0, 0)),
                    new Person( false, "O'Leary", "Jim", "408 286-2428", "San Jose", "CA", string.Empty, DateTime.Now + new TimeSpan(0, 0, 0, 0)),
                    new Person( true, "Alexander", "Harry", "415 548-7723", "Berkeley", "CA", "Likes to play with computer games far too much", DateTime.Now + new TimeSpan(0, 3, 0, 0)),
                    new Person( false, "O'Leary", "Jack", "408 286-2428", "San Jose", "CA", string.Empty, DateTime.Now + new TimeSpan(0, 0, 2, 0)),
                    new Person( true, "Straight", "Dean", "415 834-2919", "Oakland", "CA", string.Empty, DateTime.Now - new TimeSpan(3, 0, 0, 0)),
                    new Person( true, "Smith", "Stuart", "913 843-0462", "Lawrence", "KS", "Likes to read lots of books"),
                    new Person( false, "Bennet", "Abraham", "415 658-9932", "Berkeley", "CA", string.Empty),
                    new Person( true, "Dull", "Ann", "415 836-7128", "Palo Alto", "CA", string.Empty),
                    new Person( true, "Gringlesby", "Burt", "707 938-6445", "Covelo", "CA", string.Empty),
                    new Person( false, "Locksley", "Charlene", "415 585-4620", "San Francisco", "CA", string.Empty),
                    new Person( true, "Greene", "Morningstar", "615 297-2723", "Nashville", "TN", string.Empty),
                    new Person( true, "Blotchet-Halls", "Reginald", "503 745-6402", "Corvallis", "OR", string.Empty),
                    new Person( false, "Yokomoto", "Akiko", "415 935-4228", "Walnut Creek", "CA", string.Empty),
                    new Person( true, "del Castillo", "Innes", "615 996-8275", "Ann Arbor", "MI", string.Empty),
                    new Person( true, "DeFrance", "Michel", "219 547-9982", "Gary", "IN", string.Empty),
                    new Person( true, "Stringer", "Dirk", "415 843-2991", "Oakland", "CA", string.Empty),
                    new Person( false, "MacFeather", "Stearns", "415 354-7128", "Oakland", "CA", string.Empty),
                    new Person( true, "Karsen", "Livia", "415 534-9219", "Oakland", "CA", string.Empty),
                    new Person( true, "Panteley", "Sylvia", "301 946-8853", "Rockville", "MD", string.Empty),
                    new Person( true, "Hunter", "Sheryl", "415 836-7128", "Palo Alto", "CA", string.Empty),
                    new Person( false, "McBadden", "Heather", "707 448-4982", "Vacaville", "CA", string.Empty),
                    new Person( true, "Ringer", "Anne", "801 826-0752", "Salt Lake City", "UT", string.Empty),
                    new Person( false,"Ringer", "Albert", "801 826-0752", "Salt Lake City", "UT", string.Empty),
                };
			return persons;
		}

		#region IComparable Members

		public int CompareTo( object obj )
		{
			Person p = obj as Person;
			if( p == null )
			{
				return -1;
			}
			return _id - p._id;
		}

		#endregion
	} ;
}