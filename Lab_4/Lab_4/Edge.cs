using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    internal class Edge
    {
        public int v_start;
        public int v_end;
        public int weight;
        public Edge next;

        public Edge(int start, int end, int weight)
        {
            this.v_start = start;
            this.v_end = end;
            this.weight = weight;
            this.next = null;
        }
    }
}
