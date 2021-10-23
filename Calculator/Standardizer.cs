using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculator
{
    /// <summary>
    /// Has the logic to standardize an equation into something the calculator can parse
    /// </summary>
    internal class Standardizer
    {
        /// <summary>
        /// The list of operators that contribute to standardization
        /// </summary>
        private readonly Operator[] Operators;

        public Standardizer(Operator[] operators)
        {
            Operators = operators;
        }

        /// <summary>
        /// Standardizes an equation by inserting brackets and operators where needed
        /// </summary>
        /// <param name="equation">The equation to standardize</param>
        /// <returns>The provided equation that is completely standardized</returns>
        public string Standardize(string equation) => AddMultiplicationSigns(FixBrackets(RemoveWhitespace(equation)));

        /// <summary>
        /// Removes all the whitespace characters in a string
        /// </summary>
        /// <param name="equation">The equation to remove whitespace from</param>
        /// <returns>The provided equation with removed whitespace</returns>
        private static string RemoveWhitespace(string equation)
        {
            // Matches all whitespace characters
            var whitespaceChars = new Regex(@"\s+");
            return whitespaceChars.Replace(equation, "");
        }

        /// <summary>
        /// Fixes the equation by ensuring there are equal number of brackets
        /// </summary>
        /// <param name="equation">The equation to fix the brackets in</param>
        /// <returns>The provided equation with the correct number of brackets</returns>
        private static string FixBrackets(string equation)
        {
            int lBrack = 0, rBrack = 0;

            // Count the number of each bracket
            foreach (var c in equation)
            {
                if (c == '(') ++lBrack;
                else if (c == ')') ++rBrack;
            }

            // Too many closing brackets
            if (rBrack > lBrack)
            {
                throw new MathSyntaxException("Equation has too many closing brackets");
            }
            else if (lBrack > rBrack)
            {
                // Append closing brackets to the end of the equation as needed
                for (var i = rBrack; i < lBrack; i++)
                {
                    equation += ")";
                }
            }

            return equation;
        }

        /// <summary>
        /// Inserts multiplication signs in the correct places
        /// </summary>
        /// <param name="equation">The equation to fix the multiplication signs in</param>
        /// <returns>The provided equation with multiplication signs inserted at the right places</returns>
        private string AddMultiplicationSigns(string equation)
        {
            List<char> eq = new List<char>(equation);
            var operators = string.Join("", from o in Operators select o.Symbol);

            for (var i = 1; i < eq.Count; i++)
            {
                // Matches x( where x not in operators
                if (eq[i] == '(' && !operators.Contains(eq[i - 1]) && eq[i - 1] != '(')
                {
                    eq.Insert(i, '*');
                }
                // Matches )x where x not in operators
                else if (eq[i - 1] == ')' && !operators.Contains(eq[i]) && eq[i] != ')')
                {
                    eq.Insert(i, '*');
                }
            }

            return string.Join("", eq);
        }
    }
}
