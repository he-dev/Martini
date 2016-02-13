# Martini
Advanced C# INI Parser with LINQ support.

There is still a lot to do like refactoring etc. but the current experimental version already supports:
- reading and writing simple ini files
- creating new sections
- retrieving sections and properties

Sections and properties can be retrievied either dynamically as long as they meet `C#` identifier name rules:

```c#
dynamic iniFile1 = IniFile.From("test.ini");
iniFile1.Save("test2.ini");

var serv1 = ((IEnumerable<IniProperty>)iniFile1.database.server).First();
```

or via the classical indexers:

```c#
var iniFile2 = IniFile.From("test.ini");
var serv2 = iniFile2["database"]["server"].First();
var section = iniFile2.AddSection("downloads");
            
``` 
