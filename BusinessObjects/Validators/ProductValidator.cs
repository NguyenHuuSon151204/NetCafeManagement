using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using BusinessObjects.Models;




namespace BusinessObjects.Validators
{
   
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên sản phẩm không được để trống.")
                .MaximumLength(100).WithMessage("Tên sản phẩm không vượt quá 100 ký tự.");

            RuleFor(x => x.Category)
                .MaximumLength(50).WithMessage("Danh mục không vượt quá 50 ký tự.")
                .When(x => !string.IsNullOrEmpty(x.Category));

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Giá sản phẩm không được âm.");
        }
    }

}
