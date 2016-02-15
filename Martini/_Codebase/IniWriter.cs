using System.Text;

namespace Martini
{
    internal class IniWriter
    {
        public static void Save(Sentence firstSentence, string fileName)
        {
            var iniFileText = new StringBuilder();
            foreach (var sentence in firstSentence.After)
            {
                iniFileText.AppendLine(sentence.ToString());
            }
            var iniFile = iniFileText.ToString();
        }
    }
}