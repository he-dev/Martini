using System.Collections.Generic;

namespace Martini
{
    internal class CommentFactory
    {
        public static Sentence CreateComment(string text)
        {
            var section = new Sentence
            {
                Type = SentenceType.Section,
                Tokens = new List<Token>
                {
                    new Token(TokenType.CommentIndicator),
                    new Token(TokenType.Comment, text),
                }
            };
            return section;
        }
    }
}