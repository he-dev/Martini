using Martini.Collections;

namespace Martini
{
    public class IniSettings
    {
        public DuplicateSectionHandling DuplicateSectionHandling { get; set; } = DuplicateSectionHandling.Disallow;
        public DuplicatePropertyHandling DuplicatePropertyHandling { get; set; } = DuplicatePropertyHandling.Disallow;

        public InvalidLineHandling InvalidLineHandling { get; set; } = InvalidLineHandling.Disallow;

        public SectionDelimiter SectionDelimiters { get; set; } = SectionDelimiter.SquareBrackets;
        public PropertyValueDelimiter PropertyValueDelimiter { get; set; } = PropertyValueDelimiter.EqualSign;
        public CommentIndicator CommentIndicator { get; set; } = CommentIndicator.Semicolon;

        internal dynamic Delimiters => new DelimiterDictionary()
        {
            { TokenType.LeftSectionDelimiter, SectionDelimiters.Attribute<SectionDelimiterAttribute>().Left },
            { TokenType.RightSectionDelimiter, SectionDelimiters.Attribute<SectionDelimiterAttribute>().Right },
            { TokenType.ProperetyValueDelimiter, PropertyValueDelimiter.Attribute<PropertyValueDelimiterAttribute>().Delimiter },
            { TokenType.CommentIndicator, CommentIndicator.Attribute<CommentIndicatorAttribute>().Delimiter },
        };
    }
}