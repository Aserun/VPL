using System.Linq;
using CaptiveAire.VPL.Model;
using Xunit;

namespace CaptiveAire.VPL.Tests
{
    public class HierarchicalNumberTests
    {
        [Theory]
        [InlineData("1", new int[] { 1 })]
        [InlineData("1.5", new int[] { 1, 5 })]
        [InlineData("6.4.1.1", new int[] { 6, 4, 1, 1 })]
        public void ParseTests(string s, int[] components)
        {
            HierarchicalNumber parsed = new HierarchicalNumber(s);

            Assert.Equal(components, parsed.ToArray());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("asdf")]
        [InlineData("1.a")]
        [InlineData("1.")]
        public void ParseNegativeTests(string s)
        {
            HierarchicalNumber result;

            bool didParse = HierarchicalNumber.TryParse(s, out result);

            Assert.False(didParse);
        }

        [Theory]
        [InlineData("1", "2")]
        [InlineData("1.1", "1.2")]
        [InlineData("400.1.4.6", "400.1.4.7")]
        public void AddNextSibling(string s, string expected)
        {
            var parsed = new HierarchicalNumber(s);

            var nextSibling = parsed.CreateNextSibling();

            Assert.Equal(expected, nextSibling.ToString());
        }

        [Theory]
        [InlineData("1", "1.1")]
        [InlineData("64", "64.1")]
        [InlineData("1.2.3.4", "1.2.3.4.1")]
        [InlineData("100", "100.1")]
        [InlineData("101", "101.1")]
        [InlineData("1.2.34.46.678", "1.2.34.46.678.1")]
        public void CreateFirstChild(string s, string expected)
        {
            HierarchicalNumber parsed = new HierarchicalNumber(s);

            HierarchicalNumber fistChild = parsed.CreateFirstChild();

            Assert.Equal(expected, fistChild.ToString());
        }

        [Fact]
        public void TestEmptyConstructor()
        {
            HierarchicalNumber created = new HierarchicalNumber();

            Assert.Equal("1", created.ToString());
        }

    }
}