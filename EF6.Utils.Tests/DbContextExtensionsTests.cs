using EF6.Utils.Demo.Data;
using Effort;
using Effort.DataLoaders;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace EF6.Utils.Tests
{
    public class DbContextExtensionsTests
    {
        public class EmptyContext
        {
            private readonly AppDbContext _context;

            public EmptyContext()
            {
                var connection = DbConnectionFactory.CreateTransient();
                _context = new AppDbContext(connection);
                _context.Database.CreateIfNotExists();
            }

            [Fact]
            public void SaveChangesTimestamped_WhenAddingOneNewEntity_ShouldSetTheSameCreatedAndUpdatedDateTimes()
            {
                // Arrange
                var comment = new Comment { Content = "Test" };
                _context.Comments.Add(comment);

                // Act
                _context.SaveChangesTimestamped();

                // Assert
                comment.CreatedOn.Should().NotBe(default);
                comment.UpdatedOn.Should().NotBe(default);
                comment.CreatedOn.Should().Be(comment.UpdatedOn);
            }

            [Fact]
            public void SaveChangesTimestamped_WhenAddingOneNewEntity_ShouldReturnOne()
            {
                // Arrange
                var comment = new Comment { Content = "Test" };
                _context.Comments.Add(comment);

                // Act
                var saved = _context.SaveChangesTimestamped();

                // Assert
                saved.Should().Be(1);
            }

            [Fact]
            public void SaveChangesTimestamped_WhenAddingMultipleNewEntities_ShouldSetTheSameCreatedAndUpdatedDateTimes()
            {
                // Arrange
                var comments = new List<Comment>
                {
                    new Comment { Content = "Comment 1" },
                    new Comment { Content = "Comment 2" },
                };

                _context.Comments.AddRange(comments);

                // Act
                _context.SaveChangesTimestamped();

                // Assert
                comments[0].CreatedOn.Should().Be(comments[1].CreatedOn);
                comments[0].UpdatedOn.Should().Be(comments[1].UpdatedOn);
            }

            [Fact]
            public void SaveChangesTimestamped_WhenAddingMultipleNewEntities_ShouldReturnTheCorrectNumberOfEntitiesSaved()
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
                var saved = _context.SaveChangesTimestamped();

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
            public void LatestCreated_WhenEmptySet_ShouldThrowInvalidOperationException()
            {
                // Act
                Action action = () => _context.LatestCreated<Comment>();

                // Assert
                action.Should().ThrowExactly<InvalidOperationException>();
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
            public void LatestCreated_WhenNonEmptySet_ShouldReturnTheMostRecentlyCreatedRecord()
            {
                // Act
                var latestCreated = _context.LatestCreated<Comment>();

                // Assert
                latestCreated.Should().NotBeNull();
                latestCreated.Id.Should().Be(2);
            }
        }
    }
}
