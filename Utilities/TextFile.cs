using System;
using System.Collections.Generic;
using System.IO;

namespace Utilities
{
    public class TextFile
    {
        public static IEnumerable<int> EnumeratePositiveInts(string path)
        {
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    int y = 0;
                    const int n = 64000;
                    byte[] bytes = new byte[n];
                    int available = 0;
                    while ((available = bs.Read(bytes, 0, n)) != 0)
                    {
                        for (int i = 0; i < available; i++)
                        {
                            byte b = bytes[i];
                            if (b == ',')
                            {
                                yield return y;
                                y = 0;
                            }
                            else
                            {
                                y = y * 10 + (b - '0');
                            }
                        }
                    }
                    yield return y;
                }
            }
        }

        public static IEnumerable<char> EnumerateAsciiCharacters(string path)
        {
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    const int n = 64000;
                    byte[] bytes = new byte[n];
                    int available = 0;
                    while ((available = bs.Read(bytes, 0, n)) != 0)
                    {
                        for (int i = 0; i < available; i++)
                        {
                            yield return (char)bytes[i];
                        }
                    }
                }
            }
        }

        public static List<int> ReadPositiveIntsList(string path)
        {
            List<int> list = new List<int>();
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    int y = 0;
                    const int n = 64000;
                    byte[] bytes = new byte[n];
                    int available = 0;
                    while ((available = bs.Read(bytes, 0, n)) != 0)
                    {
                        for (int i = 0; i < available; i++)
                        {
                            byte b = bytes[i];
                            if (b == ',')
                            {
                                list.Add(y);
                                y = 0;
                            }
                            else
                            {
                                y = y * 10 + (b - '0');
                            }
                        }
                    }
                    list.Add(y);
                }
            }
            return list;
        }

        public static List<string> ReadStringList(string path)
        {
            List<string> list = new List<string>();
            using (StreamReader sr = File.OpenText(path))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    list.Add(s);
                }
            }
            return list;
        }

        public static HashSet<string> ReadStringSet(string path)
        {
            HashSet<string> set = new HashSet<string>();
            using (StreamReader sr = File.OpenText(path))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    set.Add(s);
                }
            }
            return set;
        }

        public static IEnumerable<string> EnumerateLines(string path)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    yield return s;
                }
            }
        }
    }
}
