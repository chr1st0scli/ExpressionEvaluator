using System;

namespace ExpressionEvaluator
{
    /// <summary>
    /// Represents a language's expression that is composed as a tree.
    /// </summary>
    [Serializable]
    public class Expression
    {
        protected Token leftOperand;
        protected Token rightOperand;
        protected Expression leftExpression;
        protected Expression rightExpression;

        public Expression ParentExpression { get; set; }

        public Expression TopParentExpression
        {
            get
            {
                var topExpr = this;
                while (topExpr.ParentExpression != null)
                    topExpr = topExpr.ParentExpression;
                return topExpr;
            }
        }

        public Token LeftOperand
        {
            get => leftOperand;
            set
            {
                leftOperand = value;
                if (leftExpression != null)
                {
                    leftExpression.ParentExpression = null;
                    leftExpression = null;
                }
            }
        }

        public Token RightOperand
        {
            get => rightOperand;
            set
            {
                rightOperand = value;
                if (rightExpression != null)
                {
                    rightExpression.ParentExpression = null;
                    rightExpression = null;
                }
            }
        }

        public Expression LeftExpression
        {
            get => leftExpression;
            set
            {
                leftExpression = value;
                leftExpression.ParentExpression = this;
                leftOperand = null;
            }
        }

        public Expression RightExpression
        {
            get => rightExpression;
            set
            {
                rightExpression = value;
                rightExpression.ParentExpression = this;
                rightOperand = null;
            }
        }

        public Token Operator { get; set; }

        public bool IsUnary { get; set; }

        public Expression NonUnaryParent
        {
            get
            {
                var curr = ParentExpression;
                while (curr != null && curr.IsUnary)
                    curr = curr.ParentExpression;
                return curr;
            }
        }
    }
}
