using System.ComponentModel.DataAnnotations;
namespace DAL.Models
{
    // Этот класс описывает модель представления для регистрации нового пользователя
    public class RegisterViewModel
    {
        // Аннотация Required указывает, что поле обязательно для заполнения
        // Аннотация Display задает отображаемое имя для поля на странице
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        // Аннотация DataType указывает тип данных для поля и используется для правильного отображения на странице
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        // Аннотация Compare указывает, что значение данного поля должно совпадать со значением указанного поля
        // Аннотация ErrorMessage задает сообщение об ошибке, которое будет отображаться, если значения не совпадают
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}