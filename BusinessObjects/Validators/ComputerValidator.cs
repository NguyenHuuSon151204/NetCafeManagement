using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using BusinessObjects.Models;

namespace BusinessObjects.Validators
{
    

    public class ComputerValidator : AbstractValidator<Computer>
    {
        public ComputerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên máy tính không được để trống.")
                .MaximumLength(50).WithMessage("Tên không vượt quá 50 ký tự.");

            RuleFor(x => x.Tier)
                .MaximumLength(20).WithMessage("Tier không vượt quá 20 ký tự.")
                .When(x => x.Tier != null);

            RuleFor(x => x.HourlyRate)
                .GreaterThanOrEqualTo(0).WithMessage("Giá theo giờ phải lớn hơn hoặc bằng 0.");

            RuleFor(x => x.Status)
                .InclusiveBetween((byte)0, (byte)2).WithMessage("Trạng thái máy tính không hợp lệ.")
                .When(x => x.Status.HasValue);
        }
    }

}
