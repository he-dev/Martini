using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Martini;

namespace Martini
{
    [DebuggerDisplay("Name = {Name} Value = {Value}")]
    public class IniProperty : IniElement
    {
        internal IniProperty(Sentence property, IniFile iniFile) : base(property, iniFile) { }

        public IEnumerable<IniComment> Comments
        {
            get { return Sentence.Comments().Select(x => new IniComment(x, IniFile)); }
        }

        public string Name => Sentence.Tokens.PropertyToken();

        public string Value
        {
            get { return Sentence.Tokens.ValueToken(); }
            set { Sentence.Tokens.ValueToken().Value = value; }
        }

        public IniComment AddComment(string text)
        {
            var comment = CommentFactory.CreateComment(text, IniFile.Delimiters);
            Sentence.Previous = comment;
            return new IniComment(comment, IniFile);
        }

        internal override string Render(FormattingOptions formattingOptions)
        {
            var text = new StringBuilder();
            text.Append(Sentence.Tokens.PropertyToken());

            if (formattingOptions.HasFlag(FormattingOptions.SpaceBeforePropertyValueDelimiter))
            {
                text.Append(Grammar.Space);
            }

            text.Append(Sentence.Tokens.PropertyValueDelimiterToken());

            if (formattingOptions.HasFlag(FormattingOptions.SpaceAfterPropertyValueDelimiter))
            {
                text.Append(Grammar.Space);
            }

            text.Append(Sentence.Tokens.ValueToken());

            return text.ToString();
        }
    }
}