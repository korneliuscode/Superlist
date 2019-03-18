using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ClickAndQuery.Utility;

namespace ClickAndQuery
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			FileControlPreferences = new FileControlPreferences( System.IO.Path.Combine( DataFolder, "ControlPreferences.xml" ) );
			FileControlPreferences.Load();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new Forms.ClickAndQueryForm() );

			FileControlPreferences.Dispose();
			FileControlPreferences = null;
		}

		public static bool RunQueryOnOpen
		{
			get
			{
				string result = FileControlPreferences.GetValue( "Options", "RunQueryOnOpen" ); 
				return result == null || result == true.ToString( System.Globalization.CultureInfo.InvariantCulture ); // default is true
			}
			set
			{
				FileControlPreferences.SetValue( "Options", "RunQueryOnOpen", value.ToString( System.Globalization.CultureInfo.InvariantCulture ) );
			}
		}

		private static string DataFolder
		{
			get
			{
				string localAppData = System.Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );
				localAppData = System.IO.Path.Combine( localAppData, @"BinaryComponents\ClickAndQuery" );

				return localAppData;
			}
		}

		public static FileControlPreferences FileControlPreferences
		{
			get
			{
				return _controlPreferences;
			}
			private set
			{
				_controlPreferences = value;
			}
		}

		private static FileControlPreferences _controlPreferences = null;
	}
}