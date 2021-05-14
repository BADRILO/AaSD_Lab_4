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

        public bool directed { get; private set; }


        public Graph()
        {
            Console.Write("Enter number of graph vertices: ");
            while (!Int32.TryParse(Console.ReadLine(), out v_count) ||
                    v_count < 2)
            {
                Console.Write("\nInvalid input. Try again: ");
            }
            Console.WriteLine("\n");

            //initializing adjecency list
            adjacencyList = new Edge[v_count + 1];



            byte type;

            Console.Write("Graph directed(1) or non-directed(0): ");
            while (!Byte.TryParse(Console.ReadLine(), out type) ||
                   (type != 0 && type != 1))
            {
                Console.Write("\nInvalid input. Try again: ");
            }
            this.directed = type == 1 ? true : false;
            Console.WriteLine("\n");



            Console.WriteLine("Enter weights of edges, for missing edges enter 0.\n");
            for (int i = 1; i <= v_count; i++)
            {
                if (!directed)
                {
                    for (int j = i + 1; j <= v_count; j++)
                    {
                        int weight;
                        Console.Write($"From #{i, 2} to #{j, 2} weight: ");
                        while (!Int32.TryParse(Console.ReadLine(), out weight) ||
                                weight < 0)
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

                        Console.Write($"From #{i, 2} to #{j, 2} weight: ");
                        while (!Int32.TryParse(Console.ReadLine(), out weight) || weight < 0)
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
                Console.Write($"List for vertex # {i, 2}: ");
                printEdgeList(i);
            }
        }

        public void Kruskal()
        {
            if (directed) return; //this algorithm doesn't work with directed graphs

            //Creating and initializing
            List<Edge> edge_list = new(e_count);  //list of edges in graph
            List<Edge> MST_edges = new(v_count - 1); //list of edges in MST
            int[] component = new int[v_count + 1]; //array to keep numbers of connected components to avoid cycles

            //Initial int[] component
            //Each vertex is treated as a separate component.
            for (int i = 1; i <= v_count; i++)
            {
                component[i] = i;
            }

            Edge current;
            //Fill edge_list
            for (int i = 1; i <= v_count; i++)
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
                Console.WriteLine($" {MST_edges[i].v_start,2},{ MST_edges[i].v_end,2} edge weight: {MST_edges[i].weight}");
            }
            Console.WriteLine($"MST lenght: {MST_len}\n\n");
        }

        public void Dijkstra()
        {
            // CREATING AND INITIALIZING VARIABLES AND DATA STRUCTURES
            int v_source;
            int start, end, path_len;
            Edge fringe_list = null;                        // reference to a linked list of fringe edges
            Edge tree_list = null;                          // reference to linked list of SP_Tree edges
            int[] distance = new int[v_count + 1];          //array of distances
            char[] marks_status = new char[v_count + 1];    //array to register the status of each vertex: 'u'-unseen, 'f'-fringe, 't'-in MST
                                                            // reference to linked list of Shortest Path edges

            for (int i = 1; i <= v_count; i++)              //mark_status initializing
            {
                marks_status[i] = 'u';
            }

            for (int i = 1; i <= v_count; i++)              //distance initializing
            {
                distance[i] = 0;
            }

            Console.WriteLine("DIJKSTRA ALGORITHM\n\n");    //v_source initializing
            Console.Write($"Enter number of source vertex(from 1 to {v_count}): ");
            while (!Int32.TryParse(Console.ReadLine(), out v_source) ||
                    v_source < 1 ||
                    v_source > v_count)
            {
                Console.Write("\nInvalid input. Try again: ");
            }
            Console.WriteLine("\n");



            // PROCESSING START VERTEX
            Edge current = adjacencyList[v_source];
            marks_status[v_source] = 't'; //including the start vertex into Shortest Path Tree

            while (current != null)       //creating initial fringe for v_source
            {
                start = current.v_start;
                end = current.v_end;
                path_len = current.weight;
                fringe_list = insertFringeSorted(fringe_list, start, end, path_len);
                marks_status[end] = 'f';
                current = current.next;
            }



            // PROCESSING OTHER VERTICES
            while (fringe_list != null)
            {
                Edge temp_ref = fringe_list;
                fringe_list = fringe_list.next;     //excluding the 1st edge from fringe
                temp_ref.next = tree_list;          //including the edge at the beginning of Shortest Path Tree
                tree_list = temp_ref;
                marks_status[tree_list.v_end] = 't';
                distance[tree_list.v_end] = tree_list.weight;


                current = adjacencyList[tree_list.v_end];  //take a list of edges that are incident to this vertex
                while (current != null)                    //updating fringe for new vertex included in SP_Tree
                {
                    if (marks_status[current.v_end] != 't')//if not in SPT
                    {
                        start = current.v_start;
                        end = current.v_end;
                        path_len = current.weight + distance[tree_list.v_end];
                        fringe_list = insertFringeSorted(fringe_list, start, end, path_len);
                        marks_status[end] = 'f';
                    }
                    current = current.next;
                }
            }

            // PRINTING SP_TREE
            Console.WriteLine(" \nList of edges in Shortest Path tree: ");
            printTreeList(tree_list, v_source);


            // EXTRACTING SPECIFIC SHORTEST PATH FROM SP_TREE
            int v_destination;

            while (true)
            {
                Console.Write($"Enter number of destination vertex(from 1 to {v_count}, not including source vertex)\n" +
                              $"Or enter '0' to leave this loop: ");
                while (!Int32.TryParse(Console.ReadLine(), out v_destination) ||
                        v_destination == v_source ||
                        v_destination < 0 ||
                        v_destination > v_count)
                {
                    Console.Write("\nInvalid input. Try again: ");
                }
                Console.WriteLine("\n");
                if (v_destination == 0) break;


                //finding and printing path
                int length = findTreePath(tree_list, v_source, v_destination);
                Console.WriteLine($"\nPath length: {length}\n");
            }
            //make a request to continue
            int answer;

            Console.Write("Choose another vertex? \nEnter '1'(yes) or '0'(no): ");
            while (!Int32.TryParse(Console.ReadLine(), out answer) ||
                   (answer != 0 && answer != 1))
            {
                Console.Write("\nInvalid input. Try again: ");
            }
            Console.WriteLine("\n");

            if (answer == 1) this.Dijkstra();
        }

        private void printTreeList(Edge edge_list, int start)
        {
            while (edge_list != null)
            {
                Console.WriteLine($"Start vertex: {edge_list.v_start}, " +
                                  $"end vertex: {edge_list.v_end}, " +
                                  $"distance {start} ==> {edge_list.v_end}: {edge_list.weight}");
                edge_list = edge_list.next;
            }
            Console.WriteLine("\n");
        }

        private void printEdgeList(int i)
        {
            Edge current = adjacencyList[i];

            while (current != null)
            {
                Console.Write($"({i, 2}--{current.v_end, 2}|{current.weight, 2}), ");
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

        private Edge insertFringeSorted(Edge fringe, int start, int end, int weight)
        {
            Edge new_edge;
            Edge previous = null;
            Edge current = fringe;
            int insert_search = 0; //to choose different options for action


            //searching for a position to insert a new edge into a fringe linked list
            while (current != null && current.weight < weight && current.v_end != end)
            {
                previous = current;
                current = current.next;
            }

            //analyzing what to do with a new edge
            if (current == null)    //insert the edge, no search for a duplicate edge
                insert_search = 1; 
            else if (current.weight >= weight)  //insert the edge, search for a duplicate edge
                insert_search = 2; 

            //updating the fringe linked list
            if (insert_search != 0)
            {
                new_edge = new Edge(start, end, weight);

                if (previous == null)   //inserting a new candidate edge at the beginning of the fringe list
                { 
                    new_edge.next = fringe;
                    fringe = previous = new_edge;
                }
                else     //inserting within of the fringe list
                { 
                    new_edge.next = current;
                    previous.next = new_edge;
                    previous = new_edge;
                }

                if (insert_search == 2)     // searching for a possible duplicate candidate edge
                { 
                    while (current != null && current.v_end != end)
                    {
                        previous = current;
                        current = current.next;
                    }
                    if (current != null) //deleting the duplicate edge
                        previous.next = current.next;
                }
            }
            return fringe;
        }

        private int findTreePath(Edge tree, int source, int end)
        {
            Edge current = tree;

            while (current != null && current.v_end != end)
            {
                current = current.next;
            }

            if (current == null)
            {
                Console.WriteLine("Path doesn't exist!");
                return -1;
            }
            else if (current.v_start == source)
            {
                Console.Write($"{source}=={current.weight}==>{current.v_end}");
                return current.weight;
            }
            else
            {
                int prev_edge = findTreePath(tree, source, current.v_start);
                Console.Write($"=={current.weight - prev_edge}==>{current.v_end}");
                return current.weight;
            }
        }
    }
}
