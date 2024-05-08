
using CptS321;
using NUnit.Framework;

namespace Expression_Tree_Test
{
    public class Tests
    {
        ExpressionTree expressionTree;

        [SetUp]
        public void Setup()
        {
            this.expressionTree = new ExpressionTree("A1-12-C1");
            this.expressionTree.SetVariable("A1", 3);
            this.expressionTree.SetVariable("C1", 1);
        }

        // test used in example
        [Test]
        public void Test1()
        {
            Assert.AreEqual(this.expressionTree.Evaluate(), -10);
        }

        // just numbers with  no variables
        [Test]
        public void Test2()
        {
            this.expressionTree = new ExpressionTree("1+2+3");
            Assert.AreEqual(this.expressionTree.Evaluate(), 6);
        }

        // differend operator
        [Test]
        public void Test3()
        {
            this.expressionTree = new ExpressionTree("2*2*3");
            Assert.AreEqual(this.expressionTree.Evaluate(), 12);
        }

        // divide operator
        [Test]
        public void Test4()
        {
            this.expressionTree = new ExpressionTree("12/4");
            Assert.AreEqual(this.expressionTree.Evaluate(), 3);
        }

        // just a number
        [Test]
        public void Test5()
        {
            this.expressionTree = new ExpressionTree("1");
            Assert.AreEqual(this.expressionTree.Evaluate(), 1);
        }

        // just a variable
        [Test]
        public void Test6()
        {
            this.expressionTree = new ExpressionTree("c");
            this.expressionTree.SetVariable("c", 0);
            Assert.AreEqual(this.expressionTree.Evaluate(), 0);
        }

        // long expression
        [Test]
        public void Test7()
        {
            this.expressionTree = new ExpressionTree("a+b+25+c+20+0");
            this.expressionTree.SetVariable("a", 10);
            this.expressionTree.SetVariable("b", 2);
            this.expressionTree.SetVariable("c", 1);
            Assert.AreEqual(this.expressionTree.Evaluate(), 58);

        }

        // testing heirachy of ops
        [Test]
        public void Test8()
        {
            this.expressionTree = new ExpressionTree("5-1/4");

            Assert.AreEqual(this.expressionTree.Evaluate(), 4.75);
        }

        // parenthesis 
        [Test]
        public void Test9()
        {
            this.expressionTree = new ExpressionTree("(5-1)/4");

            Assert.AreEqual(this.expressionTree.Evaluate(), 1);
        }

        // complicated expression
        [Test]
        public void Test10()
        {
            this.expressionTree = new ExpressionTree("(2+1)*(1-2)/2");

            Assert.AreEqual(this.expressionTree.Evaluate(), -1.5);

        }
    }
}
