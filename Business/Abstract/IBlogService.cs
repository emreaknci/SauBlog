﻿using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBlogService
    {
        Task<IDataResult<Blog>> AddAsync(BlogForCreateDto dto);
        Task<IDataResult<Blog>> DeleteAsync(int id);
        Task<IDataResult<Blog>> UpdateAsync(BlogForUpdateDto dto);

        IDataResult<List<Blog>> GetAll();
        Task<IDataResult<Blog>> GetById(int id);
        Task<IDataResult<Blog>> GetByIdWithCategories(int id);
    }
}