using System.Text;

namespace BLM16.Util.Calculator.Validators;

/// <summary>
/// Validates that a value falls between a min and max value.
/// </summary>
public class RangeValidator : IFunctionValidator
{
	/// <summary>
	/// Minimum accepted value. Defaults to <see cref="double.NegativeInfinity"/>.
	/// </summary>
	/// <remarks>
	/// <see cref="MinInclusive"/> defaults to true.
	/// </remarks>
	public double Min { get; init; } = double.NegativeInfinity;
	/// <summary>
	/// Maximum accepted value. Defaults to <see cref="double.PositiveInfinity"/>.
	/// </summary>
	/// <remarks>
	/// <see cref="MaxInclusive"/> defaults to true.
	/// </remarks>
	public double Max { get; init; } = double.PositiveInfinity;

	public bool MinInclusive { get; init; } = true;
	public bool MaxInclusive { get; init; } = true;

	public void ValidateOrThrow(double val, string functionName)
	{
		if (val < Min || (!MinInclusive && val == Min) ||
			val > Max || (!MaxInclusive && val == Max))
		{
			var range = new StringBuilder()
				.Append((MinInclusive || Min != double.NegativeInfinity) ? '[' : '(')
				.Append(Min).Append(", ").Append(Max)
				.Append((MaxInclusive && Max != double.PositiveInfinity) ? ']' : ')');

			throw new MathSyntaxException($"{functionName} requires the value to be in the range {range} but got {val}.");
		}
	}
}
