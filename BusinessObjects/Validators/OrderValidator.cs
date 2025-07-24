using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using BusinessObjects.Models;


namespace BusinessObjects.Validators
{
    
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(x => x.OrderTime)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Thời gian đặt hàng không được ở tương lai.")
                .When(x => x.OrderTime.HasValue);

            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Tổng tiền không hợp lệ.")
                .When(x => x.TotalAmount.HasValue);
        }
    }

}
