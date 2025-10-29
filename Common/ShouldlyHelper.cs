using Google.Protobuf.Compiler;
using Mysqlx.Crud;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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
