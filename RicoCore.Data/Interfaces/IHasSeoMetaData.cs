namespace RicoCore.Data.Interfaces
{
    public interface IHasSeoMetaData
    {
        string MetaTitle { set; get; }
        string Url { set; get; }
        string MetaKeywords { set; get; }
        string MetaDescription { get; set; }
    }
}