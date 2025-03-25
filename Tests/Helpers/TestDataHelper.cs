using Notebook.Models;

namespace Tests.Helpers
{
    public static class TestDataHelper
    {
        public static User CreateUser()
        {
            return new User
            {
                UserName = "User 1",
                Email = "test@test.test",
                Id = Guid.NewGuid().ToString(),
                Books = new HashSet<Book>()
            };
        }

        public static Book CreateBook(User user)
        {
            return new Book
            {
                Name = "Book 1",
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Pages = new HashSet<Page>(),
                User = user
            };
        }

        public static Page CreatePage(Book book)
        {
            return new Page
            {
                Id = Guid.NewGuid().ToString(),
                BookId = book.Id,
                Book = book,
                Content = "Page content",
                Index = 0
            };
        }
    }
}
