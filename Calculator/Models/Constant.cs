using System;
using System.Collections.Generic;
using System.Linq;

namespace BLM16.Util.Calculator.Models;

/// <summary>
/// A constant used by the calculator
/// </summary>
public struct Constant
{
    /// <summary>
    /// The value of the constant
    /// </summary>
    public double Value { get; private set; }
    /// <summary>
    /// The symbols that represent the constant
    /// </summary>
    public string[] Symbols { get; private set; }

    /// <exception cref="ArgumentException">Thrown when null values are used as symbols</exception>
    public Constant(double value, params string[] symbols)
    {
        if (symbols.Length == 0 || symbols.Where(s => s == "").Any())
        {
            throw new ArgumentException("Constant symbols must not be null");
        }

        Symbols = symbols;
        Value = value;
    }
}

/// <summary>
/// Contains all the default constants built into the Calculator
/// </summary>
public static class DefaultConstants
{
    /// <summary>
    /// The constant PI
    /// </summary>
    public static Constant PI
        => new(Math.PI, "pi", "π");

    /// <summary>
    /// Euler's number
    /// </summary>
    public static Constant E
        => new(Math.E, "e");

    /// <summary>
    /// The golden ratio
    /// </summary>
    public static Constant Phi
        => new(1.6180339887498948, "phi", "φ");
}
