using Notebook.Data;
using Notebook.Features;
using Notebook.Models;
using Notebook.Models.Domain;
using Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Features.PageFeatures
{
    public class DeletePageFeatureTests
    {
        [Fact]
        public async Task Execute_ShouldReturnNotFoundError_WhenPageDoesNotExist()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("DeletePageFeatureTests_PageNotFound");
            var user = TestDataHelper.CreateUser();
            var feature = new DeletePageFeature(context);

            // Act
            var result = await feature.Execute("nonexistentPageId", user);

            // Assert
            Assert.False(result.Response);
            Assert.Equal(ErrorType.NotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldDeletePage_WhenPageExistsAndBelongsToUser()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("DeletePageFeatureTests_ValidPage");
            var user = TestDataHelper.CreateUser();
            var book = TestDataHelper.CreateBook(user);
            var page = TestDataHelper.CreatePage(book);
            page.Id = "page1";
            context.Pages.Add(page);
            await context.SaveChangesAsync();

            var feature = new DeletePageFeature(context);

            // Act
            var result = await feature.Execute("page1", user);

            // Assert
            Assert.True(result.Response);
            Assert.Null(result.Error);
            Assert.Null(await context.Pages.FindAsync("page1"));
        }

        [Fact]
        public async Task Execute_ShouldReturnNotFoundError_WhenPageBelongsToAnotherUser()
        {
            // Arrange
            using var context = TestDbContextHelper.GetDbContext("DeletePageFeatureTests_PageBelongsToAnotherUser");
            var user = TestDataHelper.CreateUser();
            var anotherUser = TestDataHelper.CreateUser();
            var book = TestDataHelper.CreateBook(anotherUser);
            var page = TestDataHelper.CreatePage(book);
            context.Pages.Add(page);
            await context.SaveChangesAsync();

            var feature = new DeletePageFeature(context);

            // Act
            var result = await feature.Execute("page1", user);

            // Assert
            Assert.False(result.Response);
            Assert.Equal(ErrorType.NotFound, result.Error);
        }
    }
}
