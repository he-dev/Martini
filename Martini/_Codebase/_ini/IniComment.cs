using System.Diagnostics;

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
    }
}