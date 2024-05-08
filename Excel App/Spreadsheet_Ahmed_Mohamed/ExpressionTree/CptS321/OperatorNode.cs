using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// return the operation set for  each symbol
    /// </summary>
    public class OperatorNode : Node
    {
        private char operation;
        public Node Left;
        public Node Right;

        public OperatorNode(char operation)
        {
            this.operation = operation;
        }

        // create defferent ops depending on the op input
        public override double getValue()
        {
            if (this.operation == '+')
            {
                return this.Left.getValue() + Right.getValue();
            }
            else if (this.operation == '-')
            {
                return this.Left.getValue() - this.Right.getValue();
            }
            else if (this.operation == '*')
            {
                return this.Left.getValue() * this.Right.getValue();
            }
            else if (this.operation == '/')
            {
                return this.Left.getValue() / this.Right.getValue();
            }
            else
            {
                return 0;
            }
        }
    }
}
