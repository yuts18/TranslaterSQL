namespace TranslaterSQL
{
    partial class Video
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.buferButton = new System.Windows.Forms.Button();
            this.CleanButton = new System.Windows.Forms.Button();
            this.SpeakerButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.exit = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.LightYellow;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(805, 100);
            this.label1.TabIndex = 1;
            this.label1.Text = "Перевод видео";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.buferButton);
            this.panel1.Controls.Add(this.CleanButton);
            this.panel1.Controls.Add(this.SpeakerButton);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(-2, -3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(805, 457);
            this.panel1.TabIndex = 3;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(650, 380);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(117, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "Сменить язык";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // buferButton
            // 
            this.buferButton.Location = new System.Drawing.Point(466, 380);
            this.buferButton.Name = "buferButton";
            this.buferButton.Size = new System.Drawing.Size(135, 23);
            this.buferButton.TabIndex = 3;
            this.buferButton.Text = "Вставить из буфера";
            this.buferButton.UseVisualStyleBackColor = true;
            // 
            // CleanButton
            // 
            this.CleanButton.Location = new System.Drawing.Point(357, 380);
            this.CleanButton.Name = "CleanButton";
            this.CleanButton.Size = new System.Drawing.Size(75, 23);
            this.CleanButton.TabIndex = 2;
            this.CleanButton.Text = "Очистить";
            this.CleanButton.UseVisualStyleBackColor = true;
            // 
            // SpeakerButton
            // 
            this.SpeakerButton.Location = new System.Drawing.Point(225, 380);
            this.SpeakerButton.Name = "SpeakerButton";
            this.SpeakerButton.Size = new System.Drawing.Size(90, 23);
            this.SpeakerButton.TabIndex = 1;
            this.SpeakerButton.Text = "Озвучить";
            this.SpeakerButton.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(8)))), ((int)(((byte)(56)))));
            this.panel2.Controls.Add(this.exit);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(805, 100);
            this.panel2.TabIndex = 0;
            // 
            // exit
            // 
            this.exit.AutoSize = true;
            this.exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.exit.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.exit.Location = new System.Drawing.Point(775, 0);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(30, 29);
            this.exit.TabIndex = 2;
            this.exit.Text = "Х";
            // 
            // Video
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "Video";
            this.Text = "Video";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button buferButton;
        private System.Windows.Forms.Button CleanButton;
        private System.Windows.Forms.Button SpeakerButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label exit;
    }
}