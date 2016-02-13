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

---

Here's a list of some features that will come next:

DuplicateSectionHandling
- **Allow** - Duplicate sections will be read as they are
- **Disallow** - Duplicate sections will throw an exception
- **Merge** - Duplicate sections will be merged

DuplicatePropertyHandling
- **Allow** - Duplicate properties will be read as they are
- **Disallow** - Duplicate properties will throw an exceptoin
- **TakeFirst** - Duplicate properties will be resolved by taking the first property
- **TakeLast** - Duplicate properties will be resolved by taking the last property
- **Rename** - Duplicate properties will be resolved by renaming subsequent properties

InvalidLineHandling
- **Disallow**- Invalid lines will throw an exception
- **Ignore** - Invlid lines will be ignored
- **Allow** - Invalid lines will be read as the are