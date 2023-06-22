using BLM16.Util.Calculator.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BLM16.Util.Calculator.Tests;

/// <summary>
/// Contains the integration tests for the Calculator
/// </summary>
[TestClass]
public class CalculatorTests
{
    #region With Defaults

    private static readonly Calculator Calculator_WithDefaults = new();

    /// <summary>
    /// Checks that the <see cref="Calculator"/> with the default symbols correctly computes equations
    /// </summary>
    /// <param name="equation">The equation to calculate</param>
    /// <param name="expected">The expected result</param>
    [DataTestMethod]
    [DataRow("31 + 7 * 5 - 9 / 4 ^ 2", 65.4375)]
    [DataRow("14.6 * 9.4 + 12.1 - 3 ^ 5 / 4", 88.59)]
    [DataRow("19 + 4 * (3 + 6)^2", 343)]
    [DataRow("-6 * (-3) + 14 / 7", 20)]
    [DataRow("9e + 6pi * 3", 81.01320422074768)]
    [DataRow("sqrt(64) * 4 - 12", 20)]
    [DataRow("cbrt(8)deg(asin(1))", 180)]
    [DataRow("sqrt(log(10^(2 + 4))^2 / 4)", 3)]
    public void Calculator_CalculatesEquation_WithDefaults(string equation, double expected)
        => Assert.AreEqual(expected, Calculator_WithDefaults.Calculate(equation));

    #endregion

    #region With Customs

    private const double phi = 1.61803399;

	private static readonly Operator _sumOverRemOperator = new('$', 20, (num1, num2) => (num1 + num2)/(num1 - num2));
    private static readonly Constant _goldenRatio = new(phi, "phi", "φ");
    private static readonly Function _halfSquare = new((double val) => (Math.Pow(val, 2) / 2).ToString("F339").TrimEnd('0').TrimEnd('.'), "hlfsq");


	private static readonly Calculator CustomCalculator = new()
    {
        Operators = new[] { DefaultOperators.Modulus, _sumOverRemOperator },
        Constants = new[] { _goldenRatio },
        Functions = new[] { DefaultFunctions.Sqrt, _halfSquare }
    };

    /// <summary>
    /// Checks that the <see cref="Calculator"/> with custom operators correctly computes equations
    /// </summary>
    /// <param name="equation">The equation to calculate</param>
    /// <param name="expected">The expected result</param>
    [DataTestMethod]
    [DataRow("24 % 2", 0)]
    [DataRow("3 $ 5", -4)]
    [DataRow("(4(-3 + -6 * 2) % 7) $ 2", 5)]
    public void Calculator_CalculatesEquation_WithCustomOperators(string equation, double expected)
        => Assert.AreEqual(expected, CustomCalculator.Calculate(equation));

    /// <summary>
    /// Checks that the <see cref="Calculator"/> with custom constants correctly computes equations
    /// </summary>
    /// <param name="equation">The equation to calculate</param>
    /// <param name="expected">The expected result</param>
    [DataTestMethod]
    [DataRow("3phi", 3 * phi)]
    [DataRow("φ^2", phi * phi)]
    [DataRow("phi(2 + φ)", phi * (2 + phi))]
    public void Calculator_CalculatesEquation_WithCustomConstants(string equation, double expected)
        => Assert.AreEqual(expected, CustomCalculator.Calculate(equation));

    /// <summary>
    /// Checks that the <see cref="Calculator"/> with custom functions correctly computes equations
    /// </summary>
    /// <param name="equation"></param>
    /// <param name="expected"></param>
    [DataTestMethod]
    [DataRow("hlfsq(4)", 8)]
    [DataRow("3*hlfsq(19 - 7)", 216)]
    [DataRow("sqrt(2 * hlfsq(99))", 99)]
    public void Calculator_CalculatesEquation_WithCustomFunctions(string equation, double expected)
        => Assert.AreEqual(expected, CustomCalculator.Calculate(equation));

    /// <summary>
    /// Checks that the <see cref="Calculator"/> with multiple customs correctly computes equations
    /// </summary>
    /// <param name="equation"></param>
    /// <param name="expected"></param>
    [DataTestMethod]
    [DataRow("hlfsq(φ)", phi * phi / 2)]
    [DataRow("hlfsq(6) $ 5", 23.0 / 13.0)]
    public void Calculator_CalculatesEquation_WithCustomAllSymbols(string equation, double expected)
        => Assert.AreEqual(expected, CustomCalculator.Calculate(equation));

    #endregion
}
