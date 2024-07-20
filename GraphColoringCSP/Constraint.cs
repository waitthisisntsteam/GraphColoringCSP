using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoringCSP
{
    public class Constraint
    {
        public List<Variable> PartsOfProblem;
        public Func<List<Variable>, bool> ConstraintFunction;

        public Constraint(List<Variable> variables, Func<List<Variable>, bool> constraintFunc) 
        {
            PartsOfProblem = new();

            foreach (var v in variables)
            {
                PartsOfProblem.Add(v);
            }

            ConstraintFunction = constraintFunc;
        }
    }
}
