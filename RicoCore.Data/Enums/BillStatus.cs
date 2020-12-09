using System.ComponentModel;

namespace RicoCore.Data.Enums
{
    public enum BillStatus
    {
        [Description("Đơn hàng mới")]
        New = 1,

        [Description("Đang xử lý")]
        InProgress = 2,

        [Description("Trả lại")]
        Returned = 3,

        [Description("Đã hủy")]
        Cancelled = 4,

        [Description("Đang giao hàng")]
        Shipping = 5,

        [Description("Giao thành công")]
        Completed = 6,

        [Description("Đang tạm hoãn")]
        Pending = 7,
    }
}