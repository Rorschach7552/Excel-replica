using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// inherets from node class override to return constant value
    /// </summary>
    public class ConstantNode : Node
    {
        private double constantValue;

        public ConstantNode(double constantValue)
        {
            this.constantValue = constantValue;
        }

        public override double getValue()
        {
            return this.constantValue;
        }
    }
}
