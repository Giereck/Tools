using ImageTools.Infrastructure.Converters;
using Xunit;

namespace ImageTools.Tests.Infrastructure.Converters
{
    public class NullToBooleanConverterTests
    {
        [Fact]
        public void Convert_ShouldReturnTrue_WhenValueIsNull()
        {
            var sut = new NullToBooleanConverter();

            var result = sut.Convert(null, null, null, null);

            Assert.IsType<bool>(result);
            Assert.True((bool) result);
        }

        [Fact]
        public void Convert_ShouldReturnFalse_WhenValueIsNullAndParameterIsTrue()
        {
            var sut = new NullToBooleanConverter();

            var result = sut.Convert(null, null, true, null);

            Assert.IsType<bool>(result);
            Assert.False((bool)result);
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData(1)]
        [InlineData(true)]
        [InlineData(false)]
        public void Convert_ShouldReturnFalse_WhenValueIsNotNull(object value)
        {
            var sut = new NullToBooleanConverter();

            var result = sut.Convert(value, null, null, null);

            Assert.IsType<bool>(result);
            Assert.False((bool)result);
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData(1)]
        [InlineData(true)]
        [InlineData(false)]
        public void Convert_ShouldReturnTrue_WhenValueIsNotNullAndParameterIsTrue(object value)
        {
            var sut = new NullToBooleanConverter();

            var result = sut.Convert(value, null, true, null);

            Assert.IsType<bool>(result);
            Assert.True((bool)result);
        }
    }
}