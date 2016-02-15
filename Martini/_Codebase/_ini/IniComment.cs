using System.Diagnostics;

namespace Martini
{
    [DebuggerDisplay("Text = {Text}")]
    public class IniComment : IniElement
    {
        internal IniComment(Sentence comment) : base(comment) { }

        public string Text
        {
            get { return Sentence.Tokens.CommentToken(); }
            set { Sentence.Tokens.CommentToken().Value = value; }
        }
    }
}