using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wordle_analogue
{
    // Класс для буквы
    public class AlphabetLetter
    {
        public char letter; // Символ, который соответствует букве
        public Color Color; // Цвет буквы (светло-зеленый, желтый, серый)
        public double X; // X-координата буквы на изображении
        public double Y; // Y-координата буквы на сетке

        public AlphabetLetter(char letter, double x, double y)
        {
            this.letter = letter;
            Color = Color.Gray;
            X = x;
            Y = y;
        }

        public AlphabetLetter(char letter, double x, double y, Color color)
        {
            this.letter = letter;
            Color = color;
            X = x;
            Y = y;
        }

        public AlphabetLetter(char letter)
        {
            this.letter = letter;
            Color = Color.Gray;
        }
    }
}
