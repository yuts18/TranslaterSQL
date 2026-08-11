using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TranslaterSQL
{
    public class CardSelectionForm : Form
    {
        private CheckedListBox checkedList;
        private List<(string original, string translation)> allCards;
        private Button btnStart;
        private Button btnBack;
        private Button btnSelectAll;
        private Button btnClearAll;
        private Label labelInfo;

        public CardSelectionForm()
        {
            this.Text = "Выбор карточек для заучивания";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            var labelTitle = new Label
            {
                Text = "Выберите переводы для заучивания:",
                Size = new Size(560, 30),
                Location = new Point(15, 10),
                Font = new Font("Segoe UI", 13, FontStyle.Bold)
            };

            labelInfo = new Label
            {
                Text = "Выбрано: 0",
                Size = new Size(200, 25),
                Location = new Point(15, 45),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray
            };

            checkedList = new CheckedListBox
            {
                Size = new Size(560, 280),
                Location = new Point(15, 75),
                Font = new Font("Segoe UI", 11),
                CheckOnClick = true
            };

            checkedList.ItemCheck += (s, e) =>
            {
                int count = checkedList.CheckedItems.Count +
                    (e.NewValue == CheckState.Checked ? 1 : -1);
                labelInfo.Text = $"Выбрано: {count}";
            };

            btnSelectAll = new Button
            {
                Text = "Выбрать все",
                Size = new Size(130, 35),
                Location = new Point(15, 370),
                Font = new Font("Segoe UI", 10)
            };
            btnSelectAll.Click += (s, e) =>
            {
                for (int i = 0; i < checkedList.Items.Count; i++)
                    checkedList.SetItemChecked(i, true);
                labelInfo.Text = $"Выбрано: {checkedList.Items.Count}";
            };

            btnClearAll = new Button
            {
                Text = "Снять все",
                Size = new Size(130, 35),
                Location = new Point(155, 370),
                Font = new Font("Segoe UI", 10)
            };
            btnClearAll.Click += (s, e) =>
            {
                for (int i = 0; i < checkedList.Items.Count; i++)
                    checkedList.SetItemChecked(i, false);
                labelInfo.Text = "Выбрано: 0";
            };

            btnBack = new Button
            {
                Text = " Назад",
                Size = new Size(130, 40),
                Location = new Point(15, 420),
                Font = new Font("Segoe UI", 11)
            };
            btnBack.Click += (s, e) =>
            {
                new ModeSelectForm().Show();
                this.Close();
            };

            btnStart = new Button
            {
                Text = "Добавить в повторение ",
                Size = new Size(220, 40),
                Location = new Point(355, 420),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnStart.Click += BtnStart_Click;

            this.Controls.AddRange(new Control[]
            {
                labelTitle, labelInfo, checkedList,
                btnSelectAll, btnClearAll, btnBack, btnStart
            });

            LoadCards();
        }

        private void LoadCards()
        {
            try
            {
                DB db = new DB();
                int userId = loginForm.CurrentUserId;
                allCards = db.GetTranslationHistory(userId);

                if (allCards.Count == 0)
                {
                    MessageBox.Show(
                        "История переводов пуста.\nСначала переведите что-нибудь.",
                        "Нет карточек",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    this.Close();
                    return;
                }

                foreach (var card in allCards)
                {
                    string preview = card.original.Length > 40
                        ? card.original.Substring(0, 37) + "..."
                        : card.original;
                    checkedList.Items.Add($"{preview}  →  {card.translation}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (checkedList.CheckedIndices.Count == 0)
            {
                MessageBox.Show(
                    "Выберите хотя бы одну карточку!",
                    "Ничего не выбрано",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DB db = new DB();
            int userId = loginForm.CurrentUserId;
            int added = 0;

            foreach (int i in checkedList.CheckedIndices)
            {
                db.AddFlashcard(userId, allCards[i].original, allCards[i].translation);
                added++;
            }

            MessageBox.Show(
                $"Добавлено карточек: {added}\nТеперь перейдите в режим Повторение.",
                "Готово",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            new ModeSelectForm().Show();
            this.Close();
        }
    }
}