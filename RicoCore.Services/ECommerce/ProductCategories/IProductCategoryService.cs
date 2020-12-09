using RicoCore.Data.Entities.ECommerce;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Utilities.Dtos;
using System.Collections.Generic;

namespace RicoCore.Services.ECommerce.ProductCategories
{
    public interface IProductCategoryService : IWebServiceBase<ProductCategory, int, ProductCategoryViewModel>
    {
        //void Add(ProductCategoryViewModel productCategoryVm);
        //void Update(ProductCategoryViewModel productCategoryVm);
        //void Delete(Guid id);
        //ProductCategoryViewModel GetById(Guid id);
        //List<ProductCategoryViewModel> GetAll();

        bool ValidateAddProductCategoryName(ProductCategoryViewModel productCategoryVm);
        bool ValidateUpdateProductCategoryName(ProductCategoryViewModel productCategoryVm);

        bool ValidateAddProductCategoryOrder(ProductCategoryViewModel productCategoryVm);
        bool ValidateUpdateProductCategoryOrder(ProductCategoryViewModel productCategoryVm);
        ProductCategoryViewModel SetValueToNewProductCategory(int parentId);
        int SetNewProductCategoryOrder(int? parentId);        

        bool ValidateAddProductCategoryHotOrder(ProductCategoryViewModel productCategoryVm);
        bool ValidateUpdateProductCategoryHotOrder(ProductCategoryViewModel productCategoryVm);
        int SetNewProductCategoryHotOrder();

        bool ValidateAddProductCategoryHomeOrder(ProductCategoryViewModel productCategoryVm);
        bool ValidateUpdateProductCategoryHomeOrder(ProductCategoryViewModel productCategoryVm);
        int SetNewProductCategoryHomeOrder();

       
       
      

        string GetNameById(int parentId);

       

        ProductCategoryViewModel GetByUrl(string url);

        List<ProductCategoryViewModel> GetAll(string keyword);

        List<ProductCategoryViewModel> GetAllByParentId(int? parentId);

        PagedResult<ProductCategoryViewModel> GetAllPaging(string keyword, int page, int pageSize);

        PagedResult<ProductCategoryViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        List<ProductCategoryViewModel> GetHomeCategories(int top);

        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);

        void ReOrder(int sourceId, int targetId);

        List<ProductCategory> AllSubCategories(int id);

        bool HasExistsCode(string code);
    }
}