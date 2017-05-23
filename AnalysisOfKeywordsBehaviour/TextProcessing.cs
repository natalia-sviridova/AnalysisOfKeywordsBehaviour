using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisOfKeywordsBehaviour
{
    /// <summary>
    /// Контекст.
    /// </summary>
    struct Context
    {
        /// <summary>
        /// Все слова контекста.
        /// </summary>
        public List<string> Words;
        /// <summary>
        /// Коэффициент размера контекста.
        /// </summary>
        public float SizeCoeff;

        public Context(List<string> words, float sizeCoeff)
        {
            Words = new List<string>(words);
            SizeCoeff = sizeCoeff;
        }
    }

    /// <summary>
    /// Обработка анализируемого текста.
    /// </summary>
    class TextProcessing
    {
        /// <summary>
        /// Все анализируемые контексты.
        /// </summary>
        public List<Context> Contexts { get; private set; }
        /// <summary>
        /// Среднее количество слов в одном контексте.
        /// </summary>
        private int _avgNumOfWords;

        /// <summary>
        /// Формирует контексты на основе текста, считанного из заданного файла.
        /// </summary>
        /// <param name="path">Путь к файлу, содержащиий текст для анализа.</param>
        public void ProcessText(string path)
        {
            Contexts = new List<Context>();
            List<string> str = new List<string>();
            string word; int index;

            string[] lines = File.ReadAllLines(path, Constants.ENCODING);
            for (int i = 0; i < lines.Length; i++)
                if (lines[i] == Constants.DELIMITER)
                {
                    Contexts.Add(new Context(str, 0));
                    str = new List<string>();
                }
                else
                    if (lines[i] != "")
                {
                    word = "";
                    index = 0;
                    lines[i] = lines[i].ToLower();
                    while (Utility.NextWord(lines[i], ref word, ref index))
                        str.Add(word);
                }

            int sumOfWords = 0;
            for (int i = 0; i < Contexts.Count; i++)
                sumOfWords += Contexts[i].Words.Count;
            _avgNumOfWords = sumOfWords / Contexts.Count;

            for (int i = 0; i < Contexts.Count; i++)
            {
                Context cont = Contexts[i];
                cont.SizeCoeff = (float)_avgNumOfWords / Contexts[i].Words.Count;
                Contexts[i] = cont;
            }
        }
    }
}
