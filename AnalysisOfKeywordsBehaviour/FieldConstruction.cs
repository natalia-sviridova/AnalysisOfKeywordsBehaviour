using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisOfKeywordsBehaviour
{
    /// <summary>
    /// Предоставляет методы для построения ассоциативного поля.
    /// </summary>
    class FieldConstruction
    {
        /// <summary>
        /// Cлово из экспериментального списка, для которого строится ассоциативное поле.
        /// </summary>
        private string _word;

        /// <summary>
        /// Экспериментальные слова.
        /// </summary>
        public List<string> AllWords;
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
        /// Количество шагов.
        /// </summary>
        public int Steps;
        /// <summary>
        /// Количество ступеней.
        /// </summary>
        public int Levels;
        /// <summary>
        /// Количество замыкателей в поле.
        /// </summary>
        public int NumOfContactors;

        /// <summary>
        /// Экспериментальные слова и числа, показывающие, сколько раз данная лексема работала в качестве переключателя.
        /// </summary>
        public Dictionary<string, int> AllWordsAsSwitches;
        /// <summary>
        /// Экспериментальные слова и числа, показывающие, сколько раз данная лексема работала в качестве замыкателя.
        /// </summary>
        public Dictionary<string, int> AllWordsAsContactors;

        /// <summary>
        /// Все замыкатели ассоциативного поля.
        /// </summary>
        public List<string> Contactors;

        /// <summary>
        /// Все слова, использованные в ассоциативном поле.
        /// </summary>
        List<string> AllUsedWords;
        /// <summary>
        /// Слова, использованные в ассоциативном поле на текущем шаге.
        /// </summary>
        List<string> CurrentAllUsedWords;

        /// <summary>
        /// Ассоциативное поле для заданного экспериментального слова.
        /// </summary>
        public List<List<string>> Field;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="allWords">Список экспериментальных слов.</param>
        /// <param name="definitions">Список дефиниций.</param>
        /// <param name="freeAssociations">Список свободных ассоциаций.</param>
        /// <param name="directAssociations">Список направленных ассоциаций.</param>
        /// <param name="similarities">Список симиляров.</param>
        /// <param name="opposities">Список оппозитов.</param>
        /// <param name="importantWords">Список важных слов.</param>
        public FieldConstruction(List<string> allWords, List<string> definitions, List<string> freeAssociations, List<string> directAssociations, List<string> similarities, List<string> opposities)
        {
            //инициализируем списки с экспериментальными данными
            AllWords = new List<string>(allWords);
            Definitions = new List<string>(definitions);
            FreeAssociations = new List<string>(freeAssociations);
            DirectAssociations = new List<string>(directAssociations);
            Similarities = new List<string>(similarities);
            Opposities = new List<string>(opposities);
        }

        /// <summary>
        /// Создает ассоциативное поле для выбранного экспериментального слова.
        /// </summary>
        /// <param name="word">Экспериментальное слово для построения ассоциативного поля.</param>
        public void InitField(string word)
        {
            //инициализируем необходимые параметры для ассоциативного поля 
            _word = word;
            Steps = 0;
            Levels = 0;
            AllUsedWords = new List<string>();
            AllWordsAsSwitches = new Dictionary<string, int>();
            AllWordsAsContactors = new Dictionary<string, int>();
            Contactors = new List<string>();
            NumOfContactors = 0;
            for (int i = 0; i < AllWords.Count; i++)
            {
                AllWordsAsSwitches.Add(AllWords[i], 0);
                AllWordsAsContactors.Add(AllWords[i], 0);
            }

            CurrentAllUsedWords = new List<string>();
            Field = new List<List<string>>();
            Field.Add(new List<string>());
            Field[Steps].Add(_word);
            AllUsedWords.Add(_word);
            BuildField();   //вызываем метод, строящий ассоциативное поле 
        }

        /// <summary>
        /// Разбивает строку на отдельные лексемы по символам, служащим в качестве разделителей.
        /// </summary>
        /// <param name="exp">Строка для разделения.</param>
        /// <returns>Возвращает массив лексем.</returns>
        private string[] SplitExpression(string exp)
        {
            string[] arr = exp.Split(Constants.SPLITTERS, StringSplitOptions.RemoveEmptyEntries);
            return arr;
        }

        /// <summary>
        /// Добавляет одну ступень в конец ассоциативного поля.
        /// </summary>
        private void AddLevel()
        {
            Levels++;
            for (int i = 0; i < Steps; i++)
                Field[i].Add("");
        }

        /// <summary>
        /// Добавляет одну ступень в произвольное место в ассоциативном поле.
        /// </summary>
        private void LowerLevel()
        {
            Levels++;
            for (int i = 0; i < Steps; i++)
            {
                Field[i].Add("");
                int n = Field[Steps].Count;
                for (int j = Levels; j >= n; j--)
                    Field[i][j] = Field[i][j - 1];
                Field[i][n - 1] = "";
            }
        }

        /// <summary>
        /// Строит ассоциативное поле.
        /// </summary>
        public void BuildField()
        {
            Steps++;
            Field.Add(new List<string>());
            bool Stop = false;
            int l = 0;
            AddStep(AllWords.IndexOf(_word), ref l);
            if (Field[Steps].Count == 0)
            {
                Stop = true;
                Steps--;
            }
            string CurWord; int index;

            //пока на текущем шаге среди слов-стимулов есть переключатели
            while (!Stop)
            {
                Steps++;
                Field.Add(new List<string>());
                AllUsedWords.AddRange(CurrentAllUsedWords);
                CurrentAllUsedWords = new List<string>();
                for (int j = 0; j <= Levels; j++)
                {
                    if (Field[Steps - 1][j] != "")
                    {
                        int len = Field[Steps - 1][j].Length - 1;
                        if (Field[Steps - 1][j][len] == '+' || Field[Steps - 1][j][len] == '-' || Field[Steps - 1][j][len] == ' ')
                            CurWord = Field[Steps - 1][j].Substring(0, len);
                        else
                            CurWord = Field[Steps - 1][j];
                        if (CurWord == _word)
                        {
                            Stop = true;
                            break;
                        }
                    }
                }
                for (int j = 0; j <= Levels; j++)
                {
                    if (Field[Steps - 1][j] != "")
                    {
                        int len = Field[Steps - 1][j].Length - 1;
                        if (Field[Steps - 1][j][len] == '+' || Field[Steps - 1][j][len] == '-')
                            CurWord = Field[Steps - 1][j].Substring(0, len);
                        else
                            CurWord = Field[Steps - 1][j];
                        if (CurWord != "" && CurWord[CurWord.Length - 1] == ' ')
                            Field[Steps].Add("");
                        else
                        {
                            if (!Stop)
                            {
                                index = AllWords.IndexOf(CurWord);
                                if (index != -1)
                                {
                                    if (AllWordsAsSwitches[CurWord] != 0)
                                    {
                                        AllWordsAsSwitches[CurWord]++;
                                        Field[Steps].Add("");
                                    }
                                    else
                                        if (AddStep(index, ref j))
                                        AllWordsAsSwitches[CurWord]++;
                                    else
                                        Field[Steps].Add("");
                                }
                                else
                                {
                                    Field[Steps - 1][j] = CurWord;
                                    Field[Steps].Add("");
                                }
                            }
                            else
                                Field[Steps - 1][j] = CurWord;
                        }
                    }
                    else
                        Field[Steps].Add("");
                }
                if (!Stop)
                {
                    while (Field[Steps].Count < Field[Steps - 1].Count)
                        Field[Steps].Add("");
                    Stop = true; int j = 0;
                    while (Stop && j <= Levels)
                    {
                        if (Field[Steps][j] != "")
                            Stop = false;
                        j++;
                    }
                }
                if (Stop)
                    Steps--;
            }
        }

        /// <summary>
        /// Добавляет один шаг к ассоциативному полю.
        /// </summary>
        /// <param name="index">Индекс слова-стимула, от которого идет добавление, в списке экспериментальных слов.</param>
        /// <param name="lev">Текущая ступень ассоциативного поля.</param>
        /// <returns>Возвращает true, если слово-стимул является переключателем, false – в противном случае.</returns>
        public bool AddStep(int index, ref int lev)
        {
            bool IsSim = false;
            bool IsOp = false;
            int LastInd = Field[Steps - 1][lev].Length - 1;
            //определяем, является ли слово-стимул симиляром
            if (Field[Steps - 1][lev][LastInd] == '+')
            {
                IsSim = true;
                Field[Steps - 1][lev] = Field[Steps - 1][lev].Substring(0, LastInd);
            }
            else
                //определяем, является ли слово-стимул оппозитом
                if (Field[Steps - 1][lev][LastInd] == '-')
            {
                IsOp = true;
                Field[Steps - 1][lev] = Field[Steps - 1][lev].Substring(0, LastInd);
            }
            bool down = false;
            bool added = false;
            //если стлово-стимул имеет дефиницию
            if (Definitions[index] != "-")
            {
                added = false;
                string[] words = SplitExpression(Definitions[index]);
                foreach (string word in words)
                    if (AllUsedWords.IndexOf(word) != -1)
                    {
                        if (!added)
                        {
                            Field[Steps].Add(word);
                            NumOfContactors++;
                            if (Contactors.IndexOf(word) == -1)
                            {
                                Contactors.Add(word);
                                if (AllWords.IndexOf(word) == -1)
                                    AllWordsAsContactors.Add(word, 0);
                            }
                            AllWordsAsContactors[word]++;
                            added = true;
                            down = true;
                        }
                        else
                        {
                            Field[Steps][Field[Steps].Count - 1] += ", " + word;
                            NumOfContactors++;
                            if (Contactors.IndexOf(word) == -1)
                            {
                                Contactors.Add(word);
                                if (AllWords.IndexOf(word) == -1)
                                    AllWordsAsContactors.Add(word, 0);
                            }
                            AllWordsAsContactors[word]++;
                        }
                    }
                    else
                        CurrentAllUsedWords.Add(word);
                if (added)
                    Field[Steps][Field[Steps].Count - 1] += " ";
                if (!added)
                    foreach (string word in words)
                        if (!added)
                        {
                            if (AllWords.IndexOf(word) != -1)
                            {
                                added = true;
                                down = true;
                                Field[Steps].Add(word);
                            }
                        }
                        else
                        {
                            if (AllWords.IndexOf(word) != -1)
                            {
                                Field[Steps].Add(word);
                                if (Field[Steps - 1].Count < Field[Steps].Count)
                                {
                                    lev++;
                                    AddLevel();
                                }
                                else
                                {
                                    lev++;
                                    LowerLevel();
                                }
                            }
                        }
                if (!added)
                {
                    Field[Steps].Add(Definitions[index]);
                    down = true;
                }
            }
            //если стлово-стимул имеет свободную ассоциацию
            if (FreeAssociations[index] != "-")
            {
                added = false;
                string[] words = SplitExpression(FreeAssociations[index]);
                foreach (string word in words)
                    if (AllUsedWords.IndexOf(word) != -1)
                    {
                        if (!added)
                        {
                            if (!down)
                            {
                                Field[Steps].Add(word);
                                down = true;
                            }
                            else
                            {
                                Field[Steps].Add(word);
                                if (Field[Steps - 1].Count < Field[Steps].Count)
                                {
                                    lev++;
                                    AddLevel();
                                }
                                else
                                {
                                    lev++;
                                    LowerLevel();
                                }
                            }
                            NumOfContactors++;
                            if (Contactors.IndexOf(word) == -1)
                            {
                                Contactors.Add(word);
                                if (AllWords.IndexOf(word) == -1)
                                    AllWordsAsContactors.Add(word, 0);
                            }
                            AllWordsAsContactors[word]++;
                            added = true;
                        }
                        else
                        {
                            Field[Steps][Field[Steps].Count - 1] += ", " + word;
                            NumOfContactors++;
                            if (Contactors.IndexOf(word) == -1)
                            {
                                Contactors.Add(word);
                                if (AllWords.IndexOf(word) == -1)
                                    AllWordsAsContactors.Add(word, 0);
                            }
                            AllWordsAsContactors[word]++;
                        }
                    }
                    else
                        CurrentAllUsedWords.Add(word);
                if (added)
                    Field[Steps][Field[Steps].Count - 1] += " ";
                if (!added)
                    foreach (string word in words)
                        if (!down)
                        {
                            if (AllWords.IndexOf(word) != -1)
                            {
                                added = true;
                                down = true;
                                Field[Steps].Add(word);
                            }
                        }
                        else
                        {
                            if (AllWords.IndexOf(word) != -1)
                            {
                                Field[Steps].Add(word);
                                added = true;
                                if (Field[Steps - 1].Count < Field[Steps].Count)
                                {
                                    lev++;
                                    AddLevel();
                                }
                                else
                                {
                                    lev++;
                                    LowerLevel();
                                }
                            }
                        }
                if (!added)
                    if (!down)
                    {
                        Field[Steps].Add(FreeAssociations[index]);
                        down = true;
                    }
                    else
                    {
                        Field[Steps].Add(FreeAssociations[index]);
                        if (Field[Steps - 1].Count < Field[Steps].Count)
                        {
                            lev++;
                            AddLevel();
                        }
                        else
                        {
                            lev++;
                            LowerLevel();
                        }
                    }
            }
            //если стлово-стимул имеет направленную ассоциацию
            if (DirectAssociations[index] != "-")
            {
                added = false;
                string[] words = SplitExpression(DirectAssociations[index]);
                foreach (string word in words)
                    if (AllUsedWords.IndexOf(word) != -1)
                    {
                        if (!added)
                        {
                            if (!down)
                            {
                                Field[Steps].Add(word);
                                down = true;
                            }
                            else
                            {
                                Field[Steps].Add(word);
                                if (Field[Steps - 1].Count < Field[Steps].Count)
                                {
                                    lev++;
                                    AddLevel();
                                }
                                else
                                {
                                    lev++;
                                    LowerLevel();
                                }
                            }
                            NumOfContactors++;
                            if (Contactors.IndexOf(word) == -1)
                            {
                                Contactors.Add(word);
                                if (AllWords.IndexOf(word) == -1)
                                    AllWordsAsContactors.Add(word, 0);
                            }
                            AllWordsAsContactors[word]++;
                            added = true;
                        }
                        else
                        {
                            Field[Steps][Field[Steps].Count - 1] += ", " + word;
                            NumOfContactors++;
                            if (Contactors.IndexOf(word) == -1)
                            {
                                Contactors.Add(word);
                                if (AllWords.IndexOf(word) == -1)
                                    AllWordsAsContactors.Add(word, 0);
                            }
                            AllWordsAsContactors[word]++;
                        }
                    }
                    else
                        CurrentAllUsedWords.Add(word);
                if (added)
                    Field[Steps][Field[Steps].Count - 1] += " ";
                if (!added)
                    foreach (string word in words)
                        if (!down)
                        {
                            if (AllWords.IndexOf(word) != -1)
                            {
                                added = true;
                                down = true;
                                Field[Steps].Add(word);
                            }
                        }
                        else
                        {
                            if (AllWords.IndexOf(word) != -1)
                            {
                                Field[Steps].Add(word);
                                added = true;
                                if (Field[Steps - 1].Count < Field[Steps].Count)
                                {
                                    lev++;
                                    AddLevel();
                                }
                                else
                                {
                                    lev++;
                                    LowerLevel();
                                }
                            }
                        }
                if (!added)
                    if (!down)
                    {
                        Field[Steps].Add(DirectAssociations[index]);
                        down = true;
                    }
                    else
                    {
                        Field[Steps].Add(DirectAssociations[index]);
                        if (Field[Steps - 1].Count < Field[Steps].Count)
                        {
                            lev++;
                            AddLevel();
                        }
                        else
                        {
                            lev++;
                            LowerLevel();
                        }
                    }
            }
            //если стлово-стимул имеет симиляр и оно само не являлось симиляром
            if (!IsSim && Similarities[index] != "-")
            {
                if (AllUsedWords.IndexOf(Similarities[index]) != -1)
                {
                    Field[Steps].Add(Similarities[index] + " ");
                    NumOfContactors++;
                    if (Contactors.IndexOf(Similarities[index]) == -1)
                    {
                        Contactors.Add(Similarities[index]);
                        if (AllWords.IndexOf(Similarities[index]) == -1)
                            AllWordsAsContactors.Add(Similarities[index], 0);
                    }
                    AllWordsAsContactors[Similarities[index]]++;
                }
                else
                {
                    Field[Steps].Add(Similarities[index] + "+");
                    CurrentAllUsedWords.Add(Similarities[index]);
                }
                if (Field[Steps - 1].Count < Field[Steps].Count)
                {
                    lev++;
                    AddLevel();
                }
                else
                    if (down)
                {
                    lev++;
                    LowerLevel();
                }
                down = true;
            }
            //если стлово-стимул имеет оппозит и оно само не является оппозитом
            if (!IsOp && Opposities[index] != "-")
            {
                if (AllUsedWords.IndexOf(Opposities[index]) != -1)
                {
                    Field[Steps].Add(Opposities[index] + " ");
                    NumOfContactors++;
                    if (Contactors.IndexOf(Opposities[index]) == -1)
                    {
                        Contactors.Add(Opposities[index]);
                        if (AllWords.IndexOf(Opposities[index]) == -1)
                            AllWordsAsContactors.Add(Opposities[index], 0);
                    }
                    AllWordsAsContactors[Opposities[index]]++;
                }
                else
                {
                    Field[Steps].Add(Opposities[index] + "-");
                    CurrentAllUsedWords.Add(Opposities[index]);
                }
                if (Field[Steps - 1].Count < Field[Steps].Count)
                {
                    lev++;
                    AddLevel();
                }
                else
                    if (down)
                {
                    lev++;
                    LowerLevel();
                }
                down = true;
            }
            return down;
        }
    }
}
