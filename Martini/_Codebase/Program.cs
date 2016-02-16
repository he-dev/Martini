using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Martini.Collections;

namespace Martini
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            var foo = new LinkedObject<string>("foo");
            var bar = new LinkedObject<string>("bar");
            var baz = new LinkedObject<string>("baz");

            foo.Next = baz;
            foo.Next = bar;

            bar.Remove();


            //foo.Next.Remove();

            dynamic iniFile1 = IniFile.From("test.ini", new IniSettings
            {
                DuplicateSectionHandling = DuplicateSectionHandling.Merge,
                DuplicatePropertyHandling = DuplicatePropertyHandling.KeepLast
            });
            iniFile1.Save("test2.ini");

            var serv1 = iniFile1.database.server;

            var iniFile2 = IniFile.From("test.ini");
            var serv2 = iniFile2["database"]["server"];
            var section = iniFile2.AddSection("downloads");
        }
    }


    internal static class StringFormatter
    {
        public static string Unescape(this string text)
        {

            return null;
        }

        public static string Escape(this string text)
        {
            //Regex.Replace(text, )
            return null;
        }
    }

    
}

