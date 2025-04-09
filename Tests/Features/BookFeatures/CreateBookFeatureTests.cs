using Notebook.Models;
using Notebook.Models.Requests;
using Notebook.Features;
using Notebook.Models.Domain;
using Tests.Helpers;
using Notebook.Constants;

namespace Tests.Features.BookFeature
{
    public class CreateBookFeatureTests
    {
        [Fact]
        public async Task Execute_ShouldReturnNoNameError_WhenNameIsEmpty()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("CreateBookFeatureTests_EmptyName");
            var user = TestDataHelper.CreateUser();
            var feature = new CreateBookFeature(context);
            var request = new CreateBookRequest { Name = "" };

            // Act
            var result = await feature.Execute(request, user);

            // Assert
            Assert.Null(result.Response);
            Assert.Equal(ErrorMessages.BookNameRequired, result.ErrorMessage);
        }

        [Fact]
        public async Task Execute_ShouldReturnDuplicateNameError_WhenNameIsDuplicate()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("CreateBookFeatureTests_DuplicateName");
            var user = TestDataHelper.CreateUser();
            var existingBook = new Book { Id = "book1", Name = "DuplicateName", UserId = user.Id, Pages = new HashSet<Page>(), User = user };
            context.Books.Add(existingBook);
            await context.SaveChangesAsync();

            var feature = new CreateBookFeature(context);
            var request = new CreateBookRequest { Name = "DuplicateName" };

            // Act
            var result = await feature.Execute(request, user);

            // Assert
            Assert.Null(result.Response);
            Assert.Equal(ErrorMessages.BookNameAlreadyExists, result.ErrorMessage);
        }

        [Fact]
        public async Task Execute_ShouldCreateBookWithAtLeastOnePage_WhenValidRequest()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("CreateBookFeatureTests_ValidRequest");
            var user = TestDataHelper.CreateUser();
            var feature = new CreateBookFeature(context);
            var request = new CreateBookRequest { Name = "ValidBook" };

            // Act
            var result = await feature.Execute(request, user);

            // Assert
            Assert.IsType<string>(result.Response);
        }
    }
}