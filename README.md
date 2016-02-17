# Martini v1.0.0-alpha2
Advanced C# INI Parser with LINQ support.

---

**`Martini`** is a growing library for reading and writing `ini` files. Like my other projects its main purpose is simple usage with advanced features that meet all clean-code rules.

### Features
**`Martini`**'s features:

- Reading and writing `ini` files
- Creating new sections
- Creating new properties
- Creating new comments
- Retrieving sections and properties
- Dynamic sections and properties retrieval _(as long as their names meet `C#` identifier rules)_
- Indexed sections and properties retrieval _(supports any names)_
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
- Section delimiters
    - Square brackets `[]`
    - Round brackets `()`
    - Curly brackets `{}`
    - Angle brackets `<>`
- Property/value delimiters
    - Equal sign `=`
    - Colon `:` 
- Comment indicators
    - Semicolon `;`
    - Number sign `#`
- Formatting options
    - Blank line before section
    - Space after comment indicator
    - Space before property/value delimiter
    - Space after property/value delimiter
    - Quote values with spaces


### Examples

#### Sampe ini
```
; Welcome to Martini by he-dev
[dev]
name=he-dev
organization=MTk4MQ Dev.

; Martini can handle duplicate sections
[github]
; This is Martini's github address
url=https://github.com/he-dev/Martini
iniFile=test1.ini
iniFile=test2.ini

[github]
url=https://github.com/he-dev/SmartConfig

[foo]
baz=baz
```

Dynamic sections a properties retrieval:

```c#
dynamic iniFile = IniFile.From("test.ini");
var devName = iniFile1.dev.name; // = he-dev
```

Indexed section and properties retrieval:

```c#
var iniFile = IniFile.From("test.ini");
var devName = iniFile["dev"]["name"]; // = he-dev
```
