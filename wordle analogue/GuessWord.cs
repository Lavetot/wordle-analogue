using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace wordle_analogue
{
    // Основной класс игры
    public static class GuessWord
    {
        public static string Word; // Поле Word, в которое мы сохраняем загаданное слово с первой формы
        public static readonly List<string> Dict = File.ReadLines("dictionary_utf-8.txt").ToList();

        // Метод для сравнения загаданного слова и некоторого слова
        public static Color[]? ColorsForChars(string guessedWord)
        {
            List<char> WordChars = Word.ToList(); // Список с символами из загаданного слова
            List<char> guessedWordChars = guessedWord.ToList(); // Список с символами из вписанного слова
            // Если длина некоторого слова совпадает с длиной загаданного слова
            if (guessedWord.Length == Word.Length)
            {
                Color[] colors = new Color[Word.Length];    // Массив цветов с длиной, равной длине загаданного слова
                for (int i = 0; i < Word.Length; i++)       // Цикл, в котором происходит побуквенное сравнение слов
                {
                    colors[i] = Color.Gray; // Изначально присваиваем букве серый цвет
                    if (guessedWord[i] == Word[i]) // Если буквы совпадают
                    {
                        colors[i] = Color.LightGreen; // Присваиваем светло-зеленый цвет
                        WordChars[i] = ' '; // Убираем букву из списков с символами
                        guessedWordChars[i] = ' ';
                    }    
                }
                // Цикл для определения желтого цвета у букв
                for (int i = 0; i < Word.Length; i++)
                {
                    int index = WordChars.IndexOf(guessedWordChars[i]); // Индекс соответствующей буквы из вписанного слова у загаданного слова
                    if (index != -1 && guessedWordChars[i] != ' ' && colors[i] != Color.LightGreen) // Проверяем, существует ли буква в двух словах, и что ее цвет не светло-зеленый
                    {
                        colors[i] = Color.Yellow; // Если да, то присваиваем букве желтый цвет
                        WordChars[index] = ' ';   // и обнуляем ее
                    }
                }
                return colors; // Возвращаем массив
            }
            // Если длина некоторого слова не совпадает с длиной загаданного слова
            else
            {
                MessageBox.Show("Длина вписанного слова не равна длине загаданного слова");
                return null; // Возвращаем null
            }
        }
    }
}
