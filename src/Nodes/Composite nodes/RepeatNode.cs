using System;
using System.Collections.Generic;

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
        protected string name;

        /// <summary>
        /// Number of times to repeat.
        /// </summary>
        protected uint count;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        protected List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: this could be optimized as a baked array.

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="count">Iteration count - prevents method from running endlessly</param>
        public RepeatNode(string name, uint count)
        {
            this.name = name;
            this.count = count;
        }

        /// <summary>
        /// Node tick
        /// </summary>
        /// <param name="time">Time data passed to each node</param>
        /// <returns>Final completed child status</returns>
        public BehaviourTreeStatus Tick(TimeData time)
        {
            BehaviourTreeStatus childStatus = BehaviourTreeStatus.Failure;

            for (int i = 0; i < count; i++)
            {
                foreach (var child in children)
                {
                    childStatus = child.Tick(time);
                    if (childStatus == BehaviourTreeStatus.Failure)
                    {
                        ResetChildren();
                        break;
                    }
                }

            }

            return childStatus;
        }

        public void Reset()
        {
            count = 0;
            ResetChildren();
        }

        protected void ResetChildren()
        {
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

    public class RepeatUntilFailureNode : RepeatNode
    {
        /// <summary>
        /// State of the tree after completion
        /// </summary>
        protected BehaviourTreeStatus finalState;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="count">Iteration count - prevents method from running endlessly</param>
        /// <param name="finalStatus">Status returned by finished repeater sequence, true for Success, false for Failure</param>
        public RepeatUntilFailureNode(string name, uint count, bool finalStatus) : base(name, count)
        {
            this.name = name;
            this.count = count;
            this.finalState = finalStatus ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }

        public new BehaviourTreeStatus Tick(TimeData time)
        {
            BehaviourTreeStatus childStatus = BehaviourTreeStatus.Failure;

            for (int i = 0; i < count; i++)
            {
                foreach (var child in children)
                {
                    childStatus = child.Tick(time);

                    if (childStatus == BehaviourTreeStatus.Failure)
                    {
                        return finalState;
                    }
                }
            }

            return childStatus;
        }
    }

    public class RepeatUntilSuccessNode:RepeatUntilFailureNode
    {
        public RepeatUntilSuccessNode(string name, uint count, bool finalStatus) : base(name, count, finalStatus)
        {
        }

        public new BehaviourTreeStatus Tick(TimeData time)
        {
            BehaviourTreeStatus childStatus = BehaviourTreeStatus.Failure;

            for (int i = 0; i < count; i++)
            {
                foreach (var child in children)
                {
                    childStatus = child.Tick(time);

                    if (childStatus == BehaviourTreeStatus.Success)
                    {
                        return finalState;
                    }
                }
            }

            return childStatus;
        }
    }
}