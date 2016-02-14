# Martini v1.0.0-beta1
Advanced C# INI Parser with LINQ support.

---

**`Martini`** is a growing library for reading and writing `ini` files. Like my other projects its main purpose is simple usage with advanced features that meet all clean-code rules.

### Features
**`Martini`**'s features:

- Reading and writing `ini` files
- Creating new sections
- Creating new properties
- Creating new comments _(planned)_
- Retrieving sections and properties
- Dynamic sections and properties retrieval _(as long as their names meed `C#` identifier rules)_
- Indexed secitons and properties retrieval _(supports any names)_
- Duplicate sections handling
    - **Allow** - Duplicate sections will be read as they are
    - **Disallow** - Duplicate sections will throw an exception
    - **Merge** - Duplicate sections will be merged
- Duplicate properties handling
    - **Allow** - Duplicate properties will be read as they are
    - **Disallow** - Duplicate properties will throw an exceptoin
    - **KeepFirst** - Duplicate properties will be resolved by keeping the first property and throwing away the others
    - **KeepLast** - Duplicate properties will be resolved by keeping the last property and throwing away the others
    - **Rename** - Duplicate properties will be resolved by renaming all duplicate properties by appending a counter
- Custom section brackets _(planned)_
- Custom property/value delimiter _(planned)_
- Custom comment indicator _(planned)_
- Save formattings
    - Keep blank lines _(planned)_
    - Insert blank line before section _(planned)_


### Examples

#### Sampe ini
```
; last modified 1 April 2001 by [John Doe]
[owner]
name=John\;Doe
organization=Acme [Widgets] Inc.

; settings for database access
[database]
; use IP address in case network name resolution is not working
server=192.0.2.62     
port=143
file=foo.dat
file=baz.dat

[database]
server=192.0.2.68
port=146

[last]
foo=bar
```

Dynamic sections a properties retrieval:

```c#
dynamic iniFile = IniFile.From("test.ini");
var server = iniFile1.database.server; // as IEnumerable<IniProperty>
```

Indexed section and properties retrieval:

```c#
var iniFile = IniFile.From("test.ini");
var server = iniFile["database"]["server"].First();
var section = iniFile.AddSection("downloads");
```
