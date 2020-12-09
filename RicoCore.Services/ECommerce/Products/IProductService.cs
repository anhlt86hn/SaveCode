using RicoCore.Data.Entities.ECommerce;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Utilities.Dtos;
using System;
using System.Collections.Generic;

namespace RicoCore.Services.ECommerce.Products
{
    public interface IProductService : IWebServiceBase<Product, Guid, ProductViewModel>
    {
        //void Add(ProductViewModel productVm);
        //void Update(ProductViewModel productVm);
        //void Delete(Guid id);
        //ProductViewModel GetById(Guid id);
        //List<ProductViewModel> GetAll();
        //PagedResult<ProductViewModel> GetAllPaging(string keyword, int pageSize, int page = 1);
        List<TagViewModel> GetAllTags();

        //bool ValidateAddColorName(ProductItemViewModel productItemVm);
        bool ValidateAddProductItemName(ProductItemViewModel productItemVm);
        bool ValidateUpdateProductItemName(ProductItemViewModel productItemVm);

        bool ValidateAddProductItemOrder(ProductItemViewModel productItemVm);
        bool ValidateUpdateProductItemOrder(ProductItemViewModel productItemVm);
        int SetNewProductItemOrder(Guid productId);

        bool ValidateAddProductItemHotOrder(ProductItemViewModel productItemVm);
        bool ValidateUpdateProductItemHotOrder(ProductItemViewModel productItemVm);
        int SetNewProductItemHotOrder();

        bool ValidateAddProductItemHomeOrder(ProductItemViewModel productItemVm);
        bool ValidateUpdateProductItemHomeOrder(ProductItemViewModel productItemVm);
        int SetNewProductItemHomeOrder();

        ProductItemViewModel GetProductItemById(int id);
        ProductItemViewModel GetProductItemByUrl(int id, string url);
        ProductItemViewModel GetProductItemByProductId(string url, Guid productId);
        List<ProductItemViewModel> GetItemsByProductId(Guid id);
        void AddProductItem(ProductItemViewModel productItemVm);
        void UpdateProductItem(ProductItemViewModel productItemVm);
        //ProductItemViewModel PrepareProductItemModel();
        List<ProductItemViewModel> GetProductItems();
        List<ProductItemViewModel> GetProductItemsByProductId(Guid id);


        bool ValidateAddProductName(ProductViewModel productVm);
        bool ValidateUpdateProductName(ProductViewModel productVm);

        bool ValidateAddProductOrder(ProductViewModel productVm);
        bool ValidateUpdateProductOrder(ProductViewModel productVm);
        ProductViewModel SetValueToNewProduct(int productCategoryId);
        int SetNewProductOrder(int categoryId);

        bool ValidateAddProductHotOrder(ProductViewModel productVm);
        bool ValidateUpdateProductHotOrder(ProductViewModel productVm);
        int SetNewProductHotOrder();

        bool ValidateAddProductHomeOrder(ProductViewModel productVm);
        bool ValidateUpdateProductHomeOrder(ProductViewModel productVm);
        int SetNewProductHomeOrder();



        void DeleteProductItem(int id);
                     
        
        string GetDefaultImageForProductDetail(Guid productId);
     
        List<ColorViewModel> GetColors();
        string GetColor(int colorId);
        ProductViewModel GetByUrl(string url);
        //ProductItemViewModel GetProductItemByUrl(string url);
        
        
       
        void CreateTagId(ref string tagId, ref List<string> listStr, ref List<int> listDuoi);
        void CreateTagAndProductTag(Product post, ProductViewModel postVm);
        int SetNewTagOrder(out List<int> list);
        List<Product> GetProductsByTagName(string tagName);
        void Update(ProductViewModel productVm, string oldTags);

        string GetColorName(string color);
        List<ProductItemViewModel> GetProductItemsByProductIdAndColorUrl(Guid id, string color);
        PagedResult<ProductSingleViewModel> GetAllProductsPaging(string color, string keyword, int page, int pageSize, string sortBy);
        PagedResult<ProductUnitViewModel> GetAllProductsPaging(string url, int? categoryId, int page, int pageSize, string sortBy);
        PagedResult<ProductUnitViewModel> GetAllProductsPaging(int categoryId, int page, int pageSize, string sortBy, string listProductItemByCategory);

        PagedResult<ProductViewModel> GetAllPaging(int categoryId, string keyword, int page, int pageSize, string sortBy);
        PagedResult<ProductViewModel> GetAllPaging(int categoryId, string keyword, int page, int pageSize);
        PagedResult<ProductItemViewModel> GetAllProductItemsPaging(int productCategoryId, Guid productId, int productColorId, string keyword, int page, int pageSize, string sortBy);
        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize, string sortBy);
        PagedResult<ProductViewModel> GetMyWishlist(Guid userId, int page, int pageSize);

        List<ProductViewModel> GetListPaging(int page, int pageSize, string sort, out int totalRow);

        List<ProductViewModel> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize, string sort, out int totalRow);
        List<ProductViewModel> GetListProductByCategoryId(int categoryId);
        List<ProductViewModel> GetListProduct(string keyword);

        List<ProductViewModel> GetListProductByTag(string tagId, int page, int pagesize, out int totalRow);

        List<ProductSingleViewModel> GetLastestProducts(int top);

        List<ProductSingleViewModel> GetNicestProducts(int top);

        List<ProductSingleViewModel> GetHotProducts(int top);

        List<ProductSingleViewModel> GetItemsHaveSpecialPrice(int top);

        List<ProductViewModel> GetReatedProducts(Guid id, int top);
        List<ProductUnitViewModel> GetReatedProducts(int productCategoryId, ProductViewModel product);
        List<ProductViewModel> GetUpsellProducts(int top);

        List<ProductUnitViewModel> GetUpsellProductItems(int top);
        List<ProductViewModel> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        List<TagViewModel> GetListTagByProductId(Guid id);
        TagViewModel GetTagByUrl(string url);
        List<TagViewModel> GetListProductTag(string searchText);

        TagViewModel GetTag(string tagId);

        List<ProductImageViewModel> GetImages(Guid productId);

        void AddImages(Guid productId, string[] images);

        void ImportExcel(string filePath, int categoryId);

        List<WholePriceViewModel> GetWholePrices(Guid productId);

        void AddWholePrice(Guid productId, List<WholePriceViewModel> wholePrices);

        void AddQuantity(Guid productId, List<ProductQuantityViewModel> quantities);

        List<ProductQuantityViewModel> GetQuantities(Guid productId);

        List<string> GetListProductByName(string name);

        void IncreaseView(Guid id);
        void MultiDelete(IList<string> selectedIds);
        void MultiDeleteProductItem(IList<string> selectedIds);
        IList<string> GetIds(int categoryId);
        bool HasExistsProductCode(string code);
        bool HasExistsProductItemCode(string code);
        bool HasExistsProductItemUrl(string url);
    }
}