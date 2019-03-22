using System;
using System.Diagnostics;

namespace Benchmark
{
    class Settings
    {
        public const int NrOfNodes = 150;
        public const int NrOfLinks = 20;
    }

    class Node
    {
        public Link[] Links;

        public Node()
        {
            Links = new Link[Settings.NrOfLinks];
        }
    }

    class Link
    {
        public double Weight;
        public Node Node;

        public Link(double weight, Node node)
        {
            Weight = weight;
            Node = node;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Node[] nodes = new Node[Settings.NrOfNodes];
            Random random = new Random();
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int i= 0;i < Settings.NrOfNodes; i++)
            {
                nodes[i] = new Node();
            }
            for (int i = 0; i < Settings.NrOfNodes; i++)
            {
                for(int j=0; j < Settings.NrOfLinks; j++)
                {
                    double weight = random.NextDouble();
                    int nodeId = random.Next() % Settings.NrOfNodes;
                    nodes[i].Links[j] = new Link(weight, nodes[nodeId]);
                }
            }
            Trace.WriteLine(String.Format("Init in {0}ms", stopwatch.Elapsed.TotalMilliseconds));

            stopwatch.Restart();
        }
    }
}
