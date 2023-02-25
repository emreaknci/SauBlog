using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Blog
{
    public class LastBlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CommentCount { get; set; }
        public DateOnly CreatedDate { get; set; }
    }
}
