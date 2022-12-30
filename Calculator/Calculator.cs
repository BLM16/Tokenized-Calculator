using BLM16.Util.Calculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Calculator.Tests")]
namespace BLM16.Util.Calculator;

/// <summary>
/// Contains the required logic to evaluate an equation from a string
/// </summary>
public class Calculator
{
    /// <summary>
    /// Contains a list of all the symbols used
    /// </summary>
    private readonly List<string> _symbols = new();

    private Operator[] _operators = Array.Empty<Operator>();
    private Constant[] _constants = DefaultConstantList;
    private Function[] _functions = DefaultFunctionList;

    /// <summary>
    /// The operators the calculator can use.
    /// Always includes <see cref="BuiltinOperatorList"/>.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when duplicate operators are encountered.</exception>
    public Operator[] Operators
    {
        get => _operators;
        init
        {
            var symbols = value.Select(o => o.Symbol.ToString());

            CheckDuplicateSymbolsAndThrow(symbols);

            _symbols.AddRange(symbols);
            _operators = _operators.Concat(value).ToArray();
        }
    }

    /// <summary>
    /// The constants the calculator can use
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when duplicate constants are encountered.</exception>
    public Constant[] Constants
    {
        get => _constants;
        init
        {
            var symbols = value.SelectMany(c => c.Symbols);

            CheckDuplicateSymbolsAndThrow(symbols);

            _symbols.AddRange(symbols);
            _constants = value;
        }
    }

    /// <summary>
    /// The functions the calculator can use
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when duplicate functions are encountered.</exception>
    public Function[] Functions
    {
        get => _functions;
        init
        {
            var symbols = value.SelectMany(f => f.Symbols);

            CheckDuplicateSymbolsAndThrow(symbols);
                
            _symbols.AddRange(symbols);
            _functions = value;
        }
    }

    public Calculator()
    {
        // Initialize Operators here instead of the backing field so the custom
        //  init logic adds the operator symbols to _symbols.
        // Functions and Constants are overwritten instead of preserving builtins
        //  therefore their symbols should not be added to _symbols and the init
        //  logic does not need to run. This stops builtin operator duplication.
        Operators = BuiltinOperatorList;
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

        equation = new Standardizer(Operators, Constants, Functions).Standardize(equation);
        var tokens = new Lexer(Operators).Parse(equation);
        var result = new Parser().Evaluate(tokens);

        return result;
    }

    /// <summary>
    /// Checks if <paramref name="symbols"/> contains any duplicate elements
    /// or if symbols has any duplicates in <see cref="_symbols"/>.
    /// </summary>
    /// <param name="symbols">The symbols to check duplicates for</param>
    /// <exception cref="ArgumentException">Thrown when duplicate elements are encountered.</exception>
    private void CheckDuplicateSymbolsAndThrow(IEnumerable<string> symbols)
    {
        var duplicateSymbolsWithExisting = symbols.Intersect(_symbols);
        var duplicateSymbolsInGiven = symbols.GroupBy(s => s)
                                             .Where(s => s.Count() > 1)
                                             .Select(s => s.Key);

        var duplicates = duplicateSymbolsWithExisting.Union(duplicateSymbolsInGiven).ToArray();
        
        if (duplicates.Length > 0)
            throw new ArgumentException($"Duplicate symbols found: {string.Join(", ", duplicates)}");
    }

    #region Default Symbols

    /// <summary>
    /// A list of the default operators used by the calculator
    /// </summary>
    public static Operator[] BuiltinOperatorList => new[]
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
    public static Constant[] DefaultConstantList => new[]
    {
        DefaultConstants.PI,
        DefaultConstants.E
    };

    /// <summary>
    /// A list of the default functions used by the calculator
    /// </summary>
    public static Function[] DefaultFunctionList => new[]
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

    #endregion
}
