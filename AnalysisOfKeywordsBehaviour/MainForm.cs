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
    /// Главная форма приложения.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Экземпляр класса по построению ассоциативного поля экспериментального слова.
        /// </summary>
        private FieldConstruction _field;
        /// <summary>
        /// Экземпляр класса по обработке контекстов.
        /// </summary>
        private TextProcessing _textProc;
        /// <summary>
        /// Экземпляр класса по расчету совместной встречаемости экспериментальных слов.
        /// </summary>
        private WordsProcessing _wordsProc;
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
        /// Происходит, когда приложение заканчивает обработку и собирается перейти в состояние бездействия.
        /// </summary>
        void Idle(object sender, EventArgs e)
        {
            btnBuildFieldForSelectedWord.Enabled = (cmbAllWords.SelectedItem != null) && (AllWords.Count != 0) && (Markems.Count != 0) && (Definitions.Count != 0) && (FreeAssociations.Count != 0) && (DirectAssociations.Count != 0) && (Similarities.Count != 0) && (Opposities.Count != 0);
            btnBuildFieldsForAllWords.Enabled = (AllWords.Count != 0) && (Markems.Count != 0) && (Definitions.Count != 0) && (FreeAssociations.Count != 0) && (DirectAssociations.Count != 0) && (Similarities.Count != 0) && (Opposities.Count != 0);
            btnCalculateCooccurrence.Enabled = (_textProc != null) && (_wordsProc != null);
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
        }

        /// <summary>
        /// Cчитывает данные из текстового файла в соответствующий список с экспериментальными данными.
        /// </summary>
        /// <param name="path">Путь к текстовому файлу, из которого необходимо считать экспериментальные данные.</param>
        /// <param name="list">Список строк, куда необходимо занести экспериментальные данные.</param>
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

                _wordsProc = new WordsProcessing();
                _wordsProc.ProcessWords(AllWords);

                cmbAllWords.Items.Clear();
                dgvAllWords.Rows.Clear();
                dgvWords.Rows.Clear();
                foreach (string word in AllWords)
                {
                    dgvAllWords.Rows.Add(word);
                    cmbAllWords.Items.Add(word);
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

        private void текстДляАнализаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFD.FileName;
                _textProc = new TextProcessing();
                _textProc.ProcessText(fileName);
            }
        }
        //---------------------------------------------------------------------------------------------------//

        /*набор методов, предназначенный для вызова вспомогательной формы для изменения соответствующего списка экспериментальных данных*/
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
            Cursor = Cursors.WaitCursor;
            //проверяем списки с экспериментальными данными на корректность длины
            if (CheckLists())
            {
                dgvWords.Rows.Clear();
                int index = AllWords.IndexOf(cmbAllWords.SelectedItem.ToString());
                _field = new FieldConstruction(AllWords, Definitions, FreeAssociations, DirectAssociations, Similarities, Opposities);
                _field.InitField(cmbAllWords.SelectedItem.ToString());    //строим ассоциативное поле для выбранного слова

                //инициализируем параметры ассоциативного поля
                int NumOfSwitches = 0;
                int NumOfAllSwitches = 0;
                int NumOfContactors = _field.Contactors.Count;
                int NumOfAllContactors = _field.NumOfContactors;
                double NumOfSwAmongMarkems = 0;
                double NumOfConAmongMarkems = 0;
                double NumOfAllSwAmongMarkems = 0;
                double NumOfAllConAmongMarkems = 0;
                //вычисляем параметры
                for (int i = 0; i < AllWords.Count; i++)
                {
                    if (_field.AllWordsAsSwitches[AllWords[i]] != 0 || _field.AllWordsAsContactors[AllWords[i]] != 0)
                    {
                        dgvWords.Rows.Add(AllWords[i], _field.AllWordsAsSwitches[AllWords[i]], _field.AllWordsAsContactors[AllWords[i]], _field.AllWordsAsSwitches[AllWords[i]] + _field.AllWordsAsContactors[AllWords[i]]);
                        if (_field.AllWordsAsSwitches[AllWords[i]] != 0)
                        {
                            NumOfSwitches++;
                            NumOfAllSwitches += _field.AllWordsAsSwitches[AllWords[i]];
                            if (Markems.IndexOf(AllWords[i]) != -1)
                            {
                                NumOfSwAmongMarkems++;
                                NumOfAllSwAmongMarkems += _field.AllWordsAsSwitches[AllWords[i]];
                            }
                        }
                        if (_field.AllWordsAsContactors[AllWords[i]] != 0 && Markems.IndexOf(AllWords[i]) != -1)
                        {
                            NumOfConAmongMarkems++;
                            NumOfAllConAmongMarkems += _field.AllWordsAsContactors[AllWords[i]];
                        }
                    }
                }

                double NumOfMarkemsInAll = NumOfSwAmongMarkems + NumOfConAmongMarkems;
                double NumOfAllMarkemsInAll = NumOfAllSwAmongMarkems + NumOfAllConAmongMarkems;

                double ShareOfMarkemsInSwitches, ShareOfMarkemsInContactors;
                double ShareOfMarkemsInAllSwitches, ShareOfMarkemsInAllContactors;

                double ShareOfMarkemsInAll, ShareOfAllMarkemsInAll;

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
                }
                else
                {
                    ShareOfMarkemsInContactors = NumOfConAmongMarkems / NumOfContactors;
                    ShareOfMarkemsInAllContactors = NumOfAllConAmongMarkems / NumOfAllContactors;
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
                dgvAllWords.Rows[index].Cells[1].Value = NumOfSwitches.ToString() + " (" + NumOfAllSwitches.ToString() + ")";
                dgvAllWords.Rows[index].Cells[2].Value = NumOfContactors.ToString() + " (" + NumOfAllContactors.ToString() + ")";
                dgvAllWords.Rows[index].Cells[3].Value = (NumOfSwitches + NumOfContactors).ToString() + " (" + (NumOfAllSwitches + NumOfAllContactors).ToString() + ")";
                dgvAllWords.Rows[index].Cells[4].Value = String.Format("{0:0.00}", ShareOfMarkemsInSwitches) + " (" + String.Format("{0:0.00}", ShareOfMarkemsInAllSwitches) + ")";
                dgvAllWords.Rows[index].Cells[5].Value = String.Format("{0:0.00}", ShareOfMarkemsInContactors) + " (" + String.Format("{0:0.00}", ShareOfMarkemsInAllContactors) + ")";
                dgvAllWords.Rows[index].Cells[6].Value = String.Format("{0:0.00}", ShareOfMarkemsInAll) + " (" + String.Format("{0:0.00}", ShareOfAllMarkemsInAll) + ")";
                if (_field.Steps == 0)
                    dgvAllWords.Rows[index].Cells[7].Value = (_field.Steps) + " " + (_field.Levels);
                else
                    dgvAllWords.Rows[index].Cells[7].Value = (_field.Steps) + " " + (_field.Levels + 1);
                btnExportToExcel.Enabled = true;
                dgvAllWords.Rows[index].Selected = true;
                dgvAllWords.FirstDisplayedScrollingRowIndex = index;

                //делаем запрос на сохранение полученного ассоциативного поля в таблицу MS Excel
                var confirmResult = MessageBox.Show("Хотите отобразить полученную таблицу в Microsoft Excel?",
                                     "Сохранение ассоциативного поля",
                                     MessageBoxButtons.YesNo);
                //если пользователь отвечает "да", то полученное поле экспортируем в таблицу MS Excel
                if (confirmResult == DialogResult.Yes)
                    _resultWriter.ExportField(_field.Field);
            }
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Строит ассоциативные поля для всех экспериментальных слов.
        /// </summary>
        private void btnFieldsForAllWords_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            //проверяем списки с экспериментальными данными на корректность длины
            if (CheckLists())
            {
                //очищаем компоненты данных
                dgvWords.Rows.Clear();
                dgvMarkems1.Rows.Clear();
                dgvMarkems2.Rows.Clear();
                dgvСorFactor1.Rows.Clear();
                dgvСorFactor2.Rows.Clear();
                foreach (string word in AllWords)
                    dgvWords.Rows.Add(word, 0, 0, 0);

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

                _field = new FieldConstruction(AllWords, Definitions, DirectAssociations, FreeAssociations, Similarities, Opposities);
                //строим ассоциативное поле для каждого слова из экспериментального списка
                for (int k = 0; k < AllWords.Count; k++)
                {
                    _field.InitField(AllWords[k]);    //строим ассоциативное поле для выбранного слова
                    //инициализируем параметры ассоциативного поля
                    int NumOfSwitches = 0;
                    int NumOfAllSwitches = 0;
                    int NumOfContactors = _field.Contactors.Count;
                    int NumOfAllContactors = _field.NumOfContactors;
                    double NumOfSwAmongMarkems = 0;
                    double NumOfConAmongMarkems = 0;
                    double NumOfAllSwAmongMarkems = 0;
                    double NumOfAllConAmongMarkems = 0;

                    //вычисляем параметры
                    for (int i = 0; i < AllWords.Count; i++)
                    {
                        if (_field.AllWordsAsSwitches[AllWords[i]] != 0 || _field.AllWordsAsContactors[AllWords[i]] != 0)
                        {
                            dgvWords.Rows[i].Cells[1].Value = (int)dgvWords.Rows[i].Cells[1].Value + _field.AllWordsAsSwitches[AllWords[i]];
                            dgvWords.Rows[i].Cells[2].Value = (int)dgvWords.Rows[i].Cells[2].Value + _field.AllWordsAsContactors[AllWords[i]];
                            dgvWords.Rows[i].Cells[3].Value = (int)dgvWords.Rows[i].Cells[3].Value + _field.AllWordsAsSwitches[AllWords[i]] + _field.AllWordsAsContactors[AllWords[i]];

                            int ind = Markems.IndexOf(AllWords[i]);
                            if (ind != -1)
                            {
                                Markems1[ind] += _field.AllWordsAsContactors[AllWords[i]];
                                Markems2[ind] += _field.AllWordsAsSwitches[AllWords[i]] + _field.AllWordsAsContactors[AllWords[i]];
                            }
                            if (_field.AllWordsAsSwitches[AllWords[i]] != 0)
                            {
                                NumOfSwitches++;
                                NumOfAllSwitches += _field.AllWordsAsSwitches[AllWords[i]];
                                if (ind != -1)
                                {
                                    NumOfSwAmongMarkems++;
                                    NumOfAllSwAmongMarkems += _field.AllWordsAsSwitches[AllWords[i]];
                                }
                            }
                            if ((_field.AllWordsAsContactors[AllWords[i]] != 0) && (ind != -1))
                            {
                                NumOfConAmongMarkems++;
                                NumOfAllConAmongMarkems += _field.AllWordsAsContactors[AllWords[i]];
                            }
                        }
                    }

                    double NumOfMarkemsInAll = NumOfSwAmongMarkems + NumOfConAmongMarkems;
                    double NumOfAllMarkemsInAll = NumOfAllSwAmongMarkems + NumOfAllConAmongMarkems;

                    double ShareOfMarkemsInSwitches, ShareOfMarkemsInContactors;
                    double ShareOfMarkemsInAllSwitches, ShareOfMarkemsInAllContactors;

                    double ShareOfMarkemsInAll, ShareOfAllMarkemsInAll;

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
                    }
                    else
                    {
                        ShareOfMarkemsInContactors = NumOfConAmongMarkems / NumOfContactors;
                        ShareOfMarkemsInAllContactors = NumOfAllConAmongMarkems / NumOfAllContactors;
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
                    dgvAllWords.Rows[k].Cells[1].Value = NumOfSwitches.ToString() + " (" + NumOfAllSwitches.ToString() + ")\n";
                    dgvAllWords.Rows[k].Cells[2].Value = NumOfContactors.ToString() + " (" + NumOfAllContactors.ToString() + ")\n";
                    dgvAllWords.Rows[k].Cells[3].Value = (NumOfSwitches + NumOfContactors).ToString() + " (" + (NumOfAllSwitches + NumOfAllContactors).ToString() + ")";
                    dgvAllWords.Rows[k].Cells[4].Value = String.Format("{0:0.00}", ShareOfMarkemsInSwitches) + " (" + String.Format("{0:0.00}", ShareOfMarkemsInAllSwitches) + ")";
                    dgvAllWords.Rows[k].Cells[5].Value = String.Format("{0:0.00}", ShareOfMarkemsInContactors) + " (" + String.Format("{0:0.00}", ShareOfMarkemsInAllContactors) + ")";
                    dgvAllWords.Rows[k].Cells[6].Value = String.Format("{0:0.00}", ShareOfMarkemsInAll) + " (" + String.Format("{0:0.00}", ShareOfAllMarkemsInAll) + ")";
                    if (_field.Steps == 0)
                        dgvAllWords.Rows[k].Cells[7].Value = (_field.Steps) + " " + (_field.Levels);
                    else
                        dgvAllWords.Rows[k].Cells[7].Value = (_field.Steps) + " " + (_field.Levels + 1);
                }

                int[] MarkemsRang = new int[Markems.Count];
                for (int i = 0; i < Markems.Count; i++)
                    MarkemsRang[i] = Markems.Count - i;

                for (int i = 0; i < Markems.Count; i++)
                {
                    dgvMarkems1.Rows.Add(Markems[i], MarkemsRang[i], Markems1[i], 0);
                    dgvMarkems2.Rows.Add(Markems[i], MarkemsRang[i], Markems2[i], 0);
                }

                /*вычисляем коэффициенты корреляции между рангами и частотами встречаемости маркем
                  и выводим полученные коэффициенты в DataGridView*/
                int numOfGroups = Markems.Count / Constants.NUM_OF_MARKEMS;
                for (int i = 0; i < numOfGroups; i++)
                {
                    dgvСorFactor1.Rows.Add(i * 10 + 1 + " - " + (i + 1) * 10, Utility.CalcCorretationFactor(MarkemsRang, Markems1, i, true));
                    dgvСorFactor2.Rows.Add(i * 10 + 1 + " - " + (i + 1) * 10, Utility.CalcCorretationFactor(MarkemsRang, Markems2, i, true));
                }

                tbxCorFactor1.Text = Utility.CalcCorretationFactor(MarkemsRang, Markems1, 0, false).ToString();
                tbxCorFactor2.Text = Utility.CalcCorretationFactor(MarkemsRang, Markems2, 0, false).ToString();

                tbxAvgNumSwitInMarkems.Text = (NumSwitInMarkems / 50).ToString() + " (" + (NumAllSwitInMarkems / 50).ToString() + ")";
                tbxAvgNumConcInMarkems.Text = (NumConcInMarkems / 50).ToString() + " (" + (NumAllConcInMarkems / 50).ToString() + ")";
                tbxAvgNumSwitInNoMarkems.Text = (NumSwitInNoMarkems / 50).ToString() + " (" + (NumAllSwitInNoMarkems / 50).ToString() + ")";
                tbxAvgNumConcInNoMarkems.Text = (NumConcInNoMarkems / 50).ToString() + " (" + (NumAllConcInNoMarkems / 50).ToString() + ")";
                tbxAvgNumAllInMarkems.Text = (NumInMarkems / 50).ToString() + " (" + (NumAllInMarkems / 50).ToString() + ")";
                tbxAvgNumAllInNoMarkems.Text = (NumInNoMarkems / 50).ToString() + " (" + (NumAllInNoMarkems / 50).ToString() + ")";

                tbxAvgNumMarkemsSwitMarkems.Text = (NumMarkemsSwitMarkems / 50).ToString() + " (" + (NumAllMarkemsSwitMarkems / 50).ToString() + ")";
                tbxAvgNumMarkemsConcMarkems.Text = (NumMarkemsConcMarkems / 50).ToString() + " (" + (NumAllMarkemsConcMarkems / 50).ToString() + ")";
                tbxAvgNumMarkemsAllMarkems.Text = (NumMarkemsAllMarkems / 50).ToString() + " (" + (NumAllMarkemsAllMarkems / 50).ToString() + ")";
                tbxAvgNumMarkemsSwitNoMarkems.Text = (NumMarkemsSwitNoMarkems / 50).ToString() + " (" + (NumAllMarkemsSwitNoMarkems / 50).ToString() + ")";
                tbxAvgNumMarkemsConcNoMarkems.Text = (NumMarkemsConcNoMarkems / 50).ToString() + " (" + (NumAllMarkemsConcNoMarkems / 50).ToString() + ")";
                tbxAvgNumMarkemsAllNoMarkems.Text = (NumMarkemsAllNoMarkems / 50).ToString() + " (" + (NumAllMarkemsAllNoMarkems / 50).ToString() + ")";

                btnExportToExcel.Enabled = true;
                dgvAllWords.FirstDisplayedScrollingRowIndex = 0;
            }
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Рассчитывает совместную встречаемость экспериментальных слов.
        /// </summary>
        private void btnCalculateCooccurrence_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _wordsProc.CountingOfCoOccurrence(_textProc.Contexts);

            dgvCooccurrence.ColumnCount = _wordsProc.KeyWords.Count + 1;
            dgvCooccurrence.RowCount = _wordsProc.KeyWords.Count + 2;
            dgvOccurrenceByCases.ColumnCount = 7;
            dgvOccurrenceByCases.RowCount = _wordsProc.KeyWords.Count;

            for (int i = 0; i < _wordsProc.KeyWords.Count; i++)
            {
                dgvCooccurrence.Rows[0].Cells[i + 1].Value = _wordsProc.KeyWords[i];
                dgvCooccurrence.Rows[i + 1].Cells[0].Value = _wordsProc.KeyWords[i] + " " + _wordsProc.PrintRepetitions(i);
                dgvOccurrenceByCases.Rows[i].Cells[0].Value = _wordsProc.KeyWords[i];
            }

            for (int i = 0; i < _wordsProc.KeyWords.Count; i++)
                for (int j = 0; j < _wordsProc.KeyWords.Count; j++)
                    if (i >= j)
                        dgvCooccurrence.Rows[i + 1].Cells[j + 1].Value = _wordsProc.TableOfCooccurrence[i, j].Print();

            dgvCooccurrence.Rows[_wordsProc.KeyWords.Count + 1].Cells[0].Value = "Среднее количество встреч:";
            float sum;
            for (int k = 0; k < _wordsProc.KeyWords.Count; k++)
            {
                sum = 0;
                for (int j = 0; j < k; j++)
                    sum += _wordsProc.TableOfCooccurrence[k, j].CooccurCoeff;
                for (int i = k + 1; i < _wordsProc.KeyWords.Count; i++)
                    sum += _wordsProc.TableOfCooccurrence[i, k].CooccurCoeff;
                dgvCooccurrence.Rows[_wordsProc.KeyWords.Count + 1].Cells[k + 1].Value = Math.Round(sum / _textProc.Contexts.Count, 2);
            }

            tbxTripleOccurrence.Text = Utility.PrintMultipleOccurrences(_wordsProc.TripleOccurrences);
            tbxQuadrupleOccurrence.Text = Utility.PrintMultipleOccurrences(_wordsProc.QuadrupleOccurrences);

            for (int i = 0; i < _wordsProc.KeyWords.Count; i++)
                for (int j = 0; j < 6; j++)
                dgvOccurrenceByCases.Rows[i].Cells[j + 1].Value = _wordsProc.OccurrenceByCases[i, j];

            btnExportToWord.Enabled = true;
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Экспортирует таблицы с результатами в MS Excel.
        /// </summary>
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _resultWriter.ExportTables(dgvAllWords, dgvWords, dgvMarkems1, dgvСorFactor1, tbxCorFactor1.Text, dgvMarkems2, dgvСorFactor2, tbxCorFactor2.Text, dgvCooccurrence, dgvOccurrenceByCases, tbxAvgNumSwitInMarkems.Text, tbxAvgNumConcInMarkems.Text, tbxAvgNumAllInMarkems.Text, tbxAvgNumSwitInNoMarkems.Text, tbxAvgNumConcInNoMarkems.Text, tbxAvgNumAllInNoMarkems.Text, tbxAvgNumMarkemsSwitMarkems.Text, tbxAvgNumMarkemsConcMarkems.Text, tbxAvgNumMarkemsAllMarkems.Text, tbxAvgNumMarkemsSwitNoMarkems.Text, tbxAvgNumMarkemsConcNoMarkems.Text, tbxAvgNumMarkemsAllNoMarkems.Text);
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Экспортирует текстовые результаты (тройные и четверные встречаемости) в MS Word.
        /// </summary>
        private void btnExportToWord_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _resultWriter.ExportText(tbxTripleOccurrence.Text);
            _resultWriter.ExportText(tbxQuadrupleOccurrence.Text);
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Очищает компоненты данных.
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvAllWords.Rows.Count; i++)
                for (int j = 1; j < dgvAllWords.Columns.Count; j++)
                    dgvAllWords.Rows[i].Cells[j].Value = "";
            dgvWords.Rows.Clear();
            dgvMarkems1.Rows.Clear();
            dgvMarkems2.Rows.Clear();
            dgvСorFactor1.Rows.Clear();
            dgvСorFactor2.Rows.Clear();
            tbxCorFactor1.Clear();
            tbxCorFactor2.Clear();
            tbxAvgNumAllInMarkems.Clear();
            tbxAvgNumAllInNoMarkems.Clear();
            tbxAvgNumConcInMarkems.Clear();
            tbxAvgNumConcInNoMarkems.Clear();
            tbxAvgNumMarkemsAllMarkems.Clear();
            tbxAvgNumMarkemsAllNoMarkems.Clear();
            tbxAvgNumMarkemsConcMarkems.Clear();
            tbxAvgNumMarkemsConcNoMarkems.Clear();
            tbxAvgNumMarkemsSwitMarkems.Clear();
            tbxAvgNumMarkemsSwitNoMarkems.Clear();
            tbxAvgNumSwitInMarkems.Clear();
            tbxAvgNumSwitInNoMarkems.Clear();
            dgvCooccurrence.Rows.Clear();
            tbxTripleOccurrence.Clear();
            tbxQuadrupleOccurrence.Clear();
            btnExportToExcel.Enabled = false;
            btnExportToWord.Enabled = false;
        }
    }
}
