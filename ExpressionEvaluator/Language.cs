using System;

namespace ExpressionEvaluator
{
    public enum TokenType
    {
        SYMBOL, 
        ADD, 
        SUBTRACT, 
        MULTIPLY, 
        DIVIDE, 
        OPEN_PAREN, 
        CLOSE_PAREN
    }

    /// <summary>
    /// Implements the language's rules.
    /// </summary>
    public static class Language
    {
        public static int GetPrecedence(TokenType token)
        {
            if (token == TokenType.DIVIDE || token == TokenType.MULTIPLY)
                return 1;
            else if (token == TokenType.ADD || token == TokenType.SUBTRACT)
                return 0;
            else
                return -1;
        }

        public static bool IsLegalNextToken(TokenType tokenType, TokenType nextTokenType)
        {
            if (tokenType == TokenType.ADD || tokenType == TokenType.SUBTRACT || 
                tokenType == TokenType.MULTIPLY || tokenType == TokenType.DIVIDE || 
                tokenType == TokenType.OPEN_PAREN)
                return nextTokenType == TokenType.SYMBOL ||
                    nextTokenType == TokenType.ADD ||
                    nextTokenType == TokenType.SUBTRACT ||
                    nextTokenType == TokenType.OPEN_PAREN;

            else if (tokenType == TokenType.CLOSE_PAREN || tokenType == TokenType.SYMBOL)
                return nextTokenType == TokenType.ADD ||
                    nextTokenType == TokenType.SUBTRACT ||
                    nextTokenType == TokenType.MULTIPLY ||
                    nextTokenType == TokenType.DIVIDE ||
                    nextTokenType == TokenType.CLOSE_PAREN;

            else
                throw new NotImplementedException();
        }

        public static bool IsLegalStartToken(TokenType tokenType)
        {
            return tokenType == 
                TokenType.SYMBOL || 
                tokenType == TokenType.ADD || 
                tokenType == TokenType.SUBTRACT || 
                tokenType == TokenType.OPEN_PAREN;
        }

        public static bool IsLegalEndToken(TokenType tokenType)
        {
            return tokenType == TokenType.SYMBOL || tokenType == TokenType.CLOSE_PAREN;
        }

        public static bool IsOperator(char c) => c == '+' || c == '-' || c == '*' || c == '/';

        public static bool IsParen(char c) => c == '(' || c == ')';
    }
}
