namespace wordle_analogue
{
    // Форма для загадывания слова
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Кнопка "Загадать"
        private void button1_Click(object sender, EventArgs e)
        {
            Updater();
        }

        // Метод для обработки нажатий
        private void WordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) // Проверяем, нажали ли энтер
            {
                // Все то же самое, что и у кнопки "Загадать"
                e.Handled = true; // Убираем системный звук нажатия кнопки
                Updater();
            }
        }

        private void Updater()
        {
            GuessWord.Word = WordTextBox.Text.ToLower(); // Получаем загаданное слово и отправляем его в статический класс GuessWord
            WordTextBox.Clear();
            if (GuessWord.Dict.Contains(GuessWord.Word) && GuessWord.Word != "")
            {
                Form2 newForm = new Form2(); // Создаем Form2
                newForm.FormClosed += (s, args) => Application.Exit(); // Form2 будет закрывать эту форму при собственно закрытии newForm
                newForm.Show(); // Отображаем вторую форму
                this.Hide(); // Скрываем первую форму (она не закрывается, а просто скрывается, так что можно ее вызвать повторно при желании)
            }
            else
            {
                MessageBox.Show("Данного слова нет в словаре");
            }
        }

        private void PickRandomWordButton_Click(object sender, EventArgs e)
        {
            GuessWord.PickRandomWord();
            Form2 newForm = new Form2(); // Создаем Form2
            newForm.FormClosed += (s, args) => Application.Exit(); // Form2 будет закрывать эту форму при собственно закрытии newForm
            newForm.Show(); // Отображаем вторую форму
            this.Hide(); // Скрываем первую форму (она не закрывается, а просто скрывается, так что можно ее вызвать повторно при желании)
        }
    }
}
