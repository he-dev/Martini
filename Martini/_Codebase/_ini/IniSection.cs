using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Martini
{
    [DebuggerDisplay("Name = {Name}")]
    public class IniSection : IniElement
    {
        internal IniSection(Sentence section, IniFile iniFile) : base(section, iniFile) { }

        public IniProperty this[string name] =>
            Sentence.Properties()
                .Where(x => x.PropertyToken() == name)
                .Select(x => new IniProperty(x, IniFile))
                .SingleOrDefault();

        public string Name => Sentence.Tokens.SectionToken();

        public IEnumerable<IniComment> Comments => Sentence.Comments().Select(x => new IniComment(x, IniFile));

        public IEnumerable<IniProperty> Properties => Sentence.Properties().Select(x => new IniProperty(x, IniFile));

        public IniComment AddComment(string text)
        {
            var comment = CommentFactory.CreateComment(text, IniFile.Delimiters);
            Sentence.Previous = comment;
            return new IniComment(comment, IniFile);
        }

        public IniProperty AddProperty(string name, string value)
        {
            var properties = Properties.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            var propertyExists = properties.Any();

            var addProperty = new Func<IniProperty>(() =>
            {
                var property = PropertyFactory.CreateProperty(name, value, IniFile.Delimiters);
                var iniProperty = new IniProperty(property, IniFile);

                var contents = Sentence.Properties().ToList();
                var hasContents = contents.Any();
                if (!hasContents)
                {
                    Sentence.Next = iniProperty.Sentence;
                }
                else
                {
                    contents.Last().Next = iniProperty.Sentence;
                }
                return iniProperty;
            });

            if (IniFile.DuplicatePropertyHandling == DuplicatePropertyHandling.Disallow)
            {
                if (propertyExists)
                {
                    throw new DuplicatePropertiesException();
                }
                return addProperty();
            }

            if (IniFile.DuplicatePropertyHandling == DuplicatePropertyHandling.Allow)
            {
                return addProperty();
            }

            if (IniFile.DuplicatePropertyHandling == DuplicatePropertyHandling.KeepFirst)
            {
                if (propertyExists)
                {
                    return properties.Single();
                }
                return addProperty();
            }

            if (IniFile.DuplicatePropertyHandling == DuplicatePropertyHandling.KeepLast)
            {
                if (propertyExists)
                {
                    var property = properties.Single();
                    property.Value = value;
                    return property;
                }
                return addProperty();
            }

            if (IniFile.DuplicatePropertyHandling == DuplicatePropertyHandling.Rename)
            {
                if (propertyExists)
                {
                    name += properties.Count + 1;
                    return addProperty();
                }
                return addProperty();
            }

            // the compiler will complain without it
            return null;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return result != null;
        }

        public static bool operator ==(IniSection iniSection, string name)
        {
            return
                !ReferenceEquals(iniSection, null) &&
                !string.IsNullOrEmpty(name) &&
                iniSection.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator !=(IniSection iniSection, string name)
        {
            return !(iniSection == name);
        }

        protected bool Equals(IniSection other)
        {
            return
                !ReferenceEquals(other, null) &&
                Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IniSection)obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }       
    }
}