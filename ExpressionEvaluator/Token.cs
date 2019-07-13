using System;

namespace ExpressionEvaluator
{
    [Serializable]
    public class Token : IComparable
    {
        protected string value;
        protected int position;
        protected TokenOption tokenType;

        public Token(string value, int position = 0)
        {
            this.value = value;
            this.position = position;

            switch (value)
            {
                case "+":
                    tokenType = TokenOption.ADD;
                    break;
                case "-":
                    tokenType = TokenOption.SUBTRACT;
                    break;
                case "*":
                    tokenType = TokenOption.MULTIPLY;
                    break;
                case "/":
                    tokenType = TokenOption.DIVIDE;
                    break;
                case "(":
                    tokenType = TokenOption.OPEN_PAREN;
                    break;
                case ")":
                    tokenType = TokenOption.CLOSE_PAREN;
                    break;
                default:
                    tokenType = TokenOption.SYMBOL;
                    break;
            }
        }

        public TokenOption TokenType => tokenType;

        public string Value => value;

        public int Position => position;

        public bool IsOperator => tokenType == TokenOption.ADD || tokenType == TokenOption.SUBTRACT || tokenType == TokenOption.MULTIPLY || tokenType == TokenOption.DIVIDE;

        public int CompareTo(object obj) => Language.GetPrecedence(tokenType) - Language.GetPrecedence(((Token)obj).tokenType);
    }
}
