using ExpressionEvaluator;
using ExpressionEvaluator.Parsing;
using System;
using System.IO;
using Xunit;

namespace ExpressionEvaluatorTests
{
    [Trait("Compiler", "CompilerTester")]
    public class CompilerTester
    {
        [Theory]
        [InlineData("2 + 4 -3 * (5+7)")]
        [InlineData("(4-3)*(2+5)")]
        [InlineData("3 + 7-5/2/3+ (4-5) /2 *3 +4")]
        [InlineData("(4-3)* (2 + 5) - (7/2) + 1")]
        [InlineData("2+ ((4-5) /2 + 5)/3")]
        [InlineData("3 + 7/5/2+ ((4-5) /2 + 5)/3")]
        [InlineData("3 + 7/5/2- ((4-5) /2 + 5)/3")]
        [InlineData("3 + 7/5/2- ((4-5) /2 + 5) + 9")]
        [InlineData("3 + 7/5/2- ((4-5) /2 + 5)/3 + 4")]
        [InlineData("6 + (7 - (8 + 3 / 2))")]
        [InlineData("7 - (8 + 3 / 2) + 9")]
        [InlineData("6 + (7 - (8 + 3 / 2) + 9)")]
        [InlineData("6 + (7 - (8 + 3 / 2 + 1) * 9)")]
        [InlineData("6 + (7 - (8 + 3 / (2 + 1)) * 9)")]
        [InlineData("6 + (7 - (8 + 3 / (2 + 1 - (5- 3 *6 +3)) + 4) * 9)")]
        [InlineData("5 + 3 / (2 - 1) + 4")]
        [InlineData("6 - (5 + 3 / (2 - 1) + 4)")]
        public void Compile_Expressions_Correctly(string expression)
        {
            //Arrange
            string fileName = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss.bin");

            //Act
            var uncompiledExpression = new Parser(expression).Parse();
            Compiler.Compile(expression, fileName);
            var compiledExpression = Compiler.Load(fileName);
            File.Delete(fileName);

            //Assert
            Assert.Equal(uncompiledExpression.ToJSON(), compiledExpression.ToJSON());
        }
    }
}
