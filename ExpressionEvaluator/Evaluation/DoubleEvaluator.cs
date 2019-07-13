using System;
using System.Globalization;

namespace ExpressionEvaluator.Evaluation
{
    public class DoubleEvaluator : Evaluator<double>
    {
        public DoubleEvaluator()
        {
            ValueForToken = GetValueDefaultAccessor();
        }

        public override Func<Token, double> GetValueDefaultAccessor() => token => double.Parse(token.Value, CultureInfo.InvariantCulture);

        public override Token Add(Token token1, Token token2)
        {
            if (token1 == null && token2 != null)
                return new Token((+ValueForToken(token2)).ToString(CultureInfo.InvariantCulture));
            else if (token2 == null && token1 != null)
                return new Token((+ValueForToken(token1)).ToString(CultureInfo.InvariantCulture));
            else
                return new Token((ValueForToken(token1) + ValueForToken(token2)).ToString(CultureInfo.InvariantCulture));
        }

        public override Token Subtract(Token token1, Token token2)
        {
            if (token1 == null && token2 != null)
                return new Token((-ValueForToken(token2)).ToString(CultureInfo.InvariantCulture));
            else if (token2 == null && token1 != null)
                return new Token((-ValueForToken(token1)).ToString(CultureInfo.InvariantCulture));
            else
                return new Token((ValueForToken(token1) - ValueForToken(token2)).ToString(CultureInfo.InvariantCulture));
        }

        public override Token Multiply(Token token1, Token token2)
        {
            return new Token((ValueForToken(token1) * ValueForToken(token2)).ToString(CultureInfo.InvariantCulture));
        }

        public override Token Divide(Token token1, Token token2)
        {
            return new Token((ValueForToken(token1) / ValueForToken(token2)).ToString(CultureInfo.InvariantCulture));
        }
    }
}
