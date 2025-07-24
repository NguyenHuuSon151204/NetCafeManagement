using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

using FluentValidation;

namespace BusinessObjects.Validators
{
    
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Tên nhân viên không được để trống.")
                .MaximumLength(100).WithMessage("Tên không vượt quá 100 ký tự.");

            RuleFor(x => x.Phone)
                .Matches(@"^\d{10,11}$").WithMessage("Số điện thoại không hợp lệ.")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Role)
                .MaximumLength(50).WithMessage("Vai trò không vượt quá 50 ký tự.")
                .When(x => x.Role != null);
        }
    }

}
