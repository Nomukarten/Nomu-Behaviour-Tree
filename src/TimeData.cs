using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Represents time. Used to pass time values to behaviour tree nodes.
    /// </summary>
    public struct TimeData
    {
        public TimeData(double deltaTime)
        {
            this.deltaTime = deltaTime;
        }

        public double deltaTime;
    }
}
