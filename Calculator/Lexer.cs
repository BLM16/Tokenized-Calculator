using System.Collections.Generic;
using System.Linq;

namespace BLM16.Util.Calculator;

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
    /// <exception cref="MathSyntaxException">Thrown when malformed math is provided or an operator is unrecognized.</exception>
    /// <returns>A list of tokens</returns>
    public List<Token> Parse(string equation)
    {
        var tokens = new List<Token>();

        // Loop through each char and convert to tokens
        for (int i = 0; i < equation.Length; i++)
        {
            var c = equation[i];
            var isLast = i + 1 == equation.Length;

            if ("0123456789.".Contains(c))
            {
                var t = tokens.LastOrDefault();
                if (t == null || t.Type != TokenType.NUMBER)
                {
                    // Prepend a zero if the number starts with a decimal point
                    object value = c == '.' ? $"0{c}" : c;
                    if (c == '.' && (isLast || !"0123456789".Contains(equation[i + 1])))
                    {
                        throw new MathSyntaxException("Malformed double: digits must trail the decimal point");
                    }
                    tokens.Add(new Token(TokenType.NUMBER, value));
                }
                else
                {
                    var cur = t.Value.ToString();
                    if (c == '.' && cur.Contains('.'))
                    {
                        throw new MathSyntaxException($"Malformed double: cannot contain more than one decimal point");
                    }
                    else if (c == '.' && (isLast || !"0123456789".Contains(equation[i + 1])))
                    {
                        throw new MathSyntaxException("Malformed double: digits must trail the decimal point");
                    }
                    else
                    {
                        // The current and previous values are numbers so append the current to the previous
                        t.Value = cur + c;
                    }
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
                if (equation.Length == 1)
                {
                    throw new MathSyntaxException("Malformed expression: operator requires numbers to operate on");
                }
                if (tokens.LastOrDefault().Type == TokenType.OPERATOR)
                {
                    throw new MathSyntaxException("Consecutive operators found");
                }
                    
                // Get the operator from the list of operators
                var op = from o in Operators
                            where o.Symbol == c
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
