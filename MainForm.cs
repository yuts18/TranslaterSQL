
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Text;
using System.Threading; 
using System.Threading.Tasks;
using System.Windows.Forms;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.WindowsAPICodePack.Dialogs; 
using NAudio.Wave;// для аудио
using NAudio.Wave.SampleProviders;
using Newtonsoft.Json;
using Tesseract;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

public static class Logger
{
    private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app_log.txt");

    public static void Log(string message)
    {
        try
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";
            File.AppendAllText(logFilePath, logEntry);
        }
        catch
        {
            // Если логирование упало — не ломаем основное приложение
        }
    }

    public static void LogException(Exception ex, string context = "")
    {
        Log($"EXCEPTION {context}: {ex.GetType().Name} - {ex.Message}\nStackTrace:\n{ex.StackTrace}");
    }
}

namespace TranslaterSQL
{
    public partial class MainForm : Form
    {
        private WaveOutEvent waveOut;
        private AudioFileReader audioFileReader;

        private SpeechSynthesizer speechSynthesizer;
        private bool isEnglishToRussian;  // Флаг для направления перевода
        private List<KeyValuePair<string, string>> translatedWords;  // Хранение переводов

        // Регистрация горячих клавиш
        private const int MOD_ALT = 0x0001;
        private const int MOD_CONTROL = 0x0002;
        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_SCISSORS = 1;
        private const int HOTKEY_OPEN_FILE = 2;

        // DLL импорты для работы с горячими клавишами
        [DllImport("user32.dll")]
        public static extern int RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern int UnregisterHotKey(IntPtr hWnd, int id);

        public MainForm()
        {
            InitializeComponent();

            // Загрузка токена из файла если он существует
           // LoadIamTokenFromFile();
            
            this.recognizedBox.AutoSize = false;
            this.recognizedBox.Multiline = true;
            this.recognizedBox.WordWrap = true;
            this.recognizedBox.Size = new Size(this.recognizedBox.Size.Width, 150);
            
            this.translateBox.AutoSize = false;
            this.translateBox.Multiline = true;
            this.translateBox.WordWrap = true;
            this.translateBox.Size = new Size(this.translateBox.Size.Width, 150);

            //this.FormBorderStyle = FormBorderStyle.Sizable;
            //this.MinimumSize = new Size(650, 500);
            recognizedBox.Font = new Font(recognizedBox.Font.FontFamily, 14);
            translateBox.Font = new Font(translateBox.Font.FontFamily, 14);

            button4.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            button4.Size = new Size(110, 40);
          
            button1.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            button1.Size = new Size(100, 40);
            button2.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            button2.Size = new Size(80, 40);
            buferButton.Size = new Size(155, 50);
            buferButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);
           
            CleanButton.Size = new Size(110, 40);
            CleanButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            SpeakerButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            SpeakerButton.Size = new Size(120, 40);

            speechSynthesizer = new SpeechSynthesizer();
            isEnglishToRussian = true; // Изначально направление перевода - с английского на русский
            translatedWords = new List<KeyValuePair<string, string>>();

            var btnLearn = new Button
            {
                Text = "Заучивание",
                Size = new Size(120, 50),
                Location = new Point(900, 420),
                Font = new Font("Segou UI", 11),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            //MessageBox.Show($"Размер формы: {this.ClientSize.Width} x {this.ClientSize.Height}");

            btnLearn.Click += (s, e) =>
            {
                new FlashCardForm().Show();
                this.Hide();
            };
            panel1.Controls.Add(btnLearn);

            var btnRepeat = new Button
            {
                Text = "Повторение",
                Size = new Size(120, 50),
                Location = new Point(0, 420),
                Font = new Font("Segou UI", 11),

            };
            btnRepeat.Click += (s, e) =>
            {
                new SpacedRepetitionForm().Show();
                this.Hide();
            };
            panel1.Controls.Add(btnRepeat);
            // Обновляем текст кнопки направления перевода
            UpdateLanguageButtonText();

            // Инициализация горячих клавиш
            RegisterHotKey(this.Handle, HOTKEY_SCISSORS, MOD_ALT, (int)Keys.T); // ALT + T
            RegisterHotKey(this.Handle, HOTKEY_OPEN_FILE, MOD_ALT, (int)Keys.O); // ALT + O

            // Проверка наличия токена
           // CheckIamToken();
        }
        
        private CancellationTokenSource cts;

        

