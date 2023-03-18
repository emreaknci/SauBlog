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
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<IResult> RemoveAsync(int id)
        {
            var comment = await _commentDal.GetByIdAsync(id);
            if (comment != null)
            {
                _commentDal.Remove(comment);
                await _commentDal.SaveAsync();
                return new SuccessResult("Yorum Silindi");
            }

            return new ErrorResult("Yorum Bulunamadı");
        }

        public async Task<IResult> RemoveRangeAsync(List<Comment> comments)
        {
            _commentDal.RemoveRange(comments);
            await _commentDal.SaveAsync();
            return new SuccessResult("Yorumlar silindi");
        }

        public IDataResult<List<Comment>> GetAllByBlogId(int id)
        {
            var comments = _commentDal.GetAll(c => c.BlogId == id).OrderByDescending(c => c.Id).ToList();

            if (comments.Count > 0)
            {
                return new SuccessDataResult<List<Comment>>(comments);
            }

            return new ErrorDataResult<List<Comment>>(comments);
        }

        public IDataResult<List<Comment>> GetAll()
        {
            var comments = _commentDal.GetAll().OrderByDescending(c => c.Id).ToList();

            if (comments.Count > 0)
            {
                return new SuccessDataResult<List<Comment>>(comments);
            }

            return new ErrorDataResult<List<Comment>>(comments);
        }

        public IDataResult<List<Comment>> GetAllByWriterId(int writerId)
        {
            var comments = _commentDal.GetAll(w => w.WriterId == writerId).ToList();
            if (comments.Count > 0)
            {
                return new SuccessDataResult<List<Comment>>(comments);
            }

            return new ErrorDataResult<List<Comment>>(comments);
        }

        public IDataResult<List<CommentForListDto>> GetAllForListing(
            Expression<Func<CommentForListDto, bool>>? filter = null)
        {
            var comments = _commentDal.GetAllForListing(filter).ToList();

            if (comments.Count > 0)
            {
                return new SuccessDataResult<List<CommentForListDto>>(comments);
            }

            return new ErrorDataResult<List<CommentForListDto>>(comments);
        }

        public IDataResult<List<Comment>> GetAllWithWriterByBlogId(int id)
        {
            var comments = _commentDal.GetAll(c => c.BlogId == id).Include(c => c.Writer).OrderByDescending(c => c.Id)
                .ToList();

            if (comments.Count > 0)
            {
                return new SuccessDataResult<List<Comment>>(comments);
            }

            return new ErrorDataResult<List<Comment>>(comments);
        }

        public IPaginateResult<CommentForListDto> GetWithPaginate(int index, int size, string? filter)
        {
            var data = filter.IsNullOrEmpty()
                ? _commentDal.GetWithPagination(index, size)
                : _commentDal.GetWithPagination(index, size, filter: i => i.Content!.Contains(filter!.ToUpper()));

            if (index * size > data.totalCount)
                return new ErrorPaginationResult<CommentForListDto>("Hata!");

            return new SuccessPaginationResult<CommentForListDto>(index, size, data.entities, data.totalCount);
        }

    }
}
