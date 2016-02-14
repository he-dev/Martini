using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Martini.Collections;

namespace Martini._data
{
    [DebuggerDisplay("{ToString()} at {Line}")]
    internal class Sentence
    {
        // sentences are linked with each other by the linked-object 
        // thus they build a "linked list" that can easly be queried with linq


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

        public IEnumerable<Sentence> Before => _linkedObject.Before.Select(x => (Sentence)x);

        public IEnumerable<Sentence> After => _linkedObject.After.Select(x => (Sentence)x);

        public void Remove() => _linkedObject.Remove();

        public override string ToString() => string.Join(string.Empty, Tokens);
    }
}