using System.Net.Sockets;
using System.Net;
using TuanAnhBacDatSang_DoAnWeb.Models.Vnpay;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace TuanAnhBacDatSang_DoAnWeb.Libraries
{
    public class VnPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
        private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

        public PaymentResponseModel GetFullResponseData(IQueryCollection collection, string hashSecret)
        {
            var vnPay = new VnPayLibrary();

            // Lấy toàn bộ dữ liệu bắt đầu bằng "vnp_"
            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnPay.AddResponseData(key, value);
                }
            }

            // Lấy thông tin cần thiết
            var orderId = vnPay.GetResponseData("vnp_TxnRef");
            var vnPayTranId = vnPay.GetResponseData("vnp_TransactionNo");
            var vnpResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
            var vnpSecureHash = collection.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value;
            var orderInfo = vnPay.GetResponseData("vnp_OrderInfo");

            // Lấy amount và chia lại cho 100
            decimal.TryParse(vnPay.GetResponseData("vnp_Amount"), out var rawAmount);
            var amount = rawAmount / 100;

            // Optional
            var payDate = vnPay.GetResponseData("vnp_PayDate");
            var bankCode = vnPay.GetResponseData("vnp_BankCode");
            var cardType = vnPay.GetResponseData("vnp_CardType");

            // Kiểm tra chữ ký
            var checkSignature = vnPay.ValidateSignature(vnpSecureHash, hashSecret);
            if (!checkSignature)
            {
                return new PaymentResponseModel
                {
                    Success = false,
                    Message = "Sai chữ ký (invalid signature)",
                    VnPayResponseCode = vnpResponseCode
                };
            }

            return new PaymentResponseModel
            {
                Success = vnpResponseCode == "00",
                PaymentMethod = "VnPay",
                OrderDescription = orderInfo,
                OrderId = orderId,
                PaymentId = vnPayTranId,
                TransactionId = vnPayTranId,
                Token = vnpSecureHash,
                VnPayResponseCode = vnpResponseCode,
                Amount = amount,
                PayDate = payDate,
                BankCode = bankCode,
                CardType = cardType,
                Message = GetResponseMessage(vnpResponseCode)
            };
        }
        private string GetResponseMessage(string code)
        {
            return code switch
            {
                "00" => "Giao dịch thành công",
                "01" => "Gỗi hệ thống",
                "02" => "Ngân hàng từ chối thanh toán",
                "24" => "Khách hàng hủy giao dịch",
                "97" => "Không được phép truy cập",
                "99" => "Giao dịch chưa xác định",
                _ => "Không rõ trạng thái giao dịch"
            };
        }

        public string GetIpAddress(HttpContext context)
        {
            var ipAddress = string.Empty;
            try
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;

                if (remoteIpAddress != null)
                {
                    if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                            .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                    }

                    if (remoteIpAddress != null) ipAddress = remoteIpAddress.ToString();

                    return ipAddress;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "127.0.0.1";
        }

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }
        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }
        public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
        {
            var data = new StringBuilder();

            foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            var querystring = data.ToString();

            baseUrl += "?" + querystring;
            var signData = querystring;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }

            var vnpSecureHash = HmacSha512(vnpHashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnpSecureHash;

            return baseUrl;
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var rspRaw = GetResponseData();
            var myChecksum = HmacSha512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
        private string HmacSha512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        private string GetResponseData()
        {
            var data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }

            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }

            foreach (var (key, value) in _responseData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }

            return data.ToString();
        }
    }
}
public class VnPayCompare : IComparer<string>
{
    public int Compare(string x, string y)
    {
        if (x == y) return 0;
        if (x == null) return -1;
        if (y == null) return 1;
        var vnpCompare = CompareInfo.GetCompareInfo("en-US");
        return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
    }
}
