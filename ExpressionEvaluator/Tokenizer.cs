using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluator
{
    /// <summary>
    /// Produces a series of tokens, out of a mathematical expression, 
    /// that can be fed to a parser.
    /// </summary>
    public class Tokenizer
    {
        protected IList<Token> tokens;
        protected string expression;

        public Tokenizer(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("No expression is given.", nameof(expression));
            this.expression = expression;
            tokens = new List<Token>();
        }

        /// <summary>
        /// Analyzes an expression to tokens that can be later processed.
        /// </summary>
        public void Tokenize()
        {
            void FlushToTokens(StringBuilder strBuilder)
            {
                string value = strBuilder.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    AddToken(value);
                    strBuilder.Clear();
                }
            }

            int lastPosition = -1;

            void AddToken(string value)
            {
                tokens.Add(new Token(value, lastPosition));
                lastPosition = -1;
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];
                if (Language.IsOperator(c) || Language.IsParen(c))
                {
                    FlushToTokens(sb);
                    lastPosition = i;
                    AddToken(c.ToString());
                    continue;
                }
                else if (c == ' ')
                {
                    FlushToTokens(sb);
                    continue;
                }
                else
                {
                    sb.Append(c);
                    if (lastPosition == -1)
                        lastPosition = i;
                }
            }
            FlushToTokens(sb);
        }

        public IList<Token> Tokens => tokens;
    }
}
