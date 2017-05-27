using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalysisOfKeywordsBehaviour
{
    /// <summary>
    /// Вспомогательная форма для редактирования списков с экспериментальными данными.
    /// </summary>
    public partial class HelpForm : Form
    {
        /// <summary>
        /// Поле, соответствующее экземпляру класса главной формы.
        /// </summary>
        private MainForm _mainForm;
        /// <summary>
        /// Номер списка с экспериментальными данными, который необходимо редактировать.
        /// </summary>
        private int _numOfList;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="mainForm">Главная (родительская) форма.</param>
        /// <param name="numOfList">Номер списка с экспериментальными данными, которые необходимо редактировать.</param>
        public HelpForm(MainForm mainForm, int numOfList)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _numOfList = numOfList;
            tbx.Text = "";
            //выводим соответствующий список с экспериментальными данными
            switch (_numOfList)
            {
                case 0:
                    foreach (string word in _mainForm.AllWords)
                        tbx.Text += word + Environment.NewLine;
                    break;
                case 1:
                    foreach (string word in _mainForm.Markems)
                        tbx.Text += word + Environment.NewLine;
                    break;
                case 2:
                    foreach (string word in _mainForm.Definitions)
                        tbx.Text += word + Environment.NewLine;
                    break;
                case 3:
                    foreach (string word in _mainForm.FreeAssociations)
                        tbx.Text += word + Environment.NewLine;
                    break;
                case 4:
                    foreach (string word in _mainForm.DirectAssociations)
                        tbx.Text += word + Environment.NewLine;
                    break;
                case 5:
                    foreach (string word in _mainForm.Similarities)
                        tbx.Text += word + Environment.NewLine;
                    break;
                case 6:
                    foreach (string word in _mainForm.Opposities)
                        tbx.Text += word + Environment.NewLine;
                    break;
            }
        }

        /// <summary>
        /// Сохраняет измнения и возвращает управление на главную форму.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            switch (_numOfList)
            {
                case 0:
                    _mainForm.AllWords.Clear();
                    _mainForm.dgvAllWords.Rows.Clear();
                    for (int i = 0; i < tbx.Lines.Length; i++)
                    {
                        _mainForm.AllWords.Add(tbx.Lines[i]);
                        _mainForm.dgvAllWords.Rows.Add(tbx.Lines[i]);
                    }
                    break;
                case 1:
                    _mainForm.Markems.Clear();
                    _mainForm.dgvWords.Rows.Clear();
                    for (int i = 0; i < tbx.Lines.Length; i++)
                    {
                        _mainForm.Markems.Add(tbx.Lines[i]);
                        _mainForm.dgvWords.Rows.Add(tbx.Lines[i]);
                    }
                    break;
                case 2:
                    _mainForm.Definitions.Clear();
                    for (int i = 0; i < tbx.Lines.Length; i++)
                        _mainForm.Definitions.Add(tbx.Lines[i]);
                    break;
                case 3:
                    _mainForm.FreeAssociations.Clear();
                    for (int i = 0; i < tbx.Lines.Length; i++)
                        _mainForm.FreeAssociations.Add(tbx.Lines[i]);
                    break;
                case 4:
                    _mainForm.DirectAssociations.Clear();
                    for (int i = 0; i < tbx.Lines.Length; i++)
                        _mainForm.DirectAssociations.Add(tbx.Lines[i]);
                    break;
                case 5:
                    _mainForm.Similarities.Clear();
                    for (int i = 0; i < tbx.Lines.Length; i++)
                        _mainForm.Similarities.Add(tbx.Lines[i]);
                    break;
                case 6:
                    _mainForm.Opposities.Clear();
                    for (int i = 0; i < tbx.Lines.Length; i++)
                        _mainForm.Opposities.Add(tbx.Lines[i]);
                    break;
            }
            Close();
        }

        /// <summary>
        /// Отменяет измнения и возвращает управление на главную форму.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
