using System;

namespace GrayIris.Utilities.UI.Controls
{
	/// <summary>
	/// The delegate to use for the <see cref="YaTabControl.TabChanging"/> event.
	/// </summary>
	public delegate void TabChangingEventHandler( object sender, TabChangingEventArgs tcea );

    /// <summary>
    /// The delegate to use for the <see cref="YaTabControl.NewTabButtonClick"/> event.
    /// </summary>
    public delegate void NewTabEventHandler(object sender, NewTabEventArgs e);
}
