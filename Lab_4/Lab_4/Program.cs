using System;

namespace Lab_4
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new();
            graph.Prim_SAR_ADJ();
            graph.Kruskal();
            graph.Dijkstra();
        }
    }
}
