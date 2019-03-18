/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using BinaryComponents.SuperList.Helper;
using BinaryComponents.SuperList.Sections;
using BinaryComponents.Utility.Collections;

namespace BinaryComponents.SuperList
{
	public class SectionContainerControl : UserControl, ISectionHost
	{
		public SectionContainerControl( SectionFactory sectionFactory )
		{
			InitializeComponent();

			SetStyle( ControlStyles.AllPaintingInWmPaint, true );
			SetStyle( ControlStyles.DoubleBuffer, true );
			SetStyle( ControlStyles.ResizeRedraw, true );
			SetStyle( ControlStyles.SupportsTransparentBackColor, true );

			_canvas = CreateCanvas();

			if( sectionFactory == null )
			{
				_sectionFactory = new SectionFactory();
			}
			else
			{
				_sectionFactory = sectionFactory;
			}
		}

		void SectionContainerControl_Paint(object sender, PaintEventArgs e)
		{
			throw new NotImplementedException();
		}

		public SectionContainerControl()
			: this( null )
		{
		}

		/// <summary>
		/// Same as DragOver but also included is the current Section the mouse is over
		/// </summary>
		public event SuperlistDragEventHandler DragOverEx;

		/// <summary>
		/// Same as DragDrop but also included is the current section the mouse is over.
		/// </summary>
		public event SuperlistDragEventHandler DragDropEx;

		public Section SectionFromPoint( Point pt )
		{
			if( _sectionWithMouseCapture != null )
			{
				return _sectionWithMouseCapture;
			}
			else
			{
				return _canvas.SectionFromPoint( pt );
			}
		}

		public virtual void LayoutControl()
		{
			StopLazyUpdateTimer();
			_canvas.Location = Point.Empty;
			using( Graphics grfx = CreateGraphics() )
			{
				_canvas.Layout( new Section.GraphicsSettings( grfx ), ClientRectangle.Size );
			}
			Invalidate();
		}

		public virtual SectionFactory SectionFactory
		{
			get
			{
				return _sectionFactory;
			}
			set
			{
				_sectionFactory = value;
			}
		}


		protected virtual SectionContainer CreateCanvas()
		{
			return new SectionContainer( this );
		}

		protected SectionContainer Canvas
		{
			get
			{
				return _canvas;
			}
		}

		#region Implementation

		#region ISectionHost Members

		Section ISectionHost.CurrentSectionDraggedOver
		{
			get
			{
				return _currentDropSection;
			}
		}

		void ISectionHost.Invalidate( Section section )
		{
			Invalidate( section.HostBasedRectangle );
		}

		Section ISectionHost.SectionFromClientPoint( Point ptClient )
		{
			return Canvas.SectionFromPoint( ptClient );
		}


		Point ISectionHost.PointToScreen( Point pt )
		{
			return PointToScreen( pt );
		}

		Point ISectionHost.PointToClient( Point pt )
		{
			return PointToClient( pt );
		}

		bool ISectionHost.IsControlCreated
		{
			get
			{
				return IsHandleCreated;
			}
		}


		void ISectionHost.DoDragDropOperation( Section sectionToDrag )
		{
			_draggingSections = sectionToDrag.GetExpandedDragList();
			object[] data = sectionToDrag.GetDragObjects();

			_imageWindowOffX = sectionToDrag.HostBasedRectangle.X - CursorClientPosition.X;
			_imageWindowOffY = sectionToDrag.HostBasedRectangle.Y - CursorClientPosition.Y;

			_imageWindow = CreateDragImageWindow( _draggingSections, data.Length != _draggingSections.Length );

			if( DoDragDrop( data, DragDropEffects.Copy | DragDropEffects.Move ) == DragDropEffects.None )
			{
				Point cursorPos = PointToClient( Cursor.Position );
				if( _lastDragAction == DragAction.Drop )
				{
					Rectangle rc = Rectangle.Empty;
					foreach( Section s in _draggingSections )
					{
						if( rc == Rectangle.Empty )
						{
							rc = s.HostBasedRectangle;
						}
						else
						{
							rc = Rectangle.Union( rc, s.HostBasedRectangle );
						}
					}
					foreach( Section s in _draggingSections )
					{
						if( s.CanDropInVoid && !rc.Contains( cursorPos ) )
						{
							s.DroppedInVoid();
						}
					}
				}
			}
			FinishDropOperation();
		}


