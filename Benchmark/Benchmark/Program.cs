using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Benchmark
{
    class Settings
    {
        public const int NrOfNodes = 15000;
        public const int NrOfLinks = 20;
        public const int NrOfIterations = 100;
    }

    public static class Statistics
    {
        public static double NrOfCalcualtions = 0;
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
        public int NodeId;

        public Link(double weight, int nodeId)
        {
            Weight = weight;
            NodeId = nodeId;
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
            for (int i = 0; i < Settings.NrOfNodes; i++)
            {
                nodes[i] = new Node();
            }
            for (int i = 0; i < Settings.NrOfNodes; i++)
            {
                for (int j = 0; j < Settings.NrOfLinks; j++)
                {
                    double weight = random.NextDouble();
                    int nodeId = random.Next() % Settings.NrOfNodes;
                    nodes[i].Links[j] = new Link(weight, nodeId);
                }
            }
            Trace.WriteLine(String.Format("Init in {0}ms", stopwatch.Elapsed.TotalMilliseconds));
            double[] endResult = new double[Settings.NrOfNodes]; ;
            int nrOfProcessors = 15;
            for (int tries = 0; tries < 10; tries++)
            {
                stopwatch.Restart();
                double[] results = new double[Settings.NrOfIterations * Settings.NrOfNodes];

                results[0] = 1;

                for (int iteration = 1; iteration < Settings.NrOfIterations; iteration++)
                {
                    int index = iteration * Settings.NrOfNodes;

                    double[,] resultsPerProcessor = new double[Settings.NrOfNodes,nrOfProcessors];
                    Parallel.For(0, nrOfProcessors, (processor) =>
                   {
                       for (int nodeId = Settings.NrOfNodes / nrOfProcessors * processor; nodeId < Settings.NrOfNodes / nrOfProcessors * (processor + 1); nodeId++)
                       {
                           double nodeWeight = results[index - Settings.NrOfNodes + nodeId];
                           if (nodeWeight != 0.0)
                           {
                               for (int linkId = 0; linkId < Settings.NrOfLinks; linkId++)
                               {
                                   Node node = nodes[nodeId];
                                   Link link = node.Links[linkId];
                                   resultsPerProcessor[link.NodeId, processor] += link.Weight * nodeWeight;
                               }
                           }
                       }
                   });
                    Parallel.For(0, Settings.NrOfNodes, (nodeId) =>
                    {
                        for(int processor = 0; processor < nrOfProcessors; processor++)
                        {
                            results[index + nodeId] += resultsPerProcessor[nodeId,processor];
                        }
                    });
                }
                endResult = new double[Settings.NrOfNodes];

                for (int nodeId = 0; nodeId < Settings.NrOfNodes; nodeId++)
                {
                    for (int iteration = 0; iteration < Settings.NrOfIterations; iteration++)
                    {
                        endResult[nodeId] += results[iteration*Settings.NrOfNodes + nodeId];
                    }
                }


                Trace.WriteLine(String.Format("Calculate in {0}ms", stopwatch.Elapsed.TotalMilliseconds));
            }

            for(int i = 0; i < 100; i++)
            {
                Trace.Write(String.Format("{0} ", endResult[i]));
            }
        }


    }
}
