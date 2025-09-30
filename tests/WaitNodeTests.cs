using FluentBehaviourTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Xunit;

namespace tests
{
    public class WaitNodeTests
    {
        BehaviourTreeBuilder testObject;
        void Init()
        {
            testObject = new BehaviourTreeBuilder();
        }

        [Fact]
        public void CanWait()
        {
            Init();
            var invokeCount = 0;
            Stopwatch st = new Stopwatch();

            var sequence = testObject.Sequence("some-sequence")
                .Do("some-action-1", t =>
                {
                    ++invokeCount;
                    st.Start();
                    return BehaviourTreeStatus.Success;
                })
                .Wait("wait", 1000)
                .Do("some-action-2", t =>
                {
                    ++invokeCount;
                    st.Stop();
                    return BehaviourTreeStatus.Success;
                })
                .End()
                .Build();

            TimeData time = new TimeData(1);
            BehaviourTreeStatus status = BehaviourTreeStatus.Running;
            while (status != BehaviourTreeStatus.Success)
            {
                status = sequence.Tick(time);
            }

            if (status == BehaviourTreeStatus.Success)
            {
                Assert.IsType<SequenceNode>(sequence);
                Assert.Equal(2, invokeCount);
                Assert.InRange(st.ElapsedMilliseconds, 1000, 1300);
            }
            
            //Assert.Equal(BehaviourTreeStatus.Success, sequence.Tick(new TimeData()));
            
        }

        [Fact]
        public void repeat_wait_node()
        {
            Init();
            var invokeCount = 0;
            Stopwatch st = new Stopwatch();

            var sequence = testObject.Repeat("some-sequence", 3)
                .Do("some-action-1", t =>
                {
                    ++invokeCount;
                    return BehaviourTreeStatus.Success;
                })
                .Wait("wait", 1000)
                .Do("some-action-2", t =>
                {
                    ++invokeCount;
                    return BehaviourTreeStatus.Success;
                })
                .End()
                .Build();

            TimeData time = new TimeData(1);

            BehaviourTreeStatus status = BehaviourTreeStatus.Running;
            st.Start();
            while (status != BehaviourTreeStatus.Success)
            {
                status = sequence.Tick(time);
            }
            st.Stop();

            Assert.Equal(6, invokeCount);
            Assert.InRange(st.ElapsedMilliseconds, 3000, 3200);

        }
    }
}