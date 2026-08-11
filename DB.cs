using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TranslaterSQL
{
    internal class DB
    {
        private const string connectionString =
       "server=localhost;port=3307;username=root;password=root;database=mediadb";

        MySqlConnection connection = new MySqlConnection("server=localhost;port=3307;username=root;password=root;database=mediadb");

        public void openConnection()
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

        }
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
        public void SaveTranslationHistoryAsync(int userId, string sourceLang, string targetLang, string sourceText, string translatedText)
        {
            if(userId == -1)
            {
                Console.WriteLine("Пользователь не авторизован, сохранение отменено");
                return;
            }
            string query = "INSERT INTO translationshistory (UserID, SourceLanguage, TargetLanguage, SourceText, TranslatedText, TranslationDate) " +
                    "VALUES (@UserID, @SourceLang, @TargetLang, @SourceText, @TranslatedText, NOW())";

            //  Создаём новое соединение каждый раз вместо переиспользования
            using (var conn = new MySqlConnection("server=localhost;port=3307;username=root;password=root;database=mediadb"))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@SourceLang", sourceLang);
                        cmd.Parameters.AddWithValue("@TargetLang", targetLang);
                        cmd.Parameters.AddWithValue("@SourceText", sourceText);
                        cmd.Parameters.AddWithValue("@TranslatedText", translatedText);

                        int affectedRows = cmd.ExecuteNonQuery();
                        Console.WriteLine($"Сохранено строк: {affectedRows}, userId={userId}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения истории перевода: " + ex.Message);
                }
            }
        }

        public List<(string original, string translation)> GetTranslationHistory(int userId)
        {
            var result = new List<(string original, string translation)>();
            string query = "SELECT SourceText, TranslatedText FROM translationshistory WHERE UserID = @userId ORDER BY TranslationDate DESC";

            using (var conn = new MySqlConnection("server=localhost;port=3307;username=root;password=root;database=mediadb"))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string original = reader.GetString("SourceText");
                                string translation = reader.GetString("TranslatedText");
                                if (!result.Any(r => r.original == original))
                                    result.Add((original, translation));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки истории: " + ex.Message);
                }
            }
            return result;
        }

        public void AddFlashcard(int userId, string original, string translation)
        {
            string query = "INSERT INTO flashcards (user_id, original_text, translated_text, next_review)" +
                           "VALUES (@userId, @original, @translation, CURDATE())" +
                           "ON DUPLICATE KEY UPDATE original_text = original_text";
            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@original", original);
                        cmd.Parameters.AddWithValue("@translation", translation);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Ошибка добавления карточки: " + ex.Message);
                }
            }
        }

        public List<FlashCard> GetDueCards(int userId)
        {
            var result = new List<FlashCard>();
            string query = "SELECT id, original_text, translated_text, interval_days, ease_factor, repetitions " +
                           "FROM flashcards WHERE user_id = @userId AND next_review <= CURDATE() " +
                           "ORDER BY next_review ASC";

            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new FlashCard
                                {
                                    Id = reader.GetInt32("id"),
                                    Original = reader.GetString("original_text"),
                                    Translation = reader.GetString("translated_text"),
                                    IntervalDays = reader.GetInt32("interval_days"),
                                    EaseFactor = reader.GetFloat("ease_factor"),
                                    Repetitions = reader.GetInt32("repetitions")
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки карточек: " + ex.Message);
                }
            }
            return result;
        }

        public void UpdateCardAfterReview(int cardId, int quality)
        {
            FlashCard card = GetCardById(cardId);
            if (card == null) return;

            float newEase = card.EaseFactor + (0.1f - (5 - quality) * (0.08f + (5 - quality) * 0.02f));
            if (newEase < 1.3f) newEase = 1.3f;

            int newInterval;
            int newRepetitions;

            if (quality < 3)
            {
                newRepetitions = 0;
                newInterval = 1;
            }
            else
            {
                newRepetitions = card.Repetitions + 1;
                if (card.Repetitions == 0)
                    newInterval = 1;
                else if (card.Repetitions == 1)
                    newInterval = 3;
                else
                    newInterval = (int)Math.Round(card.IntervalDays * newEase);
            }

            string query = "UPDATE flashcards SET " +
                           "next_review = DATE_ADD(CURDATE(), INTERVAL @interval DAY), " +
                           "interval_days = @interval, ease_factor = @ease, repetitions = @reps " +
                           "WHERE id = @id";

            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@interval", newInterval);
                        cmd.Parameters.AddWithValue("@ease", newEase);
                        cmd.Parameters.AddWithValue("@reps", newRepetitions);
                        cmd.Parameters.AddWithValue("@id", cardId);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка обновления: " + ex.Message);
                }
            }
        }

        public FlashCard GetCardById(int cardId)
        {
            string query = "SELECT id, original_text, translated_text, interval_days, ease_factor, repetitions " +
                           "FROM flashcards WHERE id = @id";

            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", cardId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new FlashCard
                                {
                                    Id = reader.GetInt32("id"),
                                    Original = reader.GetString("original_text"),
                                    Translation = reader.GetString("translated_text"),
                                    IntervalDays = reader.GetInt32("interval_days"),
                                    EaseFactor = reader.GetFloat("ease_factor"),
                                    Repetitions = reader.GetInt32("repetitions")
                                };
                            }
                        }
                    }
                }
                catch { }
            }
            return null;
        }
        public MySqlConnection getConnection()
        {
            return connection;
        }
    }

}
