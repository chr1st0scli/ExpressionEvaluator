using System;

namespace ExpressionEvaluator.Parsing
{
    public static class ExpressionExtensions
    {
        public static void AddOperand(this Expression expr, Token operand)
        {
            if (expr.LeftOperand == null && expr.LeftExpression == null)
                expr.LeftOperand = operand;
            else
                expr.RightOperand = operand;
        }

        public static void AddExpression(this Expression expr, Expression expression)
        {
            if (expr.LeftOperand == null && expr.LeftExpression == null)
                expr.LeftExpression = expression;
            else
                expr.RightExpression = expression;
        }

        public static Expression SplitInNewBelow(this Expression expr)
        {
            Expression expression;

            if (expr.RightOperand != null)
                expression = new Expression { LeftOperand = expr.RightOperand };
            else if (expr.RightExpression != null)
                expression = new Expression { LeftExpression = expr.RightExpression };
            else
                throw new InvalidOperationException("An expression without a right operand or expression cannot be split.");

            expr.RightExpression = expression;
            return expression;
        }

        public static Expression MoveInNewBelow(this Expression expr)
        {
            var tmpParent = expr.ParentExpression;
            var expression = new Expression { LeftExpression = expr };
            if (tmpParent != null) tmpParent.RightExpression = expression;
            return expression;
        }
    }
}
