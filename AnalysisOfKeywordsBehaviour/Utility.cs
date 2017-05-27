using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisOfKeywordsBehaviour
{
    /// <summary>
    /// Предоставляет ряд вспомогательных методов для работы приложения.
    /// </summary>
    static class Utility
    {
        /// <summary>
        /// Генерирует все сочетания из n по k.
        /// </summary>
        /// <param name="n">Количество элементов в исходном множестве.</param>
        /// <param name="k">Количество элементов в сочетании.</param>
        /// <returns>Возвращает список из сгенерированных сочетаний.</returns>
        public static List<int[]> GenerateCombinations(int n, int k)
        {
            if (n < k)
                return null;

            List<int[]> res = new List<int[]>();
            int[] comb = new int[k];
            int[] combForAdding;

            for (int i = 0; i < k; i++)
                comb[i] = i;

            if (n == k)
            {
                res.Add(comb);
                return res;
            }

            int p = k - 1;
            while (p >= 0)
            {
                combForAdding = new int[k];
                for (int i = 0; i < k; i++)
                    combForAdding[i] = comb[i];
                res.Add(combForAdding);

                if (comb[k - 1] == n - 1)
                    p--;
                else
                    p = k - 1;
                if (p >= 0)
                    for (int i = k - 1; i >= p; i--)
                        comb[i] = comb[p] + i - p + 1;
            }
            return res;
        }

        /// <summary>
        /// Ищет следующее слово в заданной строке.
        /// </summary>
        /// <param name="str">Строка для поиска.</param>
        /// <param name="word">Найденное слово.</param>
        /// <param name="index">Индекс, с которого необходимо начать поиск в строке.</param>
        /// <returns>Возвращает true, если слово найдено, false - в противном случае.</returns>
        public static bool NextWord(string str, ref string word, ref int index)
        {
            //пропускаем все символы, которые не входят в алфавит
            while (index < str.Length)
            {
                if (Constants.ALPHABET.IndexOf(str[index]) == -1)
                    index++;
                else
                    break;
            }
            //если дошли до конца строки, то возращаем false (слово не найдено)
            if (index >= str.Length)
                return false;

            word = "";
            //иначе формируем следующее слово word и возвращаем true
            while (index < str.Length)
            {
                if (Constants.ALPHABET.IndexOf(str[index]) != -1)
                {
                    word += str[index];
                    index++;
                }
                else
                {
                    index++;
                    break;
                }
            }
            return true;
        }

        /*параметр IsPart отвечает за то, вычисляется ли коэфф. корреляции для группы маркем,
         состоящей из N маркем, или для всех маркем*/
        /// <summary>
        /// Вычисляет коэффициент корреляции двух рядов.
        /// </summary>
        /// <param name="matr1">Первый ряд.</param>
        /// <param name="matr2">Второй ряд.</param>
        /// <param name="k">Номер группы маркем.</param>
        /// <param name="IsPart">Если true, то будет вычисляеться коэфф. корреляции для группы из N маркем, если false - то для всех маркем.</param>
        /// <returns>Возвращает коэффициент корреляции двух рядов с точностью до сотых.</returns>
        public static double CalcCorretationFactor(int[] matr1, int[] matr2, int k, bool IsPart)
        {
            double SampleAvg1 = 0, SampleAvg2 = 0, Sum1 = 0, Sum2 = 0, Sum3 = 0;
            if (IsPart)
            {
                //вычисляем коэффициент корреляции для группы маркем
                for (int i = 0; i < Constants.NUM_OF_MARKEMS; i++)
                {
                    SampleAvg1 += matr1[k * Constants.NUM_OF_MARKEMS + i];
                    SampleAvg2 += matr2[k * Constants.NUM_OF_MARKEMS + i];
                }
                SampleAvg1 = SampleAvg1 / Constants.NUM_OF_MARKEMS;
                SampleAvg2 = SampleAvg2 / Constants.NUM_OF_MARKEMS;

                for (int i = 0; i < Constants.NUM_OF_MARKEMS; i++)
                {
                    Sum1 += (matr1[k * Constants.NUM_OF_MARKEMS + i] - SampleAvg1) * (matr2[k * Constants.NUM_OF_MARKEMS + i] - SampleAvg2);
                    Sum2 += Math.Pow((matr1[k * Constants.NUM_OF_MARKEMS + i] - SampleAvg1), 2);
                    Sum3 += Math.Pow((matr2[k * Constants.NUM_OF_MARKEMS + i] - SampleAvg2), 2);
                }
            }
            else
            {
                //вычисляем коэффициент корреляции для всех маркем
                int n = matr1.Length;
                for (int i = 0; i < n; i++)
                {
                    SampleAvg1 += matr1[i];
                    SampleAvg2 += matr2[i];
                }
                SampleAvg1 = SampleAvg1 / n;
                SampleAvg2 = SampleAvg2 / n;

                for (int i = 0; i < n; i++)
                {
                    Sum1 += (matr1[i] - SampleAvg1) * (matr2[i] - SampleAvg2);
                    Sum2 += Math.Pow((matr1[i] - SampleAvg1), 2);
                    Sum3 += Math.Pow((matr2[i] - SampleAvg2), 2);
                }
            }
            return Math.Round(Sum1 / Math.Sqrt(Sum2 * Sum3), 2);
        }

        /// <summary>
        /// Объединяет все элементы списка из строк в единую строку.
        /// </summary>
        /// <param name="list">Список, элементы которого необходимо объединить в строку.</param>
        /// <returns>Возвращает строку из элементов списка, разделенных "-".</returns>
        public static string MakeString(List<string> list)
        {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < list.Count - 1; i++)
                output.Append(list[i] + "-");
            output.Append(list[list.Count - 1]);
            return output.ToString();
        }

        /// <summary>
        /// Выводит тройные/четверные встречаемости.
        /// </summary>
        /// <param name="multOccurrence">Словарь с тройными/четверными встречаемостями.</param>
        /// <returns>Возвращает строку, состоящую из всех тройных/четверных встречаемостей.</returns>
        public static string PrintMultipleOccurrences(Dictionary<string, float> multOccurrence)
        {
            StringBuilder output = new StringBuilder();
            var items = from pair in multOccurrence
                        orderby pair.Value descending
                        select pair;
            foreach (KeyValuePair<string, float> cont in items)
                output.Append(cont.Key).Append(" - ").Append(Math.Round(cont.Value, 2).ToString()).Append(Environment.NewLine);
            return output.ToString();
        }

        /// <summary>
        /// Разбивает строку на отдельные лексемы по символам, служащим в качестве разделителей.
        /// </summary>
        /// <param name="exp">Строка для разделения.</param>
        /// <returns>Возвращает массив лексем.</returns>
        public static string[] SplitExpression(string exp)
        {
            string[] arr = exp.Split(Constants.SPLITTERS, StringSplitOptions.RemoveEmptyEntries);
            return arr;
        }
    }
}
