using System;

namespace ExpressionEvaluator.Exceptions
{
    public class InvalidExpressionException : Exception
    {
        protected Expression expression;

        public InvalidExpressionException(string message, Expression expression) 
            : base(message) => this.expression = expression;

        public Expression InvalidExpression => expression;
    }
}
