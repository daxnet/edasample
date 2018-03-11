using EdaSample.Tests.Models;
using System;
using Xunit;

namespace EdaSample.Tests
{
    public class AggregateRootTests
    {
        [Fact]
        public void CreateBookTest()
        {
            var book = new Book();
            Assert.NotEqual(Guid.Empty, book.Id);
        }
    }
}
