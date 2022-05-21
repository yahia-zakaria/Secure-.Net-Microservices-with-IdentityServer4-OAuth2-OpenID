using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace Discount.API.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration configuration;
        private readonly IDbConnection conn;

        public DiscountRepository(IConfiguration configuration, IDbConnection dbConnection)
        {
            this.configuration = configuration;
            this.conn = dbConnection;
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            conn.Open();

            var affectedRows = await conn.ExecuteAsync("INSERT INTO Coupon(ProductName, Description, Amount)" +
                " Values(@productName, @description, @amount)", new
                {
                    productName = coupon.ProductName,
                    description = coupon.Description,
                    amount = coupon.Amount
                });

            conn.Close();

            if (affectedRows == 0)
                return false;
            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            conn.Open();

            var affectedRows = await conn.ExecuteAsync("DELETE From Coupon WHERE ProductName=@productName",
                new { productName = productName });

            conn.Close();

            if (affectedRows==0)
            {
                return false;
            }
            return true;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            conn.Open();    

            var coupon = await conn.QueryFirstOrDefaultAsync<Coupon>("SELECT * From Coupon WHERE ProductName=@productName",
                new {productName = productName});

            conn.Close();

            if (coupon == null)
                return new Coupon { ProductName = "No Discount", Description = "No disc", Amount = 0};
            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            conn.Open();

            var affectedRows = await conn.ExecuteAsync("UPDATE Coupon SET ProductName=@productName, Description=@description," +
                " Amount = @amount WHERE Id=@id", new
                {
                    productName = coupon.ProductName,
                    description = coupon.Description,
                    amount = coupon.Amount,
                    id = coupon.Id
                });

            conn.Close();

            if(affectedRows==0)
                return false;
            return true;
        }
    }
}
