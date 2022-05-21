using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repository;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<DiscountService> logger;

        public DiscountService(IDiscountRepository repository, IMapper mapper, ILogger<DiscountService> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await repository.GetDiscount(request.ProductName);
            logger.LogInformation($"Discount for product with name {request.ProductName} has been retrieved");
            return mapper.Map<CouponModel>(coupon);
        }

        public override async Task<SuccessResponse> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var result = await repository.CreateDiscount(mapper.Map<Coupon>(request.Coupon));

            if (!result)
                throw new RpcException(new Status(StatusCode.Internal, "Discount has not been saved server internal error"));

            logger.LogInformation($"Discount for product with name {request.Coupon.ProductName} has been created");
            return new SuccessResponse { Success = result };
        }

        public async override Task<SuccessResponse> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var result = await repository.UpdateDiscount(mapper.Map<Coupon>(request.Coupon));

            if (!result)
                throw new RpcException(new Status(StatusCode.Internal, "Discount has not been updated server internal error"));

            logger.LogInformation($"Discount for product with name {request.Coupon.ProductName} has been updated");
            return new SuccessResponse { Success = result };
        }

        public async override Task<SuccessResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var result = await repository.DeleteDiscount(request.ProductName);

            if (!result)
                throw new RpcException(new Status(StatusCode.Internal, "Discount has not been deleted server internal error"));

            logger.LogInformation($"Discount for product with name {request.ProductName} has been deleted");
            return new SuccessResponse { Success = result };
        }
    }
}
