using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martini
{
    public class InvalidLineException : Exception
    {
        public int LineIndex { get; set; }
        public string Value { get; set; }
    }

    public class PropertyExistsException : Exception
    {
        public string Section { get; set; }
        public string Properety { get; set; }
        public override string Message => $"Property \"{Properety}\" already exists in section [{Section}].";
    }

    // todo add context info
    public class DuplicateSectionsException : Exception { }

    // todo add context info
    public class DuplicatePropertiesException : Exception { }
}
