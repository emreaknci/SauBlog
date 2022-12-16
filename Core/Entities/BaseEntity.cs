using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public DateOnly CreatedDate { get; set; }

        public DateOnly? UpdatedDate { get; set; }
        public bool Status { get; set; } = true;
    }
}
