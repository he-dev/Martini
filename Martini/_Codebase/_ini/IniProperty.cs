using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Martini;

namespace Martini
{
    [DebuggerDisplay("Name = {Name} Value = {Value}")]
    public class IniProperty : IniElement
    {
        internal IniProperty(Sentence property, IniFile iniFile) : base(property, iniFile) { }

        public List<IniComment> Comments
        {
            get { return Sentence.Comments().Select(x => new IniComment(x, IniFile)).ToList(); }
        }

        public string Name => Sentence.Tokens.PropertyToken();

        public string Value
        {
            get { return Sentence.Tokens.ValueToken(); }
            set { Sentence.Tokens.ValueToken().Value = value; }
        }
    }
}