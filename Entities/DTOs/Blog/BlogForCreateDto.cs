using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entities.DTOs.Blog
{
    public class BlogForCreateDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? WriterId { get; set; }
        public IFormFile? Image { get; set; }
        public List<int>? CategoryIds { get; set; }
    }
}
