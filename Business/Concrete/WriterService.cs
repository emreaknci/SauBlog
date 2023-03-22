using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Users;
using Entities.DTOs.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Business.Concrete
{
    public class WriterService : IWriterService
    {
        private readonly IWriterDal _writerDal;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;
        private readonly IBlogService _blogService;

        public WriterService(IWriterDal writerDal, IUserService userService, ICommentService commentService, IBlogService blogService)
        {
            _writerDal = writerDal;
            _userService = userService;
            _commentService = commentService;
            _blogService = blogService;
        }

        public async Task<IDataResult<Writer>> AddAsync(WriterForCreateDto dto)
        {
            Writer writer = new()
            {
                UserId = dto.UserId,
                NickName = dto.NickName
            };
            await _writerDal.AddAsync(writer);
            await _writerDal.SaveAsync();
            return new SuccessDataResult<Writer>(writer);
        }

        public async Task<IResult> DeleteByUserIdAsync(int userId)
        {
            var userResult = await _userService.GetById(userId);

            if (userResult != null)
            {
                var writer = _writerDal.Table.Include(w => w.Blogs)
                    .Include(w => w.Comments)
                    .FirstOrDefaultAsync(w => w.UserId == userId).Result;
                if (writer == null)
                    return new ErrorResult("Yazar bulunumadı");

                if (!writer.Comments.IsNullOrEmpty())
                    await _commentService.RemoveRangeAsync(writer.Comments!.ToList());

                if (!writer.Blogs.IsNullOrEmpty())
                    await _blogService.RemoveRangeAsync(writer.Blogs!.ToList());

                _writerDal.Remove(writer);
                await _writerDal.SaveAsync();
                return new SuccessResult("Yazar silindi");

            }
            return new ErrorResult(userResult.Message);
        }

        public async Task<IResult> ChangeNickNameAsync(int userId, string newNickName)
        {
            var writer = GetByUserId(userId).Result.Data;

            if (writer != null)
            {
                writer.NickName = newNickName;
                _writerDal.Update(writer);
                await _writerDal.SaveAsync();
                return new SuccessResult("NickName Güncellendi");

            }
            return new ErrorResult("Yazar Bulunamadı");
        }

        public IDataResult<List<Writer>> GetAll()
        {
            var list = _writerDal.GetAll().ToList();
            return new SuccessDataResult<List<Writer>>(list);
        }

        public IDataResult<List<Writer>> GetAllWithUserInfo()
        {
            var list = _writerDal.GetAll().Include(w => w.User).ToList();
            if (list != null)
            {
                return new SuccessDataResult<List<Writer>>(list);
            }
            return new ErrorDataResult<List<Writer>>("Yazar Bulunamadı");
        }

        public async Task<IDataResult<Writer>> GetById(int id)
        {
            var writer = await _writerDal.GetByIdAsync(id);

            if (writer != null)
            {
                return new SuccessDataResult<Writer>(writer);
            }
            return new ErrorDataResult<Writer>("Yazar Bulunamadı");
        }

        public async Task<IDataResult<Writer>> GetByNickName(string nickName)
        {
            var writer = await _writerDal.GetAsync(w => w.NickName == nickName);

            if (writer != null)
            {
                return new SuccessDataResult<Writer>(writer);
            }
            return new ErrorDataResult<Writer>("Yazar Bulunamadı");
        }

        public async Task<IDataResult<Writer>> GetByIdWithAllInfo(int id)
        {
            var writer = await _writerDal.Table.Include(w => w.User)
                .Include(w => w.Blogs)
                .Include(w => w.Comments)
                .FirstOrDefaultAsync(w => w.Id == id);
            if (writer != null)
            {
                return new SuccessDataResult<Writer>(writer);
            }
            return new ErrorDataResult<Writer>("Yazar Bulunamadı");
        }

        public async Task<IDataResult<Writer>> GetByIdWithUserInfoAsync(int id)
        {
            var writer = await _writerDal.Table.Include(w => w.User).FirstOrDefaultAsync(w => w.Id == id);

            if (writer != null)
            {
                return new SuccessDataResult<Writer>(writer);
            }
            return new ErrorDataResult<Writer>("Yazar Bulunamadı");
        }

        public async Task<IDataResult<Writer>> GetByUserId(int userId)
        {
            var writer = await _writerDal.GetAsync(u => u.UserId == userId);

            if (writer != null)
            {
                return new SuccessDataResult<Writer>(writer);
            }
            return new ErrorDataResult<Writer>(null, "Yazar Bulunamadı");
        }

        public async Task<IDataResult<Writer>> UpdateAsync(WriterForUpdateDto dto)
        {

            var writer = await _writerDal.GetAsync(w => w.UserId == dto.Id);
            if (writer == null)
                return new ErrorDataResult<Writer>("Yazar Bulunamadı");

            var userForUpdateDto = new UserForUpdateDto()
            {
                FirstName = dto.FirstName,
                Id = dto.Id,
                LastName = dto.LastName
            };

            var result = await _userService.UpdateAsync(userForUpdateDto);
            if (!result.Success)
                return new ErrorDataResult<Writer>(result.Message);

            writer.NickName = dto.NickName;
            _writerDal.Update(writer);
            await _writerDal.SaveAsync();
            return new SuccessDataResult<Writer>(writer, "Yazar Bilgileri Güncellendi");

        }
        public async Task<IResult> DoesBlogBelongToThisWriter(int blogId, int writerId)
        {
            var blogResult = await _blogService.GetById(blogId);
            if (!blogResult.Success) return new ErrorResult(blogResult.Message);

            var writerResult = await GetById(writerId);
            if (!writerResult.Success) return new ErrorResult(writerResult.Message);

            if (blogResult.Data!.WriterId != writerId)
                return new ErrorResult($"{blogResult.Data.Title} başlıklı blog {writerResult.Data.NickName} yazarına ait değil!");
            return new SuccessResult($"{blogResult.Data.Title} başlıklı blog {writerResult.Data.NickName} yazarına ait!");
        }
    }
}
