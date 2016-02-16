using System;
using System.Collections.Generic;
using System.Linq;
using Martini.Collections;

namespace Martini
{
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
            var sectionContents = sentence.After.Skip(1).TakeWhile(x => x.Type != SentenceType.Section);
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

        public static DynamicDictionary<TKey, TValue> ToDynamicDictionary<TSource, TKey, TValue>(
            this IEnumerable<TSource> source, string keyName, string valueName, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector)
        {
            var dynamicDictionary = new DynamicDictionary<TKey, TValue>(keyName, valueName);

            foreach (var item in source)
            {
                dynamicDictionary.Add(keySelector(item), elementSelector(item));
            }

            return dynamicDictionary;
        }

        public static List<List<Sentence>> DuplicateSectionGroups(this Sentence sentence)
        {
            if (sentence.Previous != null) { throw new ArgumentException("Sentence must be first."); }
            if (!sentence.After.Any()) { return new List<List<Sentence>>(); }

            var duplicateSecions =
                sentence.After.Sections()
                .GroupBy(x => (string)x.SectionToken(), (name, sentences) => sentences.ToList())
                .Where(x => x.Count > 1)
                .ToList();

            return duplicateSecions;
        }
    }
}