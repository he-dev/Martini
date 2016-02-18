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
    }
}