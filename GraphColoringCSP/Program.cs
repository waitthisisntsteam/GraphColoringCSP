using System.Linq;
using System.Net;

namespace GraphColoringCSP
{
    internal class Program
    {
        static KeyValuePair<List<Variable>, List<Constraint>> Generate3X3()
        {
            List<Variable> Variables = new List<Variable>();

            Variable A1 = new Variable(0, "A1");
            Variable A2 = new Variable(0, "A2");
            Variable A3 = new Variable(0, "A3");
            Variable B1 = new Variable(0, "B1");
            Variable B2 = new Variable(0, "B2");
            Variable B3 = new Variable(0, "B3");
            Variable C1 = new Variable(0, "C1");
            Variable C2 = new Variable(0, "C2");
            Variable C3 = new Variable(0, "C3");

            A1.AddNeighbor(A2);
            A1.AddNeighbor(B1);

            A2.AddNeighbor(B1);
            A2.AddNeighbor(B2);
            A2.AddNeighbor(A3);

            A3.AddNeighbor(B2);
            A3.AddNeighbor(B3);

            B1.AddNeighbor(C1);
            B1.AddNeighbor(B2);

            B2.AddNeighbor(C1);
            B2.AddNeighbor(C2);
            B2.AddNeighbor(B3);

            B3.AddNeighbor(C2);
            B3.AddNeighbor(C3);

            C1.AddNeighbor(C2);

            C2.AddNeighbor(C3);

            Variables.Add(A1); // 0
            Variables.Add(A2); // 1
            Variables.Add(A3); // 2
            Variables.Add(B1); // 3
            Variables.Add(B2); // 4
            Variables.Add(B3); // 5
            Variables.Add(C1); // 6
            Variables.Add(C2); // 7
            Variables.Add(C3); // 8



            List<Constraint> Constraints = new();

            Constraints.Add(new Constraint([A1, A2], CSPCheck));
            Constraints.Add(new Constraint([A1, B1], CSPCheck));
            Constraints.Add(new Constraint([A2, B1], CSPCheck));
            Constraints.Add(new Constraint([A2, B2], CSPCheck));
            Constraints.Add(new Constraint([A2, A3], CSPCheck));
            Constraints.Add(new Constraint([A3, B2], CSPCheck));
            Constraints.Add(new Constraint([A3, B3], CSPCheck));
            Constraints.Add(new Constraint([B1, C1], CSPCheck));
            Constraints.Add(new Constraint([B1, B2], CSPCheck));
            Constraints.Add(new Constraint([B2, C1], CSPCheck));
            Constraints.Add(new Constraint([B2, C2], CSPCheck));
            Constraints.Add(new Constraint([B2, B3], CSPCheck));
            Constraints.Add(new Constraint([B3, C2], CSPCheck));
            Constraints.Add(new Constraint([B3, C3], CSPCheck));
            Constraints.Add(new Constraint([C1, C2], CSPCheck));
            Constraints.Add(new Constraint([C2, C3], CSPCheck));

            return new KeyValuePair<List<Variable>, List<Constraint>>(Variables, Constraints);
        }

        static void Print3x3(List<Variable> path)
        {
            Console.WriteLine(path[0].Guess + "-" + path[3].Guess + "-" + path[6].Guess);
            Console.WriteLine("|/|/|");
            Console.WriteLine(path[1].Guess + "-" + path[4].Guess + "-" + path[7].Guess);
            Console.WriteLine("|/|/|");
            Console.WriteLine(path[2].Guess + "-" + path[5].Guess + "-" + path[8].Guess);
            Console.WriteLine();
            Console.WriteLine();
        }

        static bool CSPCheck(List<Variable> partsOfProblem) => !(partsOfProblem[0].Guess == partsOfProblem[1].Guess);

        static bool ConstraintCheck(List<Variable> partsOfProblem, List<Constraint> constraints)
        {
            List<Variable> usableVariables = new();

            foreach (Constraint constraint in constraints)
            {
                usableVariables.Clear();
                foreach (Variable variable in constraint.PartsOfProblem)
                {
                    if (partsOfProblem.Contains(variable))
                    {
                        usableVariables.Add(variable);
                    }
                }

                if (usableVariables.Count == constraint.PartsOfProblem.Count && !constraint.ConstraintFunction(usableVariables))
                {
                    return false;
                }
            }

            return true;
        }

        static void GenerateVariableRoutes(List<Variable> variables, List<Constraint> constraints)
        {
            List<List<Variable>> possibleValuePaths = new();

            int currentVariableIndex = 0;
            for (int domainIndex = 0; domainIndex < variables[currentVariableIndex].Domain.Count; domainIndex++)
            {               
                for (int i = 0; i < variables.Count; i++)
                {
                    variables[i].Guess = 0;
                }

                variables[currentVariableIndex].Guess = variables[currentVariableIndex].Domain[domainIndex];
                List<Variable> currentPath = new([variables[currentVariableIndex]]);
                GenerateVariableRouteHelper(variables, constraints, currentVariableIndex + 1, currentPath);
            }
        }

        static void GenerateVariableRouteHelper(List<Variable> variables, List<Constraint> constraints, int currentVariableIndex, List<Variable> currentPath)
        {
            if (currentVariableIndex < variables.Count)
            {
                for (int domainIndex = 0; domainIndex < variables[currentVariableIndex].Domain.Count; domainIndex++)
                {
                    variables[currentVariableIndex].Guess = variables[currentVariableIndex].Domain[domainIndex];

                    List<Variable> partsOfProblem = new([variables[currentVariableIndex]]);
                    for (int neighborIndex = 0; neighborIndex < variables[currentVariableIndex].Neighbors.Count; neighborIndex++)
                    {
                        if (variables[currentVariableIndex].Neighbors[neighborIndex].Guess != 0)
                        {
                            partsOfProblem.Add(variables[currentVariableIndex].Neighbors[neighborIndex]);
                        }
                    }

                    if (ConstraintCheck(partsOfProblem, constraints))
                    {
                        currentPath.Add(variables[currentVariableIndex]);                   
                        GenerateVariableRouteHelper(variables, constraints, currentVariableIndex + 1, currentPath);                    
                        currentPath.Remove(variables[currentVariableIndex]);
                    }
                    variables[currentVariableIndex].Guess = 0;
                }
            }
            else
            {
                Print3x3(currentPath);
            }
        }

        static void Main(string[] args)
        {
            var board = Generate3X3();

            GenerateVariableRoutes(board.Key, board.Value);
        }
    }
}
