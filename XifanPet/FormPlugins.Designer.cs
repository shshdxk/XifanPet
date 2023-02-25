namespace XifanPet
{
    partial class FormPlugins
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
            this.myTreeView1 = new XifanPet.Control.MyTreeView();
            this.SuspendLayout();
            // 
            // myTreeView1
            // 
            this.myTreeView1.CheckBoxes = true;
            this.myTreeView1.Location = new System.Drawing.Point(12, 12);
            this.myTreeView1.Name = "myTreeView1";
            this.myTreeView1.Size = new System.Drawing.Size(323, 426);
            this.myTreeView1.TabIndex = 3;
            this.myTreeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            // 
            // FormPlugins
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 450);
            this.Controls.Add(this.myTreeView1);
            this.Name = "FormPlugins";
            this.Text = "FormPlugins";
            this.Activated += new System.EventHandler(this.FormPlugins_Activated);
            this.Load += new System.EventHandler(this.FormPlugins_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Control.MyTreeView myTreeView1;
    }
}