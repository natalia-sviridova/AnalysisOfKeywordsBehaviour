using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisOfKeywordsBehaviour
{
    /// <summary>
    /// Константы.
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
        /// Разделители между словами.
        /// </summary>
        public static readonly char[] SPLITTERS = { ' ', ',', '-' };

        /// <summary>
        /// Кодировка, в которой находятся файлы с экспериментальными данными.
        /// </summary>
        public static readonly Encoding ENCODING = Encoding.UTF8;
    }
}