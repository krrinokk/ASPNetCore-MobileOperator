using System.ComponentModel.DataAnnotations;
using System.Resources;

namespace DAL.Models
{
    // Класс для представления модели для входа в систему

    public class LoginViewModel
    {
        // Атрибут, указывающий, что поле Email является обязательным для заполнения
        [Required]
        // Атрибут, задающий название поля в форме
        [Display(Name = "Email")]
        public string Email { get; set; }

        // Атрибут, указывающий, что поле Password является обязательным для заполнения
        [Required]
        // Атрибут, указывающий на тип данных для поля Password
        [DataType(DataType.Password)]
        // Атрибут, задающий название поля в форме
        [Display(Name = "Password")]
        public string Password { get; set; }

        // Атрибут, задающий название поля в форме
        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
