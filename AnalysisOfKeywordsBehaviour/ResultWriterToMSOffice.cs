using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalysisOfKeywordsBehaviour
{
    /// <summary>
    /// Предоставляет методы для экспорта полученных результатов в MS Office.
    /// </summary>
    class ResultWriterToMSOffice : IResultWriter
    {
        /// <summary>
        /// Экспортирует итоговые параметры полученных ассоциативных полей в MS Excel.
        /// </summary>
        public void ExportTables(DataGridView source1, DataGridView source2, DataGridView source3, DataGridView source4, string CorFactor1, DataGridView source5, DataGridView source6, string CorFactor2, DataGridView source7, string str1, string str2, string str3, string str4, string str5, string str6, string str7, string str8, string str9, string str10, string str11, string str12)
        {
            Microsoft.Office.Interop.Excel.Application exApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb = exApp.Workbooks.Add(XlSheetType.xlWorksheet);

            Worksheet workSheet1 = (Worksheet)exApp.ActiveSheet;    //создаем первую таблицу с характеристиками ассоциативных полей
            exApp.Columns.ColumnWidth = 30;
            workSheet1.Cells[1, 1] = "Слово";
            workSheet1.Cells[1, 2] = "Кол-во перелючателей";
            workSheet1.Cells[1, 3] = "Кол-во замыкателей";
            workSheet1.Cells[1, 4] = "Кол-во переключателей+замыкателей";
            workSheet1.Cells[1, 5] = "Доля маркем в переключателях";
            workSheet1.Cells[1, 6] = "Доля маркем в замыкателях";
            workSheet1.Cells[1, 7] = "Доля маркем в переключателях+замыкателях";
            workSheet1.Cells[1, 8] = "Шаги : ступени";

            //заполняем таблицу полученными данными
            for (int i = 2; i < source1.Rows.Count + 2; i++)
                for (int j = 1; j < source1.Columns.Count + 1; j++)
                    workSheet1.Cells[i, j] = source1.Rows[i - 2].Cells[j - 1].Value;

            Worksheet workSheet2 = (Worksheet)exApp.Worksheets.Add();   //создаем вторую таблицу с характеристиками ассоциативных полей
            exApp.Columns.ColumnWidth = 20;
            workSheet2.Cells[1, 1] = "Слово";
            workSheet2.Cells[1, 2] = "Как переключатель";
            workSheet2.Cells[1, 3] = "Как замыкатель";
            workSheet2.Cells[1, 4] = "Как переключатель+замыкатель";

            //заполняем таблицу полученными данными
            for (int i = 2; i < source2.Rows.Count + 2; i++)
                for (int j = 1; j <= source2.Columns.Count; j++)
                    workSheet2.Cells[i, j] = source2.Rows[i - 2].Cells[j - 1].Value;

            Worksheet workSheet3 = (Worksheet)exApp.Worksheets.Add();   //создаем третью таблицу с характеристиками ассоциативных полей
            exApp.Columns.ColumnWidth = 30;
            workSheet3.Cells[1, 1] = "Маркема";
            workSheet3.Cells[1, 2] = "Ранг";
            workSheet3.Cells[1, 3] = "Частота";

            workSheet3.Cells[1, 6] = "Группа маркем";
            workSheet3.Cells[1, 7] = "Коэфф. корреляции";

            //заполняем таблицу полученными данными
            for (int i = 2; i < source3.Rows.Count + 2; i++)
                for (int j = 1; j <= source3.Columns.Count; j++)
                    workSheet3.Cells[i, j] = source3.Rows[i - 2].Cells[j - 1].Value;
            for (int i = 2; i < source4.Rows.Count + 2; i++)
            {
                workSheet3.Cells[i, 6] = i - 1;
                workSheet3.Cells[i, 7] = source4.Rows[i - 2].Cells[1].Value;
            }

            workSheet3.Cells[8, 6] = "Общий коэфф. корреляции";
            workSheet3.Cells[8, 7] = CorFactor1;

            Worksheet workSheet4 = (Worksheet)exApp.Worksheets.Add();   //создаем четвертую таблицу с характеристиками ассоциативных полей
            exApp.Columns.ColumnWidth = 30;
            workSheet4.Cells[1, 1] = "Маркема";
            workSheet4.Cells[1, 2] = "Ранг";
            workSheet4.Cells[1, 3] = "Частота";

            workSheet4.Cells[1, 6] = "Группа маркем";
            workSheet4.Cells[1, 7] = "Коэфф. корреляции";

            //заполняем таблицу полученными данными
            for (int i = 2; i < source5.Rows.Count + 2; i++)
                for (int j = 1; j <= source5.Columns.Count; j++)
                    workSheet4.Cells[i, j] = source5.Rows[i - 2].Cells[j - 1].Value;
            for (int i = 2; i < source6.Rows.Count + 2; i++)
            {
                workSheet4.Cells[i, 6] = i - 1;
                workSheet4.Cells[i, 7] = source6.Rows[i - 2].Cells[1].Value;
            }

            workSheet4.Cells[8, 6] = "Общий коэфф. корреляции";
            workSheet4.Cells[8, 7] = CorFactor2;

            Worksheet workSheet5 = (Worksheet)exApp.Worksheets.Add();   //создаем пятую таблицу с характеристиками ассоциативных полей
            exApp.Columns.ColumnWidth = 20;
            workSheet5.Cells[1, 1] = "Cреднее кол-во переключателей для ассоциативных полей маркем:";
            workSheet5.Cells[2, 1] = "Cреднее кол-во замыкателей для ассоциативных полей маркем:";
            workSheet5.Cells[3, 1] = "Cреднее кол-во переключателей+замыкателей для ассоциативных полей маркем:";
            workSheet5.Cells[1, 2] = str1;
            workSheet5.Cells[2, 2] = str2;
            workSheet5.Cells[3, 2] = str3;

            workSheet5.Cells[5, 1] = "Cреднее кол-во переключателей для ассоциативных полей не-маркем:";
            workSheet5.Cells[6, 1] = "Cреднее кол-во замыкателей для ассоциативных полей не-маркем:";
            workSheet5.Cells[7, 1] = "Cреднее кол-во переключателей+замыкателей для ассоциативных полей не-маркем:";
            workSheet5.Cells[5, 2] = str4;
            workSheet5.Cells[6, 2] = str5;
            workSheet5.Cells[7, 2] = str6;

            workSheet5.Cells[9, 1] = "Cреднее кол-во маркем среди переключателей для ассоциативных полей маркем:";
            workSheet5.Cells[10, 1] = "Cреднее кол-во маркем среди замыкателей для ассоциативных полей маркем:";
            workSheet5.Cells[11, 1] = "Cреднее кол-во маркем среди переключателей+замыкателей для ассоциативных полей маркем:";
            workSheet5.Cells[9, 2] = str7;
            workSheet5.Cells[10, 2] = str8;
            workSheet5.Cells[11, 2] = str9;

            workSheet5.Cells[13, 1] = "Cреднее кол-во маркем среди переключателей для ассоциативных полей не-маркем:";
            workSheet5.Cells[14, 1] = "Cреднее кол-во маркем среди замыкателей для ассоциативных полей не-маркем:";
            workSheet5.Cells[15, 1] = "Cреднее кол-во маркем среди переключателей+замыкателей для ассоциативных полей не-маркем:";
            workSheet5.Cells[13, 2] = str10;
            workSheet5.Cells[14, 2] = str11;
            workSheet5.Cells[15, 2] = str12;
   
            Worksheet workSheet6 = (Worksheet)exApp.Worksheets.Add();    //создаем шестую таблицу с совместной встречаемостью
            exApp.Columns.ColumnWidth = 20;

            //заполняем таблицу полученными данными
            for (int i = 1; i < source7.Rows.Count + 1; i++)
                for (int j = 1; j < source7.Columns.Count + 1; j++)
                    workSheet6.Cells[i, j] = source7.Rows[i - 1].Cells[j - 1].Value;

            exApp.Visible = true;   //делаем объект видимым
        }

        /// <summary>
        /// Экспортирует ассоциативное поле в MS Excel.
        /// </summary>
        /// <param name="field">Ассоциативное поле.</param>
        public void ExportField(List<List<string>> field)
        {
            Microsoft.Office.Interop.Excel.Application exApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb = exApp.Workbooks.Add(XlSheetType.xlWorksheet);

            Worksheet workSheet = (Worksheet)exApp.ActiveSheet; //создаем таблицу с ассоциативным полем
            exApp.Columns.ColumnWidth = 20;

            //заполняем ассоциативное поле
            int Steps = field.Count - 1;
            int Levels = field[0].Count;
            for (int i = 0; i < Steps; i++)
                for (int j = 0; j < Levels; j++)
                    workSheet.Cells[j + 1, i + 1] = field[i][j];

            exApp.Visible = true;   //делаем объект видимым
        }

        /// <summary>
        /// Экспортирует текст в MS Word.
        /// </summary>
        /// <param name="text">Текст для экспорта.</param>
        public void ExportText(string text)
        {
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application(); //создаем объект word

            wordApp.Documents.Add();    //добавляем новый документ
            wordApp.ActiveDocument.Select();    //вставляем курсор в начало документа
            wordApp.Selection.FormattedText.Text = text;    //пишем текст, начиная с позиции курсора
            wordApp.Visible = true;     //делаем объект видимым
        }
    }
}
