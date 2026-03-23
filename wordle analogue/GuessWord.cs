using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wordle_analogue
{
    public static class GuessWord
    {
        public static string Word;

        public static Color[] ColorsForChars(string guessedWord)
        {
            Color[] colors = new Color[Word.Length];
            if (guessedWord.Length == Word.Length)
            {
                for (int i = 0; i < Word.Length; i++)
                {
                    if (guessedWord[i] == Word[i])
                        colors[i] = Color.LightGreen;
                    else if (Word.Contains(guessedWord[i]))
                        colors[i] = Color.Yellow;
                    else
                        colors[i] = Color.Gray;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Длина вписанного слова не равна длине загаданного слова");
            }

            return colors;
        }
    }
}
