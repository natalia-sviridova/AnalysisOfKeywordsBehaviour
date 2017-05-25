using LingvoNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisOfKeywordsBehaviour
{
    /// <summary>
    /// Совместная встречаемость ключевых слов.
    /// </summary>
    struct Cooccurrence
    {
        /// <summary>
        /// Коэффициент совместной встречаемости.
        /// </summary>
        public float CooccurCoeff { get; set; }

        /// <summary>
        /// Контексты, в которых появились данные слова, и коэффициенты размеров таких контекстов.
        /// </summary>
        public Dictionary<int, float> Contexts { get; set; }

        /// <summary>
        /// Конструктор структуры.
        /// </summary>
        /// <param name="cooccurCoeff">Коэффициент совместной встречаемости.</param>
        public Cooccurrence(float cooccurCoeff)
        {
            CooccurCoeff = cooccurCoeff;
            Contexts = new Dictionary<int, float>();
        }

        /// <summary>
        /// Добавляет новый контекст и его коэффициент размера.
        /// </summary>
        /// <param name="context">Номер контекста.</param>
        /// <param name="sizeOfCont">Коэффициент размера контекста.</param>
        public void Add(int context, float sizeOfCont)
        {
            Contexts.Add(context, sizeOfCont);
        }

        /// <summary>
        /// Отображает коэффициент совместной встречаемости и список контекстов в виде одной строки.
        /// </summary>
        /// <returns>Возвращает строку с коэффициентом совместной встречаемости и списком контекстов.</returns>
        public string Print()
        {
            if (CooccurCoeff == 0)
                return "0";
            else
            {
                StringBuilder output = new StringBuilder();
                output.Append(Math.Round(CooccurCoeff, 2).ToString()).Append(" (");
                var items = from pair in Contexts
                            orderby pair.Value descending
                            select pair;
                foreach (KeyValuePair<int, float> cont in items)
                    output.Append((cont.Key + 1).ToString() + ", ");
                output.Remove(output.Length - 2, 2);
                output.Append(")");
                return output.ToString();
            }
        }
    }

    /// <summary>
    /// Анализ совместной встречаемости ключевых слов.
    /// </summary>
    class WordsProcessing
    {
        /// <summary>
        /// Ключевые слова.
        /// </summary>
        public List<string> KeyWords { get; private set; }

        /// <summary>
        /// Ключевые слова и их формы.
        /// </summary>
        private List<List<string>> _words;

        /// <summary>
        /// Количество повторений каждого ключевого слова.
        /// </summary>
        public SortedDictionary<int, int>[] Repetitions { get; private set; }

        /// <summary>
        /// Таблица совместной встречаемости ключевых слов.
        /// </summary>
        public Cooccurrence[,] TableOfCooccurrence { get; private set; }

        /// <summary>
        /// Тройные встречаемости.
        /// </summary>
        public Dictionary<string, float> TripleOccurrences { get; private set; }

        /// <summary>
        /// Четверные встречаемости.
        /// </summary>
        public Dictionary<string, float> QuadrupleOccurrences { get; private set; }

        /// <summary>
        /// Преобразует все формы имени существительного к одной строке.
        /// </summary>
        /// <param name="noun">Имя существительное.</param>
        /// <returns>Возвращает строку, содержащую все формы данного имени существительного.</returns>
        private string BuildNounForms(Noun noun)
        {
            StringBuilder nounForms = new StringBuilder();
            nounForms.AppendFormat(@"{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}",
                noun[Case.Nominative], noun[Case.Nominative, Number.Plural],
                noun[Case.Genitive], noun[Case.Genitive, Number.Plural],
                noun[Case.Dative], noun[Case.Dative, Number.Plural],
                noun[Case.Accusative], noun[Case.Accusative, Number.Plural],
                noun[Case.Instrumental], noun[Case.Instrumental, Number.Plural],
                noun[Case.Locative], noun[Case.Locative, Number.Plural]);
            return nounForms.ToString();
        }

        /// <summary>
        /// Инициализирует экспериментальные слова и их формы.
        /// </summary>
        /// <param name="words">Экспериментальные слова.</param>
        public void ProcessWords(List<string> words)
        {
            KeyWords = new List<string>(words);
            _words = new List<List<string>>();

            for (int i = 0; i < words.Count; i++)
            {
                string wordsForms = BuildNounForms(Nouns.FindSimilar(words[i]));
                _words.Add(wordsForms.Split(Constants.SPLITTERS, StringSplitOptions.RemoveEmptyEntries).ToList());
            }
            Repetitions = new SortedDictionary<int, int>[KeyWords.Count];
            TableOfCooccurrence = new Cooccurrence[KeyWords.Count, KeyWords.Count];
            TripleOccurrences = new Dictionary<string, float>();
            QuadrupleOccurrences = new Dictionary<string, float>();
        }

        /// <summary>
        /// Выводит список повторений для заданного экспериментального слова.
        /// </summary>
        /// <param name="index">Индекс слова в списке экспериментальных слов.</param>
        /// <returns>Возвращает строку, содержащую количество повторений данного экспериментального слова.</returns>
        public string PrintRepetitions(int index)
        {
            if (Repetitions[index].Count == 0)
                return "";
            else
            {
                StringBuilder output = new StringBuilder();
                output.Append("(");
                foreach (KeyValuePair<int, int> rep in Repetitions[index])
                    output.Append(rep.Key + ": " + rep.Value + ", ");
                output.Remove(output.Length - 2, 2);
                output.Append(")");
                return output.ToString();
            }
        }

        /// <summary>
        /// Рассчитывает совместную встречаемость экспериментальных слов.
        /// </summary>
        /// <param name="contexts">Контексты, для которых рассчитывается совместная встречаемость.</param>
        public void CountingOfCoOccurrence(List<Context> contexts)
        {
            string curWord;
            bool found;
            int m, n;
            List<int> foundWords;
            List<int> distinctFoundWords;
            List<string> words;
            List<int[]> doubleCombinations;
            List<int[]> tripleCombinations;
            List<int[]> quadrupleCombinations;
            Dictionary<int, int> repetitions;

            for (int i = 0; i < KeyWords.Count; i++)
            {
                Repetitions[i] = new SortedDictionary<int, int>();
                for (int j = 0; j < KeyWords.Count; j++)
                    TableOfCooccurrence[i, j] = new Cooccurrence(0);
            }

            for (int i = 0; i < contexts.Count; i++)
            {
                foundWords = new List<int>();
                for (int j = 0; j < contexts[i].Words.Count; j++)
                {
                    curWord = contexts[i].Words[j];
                    found = false;
                    m = 0;
                    while (!found && m < _words.Count)
                    {
                        n = 0;
                        while (!found && n < _words[m].Count)
                            if (curWord == _words[m][n])
                                found = true;
                            else
                                n++;
                        m++;
                    }
                    if (found)
                        foundWords.Add(m - 1);
                }

                repetitions = foundWords
                              .Select(numOfWord => new { Num = numOfWord, Count = foundWords.Count(num => num == numOfWord) })
                              .Where(obj => obj.Count > 1)
                              .Distinct()
                              .ToDictionary(obj => obj.Num, obj => obj.Count);

                foreach (KeyValuePair<int, int> rep in repetitions)
                    if (Repetitions[rep.Key].ContainsKey(rep.Value))
                        Repetitions[rep.Key][rep.Value]++;
                    else
                        Repetitions[rep.Key].Add(rep.Value, 1);

                distinctFoundWords = foundWords.Distinct().ToList();

                if (distinctFoundWords.Count > 1)
                {
                    doubleCombinations = Utility.GenerateCombinations(distinctFoundWords.Count, 2);
                    foreach (int[] comb in doubleCombinations)
                    {
                        m = Math.Max(distinctFoundWords[comb[0]], distinctFoundWords[comb[1]]);
                        n = Math.Min(distinctFoundWords[comb[0]], distinctFoundWords[comb[1]]);
                        TableOfCooccurrence[m, n].CooccurCoeff += contexts[i].SizeCoeff;
                        TableOfCooccurrence[m, n].Contexts.Add(i, contexts[i].SizeCoeff);
                    }
                }
                if (distinctFoundWords.Count > 2)
                {
                    tripleCombinations = Utility.GenerateCombinations(distinctFoundWords.Count, 3);
                    foreach (int[] comb in tripleCombinations)
                    {
                        words = new List<string>();
                        for (int k = 0; k < comb.Length; k++)
                            words.Add(KeyWords[distinctFoundWords[comb[k]]]);
                        words = words.OrderBy(q => q).ToList();
                        string str = Utility.MakeString(words);
                        if (TripleOccurrences.ContainsKey(str))
                            TripleOccurrences[str] += contexts[i].SizeCoeff;
                        else
                            TripleOccurrences.Add(str, contexts[i].SizeCoeff);
                    }
                }
                if (distinctFoundWords.Count > 3)
                {
                    quadrupleCombinations = Utility.GenerateCombinations(distinctFoundWords.Count, 4);
                    foreach (int[] comb in quadrupleCombinations)
                    {
                        words = new List<string>();
                        for (int k = 0; k < comb.Length; k++)
                            words.Add(KeyWords[distinctFoundWords[comb[k]]]);
                        words = words.OrderBy(q => q).ToList();
                        string str = Utility.MakeString(words);
                        if (QuadrupleOccurrences.ContainsKey(str))
                            QuadrupleOccurrences[str] += contexts[i].SizeCoeff;
                        else
                            QuadrupleOccurrences.Add(str, contexts[i].SizeCoeff);
                    }
                }
            }
        }
    }
}
