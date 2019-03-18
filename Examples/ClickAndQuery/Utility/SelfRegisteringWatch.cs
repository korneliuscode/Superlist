using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ClickAndQuery.Utility;

namespace ClickAndQuery
{
	/// <summary>
	/// Handles registering and unregistering ControlPreference Watches (form, splitter ).
	/// </summary>
	public sealed class SelfRegisteringWatch 
	{
		public SelfRegisteringWatch( ControlPreferences.Watch watch )
		{
			_watch = watch;

			if( _watch.Control.IsHandleCreated )
			{
				RegisterWatch();
			}
			_watch.Control.HandleCreated += control_HandleCreated;
		}

		private void control_HandleDestroyed( object sender, EventArgs e )
		{
			_watch.Control.HandleCreated -= control_HandleCreated;
			_watch.Control.HandleDestroyed -= control_HandleDestroyed;
			Program.FileControlPreferences.UnregisterWatch( _watch.Control );
		}

		private void control_HandleCreated( object sender, EventArgs e )
		{
			RegisterWatch();
		}

		private void RegisterWatch()
		{
			Program.FileControlPreferences.RegisterWatch( _watch );
			_watch.Control.HandleDestroyed += control_HandleDestroyed;
		}

		private ControlPreferences.Watch _watch;
	}
}
