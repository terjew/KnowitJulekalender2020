using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Utilities;

namespace Luke23
{
    class Luke23
    {
        static Dictionary<string, (int,int)> ReadWords(string path)
        {
            Dictionary<string, (int,int)> words = new Dictionary<string, (int,int)>();
            HashSet<char> vowels = new HashSet<char>() { 'a', 'e', 'i', 'o', 'u', 'y', 'æ', 'ø', 'å' };
            var text = File.ReadAllText(path);

            string word = null;
            int value = 0;
            int numvowels = 0;
            int linestart = 0;
            int i = 0;
            while (i < text.Length)
            {
                var c = text[i];
                while (c != ' ')
                {
                    c = text[++i];
                    if (vowels.Contains(c)) numvowels++;
                }
                word = text.Substring(linestart, i - linestart);

                while (i < text.Length)
                {
                    c = text[++i];
                    if (c == '\n')
                    {
                        words.Add(word, (value, numvowels));
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
            return words;
        }

        static (string,int) DoBattle(string path, Dictionary<string,(int,int)> words)
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

        static int ScoreLine(ReadOnlySpan<char> rapline, Dictionary<string, (int,int)> words)
        {
            int start = 0;
            int pos = 0;
            bool prefixed = false;
            int score = 0;
            int previousVowels = 0;
            string previousWord = null;
            int repetitions = 0;
            if (start < rapline.Length && rapline[start] == 'j')
            {
                prefixed = true;
                pos += 4;
                start += 4;
            }

            while (pos < rapline.Length)
            {
                if (pos == rapline.Length - 1 || rapline[pos + 1] == ' ')//add word score:
                {
                    int wordScore, numVowels, vowelBonus = 0;
                    var word = new string(rapline.Slice(start, pos - start + 1));
                    (wordScore, numVowels) = words[word];

                    if (previousWord == word)
                    {
                        repetitions++;
                    }
                    else
                    {
                        repetitions = 1;
                    }

                    if (prefixed) numVowels += 2;
                    if (previousWord != null && numVowels > previousVowels)
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

                    previousWord = word;
                    previousVowels = numVowels;

                    pos ++;
                    start = pos + 1; //move to start next word
                    prefixed = false;
                    //check if next word starts with jule:
                    if (start < rapline.Length && rapline[start] == 'j')
                    {
                        prefixed = true;
                        pos += 4;
                        start += 4;
                    }
                }
                pos++;
            }
            return score;
        }

        static void Main(string[] args)
        {
            string winner = null;
            int score = 0;
            Dictionary<string, (int, int)> basewords = null;
            Performance.Benchmark("Read and solve", () =>
            {
                basewords = ReadWords("basewords.txt");
                (winner, score) = DoBattle("rap_battle.txt", basewords);
            }, 500, 100, 1000);
            Console.WriteLine($"{winner},{score}");
        }
    }
}
