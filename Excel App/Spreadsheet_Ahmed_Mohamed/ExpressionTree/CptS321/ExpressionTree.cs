namespace CptS321
{
    /// <summary>
    /// class for building the expression tree.
    /// </summary>
    public class ExpressionTree
    {
        public string Expression;
        public Dictionary<string, double> variables;
        public Node Root;

        // input expression will build dictionary with value of expression
        public ExpressionTree(string expression)
        {
            this.Expression = expression;
            this.variables = new Dictionary<string, double>();
            this.BuildExrpessionTree(expression);
        }

        // parses expressions into numbers constant and operands
        public void BuildExrpessionTree(string expression)
        {
            NodeFactory nodeFactory = new NodeFactory(variables);
            this.Root = nodeFactory.CreateNode(expression);
        }

        // input variable name and value inputs in dictionary
        public void SetVariable(string variableName, double variableValue)
        {
            this.variables[variableName] = variableValue;
        }

        // evaluate value at root
        public double Evaluate()
        {
            this.BuildExrpessionTree(this.Expression);
            return this.Root.getValue();
        }

    }
}