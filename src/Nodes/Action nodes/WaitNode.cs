using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// A behaviour tree leaf node for running an action.
    /// </summary>
    public class WaitNode : IBehaviourTreeNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// The amount of time this node waits in ms.
        /// </summary>
        public double duration;

        /// <summary>
        /// Timing amount.
        /// </summary>
        public double elapsed;

        public WaitNode(string name, double duration)
        {
            this.name = name;
            this.duration = duration;
            this.elapsed = 0;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            elapsed += time.deltaTime;
            if (elapsed >= duration)
            {
                elapsed = 0;
                return BehaviourTreeStatus.Success;
            }
            return BehaviourTreeStatus.Running;
        }
    }
}