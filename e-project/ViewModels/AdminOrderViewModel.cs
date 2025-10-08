using e_project.Models;

namespace e_project.ViewModels
{
    
    public class AdminOrderViewModel
    {
        public int OrderId { get; set; }
        public string UserEmail { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public List<PhotoOrderItem> PhotoOrderItems { get; set; }
    }

}
