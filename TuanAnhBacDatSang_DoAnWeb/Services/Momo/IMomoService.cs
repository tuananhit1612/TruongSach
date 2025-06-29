using TuanAnhBacDatSang_DoAnWeb.Models;
using TuanAnhBacDatSang_DoAnWeb.Models.Momo;

namespace TuanAnhBacDatSang_DoAnWeb.Services.Momo
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfo model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
