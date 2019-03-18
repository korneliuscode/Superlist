/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Windows.Forms;
using BinaryComponents.Utility.Win32;

namespace BinaryComponents.SuperList.Helper
{
    public partial class ImageWindow : Form
    {
        public ImageWindow(Icon iconToDraw)
        {
            if (iconToDraw == null)
            {
                throw new ArgumentNullException("iconToDraw");
            }

            Visible = false;
            WindowState = FormWindowState.Normal;

            InitializeComponent();
            TransparencyKey = BackColor;

            _iconToDraw = iconToDraw;
            Size = new Size(_iconToDraw.Width, _iconToDraw.Height);
        }


        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (_iconToDraw != null)
            {
                Size = new Size(_iconToDraw.Width, _iconToDraw.Height);
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                //
                //	We call SetWindowPos here rather than just setting the TopMost property
                //	as the property doesn't take into account Form.ShowWithoutActivation.
                IntPtr HWND_TOPMOST = new IntPtr(-1);

                User.SetWindowPos
                    (Handle, HWND_TOPMOST, 0, 0, 0, 0
                     , SetWindowPosOptions.SWP_NOSIZE | SetWindowPosOptions.SWP_NOMOVE
                       | SetWindowPosOptions.SWP_NOREDRAW | SetWindowPosOptions.SWP_NOACTIVATE);
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch ((Messages) m.Msg)
            {
                case Messages.WM_NCHITTEST:
                    {
                        m.Result = new IntPtr((int) NCHITTEST.HTTRANSPARENT);
                    }
                    return;
            }
            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_iconToDraw != null)
            {
                e.Graphics.DrawIcon(_iconToDraw, ClientRectangle);
            }
        }

        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }

        private Icon _iconToDraw = null;
    }
}