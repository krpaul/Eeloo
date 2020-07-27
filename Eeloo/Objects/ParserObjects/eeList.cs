using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Eeloo.Objects.ParserObjects
{
    public class eeList
    /* The eeList implementation:
     * 
     * The eeList is a linked list indexed by eeNumber, allowing it 
     * to (theoretically) have an infinite length (within memory constraints).
     * 
     */
    {
        public eeListNode HeadNode = null,
                          LastNode = null;

        private eeNumber _count = new eeNumber(0);

        public eeObject this[eeNumber key]
        {
            get // retrieve
            {
                return null;
            }
            set // assign
            {
                return;
            }
        }

        public eeList(eeObject firstObject)
        {
            var firstNode = new eeListNode(firstObject, _count.Copy(), null, null);
            this.HeadNode = firstNode;
        }

        public void Append(eeObject obj)
        {
            _count += eeNumber.ONE;
            if (LastNode == null) // if this is the second object to enter the list
            {
                var node = new eeListNode(obj.Copy(), _count.Copy(), HeadNode, null);
                LastNode = node;
                HeadNode.next = node;
                return;
            }

            var newNode = new eeListNode(obj.Copy(), _count.Copy(), LastNode, null);
            this.LastNode.next = newNode;
            this.LastNode = newNode;
        }

        public eeNumber NodeCount
        {
            get
            { return this._count.Copy(); }
        }

        private IEnumerable<eeListNode> Iterate()
        {
            eeListNode currNode = this.HeadNode;
            while (currNode != null)
            {
                yield return currNode;
                currNode = currNode.next;
            }
        }

        public override string ToString()
        {
            var ret = new StringBuilder("[ ");
            foreach (var node in Iterate())
                ret.Append(node.ToString() + " -> ");
            ret.Append(" ]");
            return ret.ToString();
        }

        public class eeListNode
        {
            private eeObject obj;
            public eeListNode prev, next;
            public eeNumber index;
            
            public eeListNode(eeObject obj, eeNumber index, eeListNode prev, eeListNode next)
            {
                this.obj = obj;
                this.index = index;
                this.prev = prev;
                this.next = next;
            }

            public eeObject GetObject()
            { return this.obj; }

            public override string ToString()
            { return this.obj.ToPrintableString(); }
        }
    }
}
