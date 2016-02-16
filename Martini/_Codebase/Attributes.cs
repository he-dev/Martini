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
        public string Left { get; set; }
        public string Right { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class PropertyValueDelimiterAttribute : Attribute
    {
        public string Deimiter { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class CommentIndicatorAttribute : Attribute
    {
        public string Deimiter { get; set; }
    }
}
