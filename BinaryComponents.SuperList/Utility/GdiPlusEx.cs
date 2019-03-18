/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using BinaryComponents.Utility.Win32;
using BinaryComponents.Utility.Win32.Common;

namespace BinaryComponents.WinFormsUtility.Drawing
{
    public static class GdiPlusEx
    {
        /// <summary>
        /// Alignment of the column text within the column header.
        /// </summary>
        public enum Alignment
        {
            Left,
            Center,
            Right
        }

        public enum VAlignment
        {
            Top,
            Center,
            Bottom
        }

        public enum TextSplitting
        {
            SingleLineEllipsis,
            MultiLine
        }

        public enum Ampersands
        {
            Display,
            MakeShortcut
        }

        public static void DrawString(Graphics g, string text, Font font, Color color, Rectangle rect, TextSplitting textSplitting, Ampersands ampersands)
        {
            DrawString(g, text, font, color, rect, Alignment.Left, VAlignment.Top, textSplitting, ampersands);
        }

        public static void DrawString(Graphics g, string text, Font font, Color color, Rectangle rect, Alignment alignment, VAlignment valignment, TextSplitting textSplitting, Ampersands ampersands)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            if (font == null)
            {
                throw new ArgumentNullException("font");
            }

            if (ampersands == Ampersands.Display)
            {
                text = text.Replace("&", "&&");
            }

            float[] txValues = g.Transform.Elements;
            IntPtr hClipRgn = g.Clip.GetHrgn(g);
            IntPtr hDC = g.GetHdc();

            Gdi.SelectClipRgn(hDC, hClipRgn);

            int oldGraphicsMode = Gdi.SetGraphicsMode(hDC, 2);
            XFORM oldXForm = new XFORM();

            Gdi.GetWorldTransform(hDC, ref oldXForm);

            XFORM newXForm = new XFORM();

            newXForm.eM11 = txValues[0];
            newXForm.eM12 = txValues[1];
            newXForm.eM21 = txValues[2];
            newXForm.eM22 = txValues[3];
            newXForm.eDx = txValues[4];
            newXForm.eDy = txValues[5];

            Gdi.SetWorldTransform(hDC, ref newXForm);

            try
            {
                IntPtr hFont = font.ToHfont();
                IntPtr hOldFont = Gdi.SelectObject(hDC, hFont);

                try
                {
                    RECT r = new RECT(rect);
                    User.DrawTextFlags uFormat;

                    switch (textSplitting)
                    {
                        case TextSplitting.SingleLineEllipsis:
                            uFormat
                                = User.DrawTextFlags.DT_WORD_ELLIPSIS
                                  | User.DrawTextFlags.DT_END_ELLIPSIS;
                            break;
                        case TextSplitting.MultiLine:
                            uFormat
                                = User.DrawTextFlags.DT_WORDBREAK;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }

                    switch (alignment)
                    {
                        case Alignment.Left:
                            break;
                        case Alignment.Center:
                            uFormat
                                = User.DrawTextFlags.DT_CENTER;
                            break;
                        case Alignment.Right:
                            uFormat
                                = User.DrawTextFlags.DT_RIGHT;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    switch (valignment)
                    {
                        case VAlignment.Top:
                            break;
                        case VAlignment.Bottom:
                            uFormat |= User.DrawTextFlags.DT_BOTTOM | User.DrawTextFlags.DT_SINGLELINE;
                            break;
                        case VAlignment.Center:
                            uFormat |= User.DrawTextFlags.DT_VCENTER | User.DrawTextFlags.DT_SINGLELINE;
                            break;
                    }

                    uint bgr = (uint) ((color.B << 16) | (color.G << 8) | (color.R));
                    uint oldColor = Gdi.SetTextColor(hDC, bgr);

                    try
                    {
                        BackgroundMode oldBackgroundMode = Gdi.SetBkMode(hDC, BackgroundMode.TRANSPARENT);

                        try
                        {
                            User.DrawText(hDC, text, text.Length, ref r, uFormat);
                        }
                        finally
                        {
                            Gdi.SetBkMode(hDC, oldBackgroundMode);
                        }
                    }
                    finally
                    {
                        Gdi.SetTextColor(hDC, oldColor);
                    }
                }
                finally
                {
                    Gdi.SelectObject(hDC, hOldFont);
                    Gdi.DeleteObject(hFont);
                }
            }
            finally
            {
                if (oldGraphicsMode == 1)
                {
                    oldXForm.eM11 = 1;
                    oldXForm.eM12 = 0;
                    oldXForm.eM21 = 0;
                    oldXForm.eM22 = 1;
                    oldXForm.eDx = 0;
                    oldXForm.eDx = 0;
                }

                Gdi.SetWorldTransform(hDC, ref oldXForm);
                Gdi.SetGraphicsMode(hDC, oldGraphicsMode);

                g.ReleaseHdc(hDC);

                if (hClipRgn != IntPtr.Zero)
                {
                    g.Clip.ReleaseHrgn(hClipRgn);
                }
            }
        }

