using Discount.Grpc.Protos;
using System;
using System.Threading.Tasks;
using static Discount.Grpc.Protos.DiscountProtoService;

namespace Basket.API.GrpcServices
{
    public class DiscountService
    {
        private readonly DiscountProtoServiceClient client;

        public DiscountService(DiscountProtoServiceClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            return await client.GetDiscountAsync(new GetDiscountRequest { ProductName = productName });
        }
    }
}
