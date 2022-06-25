using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistance;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderRequest>
    {
        readonly IMapper mapper;
        readonly IUnitOfWork unitOfWork;
        readonly ILogger<UpdateOrderHandler> logger;

        public UpdateOrderHandler(IMapper mapper, IUnitOfWork unitOfWork, ILogger<UpdateOrderHandler> logger)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await unitOfWork.Orders.GetByIdAsync(request.Id);

            if (orderToUpdate == null)
            {
                logger.LogError($" order {orderToUpdate.Id} not exist in the database");
                throw new NotFoundException(nameof(orderToUpdate), request.Id);
            }

            mapper.Map(request, orderToUpdate, typeof(UpdateOrderRequest), typeof(Order));
            unitOfWork.Orders.Update(orderToUpdate);
            logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated...");

            return Unit.Value;
        }
    }
}
