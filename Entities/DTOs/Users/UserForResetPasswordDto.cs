using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Users
{
    public class UserForResetPasswordDto
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
