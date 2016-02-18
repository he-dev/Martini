using System.Collections.Generic;
using System.Text;

namespace Martini
{
    internal class IniRenderer
    {
        public static string Render(Sentence globalSection, FormattingOptions options = FormattingOptions.None)
        {
            var iniBuilder = new StringBuilder();
            foreach (var section in globalSection.After.Sections())
            {
                var sectionText = RenderSectionContents(section, options);
                iniBuilder.Append(sectionText);
            }
            var ini = iniBuilder.ToString();
            return ini;
        }

        internal static string RenderSectionContents(Sentence section, FormattingOptions options)
        {
            var sectionBuilder = new StringBuilder();

            if (options.HasFlag(FormattingOptions.BlankLineBeforeSection))
            {
                sectionBuilder.AppendLine();
            }

            if (section.SectionToken() != Grammar.GlobalSectionName)
            {
                sectionBuilder.Append(RenderComments(section.Comments(), options));
                sectionBuilder.Append(RenderSection(section, options));
            }
            sectionBuilder.Append(RenderProperties(section.Contents(), options));

            var sectionText = sectionBuilder.ToString();
            return sectionText;
        }

        internal static string RenderComments(IEnumerable<Sentence> comments, FormattingOptions options)
        {
            var commentBuilder = new StringBuilder();

            foreach (var comment in comments)
            {
                commentBuilder.Append(comment.Tokens.CommentIndicaotrToken());
                if (options.HasFlag(FormattingOptions.SpaceAfterCommentIndicator))
                {
                    commentBuilder.Append(Grammar.Space);
                }
                commentBuilder.AppendLine(comment.Tokens.CommentToken());
            }

            var commentText = commentBuilder.ToString();
            return commentText;
        }

        internal static string RenderSection(Sentence section, FormattingOptions options)
        {
            var sectionBuilder = new StringBuilder();

            sectionBuilder
                .Append(section.Tokens[0])
                .Append(section.Tokens[1])
                .AppendLine(section.Tokens[2]);

            var sectionText = sectionBuilder.ToString();
            return sectionText;
        }

        internal static string RenderProperties(IEnumerable<Sentence> properties, FormattingOptions options)
        {
            var propertyBuilder = new StringBuilder();

            foreach (var property in properties)
            {
                var comments = property.Comments();
                propertyBuilder.Append(RenderComments(comments, options));

                propertyBuilder.Append(property.Tokens.PropertyToken());

                if (options.HasFlag(FormattingOptions.SpaceBeforePropertyValueDelimiter))
                {
                    propertyBuilder.Append(Grammar.Space);
                }

                propertyBuilder.Append(property.Tokens.PropertyValueDelimiterToken());

                if (options.HasFlag(FormattingOptions.SpaceAfterPropertyValueDelimiter))
                {
                    propertyBuilder.Append(Grammar.Space);
                }

                propertyBuilder.AppendLine(property.Tokens.ValueToken());
            }

            var propertyText = propertyBuilder.ToString();
            return propertyText;
        }
    }
}