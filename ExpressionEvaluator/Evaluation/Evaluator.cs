using ExpressionEvaluator.Exceptions;
using System;

namespace ExpressionEvaluator.Evaluation
{
    public abstract class Evaluator<T>
    {
        public Func<Token, T> ValueForToken { get; set; }

        public abstract Func<Token, T> GetValueDefaultAccessor();

        public abstract Token Add(Token token1, Token token2);

        public abstract Token Subtract(Token token1, Token token2);

        public abstract Token Multiply(Token token1, Token token2);

        public abstract Token Divide(Token token1, Token token2);

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

            if (expression.Operator.TokenType == TokenOption.ADD)
                result = Add(leftOperand, rightOperand);
            else if (expression.Operator.TokenType == TokenOption.SUBTRACT)
                result = Subtract(leftOperand, rightOperand);
            else if (expression.Operator.TokenType == TokenOption.MULTIPLY)
                result = Multiply(leftOperand, rightOperand);
            else if (expression.Operator.TokenType == TokenOption.DIVIDE)
                result = Divide(leftOperand, rightOperand);
            else
                throw new InvalidExpressionException("Unknown operator in expression.", expression);
            return result;
        }
    }
}
