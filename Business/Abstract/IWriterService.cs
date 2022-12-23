using Core.Entities;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IWriterService
    {
        Task<IDataResult<Writer>> AddAsync(WriterForCreateDto dto);
        Task<IResult>  DeleteAsync(int id);
        Task<IDataResult<Writer>> UpdateAsync(WriterForUpdateDto dto);

        IDataResult<List<Writer>> GetAll();
        Task<IDataResult<Writer>> GetById(int id);

    }
}
