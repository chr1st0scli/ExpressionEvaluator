using ExpressionEvaluator.Exceptions;
using System;
using System.Collections.Generic;

namespace ExpressionEvaluator.Parsing
{
    public class Parser
    {
        protected Tokenizer tokenizer;

        public Parser(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("No expression given.", nameof(expression));
            tokenizer = new Tokenizer(expression);
        }

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

                if (token.TokenType == TokenOption.SYMBOL)
                    currExpr.AddOperand(token);
                else if (token.TokenType == TokenOption.OPEN_PAREN)
                {
                    unmatchedParens++;
                    if (parenContExprs == null)
                        parenContExprs = new List<Expression>();
                    parenContExprs.Add(currExpr);
                    currExpr = new Expression();
                    tmpCurrExpr?.AddExpression(currExpr);
                }
                else if (token.TokenType == TokenOption.CLOSE_PAREN)
                {
                    unmatchedParens--;
                    if (parenContExprs == null || unmatchedParens < 0 || unmatchedParens >= parenContExprs.Count)
                        throw new UnmatchedParenthesisException();
                    currExpr = parenContExprs[unmatchedParens];
                    parenContExprs.RemoveAt(unmatchedParens);
                }
                else if (token.IsOperator)
                {
                    if (prevToken == null 
                        || (prevToken != null 
                            && (prevToken.IsOperator || prevToken.TokenType == TokenOption.OPEN_PAREN))
                        && (token.TokenType == TokenOption.ADD || token.TokenType == TokenOption.SUBTRACT))
                    {
                        currExpr = new Expression() { IsUnary = true };
                        tmpCurrExpr?.AddExpression(currExpr);
                    }
                    else if (currExpr.Operator != null)
                    {
                        if (currExpr.IsUnary)
                        {
                            if (currExpr == currExpr.NonUnaryParent)
                                currExpr = currExpr.MoveInNewBelow();
                            else
                                currExpr = currExpr.NonUnaryParent;
                        }

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
