using System;
using System.Collections.Generic;
using System.Text;

namespace wordle_analogue
{
    // Основной класс игры
    public static class GuessWord
    {
        public static string Word; // Поле Word, в которое мы сохраняем загаданное слово с первой формы

        // Метод для сравнения загаданного слова и некоторого слова
        public static Color[]? ColorsForChars(string guessedWord)
        {
            // Если длина некоторого слова совпадает с длиной загаданного слова
            if (guessedWord.Length == Word.Length)
            {
                Color[] colors = new Color[Word.Length];    // Массив цветов с длиной, равной длине загаданного слова
                for (int i = 0; i < Word.Length; i++)       // Цикл, в котором происходит сравнение слов
                {
                    if (guessedWord[i] == Word[i])          // Если буква на своем месте (позиция буквы в загаданном слове совпадает с позицией буквы в некотором слове)
                        colors[i] = Color.LightGreen;       // Присваиваем светло-зеленый цвет
                    else if (Word.Contains(guessedWord[i])) // Если буква содержится в слове, но не находится на своем месте
                        colors[i] = Color.Yellow;           // Присваиваем желтый цвет
                    else                                    // Если буквы нет в слове
                        colors[i] = Color.Gray;             // Присваиваем серый цвет
                }
                return colors;                              // Возвращаем массив
            }
            // Если длина некоторого слова не совпадает с длиной загаданного слова
            else
            {
                throw new ArgumentOutOfRangeException("Длина вписанного слова не равна длине загаданного слова"); // Выкидываем сообщение об ошибке
                return null; // Возвращаем null
            }
        }
    }
}
