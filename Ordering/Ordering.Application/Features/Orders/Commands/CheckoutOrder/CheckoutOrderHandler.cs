using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistance;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    internal class CheckoutOrderHandler : IRequestHandler<CheckoutOrderRequest, int>
    {
        readonly IUnitOfWork unitOfWork;
        readonly IMapper mapper;
        readonly ILogger<CheckoutOrderHandler> logger;
        readonly IEmailService emailService;

        public CheckoutOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, 
            ILogger<CheckoutOrderHandler> logger, IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.emailService = emailService;
        }

        public async Task<int> Handle(CheckoutOrderRequest request, CancellationToken cancellationToken)
        {
            var newOrder = unitOfWork.Orders.Add(mapper.Map<Order>(request));
            await unitOfWork.SaveChangesAsync();

            logger.LogInformation($"Order {newOrder.Id} is successfully created...");
            await Sendmail(newOrder);
            return newOrder.Id;
        }

        private async Task Sendmail(Order order)
        {
            Email email = new() { To = "yahiazakaria91@hotmail.com", Body = "Order was created",
                Subject = "Order was created" };
            try
            {
                await emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                logger.LogError($"Order {order.Id} failed due to an error im mail service: {ex.Message}");
            }
        }
    }

}
