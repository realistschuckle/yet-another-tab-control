using System;
using System.Collections.Generic;
using System.Text;

namespace GrayIris.Utilities.UI.Controls
{

    /// <summary>
    /// A class to contain the information regarding the
    /// <see cref="YaTabControl.NewTab"/> event.
    /// </summary>
    public class NewTabEventArgs : EventArgs
    {
        public NewTabEventArgs(YaTabPage newTab)
        {
            NewTab = newTab;
        }

        public YaTabPage NewTab { get; set; }
    }
}
