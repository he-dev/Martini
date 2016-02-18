using System;
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

        public static bool operator ==(IniComment iniComment, string text)
        {
            return
                !ReferenceEquals(iniComment, null) &&
                iniComment.Text.Equals(text, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator !=(IniComment iniComment, string text)
        {
            return !(iniComment == text);
        }
    }
}