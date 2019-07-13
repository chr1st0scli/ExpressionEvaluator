﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluator
{
    public class Tokenizer
    {
        protected IList<Token> tokens;
        protected string expression;

        public Tokenizer(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("", nameof(expression));
            this.expression = expression;
            tokens = new List<Token>();
        }

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