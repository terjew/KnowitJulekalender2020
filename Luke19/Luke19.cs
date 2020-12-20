using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace Luke19
{
    public class ListNode<T>
    {
        public ListNode<T> next;
        public ListNode<T> prev;
        public T data;
    }

    public class CircularList<T>
    {
        public ListNode<T> Head;
        public int Count;

        public CircularList(ListNode<T> head, int count)
        {
            Head = head;
            Count = count;
        }

        public void MoveRight(int steps)
        {
            steps = steps % Count;
            while (steps --> 0)
            {
                Head = Head.prev;
            }
        }

        public void DeleteNode(ListNode<T> node)
        {
            var prev = node.prev;
            var next = node.next;
            prev.next = next;
            next.prev = prev;
            if (node == Head)
            {
                Head = next;
            }
            Count--;
        }

        public void DeleteFirst()
        {
            DeleteNode(Head);
        }

        public void DeleteLast()
        {
            DeleteNode(Head.prev);
        }

        public void DeleteAt(int pos)
        {
            var node = Head;
            while (pos-- > 0)
            {
                node = node.next;
            }
            DeleteNode(node);
        }

        public IEnumerable<T> Enumerate()
        {
            yield return Head.data;
            var current = Head.next;
            while (current != Head)
            {
                yield return current.data;
                current = current.next;
            }
        }

        public void Print()
        {
            Console.WriteLine($"[{string.Join(',', Enumerate())}]");
        }
    }

    class Luke19
    {
        public static CircularList<string> ReadElves(string s, int pos)
        {
            ListNode<string> head = null;
            ListNode<string> current = null;
            int count = 0;
            foreach (var elf in s.Substring(pos + 1, s.Length - 2 - pos).Split(", "))
            {
                var node = new ListNode<string>() { data = elf, prev = current };
                if (current != null)
                {
                    current.next = node;
                }
                else
                {
                    head = node;
                }
                current = node;
                count++;
            }
            current.next = head;
            head.prev = current;
            return new CircularList<string>(head, count);
        }

        static void Test()
        {
            Dictionary<string, string> testCases = new Dictionary<string, string>()
            {
                {"1 3 [Jenny, Alvin, Greger, Petra, Olaug, Olaf]", "Olaf" },
                {"2 3 [Jenny, Alvin, Greger, Petra, Olaug, Olaf]", "Jenny" },
                {"3 3 [Jenny, Alvin, Greger, Petra, Olaug]", "Petra" },
                {"4 3 [Jenny, Alvin, Greger, Petra, Olaug, Olaf]", "Alvin" },
            };
            foreach (var testCase in testCases)
            {
                var winner = RunGame(testCase.Key);
                if (winner != testCase.Value) Console.WriteLine($"Error simulating game: {testCase.Key}");
            }
        }


        static void Main(string[] args)
        {
            string mostWins = null;
            Performance.TimeRun("Parse and solve", () =>
            {
                Dictionary<string, int> wins = new Dictionary<string, int>();
                using (StreamReader sr = File.OpenText("input.txt"))
                {
                    string s = String.Empty;
                    while ((s = sr.ReadLine()) != null)
                    {
                        var winner = RunGame(s);
                        var prev = wins.GetValueOrDefault(winner);
                        wins[winner] = prev + 1;
                    }
                }
                var spencerWins = wins["Spencer"];
                mostWins = wins.OrderByDescending(kvp => kvp.Value).First().Key;
            });
            Console.WriteLine(mostWins);
        }

        static string RunGame(string line)
        {
            char rule = line[0];
            int steps = 0;
            int pos = 2;
            while (line[pos] != ' ') steps = steps * 10 + line[pos++] - '0';
            var list = ReadElves(line, pos + 1);
            switch (rule)
            {
                case '1': return Rule1(list, steps);
                case '2': return Rule2(list, steps);
                case '3': return Rule3(list, steps);
                case '4': return Rule4(list, steps);
            }
            throw new ArgumentException();
        }

        static string Rule1(CircularList<string> elves, int rightMoves)
        {
            //Alltid fjern den første stolen i listen.
            while (elves.Count > 1)
            {
                elves.MoveRight(rightMoves);
                elves.DeleteFirst();
            }
            return elves.Head.data;
        }

        static string Rule2(CircularList<string> elves, int rightMoves)
        {
            //Begynn med å fjerne stolen på plass 0, deretter på plass 1, og oppover, 
            //frem til man når antall stoler(som fortsatt er i spill), deretter begynner man på første stol igjen.
            int toRemove = 0;
            while (elves.Count > 1)
            {
                elves.MoveRight(rightMoves);
                elves.DeleteAt(toRemove);
                toRemove = (++toRemove % elves.Count);
            }
            return elves.Head.data;
        }

        static string Rule3(CircularList<string> elves, int rightMoves)
        {
            //Fjern den midterste stolen. Dersom antall stoler er partall, fjernes de to stolene 
            //som er i midten, frem til det er 2 stoler igjen, da fjernes den første stolen.
            while (elves.Count > 2)
            {
                elves.MoveRight(rightMoves);
                var toRemove = elves.Count / 2;
                elves.DeleteAt(toRemove);
                if ((elves.Count % 2) == 1)
                {
                    elves.DeleteAt(toRemove);
                }
            }
            elves.MoveRight(rightMoves);
            return elves.Head.next.data;
        }

        static string Rule4(CircularList<string> elves, int rightMoves)
        {
            //Alltid fjern den siste stolen i listen.
            while (elves.Count > 1)
            {
                elves.MoveRight(rightMoves);
                elves.DeleteLast();
            }
            return elves.Head.data;
        }

    }
}
