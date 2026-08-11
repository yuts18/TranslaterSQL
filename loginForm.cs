using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace TranslaterSQL
{
    public partial class loginForm : Form
    {
        public static int CurrentUserId = -1;
        // private SqlConnection sqlConnection= null;
        public loginForm()
        {
            InitializeComponent();

            this.password.AutoSize = false;
            this.password.Size = new Size(this.password.Size.Width, 64);
        }

 
            
        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void exit_MouseEnter(object sender, EventArgs e)
        {
            exit.ForeColor = Color.Black;
        }

        private void exit_MouseLeave(object sender, EventArgs e)
        {
            exit.ForeColor = Color.White;
        }
        Point lastPoint;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }

        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void button_input_Click(object sender, EventArgs e)
        {
            String loginUser = login.Text;
            String passwordUser = password.Text;

            try
            {
                using (var conn = new MySqlConnection("server=localhost;port=3307;username=root;password=root;database=mediadb"))
                {
                    conn.Open();

                    string query = "SELECT * FROM `users` WHERE `login` = @uL AND `password` = @uP";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginUser;
                    command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passwordUser;

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count > 0)
                    {
                        CurrentUserId = Convert.ToInt32(table.Rows[0]["id"]);
                        this.Hide();
                        ModeSelectForm modeForm = new ModeSelectForm();
                        modeForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка входа: " + ex.Message);
            }
        }



        private void registerLabel_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            registerForm register_Form = new registerForm();
            register_Form.Show();
        }
    }
}
