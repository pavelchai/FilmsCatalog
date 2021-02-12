using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    public class SignUpViewModel
    {
        // [Required(ErrorMessage = "Обязательное поле")]
        [EmailAddress(ErrorMessage = "Необходимо указать корректный email")]
        // [DataType(DataType.EmailAddress)]
        [Display(Name = "Email*")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Имя*")]
        public string FirstName { get; set; }
        
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль*")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтвердить пароль*")]
        public string PasswordConfirm { get; set; }
    }
}