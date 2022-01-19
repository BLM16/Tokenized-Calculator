using System;

namespace BLM16.Util.Calculator.Models;

/// <summary>
/// A function used by the calculator
/// </summary>
public struct Function
{
    /// <summary>
    /// The symbols that represent the function
    /// </summary>
    public string[] Symbols { get; private set; }
    /// <summary>
    /// The operation the function performs
    /// </summary>
    public Func<double, string> Operation { get; private set; }

    public Function(Func<double, string> func, params string[] symbols)
    {
        Symbols = symbols;
        Operation = func;
    }
}

/// <summary>
/// Contains all the default functions built into the Calculator
/// </summary>
public static class DefaultFunctions
{
    /// <summary>
    /// The absolute function
    /// </summary>
    public static Function Abs
        => new((double val) => Math.Abs(val).ToString(), "abs");

    /// <summary>
    /// The square root function
    /// </summary>
    public static Function Sqrt
        => new((double val) => Math.Sqrt(val).ToString(), "sqrt", "root", "√");

    /// <summary>
    /// The cubed root function
    /// </summary>
    public static Function Cbrt
        => new((double val) => Math.Cbrt(val).ToString(), "cbrt", "sqrt3", "root3");

    /// <summary>
    /// The sine function with a parameter in radians
    /// </summary>
    public static Function Sin
        => new((double val) => Math.Sin(val).ToString(), "sin");

    /// <summary>
    /// The cosine function with a parameter in radians
    /// </summary>
    public static Function Cos
        => new((double val) => Math.Cos(val).ToString(), "cos");

    /// <summary>
    /// The tangent function with a parameter in radians
    /// </summary>
    public static Function Tan
        => new((double val) => Math.Tan(val).ToString(), "tan");

    /// <summary>
    /// The inverse sine function
    /// </summary>
    public static Function Asin
        => new((double val) => Math.Asin(val).ToString(), "asin", "arcsin", "sin^-1", "sin^(-1)");

    /// <summary>
    /// The inverse cosine function
    /// </summary>
    public static Function Acos
        => new((double val) => Math.Acos(val).ToString(), "acos", "arccos", "cos^-1", "cos^(-1)");

    /// <summary>
    /// The inverse tangent function
    /// </summary>
    public static Function Atan
        => new((double val) => Math.Atan(val).ToString(), "atan", "arctan", "tan^-1", "tan^(-1)");

    /// <summary>
    /// The hyperbolic sine function
    /// </summary>
    public static Function Sinh
        => new((double val) => Math.Sinh(val).ToString(), "sinh");

    /// <summary>
    /// The hyperbolic cosine function
    /// </summary>
    public static Function Cosh
        => new((double val) => Math.Cosh(val).ToString(), "cosh");

    /// <summary>
    /// The hyperbolic tangent function
    /// </summary>
    public static Function Tanh
        => new((double val) => Math.Tanh(val).ToString(), "tanh");

    /// <summary>
    /// The inverse hyperbolic sine function
    /// </summary>
    public static Function Asinh
        => new((double val) => Math.Asinh(val).ToString(), "asinh", "arcsinh", "sinh^-1", "sinh^(-1)");

    /// <summary>
    /// The inverse hyperbolic cosine function
    /// </summary>
    public static Function Acosh
        => new((double val) => Math.Acosh(val).ToString(), "acosh", "arccosh", "cosh^-1", "cosh^(-1)");

    /// <summary>
    /// The inverse hyperbolic tangent function
    /// </summary>
    public static Function Atanh
        => new((double val) => Math.Atanh(val).ToString(), "atanh", "arctanh", "tanh^-1", "tanh^(-1)");

    /// <summary>
    /// The ceiling function
    /// </summary>
    public static Function Ceil
        => new((double val) => Math.Ceiling(val).ToString(), "ceil");

    /// <summary>
    /// The floor function
    /// </summary>
    public static Function Floor
        => new((double val) => Math.Floor(val).ToString(), "floor");

    /// <summary>
    /// The sign function
    /// </summary>
    public static Function Sign
        => new((double val) => Math.Sign(val).ToString(), "sign", "sgn");

    /// <summary>
    /// The logarithm function of base e
    /// </summary>
    public static Function Log
        => new((double val) => Math.Log(val).ToString(), "loge", "log_e", "ln");

    /// <summary>
    /// The logarithm function of base 2
    /// </summary>
    public static Function Log2
        => new((double val) => Math.Log2(val).ToString(), "log2", "log_2");

    /// <summary>
    /// The logarithm function of base 10
    /// </summary>
    public static Function Log10
        => new((double val) => Math.Log10(val).ToString(), "log10", "log_10", "log");

    /// <summary>
    /// The degree function converts radians to degrees
    /// </summary>
    public static Function Deg
        => new((double val) => (val * 180 / Math.PI).ToString(), "deg");

    /// <summary>
    /// The radian function converts degrees to radians
    /// </summary>
    public static Function Rad
        => new((double val) => (val * Math.PI / 180).ToString(), "rad");
}
