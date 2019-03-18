using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using ClickAndQuery.Utility;

namespace ClickAndQuery.Forms
{
	public partial class ClickAndQueryForm : Form
	{
		public ClickAndQueryForm()
		{
			InitializeComponent();
			new SelfRegisteringWatch( new ControlPreferences.FormWatch( this ) );
			new SelfRegisteringWatch( new ControlPreferences.SplitContainerWatch( _splitContainer ) );
			_recentFiles = new RecentFiles( _fileToolStripMenuItem, _recentFilesSeparator, RecentFileClickedHandler );


			//
			// Bind our commands to their menus
			new DelegateCommand( delegate { NewDocument(); } ).Bind( _newToolStripMenuItem );
			new DelegateCommand( delegate { OpenFile(); } ).Bind( _openToolStripMenuItem );
			new DelegateCommand( delegate { Save(); } ).Bind( _saveToolStripMenuItem );
			new DelegateCommand( delegate { SaveAs(); } ).Bind( _saveAsToolStripMenuItem );
			new DelegateCommand( delegate { Close(); } ).Bind( _exitToolStripMenuItem );
			new DelegateCommand( delegate { CopyToClipboard(); } ).Bind( _copyToolStripMenuItem );
			new DelegateCommand( delegate { ShowOptionsForm(); } ).Bind( _optionsToolStripMenuItem );
			new DelegateCommand( delegate { ShowAboutForm(); } ).Bind( _aboutToolStripMenuItem );
			new DelegateCommand( delegate { _resultsControl.CancelQuery(); }, delegate { return _resultsControl.IsQueryExecuting; } ).Bind( _cancelToolStripMenuItem );
			new DelegateCommand( delegate { _resultsControl.SizeColumnsToFit(); } ).Bind( _sizeColumnsToFitToolStripMenuItem );
			new DelegateCommand( delegate { ExecuteQuery(); }, delegate { return _document != null && _document.Query.Length > 0; } ).Bind( _executeToolStripMenuItem );
			new DelegateCommand( delegate { ShowConnectionDetailsForm(); } ).Bind( _connectionDetailsToolStripMenuItem );
			
						
			ToolStripCommandHelper.BindToolStripItems( _menuStrip, _toolbarStrip );

			UpdateCaption();
		}

		private bool _startup = true;

		protected override void OnHandleCreated( EventArgs e )
		{
			base.OnHandleCreated( e );
			Application.Idle += new EventHandler( Application_Idle );
		}


		protected override void OnHandleDestroyed( EventArgs e )
		{
			base.OnHandleDestroyed( e );
			Application.Idle -= new EventHandler( Application_Idle );
			_resultsControl.CancelQuery();
			CloseCurrentConnection();
		} 


		private void RecentFileClickedHandler( FileInfo fileInfo )
		{
			OpenFile( fileInfo.FullName, true );
		}



		private void NewDocument()
		{
			Data.ConnectionDetails connectionDetails = _document.ConnectionDetails.Clone();
			_fileName = null;
			_document = new Data.Document();
			_document.ConnectionDetails = connectionDetails;
			_queryTextbox.Clear();
			_resultsControl.Clear();
			UpdateCaption();
		}

