using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using Martini._data;
using Martini._factories;

namespace Martini._ini
{
    [DebuggerDisplay("Name = {Name}")]
    public class IniSection : DynamicObject
    {
        private readonly Sentence _section;

        internal IniSection(Sentence section)
        {
            Debug.Assert(section.Tokens.SectionToken() != null);
            _section = section;
        }

        public IEnumerable<IniProperty> this[string name] =>
            _section.Contents().Properties()
                .Where(x => Extensions.PropertyToken((Sentence) x) == name)
                .Select(x => new IniProperty(x));

        public List<IniComment> Comments => _section.Comments().Select(x => new IniComment(x)).ToList();

        public string Name => _section.Tokens.SectionToken();

        public IEnumerable<IniProperty> Properties => _section.Contents().Properties().Select(x => new IniProperty(x));

        public IniProperty AddProperty(string name, string value, bool allowDuplicateProperties = false)
        {
            var newProperty = PropertyFactory.CreateProperty(name, value);

            var properties = _section.Contents().Properties().ToList();

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