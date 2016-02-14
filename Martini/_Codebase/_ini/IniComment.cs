using System.Diagnostics;
using Martini._data;

namespace Martini._ini
{
    [DebuggerDisplay("Text = {Text}")]
    public class IniComment
    {
        private readonly Sentence _comment;

        internal IniComment(Sentence comment)
        {
            _comment = comment;
        }

        public string Text
        {
            get { return _comment.Tokens.CommentToken(); }
            set { _comment.Tokens.CommentToken().Value = value; }
        }
    }
}