/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System.Drawing;
using System.Windows.Forms;

namespace BinaryComponents.SuperList.Helper
{
    public static class TextRendererEx
    {
        //
        // Summary:
        //     Draws the specified text at the specified location using the specified device
        //     context, font, color, and formatting instructions.
        //
        // Parameters:
        //   foreColor:
        //     The System.Drawing.Color to apply to the drawn text.
        //
        //   font:
        //     The System.Drawing.Font to apply to the drawn text.
        //
        //   dc:
        //     The device context in which to draw the text.
        //
        //   pt:
        //     The System.Drawing.Point that represents the upper-left corner of the drawn
        //     text.
        //
        //   flags:
        //     A bitwise combination of the System.Drawing.GDI.TextFormatFlags values.
        //
        //   text:
        //     The text to draw.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     dc is null.
        public static void DrawText(IDeviceContext dc, string text, Font font, Rectangle rc, Color foreColor, TextFormatFlags flags)
        {
            Graphics grfx = dc as Graphics;

            if (grfx != null)
            {
                rc.X += (int) grfx.Transform.OffsetX;
                rc.Y += (int) grfx.Transform.OffsetY;
            }
            TextRenderer.DrawText(dc, text, font, rc, foreColor, flags);
        }
    }
}