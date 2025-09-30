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
        /// Track overall node state
        /// </summary>
        protected BehaviourTreeStatus nodeState = BehaviourTreeStatus.Ready;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        protected List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: this could be optimized as a baked array.

        /// <summary>
        /// Current iteration index (0-based)
        /// </summary>
        protected uint currentIteration;

        /// <summary>
        /// Index of the child currently being processed
        /// </summary>
        protected int currentChildIndex;

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
            if (nodeState == BehaviourTreeStatus.Ready)
            {
                nodeState = BehaviourTreeStatus.Running;
            }
            BehaviourTreeStatus childStatus = BehaviourTreeStatus.Failure;

            while (currentIteration < count)
            {
                while (currentChildIndex < children.Count)
                {
                    childStatus = children[currentChildIndex].Tick(time);

                    if (childStatus == BehaviourTreeStatus.Running)
                    {
                        //if running stop current repeat loop for next tree re run
                        return BehaviourTreeStatus.Running;
                    }

                    if (childStatus == BehaviourTreeStatus.Failure)
                    {
                        // On failure, reset state for next tree loop
                        currentChildIndex = 0;
                        nodeState = BehaviourTreeStatus.Ready;
                        ResetChildren();
                        break;
                    }

                    // Child succeeded, advance to next child
                    currentChildIndex++;
                }

                // Clear children on completed iteration
                ResetChildren();
                currentChildIndex = 0;
                currentIteration++;
            }

            //return last completed child status
            return childStatus;
        }

        public void Reset()
        {
            currentIteration = 0;
            currentChildIndex = 0;
            nodeState = BehaviourTreeStatus.Ready;
            ResetChildren();
        }

        public BehaviourTreeStatus GetState()
        {
            return nodeState;
        }

        protected void ResetChildren()
        {
            currentChildIndex = 0;
            nodeState = BehaviourTreeStatus.Ready;

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

            if (nodeState == BehaviourTreeStatus.Ready)
            {
                nodeState = BehaviourTreeStatus.Running;
            }
            BehaviourTreeStatus childStatus = BehaviourTreeStatus.Failure;

            while (currentIteration < count)
            {
                while (currentChildIndex < children.Count)
                {
                    childStatus = children[currentChildIndex].Tick(time);

                    if (childStatus == BehaviourTreeStatus.Running)
                    {
                        //if running stop current repeat loop for next tree re run
                        return BehaviourTreeStatus.Running;
                    }

                    if (childStatus == BehaviourTreeStatus.Failure)
                    {
                        // On failure, reset state for next tree loop
                        ResetChildren();
                        return finalState;
                    }

                    // Child succeeded, advance to next child
                    currentChildIndex++;
                }

                // Clear children on completed iteration
                ResetChildren();
                currentIteration++;
            }

            //return last completed child status
            currentIteration = 0;
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
            if (nodeState == BehaviourTreeStatus.Ready)
            {
                nodeState = BehaviourTreeStatus.Running;
            }
            BehaviourTreeStatus childStatus = BehaviourTreeStatus.Failure;

            while (currentIteration < count)
            {
                while (currentChildIndex < children.Count)
                {
                    childStatus = children[currentChildIndex].Tick(time);

                    if (childStatus == BehaviourTreeStatus.Running)
                    {
                        //if running stop current repeat loop for next tree re run
                        return BehaviourTreeStatus.Running;
                    }
                    else
                    {
                        ResetChildren();

                        if(childStatus == BehaviourTreeStatus.Success)
                            return finalState;

                        break;
                    }

                    // Child succeeded, advance to next child
                    currentChildIndex++;
                }

                // Clear children on completed iteration
                ResetChildren();
                currentIteration++;
            }

            //return last completed child status
            currentIteration = 0;
            return childStatus;
        }
    }
}