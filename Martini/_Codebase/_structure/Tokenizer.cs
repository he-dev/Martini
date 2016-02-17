using System;
using System.Collections.Generic;
using System.Linq;
using Martini;
using Martini.Collections;

namespace Martini
{
    internal class Tokenizer
    {
        public static Sentence Tokenize(string ini, dynamic delimiters)
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
                var line = lines[i].TrimStart();

                var tokens = TokenizeLine(line, delimiters);
                appendSentence(new Sentence
                {
                    Line = i,
                    Tokens = tokens
                });
            }

            return firstSentence;
        }

        private static List<Token> TokenizeLine(string line, dynamic delimiters)
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
                var isDelimiterToken = delimiters.Delimiters.TryGetValue(c, out tokenType);
                if (!isDelimiterToken)
                {
                    continue;
                }

                var isEscapedToken = 
                    Grammar.EscapableTokens.Contains(tokenType) &&
                    i > 0 && line[i - 1] == Grammar.Backslash;

                if (isEscapedToken)
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
                var ignoreOtherPropertyValueDelimiter = delimiterTokens.Any(t => t.Type == TokenType.ProperetyValueDelimiter);
                if (ignoreOtherPropertyValueDelimiter)
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
}