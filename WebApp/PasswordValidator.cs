using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace WebApp.Models
{
    public class PasswordValidator:IPasswordValidator<User>
    {
        public int RequiredLength { get; set; } 
        public bool HasUpper { get; set; }
        public bool HasOneDigit { get; set; }
        public bool HasLower { get; set; }
        public PasswordValidator()
        {
            RequiredLength = 6;
            HasUpper = true;
            HasOneDigit = true;
            HasLower = true;
        }

        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();
            if (String.IsNullOrEmpty(password) || password.Length < RequiredLength)
            {
                errors.Add(new IdentityError
                {
                    Description = $"Минимальная длина пароля равна {RequiredLength}"
                });
            }
            if(HasUpper)
            {
                bool flag = false;
                for(int i=0; i<password.Length; i++)
                {
                    if (char.IsUpper(password[i])) flag = true;
                }
                if (!flag) errors.Add(new IdentityError { Description = "Пароль должен содержать минимум одну заглавную букву" });
            }
            if (HasOneDigit)
            {
                bool flag = false;
                for (int i = 0; i < password.Length; i++)
                {
                    if (char.IsDigit(password[i])) flag = true;
                }
                if (!flag) errors.Add(new IdentityError { Description = "Пароль должен содержать минимум одну цифру" });
            }
            if (HasLower)
            {
                bool flag = false;
                for (int i = 0; i < password.Length; i++)
                {
                    if (char.IsLower(password[i])) flag = true;
                }
                if (!flag) errors.Add(new IdentityError { Description = "Пароль должен содержать минимум одну строчную букву" });
            }
            return Task.FromResult(errors.Count == 0 ?
               IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
