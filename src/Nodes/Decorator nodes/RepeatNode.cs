using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Runs child nodes in sequence, until one fails.
    /// </summary>
    public class RepeatNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// Number of times to repeat.
        /// </summary>
        private uint count;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: this could be optimized as a baked array.

        public RepeatNode(string name)
        {
            this.name = name;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            BehaviourTreeStatus childStatus = BehaviourTreeStatus.Failure;

            for (int i = 0; i < count; i++)
            {
                foreach (var child in children)
                {
                    childStatus = child.Tick(time);
                    if (childStatus != BehaviourTreeStatus.Success)
                    {
                        break;
                    }
                }
            }

            return childStatus;
        }

        /// <summary>
        /// Add a child to the sequence.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}