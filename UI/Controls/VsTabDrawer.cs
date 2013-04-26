using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GrayIris.Utilities.UI.Controls
{
	/// <summary>
	/// Provides drawing capabilities that mimic the Visual
	/// Studio tabs.
	/// </summary>
	public class VsTabDrawer : YaTabDrawer
	{
		/// <summary>
		/// Creates an instance of the <see cref="VsTabDrawer"/> class.
		/// </summary>
		public VsTabDrawer()
		{
			pens = new Hashtable( 3 );
			brushes = new Hashtable( 2 );
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
			if( !brushes.ContainsKey( foreColor ) )
			{
				brushes[ foreColor ] = new SolidBrush( foreColor );
			}
			if( !brushes.ContainsKey( backColor ) )
			{
				brushes[ backColor ] = new SolidBrush( backColor );
			}
			if( !pens.ContainsKey( highlightColor ) )
			{
				pens[ highlightColor ] = new Pen( highlightColor );
			}
			if( !pens.ContainsKey( shadowColor ) )
			{
				pens[ shadowColor ] = new Pen( shadowColor );
			}
			if( !pens.ContainsKey( foreColor ) )
			{
				pens[ foreColor ] = new Pen( foreColor );
			}
			Brush fb = ( Brush ) brushes[ foreColor ];
			Brush bb = ( Brush ) brushes[ backColor ];
			Pen h = ( Pen ) pens[ highlightColor ];
			Pen s = ( Pen ) pens[ shadowColor ];
			Pen f = ( Pen ) pens[ foreColor ];
			if( active )
			{
				graphics.FillRectangle( fb, 0, 0, tabSize.Width, tabSize.Height + 1 );
				switch( dock )
				{
					case DockStyle.Bottom:
						graphics.DrawLine( h, tabSize.Width, 0, tabSize.Width, tabSize.Height );
						graphics.DrawLine( s, 0, 0, 0, tabSize.Height );
						graphics.DrawLine( s, 0, 0, tabSize.Width, 0 );
						break;
					case DockStyle.Top:
						graphics.DrawLine( h, 0, 0, tabSize.Width, 0 );
						graphics.DrawLine( h, 0, 0, 0, tabSize.Height );
						graphics.DrawLine( s, tabSize.Width, 0, tabSize.Width, tabSize.Height );
						break;
					case DockStyle.Left:
						graphics.DrawLine( h, 0, 0, tabSize.Width, 0 );
						graphics.DrawLine( s, 0, 0, 0, tabSize.Height );
						graphics.DrawLine( h, tabSize.Width, 0, tabSize.Width, tabSize.Height );
						break;
					case DockStyle.Right:
						graphics.DrawLine( s, 0, 0, tabSize.Width, 0 );
						graphics.DrawLine( h, 0, 0, 0, tabSize.Height );
						graphics.DrawLine( s, tabSize.Width, 0, tabSize.Width, tabSize.Height );
						break;
				}
			}
			else
			{
				graphics.DrawLine( f, tabSize.Width, 1, tabSize.Width, tabSize.Height - 1 );
			}
		}

		/// <summary>
		/// Inherited from <see cref="YaTabDrawer"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="VsTabDrawer"/> uses highlights. Hence, this
		/// method always returns <b>true</b>.
		/// </returns>
		public override bool UsesHighlghts
		{
			get
			{
				return true;
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
		/// Inherited from <see cref="YaTabDrawer"/>.
		/// </summary>
		/// <param name="dock">See <see cref="YaTabDrawer.SupportsTabDockStyle(DockStyle)"/>.</param>
		/// <returns>
		/// Returns <b>true</b> for all directional <see cref="DockStyle"/>s.
		/// </returns>
		public override bool SupportsTabDockStyle( DockStyle dock )
		{
			return ( dock != DockStyle.Fill && dock != DockStyle.None );
		}

		#endregion

		/// <summary>
		/// Contains pens that have been used in drawing.
		/// </summary>
		private Hashtable pens;

		/// <summary>
		/// Contains brushes that have been used in drawing.
		/// </summary>
		private Hashtable brushes;
	}
}
