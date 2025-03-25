using Notebook.Features;
using Notebook.Models.Requests;
using Tests.Helpers;
using Notebook.Models.Domain;
using Notebook.Models;

namespace Tests.Features.BookFeature
{
    public class UpdateBookFeatureTests
    {
        [Fact]
        public async Task Execute_ShouldUpdateBook_WhenBookExistsAndBelongsToUser()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("UpdateBookFeatureTests_ValidBook");
            var user = TestDataHelper.CreateUser();
            var book = TestDataHelper.CreateBook(user);
            book.Name = "OldName";
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var feature = new UpdateBookFeature(context);
            var request = new UpdateBookRequest { Name = "NewName" };

            // Act
            var result = await feature.Execute(book.Id, request, user);

            // Assert
            Assert.True(result.Response);
            var updatedBook = await context.Books.FindAsync(book.Id);
            Assert.NotNull(updatedBook);
            Assert.Equal("NewName", updatedBook.Name);
        }

        [Fact]
        public async Task Execute_ShouldReturnNotFoundError_WhenBookDoesNotExist()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("UpdateBookFeatureTests_NonExistentBook");
            var user = TestDataHelper.CreateUser();
            var feature = new UpdateBookFeature(context);
            var request = new UpdateBookRequest { Name = "NewName" };

            // Act
            var result = await feature.Execute("NonExistentBookId", request, user);

            // Assert
            Assert.False(result.Response);
            Assert.Equal(ErrorType.NotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnNoNameError_WhenNameIsEmpty()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("UpdateBookFeatureTests_EmptyName");
            var user = TestDataHelper.CreateUser();
            var book = TestDataHelper.CreateBook(user);
            book.Name = "OldName";
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var feature = new UpdateBookFeature(context);
            var request = new UpdateBookRequest { Name = "" };

            // Act
            var result = await feature.Execute(book.Id, request, user);

            // Assert
            Assert.False(result.Response);
            Assert.Equal(ErrorType.NameEmpty, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnDuplicateNameError_WhenNameIsDuplicate()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("UpdateBookFeatureTests_DuplicateName");
            var user = TestDataHelper.CreateUser();
            var existingBook = new Book { Id = "book1", Name = "DuplicateName", UserId = user.Id, Pages = new HashSet<Page>(), User = user };
            var book = TestDataHelper.CreateBook(user);
            book.Name = "OldName";
            context.Books.Add(existingBook);
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var feature = new UpdateBookFeature(context);
            var request = new UpdateBookRequest { Name = "DuplicateName" };

            // Act
            var result = await feature.Execute(book.Id, request, user);

            // Assert
            Assert.False(result.Response);
            Assert.Equal(ErrorType.DuplicateName, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnNotFoundError_WhenBookBelongsToAnotherUser()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("UpdateBookFeatureTests_OtherUserBook");
            var user = TestDataHelper.CreateUser();
            var otherUser = TestDataHelper.CreateUser();
            var book = TestDataHelper.CreateBook(otherUser);
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var feature = new UpdateBookFeature(context);
            var request = new UpdateBookRequest { Name = "NewName" };

            // Act
            var result = await feature.Execute(book.Id, request, user);

            // Assert
            Assert.False(result.Response);
            Assert.Equal(ErrorType.NotFound, result.Error);
        }
    }
}
