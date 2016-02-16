using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

namespace Martini
{
    [DebuggerDisplay("Name = {Name}")]
    public class IniSection : IniElement
    {        
        internal IniSection(Sentence section, IniFile iniFile) : base(section, iniFile) { }

        public IEnumerable<IniProperty> this[string name] =>
            Sentence.Contents().Properties()
                .Where(x => x.PropertyToken() == name)
                .Select(x => new IniProperty(x, IniFile));

        public string Name => Sentence.Tokens.SectionToken();

        public List<IniComment> Comments => Sentence.Comments().Select(x => new IniComment(x, IniFile)).ToList();

        public IEnumerable<IniProperty> Properties => Sentence.Contents().Properties().Select(x => new IniProperty(x, IniFile));

        public IniProperty AddProperty(string name, string value)
        {
            var property = PropertyFactory.CreateProperty(name, value);

            var iniProperty = new IniProperty(property, IniFile);
            Sentence.Contents().Properties().Last().Next = iniProperty.Sentence;

            
            //if (!allowDuplicateProperties)
            //{
            //    var propertyExists = properties.Any(t => t.Tokens.PropertyToken() == name);
            //    if (propertyExists)
            //    {
            //        throw new PropertyExistsException
            //        {
            //            Section = Name,
            //            Properety = name
            //        };
            //    }
            //}

            return iniProperty;
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