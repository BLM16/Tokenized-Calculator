using BLM16.Util.Calculator.Models;
using System.Collections.Generic;

namespace BLM16.Util.Calculator;

/// <summary>
/// Has the logic to evaluate an equation
/// </summary>
internal class Parser
{
    /// <summary>
    /// The stack of values that still need to be evaluated
    /// </summary>
    private readonly Stack<double> values = new();
    /// <summary>
    /// The stack of operators that need to be applied to the values
    /// </summary>
    private readonly Stack<Operator> operators = new();

    /// <summary>
    /// The default left bracket operator.
    /// This is only to be used as reference by the parser and should never be evaluated.
    /// The order of precedence is 0 so all operators are pushed after it instead of evaluated.
    /// </summary>
    private static Operator LeftBracket
        => new('(', 0, (double num1, double num2) => { return double.NaN; });

    /// <summary>
    /// Evaluates the result of a tokenized mathematical equation
    /// </summary>
    /// <param name="tokens">The list of tokens parsed from the equation</param>
    /// <returns>The result of the calculations</returns>
    public double Evaluate(List<Token> tokens)
    {
        // Operators will be evaluated if they have precedence
        // Operators and numbers will be pushed to the stack without precedence
        for (int i = 0; i < tokens.Count; i++)
        {
            Token t = tokens[i];
            switch (t.Type)
            {
                case TokenType.NUMBER:
                    values.Push(double.Parse(t.StrVal));
                    break;

                case TokenType.OPERATOR:
                    // Handle operators without values on both sides (non binary operations)
                    if (i == 0 || i == tokens.Count - 1 || tokens[i - 1].Type is TokenType.OPERATOR or TokenType.LBRACK ||
                         tokens[i + 1].Type == TokenType.RBRACK)
                    {
                        throw new MathSyntaxException("Operators must have values on both sides");
                    }

                    // Push the operator to the stack if there are no operators as precedence cannot be determined
                    if (operators.Count == 0)
                    {
                        operators.Push(t.OpVal);
                    }
                    // Push the operator to the stack if the previous operator has a lower precedence
                    else if (t.OpVal > operators.Peek())
                    {
                        operators.Push(t.OpVal);
                    }
                    else
                    {
                        // Continuously evaluate the equation from the stacks while it has precedence
                        while (operators.Count > 0 && !(t.OpVal > operators.Peek()))
                        {
                            var a = values.Pop();
                            var b = values.Pop();
                            var o = operators.Pop();

                            values.Push(o.Evaluate(b, a));
                        }
                        // Push the current operator to the stack
                        operators.Push(t.OpVal);
                    }
                    break;

                case TokenType.LBRACK:
                    // This token is simply pushed as it only provides an endpoint to RBRACK tokens
                    operators.Push(LeftBracket);
                    break;

                case TokenType.RBRACK:
                    // Evaluate all the math inside the brackets
                    // Previous precedence operations will ensure this will always follow bedmas
                    while (operators.Peek().Symbol != '(')
                    {
                        var a = values.Pop();
                        var b = values.Pop();
                        var o = operators.Pop();

                        values.Push(o.Evaluate(b, a));
                    }

                    operators.Pop(); // Pop the reference LBRACK
                    break;
            }
        }

        // Calculate the remaining operations
        while (operators.Count > 0)
        {
            var a = values.Pop();
            var b = values.Pop();
            var o = operators.Pop();

            values.Push(o.Evaluate(b, a));
        }

        return values.Pop();
    }
}
