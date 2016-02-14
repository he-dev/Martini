using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Martini._data;

namespace Martini._ini
{
    [DebuggerDisplay("Name = {Name} Value = {Value}")]
    public class IniProperty
    {
        private readonly Sentence _property;

        internal IniProperty(Sentence property)
        {
            _property = property;
        }

        public List<IniComment> Comments
        {
            get { return _property.Comments().Select(x => new IniComment(x)).ToList(); }
        }

        public string Name => _property.Tokens.PropertyToken();

        public string Value
        {
            get { return _property.Tokens.ValueToken(); }
            set { _property.Tokens.ValueToken().Value = value; }
        }
    }
}