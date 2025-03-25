using Notebook.Models;
using Notebook.Features;
using Tests.Helpers;
using Notebook.Models.Domain;

namespace Tests.Features.BookFeature
{
    public class DeleteBookFeatureTests
    {
        [Fact]
        public async Task Execute_ShouldDeleteBook_WhenBookExistsAndBelongsToUser()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("DeleteBookFeatureTests_ValidBook");
            var user = TestDataHelper.CreateUser();
            var book = TestDataHelper.CreateBook(user);
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var feature = new DeleteBookFeature(context);

            // Act
            var result = await feature.Execute(book.Id, user);

            // Assert
            Assert.True(result.Response);
            Assert.Null(await context.Books.FindAsync(book.Id));
        }

        [Fact]
        public async Task Execute_ShouldReturnNotFoundError_WhenBookDoesNotExist()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("DeleteBookFeatureTests_NonExistentBook");
            var user = TestDataHelper.CreateUser();
            var feature = new DeleteBookFeature(context);

            // Act
            var result = await feature.Execute("NonExistentBookId", user);

            // Assert
            Assert.False(result.Response);
            Assert.Equal(ErrorType.NotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnNotFoundError_WhenBookBelongsToAnotherUser()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("DeleteBookFeatureTests_OtherUserBook");
            var user = TestDataHelper.CreateUser();
            var otherUser = TestDataHelper.CreateUser();
            var book = TestDataHelper.CreateBook(otherUser);
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var feature = new DeleteBookFeature(context);

            // Act
            var result = await feature.Execute(book.Id, user);

            // Assert
            Assert.False(result.Response);
            Assert.Equal(ErrorType.NotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnNotFoundError_WhenBookIdIsNull()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("DeleteBookFeatureTests_NullBookId");
            var user = TestDataHelper.CreateUser();
            var feature = new DeleteBookFeature(context);

            // Act
            var result = await feature.Execute("null", user);

            // Assert
            Assert.False(result.Response);
            Assert.Equal(ErrorType.NotFound, result.Error);
        }
    }
}
