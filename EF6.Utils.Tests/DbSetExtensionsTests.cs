using EF6.Utils.Demo.Data;
using Effort;
using Effort.DataLoaders;
using FluentAssertions;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
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
                context.Database.CreateIfNotExists();
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
            public async Task LatestCreatedOrDefaultAsync_WhenEmptySet_ShouldReturnNull()
            {
                // Act
                var latestCreated = await _comments.LatestCreatedOrDefaultAsync();

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

            [Fact]
            public async Task LatestCreatedAsync_WhenEmptySet_ShouldThrowInvalidOperationException()
            {
                // Act
                Func<Task> action = async () => await _comments.LatestCreatedAsync();

                // Assert
                await action.Should().ThrowExactlyAsync<InvalidOperationException>();
            }

            [Fact]
            public void LatestUpdatedOrDefault_WhenEmptySet_ShouldReturnNull()
            {
                // Act
                var latestUpdated = _comments.LatestUpdatedOrDefault();

                // Assert
                latestUpdated.Should().BeNull();
            }

            [Fact]
            public async Task LatestUpdatedOrDefaultAsync_WhenEmptySet_ShouldReturnNull()
            {
                // Act
                var latestUpdated = await _comments.LatestUpdatedOrDefaultAsync();

                // Assert
                latestUpdated.Should().BeNull();
            }

            [Fact]
            public void LatestUpdated_WhenEmptySet_ShouldThrowInvalidOperationException()
            {
                // Act
                Action action = () => _comments.LatestUpdated();

                // Assert
                action.Should().ThrowExactly<InvalidOperationException>();
            }

            [Fact]
            public async Task LatestUpdatedAsync_WhenEmptySet_ShouldThrowInvalidOperationException()
            {
                // Act
                Func<Task> action = async () => await _comments.LatestUpdatedAsync();

                // Assert
                await action.Should().ThrowExactlyAsync<InvalidOperationException>();
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
                context.Database.CreateIfNotExists();
                _comments = context.Comments;
            }

            [Fact]
            public void LatestCreatedOrDefault_WhenNonEmptySet_ShouldReturnTheMostRecentlyCreatedRecord()
            {
                // Act
                var latestCreated = _comments.LatestCreatedOrDefault();

                // Assert
                latestCreated.Should().NotBeNull();
                latestCreated.Id.Should().Be(3);
            }

            [Fact]
            public async Task LatestCreatedOrDefaultAsync_WhenNonEmptySet_ShouldReturnTheMostRecentlyCreatedRecord()
            {
                // Act
                var latestCreated = await _comments.LatestCreatedOrDefaultAsync();

                // Assert
                latestCreated.Should().NotBeNull();
                latestCreated.Id.Should().Be(3);
            }

            [Fact]
            public void LatestCreated_WhenNonEmptySet_ShouldReturnTheMostRecentlyCreatedRecord()
            {
                // Act
                var latestCreated = _comments.LatestCreated();

                // Assert
                latestCreated.Should().NotBeNull();
                latestCreated.Id.Should().Be(3);
            }

            [Fact]
            public async Task LatestCreatedAsync_WhenNonEmptySet_ShouldReturnTheMostRecentlyCreatedRecord()
            {
                // Act
                var latestCreated = await _comments.LatestCreatedAsync();

                // Assert
                latestCreated.Should().NotBeNull();
                latestCreated.Id.Should().Be(3);
            }

            [Fact]
            public void LatestUpdatedOrDefault_WhenNonEmptySet_ShouldReturnTheMostRecentlyUpdatedRecord()
            {
                // Act
                var latestUpdated = _comments.LatestUpdatedOrDefault();

                // Assert
                latestUpdated.Should().NotBeNull();
                latestUpdated.Id.Should().Be(1);
            }

            [Fact]
            public async Task LatestUpdatedOrDefaultAsync_WhenNonEmptySet_ShouldReturnTheMostRecentlyUpdatedRecord()
            {
                // Act
                var latestUpdated = await _comments.LatestUpdatedOrDefaultAsync();

                // Assert
                latestUpdated.Should().NotBeNull();
                latestUpdated.Id.Should().Be(1);
            }

            [Fact]
            public void LatestUpdated_WhenNonEmptySet_ShouldReturnTheMostRecentlyUpdatedRecord()
            {
                // Act
                var latestUpdated = _comments.LatestUpdated();

                // Assert
                latestUpdated.Should().NotBeNull();
                latestUpdated.Id.Should().Be(1);
            }

            [Fact]
            public async Task LatestUpdatedAsync_WhenNonEmptySet_ShouldReturnTheMostRecentlyUpdatedRecord()
            {
                // Act
                var latestUpdated = await _comments.LatestUpdatedAsync();

                // Assert
                latestUpdated.Should().NotBeNull();
                latestUpdated.Id.Should().Be(1);
            }
        }
    }
}
