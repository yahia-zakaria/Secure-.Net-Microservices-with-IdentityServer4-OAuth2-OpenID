using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderValidator()
        {
            RuleFor(r => r.UserName)
                .NotEmpty()
                .WithMessage("{UserName} is required");
            RuleFor(r => r.TotalPrice)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("{TotalPrice} should be greater than 0");
            RuleFor(r=>r.EmailAddress)
                .EmailAddress()
                .WithMessage("{EmailAddress} is invalid");
        }
    }
}
