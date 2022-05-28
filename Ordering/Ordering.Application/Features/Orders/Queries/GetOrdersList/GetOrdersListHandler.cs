using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListHandler : IRequestHandler<GetOrdersListRequest, List<OrderDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetOrdersListHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersListRequest request, CancellationToken cancellationToken)
        {
            var orders = await unitOfWork.Orders.GetOrderByUserNameAsync(request.UserName);
            return mapper.Map<List<OrderDto>>(orders);
        }
    }
}
