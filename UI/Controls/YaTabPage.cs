using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GrayIris.Utilities.UI.Controls
{
	/// <summary>
	/// Summary description for YaTabPage.
	/// </summary>
    [ToolboxItem(false)]
	[Designer( typeof( GrayIris.Utilities.UI.Controls.Design.YaTabPageDesigner ) )]
	public class YaTabPage : ContainerControl
	{
		/// <summary>
		/// Creates an instance of the <see cref="YaTabPage"/> class.
		/// </summary>
		public YaTabPage()
		{
			base.Dock = DockStyle.Fill;
			imgIndex = -1;
		}

		/// <summary>
		/// Gets or sets the index to the image displayed on this tab.
		/// </summary>
		/// <value>
		/// The zero-based index to the image in the <see cref="TabControl.ImageList"/>
		/// that appears on the tab. The default is -1, which signifies no image.
		/// </value>
		/// <exception cref="ArgumentException">
		/// The <see cref="ImageIndex"/> value is less than -1.
		/// </exception>
		public int ImageIndex
		{
			get
			{
				return imgIndex;
			}
			set
			{
				imgIndex = value;
			}
		}

		/// <summary>
		/// Overridden from <see cref="Panel"/>.
		/// </summary>
		/// <remarks>
		/// Since the <see cref="YaTabPage"/> exists only
		/// in the context of a <see cref="YaTabControl"/>,
		/// it makes sense to always have it fill the
		/// <see cref="YaTabControl"/>. Hence, this property
		/// will always return <see cref="DockStyle.Fill"/>
		/// regardless of how it is set.
		/// </remarks>
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = DockStyle.Fill;
			}
		}

		/// <summary>
		/// Only here so that it shows up in the property panel.
		/// </summary>
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>
		/// Overriden from <see cref="Control"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="YaTabPage.ControlCollection"/>.
		/// </returns>
		protected override System.Windows.Forms.Control.ControlCollection CreateControlsInstance()
		{
			return new YaTabPage.ControlCollection( this );
		}

		/// <summary>
		/// The index of the image to use for the tab that represents this
		/// <see cref="YaTabPage"/>.
		/// </summary>
		private int imgIndex;

		/// <summary>
		/// A <see cref="YaTabPage"/>-specific
		/// <see cref="Control.ControlCollection"/>.
		/// </summary>
		public new class ControlCollection : Control.ControlCollection
		{
			/// <summary>
			/// Creates a new instance of the
			/// <see cref="YaTabPage.ControlCollection"/> class with 
			/// the specified <i>owner</i>.
			/// </summary>
			/// <param name="owner">
			/// The <see cref="YaTabPage"/> that owns this collection.
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// Thrown if <i>owner</i> is <b>null</b>.
			/// </exception>
			/// <exception cref="ArgumentException">
			/// Thrown if <i>owner</i> is not a <see cref="YaTabPage"/>.
			/// </exception>
			public ControlCollection( Control owner ) : base( owner )
			{
				if( owner == null )
				{
					throw new ArgumentNullException( "owner", "Tried to create a YaTabPage.ControlCollection with a null owner." );
				}
				YaTabPage c = owner as YaTabPage;
				if( c == null )
				{
					throw new ArgumentException( "Tried to create a YaTabPage.ControlCollection with a non-YaTabPage owner.", "owner" );
				}
			}

			/// <summary>
			/// Overridden. Adds a <see cref="Control"/> to the
			/// <see cref="YaTabPage"/>.
			/// </summary>
			/// <param name="value">
			/// The <see cref="Control"/> to add, which must not be a
			/// <see cref="YaTabPage"/>.
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// Thrown if <i>value</i> is <b>null</b>.
			/// </exception>
			/// <exception cref="ArgumentException">
			/// Thrown if <i>value</i> is a <see cref="YaTabPage"/>.
			/// </exception>
			public override void Add( Control value )
			{
				if( value == null )
				{
					throw new ArgumentNullException( "value", "Tried to add a null value to the YaTabPage.ControlCollection." );
				}
				YaTabPage p = value as YaTabPage;
				if( p != null )
				{
					throw new ArgumentException( "Tried to add a YaTabPage control to the YaTabPage.ControlCollection.", "value" );
				}
				base.Add( value );
			}
		}
	}
}
