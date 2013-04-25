using System;

namespace GrayIris.Utilities.UI.Controls
{
	/// <summary>
	/// Contains styles used to control the scroll buttons
	/// for a <see cref="YaTabControl"/>.
	/// </summary>
	public enum YaScrollButtonStyle
	{
		/// <summary>
		/// Indicates that the scroll buttons should get drawn
		/// regardless of whether the tabs extend beyond the
		/// visual tab area.
		/// </summary>
		Always,

		/// <summary>
		/// Indicates that the scroll buttons should get drawn
		/// only when the tabs extend beyond the visible span
		/// of the tab rectangle.
		/// </summary>
		Auto,

		/// <summary>
		/// Indicates that the scroll buttons should never get
		/// drawn.
		/// </summary>
		Never
	}
}
