using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistance;
using Ordering.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderRequest>
    {
        readonly IUnitOfWork unitOfWork;
        readonly IMapper mapper;
        readonly ILogger<DeleteOrderHandler> logger;

        public DeleteOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DeleteOrderHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderRequest request, CancellationToken cancellationToken)
        {
            var orderToDelete = await unitOfWork.Orders.GetByIdAsync(request.Id);

            if (orderToDelete == null)
            {
                logger.LogError($" order {orderToDelete.Id} not exist in the database");
                throw new NotFoundException(nameof(orderToDelete), request.Id);
            }

            unitOfWork.Orders.Remove(orderToDelete);
            logger.LogInformation($"Order {orderToDelete.Id} is successfully deleted...");

            return Unit.Value;
        }
    }
}
