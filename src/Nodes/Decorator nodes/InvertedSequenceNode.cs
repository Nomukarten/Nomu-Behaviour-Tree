using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Inverts the sequence: if any node fails returns success. If it passes it returns failure. 
    /// </summary>
    public class InvertedSequenceNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// The sequence of nodes to be inverted.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>();

        public InvertedSequenceNode(string name)
        {
            this.name = name;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {

            foreach (var child in children)
            {
                var childStatus = child.Tick(time);
                if (childStatus != BehaviourTreeStatus.Success)
                {
                    if (childStatus == BehaviourTreeStatus.Failure) //if any node fails return true
                        return BehaviourTreeStatus.Success;

                    else return childStatus;
                }
            }
            return BehaviourTreeStatus.Failure; //if sequence completes return failure
        }

        public void Reset()
        {
            throw new NotImplementedException();
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
