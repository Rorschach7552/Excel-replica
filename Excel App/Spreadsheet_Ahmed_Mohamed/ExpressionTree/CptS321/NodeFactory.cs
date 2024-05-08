using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{/// <summary>
/// we create nodes and parse it depending on wether its expression or variable or op
/// </summary>
    public class NodeFactory
    {
        public Dictionary<string, double> variables;

        // constructor
        public NodeFactory(Dictionary<string, double> variables)
        {
            this.variables = variables;
        }

        // parse the expression or op depeneding on input
        public Node CreateNode(string expression)
        {
            if (expression.StartsWith("(") && expression.EndsWith(")") && expression.LastIndexOf("(")==0 && expression.IndexOf(")")==expression.Length -1)
            {
                expression = expression.Substring(1, expression.Length - 2);
            }
            int parenthCount = 0;
            int operatorIndex = -1;
            for (int i = 0; i < expression.Length; i++)
            {
                if (parenthCount == 0 && (expression[i] == '+' || expression[i] == '-'))
                {
                    operatorIndex = i;

                }
                else if (expression[i] == '(')
                {
                    parenthCount++;
                }
                else if (expression[i] == ')')
                {
                    parenthCount--;
                }

            }

            if (operatorIndex > -1)
            {
                OperatorNode node = new OperatorNode(expression[operatorIndex]);
                node.Left = CreateNode(expression.Substring(0, operatorIndex));
                node.Right = CreateNode(expression.Substring(operatorIndex + 1));
                return node;
            }
            operatorIndex = -1;
            parenthCount = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (parenthCount == 0 && (expression[i] == '/' || expression[i] == '*'))
                {
                    operatorIndex = i;

                }
                else if (expression[i] == '(')
                {
                    parenthCount++;
                }
                else if (expression[i] == ')')
                {
                    parenthCount--;
                }

            }
            if (operatorIndex > -1)
            {
                OperatorNode node = new OperatorNode(expression[operatorIndex]);
                node.Left = CreateNode(expression.Substring(0, operatorIndex));
                node.Right = CreateNode(expression.Substring(operatorIndex + 1));
                return node;
            }

            double constantValue;
            bool isNumber;
            isNumber = double.TryParse(expression, out constantValue);
            if (isNumber == true)
            {
                ConstantNode constantNode = new ConstantNode(constantValue);
                return constantNode;
            }
            else
            {
                VariableNode variable = new VariableNode(expression, this.variables);
                return variable;
            }
        }
    }
}
