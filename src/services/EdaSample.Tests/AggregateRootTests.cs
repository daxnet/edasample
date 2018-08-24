// ============================================================================
//   ______    _        _____                       _
//  |  ____|  | |      / ____|                     | |
//  | |__   __| | __ _| (___   __ _ _ __ ___  _ __ | | ___
//  |  __| / _` |/ _` |\___ \ / _` | '_ ` _ \| '_ \| |/ _ \
//  | |___| (_| | (_| |____) | (_| | | | | | | |_) | |  __/
//  |______\__,_|\__,_|_____/ \__,_|_| |_| |_| .__/|_|\___|
//                                           | |
//                                           |_|
// MIT License
//
// Copyright (c) 2017-2018 Sunny Chen (daxnet)
//
// ============================================================================

using EdaSample.Common;
using EdaSample.Common.Events.Domain;
using EdaSample.Common.Repositories;
using EdaSample.Tests.Models;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EdaSample.Tests
{
    public class AggregateRootTests
    {
        [Fact]
        public void CreateBookTest()
        {
            // Arrange & Act
            var book = new Book();
            // Assert
            Assert.NotEqual(Guid.Empty, book.Id);
            Assert.Equal(1, book.Version);
        }

        [Fact]
        public void ChangeBookTitleEventTest()
        {
            // Arrange
            var book = new Book();
            // Act
            book.ChangeTitle("Hit Refresh");
            // Assert
            Assert.Equal("Hit Refresh", book.Title);
            Assert.Equal(2, book.UncommittedEvents.Count());
            Assert.Equal(2, book.Version);
        }

        [Fact]
        public async Task PersistBookTest()
        {
            // Arrange
            var domainEventsList = new List<IDomainEvent>();
            var mockRepository = new Mock<Repository>();

            mockRepository.Protected().Setup<Task>("PersistDomainEventsAsync",
                    ItExpr.IsAny<IEnumerable<IDomainEvent>>())
                .Callback<IEnumerable<IDomainEvent>>(evnts => domainEventsList.AddRange(evnts))
                .Returns(Task.CompletedTask);

            var book = new Book();
            // Act
            book.ChangeTitle("Hit Refresh");
            await mockRepository.Object.SaveAsync(book);

            // Assert
            Assert.Equal(2, domainEventsList.Count);
            Assert.Empty(book.UncommittedEvents);
            Assert.Equal(2, book.Version);
        }

        [Fact]
        public async Task RetrieveBookTest()
        {
            // Arrange
            var fakeId = Guid.NewGuid();
            var domainEventsList = new List<IDomainEvent>
                {
                    new AggregateCreatedEvent(fakeId),
                    new BookTitleChangedEvent("Hit Refresh")
                };
            var mockRepository = new Mock<Repository>();
            mockRepository.Protected().Setup<Task<IEnumerable<IDomainEvent>>>("LoadDomainEventsAsync",
                    ItExpr.IsAny<Type>(),
                    ItExpr.IsAny<Guid>())
                .Returns(Task.FromResult(domainEventsList.AsEnumerable()));

            // Act
            var book = await mockRepository.Object.GetByIdAsync<Book>(fakeId);

            // Assert
            Assert.Equal(fakeId, book.Id);
            Assert.Equal("Hit Refresh", book.Title);
            Assert.Equal(2, book.Version);
            Assert.Empty(book.UncommittedEvents);
        }

        [Fact]
        public async Task BookVersionAfterSaveTest()
        {
            // Arrange
            var domainEventsList = new List<IDomainEvent>();
            var mockRepository = new Mock<Repository>();

            mockRepository.Protected().Setup<Task>("PersistDomainEventsAsync",
                    ItExpr.IsAny<IEnumerable<IDomainEvent>>())
                .Callback<IEnumerable<IDomainEvent>>(evnts => domainEventsList.AddRange(evnts))
                .Returns(Task.CompletedTask);

            var book = new Book();

            // Act
            book.ChangeTitle("C# Cookbook");
            await mockRepository.Object.SaveAsync(book);
            book.ChangeTitle("Hit Refresh");

            // Assert
            Assert.Equal(3, book.Version);
            Assert.Single(book.UncommittedEvents);
            Assert.Equal("Hit Refresh", book.Title);
        }

        [Fact]
        public async Task BookVersionAfterLoadTest()
        {
            // Arrange
            var fakeId = Guid.NewGuid();
            var domainEventsList = new List<IDomainEvent>
                {
                    new AggregateCreatedEvent(fakeId),
                    new BookTitleChangedEvent("Hit Refresh")
                };
            var mockRepository = new Mock<Repository>();
            mockRepository.Protected().Setup<Task<IEnumerable<IDomainEvent>>>("LoadDomainEventsAsync",
                    ItExpr.IsAny<Type>(),
                    ItExpr.IsAny<Guid>())
                .Returns(Task.FromResult(domainEventsList.AsEnumerable()));

            // Act
            var book = await mockRepository.Object.GetByIdAsync<Book>(fakeId);
            book.ChangeTitle("C# Cookbook");

            // Assert
            Assert.Equal(3, book.Version);
            Assert.Single(book.UncommittedEvents);
            Assert.Equal("C# Cookbook", book.Title);
        }

        [Fact]
        public void MultithreadingBookTest()
        {
            var tasks = new List<Task>();
            var book = new Book();

            for (var i = 0; i < 100; i++)
            {
                var bookTitle = $"Book Title {i + 1}";
                tasks.Add(Task.Factory.StartNew(() => book.ChangeTitle(bookTitle)));
            }

            Task.WaitAll(tasks.ToArray());

            Assert.Equal(101, book.UncommittedEvents.Count());
            Assert.Equal(101, book.Version);
        }
    }
}