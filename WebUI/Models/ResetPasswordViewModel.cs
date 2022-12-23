using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class ResetPasswordViewModel
    {
        public int UserId { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }
    }
}
