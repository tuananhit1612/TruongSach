using TuanAnhBacDatSang_DoAnWeb.Models.Vnpay;

namespace TuanAnhBacDatSang_DoAnWeb.Services.VnPay
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
