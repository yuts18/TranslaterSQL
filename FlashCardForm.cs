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
    public partial class FlashCardForm : Form
    {
        private List<(string original, string translation)> cards;
        private int currentIndex = 0;
        private bool isFlipped = false;

        private Label labelWord;
        private Button btnFlip;
        private Button btnNext;
        private Button btnPrev;
        private Label labelProgress;

        public FlashCardForm()
        {
            InitializeComponent();
            this.Text = "Заучивание слов";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            var btnTranslator = new Button
            {
                Text = "Переводчик",
                Size = new Size(150, 40),
                Location = new Point(150, 300),
                Font = new Font("Segou UI", 11),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnTranslator.Click += (s, e) =>
            {
                new MainForm().Show();
                this.Hide();
            };
            this.Controls.Add(btnTranslator);

            labelWord = new Label
            {
                Text ="",
                Size = new Size(440,150),
                Location =new Point(20,30),
                Font = new Font("Segou UI", 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle
            };

            labelProgress = new Label
            {
                Text = "",
                Size = new Size(440, 30),
                Location = new Point(20, 190),
                Font = new Font("Segou UI", 12),
                TextAlign = ContentAlignment.MiddleCenter
               
            };

            btnFlip = new Button
            {
                Text = "Показать перевод",
                Size = new Size(200, 40),
                Location = new Point(140, 240),
                Font = new Font("Segou UI", 12)
            };

            btnPrev = new Button
            {
                Text = "Назад",
                Size = new Size(120, 40),
                Location = new Point(20, 300),
                Font = new Font("Segou UI", 12)
            };

            btnNext = new Button
            {
                Text = "Вперед",
                Size = new Size(120, 40),
                Location = new Point(340, 300),
                Font = new Font("Segou UI", 12)
            };

            btnFlip.Click += BtnFlip_Click;
            btnNext.Click += BtnNext_Click;
            btnPrev.Click += BtnPrev_Click;

            this.Controls.Add(labelWord);
            this.Controls.Add(labelProgress);
            this.Controls.Add(btnFlip);
            this.Controls.Add(btnPrev);
            this.Controls.Add(btnNext);

            LoadCards();
        }

        private void LoadCards()
        {
            try
            {
                DB db = new DB();
                int userId = loginForm.CurrentUserId;
                cards = db.GetTranslationHistory(userId);

                if (cards.Count == 0)
                {
                    MessageBox.Show("История переводов путса. Переведите что-нибудь.");
                    this.Close();
                    return;
                }

                var rng = new Random();
                cards = cards.OrderBy(x => rng.Next()).ToList();

                ShowCard();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки карточек: {ex.Message}");

            }
        }
        private void ShowCard()
        {
            if (cards.Count == 0) return;
            isFlipped = false;
            btnFlip.Text = "Показать перевод";
            labelWord.Text = cards[currentIndex].original;
            labelProgress.Text = $"{currentIndex + 1} / {cards.Count}";

        }

        private void BtnFlip_Click(object sender, EventArgs e)
        {
            if (!isFlipped)
            {
                labelWord.Text = cards[currentIndex].translation;
                btnFlip.Text = "Скрыть перевод";
                isFlipped = true;
            }

            else
            {
                labelWord.Text = cards[currentIndex].original;
                btnFlip.Text = "Показать перевод";
                isFlipped = false;
            }

        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            currentIndex = (currentIndex + 1) % cards.Count;
            ShowCard();
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            currentIndex = (currentIndex - 1 + cards.Count) % cards.Count;
            ShowCard();
        }
    }
}
