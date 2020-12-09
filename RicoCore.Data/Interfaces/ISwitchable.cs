using RicoCore.Infrastructure.Enums;

namespace RicoCore.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}