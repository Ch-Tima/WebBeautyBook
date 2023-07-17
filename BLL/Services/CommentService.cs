using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class CommentService
    {

        private readonly CommentRepository _commentRepository;

        public CommentService(CommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<Comment> GetAsync(string id)
        {
            return await _commentRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _commentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllFindAsync(Expression<Func<Comment, bool>> func)
        {
            return await _commentRepository.GetAllFindAsync(func);
        }

        public async Task InsertAsync(Comment comment)
        {
            await _commentRepository.InsertAsync(comment);
        }

        public async Task DeleteAsync(string id)
        {
            await _commentRepository.DeleteAsync(id);
        }

        public async Task UpdataAsync(Comment newComment)
        {
            if (newComment == null) return;

            await _commentRepository.UpdateAsync(newComment.Id, newComment);
        }


    }
}