        private async void buttonPlayAndTranslate_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "Видео файлы|*.mp4;*.avi;*.mov;*.mkv;*.wmv";
               
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (cts != null)
                    {
                        cts.Cancel();
                        cts.Dispose();
                    }
                    cts = new CancellationTokenSource();

                    string videoPath = openFileDialog.FileName;
                    if (!File.Exists(videoPath))
                    {
                        MessageBox.Show("Файл не найден", "Ошибка");
                        return;
                    }

                    recognizedBox.Clear();
                    translateBox.Clear();
                    this.Cursor = Cursors.WaitCursor;

                    try
                    {
                        string audioPath = ExtractAudioFromVideo(videoPath);
                        if (audioPath == null)
                        {
                            MessageBox.Show("Не удалось извлечь аудио", "Ошибка");
                            return;
                        }

                        // Запускаем воспроизведение видео
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = videoPath,
                            UseShellExecute = true
                        });

                        var audioChunks = SplitAudioByTime(audioPath, 5);// делим дорожку на отрезки по 5 секунд



                        foreach (var chunkFile in audioChunks)
                        {
                            if (cts.Token.IsCancellationRequested) break;

                            var (recognized, translated) = await RecognizeSpeech(chunkFile, isEnglishToRussian);

                            if (!string.IsNullOrWhiteSpace(recognized))
                            {
                                this.Invoke((MethodInvoker)(() =>
                                {
                                    recognizedBox.AppendText(recognized + " ");
                                    translateBox.AppendText(translated + " ");

                                    if (!translated.StartsWith("ОШИБКА:"))
                                    {
                                        int userId = loginForm.CurrentUserId;
                                        if (userId != -1)
                                        {
                                            DB db = new DB();
                                            db.SaveTranslationHistoryAsync(userId,
                                                isEnglishToRussian ? "en" : "ru",
                                                isEnglishToRussian ? "ru" : "en",
                                                recognized, translated);
                                        }
                                        translatedWords.Add(new KeyValuePair<string, string>(recognized, translated));
                                        UpdateHistoryBox();
                                    }
                                }));
                            }

                            try { File.Delete(chunkFile); } catch { }
                        }

