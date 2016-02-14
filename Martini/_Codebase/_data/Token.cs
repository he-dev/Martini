using System;
using System.Diagnostics;

namespace Martini._data
{
    [DebuggerDisplay("Type = {Type} Token = {_token}")]
    internal class Token
    {
        private string _token;

        public Token(string token)
        {
            _token = token;
        }

        public Token(char token) : this(token.ToString()) { }

        public Token(TokenType tokenType) : this((char)Grammar.DelimiterTokenTypeMap.TokenTypes[tokenType])
        {
            Type = tokenType;
        }

        public Token(TokenType tokenType, string token) : this(token)
        {
            Type = tokenType;
        }

        public string Value { get { return _token; } set { _token = value; } }

        public Sentence Sentence { get; set; }

        public TokenType Type { get; set; } = TokenType.Text;

        public int FromColumn { get; set; } = -1;

        public int ToColumn => FromColumn + Length;

        public int Length => _token.Length;

        public static bool operator ==(Token x, string y)
        {
            return
                !ReferenceEquals(x, null)
                && x.ToString().Equals(y, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator !=(Token x, string y)
        {
            return !(x == y);
        }

        public static implicit operator string(Token token) => token?.ToString();

        public override string ToString() => _token;

        protected bool Equals(Token other) => string.Equals(_token, other._token);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Token)obj);
        }

        public override int GetHashCode() => _token?.GetHashCode() ?? 0;
    }
}