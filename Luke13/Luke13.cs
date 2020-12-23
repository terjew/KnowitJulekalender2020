using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Utilities;

namespace Luke13
{
    class Luke13
    {
        static void Main(string[] args)
        {
            string remaining = null;
            string text = "phzvjbrsnkeehvglzpveenyjycwzpukigcdiotomuankejhqdhqtojmezmfqtuasuhzbbgawjlxbrqotwgythqsrzfbgisnakeopxtzbbdfbjdnuqymqqihylyszwuezoigjoxhavuyzqnqfnzvtazagvahullujteapqeogyfelzygcqujnxshrivkmhwkkmfiqpqoihcxarewxffyrwmmfghharnijxondraglvemdqfnxdhxilweqcxxsvviuxzshpfjttoymplfahmaskvtimvirhmqoudvqagacqsoeyvpouejmamchbhqfhidpsyovxeazzfbbocuydquadffumpmhwwiotpqiznyvmlnthupvvgfwrpeirltvlorgjqpwzstgjwpsixrbbjsuiumaxydtkcjxvgazonghhfgswunxjhnxvzqxnvtrdujblkbeebsdfgawvholjddwezacxiumyvhlwwbqdpfxzvhyqxcqlnqpvqnvjnygvwrzzaojhnfeywptbttgyyhtkpdbsqcaxpuzsqpadjzssfdiguijlycugnbftcmmpjjjxrjkygethmfvkxbhjkjhrrhgyplasjiunhnqkcvdyzlzxnbdlyxbthmpwrwovuibuypptvgligepclvyxvwkhziqucrnkdelmvdaecdnzeapebfkhocdoaljciemcdasdxqqzjbzhetmovgitntxmvgnfqzrtlaymmxepetgrdsqwmjsodqqrgccnahycqpltphhaeyjnnytjctmkoysduumnurtyzodhsaqdhpyytwrkvymwikkxoolrcgipaftzvwbqounhxriykepdahubsijtwsvtzjihpatpmuemzwthfzypjpiwzhckuxvfrlxlmcvmdujwsghltaukprsancpooxywxccnqgqkgmscstoupxilycjumybfcnjtycichjvkxwfqqqinzrzpthesxlcgcifvjuhyegmjrkb ";
            Performance.Benchmark("text->ienumerable", () =>
            {
                remaining = new string(GetRemainingCharacters(text).ToArray());
            }, 1000, 1000);
            Performance.Benchmark("text->string", () =>
            {
                remaining = GetRemainingCharactersString(text);
            }, 1000, 1000);
            Performance.Benchmark("file->string", () =>
            {
                remaining = GetRemainingCharactersString(File.ReadAllText("text.txt"));
            }, 100, 1000);
            Performance.Benchmark("file->ienumerable", () =>
            {
                remaining = new string(GetRemainingCharacters(File.ReadAllText("text.txt")).ToArray());
            }, 100, 1000);

            Console.WriteLine(remaining);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetRemainingCharactersString(string s)
        {
            var remaining = new char[26];
            int p = 0;
            int[] count = new int[26];
            for (int i = 0; i < s.Length - 1; i++)
            {
                var c = s[i];
                var index = c - 'a';
                count[index]++;
                if (count[index] == index + 1) remaining[p++] = c;
            }
            return new string(remaining);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<char> GetRemainingCharacters(string s)
        {
            int[] count = new int[26];
            for (int i = 0; i < s.Length - 1; i++)
            {
                var c = s[i];
                var index = c - 'a';
                count[index]++;
                if (count[index] == index + 1) yield return c;
            }
        }
    }
}
