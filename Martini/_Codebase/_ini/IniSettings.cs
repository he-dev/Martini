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
    }
}