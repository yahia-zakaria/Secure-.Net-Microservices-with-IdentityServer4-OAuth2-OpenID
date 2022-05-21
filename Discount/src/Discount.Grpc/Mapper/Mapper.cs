using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
