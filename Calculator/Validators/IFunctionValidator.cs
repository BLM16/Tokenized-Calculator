namespace BLM16.Util.Calculator.Validators;

/// <summary>
/// Represents a validator that ensures math functions are not passed any invalid values.
/// </summary>
public interface IFunctionValidator
{
	/// <summary>
	/// Throws if validation fails. If this does not throw, the value is valid.
	/// </summary>
	/// <param name="val">The value to validate.</param>
	/// <param name="functionName">The primary symbol of the function that is being validated.</param>
	public void ValidateOrThrow(double val, string functionName);
}
