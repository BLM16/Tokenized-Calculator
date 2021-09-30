using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public enum TokenType : ushort
    {
        PLUS,
        MINUS,
        MULT,
        DIV,
        SQRT,
        PI,
        LBRACK,
        RBRACK
    }

    public class Token
    {
        public readonly TokenType Type;
        public readonly string Value;
        
        Token(TokenType type, string value)
        {
            this.Type = type;
            this.Value = value;
        }
    }
}
