using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martini
{
    public abstract class IniElement : DynamicObject
    {
        internal IniElement(Sentence sentence, IniFile iniFile)
        {
            Sentence = sentence;
            IniFile = iniFile;
        }

        internal Sentence Sentence { get; }

        internal IniFile IniFile { get; }

        public void Remove()
        {
            Sentence.Remove();
        }
    }
}
