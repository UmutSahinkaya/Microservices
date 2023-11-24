using System.Threading.Tasks;
using System;
using Web.Models.Discount;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class DiscountService : IDiscountService
    {
        public Task<DiscountViewModel> GetDiscount(string discountCode)
        {
            throw new NotImplementedException();
        }
    }
}
