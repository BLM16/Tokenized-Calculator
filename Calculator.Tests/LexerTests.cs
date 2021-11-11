using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Calculator.Tests;

/// <summary>
/// Contains the tests for the Lexer class
/// </summary>
[TestClass]
public class LexerTests
{
    private readonly Lexer lexer = new(Calculator.DefaultOperatorList);

    #region Parse.Tests

    /// <summary>
    /// Checks that equations are correctly tokenized
    /// </summary>
    [TestMethod]
    public void Parse_ParsesEquation()
    {
        var tokens = lexer.Parse("5.14*(3.7+2^2)/(5-3)");

        var expected = new List<Token>()
        {
            new Token(TokenType.NUMBER, "5.14"),
            new Token(TokenType.OPERATOR, DefaultOperators.Multiplication),
            new Token(TokenType.LBRACK, '('),
            new Token(TokenType.NUMBER, "3.7"),
            new Token(TokenType.OPERATOR, DefaultOperators.Addition),
            new Token(TokenType.NUMBER, '2'),
            new Token(TokenType.OPERATOR, DefaultOperators.Exponent),
            new Token(TokenType.NUMBER, '2'),
            new Token(TokenType.RBRACK, ')'),
            new Token(TokenType.OPERATOR, DefaultOperators.Division),
            new Token(TokenType.LBRACK, '('),
            new Token(TokenType.NUMBER, '5'),
            new Token(TokenType.OPERATOR, DefaultOperators.Subtraction),
            new Token(TokenType.NUMBER, '3'),
            new Token(TokenType.RBRACK, ')')
        };

        CollectionAssert.AreEqual(expected, tokens);
    }

    /// <summary>
    /// Checks that a zero is prepended before doubles with no value before the decimal point
    /// </summary>
    [TestMethod]
    public void Parse_PrependZerosBeforeDoubles()
    {
        var tokens = lexer.Parse(".305+.307*.302");

        var expected = new List<Token>()
        {
            new Token(TokenType.NUMBER, "0.305"),
            new Token(TokenType.OPERATOR, DefaultOperators.Addition),
            new Token(TokenType.NUMBER, "0.307"),
            new Token(TokenType.OPERATOR, DefaultOperators.Multiplication),
            new Token(TokenType.NUMBER, "0.302")

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

    /// <summary>
    /// Checks that exceptions are thrown when malformed doubles are encountered
    /// </summary>
    /// <param name="equation">The equation of malformed doubles to test</param>
    [DataTestMethod]
    [ExpectedException(typeof(MathSyntaxException))]
    [DataRow("3.157.92+36.4*3")]
    [DataRow("126.*14+3")]
    [DataRow("2+13.")]
    public void Parse_ExceptionOnMalformedDoubles(string equation)
        => lexer.Parse(equation);

    /// <summary>
    /// Checks that exceptions are thrown when consecutive operators are encountered
    /// </summary>
    /// <param name="equation">The equation of consecutive operators to test</param>
    [DataTestMethod]
    [ExpectedException(typeof(MathSyntaxException))]
    [DataRow("157+*3.2/6")]
    [DataRow("134+-8")]
    public void Parse_ExceptionOnConsecutiveOperators(string equation)
        => lexer.Parse(equation);

    #endregion
}
