using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
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
        IWriterDal _writerDal;

        public WriterService(IWriterDal writerDal)
        {
            _writerDal = writerDal;
        }

        public async Task<IDataResult<Writer>> AddAsync(WriterForCreateDto dto)
        {
            Writer writer = new()
            {
               UserId= dto.UserId,
               NickName= dto.NickName
            };
            await _writerDal.AddAsync(writer);
            await _writerDal.SaveAsync();
            return new SuccessDataResult<Writer>(writer);
        }

        public Task<IDataResult<Writer>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<Writer>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<Writer>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<Writer>> UpdateAsync(WriterForUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
