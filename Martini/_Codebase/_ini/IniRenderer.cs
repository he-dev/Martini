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
                RenderSectionContents(section, options, iniBuilder);                
            }
            var ini = iniBuilder.ToString();
            return ini;
        }

        internal static void RenderSectionContents(Sentence section, FormattingOptions options, StringBuilder iniBuilder)
        {
            if (options.HasFlag(FormattingOptions.BlankLineBeforeSection))
            {
                iniBuilder.AppendLine();
            }

            if (section.SectionToken() != Grammar.GlobalSectionName)
            {
                RenderComments(section.Comments(), options, iniBuilder);
                RenderSection(section, options, iniBuilder);
            }
            RenderProperties(section.Contents(), options, iniBuilder);
        }

        internal static void RenderComments(IEnumerable<Sentence> comments, FormattingOptions options, StringBuilder iniBuilder)
        {
            foreach (var comment in comments)
            {
                iniBuilder.Append(comment.Tokens.CommentIndicaotrToken());
                if (options.HasFlag(FormattingOptions.SpaceAfterCommentIndicator))
                {
                    iniBuilder.Append(Grammar.Space);
                }
                iniBuilder.AppendLine(comment.Tokens.CommentToken());
            }
        }

        internal static void RenderSection(Sentence section, FormattingOptions options, StringBuilder iniBuilder)
        {
            if (iniBuilder.Length > 0)
            {
                iniBuilder.AppendLine();
            }
            iniBuilder
                .Append(section.Tokens[0])
                .Append(section.Tokens[1])
                .Append(section.Tokens[2]);
        }

        internal static void RenderProperties(IEnumerable<Sentence> properties, FormattingOptions options, StringBuilder iniBuilder)
        {
            foreach (var property in properties)
            {
                var comments = property.Comments();
                RenderComments(comments, options, iniBuilder);

                iniBuilder.AppendLine();
                iniBuilder.Append(property.Tokens.PropertyToken());

                if (options.HasFlag(FormattingOptions.SpaceBeforePropertyValueDelimiter))
                {
                    iniBuilder.Append(Grammar.Space);
                }

                iniBuilder.Append(property.Tokens.PropertyValueDelimiterToken());

                if (options.HasFlag(FormattingOptions.SpaceAfterPropertyValueDelimiter))
                {
                    iniBuilder.Append(Grammar.Space);
                }

                iniBuilder.Append(property.Tokens.ValueToken());
            }
        }
    }
}