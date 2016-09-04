namespace CleanChatModule
{
    partial class CleanChatForm
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
            this.cleanChatBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cleanChatBox
            // 
            this.cleanChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cleanChatBox.Location = new System.Drawing.Point(0, 0);
            this.cleanChatBox.Multiline = true;
            this.cleanChatBox.Name = "cleanChatBox";
            this.cleanChatBox.ReadOnly = true;
            this.cleanChatBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cleanChatBox.Size = new System.Drawing.Size(327, 539);
            this.cleanChatBox.TabIndex = 0;
            // 
            // CleanChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 539);
            this.Controls.Add(this.cleanChatBox);
            this.Name = "CleanChatForm";
            this.Text = "Clean Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CleanChatForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox cleanChatBox;
    }
}