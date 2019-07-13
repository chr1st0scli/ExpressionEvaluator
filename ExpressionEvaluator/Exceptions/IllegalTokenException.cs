using System;

namespace ExpressionEvaluator.Exceptions
{
    public class IllegalTokenException : Exception
    {
        protected Token token;

        public IllegalTokenException(Token token)
            : base($"Illegal token {token.Value} at position {token.Position}.")
        {
            this.token = token;
        }

        public Token IllegalToken => token;
    }
}
