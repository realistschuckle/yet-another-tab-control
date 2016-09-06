using System;

namespace GrayIris.Utilities.UI.Controls
{
	/// <summary>
	/// The delegate to use for the <see cref="YaTabControl.TabChanging"/> event.
	/// </summary>
	public delegate void TabChangingEventHandler( object sender, TabChangingEventArgs tcea );

    /// <summary>
    /// The delegate to use for the <see cref="YaTabControl.NewTabButtonClicked"/> event.
    /// </summary>
    public delegate void NewTabEventHandler(object sender, NewTabEventArgs e);

    /// <summary>
    /// The delegate to user for the <see cref="YaTabControl.TabClosing" /> event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TabClosedEventHandler(object sender, EventArgs e);
}
