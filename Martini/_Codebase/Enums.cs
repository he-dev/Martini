using System;

namespace Martini
{
    public enum DuplicateSectionHandling
    {
        Allow,
        Disallow,
        Merge
    }

    public enum DuplicatePropertyHandling
    {
        Allow,
        Disallow,
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
        Throw,
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
}