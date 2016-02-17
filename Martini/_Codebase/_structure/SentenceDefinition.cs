using System.Diagnostics;

namespace Martini
{
    [DebuggerDisplay("Type = {SentenceType}")]
    internal class SentenceDefinition
    {
        public SentenceType SentenceType { get; set; }

        // specifies the combinations of tokens that let us to identify a sentence
        public TokenType[][] AllowedTokenTypes { get; set; }

        // spefies full sentence tokens
        public TokenType[] ExactTokenTypes { get; set; }
    }
}