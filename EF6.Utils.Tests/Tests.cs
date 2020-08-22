using EF6.Utils.Demo.Data;
using Effort;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace EF6.Utils.Tests
{
    [TestFixture]
    public class Tests
    {
        private AppDbContext _context;

        [SetUp]
        public void Initialize()
        {
            var connection = DbConnectionFactory.CreateTransient();
            _context = new AppDbContext(connection);
        }

        [Test]
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

        [Test]
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
    }
}
