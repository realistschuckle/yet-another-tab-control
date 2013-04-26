using System;

namespace GrayIris.Utilities.UI.Controls
{
	/// <summary>
	/// A class to contain the information regarding the
	/// <see cref="YaTabControl.TabChanging"/> event.
	/// </summary>
	public class TabChangingEventArgs : EventArgs
	{
		/// <summary>
		/// Creates an instance of the <see cref="TabChangingEventArgs"/>
		/// class with the specified indices.
		/// </summary>
		/// <param name="currentIndex">The current selected index of the <see cref="YaTabControl"/>.</param>
		/// <param name="newIndex">The value to which the selected index changes.</param>
		public TabChangingEventArgs( int currentIndex, int newIndex )
		{
			this.cIndex = currentIndex;
			this.nIndex = newIndex;
		}

		/// <summary>
		/// Gets the value of the <see cref="YaTabControl.SelectedIndex"/> for the
		/// <see cref="YaTabControl"/> from which this event got generated.
		/// </summary>
		public int CurrentIndex
		{
			get
			{
				return cIndex;
			}
		}

		/// <summary>
		/// Gets the value to which the <see cref="YaTabControl.SelectedIndex"/>
		/// will change.
		/// </summary>
		public int NewIndex
		{
			get
			{
				return nIndex;
			}
		}

		/// <summary>
		/// Gets and sets a value indicating whether the <see cref="YaTabControl"/>'s
		/// <see cref="YaTabControl.SelectedIndex"/> should change.
		/// </summary>
		public bool Cancel
		{
			get
			{
				return cancelEvent;
			}
			set
			{
				cancelEvent = value;
			}
		}

		/// <summary>
		/// A boolean value to indicate if the event should get cancelled.
		/// </summary>
		private bool cancelEvent;

		/// <summary>
		/// The current index to report in the event.
		/// </summary>
		private int cIndex;

		/// <summary>
		/// The index to which the tab control changes.
		/// </summary>
		private int nIndex;
	}
}
