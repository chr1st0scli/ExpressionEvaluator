using ExpressionEvaluator.Exceptions;
using System;

namespace ExpressionEvaluator.Evaluation
{
    /// <summary>
    /// Evaluates a parse tree, i.e. it produces a result out of it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Evaluator<T>
    {
        public Func<Token, T> ValueForToken { get; set; }

        /// <summary>
        /// Override to supply a default manner to extract a value from a Token.
        /// </summary>
        /// <returns>The value.</returns>
        public abstract Func<Token, T> GetValueDefaultAccessor();

        /// <summary>
        /// Override to support binary and unary addition.
        /// </summary>
        /// <param name="token1">Left operand.</param>
        /// <param name="token2">Right operand.</param>
        /// <returns>The result of the addition.</returns>
        public abstract Token Add(Token token1, Token token2);

        /// <summary>
        /// Override to support binary and unary subtraction.
        /// </summary>
        /// <param name="token1">Left operand.</param>
        /// <param name="token2">Right operand.</param>
        /// <returns>The result of the subtraction.</returns>
        public abstract Token Subtract(Token token1, Token token2);

        /// <summary>
        /// Override to support binary multiplication.
        /// </summary>
        /// <param name="token1">Left operand.</param>
        /// <param name="token2">Right operand.</param>
        /// <returns>The result of the multiplication.</returns>
        public abstract Token Multiply(Token token1, Token token2);

        /// <summary>
        /// Override to support binary division.
        /// </summary>
        /// <param name="token1">Left operand.</param>
        /// <param name="token2">Right operand.</param>
        /// <returns>The result of the division.</returns>
        public abstract Token Divide(Token token1, Token token2);

        /// <summary>
        /// Recursively evaluates the expression tree until a single Token results.
        /// </summary>
        /// <param name="expression">The parent expression or the root of the expression tree.</param>
        /// <returns>The token which is the result of the evaluation.</returns>
        /// <exception cref="ExpressionEvaluator.Exceptions.InvalidExpressionException">Thrown if an expression doesn't have a left part (minimum requirement) or involves an unknown operator.</exception>
        public Token Evaluate(Expression expression)
        {
            Token leftOperand, rightOperand = null, result;
            if (expression.LeftOperand != null)
                leftOperand = expression.LeftOperand;
            else if (expression.LeftExpression != null)
                leftOperand = Evaluate(expression.LeftExpression);
            else
                throw new InvalidExpressionException("An expression must at least have a left operand or expression.", expression);

            if (expression.RightOperand != null)
                rightOperand = expression.RightOperand;
            else if (expression.RightExpression != null)
                rightOperand = Evaluate(expression.RightExpression);
            else if (expression.Operator == null)
                return leftOperand; //Leaf expression

            if (expression.Operator.TokenType == TokenType.ADD)
                result = Add(leftOperand, rightOperand);
            else if (expression.Operator.TokenType == TokenType.SUBTRACT)
                result = Subtract(leftOperand, rightOperand);
            else if (expression.Operator.TokenType == TokenType.MULTIPLY)
                result = Multiply(leftOperand, rightOperand);
            else if (expression.Operator.TokenType == TokenType.DIVIDE)
                result = Divide(leftOperand, rightOperand);
            else
                throw new InvalidExpressionException("Unknown operator in expression.", expression);
            return result;
        }
    }
}
