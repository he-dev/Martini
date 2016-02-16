using Martini.Collections;

namespace Martini
{
    public class IniSettings
    {
        public DuplicateSectionHandling DuplicateSectionHandling { get; set; } = DuplicateSectionHandling.Disallow;
        public DuplicatePropertyHandling DuplicatePropertyHandling { get; set; } = DuplicatePropertyHandling.Disallow;

        public InvalidLineHandling InvalidLineHandling { get; set; } = InvalidLineHandling.Disallow;

        public SectionDelimiter SectionDelimiter { get; set; } = SectionDelimiter.SquareBrackets;
        public PropertyValueDelimiter PropertyValueDelimiter { get; set; } = PropertyValueDelimiter.EqualSign;
        public CommentIndicator CommentIndicator { get; set; } = CommentIndicator.Semicolon;

        internal BiDictionary<TokenType, char> Delimiters => new BiDictionary<TokenType, char>("TokenTypes", "Delimiters")
        {
            { TokenType.LeftSectionDelimiter, SectionDelimiter.Attribute<SectionDelimiterAttribute>().Left },
            { TokenType.RightSectionDelimiter, SectionDelimiter.Attribute<SectionDelimiterAttribute>().Right },
            { TokenType.ProperetyValueDelimiter, PropertyValueDelimiter.Attribute<PropertyValueDelimiterAttribute>().Delimiter },
            { TokenType.RightSectionDelimiter, CommentIndicator.Attribute<CommentIndicatorAttribute>().Delimiter },
        };
    }
}