using ExpressionEvaluator.Exceptions;
using ExpressionEvaluator.Parsing;
using System;
using System.IO;
using Xunit;

namespace ExpressionEvaluatorTests
{
    [Trait("Parser", "ParserTester")]
    public class ParserTester
    {
        [Theory]
        [InlineData("-20", "1.json")]
        [InlineData("-+-+-5", "2.json")]
        [InlineData("-+-+5", "3.json")]
        [InlineData("3+-2", "4.json")]
        [InlineData("3-+2", "5.json")]
        [InlineData("3---2", "6.json")]
        [InlineData("-3 + 2", "7.json")]
        [InlineData("-3 * 2", "8.json")]
        [InlineData("2 * -3", "9.json")]
        [InlineData("4 / -2", "10.json")]
        [InlineData("4 / +2", "11.json")]
        [InlineData("4 / +-2", "12.json")]
        [InlineData("2 - +3", "13.json")]
        [InlineData("3 -+-+-+ 5", "14.json")]
        [InlineData("- (5 + 4)", "15.json")]
        [InlineData("(-12 + 20) -2", "16.json")]
        [InlineData("(8 + 2) - +3", "17.json")]
        [InlineData("3--2*4", "18.json")]
        [InlineData("3 + -2 - 4", "19.json")]
        [InlineData("3 + -2 * 4", "20.json")]
        [InlineData("3 * -2 + 4", "21.json")]
        [InlineData("3--2*-4", "22.json")]
        [InlineData("2-(-12+20)", "23.json")]
        [InlineData("2 +- (3 + 4) --5", "24.json")]
        [InlineData("2", "25.json")]
        [InlineData("(2)", "26.json")]
        [InlineData("(((2)))", "27.json")]
        [InlineData("0 -   1", "28.json")]
        [InlineData("((0)) -   1", "29.json")]
        [InlineData("((0)) -   (((53)))", "30.json")]
        [InlineData("0 - 0", "31.json")]
        [InlineData("3+5", "32.json")]
        [InlineData("8/3", "33.json")]
        [InlineData("4*3", "34.json")]
        [InlineData("4-3* 2 + 1", "35.json")]
        [InlineData("3+7/5", "36.json")]
        [InlineData("7/5 + 3", "37.json")]
        [InlineData("3 + 5 / 2 /3 + 199 -5 + 3 * 2 * 4 * 5 + 7", "38.json")]
        [InlineData("3 + 235 / 12 /3 + 199 -5 + 3 * 2 * 21 * 5 + 7", "39.json")]
        [InlineData("235 / 12 /3-43 + 199 -5 + 3 * 2 * 21 * 5 + 7", "40.json")]
        [InlineData("235 / 12 /3-43 + 199 -5 + 3 * 2 * 21 * 5 / 7", "41.json")]
        [InlineData("235 - 12 +3-43 / 3 + 199 -5 + 3 * 2 * 21 * 5 / 7", "42.json")]
        [InlineData("5 - (4 + 7) + 3", "43.json")]
        [InlineData("5 - (4 + 7) * 3", "44.json")]
        [InlineData("5 * (4 + 7) - 3", "45.json")]
        [InlineData("3 * (5 + 7) * 4", "46.json")]
        [InlineData("(4-3)* 2 + 1", "47.json")]
        [InlineData("(4-1)- 2 * 3", "48.json")]
        [InlineData("(4-1)* 2 * 3", "49.json")]
        [InlineData("1 + 2* (4-3)", "50.json")]
        [InlineData("3 * 2 - (4-1)", "51.json")]
        [InlineData("3 * 2 * (4-1)", "52.json")]
        [InlineData("3 * (7 + 5)", "53.json")]
        [InlineData("(7 + 5) / 3", "54.json")]
        [InlineData("6 + 3 * (7 + 5)", "55.json")]
        [InlineData("6 + 3 * (7 + 5)-8", "56.json")]
        [InlineData("9 - 3 * (7 + 5)", "57.json")]
        [InlineData("2 + 4 -3 * (5+7)", "58.json")]
        [InlineData("(4-3)*(2+5)", "59.json")]
        [InlineData("3 + 7-5/2/3+ (4-5) /2 *3 +4", "60.json")]
        [InlineData("(4-3)* (2 + 5) - (7/2) + 1", "61.json")]
        [InlineData("2+ ((4-5) /2 + 5)/3", "62.json")]
        [InlineData("3 + 7/5/2+ ((4-5) /2 + 5)/3", "63.json")]
        [InlineData("3 + 7/5/2- ((4-5) /2 + 5)/3", "64.json")]
        [InlineData("3 + 7/5/2- ((4-5) /2 + 5) + 9", "65.json")]
        [InlineData("3 + 7/5/2- ((4-5) /2 + 5)/3 + 4", "66.json")]
        [InlineData("6 + (7 - (8 + 3 / 2))", "67.json")]
        [InlineData("7 - (8 + 3 / 2) + 9", "68.json")]
        [InlineData("6 + (7 - (8 + 3 / 2) + 9)", "69.json")]
        [InlineData("6 + (7 - (8 + 3 / 2 + 1) * 9)", "70.json")]
        [InlineData("6 + (7 - (8 + 3 / (2 + 1)) * 9)", "71.json")]
        [InlineData("6 + (7 - (8 + 3 / (2 + 1 - (5- 3 *6 +3)) + 4) * 9)", "72.json")]
        [InlineData("5 + 3 / (2 - 1) + 4", "73.json")]
        [InlineData("6 - (5 + 3 / (2 - 1) + 4)", "74.json")]
        [InlineData("5 * -+-2 + 4", "75.json")]
        [InlineData("3 * -+-2 / 4", "76.json")]
        [InlineData("3 + -+-2 * 4", "77.json")]
        [InlineData("3 + -+-2 - 4", "78.json")]
        [InlineData("(-20)", "79.json")]
        [InlineData("((-12)) + 3", "80.json")]
        [InlineData("2 + -(-3 + 4) - -5", "81.json")]
        [InlineData("2 + -+(--(-3 + 4) + 5) - +1", "82.json")]
        [InlineData("3 + -7 / 5 / -2 - ((-4-5) / 2 + -5)/3 + -4", "83.json")]
        [InlineData("3 - -2 * -4 + 1", "84.json")]
        [InlineData("3 - -2 * -4 * 1 + 5", "85.json")]
        public void Parse_Expressions_Correctly(string expression, string fileName)
        {
            //Arrange
            var parser = new Parser(expression);
            string jsonToMatch = File.ReadAllText("ExpectedParseTrees\\" + fileName).Trim();

            //Act
            var expr = parser.Parse();
            string actualJson = expr.ToJSON(expression);

            //Assert
            Assert.Equal(jsonToMatch, actualJson);
        }

