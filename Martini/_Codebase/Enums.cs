using System;
using System.Security.AccessControl;

namespace Martini
{
    public enum DuplicateSectionHandling
    {
        Disallow,
        Allow,
        Merge
    }

    public enum DuplicatePropertyHandling
    {
        Disallow,
        Allow,
        KeepFirst,
        KeepLast,
        Rename
    }

    public enum UnqotedValueSpaceHandling
    {
        Trim,
        Keep
    }

    public enum InvalidLineHandling
    {
        Disallow,
        Ignore,
        Keep
    }

    [Flags]
    public enum FormattingOptions
    {
        None,
        BlankLineBeforeSection = 1,
        SpaceAfterCommentIndicator = 2,
        SpaceBeforePropertyValueDelimiter = 4,
        SpaceAfterPropertyValueDelimiter = 8,
        QuoteValuesWithSpaces = 16,
    }

    internal enum SentenceType
    {
        Uninitialized,
        Blank,
        Comment,
        Section,
        Property,
        Invalid
    }

    internal enum TokenType
    {
        Uninitialized,
        StartOfLine,
        EndOfLine,
        LeftSectionDelimiter,
        RightSectionDelimiter,
        ProperetyValueDelimiter,
        Text,
        Section,
        Property,
        Value,
        CommentIndicator,
        Comment,
        QuotationMark,
        EscapeSequence,
    }

    public enum SectionDelimiter
    {
        /// <summary>
        /// Brackets []
        /// </summary>
        [SectionDelimiter(Left = '[', Right = ']')]
        SquareBrackets,

        /// <summary>
        /// Parentheses {}
        /// </summary>
        [SectionDelimiter(Left = '{', Right = '}')]
        RoundBrackets,

        /// <summary>
        /// Braces ()
        /// </summary>
        [SectionDelimiter(Left = '(', Right = ')')]
        CurlyBrackets,

        /// <summary>
        /// Chevrons 
        /// </summary>
        [SectionDelimiter(Left = '<', Right = '>')]
        AngleBrackets
    }

    public enum PropertyValueDelimiter
    {
        /// <summary>
        /// =
        /// </summary>
        [PropertyValueDelimiter(Delimiter = '=')]
        EqualSign,

        /// <summary>
        /// :
        /// </summary>
        [PropertyValueDelimiter(Delimiter = ';')]
        Colon
    }

    public enum CommentIndicator
    {
        /// <summary>
        /// ;
        /// </summary>
        [CommentIndicator(Delimiter = ';')]
        Semicolon,

        /// <summary>
        /// #
        /// </summary>
        [CommentIndicator(Delimiter = '#')]
        NumberSign
    }
}