		private void ExecuteQuery()
		{
			try
			{
				if( _document.Query.Length > 0 )
				{
					if( string.IsNullOrEmpty( _document.ConnectionDetails.Database ) )
					{
						if(
							MessageBox.Show( this, 
							"Connection needs to be opened first, ppen Connection to a database?", _originalCaption, MessageBoxButtons.OKCancel ) == DialogResult.Cancel
							|| !ShowConnectionDetailsForm() )
						{
							return;
						}
					}
					Data.ConnectionDetails cd = _document.ConnectionDetails.Clone();
					List<string> taggableSources = new List<string>( new string[]{ cd.Server, cd.User, cd.Pwd } );
					bool connectionHasTags = TaggedItems.Extract( TaggedItems.ExtractPolicy.CoalesceSameNameTags, taggableSources.ToArray() ).Count > 0;

					if( !connectionHasTags )
					{
						taggableSources.Clear();
					}
					taggableSources.Add( _document.Query );
					List<TaggedItems.Tag> tags = TaggedItems.Extract( TaggedItems.ExtractPolicy.CoalesceSameNameTags, taggableSources.ToArray() );
					if( FillTagValues( tags ) )
					{

						SqlConnection sqlConnection;
						if( connectionHasTags )
						{
							if( _sqlConnection != null )
							{
								_sqlConnection.Dispose();
								_sqlConnection = null;
							}
							cd.User = TaggedItems.Replace( cd.User, tags );
							cd.Server = TaggedItems.Replace( cd.Server, tags );
							cd.Pwd = TaggedItems.Replace( cd.Pwd, tags );
							sqlConnection = cd.Connect();
						}
						else
						{
							if( _sqlConnection == null )
							{
								_sqlConnection = _document.ConnectionDetails.Connect();
							}
							sqlConnection = _sqlConnection;
						}

						SqlCommand command = new SqlCommand( TaggedItems.Replace( _document.Query, tags ), sqlConnection );

						_resultsControl.ExecuteReader( command, connectionHasTags ? CommandBehavior.CloseConnection : CommandBehavior.Default );
					}
				}
			}
			catch( Exception e )
			{
				MessageBox.Show( this, e.Message );
			}
		}

		private bool FillTagValues( List<TaggedItems.Tag> tags )
		{
			bool result = true;
			if( tags.Count > 0 )
			{
				//
				// Load historic values if any into tags
				foreach( TaggedItems.Tag tag in tags )
				{
					_tagToValuesHistory.TryGetValue( tag.Name, out tag.Value );
				}

				//
				// Ask the user
				AquireParameterValuesForm form = new AquireParameterValuesForm( tags );
				result = form.ShowDialog() == DialogResult.OK;

				//
				// Save in history
				foreach( TaggedItems.Tag tag in tags )
				{
					_tagToValuesHistory[tag.Name] =  tag.Value;
				}
			}
			return result;
		}


		private void UpdateCaption()
		{
			if( _originalCaption == null )
			{
				_originalCaption = this.Text;
			}
			if( _fileName == null )
			{
				this.Text = "Untitled - " + _originalCaption;
			}
			else
			{
				this.Text = FileNameFromFile( _fileName ) +" - " + _originalCaption;
			}
		}

		private string FileNameFromFile( string file )
		{
			return Path.GetFileName( Path.ChangeExtension( _fileName, null ) );
		}

		private void OpenFile( string file, bool executeQuery )
		{
			_resultsControl.CancelQuery();
			Cursor = Cursors.WaitCursor;
			try
			{
				using( TextReader textReader = File.OpenText( file ) )
				{
					CloseCurrentConnection();
					XmlSerializer serializer = new XmlSerializer( typeof( Data.Document ) );
					_document = (Data.Document)serializer.Deserialize( textReader );
					_queryTextbox.Text = _document.Query;

					_fileName = file;
					_recentFiles.Add( file );

					if( !string.IsNullOrEmpty( _document.SuperlistState ) )
					{
						_resultsControl.Clear();
						_resultsControl.DeSerializeState( new StringReader( _document.SuperlistState ) );
					}

					UpdateCaption();
					if( Program.RunQueryOnOpen && executeQuery )
					{
						ExecuteQuery();
					}
					else
					{
						_resultsControl.Clear();
					}
				}
			}
			catch( Exception exception )
			{
				MessageBox.Show( this, exception.Message );
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void OpenFile()
		{
			using( OpenFileDialog ofd = new OpenFileDialog() )
			{
				ofd.AddExtension = true;
				ofd.CheckFileExists = true;
				ofd.CheckPathExists = true;
				ofd.DefaultExt = "DBQuery";
				ofd.Filter = _filter;
				ofd.InitialDirectory = System.Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments );
				ofd.Multiselect = false;
				ofd.Title = "Open Query";
				ofd.ValidateNames = true;

				if( ofd.ShowDialog( this ) == DialogResult.OK )
				{
					OpenFile( ofd.FileName, true );
				}
			}
		}



	



		private bool ShowConnectionDetailsForm()
		{
			ConnectionDetailsForm form = new ConnectionDetailsForm( _document.ConnectionDetails );
			if( form.ShowDialog( this ) == DialogResult.OK )
			{
				CloseCurrentConnection();
				return true;
			}
			return false;
		}

		private void CloseCurrentConnection()
		{
			if( _sqlConnection != null )
			{
				_sqlConnection.Close();
				_sqlConnection.Dispose();
				_sqlConnection = null;
			}
		}
		private void SaveAs()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.CheckFileExists = false;
			ofd.AddExtension = true;
			ofd.Filter = _filter;
			ofd.Title = "Save Query";
			if( ofd.ShowDialog( this ) == DialogResult.OK )
			{
				_fileName = ofd.FileName;
				Save();
				UpdateCaption();
			}
		}

