using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Calculator.Tests
{
    /// <summary>
    /// Contains the tests for the Lexer class
    /// </summary>
    [TestClass]
    public class LexerTests
    {
        private readonly Lexer lexer = new Lexer();

        #region Parse.Tests

        /// <summary>
        /// Checks that equations are correctly tokenized
        /// </summary>
        [TestMethod]
        public void Parse_ParsesEquation()
        {
            var tokens = lexer.Parse("5.14*(3.7+2)/(5-3)");

            var expected = new List<Token>()
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
            };

            CollectionAssert.AreEqual(expected, tokens);
        }

        /// <summary>
        /// Checks that an exception is thrown when lexing an uncrecognized symbol
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MathSyntaxException))]
        public void Parse_ExceptionOnUnrecognizedSymbols()
            => lexer.Parse("3&5.2");

        #endregion
    }
}