		void ISectionHost.StartMouseCapture( Section section )
		{
			if( _sectionWithMouseCapture != null )
			{
				_sectionWithMouseCapture.CancelMouseCapture();
			}
			_sectionWithMouseCapture = section;
			Capture = true;
		}

		void ISectionHost.EndMouseCapture()
		{
			_sectionWithMouseCapture = null;
			Capture = false;
		}

		Section ISectionHost.SectionWithMouseCapture
		{
			get
			{
				return _sectionWithMouseCapture;
			}
		}

		Cursor ISectionHost.Cursor
		{
			get
			{
				return Cursor;
			}
			set
			{
				Cursor = value;
			}
		}

		bool ISectionHost.IsInDragOperation
		{
			get
			{
				return _draggingSections != null;
			}
		}

		ControlCollection ISectionHost.ControlCollection
		{
			get
			{
				return Controls;
			}
		}

		Graphics ISectionHost.CreateGraphics()
		{
			return CreateGraphics();
		}

		Font ISectionHost.Font
		{
			get
			{
				return Font;
			}
		}

		Color ISectionHost.TextColor
		{
			get
			{
				return ForeColor;
			}
		}

		void ISectionHost.ProcessLayoutsNow()
		{
			if( _lazyLayouts.Count == 1 && _lazyLayouts.ToArray()[0] == _canvas )
			{
				LayoutControl();
				_lazyLayouts.Clear();
			}
			else
			{
				using( Graphics grfx = CreateGraphics() )
				{
					Section.GraphicsSettings grfxSettings = new Section.GraphicsSettings( grfx );
					Section[] sectionsToLayout = _lazyLayouts.ToArray();
					_lazyLayouts.Clear();
					foreach( Section s in sectionsToLayout )
					{
						s.Layout( grfxSettings, s.Size );
						s.Invalidate();
					}
				}
			}
		}

		void ISectionHost.LazyLayout( Section s )
		{
			if( !IsHandleCreated )
			{
				return;
			}

			if( s == null )
			{
				s = _canvas;
			}

			bool found = false;
			if( s == _canvas )
			{
				_lazyLayouts.Clear();
			}
			else
			{
				//
				// Check to see if this section or any of its parents are going to be laid out.
				for( Section sectionToCheck = s; sectionToCheck != null; sectionToCheck = sectionToCheck.Parent )
				{
					if( _lazyLayouts.Contains( sectionToCheck ) )
					{
						found = true;
						break;
					}
				}
			}
			if( !found )
			{
				_lazyLayouts.Add( s );
			}
			StartLazyUpdateTimer();
		}

		protected override void OnGotFocus( EventArgs e )
		{
			base.OnGotFocus( e );

			if( _focusedSection != null )
			{
				_focusedSection.GotFocus();
			}

			Invalidate();
		}

		protected override void OnLostFocus( EventArgs e )
		{
			base.OnLostFocus( e );

			if( _focusedSection != null )
			{
				_focusedSection.LostFocus();
			}

			Invalidate();
		}

		Section ISectionHost.SectionMouseOver
		{
			get
			{
				return _sectionMouseOver;
			}
		}

		Section ISectionHost.FocusedSection
		{
			get
			{
				if( Focused )
				{
					if( _sectionWithMouseCapture != null )
					{
						return _sectionWithMouseCapture;
					}
					else
					{
						return _focusedSection;
					}
				}
				else
				{
					return null;
				}
			}
			set
			{
				if( _focusedSection != value )
				{
					Section oldFocusedSection = _focusedSection;

					_focusedSection = value;

					if( oldFocusedSection != null )
					{
						oldFocusedSection.LostFocus();
					}
					if( _focusedSection != null )
					{
						_focusedSection.GotFocus();
					}
				}
			}
		}

		object ISectionHost.Tag
		{
			get
			{
				return Tag;
			}
		}

