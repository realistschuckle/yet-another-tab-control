using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GrayIris.Utilities.UI.Controls.Design
{
	/// <summary>
	/// Provides a custom <see cref="ControlDesigner"/> for the
	/// <see cref="YaTabControl"/>.
	/// </summary>
	public class YaTabControlDesigner : ControlDesigner
	{
		/// <summary>
		/// Creates an instance of the <see cref="YaTabControlDesigner"/> class.
		/// </summary>
		public YaTabControlDesigner() {}

		/// <summary>
		/// Overridden. Inherited from <see cref="ControlDesigner"/>.
		/// </summary>
		/// <param name="component">
		/// The <see cref="IComponent"/> to which this designer gets attached.
		/// </param>
		/// <remarks>
		/// This designer exists exclusively for <see cref="YaTabControl"/>s. If
		/// <i>component</i> does not inherit from <see cref="YaTabControl"/>,
		/// then this method throws an <see cref="ArgumentException"/>.
		/// </remarks>
		/// <exception cref="ArgumentException">
		/// Thrown if this designer gets used with a class that does not
		/// inherit from <see cref="YaTabControl"/>.
		/// </exception>
		public override void Initialize( IComponent component )
		{
			ytc = component as YaTabControl;
			if( ytc == null )
			{
				this.DisplayError( new ArgumentException( "Tried to use the YaTabControlDesigner with a class that does not inherit from YaTabControl.", "component" ) );
				return;
			}
			base.Initialize( component );
			IComponentChangeService iccs = ( IComponentChangeService ) this.GetService( typeof( IComponentChangeService ) );
			if( iccs != null )
			{
				iccs.ComponentRemoved += new ComponentEventHandler( ComponentRemoved );
			}
		}

		/// <summary>
		/// Overridden. Inherited from <see cref="ControlDesigner"/>.
		/// </summary>
		public override DesignerVerbCollection Verbs
		{
			get
			{
				if( verbs == null )
				{
					verbs = new DesignerVerbCollection();
					verbs.Add( new DesignerVerb( "Add Tab", new EventHandler( AddTab ) ) );
					verbs.Add( new DesignerVerb( "Remove Tab", new EventHandler( RemoveTab ) ) );
				}
				return verbs;
			}
		}

		/// <summary>
		/// Overridden. Inherited from <see cref="ControlDesigner"/>.
		/// </summary>
		/// <param name="m">
		/// The message.
		/// </param>
		/// <remarks>
		/// Checks for WM_LBUTTONDOWN events and uses that to
		/// select the appropriate tab in the <see cref="YaTabControl"/>.
		/// </remarks>
		protected override void WndProc( ref Message m )
		{
			try
			{
				int x = 0;
				int y = 0;
				if( ytc.Created && m.HWnd == ytc.Handle )
				{
					switch( m.Msg )
					{
						case WM_LBUTTONDOWN:
							x = ( m.LParam.ToInt32() << 16 ) >> 16;
							y = m.LParam.ToInt32() >> 16;
							int oi = ytc.SelectedIndex;
							YaTabPage ot = ytc.SelectedTab;
							if( ytc.ScrollButtonStyle == YaScrollButtonStyle.Always && ytc.GetLeftScrollButtonRect().Contains( x, y ) )
							{
								ytc.ScrollTabs( -10 );
							}
							else if( ytc.ScrollButtonStyle == YaScrollButtonStyle.Always && ytc.GetRightScrollButtonRect().Contains( x, y ) )
							{
								ytc.ScrollTabs( 10 );
							}
							else
							{
								for( int i = 0; i < ytc.Controls.Count; i++ )
								{
									Rectangle r = ytc.GetTabRect( i );
									if( r.Contains( x, y ) )
									{
										ytc.SelectedIndex = i;
										RaiseComponentChanging( TypeDescriptor.GetProperties( Control )[ "SelectedIndex" ] );
										RaiseComponentChanged( TypeDescriptor.GetProperties( Control )[ "SelectedIndex" ], oi, i );
										break;
									}
								}
							}
							break;
						case WM_LBUTTONDBLCLK:
							x = ( m.LParam.ToInt32() << 16 ) >> 16;
							y = m.LParam.ToInt32() >> 16;
							if( ytc.ScrollButtonStyle == YaScrollButtonStyle.Always && ytc.GetLeftScrollButtonRect().Contains( x, y ) )
							{
								ytc.ScrollTabs( -10 );
							}
							else if( ytc.ScrollButtonStyle == YaScrollButtonStyle.Always && ytc.GetRightScrollButtonRect().Contains( x, y ) )
							{
								ytc.ScrollTabs( 10 );
							}
							return;
					}
				}
			}
			finally
			{
				base.WndProc( ref m );
			}
		}

		/// <summary>
		/// Overridden. Inherited from <see cref="IDesigner.DoDefaultAction()"/>.
		/// </summary>
		public override void DoDefaultAction() {}

		/// <summary>
		/// Id for the WM_LBUTTONDOWN message.
		/// </summary>
		private const int WM_LBUTTONDOWN = 0x0201;

		/// <summary>
		/// Id for the WM_LBUTTONDBLCLICK message.
		/// </summary>
		private const int WM_LBUTTONDBLCLK = 0x0203;
		
		/// <summary>
		/// Watches for the removal of <see cref="YaTabDrawer"/>s and, should
		/// one get removed that is assigned to the <see cref="YaTabControl"/>,
		/// then set the <see cref="YaTabControl.TabDrawer"/> property to <b>null</b>.
		/// </summary>
		/// <param name="sender">
		/// The object that send this event.
		/// </param>
		/// <param name="cea">
		/// Some <see cref="ComponentEventArgs"/>.
		/// </param>
		private void ComponentRemoved( object sender, ComponentEventArgs cea )
		{
			if( cea.Component == ytc.TabDrawer )
			{
				ytc.TabDrawer = null;
				RaiseComponentChanging( TypeDescriptor.GetProperties( Control )[ "TabDrawer" ] );
				RaiseComponentChanged( TypeDescriptor.GetProperties( Control )[ "TabDrawer" ], cea.Component, null );
			}
		}

		/// <summary>
		/// Event handler for the "Add Tab" verb.
		/// </summary>
		/// <param name="sender">
		/// The sender.
		/// </param>
		/// <param name="ea">
		/// Some <see cref="EventArgs"/>.
		/// </param>
		private void AddTab( object sender, EventArgs ea )
		{
			IDesignerHost dh = ( IDesignerHost ) GetService( typeof( IDesignerHost ) );
			if( dh != null )
			{
				int i = ytc.SelectedIndex;
                while( true )
                {
                    try
                    {
                        string name = GetNewTabName();
                        YaTabPage ytp = dh.CreateComponent(typeof(YaTabPage), name) as YaTabPage;
                        ytp.Text = name;
                        ytc.Controls.Add(ytp);
                        ytc.SelectedTab = ytp;
                        RaiseComponentChanging(TypeDescriptor.GetProperties(Control)["SelectedIndex"]);
                        RaiseComponentChanged(TypeDescriptor.GetProperties(Control)["SelectedIndex"], i, ytc.SelectedIndex);
                        break;
                    }
                    catch( Exception ) {}
                }
			}
		}

		/// <summary>
		/// Event handler for the "Remove Tab" verb.
		/// </summary>
		/// <param name="sender">
		/// The sender.
		/// </param>
		/// <param name="ea">
		/// Some <see cref="EventArgs"/>.
		/// </param>
		private void RemoveTab( object sender, EventArgs ea )
		{
			IDesignerHost dh = ( IDesignerHost ) GetService( typeof( IDesignerHost ) );
			if( dh != null )
			{
				int i = ytc.SelectedIndex;
				if( i > -1 )
				{
					YaTabPage ytp = ytc.SelectedTab;
					ytc.Controls.Remove( ytp );
					dh.DestroyComponent( ytp );
					RaiseComponentChanging( TypeDescriptor.GetProperties( Control )[ "SelectedIndex" ] );
					RaiseComponentChanged( TypeDescriptor.GetProperties( Control )[ "SelectedIndex" ], i, 0 );
				}
			}
		}

		/// <summary>
		/// Gets a new tab name for the a tab.
		/// </summary>
		/// <returns></returns>
		private string GetNewTabName()
		{
            _controlNameNumber += 1;
            return "tabPage" + _controlNameNumber;
		}

		/// <summary>
		/// Contains the verbs used to modify the <see cref="YaTabControl"/>.
		/// </summary>
		private DesignerVerbCollection verbs;

		/// <summary>
		/// Contains a cast reference to the <see cref="YaTabControl"/> that
		/// this designer handles.
		/// </summary>
		private YaTabControl ytc;

        /// <summary>
        /// Contains the most recent incremented value for generating new
        /// tab names.
        /// </summary>
        private static int _controlNameNumber;
	}
}
