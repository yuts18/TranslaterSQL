using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TranslaterSQL
{
    public class SpacedRepetitionForm : Form
    {
        private List<FlashCard> cards;
        private int currentIndex = 0;
        private bool isFlipped = false;

        private Label labelWord;
        private Label labelProgress;
        private Label labelInterval;
        private Button btnShow;
        private Button btnEasy;
        private Button btnGood;
        private Button btnHard;
        private Button btnBack;

        public SpacedRepetitionForm()
        {
            this.Text = "Повторение слов ";
            this.Size = new Size(550, 480);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            var labelTitle = new Label
            {
                Text = "Интервальное повторение",
                Size = new Size(510, 30),
                Location = new Point(15, 10),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            labelProgress = new Label
            {
                Text = "",
                Size = new Size(510, 25),
                Location = new Point(15, 45),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter
            };

            labelWord = new Label
            {
                Text = "",
                Size = new Size(510, 150),
                Location = new Point(15, 75),
                Font = new Font("Segoe UI", 18),
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            labelInterval = new Label
            {
                Text = "",
                Size = new Size(510, 25),
                Location = new Point(15, 235),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.DimGray,
                TextAlign = ContentAlignment.MiddleCenter
            };

            btnShow = new Button
            {
                Text = "Показать перевод",
                Size = new Size(200, 45),
                Location = new Point(160, 270),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnEasy = new Button
            {
                Text = "Легко ",
                Size = new Size(150, 45),
                Location = new Point(350, 330),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Visible = false
            };

            btnGood = new Button
            {
                Text = "Хорошо",
                Size = new Size(150, 45),
                Location = new Point(190, 330),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Visible = false
            };

            btnHard = new Button
            {
                Text = "Трудно ",
                Size = new Size(150, 45),
                Location = new Point(30, 330),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Visible = false
            };

            btnBack = new Button
            {
                Text = " Назад",
                Size = new Size(130, 40),
                Location = new Point(15, 410),
                Font = new Font("Segoe UI", 11)
            };

            btnShow.Click += BtnShow_Click;
            btnEasy.Click += (s, e) => SubmitAnswer(5);
            btnGood.Click += (s, e) => SubmitAnswer(3);
            btnHard.Click += (s, e) => SubmitAnswer(1);
            btnBack.Click += (s, e) => { new MainForm().Show(); this.Close(); };

            this.Controls.AddRange(new Control[]
            {
                labelTitle, labelProgress, labelWord, labelInterval,
                btnShow, btnEasy, btnGood, btnHard, btnBack
            });

            LoadCards();
        }

        private void LoadCards()
        {
            DB db = new DB();
            int userId = loginForm.CurrentUserId;
            cards = db.GetDueCards(userId);

            if (cards.Count == 0)
            {
                MessageBox.Show(
                    "Нет карточек для повторения сегодня!\nДобавьте карточки в режиме заучивания.",
                    "Готово",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                this.Close();
                return;
            }

            ShowCard();
        }

        private void ShowCard()
        {
            isFlipped = false;
            btnShow.Visible = true;
            btnEasy.Visible = false;
            btnGood.Visible = false;
            btnHard.Visible = false;

            labelWord.BackColor = Color.White;
            labelWord.Text = cards[currentIndex].Original;
            labelProgress.Text = $"Карточка {currentIndex + 1} из {cards.Count}";
            labelInterval.Text = $"Интервал: {cards[currentIndex].IntervalDays} дн. | " +
                                 $"Повторений: {cards[currentIndex].Repetitions}";
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            labelWord.Text = cards[currentIndex].Translation;
            labelWord.BackColor = Color.LightYellow;
            btnShow.Visible = false;
            btnEasy.Visible = true;
            btnGood.Visible = true;
            btnHard.Visible = true;
        }

        private void SubmitAnswer(int quality)
        {
            DB db = new DB();
            db.UpdateCardAfterReview(cards[currentIndex].Id, quality);

            currentIndex++;

            if (currentIndex >= cards.Count)
            {
                MessageBox.Show(
                    $"Сессия завершена!\nПовторено карточек: {cards.Count}",
                    "Молодец!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                new MainForm().Show();
                this.Close();
                return;
            }

            ShowCard();
        }
    }

}
