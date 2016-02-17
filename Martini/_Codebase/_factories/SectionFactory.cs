using System.Collections.Generic;

namespace Martini
{
    internal class SectionFactory
    {
        public static Sentence CreateSection(string name, dynamic delimiters)
        {
            var section = new Sentence
            {
                Type = SentenceType.Section,
                Tokens = new List<Token>
                {
                    new Token(TokenType.LeftSectionDelimiter, delimiters.TokenTypes[TokenType.LeftSectionDelimiter]),
                    new Token(TokenType.Section, name),
                    new Token(TokenType.RightSectionDelimiter, delimiters.TokenTypes[TokenType.RightSectionDelimiter]),
                }
            };
            return section;
        }
    }
}