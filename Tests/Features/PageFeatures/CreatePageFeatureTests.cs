using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Requests;
using Notebook.Features;
using Notebook.Models.Domain;
using Tests.Helpers;

namespace Tests.Features.PageFeature
{
    public class CreatePageFeatureTests
    {
        [Fact]
        public async Task Execute_ShouldCreatePage_WhenBookExists()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("CreatePageFeatureTests");
            var user = TestDataHelper.CreateUser();
            var book = new Book {
                Name = "book1",
                Id = "book1",
                UserId = user.Id,
                Pages = new HashSet<Page>(),
                User = user
            };
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var feature = new CreatePageFeature(context);
            var request = new CreatePageRequest
            {
                BookId = book.Id,
                Content = "Test content"
            };

            // Act
            var result = await feature.Execute(request, user);

            // Assert
            Assert.NotNull(result.Response);
            Assert.Equal("Test content", result.Response.Content);
            Assert.Equal(0, result.Response.Index);
        }

        [Fact]
        public async Task Execute_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var context = TestDbContextHelper.GetDbContext("CreatePageFeatureTests_NotFound");
            var user = TestDataHelper.CreateUser();
            var feature = new CreatePageFeature(context);
            var request = new CreatePageRequest
            {
                BookId = "nonexistent",
                Content = "Test content"
            };

            // Act
            var result = await feature.Execute(request, user);

            // Assert
            Assert.Null(result.Response);
            Assert.Equal(ErrorType.NotFound, result.Error);
        }
    }
}
