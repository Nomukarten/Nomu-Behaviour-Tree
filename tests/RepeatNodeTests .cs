using FluentBehaviourTree;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace tests
{
    public class RepeatNodeTests
    {
        RepeatNode repeatNode;
        RepeatUntilFailureNode repeatFailureNode;
        RepeatUntilSuccessNode repeatSuccessNode;

        void Init()
        {
            repeatNode = new RepeatNode("some-sequence", 10);
            repeatFailureNode = new RepeatUntilFailureNode("some-sequence", 10, true);
            repeatSuccessNode = new RepeatUntilSuccessNode("some-sequence", 10, false);
        }

        [Fact]
        public void can_repeat_sequence()
        {
            Init();
            int executionCount = 0;

            var time = new TimeData();

            var mockChild1 = new Mock<IBehaviourTreeNode>();
            mockChild1
                .Setup(m => m.Tick(time))
                .Returns(BehaviourTreeStatus.Success)
                .Callback(() => { executionCount++; });

            var mockChild2 = new Mock<IBehaviourTreeNode>();
            mockChild2
                .Setup(m => m.Tick(time))
                .Returns(BehaviourTreeStatus.Success)
                .Callback(() => { executionCount++; });

            repeatNode.AddChild(mockChild1.Object);
            repeatNode.AddChild(mockChild2.Object);

            Assert.Equal(BehaviourTreeStatus.Success, repeatNode.Tick(time));
            Assert.Equal(20,executionCount);
        }

        [Fact]
        public void sequence_repeats_even_if_child_returns_failure()
        {
            Init();
            int executionCount = 0;

            BehaviourTreeStatus loserStatus = BehaviourTreeStatus.Success;

            var time = new TimeData();

            var mockChild1 = new Mock<IBehaviourTreeNode>();
            mockChild1
                .Setup(m => m.Tick(time))
                .Returns(()=>loserStatus)
                .Callback(() =>
                {
                    executionCount++;
                    if (executionCount == 3)
                    {
                        loserStatus = BehaviourTreeStatus.Failure;
                    }
                });

            var mockChild2 = new Mock<IBehaviourTreeNode>();
            mockChild2
                .Setup(m => m.Tick(time))
                .Returns(BehaviourTreeStatus.Success)
                .Callback(() =>
                {
                    executionCount++;
                });

            repeatNode.AddChild(mockChild1.Object);
            repeatNode.AddChild(mockChild2.Object);

            Assert.Equal(BehaviourTreeStatus.Failure, repeatNode.Tick(time));
            mockChild2.Verify(m => m.Tick(time), Times.Exactly(2));
            Assert.Equal(12,executionCount);
        }

        [Fact]
        public void repeat_until_failure()
        {
            Init();
            int executionCount = 0;

            BehaviourTreeStatus loserStatus = BehaviourTreeStatus.Success;

            var time = new TimeData();

            var mockChild1 = new Mock<IBehaviourTreeNode>();
            mockChild1
                .Setup(m => m.Tick(time))
                .Returns(()=>loserStatus)
                .Callback(() =>
                {
                    executionCount++;
                    if (executionCount == 3)
                    {
                        loserStatus = BehaviourTreeStatus.Failure;
                    }
                });

            var mockChild2 = new Mock<IBehaviourTreeNode>();
            mockChild2
                .Setup(m => m.Tick(time))
                .Returns(BehaviourTreeStatus.Success)
                .Callback(() =>
                {
                    executionCount++;
                });

            repeatFailureNode.AddChild(mockChild1.Object);
            repeatFailureNode.AddChild(mockChild2.Object);

            Assert.Equal(BehaviourTreeStatus.Success, repeatFailureNode.Tick(time));
            Assert.Equal(5, executionCount);
        }


        [Fact]
        public void repeat_until_Success()
        {
            Init();
            int executionCount = 0;

            BehaviourTreeStatus loserStatus = BehaviourTreeStatus.Failure;

            var time = new TimeData();

            var mockChild1 = new Mock<IBehaviourTreeNode>();
            mockChild1
                .Setup(m => m.Tick(time))
                .Returns(() => loserStatus)
                .Callback(() =>
                {
                    executionCount++;
                    if (executionCount == 3)
                    {
                        loserStatus = BehaviourTreeStatus.Success;
                    }
                });

            var mockChild2 = new Mock<IBehaviourTreeNode>();
            mockChild2
                .Setup(m => m.Tick(time))
                .Returns(BehaviourTreeStatus.Failure)
                .Callback(() =>
                {
                    executionCount++;
                });

            repeatSuccessNode.AddChild(mockChild1.Object);
            repeatSuccessNode.AddChild(mockChild2.Object);

            Assert.Equal(BehaviourTreeStatus.Failure, repeatSuccessNode.Tick(time));
            Assert.Equal(4,executionCount);
        }

    }
}
