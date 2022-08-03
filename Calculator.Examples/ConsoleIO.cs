using System;
using BLM16.Util.Calculator;
using BLM16.Util.Calculator.Models;

var modulusOperator = new Operator('%', 20, (a, b) => a % b);

var calculator = new Calculator(new[] { modulusOperator });

while (true)
{
    try
    {
        Console.Write("Enter your equation: ");
        var eq = Console.ReadLine();

        var res = calculator.Calculate(eq);

        Console.WriteLine($"Result: {res}\n");
    }
    catch (MathSyntaxException e)
    {
        Console.WriteLine(e + "\n");
    }
}
