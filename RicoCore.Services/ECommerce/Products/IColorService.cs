using RicoCore.Data.Entities;
using RicoCore.Data.Entities.ECommerce;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Utilities.Dtos;
using System;
using System.Collections.Generic;

namespace RicoCore.Services.ECommerce.Products
{
    public interface IColorService : IWebServiceBase<Color, int, ColorViewModel>
    {
        //void Add(ProductViewModel productVm);
        //void Update(ProductViewModel productVm);
        //void Delete(Guid id);
        //ProductViewModel GetById(Guid id);
        //List<ProductViewModel> GetAll();
        //PagedResult<ProductViewModel> GetAllPaging(string keyword, int pageSize, int page = 1);
        PagedResult<ColorViewModel> GetAllPaging(string keyword, int page, int pageSize, string sortBy);
        void MultiDelete(IList<string> selectedIds);
        bool ValidateAddColorName(ColorViewModel vm);
        bool ValidateAddSortOrder(ColorViewModel vm);

        bool ValidateUpdateColorName(ColorViewModel vm);
        bool ValidateUpdateSortOrder(ColorViewModel vm);
        int SetNewColorOrder();
        ColorViewModel GetByUrl(string url);
    }
}