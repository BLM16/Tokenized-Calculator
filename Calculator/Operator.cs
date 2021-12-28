using System;
using System.Collections.Generic;

namespace BLM16.Util.Calculator;

/// <summary>
/// An operator used by the calculator
/// </summary>
public class Operator
{
    /// <summary>
    /// The symbol representing the operator
    /// </summary>
    public char Symbol { get; private set; }
    /// <summary>
    /// The order of precedence of the operator
    /// </summary>
    private int Order { get; set; }
    /// <summary>
    /// The function the operator performs
    /// </summary>
    private Delegate Operation { get; set; }

    public Operator(char op, int order, Func<double, double, double> operation)
    {
        Symbol = op;
        Order = order;
        Operation = operation;
    }

    /// <summary>
    /// Applies the operator's operation function to the provided values
    /// </summary>
    /// <param name="num1">The LHS of the operation</param>
    /// <param name="num2">The RHS of the operation</param>
    /// <returns>The result of the operation</returns>
    public double Evaluate(double num1, double num2) => (double)Operation.DynamicInvoke(num1, num2);

    #region Overrides

    /// <summary>
    /// Compares the operator orders to determine precedence
    /// </summary>
    public static bool operator >(Operator left, Operator right) => left.Order > right.Order;
    /// <summary>
    /// Compares the operator orders to determine precedence
    /// </summary>
    public static bool operator <(Operator left, Operator right) => left.Order < right.Order;

    public override bool Equals(object obj) => obj is Operator @operator
                                                && Symbol == @operator.Symbol
                                                && Order == @operator.Order
                                                && EqualityComparer<Delegate>.Default.Equals(Operation, @operator.Operation);

    public override int GetHashCode() => HashCode.Combine(Symbol, Order, Operation);

    #endregion
}

/// <summary>
/// Contains the default operators built into the Calculator
/// </summary>
public static class DefaultOperators
{
    /// <summary>
    /// The default addition operator
    /// </summary>
    public static Operator Addition
        => new('+', 1, (double num1, double num2) => num1 + num2);

    /// <summary>
    /// The default subtraction operator
    /// </summary>
    public static Operator Subtraction
        => new('-', 1, (double num1, double num2) => num1 - num2);

    /// <summary>
    /// The default multiplication operator
    /// </summary>
    public static Operator Multiplication
        => new('*', 2, (double num1, double num2) => num1 * num2);

    /// <summary>
    /// The default division operator
    /// </summary>
    public static Operator Division
        => new('/', 2, (double num1, double num2) => num1 / num2);

    /// <summary>
    /// The default exponent operator
    /// </summary>
    public static Operator Exponent
        => new('^', 3, (double num1, double exponent) => Math.Pow(num1, exponent));
}
