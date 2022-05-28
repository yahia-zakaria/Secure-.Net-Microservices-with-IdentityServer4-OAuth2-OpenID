namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class OrderDto
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        #region BillingAddres
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        #endregion

        #region Payment
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PayemntMethod { get; set; }
        #endregion
    }
}