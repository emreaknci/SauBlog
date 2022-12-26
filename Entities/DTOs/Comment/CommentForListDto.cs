using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Comment
{
    public class CommentForListDto
    {
        public int? Id { get; set; }
        public int? WriterId { get; set; }
        public string? WriterNickName { get; set; }
        public int? BlogId { get; set; }
        public string? BlogTitle { get; set; }
        public string? Content { get; set; }
        public DateOnly? CreatedDate { get; set; }
        public DateOnly? UpdatedDate { get; set; }
        public bool? Status { get; set; }
    }
}