                        try { File.Delete(audioPath); } catch { }
                    }
                    catch (OperationCanceledException)
                    {
                        // Перевод отменён
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }

        private string ExtractAudioFromVideo(string videoPath)
        {
            try
            {
                string ffmpegPath = @"C:\Program Files\ffmpeg-7.1-full_build\bin\ffmpeg.exe";
                string outputWav = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".wav");

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = ffmpegPath,
                        Arguments = $"-i \"{videoPath}\" -vn -acodec pcm_s16le -ar 16000 -ac 1 \"{outputWav}\"",
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"FFmpeg error: {error}");
                    return null;
                }

                Console.WriteLine($"Audio extracted to: {outputWav}");
                return File.Exists(outputWav) ? outputWav : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }
        private List<string> SplitAudioByTime(string wavPath, int chunkSeconds = 20)
        {
            var chunks = new List<string>();

            using (var reader = new AudioFileReader(wavPath))
            {
                var resampledFormat = new WaveFormat(16000, 16, 1);

                using (var resampler = new MediaFoundationResampler(reader, resampledFormat))
                {
                    resampler.ResamplerQuality = 60;

                    int bytesPerSecond = resampledFormat.AverageBytesPerSecond;
                    int chunkSize = bytesPerSecond * chunkSeconds;
                    byte[] buffer = new byte[chunkSize];
                    int bytesRead;
                    int part = 0;

                    while ((bytesRead = resampler.Read(buffer, 0, chunkSize)) > 0)
                    {
                        // Проверка: есть ли звук в буфере?
                        if (HasSignificantAudio(buffer, bytesRead))
                        {
                            string chunkPath = Path.Combine(Path.GetTempPath(), $"{Path.GetFileNameWithoutExtension(wavPath)}_chunk{part}.wav");
                            using (var writer = new WaveFileWriter(chunkPath, resampledFormat))
                            {
                                writer.Write(buffer, 0, bytesRead);
                            }
                            chunks.Add(chunkPath);
                            part++;
                        }
                    }
                }
            }

            return chunks;
        }

        // Простая проверка наличия сигнала 
        private bool HasSignificantAudio(byte[] buffer, int bytesRead)
        {
            for (int i = 0; i < bytesRead; i += 2) // 16 бит = 2 байта
            {
                short sample = BitConverter.ToInt16(buffer, i);
                if (Math.Abs(sample) > 500) // Порог амплитуды
                    return true;
            }
            return false;
        }
        private async Task<(string recognized, string translation)> RecognizeSpeech(string filePath, bool isEnglishToRussian)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                using (var form = new MultipartFormDataContent())
                {
                    var fileBytes = File.ReadAllBytes(filePath);
                    form.Add(new ByteArrayContent(fileBytes), "audio", "audio.wav");
                    form.Add(new StringContent(isEnglishToRussian ? "EN-RU" : "RU-EN"), "direction");

                    var response = await client.PostAsync(
                        new Uri("http://localhost:5000/recognize_and_translate"), form);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
                        return (json["recognized"], json["translation"]);
                    }
                    return ("", "");
                }
            }
        }
        private string ConvertWavToOgg(string wavPath)
        {
            string ffmpegPath = @"C:\Program Files\ffmpeg-7.1-full_build\bin\ffmpeg.exe"; // свой путь
            string oggPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".ogg");

            try
            {
                if (!File.Exists(wavPath))
                {
                    Logger.Log($"WAV файл не найден: {wavPath}");
                    return null;
                }

                var process = new Process();
                process.StartInfo.FileName = ffmpegPath;
                process.StartInfo.Arguments = $"-i \"{wavPath}\" -c:a libopus -b:a 48k \"{oggPath}\"";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.Start();

                string stdOutput = process.StandardOutput.ReadToEnd();
                string stdError = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    Logger.Log($"FFmpeg завершился с ошибкой (код {process.ExitCode}): {stdError}");
                    return null;
                }

                if (!File.Exists(oggPath))
                {
                    Logger.Log($"FFmpeg не создал выходной файл OGG. stderr: {stdError}");
                    return null;
                }

                Logger.Log("Конвертация WAV в OGG прошла успешно.");
                return oggPath;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "ConvertWavToOgg");
                return null;
            }
        }



        private void PlayAudio(string path)
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
            }

            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
            }

            audioFileReader = new AudioFileReader(path);
            waveOut = new WaveOutEvent();
            waveOut.Init(audioFileReader);
            waveOut.Play();
        }
        private void UpdateLanguageButtonText()
        {
            button4.Text = isEnglishToRussian ? "EN-RU" : "RU-EN";
        }

        void LaunchScissors()
        {
            try
            {
                // Открытие встроенной утилиты для создания скриншота (Windows Snipping Tool)
                Process.Start("ms-screenclip:");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске Snipping Tool: {ex.Message}. Попробуйте открыть файл вручную.", "Ошибка");
                OpenFileDialogAndProcess();
            }
        }

        void OpenFileDialogAndProcess()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imagePath = openFileDialog.FileName;
                RecognizeTextAndSave(imagePath);
            }
        }

        void SelectImageAndProcess()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imagePath = openFileDialog.FileName;
                RecognizeTextAndSave(imagePath);
            }
        }

        public class TranslationResponse
        {
            public List<Translation> translations { get; set; }
        }

        public class Translation
        {
            public string text { get; set; }
        }

        public async Task<string> TranslateTextAsync(string textToTranslate, bool isEnglishToRussian)
        {
            if (string.IsNullOrWhiteSpace(textToTranslate))
                return string.Empty;

        
            string direction = isEnglishToRussian ? "EN-RU" : "RU-EN";

          

            try
            {
                using (var client = new HttpClient())
                {
                    var requestBody = new
                    {
                       
                        text = textToTranslate,
                        direction = isEnglishToRussian ? "EN-RU" : "RU-EN"
                    };

                    string jsonBody = JsonConvert.SerializeObject(requestBody);
                    
                    client.BaseAddress = new Uri("http://localhost:5000");
                    var response = await client.PostAsync(
                        "/translate",
                        new StringContent(jsonBody, Encoding.UTF8, "application/json")
                    );
                    string responseBody = await response.Content.ReadAsStringAsync();

                 
                    if (response.IsSuccessStatusCode)
                    {
                        var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
                        return json.ContainsKey("translation") ? json["translation"] : "ОШИБКА: Нет ответа";
                    }
                    else
                    {
                        return $"ОШИБКА: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"ОШИБКА: {ex.Message}";
            }
        }

        async void RecognizeTextAndSave(string imagePath)
        {
            
            string tessdataPath = @"C:\Users\HP\Desktop\курсач\TranslaterSQL\packages\Tesseract.5.2.0\tessdata";

           
            if (!Directory.Exists(tessdataPath))
            {
                string[] possiblePaths = {
                    Path.Combine(Application.StartupPath, "tessdata"),
                    @"C:\Program Files\Tesseract-OCR\tessdata",
                    @"C:\Program Files (x86)\Tesseract-OCR\tessdata"
                };

                foreach (var path in possiblePaths)
                {
                    if (Directory.Exists(path))
                    {
                        tessdataPath = path;
                        break;
                    }
                }

                // Если всё равно не нашли
                if (!Directory.Exists(tessdataPath))
                {
                    var result = MessageBox.Show(
                        "Не найдена директория с данными для Tesseract OCR. Хотите указать путь вручную?",
                        "Ошибка Tesseract",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error);

                    if (result == DialogResult.Yes)
                    {
                        using (var folderDialog = new FolderBrowserDialog())
                        {
                            folderDialog.Description = "Выберите директорию tessdata для Tesseract OCR";
                            if (folderDialog.ShowDialog() == DialogResult.OK)
                            {
                                tessdataPath = folderDialog.SelectedPath;
                            }
                            else
                            {
                                MessageBox.Show("Операция отменена пользователем.", "Отмена");
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Без данных Tesseract OCR распознавание текста невозможно.", "Ошибка");
                        return;
                    }
                }
            }

            try
            {
                // Проверка существования файла изображения
                if (!File.Exists(imagePath))
                {
                    MessageBox.Show($"Файл изображения не найден: {imagePath}", "Ошибка");
                    return;
                }

                // Показываем индикатор прогресса
                this.Cursor = Cursors.WaitCursor;

                using (var engine = new TesseractEngine(tessdataPath, "eng+rus", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        var page = engine.Process(img);
                        string recognizedText = page.GetText().Trim();

                        if (string.IsNullOrWhiteSpace(recognizedText))
                        {
                            MessageBox.Show("Текст не распознан. Попробуйте другое изображение с более четким текстом.", "Предупреждение");
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        // Обновляем текст в поле распознанного текста
                        this.recognizedBox.Text = recognizedText;

                        // Переводим текст
                        string translatedText = await TranslateTextAsync(recognizedText, isEnglishToRussian);

                        // Обновляем текст в поле перевода
                        this.translateBox.Text = translatedText;

                        // Добавляем перевод в историю, только если он успешен (не начинается с "ОШИБКА:")
                        if (!translatedText.StartsWith("ОШИБКА:"))
                        {
                            // Логируем перевод
                            LogTranslation(recognizedText, translatedText, isEnglishToRussian);

                            // Сохраняем в список переводов
                            translatedWords.Add(new KeyValuePair<string, string>(recognizedText, translatedText));
                            DB db = new DB();

                            // Берём текущий ID пользователя (предполагаю, что он есть в статическом поле loginForm)
                            int userId = loginForm.CurrentUserId;

                            if (userId != -1) // проверяем, что пользователь залогинен
                            {
                                string sourceLang = isEnglishToRussian ? "en" : "ru";
                                string targetLang = isEnglishToRussian ? "ru" : "en";

                                // Сохраняем перевод в БД
                                db.SaveTranslationHistoryAsync(userId, sourceLang, targetLang, recognizedText, translatedText);

                            }
                            // Обновляем список истории
                            UpdateHistoryBox();
                        }
                    }
                }

                // Озвучивание перевода
                SpeakTranslatedText();

                // Возвращаем нормальный курсор
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Ошибка при распознавании текста: {ex.Message}", "Ошибка");
            }
        }

        private void UpdateHistoryBox()
        {
            historyBox.Items.Clear();
            foreach (var pair in translatedWords)
            {
                // Создаем краткую строку для отображения в списке (первые 30 символов оригинала)
                string originalPreview = pair.Key.Length > 30 ? pair.Key.Substring(0, 27) + "..." : pair.Key;
                historyBox.Items.Add(originalPreview);
            }

            // Выбираем последнюю запись, если она есть
            if (historyBox.Items.Count > 0)
            {
                historyBox.SelectedIndex = historyBox.Items.Count - 1;
            }
        }

        private void LogTranslation(string originalText, string translatedText, bool isEnglishToRussian)
        {
            try
            {
                string direction = isEnglishToRussian ? "EN → RU" : "RU → EN";
                string logEntry = $"[{DateTime.Now}] {direction}\nОригинал: {originalText}\nПеревод: {translatedText}\n----------------------\n";

                string logPath = Path.Combine(Application.StartupPath, "translation_log.txt");
                File.AppendAllText(logPath, logEntry, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при записи в лог: {ex.Message}", "Ошибка логирования");
            }
        }

        private void SpeakTranslatedText()
        {
            string textToSpeak = translateBox.Text;
            if (!string.IsNullOrEmpty(textToSpeak) && !textToSpeak.StartsWith("ОШИБКА:"))
            {
                try
                {
                    // Устанавливаем язык в зависимости от направления перевода
                    if (!isEnglishToRussian) // Если переводим на английский
                    {
                        speechSynthesizer.SelectVoiceByHints(VoiceGender.NotSet, VoiceAge.NotSet, 0,
                                                            new System.Globalization.CultureInfo("en-US"));
                    }
                    else // Если переводим на русский
                    {
                        // Проверяем наличие русского голоса
                        bool hasRussianVoice = false;
                        foreach (var voice in speechSynthesizer.GetInstalledVoices())
                        {
                            if (voice.VoiceInfo.Culture.Name.StartsWith("ru"))
                            {
                                speechSynthesizer.SelectVoice(voice.VoiceInfo.Name);
                                hasRussianVoice = true;
                                break;
                            }
                        }

                        if (!hasRussianVoice)
                        {
                            // Если русского голоса нет, используем любой доступный
                            speechSynthesizer.SelectVoiceByHints(VoiceGender.NotSet, VoiceAge.NotSet);
                        }
                    }

                    speechSynthesizer.SpeakAsync(textToSpeak);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при озвучивании текста: {ex.Message}", "Ошибка");
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                int id = (int)m.WParam;
                if (id == HOTKEY_SCISSORS) // ALT + T
                {
                    LaunchScissors();
                }
                else if (id == HOTKEY_OPEN_FILE) // ALT + O
                {
                    OpenFileDialogAndProcess();
                }
            }

            base.WndProc(ref m);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Отменяем регистрацию горячих клавиш при закрытии формы
            UnregisterHotKey(this.Handle, HOTKEY_SCISSORS);
            UnregisterHotKey(this.Handle, HOTKEY_OPEN_FILE);

            base.OnFormClosing(e);
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

        private void Button1_Click(object sender, EventArgs e) // speakerButton
        {
            SpeakTranslatedText();
        }

        private void button2_Click(object sender, EventArgs e) // clearButton
        {
            translatedWords.Clear();
            historyBox.Items.Clear();
            recognizedBox.Text = string.Empty;
            translateBox.Text = string.Empty;
        }
        private async Task<string> ExtractTextFromImageAsync(string imagePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    string tessDataPath = @"C:\Users\HP\Desktop\курсач\TranslaterSQL\packages\Tesseract.5.2.0\tessdata";  // Папка с языковыми данными
                    string lang = "eng+rus";  // Распознаём английский и русский (можно оставить только "rus" или "eng")

                    using (var engine = new TesseractEngine(tessDataPath, lang, EngineMode.Default))
                    using (var img = Pix.LoadFromFile(imagePath))
                    using (var page = engine.Process(img))
                    {
                        string text = page.GetText();
                        return text?.Trim();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка распознавания текста: {ex.Message}", "Ошибка");
                    return null;
                }
            });
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsImage())
                {
                    var image = Clipboard.GetImage();
                    if (image != null)
                    {
                        string tempImagePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".png");
                        image.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Png);

                        // Распознаём текст с картинки
                        string extractedText = await ExtractTextFromImageAsync(tempImagePath);

                        if (!string.IsNullOrEmpty(extractedText))
                        {
                            recognizedBox.Text = extractedText;
                            string translatedText = await TranslateTextAsync(extractedText, isEnglishToRussian);
                            translateBox.Text = translatedText;

                            // ← добавь сохранение:
                            if (!translatedText.StartsWith("ОШИБКА:"))
                            {
                                int userId = loginForm.CurrentUserId;
                                Console.WriteLine($"button3 userId={userId}");
                                if (userId != -1)
                                {
                                    DB db = new DB();
                                    db.SaveTranslationHistoryAsync(userId,
                                        isEnglishToRussian ? "en" : "ru",
                                        isEnglishToRussian ? "ru" : "en",
                                        extractedText, translatedText);
                                }
                                translatedWords.Add(new KeyValuePair<string, string>(extractedText, translatedText));
                                UpdateHistoryBox();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Не удалось распознать текст на изображении.", "Информация");
                        }

                        File.Delete(tempImagePath);
                    }
                }
                else
                {
                    MessageBox.Show("В буфере обмена отсутствует изображение.", "Ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private void button4_Click(object sender, EventArgs e) // languageButton
        {
            // Меняем направление перевода
            isEnglishToRussian = !isEnglishToRussian;
            UpdateLanguageButtonText();
            
            // Если есть текст для перевода, выполняем перевод в новом направлении
            if (!string.IsNullOrEmpty(recognizedBox.Text))
            {
                Task.Run(async () =>
                {
                    string translatedText = await TranslateTextAsync(recognizedBox.Text, isEnglishToRussian);

                    // Используем Invoke для безопасного обновления UI из другого потока
                    this.Invoke((MethodInvoker)delegate
                    {
                        translateBox.Text = translatedText;

                        // Если перевод успешен, обновляем историю
                        if (!translatedText.StartsWith("ОШИБКА:"))
                        {
                            LogTranslation(recognizedBox.Text, translatedText, isEnglishToRussian);
                            translatedWords.Add(new KeyValuePair<string, string>(recognizedBox.Text, translatedText));
                            UpdateHistoryBox();
                            SpeakTranslatedText();
                        }
                    });
                });
            }
        }
   
        private void historyBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Показываем выбранный перевод из истории
            int index = historyBox.SelectedIndex;
            if (index >= 0 && index < translatedWords.Count)
            {
                var pair = translatedWords[index];
                recognizedBox.Text = pair.Key;
                translateBox.Text = pair.Value;
            }
        }

       
        private async void button1_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Аудио файлы|*.mp3;*.wav";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string audioPath = openFileDialog.FileName;

                    // Открываем аудио в системном плеере (как видео)
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = audioPath,
                        UseShellExecute = true
                    });

                    // Проверяем: если это не WAV 16kHz, конвертируем во временный WAV
                    string tempWavPath = audioPath;
                    if (!audioPath.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                    {
                        string ffmpegPath = @"C:\Program Files\ffmpeg-7.1-full_build\bin\ffmpeg.exe";
                        tempWavPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".wav");

                        var process = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = ffmpegPath,
                                Arguments = $"-i \"{audioPath}\" -ar 16000 -ac 1 -c:a pcm_s16le \"{tempWavPath}\"",
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true
                            }
                        };
                        process.Start();
                        process.WaitForExit();
                        if (process.ExitCode != 0 || !File.Exists(tempWavPath))
                        {
                            MessageBox.Show("Ошибка конвертации аудио", "Ошибка");
                            return;
                        }
                    }

                    // Распознаём и переводим
                    await RecognizeAndTranslateAudioChunks(tempWavPath);

                    // Удаляем временный WAV (если был создан)
                    if (tempWavPath != audioPath)
                    {
                        try { File.Delete(tempWavPath); } catch { }
                    }
                }
            }
        }

        private async Task RecognizeAndTranslateAudioChunks(string wavPath)
        {
            var chunks = SplitAudioByTime(wavPath, 10);
            recognizedBox.Clear();
            translateBox.Clear();

            DB db = new DB();

            foreach (var chunk in chunks)
            {
                var (recognizedText, translatedText) = await RecognizeSpeech(chunk, isEnglishToRussian); // ← деструктуризация кортежа
                if (!string.IsNullOrWhiteSpace(recognizedText))
                {
                    recognizedBox.AppendText(recognizedText + Environment.NewLine);
                    translateBox.AppendText(translatedText + Environment.NewLine);

                    int userId = loginForm.CurrentUserId;
                    if (userId != -1)
                    {
                        db.SaveTranslationHistoryAsync(userId,
                            isEnglishToRussian ? "en" : "ru",
                            isEnglishToRussian ? "ru" : "en",
                            recognizedText,
                            translatedText);
                    }
                }

                try { File.Delete(chunk); } catch { }
            }
        }


        private async void button2_Click_1(object sender, EventArgs e)// видео
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Видео файлы|*.mp4;*.avi;*.mov";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string videoPath = openFileDialog.FileName;
                    Process.Start(videoPath);

                    string wavPath = ExtractAudioFromVideo(videoPath);
                    if (wavPath != null)
                    {
                        await RecognizeAndTranslateAudioChunks(wavPath);
                        try { File.Delete(wavPath); } catch { }
                    }
                    else
                    {
                        MessageBox.Show("Не удалось извлечь аудио из видео", "Ошибка");
                    }
                }
            }
        }
    }
}