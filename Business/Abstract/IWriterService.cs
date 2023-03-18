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
        Task<IResult>  DeleteByUserIdAsync(int userId);
        Task<IDataResult<Writer>> UpdateAsync(WriterForUpdateDto dto);
        Task<IResult> ChangeNickNameAsync(int userId, string nickName);

        IDataResult<List<Writer>> GetAll();
        IDataResult<List<Writer>> GetAllWithUserInfo();
        Task<IDataResult<Writer>> GetById(int id);
        Task<IDataResult<Writer>> GetByNickName(string nickName);
        Task<IDataResult<Writer>> GetByIdWithAllInfo(int id);
        Task<IDataResult<Writer>> GetByIdWithUserInfoAsync(int id);
        Task<IDataResult<Writer>> GetByUserId(int userId);

    }
}
