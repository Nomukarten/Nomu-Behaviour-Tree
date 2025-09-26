using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Current node state
        /// </summary>
        private BehaviourTreeStatus nodeState;

        /// <summary>
        /// The amount of time this node waits in ms.
        /// </summary>
        public double durationMilliseconds;

        private Stopwatch stopwatch;

        public WaitNode(string name, double durationMilliseconds)
        {
            this.name = name;
            this.durationMilliseconds = durationMilliseconds;
            stopwatch = new Stopwatch();

            nodeState = BehaviourTreeStatus.Ready;
        }

        //using stopwatch isnt ideal, it way be inaccurate on different systems
        //but for now im not going to waste more time on this node than i need to
        public BehaviourTreeStatus Tick(TimeData time)
        {
            if (nodeState == BehaviourTreeStatus.Ready)
            {
                stopwatch.Start();
            }

            //elapsed += time.deltaTime;
            if (stopwatch.ElapsedMilliseconds >= durationMilliseconds)
            {
                stopwatch.Stop();
                return BehaviourTreeStatus.Success;
            }
            return BehaviourTreeStatus.Running;
        }

        public void Reset()
        {
            nodeState = BehaviourTreeStatus.Ready;
            durationMilliseconds = 0;
            stopwatch.Reset();
        }
    }
}