using System;
using System.Collections.Generic;

namespace Calculator
{
    public class Calculator
    {
        /// <summary>
        /// The Lexer for tokenizing the equation
        /// </summary>
        private readonly Lexer _lexer;

        public Calculator()
        {
            _lexer = new Lexer();
        }

        public string Calculate(string equation)
        {
            equation = Standardizer.Standardize(equation);
            var tokens = _lexer.Parse(equation);

            return "";
        }
    }
}
