using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// A behaviour tree leaf node for running an action.
    /// </summary>
    public class ActionNode : IBehaviourTreeNode
    {

        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// Current node state
        /// </summary>
        private BehaviourTreeStatus nodeState;

        /// <summary>
        /// Function to invoke for the action.
        /// </summary>
        private Func<TimeData, BehaviourTreeStatus> fn;
        

        public ActionNode(string name, Func<TimeData, BehaviourTreeStatus> fn)
        {
            this.name=name;
            this.fn=fn;
            nodeState = BehaviourTreeStatus.Ready;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            if(nodeState != BehaviourTreeStatus.Success)
                nodeState = fn(time);
            return nodeState;
        }

        public void Reset()
        {
            nodeState = BehaviourTreeStatus.Ready;
        }
    }
}
