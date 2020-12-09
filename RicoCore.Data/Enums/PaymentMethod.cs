using System.ComponentModel;

namespace RicoCore.Data.Enums
{
    public enum PaymentMethod
    {
        [Description("Thanh toán khi nhận hàng")]
        CashOnDelivery = 1,

        [Description("Chuyển khoản internet banking")]
        OnlinBanking = 2,

        [Description("Qua cổng thanh toán")]
        PaymentGateway = 3,

        [Description("Qua VISA")]
        Visa = 4,

        [Description("Master Card")]
        MasterCard = 5,

        [Description("Thanh toán Paypal")]
        PayPal = 6,

        [Description("Qua ATM")]
        Atm = 7
    }
}