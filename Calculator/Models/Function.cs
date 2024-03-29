﻿using BLM16.Util.Calculator.Validators;
using System;
using System.Linq;

namespace BLM16.Util.Calculator.Models;

/// <summary>
/// A function used by the calculator
/// </summary>
public struct Function
{
	/// <summary>
	/// The validator to ensure the value passed to the operation is valid.
	/// </summary>
	private readonly IFunctionValidator _validator;
	private readonly Func<double, string> _operation;

	/// <summary>
	/// The symbols that represent the function
	/// </summary>
	public string[] Symbols { get; private set; }

	/// <summary>
	/// The operation the function performs.
	/// When invoked, the operation always calls the validator first like so:
	/// <code>
	/// get
	/// {
	///   return arg =>
	///   {
	///     ValidateOrThrow(arg);
	///     return Operation(arg);
	///   }
	/// }
	/// </code>
	/// </summary>
	public readonly Func<double, string> Operation
	{
		get
		{
			var @this = this; // Required to pass `this` into the lambda function
			return arg =>
			{
				@this._validator?.ValidateOrThrow(arg, @this.Symbols[0]);
				return @this._operation(arg);
			};
		}
	}

	public Function(Func<double, string> func, string primarySymbol, params string[] otherSymbols)
	{
		Symbols = new [] { primarySymbol }.Concat(otherSymbols).ToArray();
		_validator = null;
		_operation = func;
	}

	public Function(Func<double, string> func, IFunctionValidator validator, string primarySymbol, params string[] otherSymbols)
	{
		Symbols = new[] { primarySymbol }.Concat(otherSymbols).ToArray();
		_validator = validator;
		_operation = func;
	}
}

/// <summary>
/// Contains all the default functions built into the Calculator
/// </summary>
public static class DefaultFunctions
{
	/// <summary>
	/// Formats doubles after functions have been evaluated
	/// </summary>
	/// <remarks>
	/// This should be lossless for the datatype, remove trailing zeros,
	/// and avoid scientific notation which interferes in calculations when
	/// E is treated as a constant and not as scientific notation.
	/// </remarks>
	/// <returns>The double as a lossless string without scientific notation or trailing zeros</returns>
	private static string FormatAsString(this double d)
		=> d.ToString("F339").TrimEnd('0').TrimEnd('.');

	/// <summary>
	/// The absolute function
	/// </summary>
	public static Function Abs
		=> new((double val) => Math.Abs(val).FormatAsString(), "abs");

	/// <summary>
	/// The square root function
	/// </summary>
	public static Function Sqrt
		=> new((double val) => Math.Sqrt(val).FormatAsString(),
			   new RangeValidator { Min = 0 },
			   "sqrt", "root", "√");

	/// <summary>
	/// The cubed root function
	/// </summary>
	public static Function Cbrt
		=> new((double val) => Math.Cbrt(val).FormatAsString(), "cbrt", "sqrt3", "root3");

	/// <summary>
	/// The sine function with a parameter in radians
	/// </summary>
	public static Function Sin
		=> new((double val) => Math.Sin(val).FormatAsString(), "sin");

	/// <summary>
	/// The cosine function with a parameter in radians
	/// </summary>
	public static Function Cos
		=> new((double val) => Math.Cos(val).FormatAsString(), "cos");

	/// <summary>
	/// The tangent function with a parameter in radians
	/// </summary>
	public static Function Tan
		=> new((double val) => Math.Tan(val).FormatAsString(), "tan");

	/// <summary>
	/// The inverse sine function
	/// </summary>
	public static Function Asin
		=> new((double val) => Math.Asin(val).FormatAsString(),
			   new RangeValidator { Min = -1, Max = 1 },
			   "asin", "arcsin", "sin^-1", "sin^(-1)");

	/// <summary>
	/// The inverse cosine function
	/// </summary>
	public static Function Acos
		=> new((double val) => Math.Acos(val).FormatAsString(),
			   new RangeValidator { Min= -1, Max = 1 },
			   "acos", "arccos", "cos^-1", "cos^(-1)");

	/// <summary>
	/// The inverse tangent function
	/// </summary>
	public static Function Atan
		=> new((double val) => Math.Atan(val).FormatAsString(), "atan", "arctan", "tan^-1", "tan^(-1)");

	/// <summary>
	/// The hyperbolic sine function
	/// </summary>
	public static Function Sinh
		=> new((double val) => Math.Sinh(val).FormatAsString(), "sinh");

	/// <summary>
	/// The hyperbolic cosine function
	/// </summary>
	public static Function Cosh
		=> new((double val) => Math.Cosh(val).FormatAsString(), "cosh");

	/// <summary>
	/// The hyperbolic tangent function
	/// </summary>
	public static Function Tanh
		=> new((double val) => Math.Tanh(val).FormatAsString(), "tanh");

	/// <summary>
	/// The inverse hyperbolic sine function
	/// </summary>
	public static Function Asinh
		=> new((double val) => Math.Asinh(val).FormatAsString(), "asinh", "arcsinh", "sinh^-1", "sinh^(-1)");

	/// <summary>
	/// The inverse hyperbolic cosine function
	/// </summary>
	public static Function Acosh
		=> new((double val) => Math.Acosh(val).FormatAsString(),
			   new RangeValidator { Min = 1 },
			   "acosh", "arccosh", "cosh^-1", "cosh^(-1)");

	/// <summary>
	/// The inverse hyperbolic tangent function
	/// </summary>
	public static Function Atanh
		=> new((double val) => Math.Atanh(val).FormatAsString(),
			   new RangeValidator { Min = -1, Max = 1, MinInclusive = false, MaxInclusive = false },
			   "atanh", "arctanh", "tanh^-1", "tanh^(-1)");

	/// <summary>
	/// The ceiling function
	/// </summary>
	public static Function Ceil
		=> new((double val) => Math.Ceiling(val).FormatAsString(), "ceil");

	/// <summary>
	/// The floor function
	/// </summary>
	public static Function Floor
		=> new((double val) => Math.Floor(val).FormatAsString(), "floor");

	/// <summary>
	/// The sign function
	/// </summary>
	public static Function Sign
		=> new((double val) => Math.Sign(val).ToString(), "sign", "sgn");

	/// <summary>
	/// The logarithm function of base e
	/// </summary>
	public static Function Log
		=> new((double val) => Math.Log(val).FormatAsString(),
			   new RangeValidator { Min = 0, MinInclusive = false },
			   "loge", "log_e", "ln");

	/// <summary>
	/// The logarithm function of base 2
	/// </summary>
	public static Function Log2
		=> new((double val) => Math.Log2(val).FormatAsString(),
			   new RangeValidator { Min = 0, MinInclusive = false },
			   "log2", "log_2");

	/// <summary>
	/// The logarithm function of base 10
	/// </summary>
	public static Function Log10
		=> new((double val) => Math.Log10(val).FormatAsString(),
			   new RangeValidator { Min = 0, MinInclusive = false },
			   "log10", "log_10", "log");

	/// <summary>
	/// The degree function converts radians to degrees
	/// </summary>
	public static Function Deg
		=> new((double val) => (val * 180 / Math.PI).FormatAsString(), "deg");

	/// <summary>
	/// The radian function converts degrees to radians
	/// </summary>
	public static Function Rad
		=> new((double val) => (val * Math.PI / 180).FormatAsString(), "rad");
}
