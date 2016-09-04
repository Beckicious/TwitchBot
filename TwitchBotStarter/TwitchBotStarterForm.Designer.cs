namespace TwitchBotStarter
{
    partial class TwitchBotStarterForm
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
            this.txtChannel = new System.Windows.Forms.TextBox();
            this.labModules = new System.Windows.Forms.Label();
            this.comModules = new System.Windows.Forms.ComboBox();
            this.labChannel = new System.Windows.Forms.Label();
            this.butJoin = new System.Windows.Forms.Button();
            this.butLeave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtChannel
            // 
            this.txtChannel.Location = new System.Drawing.Point(66, 35);
            this.txtChannel.Name = "txtChannel";
            this.txtChannel.Size = new System.Drawing.Size(206, 20);
            this.txtChannel.TabIndex = 0;
            // 
            // labModules
            // 
            this.labModules.AutoSize = true;
            this.labModules.Location = new System.Drawing.Point(12, 15);
            this.labModules.Name = "labModules";
            this.labModules.Size = new System.Drawing.Size(47, 13);
            this.labModules.TabIndex = 1;
            this.labModules.Text = "Modules";
            // 
            // comModules
            // 
            this.comModules.FormattingEnabled = true;
            this.comModules.Location = new System.Drawing.Point(66, 12);
            this.comModules.Name = "comModules";
            this.comModules.Size = new System.Drawing.Size(206, 21);
            this.comModules.TabIndex = 2;
            this.comModules.SelectedIndexChanged += new System.EventHandler(this.comModules_SelectedIndexChanged);
            // 
            // labChannel
            // 
            this.labChannel.AutoSize = true;
            this.labChannel.Location = new System.Drawing.Point(12, 38);
            this.labChannel.Name = "labChannel";
            this.labChannel.Size = new System.Drawing.Size(46, 13);
            this.labChannel.TabIndex = 3;
            this.labChannel.Text = "Channel";
            // 
            // butJoin
            // 
            this.butJoin.Location = new System.Drawing.Point(15, 61);
            this.butJoin.Name = "butJoin";
            this.butJoin.Size = new System.Drawing.Size(124, 24);
            this.butJoin.TabIndex = 4;
            this.butJoin.Text = "Join Channel";
            this.butJoin.UseVisualStyleBackColor = true;
            this.butJoin.Click += new System.EventHandler(this.butJoin_Click);
            // 
            // butLeave
            // 
            this.butLeave.Location = new System.Drawing.Point(145, 61);
            this.butLeave.Name = "butLeave";
            this.butLeave.Size = new System.Drawing.Size(127, 24);
            this.butLeave.TabIndex = 5;
            this.butLeave.Text = "Leave Channel";
            this.butLeave.UseVisualStyleBackColor = true;
            this.butLeave.Click += new System.EventHandler(this.butLeave_Click);
            // 
            // TwitchBotStarterForm
            // 
            this.AcceptButton = this.butJoin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 98);
            this.Controls.Add(this.butLeave);
            this.Controls.Add(this.butJoin);
            this.Controls.Add(this.labChannel);
            this.Controls.Add(this.comModules);
            this.Controls.Add(this.labModules);
            this.Controls.Add(this.txtChannel);
            this.Name = "TwitchBotStarterForm";
            this.Text = "PlebBot";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtChannel;
        private System.Windows.Forms.Label labModules;
        private System.Windows.Forms.ComboBox comModules;
        private System.Windows.Forms.Label labChannel;
        private System.Windows.Forms.Button butJoin;
        private System.Windows.Forms.Button butLeave;
    }
}