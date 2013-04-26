using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace GrayIris.Utilities.UI.Controls
{
	/// <summary>
	/// Yet Another Tab Control.
	/// </summary>
	[Designer( typeof( GrayIris.Utilities.UI.Controls.Design.YaTabControlDesigner ) )]
	public class YaTabControl : Control
	{
		#region Constructor

		/// <summary>
		/// Creates a new instance of the <see cref="YaTabControl"/>
		/// class.
		/// </summary>
		public YaTabControl()
		{
			SetStyle( ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true );
			defaultImageIndex = -1;
			yaTabLengths = new ArrayList( 5 );
			leftArrow = new Point[ 3 ];
			rightArrow = new Point[ 3 ];
			for( int i = 0; i < 3; i++ )
			{
				leftArrow[ i ] = new Point( 0, 0 );
				rightArrow[ i ] = new Point( 0, 0 );
			}
			yaTabFont = Font;
			yaBoldTabFont = new Font( Font.FontFamily.Name, Font.Size, FontStyle.Bold );
			yaMargin = 3;
			yaTabDock = DockStyle.Top;
			yaSelectedIndex = -1;
			yaForeBrush = new SolidBrush( ForeColor );
			yaActiveBrush = ( Brush ) SystemBrushes.Control.Clone();
			yaActiveColor = SystemColors.Control;
			yaInactiveBrush = ( Brush ) SystemBrushes.Window.Clone();
			yaInactiveColor = SystemColors.Window;
			yaBorderPen = ( Pen ) Pens.DarkGray.Clone();
			yaShadowPen = ( Pen ) SystemPens.ControlDark.Clone();
			yaHighlightPen = ( Pen ) SystemPens.ControlLight.Clone();
			yaDisplayRectangle = Rectangle.Empty;
			yaTabsRectangle = Rectangle.Empty;
			yaClientRectangle = Rectangle.Empty;
			yaTransformedDisplayRectangle = Rectangle.Empty;
			Height = Width = 300;
			BackColor = SystemColors.Control;
			CalculateTabSpan();
			CalculateTabLengths();
			CalculateLastVisibleTabIndex();
			ChildTextChangeEventHandler = new EventHandler( YaTabPage_TextChanged );
            OverIndex = -1;
		}

		#endregion

		#region Original Public Properties

		/// <summary>
		/// Gets and sets the value of the index of the image in
		/// <see cref="ImageList"/> to use to draw the default image on tabs
		/// that do not specify an image to use.
		/// </summary>
		/// <value>
		/// The zero-based index to the image in the <see cref="YaTabControl.ImageList"/>
		/// that appears on the tab. The default is -1, which signifies no image.
		/// </value>
		/// <exception cref="ArgumentException">
		/// The value of <see cref="ImageIndex"/> is less than -1.
		/// </exception>
		public virtual int ImageIndex
		{
			get
			{
				return defaultImageIndex;
			}
			set
			{
				defaultImageIndex = value;
				CalculateTabLengths();
				InU();
			}
		}

		/// <summary>
		/// Gets and sets the <see cref="ImageList"/> used by this
		/// <see cref="YaTabControl"/>.
		/// </summary>
		/// <remarks>
		/// To display an image on a tab, set the <see cref="ImageIndex"/> property
		/// of that <see cref="YaTabPage"/>. The <see cref="ImageIndex"/> acts as the
		/// index into the <see cref="ImageList"/>.
		/// </remarks>
		public virtual ImageList ImageList
		{
			get
			{
				return images;
			}
			set
			{
				images = value;
				CalculateTabLengths();
				InU();
			}
		}

		/// <summary>
		/// Gets and sets the <see cref="YaTabDrawer"/> used
		/// to draw the tabs for the <see cref="YaTabControl"/>.
		/// </summary>
		/// <remarks>
		/// <para>The default value of this property is <b>null</b>.</para>
		/// <para>When this property is <b>null</b>, no tabs get drawn.</para>
		/// </remarks>
		public virtual YaTabDrawer TabDrawer
		{
			get
			{
				return yaTabDrawer;
			}
			set
			{
				yaTabDrawer = value;
				InU();
				OnTabDrawerChanged( new EventArgs() );
			}
		}

		/// <summary>
		/// Gets and sets the docking side of the tabs.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Thrown if the property tries to get set to
		/// <see cref="DockStyle.Fill"/> of <see cref="DockStyle.None"/>.
		/// </exception>
		/// <remarks>
		/// The default value of this property is <see cref="DockStyle.Top"/>.
		/// </remarks>
		public virtual DockStyle TabDock
		{
			get
			{
				return yaTabDock;
			}
			set
			{
				if( DockStyle.Fill == value || DockStyle.None == value )
				{
					throw new ArgumentException( "Tried to set the TabDock property to an invlaid value of Fill or None." );
				}
				if( yaTabDrawer == null || yaTabDrawer.SupportsTabDockStyle( value ) )
				{
					yaTabDock = value;
				}
				else
				{
					throw new ArgumentException( "Tried to set the TabDock property to a value not supported by the current tab drawer." );
				}
				CalculateRectangles();
				PerformLayout();
				InU();
				OnTabDockChanged( new EventArgs() );
			}
		}

		/// <summary>
		/// Gets and sets the <see cref="Font"/> used to draw the strings in
		/// the tabs.
		/// </summary>
		public virtual Font TabFont
		{
			get
			{
				return yaTabFont;
			}
			set
			{
				if( value != null )
				{
					yaTabFont = value;
					yaBoldTabFont = new Font( value.FontFamily.Name, value.Size, FontStyle.Bold );
					CalculateTabSpan();
					CalculateTabLengths();
					CalculateRectangles();
					PerformLayout();
					InU();
				}
				else
				{
					yaTabFont = Font;
				}
				OnTabFontChanged( new EventArgs() );
			}
		}

		/// <summary>
		/// Gets and sets the number of pixels to use as the margin.
		/// </summary>
		/// <remarks>
		/// This property puts a margin between the edge of the control
		/// and the tabs, between the tabs and the active tab page, and
		/// the active tab page and the edges of the control. The default
		/// value is 3.
		/// </remarks>
		/// <exception cref="ArgumentException">
		/// Thrown if this property get set to a value less than 0.
		/// </exception>
		public virtual int Margin
		{
			get
			{
				return yaMargin;
			}
			set
			{
				if( value < 0 )
				{
					throw new ArgumentException( "Tried to set the property Margin to a negative number." );
				}
				yaMargin = value;
				CalculateRectangles();
				CalculateTabLengths();
				CalculateTabSpan();
				CalculateLastVisibleTabIndex();
				PerformLayout();
				InU();
				OnMarginChanged( new EventArgs() );
			}
		}

		/// <summary>
		/// The <see cref="Color"/> of the active tab's
		/// background and the margins around the visible
		/// <see cref="YaTabPage"/>.
		/// </summary>
		/// <remarks>
		/// The default value for this is
		/// <see cref="SystemColors.Control"/>.
		/// </remarks>
		public virtual Color ActiveColor
		{
			get
			{
				return yaActiveColor;
			}
			set
			{
				yaActiveColor = value;
				yaActiveBrush.Dispose();
				yaActiveBrush = new SolidBrush( value );
				yaShadowPen.Color = ControlPaint.DarkDark( value );
				OnActiveColorChanged( new EventArgs() );
				InU();
			}
		}

		/// <summary>
		/// The <see cref="Color"/> of the inactive tabs'
		/// background.
		/// </summary>
		/// <remarks>
		/// The default value for this property is <see cref="Color.GhostWhite"/>.
		/// </remarks>
		public virtual Color InactiveColor
		{
			get
			{
				return yaInactiveColor;
			}
			set
			{
				yaInactiveColor = value;
				yaInactiveBrush.Dispose();
				yaInactiveBrush = new SolidBrush( value );
				yaHighlightPen.Color = ControlPaint.LightLight( value );
				OnInactiveColorChanged( new EventArgs() );
				InU();
			}
		}

		/// <summary>
		/// The <see cref="Color"/> of the border drawn
		/// around the control.
		/// </summary>
		/// <remarks>
		/// The default value for this property is <see cref="Color.DarkGray"/>.
		/// </remarks>
		public virtual Color BorderColor
		{
			get
			{
				return yaBorderPen.Color;
			}
			set
			{
				yaBorderPen.Color = value;
				OnBorderColorChanged( new EventArgs() );
				InU();
			}
		}

        public virtual int OverIndex { get; set; }

		/// <summary>
		/// Gets and sets the zero-based index of the selected
		/// <see cref="YaTabPage"/>.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Thrown if this property gets set to a value less than 0 when
		/// <see cref="YaTabPage"/>s exist in the control collection.
		/// </exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Thrown if this property gets set to a value greater than
		/// <see cref="Control.ControlCollection.Count"/>.
		/// </exception>
		public virtual int SelectedIndex
		{
			get
			{
				return yaSelectedIndex;
			}
			set
			{
				if( value < 0 && Controls.Count > 0 )
				{
					throw new ArgumentException( "Tried to set the property SelectedIndex to a negative number." );
				}
				else if( value >= Controls.Count )
				{
					throw new IndexOutOfRangeException( "Tried to set the property of the SelectedIndex to a value greater than the number of controls." );
				}
				TabChangingEventArgs tcea = new TabChangingEventArgs( yaSelectedIndex, value );
				OnTabChanging( tcea );
				if( tcea.Cancel )
				{
					return;
				}
				yaSelectedIndex = value;
				if( Controls.Count > 0 )
				{
                    foreach (Control ctrl in this.Controls)
                    {
                        ctrl.Visible = false;
                    }
					yaSelectedTab = ( YaTabPage ) Controls[ value ];
					yaSelectedTab.Visible = true;
					PerformLayout();
					InU();
				}
				OnTabChanged( new EventArgs() );
			}
		}

		/// <summary>
		/// Gets and sets how the scroll buttons should get
		/// shown when drawing the tabs in the tab area.
		/// </summary>
		/// <remarks>
		/// The default value for this is <see cref="YaScrollButtonStyle.Always"/>.
		/// </remarks>
		public virtual YaScrollButtonStyle ScrollButtonStyle
		{
			get
			{
				return yaShowScrollButtons;
			}
			set
			{
				yaShowScrollButtons = value;
				InU();
				OnScrollButtonStyleChanged( new EventArgs() );
			}
		}

		/// <summary>
		/// Gets and sets the currently selected tab.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Thrown if this property gets set to a <see cref="YaTabPage"/>
		/// that has not been added to the <see cref="YaTabControl"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// Thrown if this property gets set to a <b>null</b> value when
		/// <see cref="YaTabPage"/>s exist in the control.
		/// </exception>
		public virtual YaTabPage SelectedTab
		{
			get
			{
				return yaSelectedTab;
			}
			set
			{
				if( value == null && Controls.Count > 0 )
				{
					throw new ArgumentNullException( "value", "Tried to set the SelectedTab property to a null value." );
				}
				else if( value != null && !Controls.Contains( value ) )
				{
					throw new ArgumentException( "Tried to set the SelectedTab property to a YaTabPage that has not been added to this YaTabControl." );
				}
				if( Controls.Count > 0 )
				{
					int newIndex;
					for( newIndex = 0; newIndex < Controls.Count; newIndex++ )
					{
						if( value == Controls[ newIndex ] )
						{
							break;
						}
					}
					TabChangingEventArgs tcea = new TabChangingEventArgs( yaSelectedIndex, newIndex );
					OnTabChanging( tcea );
					if( tcea.Cancel )
					{
						return;
					}
					yaSelectedIndex = newIndex;
					yaSelectedTab.Visible = false;
					yaSelectedTab = value;
					yaSelectedTab.Visible = true;
					PerformLayout();
					InU();
					OnTabChanged( new EventArgs() );
				}
			}
		}

		#endregion

		#region Overridden Public Properties

		/// <summary>
		/// Inherited from <see cref="Control"/>.
		/// </summary>
		/// <remarks>See <see cref="Control.ForeColor"/>.
		/// </remarks>
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				yaForeBrush = new SolidBrush( value );
			}
		}

		/// <summary>
		/// Inherited from <see cref="Control"/>.
		/// </summary>
		public override Rectangle DisplayRectangle
		{
			get
			{
				return yaTransformedDisplayRectangle;
			}
		}

		#endregion

		#region Original Public Methods

		/// <summary>
		/// Returns the bounding rectangle for a specified tab in this tab control.
		/// </summary>
		/// <param name="index">The 0-based index of the tab you want.</param>
		/// <returns>A <see cref="Rectangle"/> that represents the bounds of the specified tab.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The index is less than zero.<br />-or-<br />The index is greater than or equal to <see cref="Control.ControlCollection.Count" />.
		/// </exception>
		public virtual Rectangle GetTabRect( int index )
		{
			if( index < 0 || index >= Controls.Count )
			{
				throw new ArgumentOutOfRangeException( "index", index, "The value of index passed to GetTabRect fell outside the valid range." );
			}
			float l = 0.0f;
			if( yaTabDock == DockStyle.Left )
			{
				l = Height - Convert.ToSingle( yaTabLengths[ 0 ] ) + yaTabLeftDif;
				for( int i = 0; i < index; i++ )
				{
					l -= Convert.ToSingle( yaTabLengths[ i + 1 ] );
				}
			}
			else
			{
				l = 2.0f * Convert.ToSingle( yaMargin ) - yaTabLeftDif;
				for( int i = 0; i < index; i++ )
				{
					l += Convert.ToSingle( yaTabLengths[ i ] );
				}
			}
			switch( yaTabDock )
			{
				case DockStyle.Bottom:
					return new Rectangle( Convert.ToInt32( l ), 3 * yaMargin + yaClientRectangle.Height, Convert.ToInt32( yaTabLengths[ index ] ), Convert.ToInt32( yaTabSpan ) + yaMargin );
				case DockStyle.Right:
					return new Rectangle( yaClientRectangle.Height, Convert.ToInt32( l ), Convert.ToInt32( yaTabSpan ) + yaMargin, Convert.ToInt32( yaTabLengths[ index ] ) );
				case DockStyle.Left:
					return new Rectangle( yaMargin, Convert.ToInt32( l ), Convert.ToInt32( yaTabSpan ) + yaMargin, Convert.ToInt32( yaTabLengths[ index ] ) );
			}
			return new Rectangle( Convert.ToInt32( l ), 2 * yaMargin, Convert.ToInt32( yaTabLengths[ index ] ), Convert.ToInt32( yaTabSpan ) + yaMargin );
		}

		/// <summary>
		/// Gets the <see cref="Rectangle"/> that contains the left
		/// scroll button.
		/// </summary>
		/// <returns>
		/// A <see cref="Rectangle"/>.
		/// </returns>
		public virtual Rectangle GetLeftScrollButtonRect()
		{
			Rectangle r = Rectangle.Empty;
			if( yaShowScrollButtons == YaScrollButtonStyle.Always )
			{
				int tabSpan = Convert.ToInt32( yaTabSpan );
				switch( yaTabDock )
				{
					case DockStyle.Top:
						r = new Rectangle( Width - 2 * yaTabsRectangle.Height, 0, yaTabsRectangle.Height, yaTabsRectangle.Height );
						break;
					case DockStyle.Bottom:
						r = new Rectangle( Width - 2 * yaTabsRectangle.Height, yaClientRectangle.Height, yaTabsRectangle.Height, yaTabsRectangle.Height );
						break;
					case DockStyle.Left:
						r = new Rectangle( 0, yaTabsRectangle.Height, yaTabsRectangle.Height, yaTabsRectangle.Height );
						break;
					case DockStyle.Right:
						r = new Rectangle( Width - yaTabsRectangle.Height, Height - 2 * yaTabsRectangle.Height, yaTabsRectangle.Height, yaTabsRectangle.Height );
						break;
				}
			}
			return r;
		}

		/// <summary>
		/// Gets the <see cref="Rectangle"/> that contains the left
		/// scroll button.
		/// </summary>
		/// <returns>
		/// A <see cref="Rectangle"/>.
		/// </returns>
		public virtual Rectangle GetRightScrollButtonRect()
		{
			Rectangle r = Rectangle.Empty;
			if( yaShowScrollButtons == YaScrollButtonStyle.Always )
			{
				int tabSpan = Convert.ToInt32( yaTabSpan );
				switch( yaTabDock )
				{
					case DockStyle.Top:
						r = new Rectangle( Width - yaTabsRectangle.Height, 0, yaTabsRectangle.Height, yaTabsRectangle.Height );
						break;
					case DockStyle.Bottom:
						r = new Rectangle( Width - yaTabsRectangle.Height, yaClientRectangle.Height, yaTabsRectangle.Height, yaTabsRectangle.Height );
						break;
					case DockStyle.Left:
						r = new Rectangle( 0, 0, yaTabsRectangle.Height, yaTabsRectangle.Height );
						break;
					case DockStyle.Right:
						r = new Rectangle( Width - yaTabsRectangle.Height, Height - yaTabsRectangle.Height, yaTabsRectangle.Height, yaTabsRectangle.Height );
						break;
				}
			}
			return r;
		}

		/// <summary>
		/// Scrolls the tabs by the specified <i>amount</i>.
		/// </summary>
		/// <param name="amount">
		/// The number of pixels to scroll the tabs.
		/// </param>
		/// <remarks>
		/// Positive amounts will scroll the tabs to the left. Negative
		/// amounts will scroll the tabs to the right.
		/// </remarks>
		public virtual void ScrollTabs( int amount )
		{
			yaTabLeftDif = Math.Max( 0, yaTabLeftDif - amount );
			if( yaTabLeftDif <= 0 || yaTabLeftDif >= yaTotalTabSpan - Convert.ToSingle( yaTabLengths[ yaTabLengths.Count - 1 ] ) )
			{
				lock( this )
				{
					yaKeepScrolling = false;
				}
			}
			if( yaTabLeftDif >= yaTotalTabSpan - Convert.ToSingle( yaTabLengths[ yaTabLengths.Count - 1 ] ) )
			{
				canScrollLeft = false;
			}
			if( yaTabLeftDif <= 0 )
			{
				canScrollRight = false;
			}
			CalculateLastVisibleTabIndex();
			InU();
		}

		#endregion

		#region Original Public Events

		/// <summary>
		/// Occurs when the selected tab is about to change.
		/// </summary>
		public event TabChangingEventHandler TabChanging;

		/// <summary>
		/// Occurs after the selected tab has changed.
		/// </summary>
		public event EventHandler TabChanged;

		/// <summary>
		/// Occurs after the border color has changed
		/// </summary>
		public event EventHandler BorderColorChanged;

		/// <summary>
		/// Occurs after the active color has changed
		/// </summary>
		public event EventHandler ActiveColorChanged;

		/// <summary>
		/// Occurs after the inactive color has changed
		/// </summary>
		public event EventHandler InactiveColorChanged;

		/// <summary>
		/// Occurs after the margin for the control has changed.
		/// </summary>
		public event EventHandler MarginChanged;

		/// <summary>
		/// Occurs after the <see cref="TabDock"/> property
		/// has changed.
		/// </summary>
		public event EventHandler TabDockChanged;

		/// <summary>
		/// Occurs after the <see cref="TabDrawer"/> property
		/// has changed.
		/// </summary>
		public event EventHandler TabDrawerChanged;

		/// <summary>
		/// Occurs after the <see cref="TabFont"/> property
		/// has changed.
		/// </summary>
		public event EventHandler TabFontChanged;

		/// <summary>
		/// Occurs after the <see cref="ScrollButtonStyle"/>
		/// property has changed.
		/// </summary>
		public event EventHandler ScrollButtonStyleChanged;

		#endregion

		#region New Protected Methods

		/// <summary>
		/// Fires the <see cref="ScrollButtonStyleChanged"/> event.
		/// </summary>
		/// <param name="ea">
		/// Some <see cref="EventArgs"/>.
		/// </param>
		protected virtual void OnScrollButtonStyleChanged( EventArgs ea )
		{
			if( ScrollButtonStyleChanged != null )
			{
				ScrollButtonStyleChanged( this, ea );
			}
		}

		/// <summary>
		/// Fires the <see cref="TabFontChanged"/> event.
		/// </summary>
		/// <param name="ea">
		/// Some <see cref="EventArgs"/>.
		/// </param>
		protected virtual void OnTabFontChanged( EventArgs ea )
		{
			if( TabFontChanged != null )
			{
				TabFontChanged( this, ea );
			}
		}

		/// <summary>
		/// Fires the <see cref="TabDrawerChanged"/> event.
		/// </summary>
		/// <param name="ea">
		/// Some <see cref="EventArgs"/>.
		/// </param>
		protected virtual void OnTabDrawerChanged( EventArgs ea )
		{
			if( TabDrawerChanged != null )
			{
				TabDrawerChanged( this, ea );
			}
		}

		/// <summary>
		/// Fires the <see cref="TabDockChanged"/> event.
		/// </summary>
		/// <param name="ea">
		/// Some <see cref="EventArgs"/>.
		/// </param>
		protected virtual void OnTabDockChanged( EventArgs ea )
		{
			if( TabDockChanged != null )
			{
				TabDockChanged( this, ea );
			}
		}

		/// <summary>
		/// Fires the <see cref="MarginChanged"/> event.
		/// </summary>
		/// <param name="ea">
		/// Some <see cref="EventArgs"/>.
		/// </param>
		protected virtual void OnMarginChanged( EventArgs ea )
		{
			if( MarginChanged != null )
			{
				MarginChanged( this, ea );
			}
		}

		/// <summary>
		/// Fires the <see cref="InactiveColorChanged"/> event.
		/// </summary>
		/// <param name="ea">
		/// Some <see cref="EventArgs"/>.
		/// </param>
		protected virtual void OnInactiveColorChanged( EventArgs ea )
		{
			if( InactiveColorChanged != null )
			{
				InactiveColorChanged( this, ea );
			}
		}

		/// <summary>
		/// Fires the <see cref="ActiveColorChanged"/> event.
		/// </summary>
		/// <param name="ea">
		/// Some <see cref="EventArgs"/>.
		/// </param>
		protected virtual void OnActiveColorChanged( EventArgs ea )
		{
			if( ActiveColorChanged != null )
			{
				ActiveColorChanged( this, ea );
			}
		}

		/// <summary>
		/// Fires the <see cref="BorderColorChanged"/> event.
		/// </summary>
		/// <param name="ea">
		/// Some <see cref="EventArgs"/>.
		/// </param>
		protected virtual void OnBorderColorChanged( EventArgs ea )
		{
			if( BorderColorChanged != null )
			{
				BorderColorChanged( this, ea );
			}
		}

		/// <summary>
		/// Fires the <see cref="TabChanging"/> event.
		/// </summary>
		/// <param name="tcea">
		/// Some <see cref="TabChangingEventArgs"/> for the event.
		/// </param>
		protected virtual void OnTabChanging( TabChangingEventArgs tcea )
		{
			if( TabChanging != null )
			{
				TabChanging( this, tcea );
			}
		}

		/// <summary>
		/// Fires the <see cref="TabChanged"/> event.
		/// </summary>
		/// <param name="ea">
		/// Some <see cref="EventArgs"/> for the event.
		/// </param>
		protected virtual void OnTabChanged( EventArgs ea )
		{
			if( TabChanged != null )
			{
				TabChanged( this, ea );
			}
		}

		#endregion

		#region Protected Overridden Methods

        /// <summary>
        /// Overridden. Inherited from <see cref="Control"/>.
        /// </summary>
        /// <param name="mea">
        /// See <see cref="Control.OnMouseLeave(MouseEventArgs)"/>.
        /// </param>
        protected override void OnMouseLeave(EventArgs mea)
        {
            OverIndex = -1;
            Invalidate();
        }

        /// <summary>
        /// Overridden. Inherited from <see cref="Control"/>.
        /// </summary>
        /// <param name="mea">
        /// See <see cref="Control.OnMouseMove(MouseEventArgs)"/>.
        /// </param>
        protected override void OnMouseMove(MouseEventArgs mea)
        {
            base.OnMouseMove( mea );
            int t = -Convert.ToInt32(yaTabLeftDif);
			Point p = new Point( mea.X - 2 * yaMargin, mea.Y );
			switch( yaTabDock )
			{
				case DockStyle.Bottom:
					p.Y -= yaClientRectangle.Height;
					break;
				case DockStyle.Left:
					p.Y = mea.X;
					p.X = Height - mea.Y;
					break;
				case DockStyle.Right:
					p.Y = Width - mea.X;
					p.X = mea.Y;
					break;
			}
            if (p.Y > yaMargin && p.Y < Convert.ToInt32(yaTabSpan + 3.0f * yaMargin))
            {
                int runningTotal = t;
                for (int i = 0; i <= yaLastVisibleTabIndex; i++)
                {
                    if (p.X >= runningTotal && p.X < runningTotal + Convert.ToInt32(yaTabLengths[i]))
                    {
                        bool changed = OverIndex != i;
                        if (changed)
                        {
                            OverIndex = i;
                            Invalidate();
                        }
                        break;
                    }
                    runningTotal += Convert.ToInt32(yaTabLengths[i]);
                }
            }
            else
            {
                OverIndex = -1;
                Invalidate();
            }
        }

		/// <summary>
		/// Overridden. Inherited from <see cref="Control"/>.
		/// </summary>
		/// <param name="mea">
		/// See <see cref="Control.OnMouseDown(MouseEventArgs)"/>.
		/// </param>
		protected override void OnMouseDown( MouseEventArgs mea )
		{
			base.OnMouseDown( mea );
			Point p = new Point( mea.X - 2 * yaMargin, mea.Y );
			switch( yaTabDock )
			{
				case DockStyle.Bottom:
					p.Y -= yaClientRectangle.Height;
					break;
				case DockStyle.Left:
					p.Y = mea.X;
					p.X = Height - mea.Y;
					break;
				case DockStyle.Right:
					p.Y = Width - mea.X;
					p.X = mea.Y;
					break;
			}
			if( p.Y > yaMargin && p.Y < Convert.ToInt32( yaTabSpan + 3.0f * yaMargin ) )
			{
				if( ( yaShowScrollButtons == YaScrollButtonStyle.Always || ( yaShowScrollButtons == YaScrollButtonStyle.Auto && yaTotalTabSpan > calcWidth ) ) && p.X >= rightArrow[ 0 ].X - 3 * yaMargin )
				{
					if( canScrollRight )
					{
						yaKeepScrolling = true;
						ScrollerThread st = new ScrollerThread( 2, this );
						Thread t = new Thread( new ThreadStart( st.ScrollIt ) );
						t.Start();
					}
				}
				else if( ( yaShowScrollButtons == YaScrollButtonStyle.Always || ( yaShowScrollButtons == YaScrollButtonStyle.Auto && yaTotalTabSpan > calcWidth ) ) && p.X >= leftArrow[ 2 ].X - 3 * yaMargin )
				{
					if( canScrollLeft )
					{
						yaKeepScrolling = true;
						ScrollerThread st = new ScrollerThread( -2, this );
						Thread t = new Thread( new ThreadStart( st.ScrollIt ) );
						t.Start();
					}
				}
				else
				{
					int t = -Convert.ToInt32( yaTabLeftDif );
					for( int i = 0; i <= yaLastVisibleTabIndex; i++ )
					{
						if( p.X >= t && p.X < t + Convert.ToInt32( yaTabLengths[ i ] ) )
						{
							SelectedIndex = i;
							break;
						}
						t += Convert.ToInt32( yaTabLengths[ i ] );
					}
				}
			}
		}

		/// <summary>
		/// Overridden. Inherited from <see cref="Control"/>.
		/// </summary>
		/// <param name="mea">
		/// Some <see cref="MouseEventArgs"/>.
		/// </param>
		protected override void OnMouseUp( MouseEventArgs mea )
		{
			lock( this )
			{
				yaKeepScrolling = false;
			}
			base.OnMouseUp( mea );
		}

		/// <summary>
		/// Overridden. Inherited from <see cref="Control"/>.
		/// </summary>
		/// <param name="cea">
		/// See <see cref="Control.OnControlAdded(ControlEventArgs)"/>.
		/// </param>
		protected override void OnControlAdded( ControlEventArgs cea )
		{
			base.OnControlAdded( cea );
			cea.Control.Visible = false;
			if( yaSelectedIndex == -1 )
			{
				yaSelectedIndex = 0;
				yaSelectedTab = ( YaTabPage ) cea.Control;
				yaSelectedTab.Visible = true;
			}
			cea.Control.TextChanged += ChildTextChangeEventHandler;
			CalculateTabLengths();
			CalculateLastVisibleTabIndex();
			InU();
		}

		/// <summary>
		/// Overridden. Inherited from <see cref="Control"/>.
		/// </summary>
		/// <param name="cea">
		/// See <see cref="Control.OnControlRemoved(ControlEventArgs)"/>.
		/// </param>
		protected override void OnControlRemoved( ControlEventArgs cea )
		{
			cea.Control.TextChanged -= ChildTextChangeEventHandler;
			base.OnControlRemoved( cea );
			if( Controls.Count > 0 )
			{
				yaSelectedIndex = 0;
				yaSelectedTab.Visible = false;
				yaSelectedTab = ( YaTabPage ) Controls[ 0 ];
				yaSelectedTab.Visible = true;
			}
			else
			{
				yaSelectedIndex = -1;
				yaSelectedTab = null;
			}
			CalculateTabLengths();
			CalculateLastVisibleTabIndex();
			InU();
		}

		/// <summary>
		/// Inherited from <see cref="Control"/>.
		/// </summary>
		/// <param name="disposing">
		/// See <see cref="Control.Dispose(bool)"/>.
		/// </param>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				base.Dispose( disposing );
				yaInactiveBrush.Dispose();
				yaActiveBrush.Dispose();
				yaForeBrush.Dispose();
				yaHighlightPen.Dispose();
				yaShadowPen.Dispose();
				yaBorderPen.Dispose();
				if( yaTabDrawer != null )
				{
					yaTabDrawer.Dispose();
				}
				foreach( Control c in Controls )
				{
					c.Dispose();
				}
			}
		}

		/// <summary>
		/// Inherited from <see cref="Control"/>.
		/// </summary>
		/// <param name="pea">
		/// See <see cref="Control.OnSizeChanged(EventArgs)"/>.
		/// </param>
		protected override void OnPaint( PaintEventArgs pea )
		{
			if( Controls.Count > 0 )
			{
				CalculateTabLengths();

				bool invert = false;
				// Create a transformation given the orientation of the tabs.
				switch( yaTabDock )
				{
					case DockStyle.Bottom:
						invert = true;
						pea.Graphics.TranslateTransform( Convert.ToSingle( calcWidth ), Convert.ToSingle( calcHeight ) );
						pea.Graphics.RotateTransform( 180.0f );
						break;
					case DockStyle.Left:
						pea.Graphics.TranslateTransform( 0, Convert.ToSingle( Height ) );
						pea.Graphics.RotateTransform( -90.0f );
						break;
					case DockStyle.Right:
						pea.Graphics.TranslateTransform( Convert.ToSingle( Width ), 0 );
						pea.Graphics.RotateTransform( 90.0f );
						break;
				}

				// Paint the areas.
				pea.Graphics.FillRectangle( yaInactiveBrush, yaTabsRectangle );
				pea.Graphics.FillRectangle( yaActiveBrush, yaClientRectangle );

				// Draws the highlight/shadow line, if applicable.
				Pen p = yaBorderPen;
				if( yaTabDrawer != null && yaTabDrawer.UsesHighlghts )
				{
					if( DockStyle.Right == yaTabDock || DockStyle.Bottom == yaTabDock )
					{
						p = yaShadowPen;
					}
					else
					{
						p = yaHighlightPen;
					}
				}
				pea.Graphics.DrawLine( p, 0, yaClientRectangle.Y, calcWidth, yaClientRectangle.Y );

				// Save the current transform so that we can go back to it
				// after printing the tabs.
				Matrix m = pea.Graphics.Transform;
				SizeF s = new SizeF( 0, yaTabSpan + 2.0f * yaMargin + 1.0f );

				// If a tab drawer exists, use it.
				if( yaTabDrawer != null )
				{
					if( !invert )
					{
						pea.Graphics.TranslateTransform( 2.0f * Convert.ToSingle( yaMargin ) - yaTabLeftDif, yaMargin + 1.0f );
					}
					else
					{
						pea.Graphics.TranslateTransform( Convert.ToSingle( Width ) - 2.0f * yaMargin - Convert.ToSingle( yaTabLengths[ 0 ] ) + yaTabLeftDif, yaMargin + 1.0f );
					}
					// The transform to the selected tab.
					Matrix selTransform = null;

					// Draw the tabs from left to right skipping over the
					// selected tab.
					for( int i = 0; i <= yaLastVisibleTabIndex; i++ )
					{
						s.Width = Convert.ToSingle( yaTabLengths[ i ] );
						if( i != yaSelectedIndex )
						{
							yaTabDrawer.DrawTab( yaActiveColor, yaInactiveColor, yaHighlightPen.Color, yaShadowPen.Color, yaBorderPen.Color, false, i == OverIndex, yaTabDock, pea.Graphics, s );
						}
						else
						{
							selTransform = pea.Graphics.Transform;
						}
						if( invert )
						{
							if( i + 1 < yaTabLengths.Count )
							{
								pea.Graphics.TranslateTransform( -Convert.ToSingle( yaTabLengths[ i + 1 ] ), 0.0f );
							}
						}
						else
						{
							pea.Graphics.TranslateTransform( s.Width, 0.0f );
						}
					}

					// Now, draw the selected tab.
					if( selTransform != null )
					{
						pea.Graphics.Transform = selTransform;
						s.Width = Convert.ToSingle( yaTabLengths[ yaSelectedIndex ] );
						yaTabDrawer.DrawTab( yaActiveColor, yaInactiveColor, yaHighlightPen.Color, yaShadowPen.Color, yaBorderPen.Color, true, SelectedIndex == OverIndex, yaTabDock, pea.Graphics, s );
					}
				}

				// Draw the strings. If the tabs are docked on the bottom, change
				// the tranformation to draw the string right-side up.
				if( !invert )
				{
					pea.Graphics.Transform = m;
					pea.Graphics.TranslateTransform( 2.0f * yaMargin - yaTabLeftDif, yaMargin );
				}
				else
				{
					pea.Graphics.ResetTransform();
					pea.Graphics.TranslateTransform( 2.0f * yaMargin - yaTabLeftDif, yaClientRectangle.Height );
				}
				YaTabPage ytp = null;
				for( int i = 0; i <= yaLastVisibleTabIndex; i++ )
				{
					s.Width = Convert.ToSingle( yaTabLengths[ i ] );
					ytp = Controls[ i ] as YaTabPage;
					if( ytp != null && images != null && ytp.ImageIndex > -1 && ytp.ImageIndex < images.Images.Count && images.Images[ ytp.ImageIndex ] != null )
					{
						pea.Graphics.DrawImage( images.Images[ ytp.ImageIndex ], 1.5f * yaMargin, yaMargin );
						if( i != yaSelectedIndex )
						{
							pea.Graphics.DrawString( Controls[ i ].Text, yaTabFont, yaForeBrush, 2.5f * yaMargin + images.Images[ ytp.ImageIndex ].Width, 1.5f * yaMargin );
						}
						else
						{
							pea.Graphics.DrawString( Controls[ i ].Text, yaBoldTabFont, yaForeBrush, 2.5f * yaMargin + images.Images[ ytp.ImageIndex ].Width, 1.5f * yaMargin );
						}
					}
					else if( ytp != null && images != null && defaultImageIndex > -1 && defaultImageIndex < images.Images.Count && images.Images[ defaultImageIndex ] != null )
					{
						pea.Graphics.DrawImage( images.Images[ defaultImageIndex ], 1.5f * yaMargin, yaMargin );
						if( i != yaSelectedIndex )
						{
							pea.Graphics.DrawString( Controls[ i ].Text, yaTabFont, yaForeBrush, 2.5f * yaMargin + images.Images[ defaultImageIndex ].Width, 1.5f * yaMargin );
						}
						else
						{
							pea.Graphics.DrawString( Controls[ i ].Text, yaBoldTabFont, yaForeBrush, 2.5f * yaMargin + images.Images[ defaultImageIndex ].Width, 1.5f * yaMargin );
						}
					}
					else
					{
						if( i != yaSelectedIndex )
						{
							pea.Graphics.DrawString( Controls[ i ].Text, yaTabFont, yaForeBrush, 1.5f * yaMargin, 1.5f * yaMargin );
						}
						else
						{
							pea.Graphics.DrawString( Controls[ i ].Text, yaBoldTabFont, yaForeBrush, 1.5f * yaMargin, 1.5f * yaMargin );
						}
					}
					pea.Graphics.TranslateTransform( s.Width, 0 );
				}

				// Draw the scroll buttons, if necessary
				canScrollLeft = canScrollRight = false;
				if( yaShowScrollButtons == YaScrollButtonStyle.Always || ( yaShowScrollButtons == YaScrollButtonStyle.Auto && yaTotalTabSpan > calcWidth ) )
				{
					if( invert )
					{
						pea.Graphics.ResetTransform();
						pea.Graphics.TranslateTransform( 0, yaClientRectangle.Height );
					}
					else
					{
						pea.Graphics.Transform = m;
					}
					pea.Graphics.FillRectangle( yaInactiveBrush, calcWidth - 2 * yaTabsRectangle.Height, 0, 2 * yaTabsRectangle.Height, yaTabsRectangle.Height );
					pea.Graphics.DrawRectangle( yaBorderPen, calcWidth - 2 * yaTabsRectangle.Height, 0, 2 * yaTabsRectangle.Height, yaTabsRectangle.Height );
					if( ( ( yaShowScrollButtons == YaScrollButtonStyle.Always && yaTotalTabSpan > calcWidth - 2 * Convert.ToInt32( yaTabsRectangle.Height ) ) || ( yaShowScrollButtons == YaScrollButtonStyle.Auto && yaTotalTabSpan > calcWidth ) ) && yaTabLeftDif < yaTotalTabSpan - Convert.ToSingle( yaTabLengths[ yaTabLengths.Count - 1 ] ) )
					{
						canScrollLeft = true;
						pea.Graphics.FillPolygon( yaBorderPen.Brush, leftArrow );
					}
					if( yaTabLeftDif > 0 )
					{
						canScrollRight = true;
						pea.Graphics.FillPolygon( yaBorderPen.Brush, rightArrow );
					}
					pea.Graphics.DrawPolygon( yaBorderPen, leftArrow );
					pea.Graphics.DrawPolygon( yaBorderPen, rightArrow );
				}
			}

			// Reset the transform and draw the border.
			pea.Graphics.ResetTransform();
			pea.Graphics.DrawRectangle( yaBorderPen, 0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1 );
		}

		/// <summary>
		/// Inherited from <see cref="Control"/>.
		/// </summary>
		/// <param name="e">
		/// See <see cref="Control.OnSizeChanged(EventArgs)"/>.
		/// </param>
		protected override void OnSizeChanged( EventArgs e )
		{
			base.OnSizeChanged( e );
			CalculateRectangles();
			CalculateLastVisibleTabIndex();
			if( yaTabLengths.Count > 0 && yaTabLeftDif >= yaTotalTabSpan - Convert.ToSingle( yaTabLengths[ yaTabLengths.Count - 1 ] ) )
			{
				yaTabLeftDif = 0;
				ScrollTabs( -Convert.ToInt32( yaTotalTabSpan - Convert.ToSingle( yaTabLengths[ yaTabLengths.Count - 1 ] ) ) );
			}
			PerformLayout();
			InU();
		}

		/// <summary>
		/// Overriden from <see cref="Control"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="YaTabControl.ControlCollection"/>.
		/// </returns>
		protected override System.Windows.Forms.Control.ControlCollection CreateControlsInstance()
		{
			return new YaTabControl.ControlCollection( this );
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Handles when the text changes for a control.
		/// </summary>
		/// <param name="sender">
		/// The <see cref="YaTabPage"/> whose text changed.
		/// </param>
		/// <param name="e">
		/// Some <see cref="EventArgs"/>.
		/// </param>
		private void YaTabPage_TextChanged( object sender, EventArgs e )
		{
			CalculateTabLengths();
			CalculateLastVisibleTabIndex();
		}

		/// <summary>
		/// Calculates the last visible tab shown on the control.
		/// </summary>
		private void CalculateLastVisibleTabIndex()
		{
			yaLastVisibleTabLeft = 0.0f;
			float t = 0.0f;
			for( int i = 0; i < yaTabLengths.Count; i++ )
			{
				yaLastVisibleTabIndex = i;
				t += Convert.ToSingle( yaTabLengths[ i ] ) + 2.0f;
				if( t > calcWidth + yaTabLeftDif )
				{
					break;
				}
				yaLastVisibleTabLeft = t;
			}
		}

		/// <summary>
		/// Calculates and caches the length of each tab given the value
		/// of the <see cref="Control.Text"/> property of each
		/// <see cref="YaTabPage"/>.
		/// </summary>
		private void CalculateTabLengths()
		{
			yaTotalTabSpan = 0.0f;
			yaTabLengths.Clear();
			Graphics g = CreateGraphics();
			float f = 0.0f;
			YaTabPage ytp;
			for( int i = 0; i < Controls.Count; i++ )
			{
				f = g.MeasureString( Controls[ i ].Text, yaBoldTabFont ).Width + 4.0f * Convert.ToSingle( yaMargin );
				ytp = Controls[ i ] as YaTabPage;
				if( ytp != null && images != null && ytp.ImageIndex > -1 && ytp.ImageIndex < images.Images.Count && images.Images[ ytp.ImageIndex ] != null )
				{
					f += images.Images[ ytp.ImageIndex ].Width + 2.0f * yaMargin;
				}
				else if( ytp != null && images != null && defaultImageIndex > -1 && defaultImageIndex < images.Images.Count && images.Images[ defaultImageIndex ] != null )
				{
					f += images.Images[ defaultImageIndex ].Width + 2.0f * yaMargin;
				}
				yaTabLengths.Add( f );
				yaTotalTabSpan += f;
			}
		}

		/// <summary>
		/// Calculates the span of a tab given the value of the <see cref="Font"/>
		/// property.
		/// </summary>
		private void CalculateTabSpan()
		{
			yaTabSpan = 0;
			if( images != null )
			{
				for( int i = 0; i < images.Images.Count; i++ )
				{
					yaTabSpan = Math.Max( yaTabSpan, images.Images[ i ].Height );
				}
			}
			yaTabSpan = Math.Max( yaTabSpan, CreateGraphics().MeasureString( @"ABCDEFGHIJKLMNOPQURSTUVWXYZabcdefghijklmnopqrstuvwxyz", yaTabFont ).Height + 1.0f );
		}

		/// <summary>
		/// Calculates the rectangles for the tab area, the client area,
		/// the display area, and the transformed display area.
		/// </summary>
		private void CalculateRectangles()
		{
			int spanAndMargin = Convert.ToInt32( yaTabSpan ) + 3 * yaMargin + 2;
			Size s;
			
			calcHeight = ( DockStyle.Top == yaTabDock || DockStyle.Bottom == yaTabDock )? Height : Width;
			calcWidth = ( DockStyle.Top == yaTabDock || DockStyle.Bottom == yaTabDock )? Width : Height;

			yaTabsRectangle.X = 0;
			yaTabsRectangle.Y = 0;
			s = yaTabsRectangle.Size;
			s.Width = calcWidth;
			s.Height = spanAndMargin;
			yaTabsRectangle.Size = s;

			leftArrow[ 0 ].X = s.Width - s.Height - yaMargin;
			leftArrow[ 0 ].Y = yaMargin;
			leftArrow[ 1 ].X = leftArrow[ 0 ].X;
			leftArrow[ 1 ].Y = s.Height - yaMargin;
			leftArrow[ 2 ].X = s.Width - 2 * s.Height + yaMargin;
			leftArrow[ 2 ].Y = s.Height / 2;

			rightArrow[ 0 ].X = s.Width - s.Height + yaMargin;
			rightArrow[ 0 ].Y = yaMargin;
			rightArrow[ 1 ].X = rightArrow[ 0 ].X;
			rightArrow[ 1 ].Y = s.Height - yaMargin;
			rightArrow[ 2 ].X = s.Width - yaMargin;
			rightArrow[ 2 ].Y = s.Height / 2;

			yaClientRectangle.X = 0;
			yaClientRectangle.Y = spanAndMargin;
			s = yaClientRectangle.Size;
			s.Width = calcWidth;
			s.Height = calcHeight - spanAndMargin;
			yaClientRectangle.Size = s;

			yaDisplayRectangle.X = yaMargin + 1;
			yaDisplayRectangle.Y = spanAndMargin + yaMargin + 1;
			s = yaDisplayRectangle.Size;
			s.Width = calcWidth - 2 * ( yaMargin + 1 );
			s.Height = yaClientRectangle.Size.Height - 2 * yaMargin - 2;
			yaDisplayRectangle.Size = s;

			switch( yaTabDock )
			{
				case DockStyle.Top:
					yaTransformedDisplayRectangle.Location = yaDisplayRectangle.Location;
					yaTransformedDisplayRectangle.Size = yaDisplayRectangle.Size;
					break;
				case DockStyle.Bottom:
					yaTransformedDisplayRectangle.X = yaMargin + 1;
					yaTransformedDisplayRectangle.Y = yaMargin + 1;
					yaTransformedDisplayRectangle.Size = yaDisplayRectangle.Size;
					break;
				case DockStyle.Right:
					yaTransformedDisplayRectangle.X = yaMargin + 1;
					yaTransformedDisplayRectangle.Y = yaMargin + 1;
					s.Height = yaDisplayRectangle.Size.Width;
					s.Width = yaDisplayRectangle.Size.Height;
					yaTransformedDisplayRectangle.Size = s;
					break;
				case DockStyle.Left:
					yaTransformedDisplayRectangle.X = yaDisplayRectangle.Top;
					yaTransformedDisplayRectangle.Y = calcWidth - yaDisplayRectangle.Right;
					s.Height = yaDisplayRectangle.Size.Width;
					s.Width = yaDisplayRectangle.Size.Height;
					yaTransformedDisplayRectangle.Size = s;
					break;
			}
		}

		/// <summary>
		/// Invalidates and updates the <see cref="YaTabControl"/>.
		/// </summary>
		private void InU()
		{
			Invalidate();
			Update();
		}

		/// <summary>
		/// Monitors when child <see cref="YaTabPage"/>s have their
		/// <see cref="YaTabPage.Text"/> property changed.
		/// </summary>
		/// <param name="sender">A <see cref="YaTabPage"/>.</param>
		/// <param name="ea">Some <see cref="EventArgs"/>.</param>
		private void ChildTabTextChanged( object sender, EventArgs ea )
		{
			CalculateTabLengths();
			InU();
		}

		#endregion

		#region Private Members

		/// <summary>
		/// The index to use as the default image for the tabs.
		/// </summary>
		private int defaultImageIndex;

		/// <summary>
		/// The <see cref="ImageList"/> used to draw the images in
		/// the tabs.
		/// </summary>
		private ImageList images;

		/// <summary>
		/// A flag to indicate if the tabs can scroll left.
		/// </summary>
		private bool canScrollLeft;

		/// <summary>
		/// A flag to indicate if the tabs can scroll right.
		/// </summary>
		private bool canScrollRight;

		/// <summary>
		/// A flag to indicate if scroll buttons should get drawn.
		/// </summary>
		private YaScrollButtonStyle yaShowScrollButtons;

		/// <summary>
		/// The array of floats whose each entry measures a tab's width.
		/// </summary>
		private ArrayList yaTabLengths;

		/// <summary>
		/// The sum of the lengths of all the tabs.
		/// </summary>
		private float yaTotalTabSpan;

		/// <summary>
		/// The margin around the visible <see cref="YaTabPage"/>.
		/// </summary>
		private int yaMargin;

		/// <summary>
		/// The span of the tabs. Used as the height/width of the
		/// tabs, depending on the orientation.
		/// </summary>
		private float yaTabSpan;

		/// <summary>
		/// The amount that the tabs have been scrolled to the left.
		/// </summary>
		private float yaTabLeftDif;

		/// <summary>
		/// The <see cref="Point"/>s that define the left scroll arrow.
		/// </summary>
		private Point[] leftArrow;

		/// <summary>
		/// The <see cref="Point"/>s that define the right scroll arrow.
		/// </summary>
		private Point[] rightArrow;

		/// <summary>
		/// The index of the last visible tab.
		/// </summary>
		private int yaLastVisibleTabIndex;

		/// <summary>
		/// The length from the left of the tab control
		/// to the left of the last visible tab.
		/// </summary>
		private float yaLastVisibleTabLeft;

		/// <summary>
		/// The brush used to draw the strings in the tabs.
		/// </summary>
		private Brush yaForeBrush;

		/// <summary>
		/// The color of the active tab and area.
		/// </summary>
		private Color yaActiveColor;

		/// <summary>
		/// The brush used to color the active-colored area.
		/// </summary>
		private Brush yaActiveBrush;

		/// <summary>
		/// The color of the inactive areas.
		/// </summary>
		private Color yaInactiveColor;

		/// <summary>
		/// The brush used to color the inactive-colored area.
		/// </summary>
		private Brush yaInactiveBrush;

		/// <summary>
		/// The pen used to draw the highlight lines.
		/// </summary>
		private Pen yaHighlightPen;

		/// <summary>
		/// The pen used to draw the shadow lines.
		/// </summary>
		private Pen yaShadowPen;

		/// <summary>
		/// The pen used to draw the border.
		/// </summary>
		private Pen yaBorderPen;

		/// <summary>
		/// The index of the selected tab.
		/// </summary>
		private int yaSelectedIndex;

		/// <summary>
		/// The currently selected tab.
		/// </summary>
		private YaTabPage yaSelectedTab;

		/// <summary>
		/// The side on which the tabs get docked.
		/// </summary>
		private DockStyle yaTabDock;

		/// <summary>
		/// The rectangle in which the tabs get drawn.
		/// </summary>
		private Rectangle yaTabsRectangle;

		/// <summary>
		/// The rectangle in which the client gets drawn.
		/// </summary>
		private Rectangle yaClientRectangle;

		/// <summary>
		/// The rectangle in which the currently selected
		/// <see cref="YaTabPage"/> gets drawn oriented as
		/// if the tabs were docked to the top of the control.
		/// </summary>
		private Rectangle yaDisplayRectangle;

		/// <summary>
		/// The rectangle transformed for the <see cref="DisplayRectangle"/>
		/// property to return.
		/// </summary>
		private Rectangle yaTransformedDisplayRectangle;

		/// <summary>
		/// The height used to calculate the rectangles.
		/// </summary>
		private int calcHeight;

		/// <summary>
		/// The width used to calculate the rectangles.
		/// </summary>
		private int calcWidth;

		/// <summary>
		/// The regular font used to draw the strings in the tabs.
		/// </summary>
		private Font yaTabFont;

		/// <summary>
		/// The bold font used to draw the strings in the active tab.
		/// </summary>
		private Font yaBoldTabFont;

		/// <summary>
		/// The <see cref="YaTabDrawer"/> used to draw the
		/// tabs.
		/// </summary>
		private YaTabDrawer yaTabDrawer;

		/// <summary>
		/// Used to monitor the text changing of a <see cref="YaTabPage" />.
		/// </summary>
		private EventHandler ChildTextChangeEventHandler;

		/// <summary>
		/// Used to monitor if a person has elected to scroll the tabs.
		/// </summary>
		private bool yaKeepScrolling;

		#endregion

		#region Private Inner Classes

		/// <summary>
		/// Let's the tabs scroll.
		/// </summary>
		private class ScrollerThread
		{
			/// <summary>
			/// Creates a new instance of the
			/// <see cref="YaTabControl.ScrollerThread"/> class.
			/// </summary>
			/// <param name="amount">The amount to scroll.</param>
			/// <param name="control">The control to scroll.</param>
			public ScrollerThread( int amount, YaTabControl control )
			{
				this.tabControl = control;
				this.amount = new object[] { amount };
				scroller = new ScrollTabsDelegate( tabControl.ScrollTabs );
			}

			/// <summary>
			/// Scrolls the tabs on the <see cref="YaTabControl"/>
			/// by the given amount.
			/// </summary>
			public void ScrollIt()
			{
				bool keepScrolling = false;
				lock( tabControl )
				{
					keepScrolling = tabControl.yaKeepScrolling;
				}
				while( keepScrolling )
				{
					
					tabControl.Invoke( scroller, amount );
					lock( tabControl )
					{
						keepScrolling = tabControl.yaKeepScrolling;
					}
				}
			}

			/// <summary>
			/// The control to scroll.
			/// </summary>
			private YaTabControl tabControl;

			/// <summary>
			/// The amount to scroll.
			/// </summary>
			private object[] amount;

			/// <summary>
			/// A delegate to scroll the tabs.
			/// </summary>
			private ScrollTabsDelegate scroller;

			/// <summary>
			/// A delegate to use in scrolling the tabs.
			/// </summary>
			private delegate void ScrollTabsDelegate( int amount );
		}

		#endregion

		#region Public Inner Classes

		/// <summary>
		/// A <see cref="YaTabControl"/>-specific
		/// <see cref="Control.ControlCollection"/>.
		/// </summary>
		public new class ControlCollection : Control.ControlCollection
		{
			/// <summary>
			/// Creates a new instance of the
			/// <see cref="YaTabControl.ControlCollection"/> class with 
			/// the specified <i>owner</i>.
			/// </summary>
			/// <param name="owner">
			/// The <see cref="YaTabControl"/> that owns this collection.
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// Thrown if <i>owner</i> is <b>null</b>.
			/// </exception>
			/// <exception cref="ArgumentException">
			/// Thrown if <i>owner</i> is not a <see cref="YaTabControl"/>.
			/// </exception>
			public ControlCollection( Control owner ) : base( owner )
			{
				if( owner == null )
				{
					throw new ArgumentNullException( "owner", "Tried to create a YaTabControl.ControlCollection with a null owner." );
				}
				this.owner = owner as YaTabControl;
				if( this.owner == null )
				{
					throw new ArgumentException( "Tried to create a YaTabControl.ControlCollection with a non-YaTabControl owner.", "owner" );
				}
				monitor = new EventHandler( this.owner.ChildTabTextChanged );
			}

			/// <summary>
			/// Overridden. Adds a <see cref="Control"/> to the
			/// <see cref="YaTabControl"/>.
			/// </summary>
			/// <param name="value">
			/// The <see cref="Control"/> to add, which must be a
			/// <see cref="YaTabPage"/>.
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// Thrown if <i>value</i> is <b>null</b>.
			/// </exception>
			/// <exception cref="ArgumentException">
			/// Thrown if <i>value</i> is not a <see cref="YaTabPage"/>.
			/// </exception>
			public override void Add( Control value )
			{
				if( value == null )
				{
					throw new ArgumentNullException( "value", "Tried to add a null value to the YaTabControl.ControlCollection." );
				}
				YaTabPage p = value as YaTabPage;
				if( p == null )
				{
					throw new ArgumentException( "Tried to add a non-YaTabPage control to the YaTabControl.ControlCollection.", "value" );
				}
				p.SendToBack();
				base.Add( p );
				p.TextChanged += monitor;
			}

			/// <summary>
			/// Overridden. Inherited from <see cref="Control.ControlCollection.Remove( Control )"/>.
			/// </summary>
			/// <param name="value"></param>
			public override void Remove( Control value )
			{
				value.TextChanged -= monitor;
				base.Remove( value );
			}

			/// <summary>
			/// Overridden. Inherited from <see cref="Control.ControlCollection.Clear()"/>.
			/// </summary>
			public override void Clear()
			{
				foreach( Control c in this )
				{
					c.TextChanged -= monitor;
				}
				base.Clear();
			}

			/// <summary>
			/// The owner of this <see cref="YaTabControl.ControlCollection"/>.
			/// </summary>
			private YaTabControl owner;

			private EventHandler monitor;
		}

		#endregion
	}
}
