using Microsoft.AspNetCore.Mvc;
using Moq;
using Notebook.Controllers;
using Notebook.Features;
using Notebook.Models;
using Notebook.Models.Requests;
using Notebook.Models.Responses;
using Notebook.Models.Domain;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace Notebook.Tests.Controllers
{
    public class PageControllerTests
    {
        private readonly Mock<GetPageFeature> _mockGetPageFeature = new();
        private readonly Mock<CreatePageFeature> _mockCreatePageFeature = new();
        private readonly Mock<DeletePageFeature> _mockDeletePageFeature = new();
        private readonly Mock<UserManager<User>> _mockUserManager = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>());

        private PageController CreateController()
        {
            return new PageController(
                _mockGetPageFeature.Object,
                _mockCreatePageFeature.Object,
                _mockUserManager.Object,
                _mockDeletePageFeature.Object
            );
        }

        [Fact]
        public async Task GetPage_ReturnsNotFound_WhenPageDoesNotExist()
        {
            // Arrange
            var controller = CreateController();
            _mockGetPageFeature
                .Setup(f => f.Execute(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(new FeatureResult<PageResponse> { Error = ErrorType.NotFound });

            // Act
            var result = await controller.GetPage("nonexistent-id");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenRequestIsInvalid()
        {
            // Arrange
            var controller = CreateController();
            _mockCreatePageFeature
                .Setup(f => f.Execute(It.IsAny<CreatePageRequest>(), It.IsAny<User>()))
                .ReturnsAsync(new FeatureResult<PageResponse> { Response = null });

            // Act
            var result = await controller.Post(new CreatePageRequest { BookId = "valid-id", Content = "valid-content" });

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task DeletePage_ReturnsOk_WhenPageIsDeleted()
        {
            // Arrange
            var controller = CreateController();
            _mockDeletePageFeature
                .Setup(f => f.Execute(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(new FeatureResult<bool> { Error = null });

            // Act
            var result = await controller.DeletePage("valid-id");

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
