using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using BusinessObjects.Models;

namespace BusinessObjects.Validators
{
   

    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên khách hàng không được để trống.")
                .MaximumLength(100).WithMessage("Tên không vượt quá 100 ký tự.");

            RuleFor(x => x.Phone)
                .Matches(@"^\d{10,11}$").WithMessage("Số điện thoại phải có 10-11 chữ số.")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("Số dư phải >= 0.")
                .When(x => x.Balance.HasValue);
        }
    }

}
