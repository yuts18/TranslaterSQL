using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TranslaterSQL
{
    public partial class registerForm : Form
    {
        public registerForm()
        {
            InitializeComponent();
            this.registerLabel.Click += new System.EventHandler(this.registerLabel_Click);
            this.password.AutoSize = false;
            this.password.Size = new Size(this.password.Size.Width, 64);
            userRole.Text = "Введите роль";
            userRole.ForeColor= Color.Gray;
            userLevelAccess.Text = "Введите уровень доступа";
            userLevelAccess.ForeColor = Color.Gray;
            login.Text = "Введите логин";
            login.ForeColor = Color.Gray;
            password.Text = "Введите пароль";
            password.ForeColor = Color.Gray;
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        Point lastPoint;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void registr_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void registr_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void userRole_Enter(object sender, EventArgs e)
        {
            if (userRole.Text == "Введите роль")
            {
                userRole.Text = "";
                userRole.ForeColor = Color.Black;
            }
        }

        private void userRole_Leave(object sender, EventArgs e)
        {
            if (userRole.Text == "")
            {
                userRole.Text = "Введите роль";
                userRole.ForeColor= Color.Gray;
            }
        }

        private void userLevelAccess_Enter(object sender, EventArgs e)
        {
            if (userLevelAccess.Text == "Введите уровень доступа")
            {
                userLevelAccess.Text = "";
                userLevelAccess.ForeColor = Color.Black;
            }
        }

        private void userLevelAccess_Leave(object sender, EventArgs e)
        {
            if (userLevelAccess.Text == "")
            {
                userLevelAccess.Text = "Введите уровень доступа";
                userLevelAccess.ForeColor = Color.Gray;
            }
        }
        private void login_Enter(object sender, EventArgs e)
        {
            if (login.Text == "Введите логин")
            {
                login.Text = "";
                login.ForeColor = Color.Black;
            }
        }

        private void login_Leave(object sender, EventArgs e)
        {
            if (login.Text == "")
            {
                login.Text = "Введите логин";
                login.ForeColor = Color.Gray;
            }
        }

        private void password_Enter(object sender, EventArgs e)
        {
            if (password.Text == "Введите пароль")
            {
                password.Text = "";
                password.ForeColor = Color.Black;
            }
        }

        private void password_Leave(object sender, EventArgs e)
        {
            if (password.Text == "")
            {
                password.Text = "Введите пароль";
                password.ForeColor = Color.Gray;
            }
        }
        private void button_register_Click(object sender, EventArgs e)
        {
            if (login.Text == "Введите логин")
            {
                MessageBox.Show("Введите логин");
                return;
            }
            if (password.Text == "Введите пароль")
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            if (userLevelAccess.Text == "Введите уровень доступа")
            {
                MessageBox.Show("Введите уровень доступа");
                return;
            }
            if (userRole.Text == "Введите роль")
            {
                MessageBox.Show("Введите роль");
                return;
            }

            if (ifUserExists())
                return;


            DB db = new DB();
            MySqlCommand command = new MySqlCommand("INSERT INTO `users` (`login`, `password`, `level_access`, `role`) VALUES (@login, @password, @access, @role)", db.getConnection());

            command.Parameters.Add("@login", MySqlDbType.VarChar).Value =login.Text ;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value=password.Text;
            command.Parameters.Add("@access", MySqlDbType.VarChar).Value=userLevelAccess.Text;
            command.Parameters.Add("@role", MySqlDbType.VarChar).Value=userRole.Text;

           
          

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Аккаунт был создан");

                    this.Hide();
                    MainForm mainForm = new MainForm();
                    mainForm.Show();
                }
                else
                    MessageBox.Show("Аккаунт не был создан");
            

           


        }

      public Boolean ifUserExists()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @uL", db.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = login.Text;
            

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Такой пользователь уже есть. Введите другой логин");
                return true;
            }
            else
                return false;

        }

        private void registerLabel_Click(object sender, EventArgs e)
        {
            this.Hide();    
            loginForm login_Form = new loginForm();
            login_Form.Show();
        }
    }
}
