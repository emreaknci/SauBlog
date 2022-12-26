using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.User.Models
{
    public class ChangePasswordModel
    {
        public int UserId{ get; set; }
        [Required]
        public string OldPassword{ get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmNewPassword { get; set; }
    }
}
