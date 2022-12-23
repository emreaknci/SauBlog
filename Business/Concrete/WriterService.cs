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

        public IDataResult<List<Writer>> GetAll()
        {
            var list = _writerDal.GetAll().ToList();
            return new SuccessDataResult<List<Writer>>(list);
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

        public async Task<IDataResult<Writer>> UpdateAsync(WriterForUpdateDto dto)
        {

            var writer = await _writerDal.GetAsync(w => w.UserId == dto.Id);
            if (writer == null)
                return new ErrorDataResult<Writer>("Yazar Bulunamadı");

            var userForUpdateDto = new UserForUpdateDto()
            {
                Email = dto.Email,
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
