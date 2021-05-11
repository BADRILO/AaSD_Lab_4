using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    class Graph
    {
        private int v_count = 0;
        private int e_count = 0;
        private Edge[] adjacencyList;


        public Graph()
        {
            Console.Write("Enter number of graph vertices: ");
            while (!Int32.TryParse(Console.ReadLine(), out v_count))
            {
                Console.Write("\nInvalid input. Try again: ");
            }
            Console.WriteLine("\n");

            //initial adjecency list
            adjacencyList = new Edge[v_count + 1];

            byte directed;
            Console.Write("Graph directed(1) or non-directed(0): ");
            while (!Byte.TryParse(Console.ReadLine(), out directed))
            {
                Console.Write("\nInvalid input. Try again: ");
            }
            Console.WriteLine("\n");

            Console.WriteLine("Enter weights of edges, for missing edges enter 0.\n");
            for (int i = 1; i <= v_count; i++)
            {
                if (directed == 0)
                {
                    for (int j = i + 1; j <= v_count; j++)
                    {
                        int weight;
                        Console.Write($"From #{i} to #{j} weight == ");
                        while (!Int32.TryParse(Console.ReadLine(), out weight) && weight < 0)
                        {
                            Console.Write("\nInvalid input. Try again: ");
                        }

                        if (weight != 0)
                        {
                            insertSortedNodes(i, j, weight);
                            insertSortedNodes(j, i, weight);
                            this.e_count += 2;
                        }
                    }
                }
                else
                {
                    for (int j = 1; j <= v_count; j++)
                    {
                        if (j == i) continue;

                        int weight;
                        Console.Write($"From #{i} to #{j} weight == ");
                        while (!Int32.TryParse(Console.ReadLine(), out weight) && weight < 0)
                        {
                            Console.Write("\nInvalid input. Try again: ");
                        }

                        if (weight != 0)
                        {
                            insertSortedNodes(i, j, weight);
                            this.e_count++;
                        }
                    }
                }

            }
            Console.WriteLine();

            for (int i = 1; i <= v_count; i++)
            {
                Console.Write("list for vertex #" + i + ": ");
                printList(i);
            }
        }

        public void Kruskal()
        {
            //Creating and initializing
            List<Edge> edge_list = new(e_count);  //list of edges in graph
            List<Edge> MST_edges = new(v_count - 1); //list of edges in MST
            int[] component = new int[v_count + 1]; //array to keep numbers of connected components to avoid cycles

            //initial int[] component
            //Each vertex is treated as a separate component.
            for (int i = 1; i <= v_count; i++)
            {
                component[i] = i;
            } 

            Edge current;
            //fill edge_list
            for(int i = 1; i <= v_count; i++)
            {
                current = adjacencyList[i];

                while (current != null)
                {
                    if (i < current.v_end)
                    {
                        edge_list.Add(new Edge(current.v_start, current.v_end, current.weight));
                    }
                    current = current.next;
                }
            }

            //Sorting edges by weights
            edge_list.Sort(new Comparison<Edge>((e1, e2) => e1.weight - e2.weight));

            //Selecting edges and including into MST_edges
            for (int i = 0; i < edge_list.Count && MST_edges.Count < v_count - 1; i++)
            {
                int a = edge_list[i].v_start;
                int b = edge_list[i].v_end;

                //both vertices belong to different components (to avoid cycles)
                if (component[a] != component[b])
                { 
                    MST_edges.Add(edge_list[i]);

                    //updating the list of connected components
                    int keep = component[b];

                    for (int j = 1; j <= v_count; j++)
                    {
                        if (component[j] == keep)
                        {
                            component[j] = component[a];
                        }
                    }         
                }
            }

            //Printing
            int MST_len = 0;
            Console.WriteLine(" \nList of edges in MST: ");
            for (int i = 0; i < MST_edges.Count; i++)
            {
                MST_len += MST_edges[i].weight;
                Console.WriteLine($" {MST_edges[i].v_start, 2},{ MST_edges[i].v_end, 2} edge weight: {MST_edges[i].weight}");
            }
            Console.WriteLine($"MST lenght: {MST_len}\n\n");
        }

        private void printList(int i)
        {
            Edge current = adjacencyList[i];

            while (current != null)
            {
                Console.Write($"({i}--{current.v_end}|{current.weight}), ");
                current = current.next;
            }
            Console.WriteLine();
        }

        private void insertSortedNodes(int start, int end, int weight)
        {
            Edge current = adjacencyList[start];
            Edge previous = null;  


            //search position for inserting
            while (current != null)
            {
                if (current.v_end < end)
                {
                    previous = current;
                    current = current.next;
                }
                else
                {
                    break;
                }
            }

            Edge new_edge = new Edge(start, end, weight);

            //inserting at the beginning of a linked list
            if (previous == null)
            {
                new_edge.next = adjacencyList[start];
                adjacencyList[start] = new_edge;
            }
            //inserting in other cases
            else
            {
                new_edge.next = current;
                previous.next = new_edge;
            }
        }
    }
}
