using System.Windows;
using ImageTools.Infrastructure.Converters;
using Xunit;

namespace ImageTools.Tests.Infrastructure.Converters
{
    public class StringToVisibilityConverterTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(123)]
        [InlineData(true)]
        [InlineData(false)]
        public void Convert_ShouldReturnCollapsed_WhenValueIsNotString(object value)
        {
            var sut = new StringToVisibilityConverter();

            var result = sut.Convert(value, null, null, null);

            Assert.Equal(Visibility.Collapsed, result);
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData("1")]
        public void Convert_ShouldReturnVisible_WhenValueIsString(object value)
        {
            var sut = new StringToVisibilityConverter();

            var result = sut.Convert(value, null, null, null);

            Assert.Equal(Visibility.Visible, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(123)]
        [InlineData(true)]
        [InlineData(false)]
        public void Convert_ShouldReturnCollapsed_WhenValueIsNotStringAndParameterIsFalse(object value)
        {
            var sut = new StringToVisibilityConverter();

            var result = sut.Convert(value, null, false, null);

            Assert.Equal(Visibility.Collapsed, result);
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData("1")]
        public void Convert_ShouldReturnVisible_WhenValueIsStringAndParameterIsFalse(object value)
        {
            var sut = new StringToVisibilityConverter();

            var result = sut.Convert(value, null, false, null);

            Assert.Equal(Visibility.Visible, result);
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData("1")]
        public void Convert_ShouldReturnCollapsed_WhenValueIsStringAndParameterIsTrue(object value)
        {
            var sut = new StringToVisibilityConverter();

            var result = sut.Convert(value, null, true, null);

            Assert.Equal(Visibility.Collapsed, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(123)]
        [InlineData(true)]
        [InlineData(false)]
        public void Convert_ShouldReturnVisible_WhenValueIsNotStringAndParameterIsTrue(object value)
        {
            var sut = new StringToVisibilityConverter();

            var result = sut.Convert(value, null, true, null);

            Assert.Equal(Visibility.Visible, result);
        }
    }
}