		#endregion

		#region Mouse Handling

		protected override void OnMouseCaptureChanged( EventArgs e )
		{
			base.OnMouseCaptureChanged( e );
			if( _sectionWithMouseCapture != null )
			{
				_sectionWithMouseCapture.CancelDrag();
			}
		}

		protected override void OnMouseDown( MouseEventArgs e )
		{
			base.OnMouseDown( e );

			Section s = SectionFromPoint( new Point( e.X, e.Y ) );
			SectionMouseOver = s;
			if( s != null )
			{
				s.MouseDown( e );
				_sectionMouseButtonPressed = s;
			}
		}


		protected override void OnDoubleClick( EventArgs e )
		{
			Point pt = PointToClient( Cursor.Position );
			Section s = SectionFromPoint( pt );
			if( s != null )
			{
				if( !s.MouseDoubleClick( pt ) )
				{
					base.OnDoubleClick( e );
				}
			}
		}

		protected override void OnMouseClick( MouseEventArgs e )
		{
			base.OnMouseClick( e );

			Point mousePosition = new Point( e.X, e.Y );
			Section s = SectionFromPoint( mousePosition );

			if( s != null )
			{
				s.MouseClick( e );
			}
		}

		protected override void OnMouseUp( MouseEventArgs e )
		{
			base.OnMouseUp( e );

			if( _sectionMouseButtonPressed != null )
			{
				_sectionMouseButtonPressed.MouseUp( e );
				_sectionMouseButtonPressed = null;
			}
		}

		protected override void OnMouseMove( MouseEventArgs e )
		{
			base.OnMouseMove( e );
			Point mousePosition = new Point( e.X, e.Y );
			Section s = SectionFromPoint( mousePosition );
			SectionMouseOver = s;
			if( s != null )
			{
				s.MouseMove( mousePosition, e );
			}
		}

		protected override void OnMouseLeave( EventArgs e )
		{
			base.OnMouseLeave( e );
			SectionMouseOver = null;
		}

		#endregion

		#region Keyboard Handling

		protected override void OnKeyDown( KeyEventArgs e )
		{
			base.OnKeyDown( e );

			Section focusedSection = ((ISectionHost)this).FocusedSection;

			if( focusedSection != null )
			{
				focusedSection.KeyDown( e );
			}
		}

		#endregion

		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
			if( disposing )
			{
				if( _canvas != null )
				{
					_canvas.Dispose();
					_canvas = null;
				}

				StopLazyUpdateTimer();
			}
		}

		protected override void OnHandleCreated( EventArgs e )
		{
			base.OnHandleCreated( e );
			LayoutControl();
		}

		protected override void OnSizeChanged( EventArgs e )
		{
			base.OnSizeChanged( e );
			if( IsHandleCreated )
			{
				LayoutControl();
			}
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			//System.Diagnostics.Debug.WriteLine( string.Format( "Painting: W{0} CR{1}", this.ClientRectangle, e.ClipRectangle ) );
			Section.GraphicsSettings gs = new Section.GraphicsSettings( e.Graphics );
			_canvas.PaintBackground( gs, e.ClipRectangle );
			_canvas.Paint( gs, e.ClipRectangle );
		}

		private Section SectionMouseOver
		{
			get
			{
				return _sectionMouseOver;
			}
			set
			{
				if( _sectionMouseOver != value )
				{
					if( _sectionMouseOver != null )
					{
						_sectionMouseOver.MouseLeave();
					}

					_sectionMouseOver = value;

					if( _sectionMouseOver != null )
					{
						_sectionMouseOver.MouseEnter();
					}
				}
			}
		}

		private void StartLazyUpdateTimer()
		{
			if( IsHandleCreated )
			{
				if( _lazyUpdateTimer == null )
				{
					_lazyUpdateTimer = new Timer();
					_lazyUpdateTimer.Interval = 30;
					_lazyUpdateTimer.Tick += LazyUpdateTimerTick;
					_lazyUpdateTimer.Start();
				}
			}
		}

