using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using BusinessObjects.Models;

namespace BusinessObjects.Validators
{
    

    public class ShiftValidator : AbstractValidator<Shift>
    {
        public ShiftValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotNull().WithMessage("Ca làm phải gán cho một nhân viên.");

            RuleFor(x => x.StartTime)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Thời gian bắt đầu không hợp lệ.");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage("Thời gian kết thúc phải lớn hơn thời gian bắt đầu.")
                .When(x => x.EndTime.HasValue);

            RuleFor(x => x.StartCash)
                .GreaterThanOrEqualTo(0).WithMessage("Số tiền đầu ca không được âm.");

            RuleFor(x => x.EndCash)
                .GreaterThanOrEqualTo(0).WithMessage("Số tiền cuối ca không được âm.")
                .When(x => x.EndCash.HasValue);

            RuleFor(x => x.Notes)
                .MaximumLength(200).WithMessage("Ghi chú không vượt quá 200 ký tự.")
                .When(x => !string.IsNullOrEmpty(x.Notes));
        }
    }

}
