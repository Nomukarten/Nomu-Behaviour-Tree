using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Interface for behaviour tree nodes.
    /// </summary>
    public interface IBehaviourTreeNode
    {
        /// <summary>
        /// Update the time of the behaviour tree.
        /// </summary>
        BehaviourTreeStatus Tick(TimeData time);

        /// <summary>
        /// Reset the state of the node
        /// </summary>
        void Reset();

        /// <summary>
        /// Get current node state
        /// </summary>
        BehaviourTreeStatus GetState();

    }
}
