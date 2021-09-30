using System;
using System.Collections.Generic;

namespace Calculator
{
    public class Calculator
    {
        /// <summary>
        /// The Lexer for tokenizing the equation
        /// </summary>
        private Lexer _lexer;

        public Calculator()
        {
            _lexer = new Lexer();
        }

        public static string Calculate(string equation)
        {
            equation = Standardizer.Standardize(equation);

            return "";
        }
    }
}
