using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

            dynamic iniFile1 = IniFile.From("test.ini", new IniParserSettings
            {
                DuplicateSectionHandling = DuplicateSectionHandling.Merge,
                DuplicatePropertyHandling = DuplicatePropertyHandling.KeepLast
            });
            iniFile1.Save("test2.ini");

            var serv1 = ((IEnumerable<IniProperty>)iniFile1.database.server).First();

            var iniFile2 = IniFile.From("test.ini");
            var serv2 = iniFile2["database"]["server"].First();
            var section = iniFile2.AddSection("downloads");
        }
    }

    public enum DuplicateSectionHandling
    {
        Allow,
        Disallow,
        Merge
    }

    public enum DuplicatePropertyHandling
    {
        Allow,
        Disallow,
        KeepFirst,
        KeepLast,
        Rename
    }

    public enum InvalidLineHandling
    {
        Throw,
        Ignore,
        Keep
    }

    public class IniParserSettings
    {
        public DuplicateSectionHandling DuplicateSectionHandling { get; set; } = DuplicateSectionHandling.Allow;
        public DuplicatePropertyHandling DuplicatePropertyHandling { get; set; } = DuplicatePropertyHandling.Allow;
        public InvalidLineHandling InvalidLineHandling { get; set; } = InvalidLineHandling.Throw;
    }

    [Flags]
    public enum Formattings
    {
        None,
        EmptyLineBeforeSection,
    }

    internal enum TokenType
    {
        Uninitialized,
        StartOfLine,
        EndOfLine,
        LeftSectionDelimiter,
        RightSectionDelimiter,
        ProperetyValueDelimiter,
        Text,
        Section,
        Property,
        Value,
        CommentIndicator,
        Comment,
        QuotationMark,
        EscapeSequence,
    }

    internal enum SentenceType
    {
        Uninitialized,
        Blank,
        Comment,
        Section,
        Property,
        Invalid
    }

    [DebuggerDisplay("Type = {Type} Token = {_token}")]
    internal class Token
    {
        private string _token;

        public Token(string token)
        {
            _token = token;
        }

        public Token(char token) : this(token.ToString()) { }

        public Token(TokenType tokenType) : this((char)Grammar.DelimiterTokenTypeMap.TokenTypes[tokenType])
        {
            Type = tokenType;
        }

        public Token(TokenType tokenType, string token) : this(token)
        {
            Type = tokenType;
        }

        public string Value { get { return _token; } set { _token = value; } }

        public Sentence Sentence { get; set; }

        public TokenType Type { get; set; } = TokenType.Text;

        public int FromColumn { get; set; } = -1;

        public int ToColumn => FromColumn + Length;

        public int Length => _token.Length;

        public static bool operator ==(Token x, string y)
        {
            return
                !ReferenceEquals(x, null)
                && x.ToString().Equals(y, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator !=(Token x, string y)
        {
            return !(x == y);
        }

        public static implicit operator string(Token token)
        {
            return token?.ToString();
        }

        public override string ToString()
        {
            return _token;
        }

        protected bool Equals(Token other)
        {
            return string.Equals(_token, other._token);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Token)obj);
        }

        public override int GetHashCode()
        {
            return _token?.GetHashCode() ?? 0;
        }
    }

    [DebuggerDisplay("_object = {_object?.ToString()}")]
    internal class LinkedObject<T>
    {
        private T _object;
        private LinkedObject<T> _previous;
        private LinkedObject<T> _next;

        public LinkedObject()
        {
        }

        public LinkedObject(T @object)
        {
            _object = @object;
        }

        public static implicit operator T(LinkedObject<T> linkedObject)
        {
            return linkedObject._object;
        }

        public LinkedObject<T> Previous
        {
            get { return _previous; }
            set
            {
                value.Remove();

                value._previous = _previous;
                value._next = this;

                if (_previous != null)
                {
                    _previous._next = value;
                }

                _previous = value;
            }
        }

        public LinkedObject<T> Next
        {
            get { return _next; }
            set
            {
                value.Remove();

                value._previous = this;
                value._next = _next;

                if (_next != null)
                {
                    _next._previous = value;
                }

                _next = value;
            }
        }

        public IEnumerable<LinkedObject<T>> Before
        {
            get
            {
                var item = this;
                while (item != null)
                {
                    yield return item;
                    item = item.Previous;
                }
            }
        }

        public IEnumerable<LinkedObject<T>> After
        {
            get
            {
                var item = this;
                while (item != null)
                {
                    yield return item;
                    item = item.Next;
                }
            }
        }

        public void Remove()
        {
            if (_previous != null)
            {
                _previous._next = _next;
            }

            if (_next != null)
            {
                _next._previous = _previous;
            }

            _previous = null;
            _next = null;
        }
    }

    [DebuggerDisplay("{ToString()} at {Line}")]
    internal class Sentence
    {
        private readonly LinkedObject<Sentence> _linkedObject;
        private List<Token> _tokens = new List<Token>();

        public Sentence()
        {
            _linkedObject = new LinkedObject<Sentence>(this);
        }

        public SentenceType Type { get; set; } = SentenceType.Uninitialized;

        public int Line { get; set; }

        public List<Token> Tokens
        {
            get { return _tokens; }
            set
            {
                _tokens = value;
                _tokens.ForEach(t => t.Sentence = this);
            }
        }

        public Sentence Previous
        {
            get { return _linkedObject.Previous; }
            set { _linkedObject.Previous = value._linkedObject; }
        }

        public Sentence Next
        {
            get { return _linkedObject.Next; }
            set { _linkedObject.Next = value._linkedObject; }
        }

        public IEnumerable<Sentence> Before
        {
            get { return _linkedObject.Before.Select(x => (Sentence)x); }
        }

        public IEnumerable<Sentence> After
        {
            get { return _linkedObject.After.Select(x => (Sentence)x); }
        }

        public void Remove()
        {
            _linkedObject.Remove();
        }

        public override string ToString()
        {
            return string.Join(string.Empty, Tokens);
        }
    }

    internal class Tokenizer
    {
        public static Sentence Tokenize(string ini)
        {
            var lines = ini.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var firstSentence = (Sentence)null;

            var appendSentence = new Action<Sentence>(next =>
            {
                if (firstSentence == null)
                {
                    firstSentence = next;
                }
                else
                {
                    firstSentence.After.Last().Next = next;
                }
            });

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                var tokens = TokenizeLine(line);
                appendSentence(new Sentence
                {
                    Line = i,
                    Tokens = tokens
                });
            }

            return firstSentence;
        }

        private static List<Token> TokenizeLine(string line)
        {
            var isEmptyLine = string.IsNullOrWhiteSpace(line);
            if (isEmptyLine)
            {
                return new List<Token>();
            }

            // initialize tokens with start-of-line
            var delimiterTokens = new List<Token>
            {
                new Token(Grammar.Space)
                {
                    Type = TokenType.StartOfLine,
                }
            };

            // determine delimiter tokens
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];

                TokenType tokenType;
                var isDelimiterToken = Grammar.DelimiterTokenTypeMap.Delimiters.TryGetValue(c, out tokenType);
                if (!isDelimiterToken)
                {
                    continue;
                }

                var isEscapedDelimiterToken = i > 0 && Grammar.EscapeCharacters.Contains(c) && line[i - 1] == Grammar.Backslash;
                if (isEscapedDelimiterToken)
                {
                    continue;
                }

                // ignore inline section delimiters
                var ignoreInlineSectionDelimiter =
                    // is inline left bracket?
                    (tokenType == TokenType.LeftSectionDelimiter && i > 0) ||
                    // is inline right bracket?
                    (tokenType == TokenType.RightSectionDelimiter && i <= line.Length - 1 && delimiterTokens.Skip(1).FirstOrDefault()?.Type != TokenType.LeftSectionDelimiter);
                if (ignoreInlineSectionDelimiter)
                {
                    continue;
                }

                // if equal sign already found then ignore the others
                var ignoreInlinePropertyValueSeparator = delimiterTokens.Any(t => t.Type == TokenType.ProperetyValueDelimiter);
                if (ignoreInlinePropertyValueSeparator)
                {
                    continue;
                }

                // collect token
                var token = new Token(c)
                {
                    FromColumn = i,
                    Type = tokenType
                };
                delimiterTokens.Add(token);
            }

            // add end-of-line token
            delimiterTokens.Add(new Token(Grammar.Space)
            {
                Type = TokenType.EndOfLine,
                FromColumn = line.Length
            });

            // get what's left as text tokens and put everything in a new list

            var previousToken = delimiterTokens.First();

            var allTokens = new List<Token>();

            foreach (var currentToken in delimiterTokens)
            {
                // calc the position and length of the text between delimiter tokens

                var previousColumn = previousToken.FromColumn;
                var previousLength = previousToken.Length;

                var currentColumn = currentToken.FromColumn;

                var startIndex = previousColumn + previousLength;
                var textLength = currentColumn - (previousColumn + previousLength);

                if (textLength == 0 && currentToken.Type != TokenType.EndOfLine)
                {
                    allTokens.Add(currentToken);
                }

                if (textLength > 0)
                {
                    var text = line.Substring(startIndex, textLength);
                    var textToken = new Token(text)
                    {
                        Type = TokenType.Text,
                        FromColumn = currentColumn
                    };
                    allTokens.Add(textToken);

                    if (currentToken.Type != TokenType.EndOfLine)
                    {
                        allTokens.Add(currentToken);
                    }
                }

                previousToken = currentToken;
            }

            return allTokens;
        }
    }

    [DebuggerDisplay("Type = {SentenceType}")]
    internal class SentenceDefinition
    {
        public SentenceType SentenceType { get; set; }

        public TokenType[][] AllowedTokenTypes { get; set; }

        public TokenType[] ExactTokenTypes { get; set; }
    }

    internal class Grammar
    {
        public static readonly AutoKeyDictionary<SentenceType, SentenceDefinition> SentenceDefinitions =
            new AutoKeyDictionary<SentenceType, SentenceDefinition>(x => x.SentenceType)
            {
                new SentenceDefinition
                {
                    SentenceType = SentenceType.Blank,
                    AllowedTokenTypes = new[] {new TokenType[] {}},
                    ExactTokenTypes = new TokenType[] {}
                },
                new SentenceDefinition
                {
                    SentenceType = SentenceType.Comment,
                    AllowedTokenTypes = new[]
                    {
                        new TokenType[] {TokenType.CommentIndicator, TokenType.Text},
                        new TokenType[] {TokenType.CommentIndicator},
                    },
                    ExactTokenTypes = new TokenType[] {TokenType.CommentIndicator, TokenType.Comment}
                },
                new SentenceDefinition
                {
                    SentenceType = SentenceType.Section,
                    AllowedTokenTypes = new[]
                    {
                        new TokenType[] { TokenType.LeftSectionDelimiter, TokenType.Text, TokenType.RightSectionDelimiter },
                    },
                    ExactTokenTypes = new TokenType[] {TokenType.LeftSectionDelimiter, TokenType.Section, TokenType.RightSectionDelimiter},
                },
                new SentenceDefinition
                {
                    SentenceType = SentenceType.Property,
                    AllowedTokenTypes = new[]
                    {
                        new TokenType[] {TokenType.Text, TokenType.ProperetyValueDelimiter, TokenType.Text},
                        new TokenType[] {TokenType.Text, TokenType.ProperetyValueDelimiter},
                    },
                    ExactTokenTypes = new TokenType[] {TokenType.Property, TokenType.ProperetyValueDelimiter, TokenType.Value},
                },
            };

        public static readonly string Space = ' '.ToString();

        public static readonly char Backslash = '\\';

        public static dynamic DelimiterTokenTypeMap = new BiDictionary<char, TokenType>("Delimiters", "TokenTypes")
        {
            {'[', TokenType.LeftSectionDelimiter},
            {']', TokenType.RightSectionDelimiter},
            {'=', TokenType.ProperetyValueDelimiter},
            {';', TokenType.CommentIndicator},
        };

        public static readonly HashSet<char> EscapeCharacters = new HashSet<char>
        {
            '\\',
            't',
            ';',
            '=',
            '"'
        };
    }

    internal class KeyPair<TKey1, TKey2>
    {
        public TKey1 Key1 { get; set; }
        public TKey2 Key2 { get; set; }
    }

    internal class BiDictionary<TKey1, TKey2> : DynamicObject, IEnumerable<KeyPair<TKey1, TKey2>>
    {
        private readonly Dictionary<TKey1, TKey2> _K1K2 = new Dictionary<TKey1, TKey2>();
        private readonly Dictionary<TKey2, TKey1> _K2K1 = new Dictionary<TKey2, TKey1>();

        private readonly string _key1Name;
        private readonly string _key2Name;

        public BiDictionary(string key1Name, string key2Name)
        {
            _key1Name = key1Name;
            _key2Name = key2Name;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name == _key1Name)
            {
                result = _K1K2;
                return true;
            }

            if (binder.Name == _key2Name)
            {
                result = _K2K1;
                return true;
            }

            result = null;
            return false;
        }

        public void Add(TKey1 key1, TKey2 key2)
        {
            _K1K2.Add(key1, key2);
            _K2K1.Add(key2, key1);
        }

        public IEnumerator<KeyPair<TKey1, TKey2>> GetEnumerator()
        {
            return _K1K2.Zip(_K2K1, (d1, d2) => new KeyPair<TKey1, TKey2>
            {
                Key1 = d1.Key,
                Key2 = d2.Key
            }).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class Parser
    {
        public static IniFile Parse(string ini, IniParserSettings settings)
        {
            var firstSentence = Tokenizer.Tokenize(ini);
            DetermineSentenceType(firstSentence);

            HandleDuplicateSections(firstSentence, settings.DuplicateSectionHandling);
            HandleDuplicateProperties(firstSentence, settings.DuplicatePropertyHandling);

            var iniFile = new IniFile(firstSentence);
            return iniFile;
        }

        private static void HandleDuplicateSections(Sentence firstSentence, DuplicateSectionHandling duplicateSectionHandling)
        {
            if (duplicateSectionHandling == DuplicateSectionHandling.Allow)
            {
                return;
            }

            var duplicateSectionsGroups =
                firstSentence.After.Sections()
                .GroupBy(x => (string)x.SectionToken(), (name, sections) => new
                {
                    name,
                    sections
                })
                .Where(x => x.sections.Count() > 1)
                .ToList();

            if (!duplicateSectionsGroups.Any())
            {
                return;
            }

            if (duplicateSectionHandling == DuplicateSectionHandling.Disallow)
            {
                throw new DuplicateSectionsException();
            }

            if (duplicateSectionHandling == DuplicateSectionHandling.Merge)
            {
                foreach (var duplicateSectionsGroup in duplicateSectionsGroups)
                {
                    var baseSection = duplicateSectionsGroup.sections.First();
                    var mergeSections = duplicateSectionsGroup.sections.Skip(1);
                    foreach (var mergeSection in mergeSections)
                    {
                        var sectionContent = mergeSection.Contents().ToList();
                        foreach (var sentence in sectionContent)
                        {
                            var lastBaseSectionProperty = baseSection.Contents().Last();
                            lastBaseSectionProperty.Next = sentence;
                        }
                        mergeSection.Remove();
                    }
                }
            }
        }

        private static void HandleDuplicateProperties(Sentence firstSentence, DuplicatePropertyHandling duplicatePropertyHandling)
        {
            if (duplicatePropertyHandling == DuplicatePropertyHandling.Allow)
            {
                return;
            }

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var section in firstSentence.After.Sections())
            {
                var duplicatePropertyGroups =
                    section.Contents().Properties()
                    .GroupBy(x => (string)x.PropertyToken(), (name, properties) => new
                    {
                        name,
                        properties
                    })
                    .Where(x => x.properties.Count() > 1)
                    .ToList();

                if (!duplicatePropertyGroups.Any())
                {
                    continue;
                }

                if (duplicatePropertyHandling == DuplicatePropertyHandling.Disallow)
                {
                    throw new DuplicatePropertiesException();
                }

                if (duplicatePropertyHandling == DuplicatePropertyHandling.Rename)
                {
                    foreach (var duplicatePropertyGroup in duplicatePropertyGroups)
                    {
                        // append counter to each property
                        var counter = 1;
                        foreach (var property in duplicatePropertyGroup.properties)
                        {
                            var name = property.PropertyToken().Value;
                            property.Tokens.PropertyToken().Value = $"{name}{counter++}";
                        }
                    }
                }

                var removeProperties = new Action<IEnumerable<Sentence>>(propertiesToRemove =>
                {
                    foreach (var propertyToRemove in propertiesToRemove)
                    {
                        // also remove comments
                        var comments = propertyToRemove.Comments();
                        foreach (var comment in comments)
                        {
                            comment.Remove();
                        }
                        // finally the property
                        propertyToRemove.Remove();
                    }
                });

                if (duplicatePropertyHandling == DuplicatePropertyHandling.KeepFirst)
                {
                    foreach (var duplicatePropertyGroup in duplicatePropertyGroups.ToList())
                    {
                        // skip the first property that we want to keep
                        var propertiesToRemove = duplicatePropertyGroup.properties.Skip(1);
                        removeProperties(propertiesToRemove);
                    }
                }

                if (duplicatePropertyHandling == DuplicatePropertyHandling.KeepLast)
                {
                    foreach (var duplicatePropertyGroup in duplicatePropertyGroups.ToList())
                    {
                        // skip the first property that we want to keep
                        var lastProperty = duplicatePropertyGroup.properties.Last();
                        var propertiesToRemove = duplicatePropertyGroup.properties.Where(p => p != lastProperty);
                        removeProperties(propertiesToRemove);                        
                    }
                }
            }
        }

        public static void DetermineSentenceType(Sentence sentence)
        {
            foreach (var currentSentence in sentence.After)
            {
                if (!currentSentence.Tokens.Any())
                {
                    currentSentence.Type = SentenceType.Blank;
                    continue;
                }

                var sentenceDefinitionMatch =
                    (from sentenceDefinition in Grammar.SentenceDefinitions
                     from tokens in sentenceDefinition.AllowedTokenTypes
                     where tokens.SequenceEqual(currentSentence.Tokens.Select(t => t.Type))
                     select sentenceDefinition).SingleOrDefault();

                if (sentenceDefinitionMatch == null)
                {
                    currentSentence.Type = SentenceType.Invalid;
                    continue;
                }

                currentSentence.Type = sentenceDefinitionMatch.SentenceType;

                // update token types to exact types

                var exactTokenTypes = sentenceDefinitionMatch.ExactTokenTypes;
                for (var i = 0; i < sentenceDefinitionMatch.ExactTokenTypes.Length; i++)
                {
                    var exactTokenType = exactTokenTypes[i];
                    if (i < currentSentence.Tokens.Count)
                    {
                        currentSentence.Tokens[i].Type = exactTokenType;
                    }
                    // found less tokens then actually required so add the missing one
                    else
                    {
                        var newToken = new Token(string.Empty)
                        {
                            Type = exactTokenTypes[i],
                            Sentence = currentSentence,
                            FromColumn = currentSentence.Tokens.Last().ToColumn + 1
                        };
                        currentSentence.Tokens.Add(newToken);
                    }
                }
            }
        }
    }

    internal class AutoKeyDictionary<TKey, T> : IEnumerable<T>
    {
        private readonly Func<T, TKey> _getKey;
        private readonly Dictionary<TKey, T> _items = new Dictionary<TKey, T>();

        public AutoKeyDictionary(Func<T, TKey> getKey)
        {
            _getKey = getKey;
        }

        public T this[TKey key]
        {
            get { return _items[key]; }
            set { _items[key] = value; }
        }

        public bool TryGetValue(TKey key, out T value)
        {
            return _items.TryGetValue(key, out value);
        }

        public bool Contains(T item)
        {
            return _items.ContainsKey(_getKey(item));
        }

        public void Add(T item)
        {
            _items[_getKey(item)] = item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

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

    internal static class Extensions
    {
        public static Token SectionToken(this Sentence sentence)
        {
            return sentence.Tokens.SectionToken();
        }

        public static Token PropertyToken(this Sentence sentence)
        {
            return sentence.Tokens.PropertyToken();
        }

        public static Token SectionToken(this IEnumerable<Token> tokens)
        {
            return tokens.Single(t => t.Type == TokenType.Section);
        }

        public static Token PropertyToken(this IEnumerable<Token> tokens)
        {
            return tokens.Single(t => t.Type == TokenType.Property);
        }

        public static Token ValueToken(this IEnumerable<Token> tokens)
        {
            return tokens.Single(t => t.Type == TokenType.Value);
        }

        public static Token CommentToken(this IEnumerable<Token> tokens)
        {
            return tokens.Single(t => t.Type == TokenType.Comment);
        }

        public static IEnumerable<Sentence> Comments(this Sentence sentences)
        {
            return sentences.Before.Skip(1).TakeWhile(x => x.Type == SentenceType.Comment).Reverse();
        }

        public static IEnumerable<Sentence> Contents(this Sentence sentence)
        {
            var sectionContents = sentence.After.Skip(1).TakeWhile(x => x.Type != SentenceType.Section).ToList();
            return sectionContents;
        }

        public static IEnumerable<Sentence> Sections(this IEnumerable<Sentence> sentences)
        {
            return sentences.Where(x => x.Type == SentenceType.Section);
        }

        public static IEnumerable<Sentence> Properties(this IEnumerable<Sentence> sentences)
        {
            return sentences.Where(x => x.Type == SentenceType.Property);
        }
    }

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

    internal class IniFile : DynamicObject
    {
        private readonly Sentence _firstSentence;

        internal IniFile(Sentence firsSentence)
        {
            _firstSentence = firsSentence;
        }

        public IniSection this[string name]
        {
            get
            {
                var sentence =
                    _firstSentence.After.Sections()
                    .SingleOrDefault(x => x.SectionToken() == name);

                var section = sentence == null ? null : new IniSection(sentence);
                return section;
            }
        }

        public static IniFile From(string fileName, IniParserSettings settings = null)
        {
            var iniText = File.ReadAllText(fileName);
            var iniFile = Parser.Parse(iniText, settings);
            return iniFile;
        }

        public void Save(string fileName)
        {
            IniWriter.Save(_firstSentence, fileName);
        }

        public IniSection AddSection(string name)
        {
            var section = SectionFactory.CreateSection(name);
            var lastSentence = _firstSentence.After.Last();
            lastSentence.Next = section;
            return new IniSection(section);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return (result != null);
        }
    }

    [DebuggerDisplay("Name = {Name}")]
    public class IniSection : DynamicObject
    {
        private readonly Sentence _section;

        internal IniSection(Sentence section)
        {
            Debug.Assert(section.Tokens.SectionToken() != null);
            _section = section;
        }

        public IEnumerable<IniProperty> this[string name]
        {
            get { return Properties.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); }
        }

        public List<IniComment> Comments
        {
            get { return _section.Comments().Select(x => new IniComment(x)).ToList(); }
        }

        public string Name
        {
            get { return _section.Tokens.SectionToken(); }
        }

        public IEnumerable<IniProperty> Properties
        {
            get
            {
                return _section.Contents().Properties().Select(x => new IniProperty(x));
            }
        }

        public IniProperty AddProperty(string name, string value, bool allowDuplicateProperties = false)
        {
            var newProperty = PropertyFactory.CreateProperty(name, value);

            var properties = _section.Contents().Properties().ToList();

            if (!allowDuplicateProperties)
            {
                var propertyExists = properties.Any(t => t.Tokens.PropertyToken() == name);
                if (propertyExists)
                {
                    throw new PropertyExistsException
                    {
                        Section = Name,
                        Properety = name
                    };
                }
            }

            properties.Last().Next = newProperty;

            return new IniProperty(newProperty);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return ((IEnumerable<IniProperty>)result).Any();
        }
    }

    [DebuggerDisplay("Text = {Text}")]
    public class IniComment
    {
        private readonly Sentence _comment;

        internal IniComment(Sentence comment)
        {
            _comment = comment;
        }

        public string Text
        {
            get { return _comment.Tokens.CommentToken(); }
            set { _comment.Tokens.CommentToken().Value = value; }
        }
    }

    [DebuggerDisplay("Name = {Name} ValueToken = {Value}")]
    public class IniProperty
    {
        private readonly Sentence _property;

        internal IniProperty(Sentence property)
        {
            _property = property;
        }

        public List<IniComment> Comments
        {
            get { return _property.Comments().Select(x => new IniComment(x)).ToList(); }
        }

        public string Name
        {
            get { return _property.Tokens.PropertyToken(); }
        }

        public string Value
        {
            get { return _property.Tokens.ValueToken(); }
        }
    }

    internal class SectionFactory
    {
        public static Sentence CreateSection(string name)
        {
            var section = new Sentence
            {
                Type = SentenceType.Section,
                Tokens = new List<Token>
                {
                    new Token(TokenType.LeftSectionDelimiter),
                    new Token(TokenType.Section, name),
                    new Token(TokenType.RightSectionDelimiter),
                }
            };
            return section;
        }
    }

    internal class PropertyFactory
    {
        public static Sentence CreateProperty(string name, string value)
        {
            var property = new Sentence
            {
                Type = SentenceType.Property,
                Tokens = new List<Token>
                {
                    new Token(TokenType.Property, name),
                    new Token(TokenType.ProperetyValueDelimiter),
                    new Token(TokenType.Value),
                }
            };
            return property;
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

