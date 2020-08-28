﻿using EF6.Utils.Common;
using EF6.Utils.Demo.Data;
using Effort;
using Effort.DataLoaders;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EF6.Utils.Tests
{
    public class UtilsDbContextTests
    {
        public class EmptyContext
        {
            private readonly AppDbContext _context;

            public EmptyContext()
            {
                var clockMock = new Mock<IClock>();
                clockMock.Setup(c => c.Now()).Returns(25.December(2020).At(4, 20));

                var connection = DbConnectionFactory.CreateTransient();
                _context = new AppDbContext(connection, clockMock.Object);
                _context.Database.CreateIfNotExists();
            }

            [Fact]
            public void SaveChanges_WhenAddingOneNewEntity_ShouldSetTheSameCreatedAndUpdatedDateTimes()
            {
                // Arrange
                var comment = new Comment { Content = "Test" };
                _context.Comments.Add(comment);

                // Act
                _context.SaveChanges();

                // Assert
                var expected = 25.December(2020).At(4, 20);
                comment.CreatedOn.Should().Be(expected);
                comment.UpdatedOn.Should().Be(expected);
            }

            [Fact]
            public async Task SaveChangesAsync_WhenAddingOneNewEntity_ShouldSetTheSameCreatedAndUpdatedDateTimes()
            {
                // Arrange
                var comment = new Comment { Content = "Test" };
                _context.Comments.Add(comment);

                // Act
                await _context.SaveChangesAsync();

                // Assert
                var expected = 25.December(2020).At(4, 20);
                comment.CreatedOn.Should().Be(expected);
                comment.UpdatedOn.Should().Be(expected);
            }

            [Fact]
            public void SaveChanges_WhenAddingOneNewEntity_ShouldReturnOne()
            {
                // Arrange
                var comment = new Comment { Content = "Test" };
                _context.Comments.Add(comment);

                // Act
                var saved = _context.SaveChanges();

                // Assert
                saved.Should().Be(1);
            }

            [Fact]
            public async Task SaveChangesAsync_WhenAddingOneNewEntity_ShouldReturnOne()
            {
                // Arrange
                var comment = new Comment { Content = "Test" };
                _context.Comments.Add(comment);

                // Act
                var saved = await _context.SaveChangesAsync();

                // Assert
                saved.Should().Be(1);
            }

            [Fact]
            public void SaveChanges_WhenAddingMultipleNewEntities_ShouldSetTheSameCreatedAndUpdatedDateTimes()
            {
                // Arrange
                var comments = new List<Comment>
                {
                    new Comment { Content = "Comment 1" },
                    new Comment { Content = "Comment 2" },
                };

                _context.Comments.AddRange(comments);

                // Act
                _context.SaveChanges();

                // Assert
                comments[0].CreatedOn.Should().Be(comments[1].CreatedOn);
                comments[0].UpdatedOn.Should().Be(comments[1].UpdatedOn);
            }

            [Fact]
            public async Task SaveChangesAsync_WhenAddingMultipleNewEntities_ShouldSetTheSameCreatedAndUpdatedDateTimes()
            {
                // Arrange
                var comments = new List<Comment>
                {
                    new Comment { Content = "Comment 1" },
                    new Comment { Content = "Comment 2" },
                };

                _context.Comments.AddRange(comments);

                // Act
                await _context.SaveChangesAsync();

                // Assert
                comments[0].CreatedOn.Should().Be(comments[1].CreatedOn);
                comments[0].UpdatedOn.Should().Be(comments[1].UpdatedOn);
            }

            [Fact]
            public void SaveChanges_WhenAddingMultipleNewEntities_ShouldReturnTheCorrectNumberOfEntitiesSaved()
            {
                // Arrange
                var comments = new List<Comment>
                {
                    new Comment { Content = "Comment 1" },
                    new Comment { Content = "Comment 2" },
                    new Comment { Content = "Comment 3" },
                };

                _context.Comments.AddRange(comments);

                // Act
                var saved = _context.SaveChanges();

                // Assert
                saved.Should().Be(3);
            }

            [Fact]
            public async Task SaveChangesAsync_WhenAddingMultipleNewEntities_ShouldReturnTheCorrectNumberOfEntitiesSaved()
            {
                // Arrange
                var comments = new List<Comment>
                {
                    new Comment { Content = "Comment 1" },
                    new Comment { Content = "Comment 2" },
                    new Comment { Content = "Comment 3" },
                };

                _context.Comments.AddRange(comments);

                // Act
                var saved = await _context.SaveChangesAsync();

                // Assert
                saved.Should().Be(3);
            }

            [Fact]
            public void LatestCreatedOrDefault_WhenEmptySet_ShouldReturnNull()
            {
                // Act
                var latestCreated = _context.LatestCreatedOrDefault<Comment>();

                // Assert
                latestCreated.Should().BeNull();
            }

            [Fact]
            public async Task LatestCreatedOrDefaultAsync_WhenEmptySet_ShouldReturnNull()
            {
                // Act
                var latestCreated = await _context.LatestCreatedOrDefaultAsync<Comment>();

                // Assert
                latestCreated.Should().BeNull();
            }

            [Fact]
            public void LatestCreated_WhenEmptySet_ShouldThrowInvalidOperationException()
            {
                // Act
                Action action = () => _context.LatestCreated<Comment>();

                // Assert
                action.Should().ThrowExactly<InvalidOperationException>();
            }

            [Fact]
            public async Task LatestCreatedAsync_WhenEmptySet_ShouldThrowInvalidOperationException()
            {
                // Act
                Func<Task> action = async () => await _context.LatestCreatedAsync<Comment>();

                // Assert
                await action.Should().ThrowExactlyAsync<InvalidOperationException>();
            }
        }

        public class FullContext
        {
            private readonly AppDbContext _context;

            public FullContext()
            {
                var loader = new CsvDataLoader(@".\CsvFiles");
                var connection = DbConnectionFactory.CreateTransient(loader);
                _context = new AppDbContext(connection);
                _context.Database.CreateIfNotExists();
            }

            [Fact]
            public void LatestCreatedOrDefault_WhenNonEmptySet_ShouldReturnTheMostRecentlyCreatedRecord()
            {
                // Act
                var latestCreated = _context.LatestCreatedOrDefault<Comment>();

                // Assert
                latestCreated.Should().NotBeNull();
                latestCreated.Id.Should().Be(2);
            }

            [Fact]
            public async Task LatestCreatedOrDefaultAsync_WhenNonEmptySet_ShouldReturnTheMostRecentlyCreatedRecord()
            {
                // Act
                var latestCreated = await _context.LatestCreatedOrDefaultAsync<Comment>();

                // Assert
                latestCreated.Should().NotBeNull();
                latestCreated.Id.Should().Be(2);
            }

            [Fact]
            public void LatestCreated_WhenNonEmptySet_ShouldReturnTheMostRecentlyCreatedRecord()
            {
                // Act
                var latestCreated = _context.LatestCreated<Comment>();

                // Assert
                latestCreated.Should().NotBeNull();
                latestCreated.Id.Should().Be(2);
            }

            [Fact]
            public async Task LatestCreatedAsync_WhenNonEmptySet_ShouldReturnTheMostRecentlyCreatedRecord()
            {
                // Act
                var latestCreated = await _context.LatestCreatedAsync<Comment>();

                // Assert
                latestCreated.Should().NotBeNull();
                latestCreated.Id.Should().Be(2);
            }
        }
    }
}