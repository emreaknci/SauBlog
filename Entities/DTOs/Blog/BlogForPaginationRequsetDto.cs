using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.DTOs.Blog
{
    public class BlogForPaginationRequest:BasePaginationRequest
    {
        public List<int>? WriterIds{ get; set; }
        public List<int>? CategoryIds{ get; set; }
    }

   
}
