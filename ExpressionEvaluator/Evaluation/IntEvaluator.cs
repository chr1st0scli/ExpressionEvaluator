using System;

namespace ExpressionEvaluator.Evaluation
{
    public class IntEvaluator : Evaluator<int>
    {
        public IntEvaluator()
        {
            ValueForToken = GetValueDefaultAccessor();
        }

        public override Func<Token, int> GetValueDefaultAccessor() => token => int.Parse(token.Value);

        public override Token Add(Token token1, Token token2)
        {
            if (token1 == null && token2 != null)
                return new Token((+ValueForToken(token2)).ToString());
            else if (token2 == null && token1 != null)
                return new Token((+ValueForToken(token1)).ToString());
            else
                return new Token((ValueForToken(token1) + ValueForToken(token2)).ToString());
        }

        public override Token Subtract(Token token1, Token token2)
        {
            if (token1 == null && token2 != null)
                return new Token((-ValueForToken(token2)).ToString());
            else if (token2 == null && token1 != null)
                return new Token((-ValueForToken(token1)).ToString());
            else
                return new Token((ValueForToken(token1) - ValueForToken(token2)).ToString());
        }

        public override Token Multiply(Token token1, Token token2)
        {
            return new Token((ValueForToken(token1) * ValueForToken(token2)).ToString());
        }

        public override Token Divide(Token token1, Token token2)
        {
            return new Token((ValueForToken(token1) / ValueForToken(token2)).ToString());
        }
    }
}
