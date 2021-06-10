using System;
using NUnit.Framework;

namespace EnvironmentVariableTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.That(Environment.GetEnvironmentVariable("foo"), Is.EqualTo("bar"));
        }
    }
}