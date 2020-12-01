﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Utilities
{
    public class TextFile
    {
        public static IEnumerable<int> ReadPositiveIntList(string path)
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
    }
}