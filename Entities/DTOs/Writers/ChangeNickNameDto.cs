using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Writers
{
    public class ChangeNickNameDto
    {
        public int UserId{ get; set; }
        public string NewNickName{ get; set; }
    }
}
