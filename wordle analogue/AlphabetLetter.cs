using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wordle_analogue
{
    public class AlphabetLetter
    {
        public char letter;
        public Color Color;
        public double X;
        public double Y;

        public AlphabetLetter(char letter, double x, double y)
        {
            this.letter = letter;
            Color = Color.Transparent;
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
            Color = Color.Transparent;
        }
    }
}
