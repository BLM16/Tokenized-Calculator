using System.Collections.Generic;

namespace Calculator
{
    /// <summary>
    /// Has the logic to evaluate an equation
    /// </summary>
    internal class Parser
    {
        /// <summary>
        /// The stack of values that still need to be evaluated
        /// </summary>
        private readonly Stack<double> values = new Stack<double>();
        /// <summary>
        /// The stack of operators that need to be applied to the values
        /// </summary>
        private readonly Stack<string> operators = new Stack<string>();

        /// <summary>
        /// Evaluates the result of a tokenized mathematical equation
        /// </summary>
        /// <param name="tokens">The list of tokens parsed from the equation</param>
        /// <returns>The result of the calculations</returns>
        public double Evaluate(List<Token> tokens)
        {
            // Operators will be evaluated if they have precedence
            // Operators and numbers will be pushed to the stack without precedence
            foreach (var t in tokens)
            {
                switch (t.Type)
                {
                    case TokenType.NUMBER:
                        values.Push(double.Parse(t.Value));
                        break;

                    case TokenType.PLUS:
                    case TokenType.MINUS:
                    case TokenType.MULT:
                    case TokenType.DIV:
                        // Push the operator to the stack if there are no operators as precedence cannot be determined
                        if (operators.Count == 0)
                        {
                            operators.Push(t.Value);
                        }
                        // Push the operator to the stack if the previous operator has a lower precedence
                        else if("+-(".Contains(operators.Peek()))
                        {
                            operators.Push(t.Value);
                        }
                        else
                        {
                            // Continuously evaluate the equation until precedence is lost
                            while (operators.Count > 0 && !"+-(".Contains(operators.Peek()))
                            {
                                var a = values.Pop();
                                var b = values.Pop();
                                var o = operators.Pop();

                                values.Push(Calculate(a, b, o));
                            }
                            // Push the current operator to the stack
                            operators.Push(t.Value);
                        }
                        break;

                    case TokenType.LBRACK:
                        // This token is simply pushed as it only provides an endpoint to RBRACK tokens
                        operators.Push(t.Value);
                        break;

                    case TokenType.RBRACK:
                        // Evaluate all the math inside the brackets
                        // Previous precedence operations will ensure this will always follow bedmas
                        while (operators.Peek() != "(")
                        {
                            var a = values.Pop();
                            var b = values.Pop();
                            var o = operators.Pop();

                            values.Push(Calculate(a, b, o));
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

                values.Push(Calculate(a, b, o));
            }

            return values.Pop();
        }

        /// <summary>
        /// Calculates the result from given two values and an operator.
        /// This method assumes <c>a</c> and <c>b</c> are from a LIFO stack.
        /// For direction specific calculations, <c>b</c> is left of the operator.
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The second number</param>
        /// <param name="op">The operator</param>
        /// <returns>The result of applying the operator to a and b</returns>
        private double Calculate(double a, double b, string op)
        {
            switch (op)
            {
                case "+":
                    return a + b;

                case "-":
                    return b - a; // Inverse because stack is LIFO

                case "*":
                    return a * b;

                case "/":
                    return b / a; // Inverse because stack is LIFO

                default:
                    throw new MathSyntaxException($"Operator '{op}' is not defined");
            }
        }
    }
}
