using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wordle_analogue
{
    public partial class Form2 : Form
    {
        private int offset;
        private string guessedWord;
        private Color[] colors;
        private int grid_param;
        private Bitmap buffer;
        public Form2()
        {
            InitializeComponent();
            offset = 50;
            grid_param = 0;
            pictureBox1.Paint += pictureBox1_Paint;
            buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void UpdateBuffer()
        {
            using (Graphics g = Graphics.FromImage(buffer))
            {
                var brush = new SolidBrush(Color.Gray);
                if (colors != null)
                {
                    var X = pictureBox1.Width / colors.Length;
                    var Y = pictureBox1.Height / colors.Length / 2;
                    using (Font font = new Font("Arial", 40, FontStyle.Bold))
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;

                        for (int i = 0; i < colors.Length; i++)
                        {
                            brush.Color = colors[i];
                            g.FillRectangle(brush, X * i, Y * grid_param * 2, Y + offset, Y + offset);

                            string letter = guessedWord[i].ToString();
                            g.DrawString(letter, font, Brushes.Black, X * i + offset, Y * grid_param * 2 + offset, sf);
                        }
                    }
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (buffer != null)
            {
                e.Graphics.DrawImage(buffer, 0, 0);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            guessedWord = textBox1.Text.ToLower();
            colors = GuessWord.ColorsForChars(guessedWord);
            UpdateBuffer();
            pictureBox1.Refresh();
            grid_param++;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            buffer?.Dispose();
            base.OnFormClosing(e);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_Load_1(object sender, EventArgs e)
        {

        }
    }
}
