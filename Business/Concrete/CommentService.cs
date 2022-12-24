using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Concrete;
using Entities.DTOs.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CommentService : ICommentService
    {
        private readonly ICommentDal _commentDal;

        public CommentService(ICommentDal commentDal)
        {
            _commentDal = commentDal;
        }

        public async Task<IDataResult<Comment>> AddAsync(CommentForCreateDto dto)
        {
            var newComment = new Comment()
            {
                BlogId = dto.BlogId,
                WriterId = dto.WriterId,
                Content = dto.Content 
            };
            await _commentDal.AddAsync(newComment);
            await _commentDal.SaveAsync();
            return new SuccessDataResult<Comment>(newComment, "Yorum Kaydedildi");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var comment = await _commentDal.GetByIdAsync(id);
            if (comment != null) {
                _commentDal.Remove(comment);
                await _commentDal.SaveAsync();
                return new SuccessResult("Yorum Silindi");
            }
            return new ErrorResult("Yorum Bulunamadı");
        }

        public IDataResult<List<Comment>> GetAllByBlogId(int id)
        {
            var comments = _commentDal.GetAll(c => c.BlogId == id).ToList();

            if (comments.Count > 0)
            {
                return new SuccessDataResult<List<Comment>>(comments);
            }
            return new ErrorDataResult<List<Comment>>(comments);
        }
    }
}
