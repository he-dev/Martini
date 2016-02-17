using System.Collections.Generic;

namespace Martini
{
    internal class CommentFactory
    {
        public static Sentence CreateComment(string text, dynamic delimiters)
        {
            var section = new Sentence
            {
                Type = SentenceType.Comment,
                Tokens = new List<Token>
                {
                    new Token(TokenType.CommentIndicator, delimiters.TokenTypes[TokenType.CommentIndicator]),
                    new Token(TokenType.Comment, text),
                }
            };
            return section;
        }
    }
}