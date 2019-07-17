using System;

namespace ExpressionEvaluator
{
    /// <summary>
    /// Represents the minimum element of the language.
    /// </summary>
    [Serializable]
    public class Token : IComparable<Token>
    {
        protected string value;
        protected int position;
        protected TokenType tokenType;

        public Token(string value, int position = 0)
        {
            this.value = value;
            this.position = position;

            switch (value)
            {
                case "+":
                    tokenType = TokenType.ADD;
                    break;
                case "-":
                    tokenType = TokenType.SUBTRACT;
                    break;
                case "*":
                    tokenType = TokenType.MULTIPLY;
                    break;
                case "/":
                    tokenType = TokenType.DIVIDE;
                    break;
                case "(":
                    tokenType = TokenType.OPEN_PAREN;
                    break;
                case ")":
                    tokenType = TokenType.CLOSE_PAREN;
                    break;
                default:
                    tokenType = TokenType.SYMBOL;
                    break;
            }
        }

        public TokenType TokenType => tokenType;

        public string Value => value;

        public int Position => position;

        public bool IsOperator => tokenType == TokenType.ADD || tokenType == TokenType.SUBTRACT || tokenType == TokenType.MULTIPLY || tokenType == TokenType.DIVIDE;

        public int CompareTo(Token token) => Language.GetPrecedence(tokenType) - Language.GetPrecedence(token.tokenType);
    }
}
