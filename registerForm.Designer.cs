namespace TranslaterSQL
{
    partial class registerForm
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
            this.registr = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.registerLabel = new System.Windows.Forms.Label();
            this.userLevelAccess = new System.Windows.Forms.TextBox();
            this.userRole = new System.Windows.Forms.TextBox();
            this.button_register = new System.Windows.Forms.Button();
            this.password = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.login = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // exit
            // 
            this.exit.AutoSize = true;
            this.exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.exit.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.exit.Location = new System.Drawing.Point(771, 0);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(30, 29);
            this.exit.TabIndex = 2;
            this.exit.Text = "Х";
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // registr
            // 
            this.registr.BackColor = System.Drawing.Color.Purple;
            this.registr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registr.Font = new System.Drawing.Font("Comic Sans MS", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.registr.ForeColor = System.Drawing.Color.LightYellow;
            this.registr.Location = new System.Drawing.Point(0, 0);
            this.registr.Name = "registr";
            this.registr.Size = new System.Drawing.Size(801, 100);
            this.registr.TabIndex = 1;
            this.registr.Text = "Регистрация";
            this.registr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.registr.MouseDown += new System.Windows.Forms.MouseEventHandler(this.registr_MouseDown);
            this.registr.MouseMove += new System.Windows.Forms.MouseEventHandler(this.registr_MouseMove);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.registerLabel);
            this.panel1.Controls.Add(this.userLevelAccess);
            this.panel1.Controls.Add(this.userRole);
            this.panel1.Controls.Add(this.button_register);
            this.panel1.Controls.Add(this.password);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.login);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(0, -2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(801, 455);
            this.panel1.TabIndex = 1;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // registerLabel
            // 
            this.registerLabel.AutoSize = true;
            this.registerLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.registerLabel.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.registerLabel.ForeColor = System.Drawing.Color.Black;
            this.registerLabel.Location = new System.Drawing.Point(397, 420);
            this.registerLabel.Name = "registerLabel";
            this.registerLabel.Size = new System.Drawing.Size(57, 23);
            this.registerLabel.TabIndex = 8;
            this.registerLabel.Text = "Войти";
            // 
            // userLevelAccess
            // 
            this.userLevelAccess.Font = new System.Drawing.Font("Times New Roman", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userLevelAccess.Location = new System.Drawing.Point(471, 154);
            this.userLevelAccess.Multiline = true;
            this.userLevelAccess.Name = "userLevelAccess";
            this.userLevelAccess.Size = new System.Drawing.Size(302, 84);
            this.userLevelAccess.TabIndex = 7;
            this.userLevelAccess.Enter += new System.EventHandler(this.userLevelAccess_Enter);
            this.userLevelAccess.Leave += new System.EventHandler(this.userLevelAccess_Leave);
            // 
            // userRole
            // 
            this.userRole.Font = new System.Drawing.Font("Times New Roman", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userRole.Location = new System.Drawing.Point(82, 154);
            this.userRole.Multiline = true;
            this.userRole.Name = "userRole";
            this.userRole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.userRole.Size = new System.Drawing.Size(302, 64);
            this.userRole.TabIndex = 6;
            this.userRole.Enter += new System.EventHandler(this.userRole_Enter);
            this.userRole.Leave += new System.EventHandler(this.userRole_Leave);
            // 
            // button_register
            // 
            this.button_register.BackColor = System.Drawing.Color.Green;
            this.button_register.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.button_register.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button_register.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.button_register.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_register.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_register.Location = new System.Drawing.Point(313, 357);
            this.button_register.Name = "button_register";
            this.button_register.Size = new System.Drawing.Size(226, 46);
            this.button_register.TabIndex = 5;
            this.button_register.Text = "Зарегистрироваться";
            this.button_register.UseVisualStyleBackColor = false;
            this.button_register.Click += new System.EventHandler(this.button_register_Click);
            // 
            // password
            // 
            this.password.Font = new System.Drawing.Font("Times New Roman", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.Location = new System.Drawing.Point(471, 265);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(302, 48);
            this.password.TabIndex = 4;
            this.password.UseSystemPasswordChar = true;
            this.password.Enter += new System.EventHandler(this.password_Enter);
            this.password.Leave += new System.EventHandler(this.password_Leave);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::TranslaterSQL.Properties.Resources.lock__2_;
            this.pictureBox2.Location = new System.Drawing.Point(401, 265);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(64, 64);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // login
            // 
            this.login.Font = new System.Drawing.Font("Times New Roman", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login.Location = new System.Drawing.Point(82, 265);
            this.login.Multiline = true;
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(302, 64);
            this.login.TabIndex = 2;
            this.login.Enter += new System.EventHandler(this.login_Enter);
            this.login.Leave += new System.EventHandler(this.login_Leave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TranslaterSQL.Properties.Resources.user__2_;
            this.pictureBox1.Location = new System.Drawing.Point(12, 265);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(8)))), ((int)(((byte)(56)))));
            this.panel2.Controls.Add(this.exit);
            this.panel2.Controls.Add(this.registr);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(801, 100);
            this.panel2.TabIndex = 0;
            // 
            // registerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "registerForm";
            this.Text = "registerForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label exit;
        private System.Windows.Forms.Label registr;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_register;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox login;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox userLevelAccess;
        private System.Windows.Forms.TextBox userRole;
        private System.Windows.Forms.Label registerLabel;
    }
}