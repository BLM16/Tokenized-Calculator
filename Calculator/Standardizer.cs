using BLM16.Util.Calculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BLM16.Util.Calculator;

/// <summary>
/// Has the logic to standardize an equation into something the calculator can parse
/// </summary>
internal class Standardizer
{
    /// <summary>
    /// The list of operators recognized by the calculator
    /// </summary>
    private readonly Operator[] Operators;
    /// <summary>
    /// The list of constants recognized by the calculator
    /// </summary>
    private readonly Constant[] Constants;
    /// <summary>
    /// The list of functions recognized by the calculator
    /// </summary>
    private readonly Function[] Functions;

    public Standardizer(Operator[] operators, Constant[] constants, Function[] functions)
    {
        Operators = operators;
        Constants = constants;
        Functions = functions;
    }

    /// <summary>
    /// Standardizes an equation by inserting brackets and operators where needed
    /// </summary>
    /// <param name="equation">The equation to standardize</param>
    /// <returns>The provided equation that is completely standardized</returns>
    public string Standardize(string equation) => AddMultiplicationSigns(ReplaceConstants(ComputeFunctions(FixBrackets(FixRepeatingOperators(RemoveWhitespace(equation))))));

    /// <summary>
    /// Removes all the whitespace characters in a string
    /// </summary>
    /// <param name="equation">The equation to remove whitespace from</param>
    /// <returns>The provided equation with removed whitespace</returns>
    private static string RemoveWhitespace(string equation)
    {
        // Matches all whitespace characters
        var whitespaceChars = new Regex(@"\s+");
        return whitespaceChars.Replace(equation, "");
    }

    /// <summary>
    /// Fixes the equation by ensuring there are equal number of brackets
    /// </summary>
    /// <param name="equation">The equation to fix the brackets in</param>
    /// <exception cref="MathSyntaxException">Thrown when there are more closing brackets than opening ones.</exception>
    /// <returns>The provided equation with the correct number of brackets</returns>
    private static string FixBrackets(string equation)
    {
        int lBrack = 0, rBrack = 0;

        // Count the number of each bracket
        foreach (var c in equation)
        {
            if (c == '(') ++lBrack;
            else if (c == ')') ++rBrack;
        }

        // Too many closing brackets
        if (rBrack > lBrack)
        {
            throw new MathSyntaxException("Equation has too many closing brackets");
        }
        else if (lBrack > rBrack)
        {
            // Append closing brackets to the end of the equation as needed
            for (var i = rBrack; i < lBrack; i++)
            {
                equation += ")";
            }
        }

        return equation;
    }

    /// <summary>
    /// Replaces repeated operators in an equation with a simper representation
    /// </summary>
    /// <param name="equation">The equation to standardize repeating operators in</param>
    /// <returns>The provided equation with standardized repeating operators</returns>
    public static string FixRepeatingOperators(string equation)
    {
        while (equation.Contains("--") || equation.Contains("+-"))
        {
            equation = equation.Replace("--", "+");
            equation = equation.Replace("+-", "-");
        }
        return equation;
    }

    /// <summary>
    /// Replaces the functions in an equation with their results
    /// </summary>
    /// <param name="equation">The equation to solve functions in</param>
    /// <returns>The provided equation with computed functions</returns>
    /// <exception cref="MathSyntaxException">Thrown if incorrect function syntax is found</exception>
    private string ComputeFunctions(string equation)
    {
        // Maps function's symbols to their operation
        var funcsDict = new Dictionary<string, Func<double, string>>();
        Array.ForEach(Functions, f =>
            Array.ForEach(f.Symbols, s => funcsDict.Add(s.ToLower(), f.Operation)));

        // Order the dictionary by key length
        List<KeyValuePair<string, Func<double, string>>> functions = funcsDict.OrderByDescending(e => e.Key.Length).ToList();

        foreach (var f in functions)
        {
            // Loop through all occurences of the function name in the equation
            while (equation.IndexOf(f.Key) != -1)
            {
                // Find the start index of the function
                var startIndex = equation.IndexOf(f.Key);
                if (startIndex + f.Key.Length >= equation.Length || equation[startIndex + f.Key.Length] != '(')
                {
                    // Function identifier isn't followed by (
                    throw new MathSyntaxException("Function identifier not followed by parentheses: " + f.Key + "?");
                }

                // Find the end index of the function keeping in mind nested parentheses are allowed
                int endIndex = 0, lBrack = 0;
                for (int i = startIndex + f.Key.Length + 1; i < equation.Length; i++)
                {
                    if (equation[i] == ')')
                    {
                        if (lBrack == 0)
                        {
                            endIndex = i;
                            break;
                        }
                        else
                        {
                            lBrack--;
                        }
                    }
                    else if (equation[i] == '(')
                    {
                        lBrack++;
                    }
                }

                // Create a new calculator with the same operators, constants, and functions to recursively evalutate the function's contents
                var calc = new Calculator(Operators, Constants, Functions);

                // Get the value captured by the function
                var sub = equation[(startIndex + f.Key.Length + 1)..endIndex];
                // Compute the function
                var result = f.Value(calc.Calculate(sub));

                // Replace the function with its value in the equation
                equation = equation.Replace(f.Key + '(' + sub + ')', $"({result})");
            }
        }

        return equation;
    }

    /// <summary>
    /// Replaces the constants in an equation with their values
    /// </summary>
    /// <param name="equation">The equation to replace constants in</param>
    /// <returns>The provided equation where the constants have been replaced</returns>
    private string ReplaceConstants(string equation)
    {
        // Maps the constant's symbols to its value
        var constants = new Dictionary<string, double>();
        Array.ForEach(Constants, c =>
            Array.ForEach(c.Symbols, s => constants.Add(s.ToLower(), c.Value)));

        // Order the dictionary by key length, replace all the keys in the equation with the values
        constants.OrderByDescending(e => e.Key.Length)
                 .ToList()
                 .ForEach(e => equation = equation.Replace(e.Key.ToLower(), $"({e.Value})"));

        return equation;
    }

    /// <summary>
    /// Inserts multiplication signs in the correct places
    /// </summary>
    /// <param name="equation">The equation to fix the multiplication signs in</param>
    /// <returns>The provided equation with multiplication signs inserted at the right places</returns>
    private string AddMultiplicationSigns(string equation)
    {
        var eq = new List<char>(equation);
        var operators = string.Join("", from o in Operators select o.Symbol);

        for (var i = 1; i < eq.Count; i++)
        {
            // Matches x( where x not in operators
            if (eq[i] == '(' && !operators.Contains(eq[i - 1]) && eq[i - 1] != '(')
            {
                eq.Insert(i, '*');
            }
            // Matches )x where x not in operators
            else if (eq[i - 1] == ')' && !operators.Contains(eq[i]) && eq[i] != ')')
            {
                eq.Insert(i, '*');
            }
        }

        return string.Join("", eq);
    }
}
