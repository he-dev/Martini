using System.Collections.Generic;
using System.Diagnostics;

namespace Martini.Collections
{
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
}