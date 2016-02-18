using System.Collections.Generic;
using Martini.Collections;

namespace Martini
{
    internal class PropertyFactory
    {
        public static Sentence CreateProperty(string name, string value, DelimiterDictionary delimiters)
        {
            var property = new Sentence
            {
                Type = SentenceType.Property,
                Tokens = new List<Token>
                {
                    new Token(TokenType.Property, name),
                    new Token(TokenType.ProperetyValueDelimiter, delimiters.TokenTypes[TokenType.ProperetyValueDelimiter]),
                    new Token(TokenType.Value, value),
                }
            };
            return property;
        }
    }
}