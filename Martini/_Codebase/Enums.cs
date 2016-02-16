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
    public enum Formattings
    {
        None,
        EmptyLineBeforeSection,
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
        AngleBrrackets
    }

    public enum PropertyValueDelimiter
    {
        /// <summary>
        /// =
        /// </summary>
        [PropertyValueDelimiter(Deimiter = '=')]
        EqualSign,

        /// <summary>
        /// :
        /// </summary>
        [PropertyValueDelimiter(Deimiter = ';')]
        Colon
    }

    public enum CommentIndicator
    {
        /// <summary>
        /// ;
        /// </summary>
        [CommentIndicator(Deimiter = ';')]
        Semicolon,

        /// <summary>
        /// #
        /// </summary>
        [CommentIndicator(Deimiter = '#')]
        NumberSign
    }
}