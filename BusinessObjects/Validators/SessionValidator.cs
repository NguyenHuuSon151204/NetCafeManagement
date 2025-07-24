using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using BusinessObjects.Models;

namespace BusinessObjects.Validators
{

    public class SessionValidator : AbstractValidator<Session>
    {
        public SessionValidator()
        {
            RuleFor(x => x.ComputerId)
                .NotNull().WithMessage("Vui lòng chọn máy tính.");

            RuleFor(x => x.CustomerId)
                .NotNull().WithMessage("Vui lòng chọn khách hàng.");

            RuleFor(x => x.StartTime)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Thời gian bắt đầu không hợp lệ.");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage("Thời gian kết thúc phải lớn hơn thời gian bắt đầu.")
                .When(x => x.EndTime.HasValue);

            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Tổng tiền không được âm.")
                .When(x => x.TotalAmount.HasValue);
        }
    }

}
