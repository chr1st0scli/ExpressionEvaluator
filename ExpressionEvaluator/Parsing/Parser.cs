using ExpressionEvaluator.Exceptions;
using System;
using System.Collections.Generic;

namespace ExpressionEvaluator.Parsing
{
    /// <summary>
    /// Builds an expression parse tree, out of a sequence of tokens, which makes sense to an evaluator.
    /// </summary>
    public class Parser
    {
        protected Tokenizer tokenizer;

        public Parser(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("No expression given.", nameof(expression));
            tokenizer = new Tokenizer(expression);
        }

        /// <summary>
        /// Builds an expression tree to be evaluated. Expressions with higher precedence are nearest 
        /// to the bottom of the tree, while expressions with lower precedence are above and must be 
        /// evaluated last. Binary expressions have two operands (left and right), while unary expressions 
        /// only have a left part. The tree is built in a left associative manner.
        /// </summary>
        /// <returns>The top (or root) expression of the tree that must be evaluated last.</returns>
        /// <exception cref="ExpressionEvaluator.Exceptions.IllegalTokenException">Thrown if a token is not expected where it occurs.</exception>
        /// <exception cref="ExpressionEvaluator.Exceptions.UnmatchedParenthesisException">Thrown if an opening or closing parenthesis is not matched.</exception>
        public Expression Parse()
        {
            tokenizer.Tokenize();
            CheckBoundaryTokens();

            int unmatchedParens = 0;
            Token prevToken = null;
            Expression currExpr = new Expression();
            List<Expression> parenContExprs = null;

            foreach (var token in tokenizer.Tokens)
            {
                //Check for legal next tokens
                if (prevToken != null && !Language.IsLegalNextToken(prevToken.TokenType, token.TokenType))
                    throw new IllegalTokenException(token);
                
                var tmpCurrExpr = currExpr;

                if (token.TokenType == TokenType.SYMBOL)
                    currExpr.AddOperand(token);
                else if (token.TokenType == TokenType.OPEN_PAREN)
                {
                    unmatchedParens++;
                    if (parenContExprs == null)
                        parenContExprs = new List<Expression>();
                    parenContExprs.Add(currExpr);
                    currExpr = new Expression();
                    tmpCurrExpr.AddExpression(currExpr);
                }
                else if (token.TokenType == TokenType.CLOSE_PAREN)
                {
                    unmatchedParens--;
                    if (parenContExprs == null || unmatchedParens < 0)
                        throw new UnmatchedParenthesisException();
                    currExpr = parenContExprs[unmatchedParens];
                    parenContExprs.RemoveAt(unmatchedParens);
                }
                else if (token.IsOperator)
                {
                    if ((prevToken == null
                        || (prevToken != null && (prevToken.IsOperator || prevToken.TokenType == TokenType.OPEN_PAREN)))
                        && (token.TokenType == TokenType.ADD || token.TokenType == TokenType.SUBTRACT))
                    {
                        currExpr = new Expression() { IsUnary = true };
                        tmpCurrExpr.AddExpression(currExpr);
                    }
                    else if (currExpr.Operator != null)
                    {
                        if (currExpr.IsUnary)
                            currExpr = currExpr.NonUnaryParent;

                        if (currExpr.Operator != null)
                        {
                            if (token.CompareTo(currExpr.Operator) == 0)
                                currExpr = currExpr.MoveInNewBelow();
                            else if (token.CompareTo(currExpr.Operator) > 0)
                                currExpr = currExpr.SplitInNewBelow();
                            else if (token.CompareTo(currExpr.Operator) < 0)
                            {
                                if (currExpr.ParentExpression != null)
                                {
                                    if (unmatchedParens > 0 && parenContExprs[unmatchedParens - 1] == currExpr.ParentExpression)
                                        currExpr = currExpr.MoveInNewBelow();
                                    else
                                        currExpr = currExpr.ParentExpression.MoveInNewBelow();
                                }
                                else
                                    currExpr = currExpr.MoveInNewBelow();
                            }
                        }
                    }
                    currExpr.Operator = token;
                }
                prevToken = token;
            }

            //Check for unmatched parentheses
            if (unmatchedParens != 0)
                throw new UnmatchedParenthesisException();

            return currExpr.TopParentExpression;
        }

        private void CheckBoundaryTokens()
        {
            //Check for legal start and end tokens
            if (tokenizer.Tokens.Count > 0)
            {
                if (!Language.IsLegalStartToken(tokenizer.Tokens[0].TokenType))
                    throw new IllegalTokenException(tokenizer.Tokens[0]);
                if (!Language.IsLegalEndToken(tokenizer.Tokens[tokenizer.Tokens.Count - 1].TokenType))
                    throw new IllegalTokenException(tokenizer.Tokens[tokenizer.Tokens.Count - 1]);
            }
        }
    }
}
