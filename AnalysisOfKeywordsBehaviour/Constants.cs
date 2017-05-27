using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisOfKeywordsBehaviour
{
    /// <summary>
    /// Предоставляет константы для работы приложения.
    /// </summary>
    static class Constants
    {
        /// <summary>
        /// Разделитель между контекстами.
        /// </summary>
        public const string DELIMITER = "@";

        /// <summary>
        /// Алфавит, используемый в контекстах.
        /// </summary>
        public const string ALPHABET = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        /// <summary>
        /// Количество маркем в одной группе для расчета коэффициента корреляции.
        /// </summary>
        public const int NUM_OF_MARKEMS = 10;

        /// <summary>
        /// Разделители между словами.
        /// </summary>
        public static readonly char[] SPLITTERS = { ' ', ',', '-' };

        /// <summary>
        /// Кодировка, в которой находятся файлы с экспериментальными данными.
        /// </summary>
        public static readonly Encoding ENCODING = Encoding.UTF8;
    }
}