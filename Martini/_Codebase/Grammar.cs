using System.Collections.Generic;
using Martini.Collections;

namespace Martini
{
    internal class Grammar
    {
        public Grammar(IniSettings settings)
        {
            
        }

        // defines each type of sentence and the possible tokens
        public static readonly AutoKeyDictionary<SentenceType, SentenceDefinition> SentenceDefinitions =
            new AutoKeyDictionary<SentenceType, SentenceDefinition>(x => x.SentenceType)
            {
                new SentenceDefinition
                {
                    SentenceType = SentenceType.Blank,
                    AllowedTokenTypes = new[] {new TokenType[] {}},
                    ExactTokenTypes = new TokenType[] {}
                },
                new SentenceDefinition
                {
                    SentenceType = SentenceType.Comment,
                    AllowedTokenTypes = new[]
                    {
                        new TokenType[] {TokenType.CommentIndicator, TokenType.Text},
                        new TokenType[] {TokenType.CommentIndicator},
                    },
                    ExactTokenTypes = new TokenType[] {TokenType.CommentIndicator, TokenType.Comment}
                },
                new SentenceDefinition
                {
                    SentenceType = SentenceType.Section,
                    AllowedTokenTypes = new[]
                    {
                        new TokenType[] { TokenType.LeftSectionDelimiter, TokenType.Text, TokenType.RightSectionDelimiter },
                    },
                    ExactTokenTypes = new TokenType[] {TokenType.LeftSectionDelimiter, TokenType.Section, TokenType.RightSectionDelimiter},
                },
                new SentenceDefinition
                {
                    SentenceType = SentenceType.Property,
                    AllowedTokenTypes = new[]
                    {
                        new TokenType[] {TokenType.Text, TokenType.ProperetyValueDelimiter, TokenType.Text},
                        new TokenType[] {TokenType.Text, TokenType.ProperetyValueDelimiter},
                    },
                    ExactTokenTypes = new TokenType[] {TokenType.Property, TokenType.ProperetyValueDelimiter, TokenType.Value},
                },
            };

        public static readonly string Space = ' '.ToString();

        public static readonly char Backslash = '\\';

        public static dynamic DelimiterTokenTypeMap = new BiDictionary<char, TokenType>("Delimiters", "TokenTypes")
        {
            {'[', TokenType.LeftSectionDelimiter},
            {']', TokenType.RightSectionDelimiter},
            {'=', TokenType.ProperetyValueDelimiter},
            {';', TokenType.CommentIndicator},
        };

        public static readonly HashSet<char> EscapeCharacters = new HashSet<char>
        {
            '\\',
            't',
            ';',
            '=',
            '"'
        };
    }
}