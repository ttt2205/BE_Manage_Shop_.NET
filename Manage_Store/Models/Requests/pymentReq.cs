using System.ComponentModel.DataAnnotations;

namespace Manage_Store.Models.Requests
{
    public class PaymentReq
    {

        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }

    }
}