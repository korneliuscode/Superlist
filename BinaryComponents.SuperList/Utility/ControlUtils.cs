/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using BinaryComponents.Utility.Win32;
using System.Windows.Forms;
using System.Drawing;

namespace BinaryComponents.WinFormsUtility.Controls
{
    public static class ControlUtils
    {
        /// <summary>
        /// Returns true if any part of the client based rectangle is visible.
        /// </summary>
        /// <param name="control">Control to check.</param>
        /// <param name="rectangleToCheck">Client based rectangle to check.</param>
        public static bool IsClientRectangleVisible(Control control, Rectangle rectangleToCheck)
        {
            if (!control.IsHandleCreated)
            {
                return false;
            }

            Utility.Win32.Common.RECT rcClip, rcClient = new Utility.Win32.Common.RECT(rectangleToCheck);

            using (Graphics grfx = control.CreateGraphics())
            {
                IntPtr hdc = IntPtr.Zero;

                try
                {
                    hdc = grfx.GetHdc();

                    RegionValue result = (RegionValue) Gdi.GetClipBox(hdc, out rcClip);

                    return result != RegionValue.NULLREGION;
                }
                finally
                {
                    if (hdc != IntPtr.Zero)
                    {
                        grfx.ReleaseHdc(hdc);
                    }
                }
            }
        }
    }
}