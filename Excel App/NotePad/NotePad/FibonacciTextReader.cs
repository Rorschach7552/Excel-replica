using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
namespace NotePad
{ /// <summary>
/// FibonacciTextReader inherets fromtext reader.
/// </summary>
    public class FibonacciTextReader : System.IO.TextReader
    {
        private int count = 0;
        private readonly int max;
        private readonly BigInteger[] fibArray;

        // constructor for Textreader Class
        public FibonacciTextReader(int length) // input length for max number of inputs for fibonacci series
        {
            max = length;
            fibArray = FibonacciGenrator(length);
        }

        /// <summary>
        /// this method takes in length of an array as an input/ output is array with size length full of fib numbers.
        /// </summary>
        /// <param name="length"></param>
        /// <return array with size=length with fib numbers ></returns>
        public static BigInteger[] FibonacciGenrator(int length) // method to generate fibonacci array with nth size
        {
            BigInteger[] array = new BigInteger[length];

            array[0] = 0;

            array[1] = 1;

            for (int i = 2; i < length; i++)
            {
                array[i] = array[i - 1] + array[i - 2];
            }

            return array;
        }

        public override string? ReadLine()
        {
            string fibINdex = this.fibArray[this.count].ToString();
            this.count++;
            return fibINdex;
        }

        public override string ReadToEnd()
        {
            StringBuilder sb = new();

            for (int i = 0; i < max; i++)
            {
                sb.AppendLine(ReadLine());
            }

            return sb.ToString();
        }
    }
}
