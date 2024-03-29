﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLM16.Util.Calculator.Tests;

/// <summary>
/// Contains the tests for the Standardizer class
/// </summary>
[TestClass]
public class StandardizerTests
{
    private readonly Standardizer standardizer = new(Calculator.BuiltinOperatorList, Calculator.DefaultConstantList, Calculator.DefaultFunctionList);

    /// <summary>
    /// Checks that whitespace is correctly removed from the equations
    /// </summary>
    /// <param name="equation">The equation to standardize</param>
    /// <param name="expected">The expected output</param>
    [DataTestMethod]
    [DataRow("35 + 7 * 9/3", "35+7*9/3")]
    [DataRow("(267 + 35) * 34 /2 + 2", "(267+35)*34/2+2")]
    [DataRow("(12 +7)*(19 - 4) + 6*(7 / 4)", "(12+7)*(19-4)+6*(7/4)")]
    public void RemoveWhitespace_RemovesSpaces(string equation, string expected)
        => Assert.AreEqual(expected, standardizer.Standardize(equation));

    #region FixBrackets.Tests

    /// <summary>
    /// Checks that brackets are inserted correctly to standardize equations
    /// </summary>
    /// <param name="equation">The equation to standardize</param>
    /// <param name="expected">The expected output</param>
    [DataTestMethod]
    [DataRow("7*(35+2", "7*(35+2)")]
    [DataRow("(63+7)*3*(35+(35-2", "(63+7)*3*(35+(35-2))")]
    [DataRow("(((35", "(((35)))")]
    public void FixBrackets_FixesBrackets(string equation, string expected)
        => Assert.AreEqual(expected, standardizer.Standardize(equation));

    /// <summary>
    /// Checks that an exception is thrown with too many closing brackets
    /// </summary>
    /// <param name="equation">The equation to standardize</param>
    [DataTestMethod]
    [DataRow("35+4)")]
    [DataRow("37*(16*(14+13/2)))")]
    [ExpectedException(typeof(MathSyntaxException))]
    public void FixBrackets_ExceptionOnTooManyClosingBrackets(string equation)
        => standardizer.Standardize(equation);

    #endregion

    /// <summary>
    /// Checks that repeating operators are simplified where possible
    /// </summary>
    /// <param name="equation">The equation to standardize</param>
    /// <param name="expected">The expected output</param>
    [DataTestMethod]
    [DataRow("3+-5", "3-5")]
    [DataRow("22--5", "22+5")]
    public void FixRepeatingOperators(string equation, string expected)
        => Assert.AreEqual(expected, standardizer.Standardize(equation));

    /// <summary>
    /// Checks that functions are correctly evaluated
    /// </summary>
    /// <param name="equation">The equation to standardize</param>
    /// <param name="expected">The expected output</param>
    [DataTestMethod]
    [DataRow("sqrt(64)", "(8)")]
    [DataRow("34log(100)", "34*(2)")]
    public void ComputeFunctions_ComputesFunctions(string equation, string expected)
        => Assert.AreEqual(expected, standardizer.Standardize(equation));

    /// <summary>
    /// Checks that constants are correctly replaced by their values
    /// </summary>
    /// <param name="equation">The equation to standardize</param>
    /// <param name="expected">The expected output</param>
    [DataTestMethod]
    [DataRow("pi", "(3.141592653589793)")]
    [DataRow("e", "(2.718281828459045)")]
    [DataRow("eπ(13+2^e)/pi", "(2.718281828459045)*(3.141592653589793)*(13+2^(2.718281828459045))/(3.141592653589793)")]
    public void ReplaceConstants_ReplacesConstants(string equation, string expected)
        => Assert.AreEqual(expected, standardizer.Standardize(equation));

    /// <summary>
    /// Checks that multiplication signs are inserted correctly around brackets
    /// </summary>
    /// <param name="equation">The equation to standardize</param>
    /// <param name="expected">The expected output</param>
    [DataTestMethod]
    [DataRow("35(14+2)", "35*(14+2)")]
    [DataRow("(14+2)35", "(14+2)*35")]
    [DataRow("-(14/2)(13+9)", "-(14/2)*(13+9)")]
    [DataRow("123+(5*7)/2", "123+(5*7)/2")]
    public void AddMultiplicationSigns_AddsMultiplicationSigns(string equation, string expected)
        => Assert.AreEqual(expected, standardizer.Standardize(equation));
}
