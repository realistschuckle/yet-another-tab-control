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
	public class XlTabDrawer : YaTabDrawer
	{
		/// <summary>
		/// Creates a new instance of the <see cref="XlTabDrawer"/> class.
		/// </summary>
		public XlTabDrawer()
		{
			tabPolygon = new PointF[ 4 ];
			for( int i = 0; i < 4; i++ )
			{
				tabPolygon[ i ] = new PointF( 0.0f, 0.0f );
			}
			tabPolygon[ 0 ].X = -4.0f;
			tabPolygon[ 1 ].X = 4.0f;
		}

		#region YaTabDrawer Members

        /// <summary>
        /// Inherited from <see cref="YaTabDrawer"/>.
        /// </summary>
        /// <param name="foreColor">See <see cref="YaTabDrawer.DrawTab(Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="backColor">See <see cref="YaTabDrawer.DrawTab(Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="highlightColor">See <see cref="YaTabDrawer.DrawTab(Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="shadowColor">See <see cref="YaTabDrawer.DrawTab(Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="borderColor">See <see cref="YaTabDrawer.DrawTab(Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="active">See <see cref="YaTabDrawer.DrawTab(Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="mouseOver">See <see cref="YaTabDrawer.DrawTab(Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="dock">See <see cref="YaTabDrawer.DrawTab(Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="graphics">See <see cref="YaTabDrawer.DrawTab(Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        /// <param name="tabSize">See <see cref="YaTabDrawer.DrawTab(Color,Color,Color,Color,Color,bool,DockStyle,Graphics,SizeF)"/>.</param>
        public override void DrawTab(Color foreColor, Color backColor, Color highlightColor, Color shadowColor, Color borderColor, bool active, bool mouseOver, DockStyle dock, Graphics graphics, SizeF tabSize)
        {
			tabPolygon[ 0 ].Y = tabSize.Height;
			tabPolygon[ 2 ].X = tabSize.Width - 4.0f;
			tabPolygon[ 3 ].X = tabSize.Width + 4.0f;
			tabPolygon[ 3 ].Y = tabSize.Height;
			Brush b = null;
			if( active )
			{
				b = new SolidBrush( foreColor );
			}
			else
			{
				b = new SolidBrush( backColor );
			}
            if (mouseOver)
            {
                b.Dispose();
                b = new SolidBrush(Color.Orange);
            }
			graphics.FillPolygon( b, tabPolygon );
			b.Dispose();
			Pen p = new Pen( borderColor );
			graphics.DrawPolygon( p, tabPolygon );
			if( active )
			{
				p.Color = foreColor;
				graphics.DrawLine( p, tabPolygon[ 0 ], tabPolygon[ 3 ] );
			}
			p.Dispose();
		}

		/// <summary>
		/// Inherited from <see cref="YaTabDrawer"/>.
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
		/// Inherited from <see cref="YaTabDrawer"/>.
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
			return ( dock != DockStyle.Fill && dock != DockStyle.None );
		}

		#endregion

		/// <summary>
		/// Contains the polyogn's points to draw the trapezoidal tab shape.
		/// </summary>
		private PointF[] tabPolygon;
	}
}
