using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace Martini
{
    internal class IniFile : DynamicObject
    {
        private readonly Sentence _firstSentence;

        internal IniFile(Sentence firsSentence)
        {
            _firstSentence = firsSentence;
        }

        public IniSection this[string name]
        {
            get
            {
                var sentence =
                    _firstSentence.After.Sections()
                        .SingleOrDefault(x => Extensions.SectionToken((Sentence) x) == name);

                var section = sentence == null ? null : new IniSection(sentence);
                return section;
            }
        }

        public IEnumerable<IniSection> Sections => _firstSentence.After.Sections().Select(x => new IniSection(x));

        public static IniFile From(string fileName, IniParserSettings settings = null)
        {
            var iniText = File.ReadAllText(fileName);
            var iniFile = Parser.Parse(iniText, settings);
            return iniFile;
        }

        public void Save(string fileName)
        {
            IniWriter.Save(_firstSentence, fileName);
        }

        public IniSection AddSection(string name)
        {
            var section = SectionFactory.CreateSection(name);
            var lastSentence = _firstSentence.After.Last();
            lastSentence.Next = section;
            return new IniSection(section);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return (result != null);
        }
    }
}