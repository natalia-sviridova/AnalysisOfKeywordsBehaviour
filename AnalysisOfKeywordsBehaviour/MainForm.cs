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
        /// Экземпляр класса по построению ассоциативного поля экспериментального слова.
        /// </summary>
        private FieldConstruction _myfield;
        /// <summary>
        /// Экземпляр класса по обработке контекстов.
        /// </summary>
        private TextProcessing _text;
        /// <summary>
        /// Экземпляр класса по расчету совместной встречаемости экспериментальных слов.
        /// </summary>
        private WordsProcessing _words;
        /// <summary>
        /// Интерфейс, экспортирующий полученные результаты.
        /// </summary>
        private IResultWriter _resultWriter;

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
        /// Проверяет, какие действия могут быть доступны пользователю во время простоя приложения.
        /// </summary>
        void Idle(object sender, EventArgs e)
        {
            btnFieldForSelectedWord.Enabled = (comboBoxAllWords.SelectedItem != null) && (AllWords.Count != 0) && (Markems.Count != 0) && (Definitions.Count != 0) && (FreeAssociations.Count != 0) && (DirectAssociations.Count != 0) && (Similarities.Count != 0) && (Opposities.Count != 0);
            btnFieldsForAllWords.Enabled = (AllWords.Count != 0) && (Markems.Count != 0) && (Definitions.Count != 0) && (FreeAssociations.Count != 0) && (DirectAssociations.Count != 0) && (Similarities.Count != 0) && (Opposities.Count != 0);
            btnCalculateCooccurrence.Enabled = (_text != null) && (_words != null);
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public MainForm(IResultWriter resultWriter)
        {
            InitializeComponent();
            tlTip.SetToolTip(btnExportToExcel, "Экспортирует таблицы с результатами в MS Excel.");
            tlTip.SetToolTip(btnExportToWord, "Экспортирует текстовые результаты (тройные и четверные встречаемости) в MS Word.");
            Application.Idle += Idle;
            _resultWriter = resultWriter;

            AllWords = new List<string>();
            Markems = new List<string>();
            Definitions = new List<string>();
            FreeAssociations = new List<string>();
            DirectAssociations = new List<string>();
            Similarities = new List<string>();
            Opposities = new List<string>();
            ImportantWords = new List<string>();
        }

        /// <summary>
        /// Cчитывает данные из текстового файла в соответствующий список с экспериментальными данными.
        /// </summary>
        /// <param name="path">Путь к текстовому файлу, из которого необходимо считать экспериментальные данные.</param>
        /// <param name="list">Список строк, куда заносится соответствующий список с экспериментальными данными.</param>
        private void FillInList(string path, List<string> list)
        {
            string[] lines = File.ReadAllLines(path, Constants.ENCODING);
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

        /*набор методов, предназначенный для вызова вспомогательной формы для изменения соответствующего списка экспериментальных данных в GUI-приложении*/
        //---------------------------------------------------------------------------------------------------//
        private void экспериментальныхСловToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm form = new HelpForm(this, 0);
            form.Text = "Экспериментальные слова";
            form.Show();
        }

        private void маркемToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HelpForm form = new HelpForm(this, 1);
            form.Text = "Маркемы";
            form.Show();
        }

        private void дефиницийToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HelpForm form = new HelpForm(this, 2);
            form.Text = "Дефиниции";
            form.Show();
        }

        private void свободныхАссоциацийToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HelpForm form = new HelpForm(this, 3);
            form.Text = "Свободные ассоциации";
            form.Show();
        }

        private void направленныхАссоциацийToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HelpForm form = new HelpForm(this, 4);
            form.Text = "Направленные ассоциации";
            form.Show();
        }

        private void симиляровToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HelpForm form = new HelpForm(this, 5);
            form.Text = "Симиляры";
            form.Show();
        }

        private void оппозитовToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HelpForm form = new HelpForm(this, 6);
            form.Text = "Оппозиты";
            form.Show();
        }

        private void важныхСловToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HelpForm form = new HelpForm(this, 7);
            form.Text = "Важные слова";
            form.Show();
        }
        //---------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Проверяет списки с экспериментальными данными на корректность числа элементов.
        /// </summary>
        /// <returns>Возвращает true, если все списки содержат верное число элементов, false - в противном случае.</returns>
        private bool CheckLists()
        {
            if (AllWords.Count != Definitions.Count)
            {
                MessageBox.Show("Количество дефиниций не совпадает с количеством слов в экспериментальном списке!");
                return false;
            }
            if (AllWords.Count != FreeAssociations.Count)
            {
                MessageBox.Show("Количество свободных ассоциаций не совпадает с количеством слов в экспериментальном списке!");
                return false;
            }
            if (AllWords.Count != Similarities.Count)
            {
                MessageBox.Show("Количество симиляров не совпадает с количеством слов в экспериментальном списке!");
                return false;
            }
            if (AllWords.Count != Opposities.Count)
            {
                MessageBox.Show("Количество оппозитов не совпадает с количеством слов в экспериментальном списке!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Строит ассоциативное поле для выбранного слова.
        /// </summary>
        private void btnFieldForSelectedWord_Click(object sender, EventArgs e)
        {
            //проверяем списки с экспериментальными данными на корректность длины
            if (CheckLists())
            {
                dataGridViewWords.Rows.Clear();
                dataGridViewImportantWords.Rows.Clear();
                int index = AllWords.IndexOf(comboBoxAllWords.SelectedItem.ToString());
                _myfield = new FieldConstruction(AllWords, Definitions, FreeAssociations, DirectAssociations, Similarities, Opposities, ImportantWords);
                _myfield.InitField(comboBoxAllWords.SelectedItem.ToString());    //строим ассоциативное поле для выбранного слова

                //инициализируем параметры ассоциативного поля
                int NumOfSwitches = 0;
                int NumOfAllSwitches = 0;
                int NumOfContactors = _myfield.Contactors.Count;
                int NumOfAllContactors = _myfield.NumOfContactors;
                double NumOfSwAmongMarkems = 0;
                double NumOfConAmongMarkems = 0;
                double NumOfAllSwAmongMarkems = 0;
                double NumOfAllConAmongMarkems = 0;
                double NumOfConAmongImpWords = 0;
                double NumOfAllConAmongImpWords = 0;
                //вычисляем параметры
                for (int i = 0; i < AllWords.Count; i++)
                {
                    if (_myfield.AllWordsAsSwitches[AllWords[i]] != 0 || _myfield.AllWordsAsContactors[AllWords[i]] != 0)
                    {
                        dataGridViewWords.Rows.Add(AllWords[i], _myfield.AllWordsAsSwitches[AllWords[i]], _myfield.AllWordsAsContactors[AllWords[i]], _myfield.AllWordsAsSwitches[AllWords[i]] + _myfield.AllWordsAsContactors[AllWords[i]]);
                        if (_myfield.AllWordsAsSwitches[AllWords[i]] != 0)
                        {
                            NumOfSwitches++;
                            NumOfAllSwitches += _myfield.AllWordsAsSwitches[AllWords[i]];
                            if (Markems.IndexOf(AllWords[i]) != -1)
                            {
                                NumOfSwAmongMarkems++;
                                NumOfAllSwAmongMarkems += _myfield.AllWordsAsSwitches[AllWords[i]];
                            }
                        }
                        if (_myfield.AllWordsAsContactors[AllWords[i]] != 0 && Markems.IndexOf(AllWords[i]) != -1)
                        {
                            NumOfConAmongMarkems++;
                            NumOfAllConAmongMarkems += _myfield.AllWordsAsContactors[AllWords[i]];
                        }
                    }
                }
                for (int i = 0; i < ImportantWords.Count; i++)
                {
                    dataGridViewImportantWords.Rows.Add(ImportantWords[i], _myfield.ImpWordsAsContactors[ImportantWords[i]]);
                    if (_myfield.ImpWordsAsContactors[ImportantWords[i]] != 0)
                    {
                        NumOfConAmongImpWords++;
                        NumOfAllConAmongImpWords += _myfield.ImpWordsAsContactors[ImportantWords[i]];
                    }
                }

                double NumOfMarkemsInAll = NumOfSwAmongMarkems + NumOfConAmongMarkems;
                double NumOfAllMarkemsInAll = NumOfAllSwAmongMarkems + NumOfAllConAmongMarkems;

                double ShareOfMarkemsInSwitches;
                double ShareOfMarkemsInContactors;
                double ShareOfMarkemsInAllSwitches;
                double ShareOfMarkemsInAllContactors;

                double ShareOfMarkemsInAll;
                double ShareOfAllMarkemsInAll;

                double ShareOfImpWordsInContactors;
                double ShareOfImpWordsInAllContactors;

                if (NumOfSwitches == 0)
                {
                    ShareOfMarkemsInSwitches = 0;
                    ShareOfMarkemsInAllSwitches = 0;
                }
                else
                {
                    ShareOfMarkemsInSwitches = NumOfSwAmongMarkems / NumOfSwitches;
                    ShareOfMarkemsInAllSwitches = NumOfAllSwAmongMarkems / NumOfAllSwitches;
                }
                if (NumOfContactors == 0)
                {
                    ShareOfMarkemsInContactors = 0;
                    ShareOfMarkemsInAllContactors = 0;
                    ShareOfImpWordsInContactors = 0;
                    ShareOfImpWordsInAllContactors = 0;
                }
                else
                {
                    ShareOfMarkemsInContactors = NumOfConAmongMarkems / NumOfContactors;
                    ShareOfMarkemsInAllContactors = NumOfAllConAmongMarkems / NumOfAllContactors;
                    ShareOfImpWordsInContactors = NumOfConAmongImpWords / NumOfContactors;
                    ShareOfImpWordsInAllContactors = NumOfAllConAmongImpWords / NumOfAllContactors;
                }
                if (NumOfSwitches == 0 && NumOfContactors == 0)
                {
                    ShareOfMarkemsInAll = 0;
                    ShareOfAllMarkemsInAll = 0;
                }
                else
                {
                    ShareOfMarkemsInAll = NumOfMarkemsInAll / (NumOfSwitches + NumOfContactors);
                    ShareOfAllMarkemsInAll = NumOfAllMarkemsInAll / (NumOfAllSwitches + NumOfAllContactors);
                }

                //выводим полученные параметры в DataGridView
                dataGridViewAllWords.Rows[index].Cells[1].Value = NumOfSwitches.ToString() + " (" + NumOfAllSwitches.ToString() + ")";
                dataGridViewAllWords.Rows[index].Cells[2].Value = NumOfContactors.ToString() + " (" + NumOfAllContactors.ToString() + ")";
                dataGridViewAllWords.Rows[index].Cells[3].Value = (NumOfSwitches + NumOfContactors).ToString() + " (" + (NumOfAllSwitches + NumOfAllContactors).ToString() + ")";
                dataGridViewAllWords.Rows[index].Cells[4].Value = String.Format("{0:0.00}", ShareOfMarkemsInSwitches) + " (" + String.Format("{0:0.00}", ShareOfMarkemsInAllSwitches) + ")";
                dataGridViewAllWords.Rows[index].Cells[5].Value = String.Format("{0:0.00}", ShareOfMarkemsInContactors) + " (" + String.Format("{0:0.00}", ShareOfMarkemsInAllContactors) + ")";
                dataGridViewAllWords.Rows[index].Cells[6].Value = String.Format("{0:0.00}", ShareOfMarkemsInAll) + " (" + String.Format("{0:0.00}", ShareOfAllMarkemsInAll) + ")";
                dataGridViewAllWords.Rows[index].Cells[7].Value = String.Format("{0:0.00}", ShareOfImpWordsInContactors) + " (" + String.Format("{0:0.00}", ShareOfImpWordsInAllContactors) + ")";
                if (_myfield.Steps == 0)
                    dataGridViewAllWords.Rows[index].Cells[8].Value = (_myfield.Steps) + " " + (_myfield.Levels);
                else
                    dataGridViewAllWords.Rows[index].Cells[8].Value = (_myfield.Steps) + " " + (_myfield.Levels + 1);
                btnExportToExcel.Enabled = true;
                dataGridViewAllWords.Rows[index].Selected = true;
                dataGridViewAllWords.FirstDisplayedScrollingRowIndex = index;

                //делаем запрос на сохранение полученного ассоциативного поля в таблицу MS Excel
                var confirmResult = MessageBox.Show("Хотите отобразить полученную таблицу в Microsoft Excel?",
                                     "Сохранение ассоциативного поля",
                                     MessageBoxButtons.YesNo);
                //если пользователь отвечает "да", то полученное поле экспортируем в таблицу MS Excel
                if (confirmResult == DialogResult.Yes)
                    _resultWriter.ExportField(_myfield.Field);
            }
        }

        /// <summary>
        /// Строит ассоциативные поля для всех экспериментальных слов.
        /// </summary>
        private void btnFieldsForAllWords_Click(object sender, EventArgs e)
        {
            //проверяем списки с экспериментальными данными на корректность длины
            if (CheckLists())
            {
                //очищаем компоненты данных
                dataGridViewWords.Rows.Clear();
                dataGridViewImportantWords.Rows.Clear();
                dataGridViewMarkems1.Rows.Clear();
                dataGridViewMarkems2.Rows.Clear();
                dataGridViewСorFactor1.Rows.Clear();
                dataGridViewСorFactor2.Rows.Clear();
                foreach (string word in AllWords)
                    dataGridViewWords.Rows.Add(word, 0, 0, 0);
                foreach (string imp_word in ImportantWords)
                    dataGridViewImportantWords.Rows.Add(imp_word, 0);

                //инициализируем параметры
                int[] Markems1 = new int[Markems.Count];
                int[] Markems2 = new int[Markems.Count];
                for (int i = 0; i < Markems.Count; i++)
                {
                    Markems1[i] = 0;
                    Markems2[i] = 0;
                }

                double NumSwitInMarkems = 0;
                double NumAllSwitInMarkems = 0;
                double NumConcInMarkems = 0;
                double NumAllConcInMarkems = 0;
                double NumInMarkems = 0;
                double NumAllInMarkems = 0;

                double NumSwitInNoMarkems = 0;
                double NumAllSwitInNoMarkems = 0;
                double NumConcInNoMarkems = 0;
                double NumAllConcInNoMarkems = 0;
                double NumInNoMarkems = 0;
                double NumAllInNoMarkems = 0;

                double NumMarkemsSwitMarkems = 0;
                double NumAllMarkemsSwitMarkems = 0;
                double NumMarkemsConcMarkems = 0;
                double NumAllMarkemsConcMarkems = 0;
                double NumMarkemsAllMarkems = 0;
                double NumAllMarkemsAllMarkems = 0;

                double NumMarkemsSwitNoMarkems = 0;
                double NumAllMarkemsSwitNoMarkems = 0;
                double NumMarkemsConcNoMarkems = 0;
                double NumAllMarkemsConcNoMarkems = 0;
                double NumMarkemsAllNoMarkems = 0;
                double NumAllMarkemsAllNoMarkems = 0;

                _myfield = new FieldConstruction(AllWords, Definitions, DirectAssociations, FreeAssociations, Similarities, Opposities, ImportantWords);
                //строим ассоциативное поле для каждого слова из экспериментального списка
                for (int k = 0; k < AllWords.Count; k++)
                {
                    _myfield.InitField(AllWords[k]);    //строим ассоциативное поле для выбранного слова
                    //инициализируем параметры ассоциативного поля
                    int NumOfSwitches = 0;
                    int NumOfAllSwitches = 0;
                    int NumOfContactors = _myfield.Contactors.Count;
                    int NumOfAllContactors = _myfield.NumOfContactors;
                    double NumOfSwAmongMarkems = 0;
                    double NumOfConAmongMarkems = 0;
                    double NumOfAllSwAmongMarkems = 0;
                    double NumOfAllConAmongMarkems = 0;
                    double NumOfConAmongImpWords = 0;
                    double NumOfAllConAmongImpWords = 0;

                    /*StringBuilder switches = new StringBuilder();
                    StringBuilder contactors = new StringBuilder();

                    foreach (string cont in _myfield.Contactors)
                        if (_myfield.AllWordsAsContactors[cont] == 1)
                            if (Markems.IndexOf(cont) == -1)
                                contactors.Append(cont).Append("\n");
                            else
                                contactors.Append(cont).Append("!\n");
                        else
                        {
                            string num_of_cont = " (" + _myfield.AllWordsAsContactors[cont].ToString() + ")";
                            if (Markems.IndexOf(cont) == -1)
                                contactors.Append(cont).Append(num_of_cont).Append("\n");
                            else
                                contactors.Append(cont).Append(num_of_cont).Append("!\n");
                        }*/

                    //вычисляем параметры
                    for (int i = 0; i < AllWords.Count; i++)
                    {
                        if (_myfield.AllWordsAsSwitches[AllWords[i]] != 0 || _myfield.AllWordsAsContactors[AllWords[i]] != 0)
                        {
                            dataGridViewWords.Rows[i].Cells[1].Value = (int)dataGridViewWords.Rows[i].Cells[1].Value + _myfield.AllWordsAsSwitches[AllWords[i]];
                            dataGridViewWords.Rows[i].Cells[2].Value = (int)dataGridViewWords.Rows[i].Cells[2].Value + _myfield.AllWordsAsContactors[AllWords[i]];
                            dataGridViewWords.Rows[i].Cells[3].Value = (int)dataGridViewWords.Rows[i].Cells[3].Value + _myfield.AllWordsAsSwitches[AllWords[i]] + _myfield.AllWordsAsContactors[AllWords[i]];

                            int ind = Markems.IndexOf(AllWords[i]);
                            if (ind != -1)
                            {
                                Markems1[ind] += _myfield.AllWordsAsContactors[AllWords[i]];
                                Markems2[ind] += _myfield.AllWordsAsSwitches[AllWords[i]] + _myfield.AllWordsAsContactors[AllWords[i]];
                            }
                            if (_myfield.AllWordsAsSwitches[AllWords[i]] != 0)
                                {
                                /*if (_myfield.AllWordsAsSwitches[AllWords[i]] == 1)
                                    if (Markems.IndexOf(AllWords[i]) == -1)            
                                        switches.Append(AllWords[i]).Append("\n");
                                    else                                              
                                        switches.Append(AllWords[i]).Append("!\n");   
                                else
                                {
                                    string num_of_swit = " (" + _myfield.AllWordsAsSwitches[AllWords[i]].ToString() + ")";
                                    if (Markems.IndexOf(AllWords[i]) == -1)                            
                                        switches.Append(AllWords[i]).Append(num_of_swit).Append("\n");
                                    else                                                                
                                        switches.Append(AllWords[i]).Append(num_of_swit).Append("!\n"); 
                                }*/

                                NumOfSwitches++;
                                NumOfAllSwitches += _myfield.AllWordsAsSwitches[AllWords[i]];
                                if (ind != -1)
                                {
                                    NumOfSwAmongMarkems++;
                                    NumOfAllSwAmongMarkems += _myfield.AllWordsAsSwitches[AllWords[i]];
                                }
                            }
                            if ((_myfield.AllWordsAsContactors[AllWords[i]] != 0) && (ind != -1))
                            {
                                NumOfConAmongMarkems++;
                                NumOfAllConAmongMarkems += _myfield.AllWordsAsContactors[AllWords[i]];
                            }
                        }
                    }
                    for (int i = 0; i < ImportantWords.Count; i++)
                    {
                        dataGridViewImportantWords.Rows[i].Cells[1].Value = (int)dataGridViewImportantWords.Rows[i].Cells[1].Value + _myfield.ImpWordsAsContactors[ImportantWords[i]];
                        if (_myfield.ImpWordsAsContactors[ImportantWords[i]] != 0)
                        {
                            NumOfConAmongImpWords++;
                            NumOfAllConAmongImpWords += _myfield.ImpWordsAsContactors[ImportantWords[i]];
                        }
                    }

                    double NumOfMarkemsInAll = NumOfSwAmongMarkems + NumOfConAmongMarkems;
                    double NumOfAllMarkemsInAll = NumOfAllSwAmongMarkems + NumOfAllConAmongMarkems;

                    double ShareOfMarkemsInSwitches;
                    double ShareOfMarkemsInContactors;
                    double ShareOfMarkemsInAllSwitches;
                    double ShareOfMarkemsInAllContactors;

                    double ShareOfMarkemsInAll;
                    double ShareOfAllMarkemsInAll;

                    double ShareOfImpWordsInContactors;
                    double ShareOfImpWordsInAllContactors;

                    if (NumOfSwitches == 0)
                    {
                        ShareOfMarkemsInSwitches = 0;
                        ShareOfMarkemsInAllSwitches = 0;
                    }
                    else
                    {
                        ShareOfMarkemsInSwitches = NumOfSwAmongMarkems / NumOfSwitches;
                        ShareOfMarkemsInAllSwitches = NumOfAllSwAmongMarkems / NumOfAllSwitches;
                    }
                    if (NumOfContactors == 0)
                    {
                        ShareOfMarkemsInContactors = 0;
                        ShareOfMarkemsInAllContactors = 0;
                        ShareOfImpWordsInContactors = 0;
                        ShareOfImpWordsInAllContactors = 0;
                    }
                    else
                    {
                        ShareOfMarkemsInContactors = NumOfConAmongMarkems / NumOfContactors;
                        ShareOfMarkemsInAllContactors = NumOfAllConAmongMarkems / NumOfAllContactors;
                        ShareOfImpWordsInContactors = NumOfConAmongImpWords / NumOfContactors;
                        ShareOfImpWordsInAllContactors = NumOfAllConAmongImpWords / NumOfAllContactors;
                    }
                    if (NumOfSwitches == 0 && NumOfContactors == 0)
                    {
                        ShareOfMarkemsInAll = 0;
                        ShareOfAllMarkemsInAll = 0;
                    }
                    else
                    {
                        ShareOfMarkemsInAll = NumOfMarkemsInAll / (NumOfSwitches + NumOfContactors);
                        ShareOfAllMarkemsInAll = NumOfAllMarkemsInAll / (NumOfAllSwitches + NumOfAllContactors);
                    }

                    if (Markems.IndexOf(AllWords[k]) != -1)
                    {
                        NumSwitInMarkems += NumOfSwitches;
                        NumAllSwitInMarkems += NumOfAllSwitches;
                        NumConcInMarkems += NumOfContactors;
                        NumAllConcInMarkems += NumOfAllContactors;
                        NumInMarkems += NumOfSwitches + NumOfContactors;
                        NumAllInMarkems += NumOfAllSwitches + NumOfAllContactors;

                        NumMarkemsSwitMarkems += NumOfSwAmongMarkems;
                        NumAllMarkemsSwitMarkems += NumOfAllSwAmongMarkems;
                        NumMarkemsConcMarkems += NumOfConAmongMarkems;
                        NumAllMarkemsConcMarkems += NumOfAllConAmongMarkems;
                        NumMarkemsAllMarkems += NumOfMarkemsInAll;
                        NumAllMarkemsAllMarkems += NumOfAllMarkemsInAll;
                    }
                    else
                    {
                        NumSwitInNoMarkems += NumOfSwitches;
                        NumAllSwitInNoMarkems += NumOfAllSwitches;
                        NumConcInNoMarkems += NumOfContactors;
                        NumAllConcInNoMarkems += NumOfAllContactors;
                        NumInNoMarkems += NumOfSwitches + NumOfContactors;
                        NumAllInNoMarkems += NumOfAllSwitches + NumOfAllContactors;

                        NumMarkemsSwitNoMarkems += NumOfSwAmongMarkems;
                        NumAllMarkemsSwitNoMarkems += NumOfAllSwAmongMarkems;
                        NumMarkemsConcNoMarkems += NumOfConAmongMarkems;
                        NumAllMarkemsConcNoMarkems += NumOfAllConAmongMarkems;
                        NumMarkemsAllNoMarkems += NumOfMarkemsInAll;
                        NumAllMarkemsAllNoMarkems += NumOfAllMarkemsInAll;
                    }

                    //выводим полученные параметры в DataGridView
                    dataGridViewAllWords.Rows[k].Cells[1].Value = NumOfSwitches.ToString() + " (" + NumOfAllSwitches.ToString() + ")\n";
                    dataGridViewAllWords.Rows[k].Cells[2].Value = NumOfContactors.ToString() + " (" + NumOfAllContactors.ToString() + ")\n";
                    dataGridViewAllWords.Rows[k].Cells[3].Value = (NumOfSwitches + NumOfContactors).ToString() + " (" + (NumOfAllSwitches + NumOfAllContactors).ToString() + ")";
                    dataGridViewAllWords.Rows[k].Cells[4].Value = String.Format("{0:0.00}", ShareOfMarkemsInSwitches) + " (" + String.Format("{0:0.00}", ShareOfMarkemsInAllSwitches) + ")";
                    dataGridViewAllWords.Rows[k].Cells[5].Value = String.Format("{0:0.00}", ShareOfMarkemsInContactors) + " (" + String.Format("{0:0.00}", ShareOfMarkemsInAllContactors) + ")";
                    dataGridViewAllWords.Rows[k].Cells[6].Value = String.Format("{0:0.00}", ShareOfMarkemsInAll) + " (" + String.Format("{0:0.00}", ShareOfAllMarkemsInAll) + ")";
                    dataGridViewAllWords.Rows[k].Cells[7].Value = String.Format("{0:0.00}", ShareOfImpWordsInContactors) + " (" + String.Format("{0:0.00}", ShareOfImpWordsInAllContactors) + ")";
                    if (_myfield.Steps == 0)
                        dataGridViewAllWords.Rows[k].Cells[8].Value = (_myfield.Steps) + " " + (_myfield.Levels);
                    else
                        dataGridViewAllWords.Rows[k].Cells[8].Value = (_myfield.Steps) + " " + (_myfield.Levels + 1);
                }

                int[] MarkemsRang = new int[Markems.Count];
                for (int i = 0; i < Markems.Count; i++)
                    MarkemsRang[i] = Markems.Count - i;

                for (int i = 0; i < Markems.Count; i++)
                {
                    dataGridViewMarkems1.Rows.Add(Markems[i], MarkemsRang[i], Markems1[i], 0);
                    dataGridViewMarkems2.Rows.Add(Markems[i], MarkemsRang[i], Markems2[i], 0);
                }

                /*вычисляем коэффициенты корреляции между рангами и частотами встречаемости маркем
                  и выводим полученные коэффициенты в DataGridView*/
                for (int i = 0; i < 5; i++)
                {
                    dataGridViewСorFactor1.Rows.Add(i * 10 + 1 + " - " + (i + 1) * 10, Utility.CalcCorretationFactor(MarkemsRang, Markems1, i, true));
                    dataGridViewСorFactor2.Rows.Add(i * 10 + 1 + " - " + (i + 1) * 10, Utility.CalcCorretationFactor(MarkemsRang, Markems2, i, true));
                }

                tBCorFactor1.Text = Utility.CalcCorretationFactor(MarkemsRang, Markems1, 0, false).ToString();
                tBCorFactor2.Text = Utility.CalcCorretationFactor(MarkemsRang, Markems2, 0, false).ToString();

                tBAvgNumSwitInMarkems.Text = (NumSwitInMarkems / 50).ToString() + " (" + (NumAllSwitInMarkems / 50).ToString() + ")";
                tBAvgNumConcInMarkems.Text = (NumConcInMarkems / 50).ToString() + " (" + (NumAllConcInMarkems / 50).ToString() + ")";
                tBAvgNumSwitInNoMarkems.Text = (NumSwitInNoMarkems / 50).ToString() + " (" + (NumAllSwitInNoMarkems / 50).ToString() + ")";
                tBAvgNumConcInNoMarkems.Text = (NumConcInNoMarkems / 50).ToString() + " (" + (NumAllConcInNoMarkems / 50).ToString() + ")";
                tBAvgNumAllInMarkems.Text = (NumInMarkems / 50).ToString() + " (" + (NumAllInMarkems / 50).ToString() + ")";
                tBAvgNumAllInNoMarkems.Text = (NumInNoMarkems / 50).ToString() + " (" + (NumAllInNoMarkems / 50).ToString() + ")";

                tBAvgNumMarkemsSwitMarkems.Text = (NumMarkemsSwitMarkems / 50).ToString() + " (" + (NumAllMarkemsSwitMarkems / 50).ToString() + ")";
                tBAvgNumMarkemsConcMarkems.Text = (NumMarkemsConcMarkems / 50).ToString() + " (" + (NumAllMarkemsConcMarkems / 50).ToString() + ")";
                tBAvgNumMarkemsAllMarkems.Text = (NumMarkemsAllMarkems / 50).ToString() + " (" + (NumAllMarkemsAllMarkems / 50).ToString() + ")";
                tBAvgNumMarkemsSwitNoMarkems.Text = (NumMarkemsSwitNoMarkems / 50).ToString() + " (" + (NumAllMarkemsSwitNoMarkems / 50).ToString() + ")";
                tBAvgNumMarkemsConcNoMarkems.Text = (NumMarkemsConcNoMarkems / 50).ToString() + " (" + (NumAllMarkemsConcNoMarkems / 50).ToString() + ")";
                tBAvgNumMarkemsAllNoMarkems.Text = (NumMarkemsAllNoMarkems / 50).ToString() + " (" + (NumAllMarkemsAllNoMarkems / 50).ToString() + ")";

                btnExportToExcel.Enabled = true;
                dataGridViewAllWords.FirstDisplayedScrollingRowIndex = 0;
            }
        }

        /// <summary>
        /// Рассчитывает совместную встречаемость экспериментальных слов.
        /// </summary>
        private void btnCalculateCooccurrence_Click(object sender, EventArgs e)
        {
            _words.CountingOfCoOccurrence(_text.Contexts);

            dgvCooccurrence.ColumnCount = _words.KeyWords.Count + 1;
            dgvCooccurrence.RowCount = _words.KeyWords.Count + 2;
            for (int i = 0; i < _words.KeyWords.Count; i++)
            {
                dgvCooccurrence.Rows[0].Cells[i + 1].Value = _words.KeyWords[i];
                dgvCooccurrence.Rows[i + 1].Cells[0].Value = _words.KeyWords[i] + " " + _words.PrintRepetitions(i);
            }
            for (int i = 0; i < _words.KeyWords.Count; i++)
                for (int j = 0; j < _words.KeyWords.Count; j++)
                    if (i >= j)
                        dgvCooccurrence.Rows[i + 1].Cells[j + 1].Value = _words.TableOfCooccurrence[i, j].Print();

            dgvCooccurrence.Rows[_words.KeyWords.Count + 1].Cells[0].Value = "Среднее количество встреч";
            float sum;
            for (int k = 0; k < _words.KeyWords.Count; k++)
            {
                sum = 0;
                for (int j = 0; j < k; j++)
                    sum += _words.TableOfCooccurrence[k, j].CooccurCoeff;
                for (int i = k + 1; i < _words.KeyWords.Count; i++)
                    sum += _words.TableOfCooccurrence[i, k].CooccurCoeff;
                dgvCooccurrence.Rows[_words.KeyWords.Count + 1].Cells[k + 1].Value = Math.Round(sum / _text.Contexts.Count, 2);
            }

            tbxTripleOccurrence.Text = Utility.PrintMultipleOccurrences(_words.TripleOccurrences);
            tbxQuadrupleOccurrence.Text += Utility.PrintMultipleOccurrences(_words.QuadrupleOccurrences);

            btnExportToWord.Enabled = true;
        }

        /// <summary>
        /// Экспортирует таблицы с результатами в MS Excel.
        /// </summary>
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            _resultWriter.ExportTables(dataGridViewAllWords, dataGridViewWords, dataGridViewMarkems1, dataGridViewСorFactor1, tBCorFactor1.Text, dataGridViewMarkems2, dataGridViewСorFactor2, tBCorFactor2.Text, dataGridViewImportantWords, dgvCooccurrence, tBAvgNumSwitInMarkems.Text, tBAvgNumConcInMarkems.Text, tBAvgNumAllInMarkems.Text, tBAvgNumSwitInNoMarkems.Text, tBAvgNumConcInNoMarkems.Text, tBAvgNumAllInNoMarkems.Text, tBAvgNumMarkemsSwitMarkems.Text, tBAvgNumMarkemsConcMarkems.Text, tBAvgNumMarkemsAllMarkems.Text, tBAvgNumMarkemsSwitNoMarkems.Text, tBAvgNumMarkemsConcNoMarkems.Text, tBAvgNumMarkemsAllNoMarkems.Text);
        }

        /// <summary>
        /// Экспортирует текстовые результаты (тройные и четверные встречаемости) в MS Word.
        /// </summary>
        private void btnExportToWord_Click(object sender, EventArgs e)
        {
            _resultWriter.ExportText(tbxTripleOccurrence.Text);
            _resultWriter.ExportText(tbxQuadrupleOccurrence.Text);
        }

        /// <summary>
        /// Очищает компоненты данных.
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridViewAllWords.Rows.Count; i++)
                for (int j = 1; j < dataGridViewAllWords.Columns.Count; j++)
                    dataGridViewAllWords.Rows[i].Cells[j].Value = "";
            dataGridViewWords.Rows.Clear();
            dataGridViewMarkems1.Rows.Clear();
            dataGridViewMarkems2.Rows.Clear();
            dataGridViewСorFactor1.Rows.Clear();
            dataGridViewСorFactor2.Rows.Clear();
            tBCorFactor1.Clear();
            tBCorFactor2.Clear();
            dgvCooccurrence.Rows.Clear();
            tbxTripleOccurrence.Clear();
            tbxQuadrupleOccurrence.Clear();
            btnExportToExcel.Enabled = false;
            btnExportToWord.Enabled = false;
        }
    }
}
