using CptS321;
/// <summary>
/// represent menu items and store user input as long as he didnt quite
/// </summary>
ExpressionTree expressionTree = new ExpressionTree("(1+2)*(2+1)/2");
string userInput = "";
while (userInput.Equals("4") == false)
{
    Console.WriteLine("Menu (current expression=" + expressionTree.Expression + ")");
    Console.WriteLine("1-enter new expression");
    Console.WriteLine("2-set a variable value");
    Console.WriteLine("3-Evaluate Tree");
    Console.WriteLine("4-quit");
    userInput = Console.ReadLine();
    if (userInput.Equals("1"))
    {
        Console.Write("Enter new expression: ");
        string expression = Console.ReadLine();
        expressionTree.Expression = expression;
        expressionTree.BuildExrpessionTree(expression);
    }
    else if (userInput.Equals("2"))
    {
        Console.Write("Enter variable name: ");
        string variableName = Console.ReadLine();
        Console.Write("enter variable value: ");
        double variableValue = Convert.ToDouble(Console.ReadLine());
        expressionTree.SetVariable(variableName, variableValue);
    }
    else if (userInput.Equals("3"))
    {
        Console.WriteLine(expressionTree.Evaluate());
    }
    else if (userInput.Equals("4"))
    {
        Console.WriteLine("done!");
    }
}
