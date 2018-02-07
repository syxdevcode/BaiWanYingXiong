namespace BaiWanYingXiong
{
    partial class Form2
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 346);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form2";
            this.Text = "Form2";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmCutter_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.frmCutter_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmCutter_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmCutter_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmCutter_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
    }
}