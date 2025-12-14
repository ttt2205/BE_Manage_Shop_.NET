using Manage_Store.Services;
using Manage_Store.Models.Requests;
using Manage_Store.Models.Dtos;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Manage_Store.Services.Impl
{
    public class MomoService : IMomoService
    {
        private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public MomoService(IConfiguration config, HttpClient httpClient)
    {
        _config = config;
        _httpClient = httpClient;
    }

    public async Task<string> CreatePaymentUrl(OrderInfoDto model)
    {
        model.OrderId = DateTime.UtcNow.Ticks.ToString();
        model.OrderInfo = "Khách hàng: " + model.FullName + ". Nội dung: " + model.OrderInfo;

        string endpoint = _config["Momo:ApiEndpoint"];
        string partnerCode = _config["Momo:PartnerCode"];
        string accessKey = _config["Momo:AccessKey"];
        string secretKey = _config["Momo:SecretKey"];
        string orderInfo = model.OrderInfo;
        string redirectUrl = "http://localhost:5180/order-success";
        string ipnUrl = "https://webhook.site/b3086a60-705a-4e2a-a9e6-054593922896";
        string requestType = "captureWallet";
        string amount = model.Amount.ToString();
        string orderId = model.OrderId;
        string requestId = model.OrderId;
        string extraData = "";

        string rawHash = "accessKey=" + accessKey +
                         "&amount=" + amount +
                         "&extraData=" + extraData +
                         "&ipnUrl=" + ipnUrl +
                         "&orderId=" + orderId +
                         "&orderInfo=" + orderInfo +
                         "&partnerCode=" + partnerCode +
                         "&redirectUrl=" + redirectUrl +
                         "&requestId=" + requestId +
                         "&requestType=" + requestType;

        string signature = ComputeHmacSha256(rawHash, secretKey);

        var requestData = new MomoPaymentRequest
        {
            partnerCode = partnerCode,
            requestId = requestId,
            amount = (long)model.Amount,
            orderId = orderId,
            orderInfo = orderInfo,
            redirectUrl = redirectUrl,
            ipnUrl = ipnUrl,
            requestType = requestType,
            extraData = extraData,
            lang = "vi",
            signature = signature
        };

        var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(endpoint, content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var momoResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
            return momoResponse.GetProperty("payUrl").GetString(); 
        }

        return null;
    }

    private string ComputeHmacSha256(string message, string secretKey)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var messageBytes = Encoding.UTF8.GetBytes(message);

        byte[] hashBytes;
        using (var hmac = new HMACSHA256(keyBytes))
        {
            hashBytes = hmac.ComputeHash(messageBytes);
        }
        
        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        return hashString;
    }

    }

    
}