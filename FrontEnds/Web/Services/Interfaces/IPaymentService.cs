using System.Threading.Tasks;
using Web.Models.FakePayment;

namespace Web.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput);
    }
}
