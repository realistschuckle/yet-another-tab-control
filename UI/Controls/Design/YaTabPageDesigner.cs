using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GrayIris.Utilities.UI.Controls.Design
{
	/// <summary>
	/// Summary description for YaTabPageDesigner.
	/// </summary>
	public class YaTabPageDesigner : ScrollableControlDesigner
	{
		/// <summary>
		/// Creates a new instance of the <see cref="YaTabPageDesigner"/> class.
		/// </summary>
		public YaTabPageDesigner() {}

		/// <summary>
		/// Shadows the <see cref="YaTabPage.Text"/> property.
		/// </summary>
		public string Text
		{
			get
			{
				return ytp.Text;
			}
			set
			{
				string ot = ytp.Text;
				ytp.Text = value;
				IComponentChangeService iccs = GetService( typeof( IComponentChangeService ) ) as IComponentChangeService;
				if( iccs != null )
				{
					YaTabControl ytc = ytp.Parent as YaTabControl;
					if( ytc != null )
					{
						ytc.SelectedIndex = ytc.SelectedIndex;
					}
				}
			}
		}

		/// <summary>
		/// Overridden. Inherited from
		/// <see cref="ControlDesigner.OnPaintAdornments(PaintEventArgs)"/>.
		/// </summary>
		/// <param name="pea">
		/// Some <see cref="PaintEventArgs"/>.
		/// </param>
		protected override void OnPaintAdornments( PaintEventArgs pea )
		{
			base.OnPaintAdornments( pea );

			// My thanks to bschurter (Bruce), CodeProject member #1255339 for this!
			using( Pen p = new Pen( SystemColors.ControlDark, 1 ) )
			{
				p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
				pea.Graphics.DrawRectangle( p, 0, 0, ytp.Width - 1, ytp.Height - 1 );
			}
		}

		/// <summary>
		/// Overridden. Inherited from <see cref="ControlDesigner.Initialize( IComponent )"/>.
		/// </summary>
		/// <param name="component">
		/// The <see cref="IComponent"/> hosted by the designer.
		/// </param>
		public override void Initialize( IComponent component )
		{
			ytp = component as YaTabPage;
			if( ytp == null )
			{
				DisplayError( new Exception( "You attempted to use a YaTabPageDesigner with a class that does not inherit from YaTabPage." ) );
			}
			base.Initialize( component );
		}

		/// <summary>
		/// Overridden. Inherited from <see cref="ControlDesigner.PreFilterProperties(IDictionary)"/>.
		/// </summary>
		/// <param name="properties"></param>
		protected override void PreFilterProperties( IDictionary properties )
		{
			base.PreFilterProperties( properties );
			properties[ "Text" ] = TypeDescriptor.CreateProperty( typeof( YaTabPageDesigner ), ( PropertyDescriptor ) properties[ "Text" ], new Attribute[ 0 ] );
		}

		/// <summary>
		/// The <see cref="YaTabPage"/> hosted by the designer.
		/// </summary>
		private YaTabPage ytp;
	}
}
