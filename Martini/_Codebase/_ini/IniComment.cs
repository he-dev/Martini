using System.Diagnostics;
using System.Text;

namespace Martini
{
    [DebuggerDisplay("Text = {Text}")]
    public class IniComment : IniElement
    {
        internal IniComment(Sentence comment, IniFile iniFile) : base(comment, iniFile) { }

        public string Text
        {
            get { return Sentence.Tokens.CommentToken(); }
            set { Sentence.Tokens.CommentToken().Value = value; }
        }

        internal override string Render(FormattingOptions formattingOptions)
        {
            var text = new StringBuilder().Append(Sentence.Tokens.CommentIndicaotrToken());
            if (formattingOptions.HasFlag(FormattingOptions.SpaceAfterCommentIndicator))
            {
                text.Append(Grammar.Space);
            }
            text.Append(Sentence.Tokens.CommentToken());

            return text.ToString();
        }
    }
}