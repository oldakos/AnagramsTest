using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AnagramsTest
{
    internal class Node
    {
        internal void AddSuffix(IEnumerable<char> word)
        {
            Node currentNode = this;
            Node nextNode = this;
            foreach (char c in word)
            {
                bool foundNext = currentNode.TryGetChild(ref nextNode, c);
                if (foundNext)
                {
                    currentNode = nextNode;
                }
                else
                {
                    nextNode = currentNode.CreateChild(c);
                    currentNode = nextNode;
                }
            }
            if (!currentNode.IsLeaf)
            {
                currentNode.MakeLeaf();
            }
        }

        internal enum SearchResult
        {
            ABSENT,
            FULL,
            PARTIAL
        }

        /// <summary>
        /// Check whether a path given by the word exists through this Node's children.
        /// </summary>
        /// <param name="word">The characters following this node's character.</param>
        /// <returns>ABSENT if given word not traversable.<br/>FULL if traversable and final node is marked as end of a dictionary entry.<br/>PARTIAL if traversable but final node not end of any dictionary entry.</returns>
        internal SearchResult CheckSuffix(IEnumerable<char> word)
        {
            //depends on all lowercase input

            Node currentNode = this;
            Node nextNode = this;
            foreach (char c in word)
            {
                bool foundNext = currentNode.TryGetChild(ref nextNode, Char.ToLower(c));
                if (foundNext)
                {
                    currentNode = nextNode;
                }
                else
                {
                    return SearchResult.ABSENT;
                }
            }
            if (currentNode.IsLeaf)
            {
                return SearchResult.FULL;
            }
            else
            {
                return SearchResult.PARTIAL;
            }
        }        

        private static readonly Node NULLNODE = new Node('\0');

        readonly internal char Value;
        readonly internal List<Node> Children;

        internal Node(char value)
        {
            Value = value;
            Children = new List<Node>();
        }

        internal Node CreateChild(char value)
        {
            Node n = new Node(value);
            Children.Add(n);
            return n;
        }

        internal bool TryGetChild(ref Node result, char value)
        {
            foreach (var child in Children)
            {
                if (child.Value == value)
                {
                    result = child;
                    return true;
                }
            }
            return false;
        }

        internal void MakeLeaf()
        {
            Children.Add(NULLNODE);
        }

        internal bool IsLeaf
        {
            get
            {
                return Children.Contains(NULLNODE);
            }
        }

    }
}
