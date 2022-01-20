using System;

namespace BLM16.Util.Calculator.Models;

/// <summary>
/// Builtin token types
/// </summary>
public enum TokenType : ushort
{
    NUMBER,
    OPERATOR,
    LBRACK,
    RBRACK
}

/// <summary>
/// Represents a mathematical symbol or number with a type and value
/// </summary>
public struct Token
{
    /// <summary>
    /// The type of token
    /// </summary>
    public readonly TokenType Type;
    /// <summary>
    /// The value of the token if it is a string
    /// </summary>
    public string StrVal { get; set; } = string.Empty;
    /// <summary>
    /// The value of the token if it is an operator
    /// </summary>
    public Operator OpVal { get; set; } = default;

    public Token(TokenType type, string value)
    {
        Type = type;
        StrVal = value;
    }

    public Token(TokenType type, char value) : this(type, value.ToString()) { }

    public Token(TokenType type, Operator value)
    {
        Type = type;
        OpVal = value;
    }

    #region Overrides

    public override bool Equals(object obj)
    {
        if (obj is not Token)
        {
            return false;
        }

        var token = (Token)obj;
        bool areValsEqual;

        if (Type == TokenType.OPERATOR)
        {
            areValsEqual = OpVal.Equals(token.OpVal);
        }
        else
        {
            areValsEqual = StrVal.Equals(token.StrVal);
        }

        return Type == token.Type && areValsEqual;
    }

    public override int GetHashCode() => HashCode.Combine(Type, StrVal, OpVal);

    public static bool operator ==(Token left, Token right) => left.Equals(right);

    public static bool operator !=(Token left, Token right) => !left.Equals(right);

    #endregion
}
