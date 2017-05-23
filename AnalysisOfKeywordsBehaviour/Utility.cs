using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisOfKeywordsBehaviour
{
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
    }
}
