using System;

namespace ExpressionEvaluator
{
    public enum TokenOption
    {
        SYMBOL, 
        ADD, 
        SUBTRACT, 
        MULTIPLY, 
        DIVIDE, 
        OPEN_PAREN, 
        CLOSE_PAREN
    }

    public static class Language
    {
        public static int GetPrecedence(TokenOption token)
        {
            if (token == TokenOption.DIVIDE || token == TokenOption.MULTIPLY)
                return 1;
            else if (token == TokenOption.ADD || token == TokenOption.SUBTRACT)
                return 0;
            else
                return -1;
        }

        public static bool IsLegalNextToken(TokenOption tokenType, TokenOption nextTokenType)
        {
            if (tokenType == TokenOption.ADD || tokenType == TokenOption.SUBTRACT || 
                tokenType == TokenOption.MULTIPLY || tokenType == TokenOption.DIVIDE || 
                tokenType == TokenOption.OPEN_PAREN)
                return nextTokenType == TokenOption.SYMBOL ||
                    nextTokenType == TokenOption.ADD ||
                    nextTokenType == TokenOption.SUBTRACT ||
                    nextTokenType == TokenOption.OPEN_PAREN;

            else if (tokenType == TokenOption.CLOSE_PAREN || tokenType == TokenOption.SYMBOL)
                return nextTokenType == TokenOption.ADD ||
                    nextTokenType == TokenOption.SUBTRACT ||
                    nextTokenType == TokenOption.MULTIPLY ||
                    nextTokenType == TokenOption.DIVIDE ||
                    nextTokenType == TokenOption.CLOSE_PAREN;

            else
                throw new NotImplementedException();
        }

        public static bool IsLegalStartToken(TokenOption tokenType)
        {
            return tokenType == 
                TokenOption.SYMBOL || 
                tokenType == TokenOption.ADD || 
                tokenType == TokenOption.SUBTRACT || 
                tokenType == TokenOption.OPEN_PAREN;
        }

        public static bool IsLegalEndToken(TokenOption tokenType)
        {
            return tokenType == TokenOption.SYMBOL || tokenType == TokenOption.CLOSE_PAREN;
        }

        public static bool IsOperator(char c) => c == '+' || c == '-' || c == '*' || c == '/';

        public static bool IsParen(char c) => c == '(' || c == ')';
    }
}
