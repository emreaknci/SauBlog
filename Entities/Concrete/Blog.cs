
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrete
{
    public class Blog : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImagePath { get; set; }
        public int? WriterId { get; set; }
        public Writer? Writer { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Category>? Categories { get; set; }
    }
}