		protected void StopLazyUpdateTimer()
		{
			if( _lazyUpdateTimer != null )
			{
				_lazyUpdateTimer.Tick -= LazyUpdateTimerTick;
				_lazyUpdateTimer.Stop();
				_lazyUpdateTimer.Dispose();
				_lazyUpdateTimer = null;
			}
		}


		private void LazyUpdateTimerTick( object sender, EventArgs e )
		{
			((ISectionHost)this).ProcessLayoutsNow();
			StopLazyUpdateTimer();
		}

		private void InitializeComponent()
		{
			SuspendLayout();
			// 
			// SectionContainerControl
			// 
			AllowDrop = true;
			Name = "SectionContainerControl";

			ResumeLayout( false );
		}

		#region Drag Drop

		protected override void OnDragDrop( DragEventArgs e )
		{
			base.OnDragDrop( e );

			Point dropPoint = PointToClient( new Point( e.X, e.Y ) );
			Section s = null;
			if( _currentDropSection == null )
			{
				s = GetDroppableSection( dropPoint, e.Data );
			}
			else
			{
				s = _currentDropSection;
			}

			if( s != null )
			{
				if( _currentDropSection != null && _currentDropSection.IsAncestorOf(  s ) )
				{
					s = _currentDropSection;
				}
				s.Drop( e, dropPoint );
				OnDragDropEx( new SuperlistDragEventArgs( s, e ) );
			}
		}


		protected override void OnDragLeave( EventArgs e )
		{
			base.OnDragLeave( e );

			if( _currentDropSection != null )
			{
				_currentDropSection.DragLeave();
				_currentDropSection = null;
			}
		}


		protected virtual void OnDragOverEx( SuperlistDragEventArgs ea )
		{
			if( DragOverEx != null )
			{
				DragOverEx( this, ea );
			}
		}

		protected virtual void OnDragDropEx( SuperlistDragEventArgs ea )
		{
			if( DragDropEx != null )
			{
				DragDropEx( this, ea );
			}
		}


