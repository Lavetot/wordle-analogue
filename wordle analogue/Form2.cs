using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace wordle_analogue
{
    // Основная форма (отгадывание слова)
    public partial class Form2 : Form
    {
        // Поля
        private int offset;
        private string guessedWord;
        private Color[] colors;
        private int grid_param;
        private Bitmap buffer;
        private Bitmap alphabetBuffer;
        private Image AlphabetImage;
        private Dictionary<char, AlphabetLetter> alphabetLetters;
        private string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя012";
        public Form2()
        {
            Directory.CreateDirectory("last_game"); // Создаем директорию last_game, если ее еще не было
            InitializeComponent();

            WordLength.Text = $"Длина загаданного слова: {GuessWord.Word.Length}";

            offset = 50; // Задаем оффсет для создания расстояния между квадратами
            grid_param = 0; // Параметр для отрисовки квадратов ниже

            pictureBox1.Paint += pictureBox1_Paint;
            AlphabetPicture.Paint += AlphabetPicture_Paint;

            buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height); // Буфер для сохранения отрисованных квадратов
            alphabetBuffer = new Bitmap(AlphabetPicture.Width, AlphabetPicture.Height); // Буфер для сохранения отрисованных под буквами алфавита цветов
            AlphabetImage = new Bitmap(AlphabetPicture.Image); // Копируем изображение алфавита с формы

            alphabetLetters = new Dictionary<char, AlphabetLetter>(); // Инициализируем словарь букв из алфавита
            InitLetters(); // Инициализируем буквы (даем им символы и координаты)
        }

        // Метод для обновления буффера
        private void UpdateBuffer()
        {
            using (Graphics g = Graphics.FromImage(buffer))
            {
                var brush = new SolidBrush(Color.Gray); // Создаем brush
                if (colors != null) // Проверяем, что colors непустой
                {
                    var X = (float)pictureBox1.Width / colors.Length; // базовая X-координата квадрата
                    var Y = (float)pictureBox1.Height / (colors.Length + 1) / 2; // базовая Y-координата квадрата (делим на 2, чтобы квадраты нормально располагались)
                    using (Font font = new Font("Arial", 40, FontStyle.Bold)) // Задаем шрифт для текста в квадратах
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.Alignment = StringAlignment.Center; // Центрируем
                        sf.LineAlignment = StringAlignment.Center;

                        for (int i = 0; i < colors.Length; i++) // Цикл для отрисовки квадратов
                        {
                            brush.Color = colors[i]; // Берем цвет из colors
                            g.FillRectangle(brush, X * i, Y * grid_param * 2, Y + offset, Y + offset); // Собственно отрисовываем квадрат
                                                                                                       // (grid_param умножаем на 2, чтобы выглядело нормально)

                            string letter = guessedWord[i].ToString(); // Берем букву из слова
                            g.DrawString(letter, font, Brushes.Black, X * i + offset, Y * grid_param * 2 + offset, sf); // Отрисовываем ее в квадрате
                        }
                    }
                }
            }
        }

        // Метод для инициализации
        // Метод дает каждой букве определенный символ и значение координат (левый верхний угол квадрата)
        private void InitLetters()
        {
            foreach (var letter in alphabet)
            {
                alphabetLetters.Add(letter, new AlphabetLetter(letter)); // Добавляем буквы в словарь
            }
            int columns = 6; // Количество столбцов
            float cellWidth = AlphabetPicture.Width / columns; // Ширина квадрата сетки
            float cellHeight = AlphabetPicture.Height / columns; // Высота квадрата сетки

            int index = 0; // Индекс буквы
            foreach (var letter in alphabetLetters.Values)
            {
                int col = index % columns; // Столбец
                int row = index / columns; // Строка

                letter.X = col * cellWidth; // Даем координаты соответствующей букве
                letter.Y = row * cellHeight;

                index++;
            }
        }

        // Метод для обновления буфера алфавита
        private void UpdateAlphabetBuffer()
        {
            using (Graphics g = Graphics.FromImage(alphabetBuffer))
            {
                var brush = new SolidBrush(Color.Transparent); // По умолчанию делаем brush прозрачным
                List<char> alphabetChars = alphabetLetters.Keys.ToList(); // Выделяем символы из словаря букв
                if (colors != null)
                {
                    var X = AlphabetPicture.Width / 6; // Координата X квадрата сетки
                    var Y = AlphabetPicture.Height / 6; // Координата Y квадрата сетки
                    for (int i = 0; i < colors.Length; i++)
                    {
                        var index = alphabetChars.IndexOf(guessedWord[i]); // Берем индекс буквы из словаря
                        var letter_color = alphabetLetters[alphabetChars[index]].Color;
                        if (colors[i] == Color.LightGreen || (colors[i] == Color.Yellow && (letter_color == Color.Gray || letter_color == Color.Yellow)))
                        {
                            letter_color = colors[i]; // Находим соответствующий букве цвет
                            alphabetLetters[alphabetChars[index]].Color = colors[i];
                            brush.Color = colors[i];
                        }
                        else
                        {
                            brush.Color = letter_color;
                        }
                        g.FillRectangle(brush, (float)alphabetLetters[alphabetChars[index]].X, (float)alphabetLetters[alphabetChars[index]].Y, X, Y); // Отрисовываем квадрат
                    }
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (buffer != null) // Проверка на то, что буфер непустой
                e.Graphics.DrawImage(buffer, 0, 0); // Отрисовываем буфер 
        }

        // Кнопка "Отправить" (можно нажимать энтер для отправки)
        private void button1_Click(object sender, EventArgs e)
        {
            Updater();
        }

        // Переписанный метод закрытия формы
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            var culture = new CultureInfo("ru-RU"); // 
            DateTime tempTime = DateTime.Now; // Получаем текущее время системы
            string now = tempTime.ToString(culture).Replace(':', '-'); // Переводим его в строку, меняем двоеточия на -
                                                                       // чтобы можно было сохранить картинку
            buffer.Save($"last_game/{now}.png"); // Сохраняем картинку
            using (Graphics g = Graphics.FromImage(alphabetBuffer))
            {
                g.DrawImage(AlphabetImage, 0, 0);
            }
            alphabetBuffer.Save($"last_game/{now}-alphabet.png");
            buffer?.Dispose(); // Избавляеся от буферов
            alphabetBuffer?.Dispose();
            AlphabetImage?.Dispose();
            base.OnFormClosing(e);
        }


        // Метод для обработки событий нажатия кнопок
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) // Проверяем нажатие энтера
            {
                // Все то же самое, что и в кнопке "Отправить"
                e.Handled = true; // Убираем системный звук нажатия кнопки
                Updater();
            }
        }

        private void AlphabetPicture_Paint(object sender, PaintEventArgs e)
        {
            if (alphabetBuffer != null)
            {
                e.Graphics.DrawImage(alphabetBuffer, 0, 0); // Рисуем буфер с цветными квадратами
                e.Graphics.DrawImage(AlphabetImage, 0, 0); // Отрисовываем алфавит поверх квадратов
            }
        }

        // Метод для обновления pictureBox1 и AlphabetPicture 
        private void Updater()
        {
            guessedWord = textBox1.Text.ToLower(); // Получаем слово и приводим его в нижний регистр
            textBox1.Clear();
            if (GuessWord.Dict.Contains(guessedWord) && guessedWord.Length == GuessWord.Word.Length)
            {
                colors = GuessWord.ColorsForChars(guessedWord); // Узнаем, насколько оно соответствует загаданному слову
                UpdateBuffer(); // Обновляем буфер
                UpdateAlphabetBuffer();
                pictureBox1.Refresh(); // Обновляем pictureBox
                AlphabetPicture.Refresh();
                grid_param++; // Прибавляем grid_param (перемещаемся вниз)
            }
            else if (GuessWord.Dict.Contains(guessedWord) && guessedWord.Length != GuessWord.Word.Length)
            {
                MessageBox.Show("Длина загаданного слова не соответствует длине данного слова");
            }
            else
            {
                MessageBox.Show("Данного слова нет в словаре");
            }
            
        }
    }
}
