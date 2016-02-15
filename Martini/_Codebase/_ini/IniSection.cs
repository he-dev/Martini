using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

namespace Martini
{
    [DebuggerDisplay("Name = {Name}")]
    public class IniSection : IniElement
    {
        internal IniSection(Sentence section) : base(section) { }

        public IEnumerable<IniProperty> this[string name] =>
            Sentence.Contents().Properties()
                .Where(x => x.PropertyToken() == name)
                .Select(x => new IniProperty(x));

        public List<IniComment> Comments => Sentence.Comments().Select(x => new IniComment(x)).ToList();

        public string Name => Sentence.Tokens.SectionToken();

        public IEnumerable<IniProperty> Properties => Sentence.Contents().Properties().Select(x => new IniProperty(x));

        public IniProperty AddProperty(string name, string value, bool allowDuplicateProperties = false)
        {
            var newProperty = PropertyFactory.CreateProperty(name, value);

            var properties = Sentence.Contents().Properties().ToList();

            if (!allowDuplicateProperties)
            {
                var propertyExists = properties.Any(t => t.Tokens.PropertyToken() == name);
                if (propertyExists)
                {
                    throw new PropertyExistsException
                    {
                        Section = Name,
                        Properety = name
                    };
                }
            }

            properties.Last().Next = newProperty;

            return new IniProperty(newProperty);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return ((IEnumerable<IniProperty>)result).Any();
        }
    }
}