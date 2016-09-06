using GrayIris.Utilities.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example
{
    public partial class YatcForm : Form
    {
        public YatcForm()
        {
            InitializeComponent();
            _tabs.TabChanging += HandleChangingTab;
        }

        private void ChangeToExcel(object sender, EventArgs e)
        {
            _tabs.TabDrawer = _xlTabDrawer;
            _drawExcel.Checked = true;
            _drawOvals.Checked = false;
            _drawVisualStudio.Checked = false;
        }

        private void ChangeToOvals(object sender, EventArgs e)
        {
            _tabs.TabDrawer = _ovalTabDrawer;
            _drawExcel.Checked = false;
            _drawOvals.Checked = true;
            _drawVisualStudio.Checked = false;
        }

        private void ChangeToVisualStudio(object sender, EventArgs e)
        {
            _tabs.TabDrawer = _vsTabDrawer;
            _drawExcel.Checked = false;
            _drawOvals.Checked = false;
            _drawVisualStudio.Checked = true;
        }

        private void ChangeDockState(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if(menuItem == null)
            {
                return;
            }
            _dockBottom.Checked = sender == _dockBottom;
            _dockFill.Checked = sender == _dockFill;
            _dockLeft.Checked = sender == _dockLeft;
            _dockNone.Checked = sender == _dockNone;
            _dockRight.Checked = sender == _dockRight;
            _dockTop.Checked = sender == _dockTop;
            _tabs.Dock = (DockStyle)Enum.Parse(typeof(DockStyle), menuItem.Text);
        }

        private void ChangeTabLocation(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem == null)
            {
                return;
            }
            _dockTabsLeft.Checked = sender == _dockTabsLeft;
            _dockTabsRight.Checked = sender == _dockTabsRight;
            _dockTabsTop.Checked = sender == _dockTabsTop;
            _dockTabsBottom.Checked = sender == _dockTabsBottom;
            _tabs.TabDock = (DockStyle)Enum.Parse(typeof(DockStyle), menuItem.Text);
        }

        private void ChangeSelectingCapability(object sender, EventArgs e)
        {
            _preventSelection.Checked = !_preventSelection.Checked;
        }

        private void HandleChangingTab(object sender, TabChangingEventArgs args)
        {
            args.Cancel = args.NewIndex == 1 && _preventSelection.Checked;
        }

        private void _tabs_NewTabButtonClicked(object sender, NewTabEventArgs e)
        {
            //_tabs.SelectedTab = e.NewTab;
        }

    }
}
