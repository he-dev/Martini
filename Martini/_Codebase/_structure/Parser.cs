using System;
using System.Collections.Generic;
using System.Linq;

namespace Martini
{
    internal class Parser
    {
        public static Sentence Parse(string ini, IniSettings settings)
        {
            var firstSentence = Tokenizer.Tokenize(ini, settings.Delimiters);
            DetermineSentenceType(firstSentence);

            HandleDuplicateSections(firstSentence, settings.DuplicateSectionHandling);
            HandleDuplicateProperties(firstSentence, settings.DuplicatePropertyHandling);

            return firstSentence;
        }

        internal static void DetermineSentenceType(Sentence sentence)
        {
            foreach (var currentSentence in sentence.After)
            {
                var isBlank = !currentSentence.Tokens.Any();
                if (isBlank)
                {
                    currentSentence.Type = SentenceType.Blank;
                    continue;
                }

                var sentenceDefinitionMatch =
                    (from sentenceDefinition in Grammar.SentenceDefinitions
                     from tokens in sentenceDefinition.AllowedTokenTypes
                         // compare tokens with token patterns to identify the type of sentence
                     where tokens.SequenceEqual(currentSentence.Tokens.Select(t => t.Type))
                     select sentenceDefinition).SingleOrDefault();

                var foundMatch = sentenceDefinitionMatch != null;
                if (!foundMatch)
                {
                    currentSentence.Type = SentenceType.Invalid;
                    continue;
                }

                // update sentence type
                currentSentence.Type = sentenceDefinitionMatch.SentenceType;

                // update token types to exact types

                var exactTokenTypes = sentenceDefinitionMatch.ExactTokenTypes;
                for (var i = 0; i < sentenceDefinitionMatch.ExactTokenTypes.Length; i++)
                {
                    var exactTokenType = exactTokenTypes[i];

                    var containsToken = i < currentSentence.Tokens.Count;
                    if (containsToken)
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

        internal static void HandleDuplicateSections(Sentence firstSentence, DuplicateSectionHandling duplicateSectionHandling)
        {
            if (duplicateSectionHandling == DuplicateSectionHandling.Allow)
            {
                return;
            }

            var duplicateSectionsGroups = firstSentence.DuplicateSectionGroups();

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
                foreach (var duplicateSecionGroup in duplicateSectionsGroups)
                {
                    var baseSection = duplicateSecionGroup.First();
                    var mergeSections = duplicateSecionGroup.Skip(1);
                    foreach (var mergeSection in mergeSections)
                    {
                        var properties = mergeSection.Properties().ToList();
                        foreach (var property in properties)
                        {
                            var lastBaseSectionProperty = baseSection.Properties().Last();
                            lastBaseSectionProperty.Next = property;
                        }
                        mergeSection.Remove();
                    }
                }
            }
        }

        internal static void HandleDuplicateProperties(Sentence firstSentence, DuplicatePropertyHandling duplicatePropertyHandling)
        {
            if (duplicatePropertyHandling == DuplicatePropertyHandling.Allow)
            {
                return;
            }

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var section in firstSentence.After.Sections())
            {
                var duplicatePropertyGroups =
                    section.Properties()
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
    }
}