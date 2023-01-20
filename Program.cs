using System.Runtime.CompilerServices;
using System.Text;

namespace AnagramsTest
{
    internal class Program
    {
        const string DICTIONARY_PATH = "words_alpha.txt";
        const string ALPHA_CHARS = "abcdefghijklmnopqrstuvwxyz";

        static void Main(string[] args)
        {
            Node root = new Node('\0');


            DateTime start = DateTime.Now;

            IEnumerable<string> lines = File.ReadLines(DICTIONARY_PATH);
            foreach (var line in lines)
            {
                root.AddSuffix(line.Trim());
            }

            Console.WriteLine(DateTime.Now - start);

            //string[] checks = { "Dog", "thi", "Mountains", "Swashbuckle", "Swashbuckleckleckle", "BBQ", "moun" };

            //foreach (var word in checks)
            //{
            //    Console.WriteLine($"{word}? {root.CheckSuffix(word)}");
            //}

            while (true)
            {
                Console.WriteLine("Type any number of letters to use, or press Enter to use 9 random letters. Type STOP to stop.");
                string? line = Console.ReadLine();
                if (line == null) break;
                if (line.ToUpper().Equals("STOP")) break;

                List<char> chars = new List<char>();

                if (line.Length == 0)
                {
                    AddRandomLetters(chars, 9);
                }
                else
                {
                    foreach (var c in line)
                    {
                        if (!char.IsWhiteSpace(c)) chars.Add(c);
                    }
                }


                int bestScore;
                List<string> bestWords;
                Search(chars, root, out bestWords, out bestScore);

                StringBuilder sb = new StringBuilder();
                foreach (var c in chars)
                {
                    sb.Append(' ');
                    sb.Append(c);
                }

                Console.WriteLine($"The used letters are:{sb.ToString()}");
                Console.WriteLine($"The greatest length of an anagram is {bestScore}.");
                Console.WriteLine("The following all have the same score:");
                foreach (var word in bestWords)
                {
                    Console.WriteLine($"- {word}");
                }
                Console.WriteLine();
            }
        }

        static void AddRandomLetters(List<char> list, int count)
        {
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                int index = r.Next(ALPHA_CHARS.Length);
                list.Add(ALPHA_CHARS[index]);
            }
        }

        static void Search(List<char> chars, Node dictionaryPosition, out List<string> bestWords, out int score)
        {
            score = 0;
            bestWords = new List<string>();

            for (int i = 0; i < chars.Count; i++)
            {
                char selectedChar = chars[i];

                Node subDictionaryPosition = dictionaryPosition;
                bool childExists = dictionaryPosition.TryGetChild(ref subDictionaryPosition, selectedChar);
                if (!childExists) continue;

                List<char> subChars = new List<char>();
                for (int j = 0; j < i; j++)
                {
                    subChars.Add(chars[j]);
                }
                for (int j = i + 1; j < chars.Count; j++)
                {
                    subChars.Add(chars[j]);
                }

                int subScore;
                List<string> subBestWords;
                Search(subChars, subDictionaryPosition, out subBestWords, out subScore);

                int newScore = subScore + GetValue(selectedChar);

                if (newScore > score)
                {
                    score = newScore;
                    bestWords.Clear();
                    foreach (var suffix in subBestWords)
                    {
                        bestWords.Add(selectedChar + suffix);
                    }
                }
                else if (newScore == score)
                {
                    foreach (var suffix in subBestWords)
                    {
                        bestWords.Add(selectedChar + suffix);
                    }
                }
            }

            bool fail = (score == 0) && (dictionaryPosition.IsLeaf == false);
            if (fail) score = int.MinValue;
            if (score == 0 && !fail) bestWords.Add("");
        }

        static int GetValue(char c)
        {
            return 1;
        }
    }
}