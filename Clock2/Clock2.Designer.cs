namespace Clock2
{
    partial class Clock2
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
            this.SuspendLayout();
            // 
            // Clock2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 388);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(50, 60);
            this.Name = "Clock2";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Clock1";
            this.Load += new System.EventHandler(this.Clock2_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Clock2_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Clock2_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Clock2_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}