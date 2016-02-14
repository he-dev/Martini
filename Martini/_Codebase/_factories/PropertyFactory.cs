using System.Collections.Generic;
using Martini._data;

namespace Martini._factories
{
    internal class PropertyFactory
    {
        public static Sentence CreateProperty(string name, string value)
        {
            var property = new Sentence
            {
                Type = SentenceType.Property,
                Tokens = new List<Token>
                {
                    new Token(TokenType.Property, name),
                    new Token(TokenType.ProperetyValueDelimiter),
                    new Token(TokenType.Value),
                }
            };
            return property;
        }
    }
}