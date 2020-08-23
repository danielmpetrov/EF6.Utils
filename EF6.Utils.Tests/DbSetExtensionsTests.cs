using EF6.Utils.Demo.Data;
using Effort;
using Effort.DataLoaders;
using FluentAssertions;
using System;
using System.Data.Entity;
using Xunit;

namespace EF6.Utils.Tests
{
    public class DbSetExtensionsTests
    {
        public class EmptyContext
        {
            private readonly DbSet<Comment> _comments;

            public EmptyContext()
            {
                var connection = DbConnectionFactory.CreateTransient();
                var context = new AppDbContext(connection);
                _comments = context.Comments;
            }

            [Fact]
            public void LatestCreatedOrDefault_WhenEmptySet_ShouldReturnNull()
            {
                // Act
                var latestCreated = _comments.LatestCreatedOrDefault();

                // Assert
                latestCreated.Should().BeNull();
            }

            [Fact]
            public void LatestCreated_WhenEmptySet_ShouldThrowInvalidOperationException()
            {
                // Act
                Action action = () => _comments.LatestCreated();

                // Assert
                action.Should().ThrowExactly<InvalidOperationException>();
            }
        }

        public class FullContext
        {
            private readonly DbSet<Comment> _comments;

            public FullContext()
            {
                var loader = new CsvDataLoader(@".\CsvFiles");
                var connection = DbConnectionFactory.CreateTransient(loader);
                var context = new AppDbContext(connection);
                _comments = context.Comments;
            }

            [Fact]
            public void LatestCreatedOrDefault_WhenNonEmptySet_ShouldReturnTheMostRecentlyCreatedRecord()
            {
                // Act
                var latestCreated = _comments.LatestCreatedOrDefault();

                // Assert
                latestCreated.Should().NotBeNull();
                latestCreated.Id.Should().Be(2);
            }

            [Fact]
            public void LatestCreated_WhenNonEmptySet_ShouldReturnTheMostRecentlyCreatedRecord()
            {
                // Act
                var latestCreated = _comments.LatestCreated();

                // Assert
                latestCreated.Should().NotBeNull();
                latestCreated.Id.Should().Be(2);
            }
        }
    }
}
