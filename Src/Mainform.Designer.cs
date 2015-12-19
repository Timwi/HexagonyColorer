namespace HexagonyColorer
{
    partial class Mainform
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainform));
            this.ctSplit = new RT.Util.Controls.SplitContainerEx();
            this.ctScroll = new System.Windows.Forms.Panel();
            this.ctImage = new RT.Util.Controls.DoubleBufferedPanel();
            this.lstPaths = new System.Windows.Forms.ListBox();
            this.ctMainToolStrip = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miNewFromSource = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.miSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.miRevert = new System.Windows.Forms.ToolStripMenuItem();
            this.miRefreshSource = new System.Windows.Forms.ToolStripMenuItem();
            this.miSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.miExport = new System.Windows.Forms.ToolStripMenuItem();
            this.miSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miStartPath = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetPathStartPos = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.ctSplit)).BeginInit();
            this.ctSplit.Panel1.SuspendLayout();
            this.ctSplit.Panel2.SuspendLayout();
            this.ctSplit.SuspendLayout();
            this.ctScroll.SuspendLayout();
            this.ctMainToolStrip.SuspendLayout();
            this.mnuContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctSplit
            // 
            this.ctSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctSplit.Location = new System.Drawing.Point(0, 24);
            this.ctSplit.Name = "ctSplit";
            this.ctSplit.PaintSplitter = true;
            // 
            // ctSplit.Panel1
            // 
            this.ctSplit.Panel1.Controls.Add(this.ctScroll);
            // 
            // ctSplit.Panel2
            // 
            this.ctSplit.Panel2.Controls.Add(this.lstPaths);
            this.ctSplit.Size = new System.Drawing.Size(1099, 649);
            this.ctSplit.SplitterDistance = 683;
            this.ctSplit.TabIndex = 0;
            // 
            // ctScroll
            // 
            this.ctScroll.AutoScroll = true;
            this.ctScroll.Controls.Add(this.ctImage);
            this.ctScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctScroll.Location = new System.Drawing.Point(0, 0);
            this.ctScroll.Name = "ctScroll";
            this.ctScroll.Size = new System.Drawing.Size(683, 649);
            this.ctScroll.TabIndex = 0;
            // 
            // ctImage
            // 
            this.ctImage.Location = new System.Drawing.Point(0, 0);
            this.ctImage.Name = "ctImage";
            this.ctImage.Size = new System.Drawing.Size(200, 100);
            this.ctImage.TabIndex = 0;
            this.ctImage.PaintBuffer += new System.Windows.Forms.PaintEventHandler(this.paintImage);
            this.ctImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseDown);
            // 
            // lstPaths
            // 
            this.lstPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPaths.FormattingEnabled = true;
            this.lstPaths.IntegralHeight = false;
            this.lstPaths.Location = new System.Drawing.Point(0, 0);
            this.lstPaths.Name = "lstPaths";
            this.lstPaths.Size = new System.Drawing.Size(412, 649);
            this.lstPaths.TabIndex = 0;
            this.lstPaths.SelectedIndexChanged += new System.EventHandler(this.selectPath);
            this.lstPaths.DoubleClick += new System.EventHandler(this.editPath);
            this.lstPaths.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstKeyDown);
            // 
            // ctMainToolStrip
            // 
            this.ctMainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.editToolStripMenuItem});
            this.ctMainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ctMainToolStrip.Name = "ctMainToolStrip";
            this.ctMainToolStrip.Size = new System.Drawing.Size(1099, 24);
            this.ctMainToolStrip.TabIndex = 1;
            this.ctMainToolStrip.Text = "toolStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNewFromSource,
            this.miOpen,
            this.miSave,
            this.miSaveAs,
            this.miRevert,
            this.miRefreshSource,
            this.miSep2,
            this.miExport,
            this.miSep1,
            this.miExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // miNewFromSource
            // 
            this.miNewFromSource.Name = "miNewFromSource";
            this.miNewFromSource.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.miNewFromSource.Size = new System.Drawing.Size(268, 22);
            this.miNewFromSource.Text = "Ope&n Hexagony source file...";
            this.miNewFromSource.Click += new System.EventHandler(this.newFile);
            // 
            // miOpen
            // 
            this.miOpen.Name = "miOpen";
            this.miOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.miOpen.Size = new System.Drawing.Size(268, 22);
            this.miOpen.Text = "&Open...";
            this.miOpen.Click += new System.EventHandler(this.open);
            // 
            // miSave
            // 
            this.miSave.Name = "miSave";
            this.miSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.miSave.Size = new System.Drawing.Size(268, 22);
            this.miSave.Text = "&Save";
            this.miSave.Click += new System.EventHandler(this.save);
            // 
            // miSaveAs
            // 
            this.miSaveAs.Name = "miSaveAs";
            this.miSaveAs.Size = new System.Drawing.Size(268, 22);
            this.miSaveAs.Text = "Save &as...";
            this.miSaveAs.Click += new System.EventHandler(this.saveAs);
            // 
            // miRevert
            // 
            this.miRevert.Name = "miRevert";
            this.miRevert.Size = new System.Drawing.Size(268, 22);
            this.miRevert.Text = "&Revert";
            this.miRevert.Click += new System.EventHandler(this.revert);
            // 
            // miRefreshSource
            // 
            this.miRefreshSource.Name = "miRefreshSource";
            this.miRefreshSource.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.miRefreshSource.Size = new System.Drawing.Size(268, 22);
            this.miRefreshSource.Text = "Re&fresh source";
            this.miRefreshSource.Click += new System.EventHandler(this.refreshSource);
            // 
            // miSep2
            // 
            this.miSep2.Name = "miSep2";
            this.miSep2.Size = new System.Drawing.Size(265, 6);
            // 
            // miExport
            // 
            this.miExport.Name = "miExport";
            this.miExport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.miExport.Size = new System.Drawing.Size(268, 22);
            this.miExport.Text = "&Export as PNG...";
            this.miExport.Click += new System.EventHandler(this.export);
            // 
            // miSep1
            // 
            this.miSep1.Name = "miSep1";
            this.miSep1.Size = new System.Drawing.Size(265, 6);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(268, 22);
            this.miExit.Text = "E&xit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deletePathToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // deletePathToolStripMenuItem
            // 
            this.deletePathToolStripMenuItem.Name = "deletePathToolStripMenuItem";
            this.deletePathToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deletePathToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.deletePathToolStripMenuItem.Text = "&Delete path";
            this.deletePathToolStripMenuItem.Click += new System.EventHandler(this.deletePath);
            // 
            // mnuContext
            // 
            this.mnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miStartPath,
            this.miSetPathStartPos});
            this.mnuContext.Name = "mnuContext";
            this.mnuContext.Size = new System.Drawing.Size(296, 48);
            // 
            // miStartPath
            // 
            this.miStartPath.Name = "miStartPath";
            this.miStartPath.Size = new System.Drawing.Size(295, 22);
            this.miStartPath.Text = "&Start new path here";
            this.miStartPath.Click += new System.EventHandler(this.startPath);
            // 
            // miSetPathStartPos
            // 
            this.miSetPathStartPos.Name = "miSetPathStartPos";
            this.miSetPathStartPos.Size = new System.Drawing.Size(295, 22);
            this.miSetPathStartPos.Text = "Set &current path’s starting position to here";
            this.miSetPathStartPos.Click += new System.EventHandler(this.setStartPos);
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 673);
            this.Controls.Add(this.ctSplit);
            this.Controls.Add(this.ctMainToolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Mainform";
            this.Text = "Hexagony Colorizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.exiting);
            this.ctSplit.Panel1.ResumeLayout(false);
            this.ctSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ctSplit)).EndInit();
            this.ctSplit.ResumeLayout(false);
            this.ctScroll.ResumeLayout(false);
            this.ctMainToolStrip.ResumeLayout(false);
            this.ctMainToolStrip.PerformLayout();
            this.mnuContext.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RT.Util.Controls.SplitContainerEx ctSplit;
        private System.Windows.Forms.Panel ctScroll;
        private System.Windows.Forms.ListBox lstPaths;
        private System.Windows.Forms.MenuStrip ctMainToolStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem miNewFromSource;
        private System.Windows.Forms.ToolStripMenuItem miOpen;
        private System.Windows.Forms.ToolStripMenuItem miSave;
        private System.Windows.Forms.ToolStripMenuItem miSaveAs;
        private System.Windows.Forms.ToolStripMenuItem miRevert;
        private System.Windows.Forms.ToolStripSeparator miSep1;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private RT.Util.Controls.DoubleBufferedPanel ctImage;
        private System.Windows.Forms.ContextMenuStrip mnuContext;
        private System.Windows.Forms.ToolStripMenuItem miStartPath;
        private System.Windows.Forms.ToolStripMenuItem miSetPathStartPos;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deletePathToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator miSep2;
        private System.Windows.Forms.ToolStripMenuItem miExport;
        private System.Windows.Forms.ToolStripMenuItem miRefreshSource;
    }
}

