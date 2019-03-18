using System;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace ClickAndQuery.Utility
{
	public static class TaggedItems
	{
		#region Tag Class
		public class Tag
		{
			public string Name;

			public string Value;

			public int StartIndex;
			public int EndIndex;
			public int TagSourceCharCount
			{
				get
				{
					return EndIndex - StartIndex;
				}
			}
		}
		#endregion

		public enum ExtractPolicy{ IncludeAllTags, CoalesceSameNameTags };

		public static List<Tag> Extract( ExtractPolicy extractPolicy, string source )
		{
			return Extract( extractPolicy, new string[] { source } );
		}
		public static List<Tag> Extract( ExtractPolicy extractPolicy, IEnumerable<string> sources )
		{
			BinaryComponents.Utility.Collections.Set<string> tagsFound = new BinaryComponents.Utility.Collections.Set<string>();
			List<Tag> tags = new List<Tag>();

			foreach( string source in sources )
			{
				int start = 0;
				while( source != null )
				{
					int found = source.IndexOf( PrefixParam, start );
					if( found == -1 )
					{
						break; // finished
					}
					else
					{
						Tag tag = new Tag();
						tag.StartIndex = found;

						start = found + 1;
						if( source.Length > found + 2 && source[found + 1] == '{' )
						{
							start++; // Jump over bracket.

							//
							//	Get Name
							int separator = source.IndexOf( '}', start );
							if( separator != -1 )
							{
								string name = source.Substring( start, separator - start ).Trim();

								if( name.Length > 0 )
								{
									tag.Name = name;
									tag.EndIndex = separator + 1;
									bool alreadyFound = tagsFound.Contains( tag.Name );
									if( extractPolicy == ExtractPolicy.IncludeAllTags || !alreadyFound )
									{
										if( !alreadyFound )
										{
											tagsFound.Add( tag.Name );
										}
										tags.Add( tag );
									}
								}
							}
						}
					}
				}
			}
			return tags;
		}

		public static string Replace( string source, List<Tag> tags )
		{
			List<Tag> tagsFound = Extract( ExtractPolicy.IncludeAllTags, source );
			string updatedSource = source;

			//
			//	We reverse so when we replace tags in the 'source' we don't upset
			//	the indexes.
			tagsFound.Reverse();

			foreach( Tag tag in tagsFound )
			{
				Tag tagWithValue = FindTag( tag.Name, tags );
				if( tagWithValue == null )
					throw new Exception( string.Format( "No matching tag '{0}' with a value was found", tag.Name ) );
				if( tagWithValue.Value == null )
					throw new Exception( string.Format( "A value was not supplied for Tag '{0}'", tag.Name ) );

				updatedSource = updatedSource.Remove( tag.StartIndex, tag.TagSourceCharCount );
				updatedSource = updatedSource.Insert( tag.StartIndex, tagWithValue.Value );
			}
			return updatedSource;
		}

		private static Tag FindTag( string tagName, IEnumerable<Tag> tags )
		{
			foreach( Tag tag in tags )
			{
				if( tag.Name == tagName )
				{
					return tag;
				}
			}
			return null;
		}

		private const string PrefixParam = "$";
	}
}
