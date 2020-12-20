using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Luke20
{
    class HierarchyNode
    {
        public HierarchyNode Leader;
        public readonly List<HierarchyNode> Subordinates = new List<HierarchyNode>();
        public readonly string Name;
        public bool Verified;
        public HierarchyNode(string name, HierarchyNode leader)
        {
            Name = name;
            Leader = leader;
            if (leader != null) leader.Subordinates.Add(this);
        }
    }

    class Luke20
    {

        static void Main(string[] args)
        {
            int result = 0;
            Performance.TimeRun("Read and solve", () =>
            {
                result = Solve();
            }, 1, 10, 2);
            Console.WriteLine(result);
        }

        static int Solve()
        {
            Dictionary<string, HierarchyNode> nodes = new Dictionary<string, HierarchyNode>();
            HierarchyNode julenissen = new HierarchyNode("Julenissen", null);
            var lines = TextFile.ReadStringList("elves.txt");
            foreach (var line in lines)
            {
                var names = line.Split("🎄");
                HierarchyNode prev = julenissen;
                for (int i = 0; i < names.Length; i++)
                {
                    var name = names[i];
                    HierarchyNode node = null;
                    if (!nodes.ContainsKey(name))
                    {
                        node = new HierarchyNode(name, prev);
                        nodes.Add(name, node);
                    }
                    else
                    {
                        node = nodes[name];
                    }
                    if (i == names.Length - 1)
                    {
                        node.Verified = true;
                    }
                    prev = node;
                }
            }

            int arbeidere = nodes.Count(n => n.Value.Subordinates.Count == 0);

            var gamle = nodes.Values.Where(n => !n.Verified).ToHashSet();
            foreach (var node in gamle)
            {
                nodes.Remove(node.Name);
                DeleteNode(node);
            }

            var overflødige = nodes.Values.Where(n => !gamle.Contains(n) && n.Subordinates.Count == 1 && n.Subordinates[0].Subordinates.Count > 0).Count();
            //Etter reorganiseringen, hvor mange flere arbeideralver er det enn mellomledere?
            int mellomledere = nodes.Count - arbeidere - overflødige;
            return arbeidere - mellomledere;
        }

        static void DeleteNode(HierarchyNode node)
        {
            node.Leader.Subordinates.Remove(node);
            node.Leader.Subordinates.AddRange(node.Subordinates);
            foreach (var sub in node.Subordinates)
            {
                sub.Leader = node.Leader;
            }
        }
    }
}
