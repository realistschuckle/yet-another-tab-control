using System;
using System.Collections.Generic;
using System.Text;

namespace GrayIris.Utilities.UI.Controls
{
    /// <summary>
    /// A class to contain the information regarding the
    /// <see cref="YaTabControl.TabClosing"/> event.
    /// </summary>
    public class TabClosingEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }
}
