using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace WebApp.ViewModel
{
    public class ChangePasswordUserViewModel
    {
        public string Id { get; set; }
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage ="Пароли не совпадают")]
        public string NewPasswordConfirmed { get; set; }
    }
}
