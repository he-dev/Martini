namespace Martini.Collections
{
    internal class DelimiterDictionary : BiDictionary<TokenType, char>
    {
        public DelimiterDictionary() : base("TokenTypes", "Delimiters") { }
    }
}