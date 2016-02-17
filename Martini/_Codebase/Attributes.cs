using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martini
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SectionDelimiterAttribute : Attribute
    {
        public char Left { get; set; }
        public char Right { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class PropertyValueDelimiterAttribute : Attribute
    {
        public char Delimiter { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class CommentIndicatorAttribute : Attribute
    {
        public char Delimiter { get; set; }
    }
}
