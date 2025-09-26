using FluentBehaviourTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        //[Fact]
        //public void CanWait()
        //{
        //    Init();

        //    var time = new TimeData();

        //    var invokeCount = 0;

        //    Stopwatch stopwatch = new Stopwatch();

        //    var waitNode = new WaitNode("wait-action", 1000);
        //    var startTimerNode =
        //        new ActionNode(
        //            "some-action",
        //            t =>
        //            {
        //                ++invokeCount;
        //                stopwatch.Start();
        //                //Assert.Equal(time, t);
        //                return BehaviourTreeStatus.Success;
        //            }
        //        );
        //    var actionNode =
        //        new ActionNode(
        //            "some-action",
        //            t =>
        //            {
        //                stopwatch.Stop();
        //               // Assert.Equal(time, t);

        //                ++invokeCount;
        //                return BehaviourTreeStatus.Success;
        //            }
        //        );

        //    testObject.AddChild(startTimerNode);
        //    testObject.AddChild(waitNode);
        //    testObject.AddChild(actionNode);

        //    testObject.Tick(time);
            
        //    //Thread.Sleep(1000);
        //    Assert.Equal(BehaviourTreeStatus.Running, testObject.Tick(time));
        //    Assert.Equal(2, invokeCount);
        //    Assert.InRange(stopwatch.ElapsedMilliseconds, 1000,1100);
        //}
        
        [Fact]
        public void CanWait2()
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
                .Wait("wait",1000)
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
                Assert.InRange(st.ElapsedMilliseconds, 1000, 1200);
            }
            
            //Assert.Equal(BehaviourTreeStatus.Success, sequence.Tick(new TimeData()));
            
        }
    }
}