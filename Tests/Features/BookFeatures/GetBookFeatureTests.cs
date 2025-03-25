using Notebook.Models;
using Notebook.Features;
using Notebook.Models.Responses;
using Tests.Helpers;
using Notebook.Models.Domain;

namespace Tests.Features.BookFeature
{
    public class GetBookFeatureTests
    {
        [Fact]
        public async Task Execute_ShouldReturnBook_WhenBookExistsAndBelongsToUser()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("GetBookFeatureTests_ValidBook");
            var user = TestDataHelper.CreateUser();
            var book = TestDataHelper.CreateBook(user);
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var feature = new GetBookFeature(context);

            // Act
            var result = await feature.Execute(book.Id, user);

            // Assert
            Assert.NotNull(result.Response);
            Assert.Equal(book.Id, result.Response.Id);
            Assert.Equal(book.Name, result.Response.Name);
        }

        [Fact]
        public async Task Execute_ShouldReturnNotFoundError_WhenBookDoesNotExist()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("GetBookFeatureTests_NonExistentBook");
            var user = TestDataHelper.CreateUser();
            var feature = new GetBookFeature(context);

            // Act
            var result = await feature.Execute("NonExistentBookId", user);

            // Assert
            Assert.Null(result.Response);
            Assert.Equal(ErrorType.NotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnNotFoundError_WhenBookBelongsToAnotherUser()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("GetBookFeatureTests_OtherUserBook");
            var user = TestDataHelper.CreateUser();
            var otherUser = TestDataHelper.CreateUser();
            var book = TestDataHelper.CreateBook(otherUser);
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var feature = new GetBookFeature(context);

            // Act
            var result = await feature.Execute(book.Id, user);

            // Assert
            Assert.Null(result.Response);
            Assert.Equal(ErrorType.NotFound, result.Error);
        }
    }
}
