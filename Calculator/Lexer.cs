using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
    /// <summary>
    /// Has the logic to tokenize an equation
    /// </summary>
    internal class Lexer
    {
        /// <summary>
        /// The list of operators to parse
        /// </summary>
        private readonly Operator[] Operators;

        public Lexer(Operator[] operators)
        {
            Operators = operators;
        }

        /// <summary>
        /// Tokenizes an equation
        /// </summary>
        /// <param name="equation">The equation to tokenize</param>
        /// <returns>A list of tokens</returns>
        public List<Token> Parse(string equation)
        {
            var tokens = new List<Token>();

            // Loop through each char and convert to tokens
            foreach (char c in equation)
            {
                if ("0123456789.".Contains(c))
                {
                    var t = tokens.LastOrDefault();
                    if (t == null || t.Type != TokenType.NUMBER)
                    {
                        tokens.Add(new Token(TokenType.NUMBER, c));
                    }
                    else
                    {
                        // The current and previous values are numbers so append the current to the previous
                        t.Value = t.Value.ToString() + c;
                    }
                }
                else if (c == '(')
                {
                    tokens.Add(new Token(TokenType.LBRACK, '('));
                }
                else if (c == ')')
                {
                    tokens.Add(new Token(TokenType.RBRACK, ')'));
                }
                else
                {
                    // Get the operator from the list of operators
                    var op = from o in Operators
                             where o.Symbol == c.ToString()
                             select o;

                    if (op.Any())
                    {
                        tokens.Add(new Token(TokenType.OPERATOR, op.First()));
                    }
                    else
                    {
                        // If the operator doesn't exist throw an error
                        throw new MathSyntaxException($"Unrecognized operator: {c}");
                    }
                }
            }

            return tokens;
        }
    }
}
