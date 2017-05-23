using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalysisOfKeywordsBehaviour
{
    /// <summary>
    /// Главная форма.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Экспериментальные слова.
        /// </summary>
        public List<string> AllWords;
        /// <summary>
        /// Маркемы.
        /// </summary>
        public List<string> Markems;
        /// <summary>
        /// Дефиниции.
        /// </summary>
        public List<string> Definitions;
        /// <summary>
        /// Свободные ассоциации.
        /// </summary>
        public List<string> FreeAssociations;
        /// <summary>
        /// Направленные ассоциации.
        /// </summary>
        public List<string> DirectAssociations;
        /// <summary>
        /// Симиляры.
        /// </summary>
        public List<string> Similarities;
        /// <summary>
        /// Оппозиты.
        /// </summary>
        public List<string> Opposities;
        /// <summary>
        /// "Важные слова".
        /// </summary>
        public List<string> ImportantWords;

        /// <summary>
        /// Экземпляр класса по построению ассоциативного поля экспериментального слова.
        /// </summary>
        private FieldConstruction _myfield;

        private TextProcessing _text;
        private WordsProcessing _words;

        /// <summary>
        /// Проверяет, какие действия могут быть доступны пользователю во время простоя приложения.
        /// </summary>
        void Idle(object sender, EventArgs e)
        {
            btnFieldsForAllWords.Enabled = (AllWords.Count != 0) && (Markems.Count != 0) && (Definitions.Count != 0) && (FreeAssociations.Count != 0) && (DirectAssociations.Count != 0) && (Similarities.Count != 0) && (Opposities.Count != 0);
            btnFieldForSelectedWord.Enabled = (comboBoxAllWords.SelectedItem != null) && (AllWords.Count != 0) && (Markems.Count != 0) && (Definitions.Count != 0) && (FreeAssociations.Count != 0) && (DirectAssociations.Count != 0) && (Similarities.Count != 0) && (Opposities.Count != 0);
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            /*AllWords = new List<string>();
            Markems = new List<string>();
            Definitions = new List<string>();
            FreeAssociations = new List<string>();
            DirectAssociations = new List<string>();
            Similarities = new List<string>();
            Opposities = new List<string>();
            ImportantWords = new List<string>();*/
            Application.Idle += Idle;
        }

        /// <summary>
        /// Cчитывает данные из текстового файла в соответствующий список с экспериментальными данными.
        /// </summary>
        /// <param name="path">Путь к текстовому файлу, из которого необходимо считать экспериментальные данные.</param>
        /// <param name="list">Список строк, куда заносится соответствующий список с экспериментальными данными.</param>
        private void FillInList(string path, List<string> list)
        {
            string[] lines = File.ReadAllLines(path, Constants.ENCODING);
            list = new List<string>();
            for (int i = 0; i < lines.Length; i++)
                list.Add(lines[i]);
        }

        /*набор методов, предназначенный для работы с загрузкой экспериментальных данных*/
        //---------------------------------------------------------------------------------------------------//
        private void экспериментальныеСловаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFD.FileName;
                FillInList(fileName, AllWords);

                _words = new WordsProcessing();
                _words.ProcessWords(AllWords);

                comboBoxAllWords.Items.Clear();
                dataGridViewAllWords.Rows.Clear();
                dataGridViewWords.Rows.Clear();
                foreach (string word in AllWords)
                {
                    dataGridViewAllWords.Rows.Add(word);
                    comboBoxAllWords.Items.Add(word);
                }
            }
        }

        private void маркемToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFD.FileName;
                FillInList(fileName, Markems);
            }
        }

        private void дефиницииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFD.FileName;
                FillInList(fileName, Definitions);
            }
        }

        private void свободныеАссоциацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFD.FileName;
                FillInList(fileName, FreeAssociations);
            }
        }

        private void направленныеАссоциацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFD.FileName;
                FillInList(fileName, DirectAssociations);
            }
        }

        private void симилярыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFD.FileName;
                FillInList(fileName, Similarities);
            }
        }

        private void оппозитыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFD.FileName;
                FillInList(fileName, Opposities);
            }
        }

        private void важныеСловаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFD.FileName;
                FillInList(fileName, ImportantWords);
            }
        }

        private void текстДляАнализаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                //Cursor = Cursors.WaitCursor;
                string fileName = openFD.FileName;
                _text = new TextProcessing();
                _text.ProcessText(fileName);
                //Cursor = Cursors.Default;
            }
        }

        //---------------------------------------------------------------------------------------------------//
    }
}
