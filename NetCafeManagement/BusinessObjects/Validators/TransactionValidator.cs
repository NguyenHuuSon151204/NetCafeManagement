using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using BusinessObjects.Models;

namespace BusinessObjects.Validators
{
    

    public class TransactionValidator : AbstractValidator<Transaction>
    {
        public TransactionValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotNull().WithMessage("Giao dịch phải gán cho một khách hàng.");

            RuleFor(x => x.Amount)
                .NotEqual(0).WithMessage("Số tiền giao dịch phải khác 0.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Loại giao dịch không được để trống.")
                .Must(type => type == "Nạp tiền" || type == "Hoàn tiền" || type == "Trừ tiền")
                .WithMessage("Loại giao dịch không hợp lệ (Nạp tiền, Hoàn tiền, Trừ tiền).");

            RuleFor(x => x.CreatedAt)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Ngày giao dịch không được vượt quá hiện tại.")
                .When(x => x.CreatedAt.HasValue);
        }
    }

}