        public static Size MeasureString(Graphics g, string text, Font font, int width)
        {
            Size size;
            TextDetails td = new TextDetails(text, font, width);

            if (_mapTextSizes.TryGetValue(td, out size))
            {
                return size;
            }

            IntPtr hDC = g.GetHdc();

            try
            {
                IntPtr hFont = font.ToHfont();

                try
                {
                    IntPtr hOldFont = Gdi.SelectObject(hDC, hFont);

                    try
                    {
                        Rectangle rect = new Rectangle(0, 0, width, 0);
                        RECT r = new RECT(rect);
                        User.DrawTextFlags uFormat = User.DrawTextFlags.DT_WORDBREAK | User.DrawTextFlags.DT_CALCRECT;

                        User.DrawText(hDC, text, text.Length, ref r, uFormat);

                        size = new Size(r.Right, r.Bottom);

                        _mapTextSizes[td] = size;

                        return size;
                    }
                    finally
                    {
                        Gdi.SelectObject(hDC, hOldFont);
                    }
                }
                finally
                {
                    Gdi.DeleteObject(hFont);
                }
            }
            finally
            {
                g.ReleaseHdc(hDC);
            }
        }

        public static void DrawRoundRect(Graphics g, Pen p, Rectangle rect, int radius)
        {
            DrawRoundRect(g, p, rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        public static void DrawRoundRect(Graphics g, Pen p, int x, int y, int width, int height, int radius)
        {
            if (width <= 0 || height <= 0)
            {
                return;
            }

            radius = Math.Min(radius, height/2 - 1);
            radius = Math.Min(radius, width/2 - 1);

            if (radius <= 0)
            {
                g.DrawRectangle(p, x, y, width, height);
                return;
            }

            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddLine(x + radius, y, x + width - (radius*2), y);
                gp.AddArc(x + width - (radius*2), y, radius*2, radius*2, 270, 90);
                gp.AddLine(x + width, y + radius, x + width, y + height - (radius*2));
                gp.AddArc(x + width - (radius*2), y + height - (radius*2), radius*2, radius*2, 0, 90);
                gp.AddLine(x + width - (radius*2), y + height, x + radius, y + height);
                gp.AddArc(x, y + height - (radius*2), radius*2, radius*2, 90, 90);
                gp.AddLine(x, y + height - (radius*2), x, y + radius);
                gp.AddArc(x, y, radius*2, radius*2, 180, 90);
                gp.CloseFigure();

                g.DrawPath(p, gp);
            }
        }

        public static void FillRoundRect(Graphics g, Brush b, Rectangle rect, int radius)
        {
            FillRoundRect(g, b, rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        public static void FillRoundRect(Graphics g, Brush b, int x, int y, int width, int height, int radius)
        {
            if (width <= 0 || height <= 0)
            {
                return;
            }

            radius = Math.Min(radius, height/2);
            radius = Math.Min(radius, width/2);

            if (radius == 0)
            {
                g.FillRectangle(b, x, y, width, height);
                return;
            }

            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddLine(x + radius, y, x + width - (radius*2), y);
                gp.AddArc(x + width - (radius*2), y, radius*2, radius*2, 270, 90);
                gp.AddLine(x + width, y + radius, x + width, y + height - (radius*2));
                gp.AddArc(x + width - (radius*2), y + height - (radius*2), radius*2, radius*2, 0, 90);
                gp.AddLine(x + width - (radius*2), y + height, x + radius, y + height);
                gp.AddArc(x, y + height - (radius*2), radius*2, radius*2, 90, 90);
                gp.AddLine(x, y + height - (radius*2), x, y + radius);
                gp.AddArc(x, y, radius*2, radius*2, 180, 90);
                gp.CloseFigure();

                g.FillPath(b, gp);
            }
        }

        public static IDisposable SaveState(Graphics g)
        {
            return new GraphicsStateDisposer(g);
        }

        #region GraphicsStateDisposer

        private sealed class GraphicsStateDisposer : IDisposable
        {
            internal GraphicsStateDisposer(Graphics g)
            {
                _g = g;
                _state = _g.Save();
            }

            #region IDisposable Members

            public void Dispose()
            {
                if (_g != null)
                {
                    _g.Restore(_state);
                    _g = null;
                    _state = null;
                }
            }

            #endregion

            private Graphics _g;
            private GraphicsState _state;
        }

        #endregion

        #region TextDetails

        private sealed class TextDetails
        {
            internal TextDetails(string text, Font font, int width)
            {
                _text = text;
                _font = font;
                _width = width;
            }

            public override int GetHashCode()
            {
                return _text.GetHashCode() ^ _font.GetHashCode() ^ _width;
            }

            public override bool Equals(object obj)
            {
                TextDetails td = obj as TextDetails;

                if (td == null)
                {
                    return false;
                }

                return _text == td._text && _font.Equals(td._font) && _width == td._width;
            }

            private string _text;
            private Font _font;
            private int _width;
        }

        #endregion

        private static Dictionary<TextDetails, Size> _mapTextSizes = new Dictionary<TextDetails, Size>();
    }
}