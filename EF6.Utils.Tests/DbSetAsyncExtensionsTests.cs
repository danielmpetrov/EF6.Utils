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
    public class DbSetAsyncExtensionsTests
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
            public async Task LatestCreatedOrDefault_WhenEmptySet_ShouldReturnNull()
            {
                // Act
                var latestCreated = await _comments.LatestCreatedOrDefaultAsync();

                // Assert
                latestCreated.Should().BeNull();
            }

            [Fact]
            public async Task LatestCreated_WhenEmptySet_ShouldThrowInvalidOperationException()
            {
                // Act
                Func<Task> action = async () => await _comments.LatestCreatedAsync();

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
            public async Task LatestCreatedOrDefault_WhenNonEmptySet_ShouldReturnTheMostRecentlyCreatedRecord()
            {
                // Act
                var latestCreated = await _comments.LatestCreatedOrDefaultAsync();

                // Assert
                latestCreated.Should().NotBeNull();
                latestCreated.Id.Should().Be(2);
            }

            [Fact]
            public async Task LatestCreated_WhenNonEmptySet_ShouldReturnTheMostRecentlyCreatedRecord()
            {
                // Act
                var latestCreated = await _comments.LatestCreatedAsync();

                // Assert
                latestCreated.Should().NotBeNull();
                latestCreated.Id.Should().Be(2);
            }
        }
    }
}
