using System.Collections.Generic;

namespace Martini.Collections
{
    internal class DelimiterDictionary : BiDictionary<TokenType, char>
    {
        public DelimiterDictionary() : base("TokenTypes", "Delimiters") { }

        public IReadOnlyDictionary<TokenType, char> TokenTypes => _K1K2;
        public IReadOnlyDictionary<char, TokenType> Delimiters => _K2K1;
    }
}