using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GrayIris.Utilities.UI.Controls
{
    /// <summary>
    /// The <see cref="XlTabDrawer"/> draws tabs similar to those
    /// found in Microsoft Excel.
    /// </summary>
    public class XlTabDrawer : YaTabDrawerBase
    {
        /// <summary>
        /// Creates a new instance of the <see cref="XlTabDrawer"/> class.
        /// </summary>
        public XlTabDrawer()
        {
            tabPolygon = new PointF[4];
            for (int i = 0; i < 4; i++)
            {
                tabPolygon[i] = new PointF(0.0f, 0.0f);
            }
            tabPolygon[0].X = -4.0f;
            tabPolygon[1].X = 4.0f;
        }

        #region YaTabDrawer Members

        /// <summary>
        /// Inherited from <see cref="YaTabDrawerBase"/>.
        /// </summary>
        /// <param name="foreColor">See <see cref="YaTabDrawerBase.DrawTab(Color,Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="backColor">See <see cref="YaTabDrawerBase.DrawTab(Color,Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="highlightColor">See <see cref="YaTabDrawerBase.DrawTab(Color,Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="shadowColor">See <see cref="YaTabDrawerBase.DrawTab(Color,Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="borderColor">See <see cref="YaTabDrawerBase.DrawTab(Color,Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="hoverColor">See <see cref="YaTabDrawerBase.DrawTab(Color,Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="active">See <see cref="YaTabDrawerBase.DrawTab(Color,Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="mouseOver">See <see cref="YaTabDrawerBase.DrawTab(Color,Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="dock">See <see cref="YaTabDrawerBase.DrawTab(Color,Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="graphics">See <see cref="YaTabDrawerBase.DrawTab(Color,Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="tabSize">See <see cref="YaTabDrawerBase.DrawTab(Color,Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        public override void DrawTab(Color foreColor, Color backColor, Color highlightColor, Color shadowColor, Color borderColor, Color hoverColor, bool active, bool mouseOver, DockStyle dock, Graphics graphics, SizeF tabSize, bool isNewTab)
        {
            tabPolygon[0].Y = tabSize.Height;
            tabPolygon[2].X = tabSize.Width - 4.0f;
            tabPolygon[3].X = tabSize.Width + 4.0f;
            tabPolygon[3].Y = tabSize.Height;
            Brush b = null;
            if (active)
            {
                b = new SolidBrush(foreColor);
            }
            else
            {
                b = new SolidBrush(backColor);
            }
            if (mouseOver)
            {
                b.Dispose();
                b = new SolidBrush(hoverColor);
            }
            graphics.FillPolygon(b, tabPolygon);
            b.Dispose();
            Pen p = new Pen(borderColor);
            graphics.DrawPolygon(p, tabPolygon);
            if (active)
            {
                p.Color = foreColor;
                graphics.DrawLine(p, tabPolygon[0], tabPolygon[3]);
            }

            if (!isNewTab)   //close button
            {
                p.Color = FromHex("#cccccc");
                var padding = 3;
                var bottom = tabSize.Height - 6;
                var diameter = bottom - padding;
                var x = tabSize.Width - diameter - padding - 2;
                var y = padding;
                var width = diameter;
                var height = diameter;
                var rect = new RectangleF(x, y, width, height);
                //var fill = new SolidBrush(FromHex("#cccccc"));
                //graphics.FillEllipse(fill, x, y, width, height);
                graphics.DrawImage(Resources.x, rect);

            }
            p.Dispose();
        }

        private Color FromHex(string hex)
        {
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            if (hex.Length != 6) throw new Exception("Color not valid");

            return Color.FromArgb(
                int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
        }


        /// <summary>
        /// Inherited from <see cref="YaTabDrawerBase"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="XlTabDrawer"/> uses highlights. Hence, this
        /// method always returns <b>true</b>.
        /// </returns>
        public override bool UsesHighlghts
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Inherited from <see cref="YaTabDrawerBase"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="VsTabDrawer"/> supports all directional
        /// <see cref="DockStyle"/>s.
        /// </returns>
        public override DockStyle[] SupportedTabDockStyles
        {
            get
            {
                return new DockStyle[] { DockStyle.Bottom, DockStyle.Top, DockStyle.Left, DockStyle.Right };
            }
        }

        /// <summary>
        /// Returns the <see cref="DockStyle"/>s 
        /// </summary>
        public override bool SupportsTabDockStyle(DockStyle dock)
        {
            return (dock != DockStyle.Fill && dock != DockStyle.None);
        }


        #endregion

        /// <summary>
        /// Contains the polyogn's points to draw the trapezoidal tab shape.
        /// </summary>
        private PointF[] tabPolygon;
    }
}
