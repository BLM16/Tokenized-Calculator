using System;

namespace Calculator
{
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
    public class Token
    {
        /// <summary>
        /// The type of token
        /// </summary>
        public readonly TokenType Type;
        /// <summary>
        /// The value of the token
        /// </summary>
        public object Value;

        public Token(TokenType type, object value)
        {
            Type = type;
            Value = value;
        }

        #region Overrides

        public override bool Equals(object obj) => obj is Token @token
                                                   && Type == @token.Type
                                                   && Value.Equals(@token.Value);

        public override int GetHashCode() => HashCode.Combine(Type, Value);

        #endregion
    }
}
