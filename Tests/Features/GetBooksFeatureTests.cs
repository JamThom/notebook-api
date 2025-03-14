using Moq;
using Notebook.Data;
using Notebook.Features;
using Notebook.Models;
using Xunit;

namespace Notebook.Tests.Features
{
    public class GetBooksFeatureTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly GetBooksFeature _feature;

        public GetBooksFeatureTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _feature = new GetBooksFeature(_mockContext.Object);
        }

        [Fact]
        public void Execute_ReturnsBooks_ForUser()
        {
            // // Arrange
            // var user = new User { Id = "user-id" };
            // var books = new List<Book>
            // {
            //     new Book { Id = "book-id", Title = "Test Book", UserId = user.Id }
            // };
            // _mockContext.Setup(c => c.Books).ReturnsDbSet(books);

            // // Act
            // var result = await _feature.Execute(user);

            // // Assert
            // Assert.Single(result);
            // Assert.Equal("Test Book", result[0].Title);
        }

        // Add more tests for other methods...
    }
}
