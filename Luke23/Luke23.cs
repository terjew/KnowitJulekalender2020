using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Utilities;

namespace Luke23
{
    struct WordInfo
    {
        public int score;
        public int vowels;
        public int length;
    }

    class Luke23
    {
        static WordInfo[] ReadWords(string path, WordInfo[] wordsArr)
        {
            var text = File.ReadAllText(path);
            int value = 0;
            int numvowels = 0;
            int linestart = 0;
            int i = 0;
            while (i < text.Length)
            {
                var c = text[i];
                while (c != ' ')
                {
                    if (c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u' || c == 'y' || c == 'æ' || c == 'ø' || c == 'å') numvowels++;
                    c = text[++i];
                }
                var w0 = text[linestart];
                var w1 = text[linestart + 1];
                var length = i - linestart;
                while (i < text.Length)
                {
                    c = text[++i];
                    if (c == '\n')
                    {
                        var index = w0 << 8 | w1;
                        wordsArr[index] = new WordInfo()
                        {
                            score = value,
                            vowels = numvowels,
                            length = length,
                        };
                        linestart = ++i;
                        value = 0;
                        numvowels = 0;
                        break;
                    }
                    else
                    {
                        value = value * 10 + (c - '0');
                    }
                }
            }

            return wordsArr;
        }

        static (string,int) DoBattle(string path, WordInfo[] words)
        {
            int lilScore = 0;
            int nizScore = 0;

            using (StreamReader sr = File.OpenText(path))
            {
                string line = String.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    var first = line[0];
                    if (first == 'L') //Lil Niz X:
                    {
                        lilScore += ScoreLine(line.AsSpan(11), words);
                    }
                    else if (first == 'N') //Nizzy G:
                    {
                        nizScore += ScoreLine(line.AsSpan(9), words);
                    }
                }
            }
            return lilScore > nizScore ? ("Lil Niz X", lilScore) : ("Nizzy G", nizScore);
        }

        static int ScoreLine(ReadOnlySpan<char> rapline, WordInfo[] words)
        {
            int pos = 0;
            bool prefixed = false;
            int score = 0;
            int previousVowels = 999;
            int previousIndex = -1;
            int repetitions = 1;
            WordInfo info = words[0];
            while (pos < rapline.Length)
            {
                if (rapline[pos] == 'j')
                {
                    prefixed = true;
                    pos += 4;
                }

                int wordScore, numVowels, vowelBonus = 0;
                var w0 = rapline[pos];
                var w1 = rapline[pos + 1];
                int index = w0 << 8 | w1;
                if (index == previousIndex)
                {
                    repetitions++;
                }
                else
                {
                    repetitions = 1;
                    info = words[index];
                }

                wordScore = info.score;
                numVowels = info.vowels;
                if (numVowels > previousVowels)
                {
                    vowelBonus = numVowels - previousVowels;
                    if (prefixed) vowelBonus *= 2;
                    wordScore += vowelBonus;
                }
                if (repetitions > 1)
                {
                    wordScore /= repetitions;
                }

                score += wordScore;
                previousIndex = index;
                previousVowels = numVowels;
                prefixed = false;
                pos += info.length + 1;
            }
            return score;
        }

        static void Main(string[] args)
        {
            string winner = null;
            int score = 0;
            WordInfo[] basewords = new WordInfo[65536];
            Performance.Benchmark("Read and solve", () =>
            {
                ReadWords("basewords.txt", basewords);
                (winner, score) = DoBattle("rap_battle.txt", basewords);
            }, 500, 100, 1000);
            Console.WriteLine($"{winner},{score}");
        }
    }
}
