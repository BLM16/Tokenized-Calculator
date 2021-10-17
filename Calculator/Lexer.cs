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
                switch (c)
                {
                    case '+':
                        tokens.Add(new Token(TokenType.PLUS, c));
                        break;

                    case '-':
                        tokens.Add(new Token(TokenType.MINUS, c));
                        break;

                    case '*':
                        tokens.Add(new Token(TokenType.MULT, c));
                        break;

                    case '/':
                        tokens.Add(new Token(TokenType.DIV, c));
                        break;

                    case '(':
                        tokens.Add(new Token(TokenType.LBRACK, c));
                        break;

                    case ')':
                        tokens.Add(new Token(TokenType.RBRACK, c));
                        break;

                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '0':
                    case '.':
                        // Append the current number value to the previous token if it is a number too
                        // Create a new token otherwise
                        var t = tokens.LastOrDefault();
                        if (t == null || t.Type != TokenType.NUMBER)
                        {
                            tokens.Add(new Token(TokenType.NUMBER, c));
                        }
                        else
                        {
                            t.Value += c;
                        }
                        break;

                    default:
                        // TODO: Handle non-builtins here
                        throw new MathSyntaxException($"Unexpected token: {c}");
                        break;
                }
            }
            
            return tokens;
        }
    }
}
