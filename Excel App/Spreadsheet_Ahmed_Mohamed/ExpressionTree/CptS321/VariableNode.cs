using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// inherents from node class constructor returns variable name and variable.
    /// </summary>
    public class VariableNode : Node
    {
        private string variableName;
        private Dictionary<string, double> variables;

        // input variable name and dict of var values
        public VariableNode(string variableName, Dictionary<string, double> variables)
        {
            this.variableName = variableName;
            this.variables = variables;
        }

        // override getvalue
        public override double getValue()
        {
            try
            {
                return this.variables[this.variableName];
            }
            catch (Exception ex)
            {
                return 0.0;
            }
        }
    }
}
