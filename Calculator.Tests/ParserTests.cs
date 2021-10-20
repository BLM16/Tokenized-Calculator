using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Calculator.Tests
{
    /// <summary>
    /// Contains the tests for the Parser class
    /// </summary>
    [TestClass]
    public class ParserTests
    {
        private readonly Parser parser = new Parser();

        /// <summary>
        /// Checks that tokenized equations are evaluated correctly
        /// </summary>
        /// <param name="tokens">The tokenized equation to evaluate</param>
        /// <param name="expected">The expected result</param>
        [DataTestMethod]
        [DynamicData(nameof(LexedEquations), DynamicDataSourceType.Property)]
        public void Evaluate_EvaluatesEquation(List<Token> tokens, double expected)
            => Assert.AreEqual(expected, parser.Evaluate(tokens));

        /// <summary>
        /// The lexed equations and expected values to test with
        /// </summary>
        public static IEnumerable<object[]> LexedEquations
        {
            get
            {
                yield return new object[]
                {
                    new List<Token>()
                    {
                        new Token(TokenType.NUMBER, "5.14"),
                        new Token(TokenType.MULT, '*'),
                        new Token(TokenType.LBRACK, '('),
                        new Token(TokenType.NUMBER, "3.7"),
                        new Token(TokenType.PLUS, '+'),
                        new Token(TokenType.NUMBER, '2'),
                        new Token(TokenType.RBRACK, ')'),
                        new Token(TokenType.DIV, '/'),
                        new Token(TokenType.LBRACK, '('),
                        new Token(TokenType.NUMBER, '5'),
                        new Token(TokenType.MINUS, '-'),
                        new Token(TokenType.NUMBER, '3'),
                        new Token(TokenType.RBRACK, ')')
                    },
                    14.649d
                };

                yield return new object[]
                {
                    new List<Token>()
                    {
                        new Token(TokenType.LBRACK, "("),
                        new Token(TokenType.NUMBER, "12"),
                        new Token(TokenType.PLUS, "+"),
                        new Token(TokenType.NUMBER, "7"),
                        new Token(TokenType.RBRACK, ")"),
                        new Token(TokenType.MULT, "*"),
                        new Token(TokenType.LBRACK, "("),
                        new Token(TokenType.NUMBER, "19"),
                        new Token(TokenType.MINUS, "-"),
                        new Token(TokenType.NUMBER, "4"),
                        new Token(TokenType.RBRACK, ")"),
                        new Token(TokenType.PLUS, "+"),
                        new Token(TokenType.NUMBER, "6"),
                        new Token(TokenType.MULT, "*"),
                        new Token(TokenType.LBRACK, "("),
                        new Token(TokenType.NUMBER, "7"),
                        new Token(TokenType.DIV, "/"),
                        new Token(TokenType.NUMBER, "4"),
                        new Token(TokenType.RBRACK, ")")
                    },
                    295.5d
                };

                yield return new object[]
                {
                    new List<Token>()
                    {
                        new Token(TokenType.LBRACK, "("),
                        new Token(TokenType.NUMBER, "7"),
                        new Token(TokenType.MULT, "*"),
                        new Token(TokenType.NUMBER, "8"),
                        new Token(TokenType.MINUS, "-"),
                        new Token(TokenType.NUMBER, "4"),
                        new Token(TokenType.RBRACK, ")"),
                        new Token(TokenType.DIV, "/"),
                        new Token(TokenType.LBRACK, "("),
                        new Token(TokenType.NUMBER, "6"),
                        new Token(TokenType.MINUS, "-"),
                        new Token(TokenType.NUMBER, "2"),
                        new Token(TokenType.RBRACK, ")")
                    },
                    13d
                };
            }
        }
    }
}
