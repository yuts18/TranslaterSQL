using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TranslaterSQL
{
    public partial class ModeSelectForm : Form
    {
        public ModeSelectForm()
        {
            InitializeComponent();

            this.Text = "Выберите режим";
            this.Size = new Size(620, 250);
            this.StartPosition = FormStartPosition.CenterScreen;

            var btnTranslate = new Button
            {
                Text = "Переводчик",
                Size = new Size(150, 60),
                Location = new Point(30, 60),
                Font = new Font("Segoe UI", 14)
            };

            var btnLearn = new Button
            {
                Text = "Заучивание",
                Size = new Size(150, 60),
                Location = new Point(210, 60),
                Font = new Font("Segoe UI", 14)
            };
            var btnRepeat = new Button
            {
                Text = "Повторение",
                Size = new Size(150, 60),
                Location = new Point(410, 60),
                Font = new Font("Segou UI", 14),
                
            };


            btnTranslate.Click += (s, e) =>
            {
                new MainForm().Show();
                this.Hide();
            };

            btnLearn.Click += (s, e) => { new CardSelectionForm().Show(); this.Hide(); };

            btnRepeat.Click += (s, e) =>
            {
                new SpacedRepetitionForm().Show();
                this.Hide();
            };
            this.Controls.Add(btnTranslate);
            this.Controls.Add(btnLearn);
            this.Controls.Add(btnRepeat);
        }
    }
}
