using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    class Edge
    {
        protected int v_start;
        protected int v_end;
        protected int weight;
        protected Edge next;

        public Edge(int start, int end, int weight)
        {
            this.v_start = start;
            this.v_end = end;
            this.weight = weight;
            this.next = null;
        }
    }
}
