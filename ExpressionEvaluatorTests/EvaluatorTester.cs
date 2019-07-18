using ExpressionEvaluator;
using ExpressionEvaluator.Evaluation;
using ExpressionEvaluator.Parsing;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace ExpressionEvaluatorTests
{
    [Trait("Evaluator", "EvaluatorTester")]
    public class EvaluatorTester
    {
        [Theory]
        [InlineData("-20", -20)]
        [InlineData("-+-+-5", -5)]
        [InlineData("-+-+5", 5)]
        [InlineData("3+-2", 1)]
        [InlineData("3-+2", 1)]
        [InlineData("3---2", 1)]
        [InlineData("-3 + 2", -1)]
        [InlineData("-3 * 2", -6)]
        [InlineData("2 * -3", -6)]
        [InlineData("4 / -2", -2)]
        [InlineData("4 / +2", 2)]
        [InlineData("4 / +-2", -2)]
        [InlineData("2 - +3", -1)]
        [InlineData("3 -+-+-+ 5", -2)]
        [InlineData("- (5 + 4)", -9)]
        [InlineData("(-12 + 20) -2", 6)]
        [InlineData("(8 + 2) - +3", 7)]
        [InlineData("3--2*4", 11)]
        [InlineData("3 + -2 - 4", -3)]
        [InlineData("3 + -2 * 4", -5)]
        [InlineData("3 * -2 + 4", -2)]
        [InlineData("3--2*-4", -5)]
        [InlineData("2-(-12+20)", -6)]
        [InlineData("2 +- (3 + 4) --5", 0)]
        [InlineData("2", 2)]
        [InlineData("(2)", 2)]
        [InlineData("(((2)))", 2)]
        [InlineData("0 -   1", -1)]
        [InlineData("((0)) -   1", -1)]
        [InlineData("((0)) -   (((53)))", -53)]
        [InlineData("0 - 0", 0)]
        [InlineData("3+5", 8)]
        [InlineData("8/3", 2)]
        [InlineData("4*3", 12)]
        [InlineData("4-3* 2 + 1", -1)]
        [InlineData("3+7/5", 4)]
        [InlineData("7/5 + 3", 4)]
        [InlineData("3 + 5 / 2 /3 + 199 -5 + 3 * 2 * 4 * 5 + 7", 324)]
        [InlineData("3 + 235 / 12 /3 + 199 -5 + 3 * 2 * 21 * 5 + 7", 840)]
        [InlineData("235 / 12 /3-43 + 199 -5 + 3 * 2 * 21 * 5 + 7", 794)]
        [InlineData("235 / 12 /3-43 + 199 -5 + 3 * 2 * 21 * 5 / 7", 247)]
        [InlineData("235 - 12 +3-43 / 3 + 199 -5 + 3 * 2 * 21 * 5 / 7", 496)]
        [InlineData("5 - (4 + 7) + 3", -3)]
        [InlineData("5 - (4 + 7) * 3", -28)]
        [InlineData("5 * (4 + 7) - 3", 52)]
        [InlineData("3 * (5 + 7) * 4", 144)]
        [InlineData("(4-3)* 2 + 1", 3)]
        [InlineData("(4-1)- 2 * 3", -3)]
        [InlineData("(4-1)* 2 * 3", 18)]
        [InlineData("1 + 2* (4-3)", 3)]
        [InlineData("3 * 2 - (4-1)", 3)]
        [InlineData("3 * 2 * (4-1)", 18)]
        [InlineData("3 * (7 + 5)", 36)]
        [InlineData("(7 + 5) / 3", 4)]
        [InlineData("6 + 3 * (7 + 5)", 42)]
        [InlineData("6 + 3 * (7 + 5)-8", 34)]
        [InlineData("9 - 3 * (7 + 5)", -27)]
        [InlineData("2 + 4 -3 * (5+7)", -30)]
        [InlineData("(4-3)*(2+5)", 7)]
        [InlineData("3 + 7-5/2/3+ (4-5) /2 *3 +4", 14)]
        [InlineData("(4-3)* (2 + 5) - (7/2) + 1", 5)]
        [InlineData("2+ ((4-5) /2 + 5)/3", 3)]
        [InlineData("3 + 7/5/2+ ((4-5) /2 + 5)/3", 4)]
        [InlineData("3 + 7/5/2- ((4-5) /2 + 5)/3", 2)]
        [InlineData("3 + 7/5/2- ((4-5) /2 + 5) + 9", 7)]
        [InlineData("3 + 7/5/2- ((4-5) /2 + 5)/3 + 4", 6)]
        [InlineData("6 + (7 - (8 + 3 / 2))", 4)]
        [InlineData("7 - (8 + 3 / 2) + 9", 7)]
        [InlineData("6 + (7 - (8 + 3 / 2) + 9)", 13)]
        [InlineData("6 + (7 - (8 + 3 / 2 + 1) * 9)", -77)]
        [InlineData("6 + (7 - (8 + 3 / (2 + 1)) * 9)", -68)]
        [InlineData("6 + (7 - (8 + 3 / (2 + 1 - (5- 3 *6 +3)) + 4) * 9)", -95)]
        [InlineData("5 + 3 / (2 - 1) + 4", 12)]
        [InlineData("6 - (5 + 3 / (2 - 1) + 4)", -6)]
        [InlineData("5 * -+-2 + 4", 14)]
        [InlineData("3 * -+-2 / 4", 1)]
        [InlineData("3 + -+-2 * 4", 11)]
        [InlineData("3 + -+-2 - 4", 1)]
        [InlineData("(-20)", -20)]
        [InlineData("((-12)) + 3", -9)]
        [InlineData("2 + -(-3 + 4) - -5", 6)]
        [InlineData("2 + -+(--(-3 + 4) + 5) - +1", -5)]
        [InlineData("3 + -7 / 5 / -2 - ((-4-5) / 2 + -5)/3 + -4", 2)]
        [InlineData("3 - -2 * -4 + 1", -4)]
        [InlineData("3 - -2 * -4 * 1 + 5", 0)]
        public void Evaluate_IntExpressions_Correctly(string expression, int expected)
        {
            //Arrange
            var parser = new Parser(expression);
            var evaluator = new IntEvaluator();

            //Act
            Token t = evaluator.Evaluate(parser.Parse());

            //Assert
            Assert.Equal(expected, int.Parse(t.Value));
        }

        [Theory]
        [InlineData("21.15", 21.15)]
        [InlineData("0 -   1.789", -1.79)]
        [InlineData("0 - 0", 0)]
        [InlineData("3.61+5.0", 8.61)]
        [InlineData("8.46/3.72", 2.27)]
        [InlineData("4.78*3.31", 15.82)]
        [InlineData("3.92 + 5.456 / 2.123 /3.2 + 199.0 -5.645 + 3.47 * 2.1 * 2.694 * 5.1 + 7.7", 305.90)]
        [InlineData("3.6 + 235.5 / 12.4 /3.3 + 199.2 -5.1 + 3.0 * 2.1 * 21.2 * 5.3 + 7.4", 918.72)]
        [InlineData("235.6 / 12.1 /3.5-43.6 + 199.7 -5.8 + 3.9 * 0.2 * 21.0 * 5.4 + 7.1", 251.42)]
        [InlineData("235.0 / 12.0 /3.0-43.67 + 199.45 -5.7 + 3.2 * 2.2 * 21.1 * 5.2 / 7.1", 265.40)]
        [InlineData("235.0 - 12.0 +3.0-43.0 / 3.0 + 199.0 -5.0 + 3.0 * 2.0 * 21.0 * 5.0 / 7.0", 495.67)]
        [InlineData("6.2 + (7.3 - (8.45 + 3.33 / (2.89 + 1.12 - (5.7- 3.8 *6.66 +3.54)) + 4.32) * 9.89)", -114.44)]
        [InlineData("3.983 + -7.123 / 5.45 / -2.2 - ((-4.7-5.6) / 2.3 + -5.89)/3.56 + -4.98", 2.51)]
        public void Evaluate_DoubleExpressions_Correctly(string expression, double expected)
        {
            //Arrange
            var parser = new Parser(expression);
            var eval = new DoubleEvaluator();

            //Act
            Token t = eval.Evaluate(parser.Parse());

            //Assert
            Assert.Equal(expected, Math.Round(double.Parse(t.Value, CultureInfo.InvariantCulture), 2));
        }

        [Fact]
        public void Evaluate_SymbolExpression_Correctly()
        {
            //Demonstrates how to evaluate a general expression by deferring the loading of values from a data source.

            //Arrange
            string expression = "a +- (b + c) --d";
            var dataSource = new Dictionary<string, int>
            {
                { "a", 2}, { "b", 3 }, { "c", -4 }, { "d", 5 }
            };
            var evaluator = new IntEvaluator();
            evaluator.ValueForToken = token => 
            {
                if (dataSource.ContainsKey(token.Value))
                    return dataSource[token.Value];
                else
                    return evaluator.GetValueDefaultAccessor()(token); //In successive token evaluations, the symbol is already replaced so we should use the default accessor
            };

            //Act
            var result = evaluator.Evaluate(new Parser(expression).Parse());

            //Assert
            Assert.Equal("8", result.Value);
        }
    }
}
