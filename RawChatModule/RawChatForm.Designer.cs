﻿namespace RawChatModule
{
    partial class RawChatForm
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
            this.rawChatBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // rawChatBox
            // 
            this.rawChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rawChatBox.Location = new System.Drawing.Point(0, 0);
            this.rawChatBox.Multiline = true;
            this.rawChatBox.Name = "rawChatBox";
            this.rawChatBox.ReadOnly = true;
            this.rawChatBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.rawChatBox.Size = new System.Drawing.Size(327, 539);
            this.rawChatBox.TabIndex = 0;
            // 
            // RawChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 539);
            this.Controls.Add(this.rawChatBox);
            this.Name = "RawChatForm";
            this.Text = "Raw Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RawChatForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox rawChatBox;
    }
}