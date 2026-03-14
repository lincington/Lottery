using NUnit.Framework;
using Shouldly;

namespace Common
{
    public class ShouldlyHelper
    {
        [Test]
        public void BasicAssertionExamples()
        {
            // Basic equality
            var myValue = 42;
            myValue.ShouldBe(42);
            // String operations
            string name = "John Smith";
            name.ShouldContain("Smith");
            name.ShouldStartWith("John");
            name.ShouldNotBeNullOrEmpty();
        }
    }
}
