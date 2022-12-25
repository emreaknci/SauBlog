using Core.DataAccess;
using Entities.Concrete;
using System.Linq.Expressions;
using Entities.DTOs.Blog;

namespace DataAccess.Abstract;

public interface IBlogDal : IRepository<Blog>
{
    (List<BlogForListDto> list, int totalCount) GetWithPagination(int index, int size, bool tracking = true, Expression<Func<BlogForListDto, bool>>? filter = null);
    List<Blog>? GetAllByCategoryId(int categoryId);

}