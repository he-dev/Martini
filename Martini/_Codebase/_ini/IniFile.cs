using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Martini.Collections;

namespace Martini
{
    public class IniFile : DynamicObject
    {
        public IniFile(IniSettings settings = null)
        {
            settings = settings ?? new IniSettings();

            DuplicateSectionHandling = settings.DuplicateSectionHandling;
            DuplicatePropertyHandling = settings.DuplicatePropertyHandling;
            Delimiters = settings.Delimiters;
            GlobalSection = SectionFactory.CreateSection(Grammar.GlobalSectionName, Delimiters);
        }

        internal IniFile(Sentence globalSection, IniSettings settings = null) : this(settings)
        {
            GlobalSection = globalSection;
        }

        internal Sentence GlobalSection { get; }

        public DuplicateSectionHandling DuplicateSectionHandling { get; }

        public DuplicatePropertyHandling DuplicatePropertyHandling { get; }

        internal dynamic Delimiters { get; }

        public IniSection this[string name] => GlobalSection.After.Sections().Where(x => x.SectionToken() == name).Select(x => new IniSection(x, this)).SingleOrDefault();

        public IEnumerable<IniSection> Sections => GlobalSection.After.Sections().Select(x => new IniSection(x, this));

        public bool ContainsSection(string name)
        {
            return this[name] != null;
        }

        public static IniFile From(string fileName, IniSettings settings = null)
        {
            var iniText = File.ReadAllText(fileName);
            var firstSentence = Parser.Parse(iniText, settings);
            var iniFile = new IniFile(firstSentence, settings);
            return iniFile;
        }

        public void Save(string fileName, FormattingOptions options = FormattingOptions.None)
        {
            var iniText = IniRenderer.Render(GlobalSection, options);
        }

        public IniSection AddSection(string name)
        {
            var sections = Sections.Where(x => x == name).ToList();
            var sectionExists = sections.Any();

            var addSection = new Func<IniSection>(() =>
            {
                var section = SectionFactory.CreateSection(name, Delimiters);
                GlobalSection.After.Last().Next = section;
                return new IniSection(section, this);
            });

            if (DuplicateSectionHandling == DuplicateSectionHandling.Disallow)
            {
                if (sectionExists)
                {
                    throw new DuplicateSectionsException();
                }
                return addSection();
            }

            if (DuplicateSectionHandling == DuplicateSectionHandling.Allow)
            {
                return addSection();
            }

            if (DuplicateSectionHandling == DuplicateSectionHandling.Merge)
            {
                if (sectionExists)
                {
                    return sections.Single();
                }
                return addSection();
            }

            return null;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return true; //(result != null);
        }
    }
}