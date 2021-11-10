using System;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter your equation: ");
                    var eq = Console.ReadLine();
            
                    var calculator = new Calculator.Calculator();
                    var res = calculator.Calculate(eq);

                    Console.WriteLine($"Result: {res}\n");
                }
                catch (Calculator.MathSyntaxException e)
                {
                    Console.WriteLine(e + "\n");
                }
            }
        }
    }
}
