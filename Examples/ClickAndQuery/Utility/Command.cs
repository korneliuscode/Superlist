using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace ClickAndQuery.Utility
{
	public abstract class Command
	{
		public abstract bool IsEnabled { get; }
		public abstract void Invoke();

		public void Bind( ToolStripItem item )
		{
			item.Tag = this;
			item.Click += delegate { Invoke(); };
		}
	}

	public delegate void CommandInvokeHandler();
	public delegate bool CommandIsEnabledHandler();

	public class DelegateCommand: Command
	{
		public DelegateCommand( CommandInvokeHandler commandInvokeHandler, CommandIsEnabledHandler commandIsEnabledHandler )
		{
			_commandInvokeHandler = commandInvokeHandler;
			_commandIsEnabledHandler = commandIsEnabledHandler;
		}
		public DelegateCommand( CommandInvokeHandler commandInvokeHandler )
			: this( commandInvokeHandler, null )
		{
		}
		public override bool IsEnabled
		{
			get
			{
				return _commandIsEnabledHandler == null ? true : _commandIsEnabledHandler();
			}
		}

		public override void Invoke()
		{
			_commandInvokeHandler();
		}

		private CommandInvokeHandler _commandInvokeHandler;
		private CommandIsEnabledHandler _commandIsEnabledHandler;
	}
}
