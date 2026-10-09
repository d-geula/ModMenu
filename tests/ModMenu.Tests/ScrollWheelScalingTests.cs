using ModMenu.UI;
using Xunit;

namespace ModMenu.Tests
{
    public class ScrollWheelScalingTests
    {
        [Fact]
        public void OtherMenusKeepTheirSensitivityUnlessOptedIn()
        {
            Assert.Equal(90f, ScrollWheelScaling.ScaleSensitivity(90f, 3f, false, false));
            Assert.Equal(270f, ScrollWheelScaling.ScaleSensitivity(90f, 3f, false, true));
            Assert.Equal(120f, ScrollWheelScaling.ScaleSensitivity(40f, 3f, true, false));
        }

        [Theory]
        [InlineData(0f, 3f, 0f)]
        [InlineData(-40f, 3f, -120f)]
        [InlineData(40f, 1f, 40f)]
        [InlineData(40f, 0.5f, 20f)]
        public void PreservesDisabledOrReversedScrollingAndAllowsOriginalOrSlowerSpeed(float original, float multiplier, float expected)
        {
            Assert.Equal(expected, ScrollWheelScaling.ScaleSensitivity(original, multiplier, true, false));
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-3f)]
        [InlineData(float.NaN)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.NegativeInfinity)]
        public void InvalidMultiplierLeavesMenuUnchanged(float multiplier)
        {
            Assert.Equal(40f, ScrollWheelScaling.ScaleSensitivity(40f, multiplier, true, true));
        }

        [Fact]
        public void OverflowLeavesMenuUnchanged()
        {
            Assert.Equal(float.MaxValue, ScrollWheelScaling.ScaleSensitivity(float.MaxValue, 10f, true, true));
        }

        [Theory]
        [InlineData(float.NaN)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.NegativeInfinity)]
        public void InvalidOriginalSensitivityIsNotRewritten(float original)
        {
            Assert.Equal(original, ScrollWheelScaling.ScaleSensitivity(original, 3f, true, true));
        }
    }
}
