using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace WebApp.ViewModel
{ 
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Это поле не должно быть пустым")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage ="Это поле не должно быть пустым")]
        [Display(Name ="Год рождения")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Это поле не должно быть пустым")]
        [Display(Name ="Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Это поле не должно быть пустым")]
        [Compare("Password", ErrorMessage ="Пароли не совпадают")]
        [Display(Name ="Подтверждение пароля")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

    }
}
