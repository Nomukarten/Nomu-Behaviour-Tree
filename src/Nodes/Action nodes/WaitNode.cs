using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

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

        private long startTimestampTicks = -1;
        private static readonly double TimestampTickToMilliseconds = 1000.0 / Stopwatch.Frequency;
        private long Ticks;
        private long elapsedTicks;
        private double elapsedMs;

        public WaitNode(string name, double durationMilliseconds)
        {
            this.name = name;
            this.durationMilliseconds = durationMilliseconds;
            nodeState = BehaviourTreeStatus.Ready;

            Ticks = 0;
            elapsedTicks = 0;
            elapsedMs = 0;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            if (nodeState == BehaviourTreeStatus.Ready)
            {
                nodeState = BehaviourTreeStatus.Running;
                startTimestampTicks = Stopwatch.GetTimestamp();
            }

            Ticks = Stopwatch.GetTimestamp();
            elapsedTicks = Ticks - startTimestampTicks;
            elapsedMs = elapsedTicks * TimestampTickToMilliseconds;

            if (elapsedMs >= durationMilliseconds)
            {
                // Reset for potential re-use on next activation
                nodeState = BehaviourTreeStatus.Ready;
                startTimestampTicks = -1;
                return BehaviourTreeStatus.Success;
            }
            return BehaviourTreeStatus.Running;
        }

        public void Reset()
        {
            nodeState = BehaviourTreeStatus.Ready;
            startTimestampTicks = -1;
            Ticks = 0;
            elapsedTicks = 0;
            elapsedMs = 0;
        }

        public BehaviourTreeStatus GetState()
        {
            return nodeState;
        }
    }
}