        [Theory]
        [InlineData("3+*3")]
        [InlineData("3+/3")]
        [InlineData("(3+)+3")]
        [InlineData("3-*3")]
        [InlineData("3-/3")]
        [InlineData("(3-)-3")]
        [InlineData("(3)(-3)")]
        [InlineData("3**3")]
        [InlineData("3*/3")]
        [InlineData("3//3")]
        [InlineData("3/*3")]
        [InlineData("(3+*)3")]
        [InlineData("(3+/)3")]
        [InlineData("(*3+3)")]
        [InlineData("(/3+3)")]
        [InlineData("()3+3")]
        [InlineData("3 3 + 3")]
        [InlineData("3 (3*3")]
        [InlineData("(3) 3 + 3")]
        [InlineData("(3) (3*3)")]
        [InlineData("*3 + 3")]
        [InlineData("/3 + 3")]
        [InlineData(")3 + 3")]
        [InlineData("3 + 3 +")]
        [InlineData("3 + 3 -")]
        [InlineData("3 + 3 *")]
        [InlineData("3 + 3 /")]
        [InlineData("3 + 3 + (")]
        public void Parse_IllegalExpressions_Error(string expression)
        {
            //Arrange
            var parser = new Parser(expression);

            //Act
            void DoAct() => parser.Parse();

            //Assert
            Assert.Throws<IllegalTokenException>(DoAct);
        }

        [Theory]
        [InlineData("(3+3")]
        [InlineData("3+(3")]
        [InlineData("3+3)")]
        [InlineData("3)+3")]
        [InlineData("(3+3))")]
        [InlineData("(3))+3")]
        [InlineData("((3+3)")]
        [InlineData("((3)+3")]
        [InlineData("(3)+(((3")]
        [InlineData("3))))+3")]
        public void Parse_UnmatchedParens_Error(string expression)
        {
            //Arrange
            var parser = new Parser(expression);

            //Act
            void DoAct() => parser.Parse();

            //Assert
            Assert.Throws<UnmatchedParenthesisException>(DoAct);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Parse_NoExpression_Error(string expression)
        {
            //Arrange
            void DoAct() => new Parser(expression).Parse();

            //Act, Assert
            Assert.Throws<ArgumentException>(DoAct);
        }
    }
}
