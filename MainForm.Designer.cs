namespace TranslaterSQL
{
    partial class MainForm
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
            this.exit = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.historyBox = new System.Windows.Forms.ComboBox();
            this.recognizedBox = new System.Windows.Forms.TextBox();
            this.translateBox = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.buferButton = new System.Windows.Forms.Button();
            this.CleanButton = new System.Windows.Forms.Button();
            this.SpeakerButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // exit
            // 
            this.exit.AutoSize = true;
            this.exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.exit.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.exit.Location = new System.Drawing.Point(975, 5);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(30, 29);
            this.exit.TabIndex = 2;
            this.exit.Text = "Х";
            this.exit.Click += new System.EventHandler(this.exit_Click);
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
            this.label1.Text = "Главный экран\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.historyBox);
            this.panel1.Controls.Add(this.recognizedBox);
            this.panel1.Controls.Add(this.translateBox);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.buferButton);
            this.panel1.Controls.Add(this.CleanButton);
            this.panel1.Controls.Add(this.SpeakerButton);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(-5, -5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1005, 507);
            this.panel1.TabIndex = 1;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // historyBox
            // 
            this.historyBox.FormattingEnabled = true;
            this.historyBox.Location = new System.Drawing.Point(780, 110);
            this.historyBox.Name = "historyBox";
            this.historyBox.Size = new System.Drawing.Size(200, 21);
            this.historyBox.TabIndex = 7;
            this.historyBox.SelectedIndexChanged += new System.EventHandler(this.historyBox_SelectedIndexChanged);
            // 
            // recognizedBox
            // 
            this.recognizedBox.Location = new System.Drawing.Point(20, 150);
            this.recognizedBox.Name = "recognizedBox";
            this.recognizedBox.Size = new System.Drawing.Size(460, 20);
            this.recognizedBox.TabIndex = 6;
            // 
            // translateBox
            // 
            this.translateBox.Location = new System.Drawing.Point(510, 150);
            this.translateBox.Name = "translateBox";
            this.translateBox.Size = new System.Drawing.Size(460, 20);
            this.translateBox.TabIndex = 5;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(780, 420);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(110, 50);
            this.button4.TabIndex = 4;
            this.button4.Text = "Сменить язык";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // buferButton
            // 
            this.buferButton.Location = new System.Drawing.Point(620, 420);
            this.buferButton.Name = "buferButton";
            this.buferButton.Size = new System.Drawing.Size(200, 100);
            this.buferButton.TabIndex = 3;
            this.buferButton.Text = "Вставить из буфера";
            this.buferButton.UseVisualStyleBackColor = true;
            this.buferButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // CleanButton
            // 
            this.CleanButton.Location = new System.Drawing.Point(490, 420);
            this.CleanButton.Name = "CleanButton";
            this.CleanButton.Size = new System.Drawing.Size(110, 50);
            this.CleanButton.TabIndex = 2;
            this.CleanButton.Text = "Очистить";
            this.CleanButton.UseVisualStyleBackColor = true;
            this.CleanButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // SpeakerButton
            // 
            this.SpeakerButton.Location = new System.Drawing.Point(360, 420);
            this.SpeakerButton.Name = "SpeakerButton";
            this.SpeakerButton.Size = new System.Drawing.Size(110, 50);
            this.SpeakerButton.TabIndex = 1;
            this.SpeakerButton.Text = "Озвучить";
            this.SpeakerButton.UseVisualStyleBackColor = true;
            this.SpeakerButton.Click += new System.EventHandler(this.Button1_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(8)))), ((int)(((byte)(56)))));
            this.panel2.Controls.Add(this.exit);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1005, 100);
            this.panel2.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(250, 420);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 50);
            this.button1.TabIndex = 8;
            this.button1.Text = "Аудио";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(140, 420);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 50);
            this.button2.TabIndex = 9;
            this.button2.Text = "Видео";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 500);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
      
        private System.Windows.Forms.Label exit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button buferButton;
        private System.Windows.Forms.Button CleanButton;
        private System.Windows.Forms.Button SpeakerButton;
        private System.Windows.Forms.TextBox recognizedBox;
        private System.Windows.Forms.TextBox translateBox;
        private System.Windows.Forms.ComboBox historyBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        
    }
}