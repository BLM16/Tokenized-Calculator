namespace Calculator
{
    /// <summary>
    /// Contains the required logic to evaluate an equation from a string
    /// </summary>
    public class Calculator
    {
        /// <summary>
        /// The Lexer for tokenizing the equation
        /// </summary>
        private readonly Lexer lexer;
        /// <summary>
        /// The Parser for evaluating the equation
        /// </summary>
        private readonly Parser parser;

        public Calculator()
        {
            lexer = new Lexer();
            parser = new Parser();
        }

        /// <summary>
        /// Runs the required steps to calculate a given equation
        /// </summary>
        /// <param name="equation">The equation to evaluate</param>
        /// <returns>The result of evaluating the equation</returns>
        public double Calculate(string equation)
        {
            equation = Standardizer.Standardize(equation);
            var tokens = lexer.Parse(equation);
            var result = parser.Evaluate(tokens);

            return result;
        }
    }
}
