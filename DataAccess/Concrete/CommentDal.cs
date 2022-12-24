using Core.DataAccess;
using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;

namespace DataAccess.Concrete
{
    public class CommentDal : EfBaseRepository<Comment, BlogDbContext>, ICommentDal
    {
        public CommentDal(BlogDbContext context) : base(context)
        {
        }
    }
}
