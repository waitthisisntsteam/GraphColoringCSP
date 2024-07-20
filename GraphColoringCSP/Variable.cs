using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoringCSP
{
    public class Variable
    {
        public string Slot;

        public int Color;
        public int Guess;
        public List<int> Domain;

        public List<Variable> Neighbors;

        public Variable(int data, string slot)
        {
            Slot = slot;

            Neighbors = new();

            Color = data;

            Guess = 0;

            Domain = new();
            Domain.Add(1);
            Domain.Add(2);
            Domain.Add(3);
        }

        public void AddNeighbor(Variable Neighbor)
        {
            Neighbors.Add(Neighbor);
            Neighbor.Neighbors.Add(this);
        }
    }
}
