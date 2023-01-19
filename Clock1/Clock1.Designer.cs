namespace Clock1
{
    partial class Clock1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelN = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(586, 366);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // statusStrip1
            // 
            this.statusStrip1.GripMargin = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.statusStrip1.Location = new System.Drawing.Point(0, 366);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(743, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // panelBottom
            // 
            this.panelBottom.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.panelBottom.Location = new System.Drawing.Point(0, 385);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(743, 3);
            this.panelBottom.TabIndex = 2;
            this.panelBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelBottom_MouseDown);
            this.panelBottom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelBottom_MouseMove);
            this.panelBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelBottom_MouseUp);
            // 
            // panelRight
            // 
            this.panelRight.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.panelRight.Location = new System.Drawing.Point(740, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(3, 388);
            this.panelRight.TabIndex = 3;
            this.panelRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelRight_MouseDown);
            this.panelRight.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelRight_MouseMove);
            this.panelRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelRight_MouseUp);
            // 
            // panelN
            // 
            this.panelN.BackColor = System.Drawing.Color.Transparent;
            this.panelN.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.panelN.Location = new System.Drawing.Point(730, 375);
            this.panelN.Name = "panelN";
            this.panelN.Size = new System.Drawing.Size(10, 10);
            this.panelN.TabIndex = 4;
            this.panelN.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelN_MouseDown);
            this.panelN.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelN_MouseMove);
            this.panelN.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelN_MouseUp);
            // 
            // Clock1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 388);
            this.Controls.Add(this.panelN);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(50, 60);
            this.Name = "Clock1";
            this.ShowInTaskbar = false;
            this.Text = "Clock1";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Clock1_Load);
            this.SizeChanged += new System.EventHandler(this.Clock1_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelN;
    }
}