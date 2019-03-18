using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace ClickAndQuery.Data
{
	public class ConnectionDetails
	{
		public SqlConnection Connect()
		{
			string connectionString;
			switch( AuthenticationStyle )
			{
				case AuthenticationStyle.Integrated:
					connectionString = string.Format( "Data Source={0};Initial Catalog={1};Integrated Security=true;Asynchronous Processing=True", Server, Database );
					break;
				case AuthenticationStyle.Server:
					connectionString = string.Format( "Server = {0}; Database = {1}; Integrated Security=false;User ID = {2}; pwd={3};Asynchronous Processing=True", Server, Database, User, Pwd );
					break;
				default:
					throw new NotImplementedException();
			}

			SqlConnection connection =  new SqlConnection( connectionString );
			connection.Open();
			return connection;
		}

		public ConnectionDetails Clone()
		{
			ConnectionDetails ds = new ConnectionDetails();
			ds.Server = Server;
			ds.Database = Database;
			ds.AuthenticationStyle = AuthenticationStyle;
			ds.User = User;
			ds.Pwd = Pwd;
			return ds;
		}

		public string Server = "(local)";
		public string Database = null;
		public AuthenticationStyle AuthenticationStyle = AuthenticationStyle.Integrated;
		public string User;
		public string Pwd;
	}
}
