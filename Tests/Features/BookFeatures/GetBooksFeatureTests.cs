using Notebook.Features;
using Tests.Helpers;

namespace Tests.Features.BookFeature
{
    public class GetBooksFeatureTests
    {
        [Fact]
        public async Task Execute_ShouldReturnBooks_WhenBooksExistForUser()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("GetBooksFeatureTests_ValidBooks");
            var user = TestDataHelper.CreateUser();
            var book1 = TestDataHelper.CreateBook(user);
            var book2 = TestDataHelper.CreateBook(user);
            book1.Name = "Book1";
            book2.Name = "Book2";
            context.Books.AddRange(book1, book2);
            await context.SaveChangesAsync();

            var feature = new GetBooksFeature(context);

            // Act
            var result = await feature.Execute(user);

            // Assert
            Assert.NotNull(result.Response);
            Assert.Equal(2, result.Response.Count);
            Assert.Contains(result.Response, b => b.Name == "Book1");
            Assert.Contains(result.Response, b => b.Name == "Book2");
        }

        [Fact]
        public async Task Execute_ShouldReturnEmptyList_WhenNoBooksExistForUser()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("GetBooksFeatureTests_NoBooks");
            var user = TestDataHelper.CreateUser();
            var feature = new GetBooksFeature(context);

            // Act
            var result = await feature.Execute(user);

            // Assert
            Assert.NotNull(result.Response);
            Assert.Empty(result.Response);
        }

        [Fact]
        public async Task Execute_ShouldNotReturnBooks_BelongingToOtherUsers()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("GetBooksFeatureTests_OtherUserBooks");
            var user = TestDataHelper.CreateUser();
            var otherUser = TestDataHelper.CreateUser();
            var userBook = TestDataHelper.CreateBook(user);
            userBook.Name = "UserBook";
            var otherUserBook = TestDataHelper.CreateBook(otherUser);
            context.Books.AddRange(userBook, otherUserBook);
            await context.SaveChangesAsync();

            var feature = new GetBooksFeature(context);

            // Act
            var result = await feature.Execute(user);

            // Assert
            Assert.NotNull(result.Response);
            Assert.Single(result.Response);
            Assert.Equal("UserBook", result.Response.First().Name);
        }
    }
}