		private void Save()
		{
			if( string.IsNullOrEmpty( _fileName ) )
			{
				SaveAs();
			}
			else
			{
				try
				{
					//
					//	Save Superlist state into our document variable before serializing
					//	the lot out.
					using( StringWriter stringWriter = new StringWriter() )
					{
						_resultsControl.SerializeState( stringWriter );
						_document.SuperlistState = stringWriter.ToString();
					}
					using( TextWriter textWriter = File.CreateText( _fileName ) )
					{
						XmlSerializer serializer = new XmlSerializer( typeof( Data.Document ) );
						serializer.Serialize( textWriter, _document );
						_recentFiles.Add( _fileName );
					}
				}
				catch( Exception exception )
				{
					MessageBox.Show( this, exception.Message );
				}
			}
		}

		private void _queryTextbox_TextChanged( object sender, EventArgs e )
		{
			_document.Query = _queryTextbox.Text;
		}

		private void CopyToClipboard()
		{
			if( _queryTextbox.Focused )
			{
				Clipboard.SetText( _queryTextbox.Text );
			}
			else
			{
				_resultsControl.CopyToClipboard();
			}
		}


		private void ShowOptionsForm()
		{
			OptionsForm form = new OptionsForm();
			form.ShowDialog( this );
		}

		private void ShowAboutForm()
		{
			AboutForm form = new AboutForm();
			form.ShowDialog();
		}

		private void Application_Idle( object sender, EventArgs e )
		{
			if( _startup )
			{
				_startup = false;
				string[] cmdLine = Environment.GetCommandLineArgs();
				bool edit = false;
				foreach( string s in cmdLine )
				{
					if( s == "\\Edit" )
					{
						edit = true;
					}
				}
				if( cmdLine.Length > 1 )
				{
					OpenFile( cmdLine[1], !edit );
				}
			}

			ToolStripCommandHelper.SetEnabledProperties( _menuStrip );
			ToolStripCommandHelper.SetEnabledProperties( _toolbarStrip );

			_cancelToolStripMenuItem.Enabled = _resultsControl.IsQueryExecuting;
			if( _resultsControl.IsQueryFinished && _resultsControl.LastQueryError != null )
			{
				Exception exception = _resultsControl.LastQueryError;
				_resultsControl.LastQueryError = null;
				MessageBox.Show( this, exception.Message, _originalCaption );
			}
		}



		private Dictionary<string, string> _tagToValuesHistory = new Dictionary<string, string>();
		private SqlConnection _sqlConnection = null;
		private string _filter = "DBQuery files|*.DBQuery|Query files|*.qry";
		private string _fileName = null;
		private Data.Document _document = new ClickAndQuery.Data.Document();
		private string _originalCaption;
		private RecentFiles _recentFiles;
	}
}