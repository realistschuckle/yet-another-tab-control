namespace Example
{
    partial class YatcForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripMenuItem tabDrawerToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem dockStyleToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem tabLocationToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem preventToolStripMenuItem;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YatcForm));
            this._drawExcel = new System.Windows.Forms.ToolStripMenuItem();
            this._drawOvals = new System.Windows.Forms.ToolStripMenuItem();
            this._drawVisualStudio = new System.Windows.Forms.ToolStripMenuItem();
            this._dockNone = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._dockFill = new System.Windows.Forms.ToolStripMenuItem();
            this._dockLeft = new System.Windows.Forms.ToolStripMenuItem();
            this._dockTop = new System.Windows.Forms.ToolStripMenuItem();
            this._dockRight = new System.Windows.Forms.ToolStripMenuItem();
            this._dockBottom = new System.Windows.Forms.ToolStripMenuItem();
            this._dockTabsTop = new System.Windows.Forms.ToolStripMenuItem();
            this._dockTabsRight = new System.Windows.Forms.ToolStripMenuItem();
            this._dockTabsBottom = new System.Windows.Forms.ToolStripMenuItem();
            this._dockTabsLeft = new System.Windows.Forms.ToolStripMenuItem();
            this._preventSelection = new System.Windows.Forms.ToolStripMenuItem();
            this._ovalTabDrawer = new GrayIris.Utilities.UI.Controls.OvalTabDrawer();
            this._vsTabDrawer = new GrayIris.Utilities.UI.Controls.VsTabDrawer();
            this._menu = new System.Windows.Forms.MenuStrip();
            this._tabs = new GrayIris.Utilities.UI.Controls.YaTabControl();
            this._tabpage1 = new GrayIris.Utilities.UI.Controls.YaTabPage();
            this._tabpage2 = new GrayIris.Utilities.UI.Controls.YaTabPage();
            this._tabpage3 = new GrayIris.Utilities.UI.Controls.YaTabPage();
            this._images = new System.Windows.Forms.ImageList(this.components);
            this._xlTabDrawer = new GrayIris.Utilities.UI.Controls.XlTabDrawer();
            tabDrawerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            dockStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tabLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            preventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._menu.SuspendLayout();
            this._tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabDrawerToolStripMenuItem
            // 
            tabDrawerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._drawExcel,
            this._drawOvals,
            this._drawVisualStudio});
            tabDrawerToolStripMenuItem.Name = "tabDrawerToolStripMenuItem";
            tabDrawerToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            tabDrawerToolStripMenuItem.Text = "Tab Drawer";
            // 
            // _drawExcel
            // 
            this._drawExcel.Checked = true;
            this._drawExcel.CheckState = System.Windows.Forms.CheckState.Checked;
            this._drawExcel.Name = "_drawExcel";
            this._drawExcel.Size = new System.Drawing.Size(142, 22);
            this._drawExcel.Text = "Excel";
            this._drawExcel.Click += new System.EventHandler(this.ChangeToExcel);
            // 
            // _drawOvals
            // 
            this._drawOvals.Name = "_drawOvals";
            this._drawOvals.Size = new System.Drawing.Size(142, 22);
            this._drawOvals.Text = "Ovals";
            this._drawOvals.Click += new System.EventHandler(this.ChangeToOvals);
            // 
            // _drawVisualStudio
            // 
            this._drawVisualStudio.Name = "_drawVisualStudio";
            this._drawVisualStudio.Size = new System.Drawing.Size(142, 22);
            this._drawVisualStudio.Text = "Visual Studio";
            this._drawVisualStudio.Click += new System.EventHandler(this.ChangeToVisualStudio);
            // 
            // dockStyleToolStripMenuItem
            // 
            dockStyleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._dockNone,
            this.toolStripSeparator1,
            this._dockFill,
            this._dockLeft,
            this._dockTop,
            this._dockRight,
            this._dockBottom});
            dockStyleToolStripMenuItem.Name = "dockStyleToolStripMenuItem";
            dockStyleToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            dockStyleToolStripMenuItem.Text = "Dock Style";
            // 
            // _dockNone
            // 
            this._dockNone.Checked = true;
            this._dockNone.CheckState = System.Windows.Forms.CheckState.Checked;
            this._dockNone.Name = "_dockNone";
            this._dockNone.Size = new System.Drawing.Size(114, 22);
            this._dockNone.Text = "None";
            this._dockNone.Click += new System.EventHandler(this.ChangeDockState);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(111, 6);
            // 
            // _dockFill
            // 
            this._dockFill.Name = "_dockFill";
            this._dockFill.Size = new System.Drawing.Size(114, 22);
            this._dockFill.Text = "Fill";
            this._dockFill.Click += new System.EventHandler(this.ChangeDockState);
            // 
            // _dockLeft
            // 
            this._dockLeft.Name = "_dockLeft";
            this._dockLeft.Size = new System.Drawing.Size(114, 22);
            this._dockLeft.Text = "Left";
            this._dockLeft.Click += new System.EventHandler(this.ChangeDockState);
            // 
            // _dockTop
            // 
            this._dockTop.Name = "_dockTop";
            this._dockTop.Size = new System.Drawing.Size(114, 22);
            this._dockTop.Text = "Top";
            this._dockTop.Click += new System.EventHandler(this.ChangeDockState);
            // 
            // _dockRight
            // 
            this._dockRight.Name = "_dockRight";
            this._dockRight.Size = new System.Drawing.Size(114, 22);
            this._dockRight.Text = "Right";
            this._dockRight.Click += new System.EventHandler(this.ChangeDockState);
            // 
            // _dockBottom
            // 
            this._dockBottom.Name = "_dockBottom";
            this._dockBottom.Size = new System.Drawing.Size(114, 22);
            this._dockBottom.Text = "Bottom";
            this._dockBottom.Click += new System.EventHandler(this.ChangeDockState);
            // 
            // tabLocationToolStripMenuItem
            // 
            tabLocationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._dockTabsTop,
            this._dockTabsRight,
            this._dockTabsBottom,
            this._dockTabsLeft});
            tabLocationToolStripMenuItem.Name = "tabLocationToolStripMenuItem";
            tabLocationToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            tabLocationToolStripMenuItem.Text = "Tab Location";
            // 
            // _dockTabsTop
            // 
            this._dockTabsTop.Checked = true;
            this._dockTabsTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this._dockTabsTop.Name = "_dockTabsTop";
            this._dockTabsTop.Size = new System.Drawing.Size(114, 22);
            this._dockTabsTop.Text = "Top";
            this._dockTabsTop.Click += new System.EventHandler(this.ChangeTabLocation);
            // 
            // _dockTabsRight
            // 
            this._dockTabsRight.Name = "_dockTabsRight";
            this._dockTabsRight.Size = new System.Drawing.Size(114, 22);
            this._dockTabsRight.Text = "Right";
            this._dockTabsRight.Click += new System.EventHandler(this.ChangeTabLocation);
            // 
            // _dockTabsBottom
            // 
            this._dockTabsBottom.Name = "_dockTabsBottom";
            this._dockTabsBottom.Size = new System.Drawing.Size(114, 22);
            this._dockTabsBottom.Text = "Bottom";
            this._dockTabsBottom.Click += new System.EventHandler(this.ChangeTabLocation);
            // 
            // _dockTabsLeft
            // 
            this._dockTabsLeft.Name = "_dockTabsLeft";
            this._dockTabsLeft.Size = new System.Drawing.Size(114, 22);
            this._dockTabsLeft.Text = "Left";
            this._dockTabsLeft.Click += new System.EventHandler(this.ChangeTabLocation);
            // 
            // preventToolStripMenuItem
            // 
            preventToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._preventSelection});
            preventToolStripMenuItem.Name = "preventToolStripMenuItem";
            preventToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            preventToolStripMenuItem.Text = "Prevent";
            // 
            // _preventSelection
            // 
            this._preventSelection.Name = "_preventSelection";
            this._preventSelection.Size = new System.Drawing.Size(197, 22);
            this._preventSelection.Text = "Selecting \"Second Tab\"";
            this._preventSelection.Click += new System.EventHandler(this.ChangeSelectingCapability);
            // 
            // _menu
            // 
            this._menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tabDrawerToolStripMenuItem,
            dockStyleToolStripMenuItem,
            tabLocationToolStripMenuItem,
            preventToolStripMenuItem});
            this._menu.Location = new System.Drawing.Point(0, 0);
            this._menu.Name = "_menu";
            this._menu.Size = new System.Drawing.Size(838, 24);
            this._menu.TabIndex = 1;
            this._menu.Text = "menuStrip1";
            // 
            // _tabs
            // 
            this._tabs.ActiveColor = System.Drawing.SystemColors.Control;
            this._tabs.BackColor = System.Drawing.SystemColors.Control;
            this._tabs.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this._tabs.Controls.Add(this._tabpage1);
            this._tabs.Controls.Add(this._tabpage2);
            this._tabs.Controls.Add(this._tabpage3);
            this._tabs.HoverColor = System.Drawing.Color.Tan;
            this._tabs.ImageIndex = 0;
            this._tabs.ImageList = this._images;
            this._tabs.InactiveColor = System.Drawing.SystemColors.Window;
            this._tabs.Location = new System.Drawing.Point(224, 107);
            this._tabs.Name = "_tabs";
            this._tabs.OverIndex = -1;
            this._tabs.ScrollButtonStyle = GrayIris.Utilities.UI.Controls.YaScrollButtonStyle.Auto;
            this._tabs.SelectedIndex = 0;
            this._tabs.SelectedTab = this._tabpage1;
            this._tabs.Size = new System.Drawing.Size(390, 300);
            this._tabs.TabDock = System.Windows.Forms.DockStyle.Top;
            this._tabs.TabDrawer = this._xlTabDrawer;
            this._tabs.TabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tabs.TabIndex = 2;
            // 
            // _tabpage1
            // 
            this._tabpage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabpage1.ImageIndex = -1;
            this._tabpage1.Location = new System.Drawing.Point(4, 31);
            this._tabpage1.Name = "_tabpage1";
            this._tabpage1.Size = new System.Drawing.Size(382, 265);
            this._tabpage1.TabIndex = 0;
            this._tabpage1.Text = "First Tab";
            // 
            // _tabpage2
            // 
            this._tabpage2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabpage2.ImageIndex = 1;
            this._tabpage2.Location = new System.Drawing.Point(4, 30);
            this._tabpage2.Name = "_tabpage2";
            this._tabpage2.Size = new System.Drawing.Size(292, 266);
            this._tabpage2.TabIndex = 1;
            this._tabpage2.Text = "Second Tab";
            // 
            // _tabpage3
            // 
            this._tabpage3.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabpage3.ImageIndex = 2;
            this._tabpage3.Location = new System.Drawing.Point(4, 30);
            this._tabpage3.Name = "_tabpage3";
            this._tabpage3.Size = new System.Drawing.Size(292, 266);
            this._tabpage3.TabIndex = 2;
            this._tabpage3.Text = "Third Tab";
            // 
            // _images
            // 
            this._images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_images.ImageStream")));
            this._images.TransparentColor = System.Drawing.Color.Transparent;
            this._images.Images.SetKeyName(0, "fork.png");
            this._images.Images.SetKeyName(1, "online_support.png");
            this._images.Images.SetKeyName(2, "weather.png");
            // 
            // YatcForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 515);
            this.Controls.Add(this._tabs);
            this.Controls.Add(this._menu);
            this.MainMenuStrip = this._menu;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YatcForm";
            this.ShowIcon = false;
            this.Text = "Yet Another Tab Control Example Form";
            this._menu.ResumeLayout(false);
            this._menu.PerformLayout();
            this._tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GrayIris.Utilities.UI.Controls.OvalTabDrawer _ovalTabDrawer;
        private GrayIris.Utilities.UI.Controls.VsTabDrawer _vsTabDrawer;
        private System.Windows.Forms.MenuStrip _menu;
        private System.Windows.Forms.ToolStripMenuItem _drawOvals;
        private System.Windows.Forms.ToolStripMenuItem _drawVisualStudio;
        private System.Windows.Forms.ToolStripMenuItem _drawExcel;
        private GrayIris.Utilities.UI.Controls.YaTabControl _tabs;
        private GrayIris.Utilities.UI.Controls.XlTabDrawer _xlTabDrawer;
        private GrayIris.Utilities.UI.Controls.YaTabPage _tabpage1;
        private GrayIris.Utilities.UI.Controls.YaTabPage _tabpage2;
        private GrayIris.Utilities.UI.Controls.YaTabPage _tabpage3;
        private System.Windows.Forms.ImageList _images;
        private System.Windows.Forms.ToolStripMenuItem _dockNone;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _dockFill;
        private System.Windows.Forms.ToolStripMenuItem _dockLeft;
        private System.Windows.Forms.ToolStripMenuItem _dockTop;
        private System.Windows.Forms.ToolStripMenuItem _dockRight;
        private System.Windows.Forms.ToolStripMenuItem _dockBottom;
        private System.Windows.Forms.ToolStripMenuItem _dockTabsTop;
        private System.Windows.Forms.ToolStripMenuItem _dockTabsRight;
        private System.Windows.Forms.ToolStripMenuItem _dockTabsBottom;
        private System.Windows.Forms.ToolStripMenuItem _dockTabsLeft;
        private System.Windows.Forms.ToolStripMenuItem _preventSelection;
    }
}

