﻿namespace XifanPet
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.宠物ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.穿透ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.恢复ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.吃ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.洗ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关闭ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStripIcon;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "宠物";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStripIcon
            // 
            this.contextMenuStripIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.宠物ToolStripMenuItem,
            this.显示ToolStripMenuItem,
            this.穿透ToolStripMenuItem,
            this.恢复ToolStripMenuItem,
            this.吃ToolStripMenuItem,
            this.洗ToolStripMenuItem,
            this.关闭ToolStripMenuItem});
            this.contextMenuStripIcon.Name = "contextMenuStripIcon";
            this.contextMenuStripIcon.Size = new System.Drawing.Size(181, 180);
            this.contextMenuStripIcon.Tag = "through";
            // 
            // 宠物ToolStripMenuItem
            // 
            this.宠物ToolStripMenuItem.Name = "宠物ToolStripMenuItem";
            this.宠物ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.宠物ToolStripMenuItem.Text = "宠物";
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.Checked = true;
            this.显示ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.显示ToolStripMenuItem.Text = "显示";
            // 
            // 穿透ToolStripMenuItem
            // 
            this.穿透ToolStripMenuItem.Checked = true;
            this.穿透ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.穿透ToolStripMenuItem.Name = "穿透ToolStripMenuItem";
            this.穿透ToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.穿透ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.穿透ToolStripMenuItem.Text = "穿透";
            this.穿透ToolStripMenuItem.Click += new System.EventHandler(this.穿透ToolStripMenuItem_Click);
            // 
            // 恢复ToolStripMenuItem
            // 
            this.恢复ToolStripMenuItem.Name = "恢复ToolStripMenuItem";
            this.恢复ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.恢复ToolStripMenuItem.Text = "恢复";
            this.恢复ToolStripMenuItem.Click += new System.EventHandler(this.恢复ToolStripMenuItem_Click);
            // 
            // 吃ToolStripMenuItem
            // 
            this.吃ToolStripMenuItem.Name = "吃ToolStripMenuItem";
            this.吃ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.吃ToolStripMenuItem.Text = "吃";
            this.吃ToolStripMenuItem.Click += new System.EventHandler(this.吃ToolStripMenuItem_Click);
            // 
            // 洗ToolStripMenuItem
            // 
            this.洗ToolStripMenuItem.Name = "洗ToolStripMenuItem";
            this.洗ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.洗ToolStripMenuItem.Text = "洗";
            this.洗ToolStripMenuItem.Click += new System.EventHandler(this.洗ToolStripMenuItem_Click);
            // 
            // 关闭ToolStripMenuItem
            // 
            this.关闭ToolStripMenuItem.Name = "关闭ToolStripMenuItem";
            this.关闭ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.关闭ToolStripMenuItem.Text = "退出";
            this.关闭ToolStripMenuItem.Click += new System.EventHandler(this.关闭ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 113);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "FishForm";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FishForm_FormClosing);
            this.Load += new System.EventHandler(this.FishForm_Load);
            this.contextMenuStripIcon.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripIcon;
        private System.Windows.Forms.ToolStripMenuItem 关闭ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 穿透ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 恢复ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 吃ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 洗ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 宠物ToolStripMenuItem;
    }
}