		protected override void OnDragOver( DragEventArgs e )
		{
			base.OnDragOver( e );

			Point pt = PointToClient( new Point( e.X, e.Y ) );
			Section s;

			SuperlistDragEventArgs ea = new SuperlistDragEventArgs( SectionFromPoint( pt ), e );
			OnDragOverEx( ea );

			if( ea.SectionAllowedToDropOn == null )
			{
				s = GetDroppableSection( pt, e.Data );
			}
			else
			{
				s = ea.SectionAllowedToDropOn;
			}

			if( _currentDropSection != null && s != _currentDropSection )
			{
				_currentDropSection.DragLeave();
				if( s != null )
				{
					for( Section parent = s.Parent; parent != null; parent = parent.Parent )
					{
						parent.DescendentDraggedOver( s );
					}
				}
			}
			_currentDropSection = s;

			if( s != null )
			{
				if( e.Effect == DragDropEffects.None )
				{
					e.Effect = e.AllowedEffect;
				}
				s.DraggingOver( pt, e.Data );
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		protected override void OnGiveFeedback( GiveFeedbackEventArgs gfbevent )
		{
			base.OnGiveFeedback( gfbevent );

			Point cursorPos = Cursor.Position;
			if( _imageWindow != null )
			{
				_imageWindow.Location = new Point( cursorPos.X + _imageWindowOffX, cursorPos.Y + _imageWindowOffY );
				if( !_imageWindow.Visible )
				{
					_imageWindow.Show();
				}
			}
		}

		protected override void OnQueryContinueDrag( QueryContinueDragEventArgs e )
		{
			base.OnQueryContinueDrag( e );

			_lastDragAction = e.Action;
		}

		private void FinishDropOperation()
		{
			if( _currentDropSection != null )
			{
				_currentDropSection.DragLeave();
				_currentDropSection = null;
			}

			if( _sectionMouseButtonPressed != null )
			{
				_sectionMouseButtonPressed.MouseUp( new MouseEventArgs( MouseButtons.Left, 1, 0, 0, 0 ) );
				_sectionMouseButtonPressed = null;
			}
			_draggingSections = null;
			Cursor = Cursors.Arrow;
			if( _imageWindow != null )
			{
				_imageWindow.Dispose();
				_imageWindow = null;
			}
			_currentDropSection = null;
		}


		private static ImageWindow CreateDragImageWindow( Section[] sections, bool applyDether )
		{
			int width = 0;
			foreach( Section s in sections )
			{
				if( s.Rectangle.Width > width )
				{
					width = s.Rectangle.Width;
				}
			}
			int height = sections[sections.Length - 1].Rectangle.Bottom - sections[0].Rectangle.Top;

			using( Bitmap bmpOfSections = new Bitmap( width, height ) )
			{
				using( Bitmap finalBmp = new Bitmap( bmpOfSections.Width, bmpOfSections.Height ) )
				{
					using( Graphics grfxFinalBmp = Graphics.FromImage( finalBmp ) )
					{
						int paintYPos = 0;
						foreach( Section s in sections )
						{
							using( Graphics grfxBmpOfSections = Graphics.FromImage( bmpOfSections ) )
							{
								Rectangle rcSection = s.HostBasedRectangle;
								//
								// Draw section into its bmp
								grfxBmpOfSections.TranslateTransform( -rcSection.X, -rcSection.Y + paintYPos );
								paintYPos += s.Rectangle.Height;

								grfxBmpOfSections.TextRenderingHint = TextRenderingHint.AntiAlias;

								Section.GraphicsSettings gs = new Section.GraphicsSettings( grfxBmpOfSections );
								s.PaintBackground( gs, rcSection );
								s.Paint( gs, rcSection );
							}
						}

						// to do, add dithering support

						//
						// Now draw a transparent verison of it onto the final bmp
						grfxFinalBmp.DrawImage( bmpOfSections,
																	 new Rectangle( 0, 0, bmpOfSections.Width, bmpOfSections.Height ),
																	 0,
																	 0,
																	 finalBmp.Width,
																	 finalBmp.Height,
																	 GraphicsUnit.Pixel );

						//
						// Make the cursor from our final bmp
						return new ImageWindow( Icon.FromHandle( finalBmp.GetHicon() ) );
					}
				}
			}
		}


		private Point CursorClientPosition
		{
			get
			{
				return PointToClient( Cursor.Position );
			}
		}

		#endregion

		private HeaderSection HeaderSection
		{
			get
			{
				return (HeaderSection)_canvas.Children[1];
			}
		}

		private Section GetDroppableSection( Point pt, IDataObject dataObject )
		{
			for( Section s = SectionFromPoint( pt ); s != null; s = s.Parent )
			{
				if( s.CanDrop( dataObject, Section.CanDropQueryContext.Section ) )
				{
					return s;
				}
			}
			return null;
		}


		private Section _focusedSection = null;
		private Section _sectionWithMouseCapture = null;
		private static DragAction _lastDragAction = DragAction.Cancel;
		private static Section[] _draggingSections = null;
		private ImageWindow _imageWindow = null;
		private int _imageWindowOffX = 1, _imageWindowOffY = 1;
		private static Section _currentDropSection = null;

		private Set<Section> _lazyLayouts = new Set<Section>();
		private Timer _lazyUpdateTimer = null;
		private Section _sectionMouseButtonPressed = null;
		private Section _sectionMouseOver = null;
		private SectionContainer _canvas = null;
		private SectionFactory _sectionFactory;

		#endregion
	}

	public delegate void SuperlistDragEventHandler( object sender, SuperlistDragEventArgs ea );
	public class SuperlistDragEventArgs : EventArgs
	{
		public SuperlistDragEventArgs( Section sectionOver, DragEventArgs dragEventArgs )
		{
			SectionOver = sectionOver;
			DragEventArgs = dragEventArgs;
		}

		/// <summary>
		/// Section the mouse is currently over.
		/// </summary>
		public readonly Section SectionOver;

		/// <summary>
		/// The standard DragOver event args.
		/// </summary>
		public readonly DragEventArgs DragEventArgs;

		/// <summary>
		/// Set this property with the section if any to allow
		/// dropping on. Used only in DragOver event.
		/// </summary>
		public Section SectionAllowedToDropOn;
	}
}
