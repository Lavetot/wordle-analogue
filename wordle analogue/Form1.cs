namespace wordle_analogue
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GuessWord.Word = WordTextBox.Text.ToLower();

            Form2 newForm = new Form2();
            newForm.FormClosed += (s, args) => Application.Exit();
            newForm.Show();
            this.Hide();
        }
    }
}
