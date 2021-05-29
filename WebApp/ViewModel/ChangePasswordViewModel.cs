using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace WebApp.ViewModel
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        [Display(Name="Новый пароль")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
