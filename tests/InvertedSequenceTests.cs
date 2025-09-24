using FluentBehaviourTree;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace tests
{
    public class InvertedSequenceTests
    {
        InvertedSequenceNode testObject;

        void Init()
        {
            testObject = new InvertedSequenceNode("some-sequence");
        }
        [Fact]
        public void when_first_child_fails_then_sequence_succeeds()
        {
            Init();

            var time = new TimeData();

            var mockChild1 = new Mock<IBehaviourTreeNode>();
            mockChild1
                .Setup(m => m.Tick(time))
                .Returns(BehaviourTreeStatus.Failure);

            var mockChild2 = new Mock<IBehaviourTreeNode>();

            testObject.AddChild(mockChild1.Object);
            testObject.AddChild(mockChild2.Object);

            Assert.Equal(BehaviourTreeStatus.Success, testObject.Tick(time));

            mockChild1.Verify(m => m.Tick(time), Times.Once());
            mockChild2.Verify(m => m.Tick(time), Times.Never());
        }

        [Fact]
        public void when_second_child_fails_then_sequence_succeeds()
        {
            Init();

            var time = new TimeData();

            var mockChild1 = new Mock<IBehaviourTreeNode>();

            mockChild1
                .Setup(m => m.Tick(time))
                .Returns(BehaviourTreeStatus.Success);

            var mockChild2 = new Mock<IBehaviourTreeNode>();
            mockChild2
                .Setup(m => m.Tick(time))
                .Returns(BehaviourTreeStatus.Failure);

            testObject.AddChild(mockChild1.Object);
            testObject.AddChild(mockChild2.Object);

            Assert.Equal(BehaviourTreeStatus.Success, testObject.Tick(time));

            mockChild1.Verify(m => m.Tick(time), Times.Once());
            mockChild2.Verify(m => m.Tick(time), Times.Once());
        }

        [Fact]
        public void when_sequence_succeeds_returns_failure()
        {
            Init();

            var time = new TimeData();

            var mockChild1 = new Mock<IBehaviourTreeNode>();
                mockChild1.Setup(m => m.Tick(time))
                .Returns(BehaviourTreeStatus.Success);

            var mockChild2 = new Mock<IBehaviourTreeNode>();
            mockChild2.Setup(m => m.Tick(time))
                .Returns(BehaviourTreeStatus.Success);

            testObject.AddChild(mockChild1.Object);
            testObject.AddChild(mockChild2.Object);

            Assert.Equal(BehaviourTreeStatus.Failure, testObject.Tick(time));

            mockChild1.Verify(m => m.Tick(time), Times.Once());
            mockChild2.Verify(m => m.Tick(time), Times.Once());
        }
    }
}
