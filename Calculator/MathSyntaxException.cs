using System;

namespace BLM16.Util.Calculator;

/// <summary>
/// Exception for malformed equations
/// </summary>
public class MathSyntaxException : ApplicationException
{
    /// <summary>
    /// Exception constructor with a message
    /// </summary>
    /// <param name="msg">The message to display with the exception</param>
    public MathSyntaxException(string msg) : base(msg) { }
    /// <summary>
    /// Exception constructor with a message and an inner exception
    /// </summary>
    /// <param name="msg">The message to display with the exception</param>
    /// <param name="e">The inner exception stack trace</param>
    public MathSyntaxException(string msg, Exception e) : base(msg, e) { }
}
