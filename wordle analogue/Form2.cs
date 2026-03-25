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
        public Form2()
        {
            Directory.CreateDirectory("last_game"); // Создаем директорию last_game, если ее еще не было
            InitializeComponent();
            offset = 50; // Задаем оффсет для создания расстояния между квадратами
            grid_param = 0; // Параметр для отрисовки квадратов ниже
            pictureBox1.Paint += pictureBox1_Paint;
            buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height); // Буфер для сохранения отрисованных квадратов
        }

        // Метод для обновления буффера
        private void UpdateBuffer() 
        {
            using (Graphics g = Graphics.FromImage(buffer))
            {
                var brush = new SolidBrush(Color.Gray); // Создаем brush
                if (colors != null) // Проверяем, что colors непустой
                {
                    var X = pictureBox1.Width / colors.Length; // базовая X-координата квадрата
                    var Y = pictureBox1.Height / colors.Length / 2; // базовая Y-координата квадрата (делим на 2, чтобы квадраты нормально располагались)
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

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (buffer != null) // Проверка на то, что буфер непустой
            {
                e.Graphics.DrawImage(buffer, 0, 0); // Отрисовываем буфер 
            }

        }

        // Кнопка "Отправить" (можно нажимать энтер для отправки)
        private void button1_Click(object sender, EventArgs e)
        {
            guessedWord = textBox1.Text.ToLower(); // Получаем слово и приводим его в нижний регистр
            colors = GuessWord.ColorsForChars(guessedWord); // Узнаем, насколько оно соответствует загаданному слову
            UpdateBuffer(); // Обновляем буфер
            pictureBox1.Refresh(); // Обновляем pictureBox
            grid_param++; // Прибавляем grid_param (перемещаемся вниз)
        }

        // Переписанный метод закрытия формы
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            var culture = new CultureInfo("ru-RU"); // 
            DateTime tempTime = DateTime.Now; // Получаем текущее время системы
            string now = tempTime.ToString(culture).Replace(':', '-'); // Переводим его в строку, меняем двоеточия на -
                                                                       // чтобы можно было сохранить картинку
            buffer.Save($"last_game/{now}.png"); // Сохраняем картинку
            buffer?.Dispose(); // Избавляеся от буфера
            base.OnFormClosing(e);
        }


        // Метод для обработки событий нажатия кнопок
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) // Проверяем нажатие энтера
            {
                // Все то же самое, что и в кнопке "Отправить"
                e.Handled = true; // Убираем системный звук нажатия кнопки
                guessedWord = textBox1.Text.ToLower(); // Получаем слово и приводим его в нижний регистр
                colors = GuessWord.ColorsForChars(guessedWord); // Узнаем, насколько оно соответствует загаданному слову
                UpdateBuffer(); // Обновляем буфер
                pictureBox1.Refresh(); // Обновляем pictureBox
                grid_param++; // Прибавляем grid_param (перемещаемся вниз)
            }
        }
    }
}
