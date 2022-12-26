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

namespace Business.Concrete
{
    public class WriterService : IWriterService
    {
        private readonly IWriterDal _writerDal;
        private readonly IUserService _userService;

        public WriterService(IWriterDal writerDal, IUserService userService)
        {
            _writerDal = writerDal;
            _userService = userService;
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

        public async Task<IResult> DeleteAsync(int id)
        {
            var user = await _userService.GetById(id);

            if (user != null)
            {
                var result = await _userService.DeleteAsync(id);
                if (result.Success)
                {
                    return new SuccessResult("Yazar silindi");
                }
                return new ErrorResult(result.Message);
            }
            return new ErrorResult("Yazar Bulunamadı");
        }

        public async Task<IResult> ChangeNickNameAsync(int userId, string newNickName)
        {
            var writer =  GetByUserId(userId).Result.Data;

            if (writer != null)
            {
               writer.NickName= newNickName;
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
    }
}
