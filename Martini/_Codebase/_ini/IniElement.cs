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
        internal IniElement(Sentence sentence)
        {
            Sentence = sentence;
        }

        internal Sentence Sentence { get; private set; }

        public void Remove()
        {
            Sentence.Remove();
        }
    }
}
