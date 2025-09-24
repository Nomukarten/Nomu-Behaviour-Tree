using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Runs child nodes in sequence, until one fails.
    /// </summary>
    public class SequenceNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: this could be optimized as a baked array.

        /// <summary>
        /// Index of the last entered node
        /// </summary>
        private int currentIndex;

        public SequenceNode(string name)
        {
            this.name = name;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            while (currentIndex<children.Count)
            {
                var childStatus = children[currentIndex].Tick(time);

                if (childStatus == BehaviourTreeStatus.Running)
                {
                    return BehaviourTreeStatus.Running;
                }

                if (childStatus == BehaviourTreeStatus.Failure)
                {
                    // reset for next time
                    currentIndex = 0;
                    return BehaviourTreeStatus.Failure;
                }

                currentIndex++;
            }

            currentIndex = 0;
            return BehaviourTreeStatus.Success;

            //foreach (var child in children)
            //{
            //    var childStatus = child.Tick(time);
            //    if (childStatus != BehaviourTreeStatus.Success)
            //    {
            //        return childStatus;
            //    }
            //}

            //return BehaviourTreeStatus.Success;
        }

        public void Reset()
        {
            currentIndex = 0;
            foreach (var child in children)
            {
                child.Reset();
            }
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
