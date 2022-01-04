using BLM16.Util.Calculator.Models;

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

    /// <param name="operators">The list of operators the calculator recognizes. Defaults to <see cref="DefaultOperatorList"/> if no value is provided.</param>
    public Calculator(Operator[] operators = null, Constant[] constants = null)
    {
        // Use the default operators if no operators are provided
        this.operators = operators ?? DefaultOperatorList;
        // Use the default constants if no constants are provided
        this.constants = constants ?? DefaultConstantList;

        standardizer = new Standardizer(this.operators, this.constants);
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
    public static Operator[] DefaultOperatorList => new Operator[]
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
}
