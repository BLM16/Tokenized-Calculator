using System;
using BLM16.Util.Calculator;

while (true)
{
    try
    {
        Console.Write("Enter your equation: ");
        var eq = Console.ReadLine();

        var calculator = new Calculator();
        var res = calculator.Calculate(eq);

        Console.WriteLine($"Result: {res}\n");
    }
    catch (MathSyntaxException e)
    {
        Console.WriteLine(e + "\n");
    }
}
