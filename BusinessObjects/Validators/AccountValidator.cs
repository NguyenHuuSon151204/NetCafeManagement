using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using BusinessObjects.Models;


namespace BusinessObjects.Validators
{
    
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username không được để trống.")
                .MaximumLength(50).WithMessage("Username không được vượt quá 50 ký tự.");

            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("Password không được để trống.");

            RuleFor(x => x.Salt)
                .NotEmpty().WithMessage("Salt không được để trống.");

            RuleFor(x => x.FailedAttempts)
                .GreaterThanOrEqualTo(0).WithMessage("Số lần đăng nhập sai không hợp lệ.")
                .When(x => x.FailedAttempts.HasValue);
        }
    }

}


