/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BinaryComponents.Utility.Win32.Common
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public RECT(Rectangle rect)
        {
            Left = rect.Left;
            Top = rect.Top;
            Right = rect.Right;
            Bottom = rect.Bottom;
        }

        public Rectangle Rect
        {
            get
            {
                return new Rectangle(Left, Top, Right - Left, Bottom - Top);
            }
        }

        public Point Location
        {
            get
            {
                return new Point(Left, Top);
            }
        }

        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public POINT(Int32 x, Int32 y)
        {
            X = x;
            Y = y;
        }

        public Int32 X;
        public Int32 Y;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public SIZE(Int32 cx, Int32 cy)
        {
            CX = cx;
            CY = cy;
        }

        public Int32 CX;
        public Int32 CY;
    }
}