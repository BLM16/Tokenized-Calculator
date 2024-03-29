﻿using BLM16.Util.Calculator.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BLM16.Util.Calculator.Tests;

/// <summary>
/// Contains the tests for the Parser class
/// </summary>
[TestClass]
public class ParserTests
{
    private readonly Parser parser = new();

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
    /// Checks that malformed binary operations throw an exception
    /// </summary>
    /// <param name="tokens">The tokenized equation to test</param>
    [DataTestMethod]
    [DynamicData(nameof(MalformedBinaryOperatorEquations), DynamicDataSourceType.Property)]
    [ExpectedException(typeof(MathSyntaxException))]
    public void Evaluate_ExceptionOnMalformedBinaryOperators(List<Token> tokens)
        => parser.Evaluate(tokens);

    /// <summary>
    /// The lexed equations and expected values to test correct evaluation with
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
                    new Token(TokenType.OPERATOR, DefaultOperators.Multiplication),
                    new Token(TokenType.LBRACK, '('),
                    new Token(TokenType.NUMBER, "3.7"),
                    new Token(TokenType.OPERATOR, DefaultOperators.Addition),
                    new Token(TokenType.NUMBER, '2'),
                    new Token(TokenType.RBRACK, ')'),
                    new Token(TokenType.OPERATOR, DefaultOperators.Division),
                    new Token(TokenType.LBRACK, '('),
                    new Token(TokenType.NUMBER, '5'),
                    new Token(TokenType.OPERATOR, DefaultOperators.Subtraction),
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
                    new Token(TokenType.OPERATOR, DefaultOperators.Addition),
                    new Token(TokenType.NUMBER, "7"),
                    new Token(TokenType.RBRACK, ")"),
                    new Token(TokenType.OPERATOR, DefaultOperators.Multiplication),
                    new Token(TokenType.LBRACK, "("),
                    new Token(TokenType.NUMBER, "19"),
                    new Token(TokenType.OPERATOR, DefaultOperators.Subtraction),
                    new Token(TokenType.NUMBER, "4"),
                    new Token(TokenType.RBRACK, ")"),
                    new Token(TokenType.OPERATOR, DefaultOperators.Addition),
                    new Token(TokenType.NUMBER, "6"),
                    new Token(TokenType.OPERATOR, DefaultOperators.Multiplication),
                    new Token(TokenType.LBRACK, "("),
                    new Token(TokenType.NUMBER, "7"),
                    new Token(TokenType.OPERATOR, DefaultOperators.Division),
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
                    new Token(TokenType.OPERATOR, DefaultOperators.Multiplication),
                    new Token(TokenType.NUMBER, "8"),
                    new Token(TokenType.OPERATOR, DefaultOperators.Subtraction),
                    new Token(TokenType.NUMBER, "4"),
                    new Token(TokenType.RBRACK, ")"),
                    new Token(TokenType.OPERATOR, DefaultOperators.Division),
                    new Token(TokenType.LBRACK, "("),
                    new Token(TokenType.NUMBER, "6"),
                    new Token(TokenType.OPERATOR, DefaultOperators.Subtraction),
                    new Token(TokenType.NUMBER, "2"),
                    new Token(TokenType.RBRACK, ")")
                },
                13d
            };

            yield return new object[]
            {
                new List<Token>()
                {
                    new Token(TokenType.NUMBER, '3'),
                    new Token(TokenType.OPERATOR, DefaultOperators.Addition),
                    new Token(TokenType.NUMBER, '7'),
                    new Token(TokenType.OPERATOR, DefaultOperators.Addition),
                    new Token(TokenType.NUMBER, '5'),
                    new Token(TokenType.OPERATOR, DefaultOperators.Multiplication),
                    new Token(TokenType.NUMBER, '2'),
                },
                20d
            };

            yield return new object[]
            {
                new List<Token>()
                {
                    new Token(TokenType.NUMBER, "2"),
                    new Token(TokenType.OPERATOR, DefaultOperators.Exponent),
                    new Token(TokenType.NUMBER, "5"),
                    new Token(TokenType.OPERATOR, DefaultOperators.Multiplication),
                    new Token(TokenType.LBRACK, '('),
                    new Token(TokenType.NUMBER, '3'),
                    new Token(TokenType.OPERATOR, DefaultOperators.Addition),
                    new Token(TokenType.NUMBER, '7'),
                    new Token(TokenType.RBRACK, ')')
                },
                320d
            };

            yield return new object[]
            {
                new List<Token>()
                {
                    new Token(TokenType.NUMBER, '2'),
                    new Token(TokenType.OPERATOR, DefaultOperators.Exponent),
                    new Token(TokenType.LBRACK, '('),
                    new Token(TokenType.NUMBER, '3'),
                    new Token(TokenType.OPERATOR, DefaultOperators.Exponent),
                    new Token(TokenType.NUMBER, '2'),
                    new Token(TokenType.RBRACK, ')')
                },
                512d
            };
        }
    }

    /// <summary>
    /// The lexed equations to test malformed binary operations
    /// </summary>
    public static IEnumerable<object[]> MalformedBinaryOperatorEquations
    {
        get
        {
            yield return new[]{
                new List<Token>()
                {
                    new Token(TokenType.OPERATOR, DefaultOperators.Addition)
                }
            };
            
            yield return new[]{
                new List<Token>()
                {
                    new Token(TokenType.NUMBER, '5'),
                    new Token(TokenType.OPERATOR, DefaultOperators.Addition)
                }
            };

            yield return new[]{
                new List<Token>()
                {
                    new Token(TokenType.NUMBER, '5'),
                    new Token(TokenType.OPERATOR, DefaultOperators.Addition),
                    new Token(TokenType.OPERATOR, DefaultOperators.Division)
                }
            };

            yield return new[]{
                new List<Token>()
                {
                    new Token(TokenType.NUMBER, '5'),
                    new Token(TokenType.LBRACK, '('),
                    new Token(TokenType.OPERATOR, DefaultOperators.Division)
                }
            };

            yield return new[]{
                new List<Token>()
                {
                    new Token(TokenType.NUMBER, '5'),
                    new Token(TokenType.OPERATOR, DefaultOperators.Division),
                    new Token(TokenType.RBRACK, ')')
                }
            };
        }
    }
}
