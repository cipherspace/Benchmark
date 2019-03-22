using System;
using System.Diagnostics;

namespace Benchmark
{
    class Settings
    {
        public const int NrOfNodes = 150;
        public const int NrOfLinks = 20;
        public const int NrOfIterations = 10;
    }

    public static class Statistics
    {
        public static double NrOfCalcualtions = 0;
    }

    class Node
    {
        public Link[] Links;
        public double Weight;

        public Node()
        {
            Links = new Link[Settings.NrOfLinks];
            Weight = 0;
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
            nodes[0].Weight = 1;

            calculateNode(0, nodes[0]);

        }

        public static void calculateNode(int iteration, Node node)
        {
            Trace.WriteLine(String.Format("{0} iteration:{1} Node:{2}", Statistics.NrOfCalcualtions, iteration, node));
            Statistics.NrOfCalcualtions++;

            if (iteration > Settings.NrOfIterations)
            {
                return;
            }
            else
            {
                for(int i = 0; i < Settings.NrOfLinks; i++)
                {
                    node.Links[i].Node.Weight += node.Weight * node.Links[i].Weight;
                }
                for (int i = 0; i < Settings.NrOfLinks; i++)
                {
                    calculateNode(iteration + 1, node.Links[i].Node);
                }
            }
        }
    }
}
