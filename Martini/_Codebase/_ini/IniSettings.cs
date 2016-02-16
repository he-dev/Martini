namespace Martini
{
    public class IniSettings
    {
        public DuplicateSectionHandling DuplicateSectionHandling { get; set; } = DuplicateSectionHandling.Disallow;
        public DuplicatePropertyHandling DuplicatePropertyHandling { get; set; } = DuplicatePropertyHandling.Disallow;
        public InvalidLineHandling InvalidLineHandling { get; set; } = InvalidLineHandling.Throw;
    }
}