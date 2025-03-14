using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Requests;
using Notebook.Models.Responses;

namespace Notebook.Features

{
    public class UpdateNotebookFeature
    {
        private readonly ApplicationDbContext _notebookRepository;
        public UpdateNotebookFeature(ApplicationDbContext notebookRepository)
        {
            _notebookRepository = notebookRepository;
        }

        public async Task<Boolean> Execute(string id, UpdateNotebookRequest request, User user)
        {
            var notebook = await _notebookRepository.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);
            if (notebook == null)
            {
                return false;
            }

            notebook.Name = request.Name;

            _notebookRepository.Books.Update(notebook);
            await _notebookRepository.SaveChangesAsync();

            return true;
        }
    }
}