using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Comment
{
    public class CommentForCreateDto
    {
        public string? Content { get; set; }
        public int? WriterId { get; set; }
        public int? BlogId { get; set; }
    }
}
