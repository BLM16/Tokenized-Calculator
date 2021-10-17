using System.Collections.Generic;

namespace Calculator
{
    /// <summary>
    /// Builtin token types
    /// </summary>
    public enum TokenType : ushort
    {
        NUMBER,
        PLUS,
        MINUS,
        MULT,
        DIV,
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
        public string Value;

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public Token(TokenType type, char value) : this(type, value.ToString())
        { }

        #region Overrides

        public override bool Equals(object obj) => obj is Token token
                                                   && Type == token.Type
                                                   && Value == token.Value;

        public override int GetHashCode()
        {
            int hashCode = 1265339359;
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            return hashCode;
        }

        #endregion
    }
}
