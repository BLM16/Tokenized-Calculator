using BLM16.Util.Calculator.Models;
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Calculator.Tests")]
namespace BLM16.Util.Calculator;

/// <summary>
/// Contains the required logic to evaluate an equation from a string
/// </summary>
public class Calculator
{
    /// <summary>
    /// The Standardizer for standardizing the equation
    /// </summary>
    private readonly Standardizer standardizer;
    /// <summary>
    /// The Lexer for tokenizing the equation
    /// </summary>
    private readonly Lexer lexer;
    /// <summary>
    /// The Parser for evaluating the equation
    /// </summary>
    private readonly Parser parser;

    /// <summary>
    /// The operators the calculator can use
    /// </summary>
    private readonly Operator[] operators;
    /// <summary>
    /// The constants the calculator can use
    /// </summary>
    private readonly Constant[] constants;
    /// <summary>
    /// The functions the calculator can use
    /// </summary>
    private readonly Function[] functions;

    /// <param name="operators">The list of operators the calculator recognizes. Always includes <see cref="BuiltinOperatorList"/>.</param>
    /// <param name="constants">The list of constants the calculator recognizes. Defaults to <see cref="DefaultConstantList"/> if no value is provided.</param>
    /// <param name="functions">The list of functions the calculator recognizes. Defaults to <see cref="DefaultFunctionList"/> if no value is provided.</param>
    public Calculator(Operator[] operators = null, Constant[] constants = null, Function[] functions = null)
    {
        var _ops = new List<Operator>(BuiltinOperatorList); // Add default operators
        _ops.AddRange(operators ?? System.Array.Empty<Operator>()); // Add provided operators if any

        // Use the default operators if no operators are provided
        this.operators = _ops.ToArray();
        // Use the default constants if no constants are provided
        this.constants = constants ?? DefaultConstantList;
        // Use the default functions if no functions are provided
        this.functions = functions ?? DefaultFunctionList;

        standardizer = new Standardizer(this.operators, this.constants, this.functions);
        lexer = new Lexer(this.operators);
        parser = new Parser();
    }

    /// <summary>
    /// Runs the required steps to calculate a given equation
    /// </summary>
    /// <param name="equation">The equation to evaluate</param>
    /// <exception cref="MathSyntaxException">Thrown when malformed math is provided or unrecognized operators or tokens are found.</exception>
    /// <returns>The result of evaluating the equation</returns>
    public double Calculate(string equation)
    {
        equation = equation.ToLower();

        equation = standardizer.Standardize(equation);
        var tokens = lexer.Parse(equation);
        var result = parser.Evaluate(tokens);

        return result;
    }

    /// <summary>
    /// A list of the default operators used by the calculator
    /// </summary>
    public static Operator[] BuiltinOperatorList => new Operator[]
    {
        DefaultOperators.Addition,
        DefaultOperators.Subtraction,
        DefaultOperators.Multiplication,
        DefaultOperators.Division,
        DefaultOperators.Exponent
    };

    /// <summary>
    /// A list of the default constants used by the calculator
    /// </summary>
    public static Constant[] DefaultConstantList => new Constant[]
    {
        DefaultConstants.PI,
        DefaultConstants.E
    };

    /// <summary>
    /// A list of the default functions used by the calculator
    /// </summary>
    public static Function[] DefaultFunctionList => new Function[]
    {
        DefaultFunctions.Sqrt,
        DefaultFunctions.Cbrt,
        DefaultFunctions.Cos,
        DefaultFunctions.Sin,
        DefaultFunctions.Tan,
        DefaultFunctions.Asin,
        DefaultFunctions.Acos,
        DefaultFunctions.Atan,
        DefaultFunctions.Floor,
        DefaultFunctions.Ceil,
        DefaultFunctions.Log,
        DefaultFunctions.Log2,
        DefaultFunctions.Log10,
        DefaultFunctions.Abs,
        DefaultFunctions.Deg,
        DefaultFunctions.Rad
    };
}
