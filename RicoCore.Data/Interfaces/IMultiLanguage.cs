using RicoCore.Data.Entities;

namespace RicoCore.Data.Interfaces
{
    public interface IMultiLanguage<T>
    {
        T LanguageId { set; get; }
    }
}