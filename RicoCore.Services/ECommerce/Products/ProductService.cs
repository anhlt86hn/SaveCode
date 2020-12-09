using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RicoCore.Data.EF;
using RicoCore.Data.Entities;
using RicoCore.Data.Entities.Content;
using RicoCore.Data.Entities.ECommerce;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Infrastructure.SharedKernel;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Utilities.Constants;
using RicoCore.Utilities.Dtos;
using RicoCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RicoCore.Services.ECommerce.Products
{
    public class ProductService : WebServiceBase<Product, Guid, ProductViewModel>, IProductService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Tag, string> _tagRepository;
        private readonly IRepository<ProductTag, Guid> _productTagRepository;
        private readonly IRepository<ProductImage, Guid> _productImageRepository;
        private readonly IRepository<ProductQuantity, Guid> _productQuantityRepository;
        private readonly IRepository<WholePrice, Guid> _wholePriceRepository;
        private readonly IRepository<ProductCategory, int> _productCategoryRepository;
        private readonly IRepository<ProductWishlist, Guid> _productWishlistRepository;
        private readonly IRepository<ProductItem, int> _productItemRepository;
        private readonly IRepository<Color, int> _colorRepository;
        private readonly DbSet<Tag> Tags;
        private readonly DbSet<Product> Products;
        private readonly DbSet<ProductTag> ProductTags;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ProductColor, int> _productColorRepository;

        public ProductService(IRepository<Product, Guid> productRepository,
            IRepository<ProductTag, Guid> productTagRepository,
            IRepository<ProductWishlist, Guid> productWishlistRepository,
            IRepository<Tag, string> tagRepository,
            IRepository<ProductCategory, int> productCategoryRepository,
            IRepository<ProductImage, Guid> productImageRepository,
            IRepository<ProductQuantity, Guid> productQuantityRepository,
            IRepository<WholePrice, Guid> wholePriceRepository,
            IRepository<ProductItem, int> productItemRepository,
            IRepository<Color, int> colorRepository,
            IRepository<ProductColor, int> productColorRepository,
            AppDbContext context,
            IUnitOfWork unitOfWork)
            : base(productRepository, unitOfWork)
        {
            _colorRepository = colorRepository;
            _productColorRepository = productColorRepository;
            _productRepository = productRepository;
            _productTagRepository = productTagRepository;
            _productImageRepository = productImageRepository;
            _productQuantityRepository = productQuantityRepository;
            _wholePriceRepository = wholePriceRepository;
            _productCategoryRepository = productCategoryRepository;
            _tagRepository = tagRepository;
            _productWishlistRepository = productWishlistRepository;
            _productItemRepository = productItemRepository;
            Products = context.Set<Product>();
            ProductTags = context.Set<ProductTag>();
            Tags = context.Set<Tag>();
            _unitOfWork = unitOfWork;
        }
        public bool ValidateAddProductItemName(ProductItemViewModel productItemVm)
        {
            return _productItemRepository.GetAll().Any(x => x.Name.ToLower().Trim() == productItemVm.Name.ToLower().Trim());
        }
        public bool ValidateUpdateProductItemName(ProductItemViewModel productItemVm)
        {
            var compare = _productItemRepository.GetAllIncluding(x => x.Name.ToLower().Trim() == productItemVm.Name.ToLower().Trim() && x.ProductId == productItemVm.ProductId);
            var result = compare.Count() > 1 ? true : false;
            return result;           
        }

        public bool ValidateAddProductItemOrder(ProductItemViewModel productItemVm)
        {
            return _productItemRepository.GetAll().Any(x => x.SortOrder == productItemVm.SortOrder && x.SortOrder != 0);
        }
        public bool ValidateUpdateProductItemOrder(ProductItemViewModel productItemVm)
        {
            var compare = _productItemRepository.GetAllIncluding(x => x.SortOrder == productItemVm.SortOrder && x.SortOrder != 0 && x.ProductId == productItemVm.ProductId);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }
        public int SetNewProductItemOrder(Guid productId)
        {
            var order = 0;
            //var list = parentId == null
            //    ? _postCategoryRepository.GetAllIncluding(x => x.ParentId == null).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList()
            //    : _postCategoryRepository.GetAllIncluding(x => x.ParentId.ToString() == parentId).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();
            var list = _productItemRepository.GetAllIncluding(x => x.ProductId == productId && x.SortOrder != 0).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }

        public bool ValidateAddProductItemHotOrder(ProductItemViewModel productItemVm)
        {
            return _productItemRepository.GetAll().Any(x => x.HotOrder == productItemVm.HotOrder && x.HotOrder != 0);
        }
        public bool ValidateUpdateProductItemHotOrder(ProductItemViewModel productItemVm)
        {
            var compare = _productItemRepository.GetAllIncluding(x => x.HotOrder == productItemVm.HotOrder && x.HotOrder != 0);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }
        public int SetNewProductItemHotOrder()
        {
            var order = 0;
            var list = _productItemRepository.GetAllIncluding(x=>x.HotOrder != 0).OrderBy(x => x.HotOrder).Select(x => x.HotOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }

        public bool ValidateAddProductItemHomeOrder(ProductItemViewModel productItemVm)
        {
            return _productItemRepository.GetAll().Any(x => x.HomeOrder == productItemVm.HomeOrder && x.HomeOrder != 0);
        }
        public bool ValidateUpdateProductItemHomeOrder(ProductItemViewModel productItemVm)
        {
            var compare = _productItemRepository.GetAllIncluding(x => x.HomeOrder == productItemVm.HomeOrder && x.HomeOrder != 0);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }
        public int SetNewProductItemHomeOrder()
        {
            var order = 0;
            var list = _productItemRepository.GetAllIncluding(x=>x.HomeOrder != 0).OrderBy(x => x.HomeOrder).Select(x => x.HomeOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }


        public bool ValidateAddProductName(ProductViewModel productVm)
        {
            return _productRepository.GetAll().Any(x => x.Name.ToLower().Trim() == productVm.Name.ToLower().Trim());
        }
        public bool ValidateUpdateProductName(ProductViewModel productVm)
        {
            var compare = _productRepository.GetAllIncluding(x => x.Name.ToLower().Trim() == productVm.Name.ToLower().Trim() && x.CategoryId == productVm.CategoryId);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }

        public bool ValidateAddProductOrder(ProductViewModel productVm)
        {
            return _productRepository.GetAll().Any(x => x.SortOrder == productVm.SortOrder && x.SortOrder != 0 && x.CategoryId == productVm.CategoryId);
        }
        public bool ValidateUpdateProductOrder(ProductViewModel productVm)
        {
            var compare = _productRepository.GetAllIncluding(x => x.SortOrder == productVm.SortOrder && x.SortOrder != 0 && x.CategoryId == productVm.CategoryId);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }
        public ProductViewModel SetValueToNewProduct(int productCategoryId)
        {
            var product = new ProductViewModel();

            var productCategory = _productCategoryRepository.GetById(productCategoryId);
            if (productCategory == null)
                throw new Exception("Product Category is null");

            var productCategoryName = productCategory.Name;            
            product.CategoryId = productCategoryId;
            product.CategoryName = productCategoryName;
            product.SortOrder = SetNewProductOrder(productCategoryId);
            product.HomeOrder = SetNewProductHomeOrder();
            product.HotOrder = SetNewProductHotOrder();
            return product;
        }
        public int SetNewProductOrder(int categoryId)
        {
            var order = 0;
            var list = _productRepository.GetAllIncluding(x => x.CategoryId == categoryId && x.SortOrder != 0).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();
            order = CommonMethods.GetOrder(list);
            return order;
        }

        public bool ValidateAddProductHotOrder(ProductViewModel productVm)
        {
            return _productRepository.GetAll().Any(x => x.HotOrder == productVm.HotOrder && x.HotOrder != 0);
        }
        public bool ValidateUpdateProductHotOrder(ProductViewModel productVm)
        {
            var compare = _productRepository.GetAllIncluding(x => x.HotOrder == productVm.HotOrder && x.HotOrder != 0);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }
        public int SetNewProductHotOrder()
        {
            var order = 0;
            var list = _productRepository.GetAllIncluding(x=>x.HotOrder != 0).OrderBy(x => x.HotOrder).Select(x => x.HotOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }

        public bool ValidateAddProductHomeOrder(ProductViewModel productVm)
        {
            return _productRepository.GetAll().Any(x => x.HomeOrder == productVm.HomeOrder && x.HomeOrder != 0);
        }
        public bool ValidateUpdateProductHomeOrder(ProductViewModel productVm)
        {
            var compare = _productRepository.GetAllIncluding(x => x.HomeOrder == productVm.HomeOrder && x.HomeOrder != 0);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }
        public int SetNewProductHomeOrder()
        {
            var order = 0;
            var list = _productRepository.GetAllIncluding(x=>x.HomeOrder != 0).OrderBy(x => x.HomeOrder).Select(x => x.HomeOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }

        public string GetDefaultImageForProductDetail(Guid productId)
        {
            return _productItemRepository.GetAllIncluding(x => x.ProductId == productId).OrderByDescending(x => x.SortOrder).Select(x => x.Image1).FirstOrDefault();
        }

        public ProductItemViewModel GetProductItemById(int id)
        {
            var query = _productItemRepository.GetById(id);
            var vm = Mapper.Map<ProductItem, ProductItemViewModel>(query);
            return vm;
        }
        //public ProductItemViewModel GetProductItemById(int id)
        //{
        //    var query = 
        //    from pi in _productItemRepository.GetAllIncluding(x => x.Id == id)
        //    join pc in _productCategoryRepository.GetAll() on pi.ProductCategoryId equals pc.Id
        //    join p in _productRepository.GetAll() on pi.ProductId equals p.Id         
        //    select new ProductItemViewModel()
        //    {
        //        Id = pi.Id,
        //        Code = pi.Code,
        //        Name = pi.Name,
        //        ProductCategoryId = pi.ProductCategoryId,
        //        //ProductCategoryName = pc.Name,
        //        ProductId = pi.ProductId,
        //        //ProductCode = p.Code,
        //        //ColorId = pi.ColorId,
        //        //ColorCode = pi.ColorCode,
        //        ColorName = pi.ColorName,
        //        ColorImage = pi.ColorImage,
        //        ColorEnglishName = pi.ColorEnglishName,
        //        Image1 = pi.Image1,
        //        Image1Alt = pi.Image1Alt,
        //        Image2 = pi.Image2,
        //        Image2Alt = pi.Image2Alt,
        //        Image3 = pi.Image3,
        //        Image3Alt = pi.Image3Alt,
        //        Image4 = pi.Image4,
        //        Image4Alt = pi.Image4Alt,
        //        Image5 = pi.Image5,
        //        Image5Alt = pi.Image5Alt,
        //        Url = pi.Url,
        //        MetaTitle = pi.MetaTitle,
        //        MetaDescription = pi.MetaDescription,
        //        MetaKeywords = pi.MetaKeywords,
        //        Content = pi.Content,
        //        //LinkTitle = pi.LinkTitle,
        //        SortOrder = pi.SortOrder,
        //        Status = pi.Status,
        //        DateCreated = pi.DateCreated,
        //        DateModified = pi.DateModified
        //    };
        //    return query.FirstOrDefault();
        //}

        public ProductItemViewModel GetProductItemByUrl(int id, string url)
        {
            var query = _productItemRepository.FirstOrDefault(x => x.Id == id && x.Url == url);
            var result = Mapper.Map<ProductItem, ProductItemViewModel>(query);
            return result;
            //var query =
            //from pi in _productItemRepository.GetAll()
            //join pc in _productCategoryRepository.GetAll() on pi.ProductCategoryId equals pc.Id
            //join p in _productRepository.GetAll() on pi.ProductId equals p.Id            
            //where pi.Url == url && pi.Id == id
            //select new ProductItemViewModel()
            //{
            //    Id = pi.Id,
            //    Code = pi.Code,
            //    Name = pi.Name,
            //    ProductCategoryId = pi.ProductCategoryId,
            //    //ProductCategoryName = pc.Name,
            //    ProductId = pi.ProductId,
            //    //ProductCode = p.Code,
            //    //ColorId = pi.ColorId,
            //    //ColorCode = pi.ColorCode,
            //    //ColorName = pi.ColorName,
            //    //ColorImage = pi.ColorImage,
            //    //ColorEnglishName = pi.ColorEnglishName,
            //    Price = pi.Price,
            //    PromotionPrice = pi.PromotionPrice,
            //    Image1 = pi.Image1,
            //    Image1Alt = pi.Image1Alt,
            //    Image2 = pi.Image2,
            //    Image2Alt = pi.Image2Alt,
            //    Image3 = pi.Image3,
            //    Image3Alt = pi.Image3Alt,
            //    Image4 = pi.Image4,
            //    Image4Alt = pi.Image4Alt,
            //    Image5 = pi.Image5,
            //    Image5Alt = pi.Image5Alt,
            //    Url = pi.Url,            
            //    MetaTitle = pi.MetaTitle,
            //    MetaDescription = pi.MetaDescription,
            //    MetaKeywords = pi.MetaKeywords,
            //    Content = pi.Content,
            //    //LinkTitle = pi.LinkTitle,
            //    SortOrder = pi.SortOrder,
            //    Status = pi.Status,
            //    DateCreated = pi.DateCreated,
            //    DateModified = pi.DateModified
            //};
            //return query.FirstOrDefault();
        }

        public ProductItemViewModel GetProductItemByUrls(string url, string productUrl)
        {
            var productItem = _productItemRepository.GetAll();
            var product = _productRepository.GetAll();
            var query = (from pi in productItem
                        join p in product
                        on pi.ProductId equals p.Id
                        where pi.Url == url && p.Url == productUrl
                        select new ProductItemViewModel
                        {
                            Id = pi.Id,
                            Url = pi.Url,
                            //Code = pi.Code,
                            //ColorCode = pi.ColorCode,
                            //ColorEnglishName = pi.ColorEnglishName,
                            //ColorImage = pi.ColorImage,
                            // ColorName = pi.ColorName,
                              Content = pi.Content,                              
                                Name = pi.Name,
                                   Image1 = pi.Image1,
                                   Image1Alt = pi.Image1Alt,
                                   Image2 = pi.Image2,
                                   Image2Alt = pi.Image2Alt,
                                    Quantity = pi.Quantity,
                                    MetaTitle = pi.MetaTitle,
                                    MetaDescription = pi.MetaDescription,
                                    MetaKeywords = pi.MetaKeywords
                        }).FirstOrDefault();
            return query;                 
        }        

        private string GenearateProductItemCode()
        {
            var code = $"{CommonHelper.GenerateRandomNumberCode()}";
            while (HasExistsProductItemCode(code))
            {
                code = $"{CommonHelper.GenerateRandomNumberCode()}";
            }
            return code;
        }

        public void AddProductItem(ProductItemViewModel productItemVm)
        {
            var productColor = _productColorRepository.GetById(productItemVm.ProductColorId);

            if (!string.IsNullOrWhiteSpace(productColor.Name))
                productItemVm.ColorName = productColor.Name.Trim();

            if (!string.IsNullOrWhiteSpace(productColor.ColorImage))
                productItemVm.ColorImage = productColor.ColorImage.Trim();

            var product = _productRepository.GetById(productItemVm.ProductId);

            if (!string.IsNullOrWhiteSpace(product.Name))
                productItemVm.ProductName = product.Name.Trim();

            if (!string.IsNullOrWhiteSpace(product.Code))
                productItemVm.ProductCode = product.Code.Trim();

            var productCategoryName = _productCategoryRepository.GetById(productItemVm.ProductCategoryId).Name;
            if (!string.IsNullOrWhiteSpace(productCategoryName))
                productItemVm.ProductCategoryName = productCategoryName.Trim();

            var productItem = Mapper.Map<ProductItemViewModel, ProductItem>(productItemVm);                            
            productItem.Code = GenerateProductItemCode();
            
            if (!string.IsNullOrWhiteSpace(productItemVm.Name))
            {
                productItem.Name = productItemVm.Name.Trim();
            }
            var getUrlFromName = TextHelper.ToUnsignString(productItem.Name.ToLower());

            var colorUrl = TextHelper.ToUnsignString(productItemVm.ColorName.ToLower());
            productItem.ColorUrl = colorUrl;

            if (string.IsNullOrWhiteSpace(productItemVm.Url) || productItemVm.Url.ToLower().Trim() == getUrlFromName)
                productItem.Url = getUrlFromName;

            if (!string.IsNullOrWhiteSpace(productItemVm.Url) && productItemVm.Url.ToLower().Trim() != getUrlFromName)
                productItem.Url = productItemVm.Url.ToLower().Trim();
            //productItem.Url = TextHelper.ToUnsignString(productItem.ColorName).ToLower();
            //while (HasExistsProductItemUrl(productItem.Url))
            //{
            //    var insertedUrl = TextHelper.ToUnsignString(productItem.ColorName).ToLower();
            //    productItem.Url = $"{insertedUrl}-{CommonHelper.GenerateExtensionOfUrl()}";
            //}
            _productItemRepository.Insert(productItem);
        }
        public void UpdateProductItem(ProductItemViewModel productItemVm)
        {
            var productItem = _productItemRepository.GetById(productItemVm.Id);
            var createdDate = productItem.DateCreated;

            var colorName = _productColorRepository.GetById(productItemVm.ProductColorId).Name;
            if (!string.IsNullOrWhiteSpace(colorName))
                productItemVm.ColorName = colorName.Trim();            

            var colorImage = _productColorRepository.GetById(productItemVm.ProductColorId).ColorImage;
            if (!string.IsNullOrWhiteSpace(colorImage))
                productItemVm.ColorImage = colorImage.Trim();

            Mapper.Map(productItemVm, productItem);

            var colorUrl = TextHelper.ToUnsignString(productItemVm.ColorName.ToLower());
            productItem.ColorUrl = colorUrl;

            var result = new DateTime(0001, 1, 1, 0, 0, 0);
            int compare = DateTime.Compare(createdDate, result);
            if(compare == 0)
            {
                createdDate = DateTime.Now;
            }
                      
            productItem.DateCreated = createdDate;

            var productCategoryName = _productCategoryRepository.GetById(productItemVm.ProductCategoryId).Name;
            if (!string.IsNullOrWhiteSpace(productCategoryName))
                productItem.ProductCategoryName = productCategoryName.Trim();
            var product = _productRepository.GetById(productItemVm.ProductId);

            if (!string.IsNullOrWhiteSpace(product.Name))
                productItem.ProductName = product.Name.Trim();

            if (!string.IsNullOrWhiteSpace(product.Code))
                productItem.ProductCode = product.Code.Trim();

            if (!string.IsNullOrWhiteSpace(productItemVm.Name))
            {
                productItem.Name = productItemVm.Name.Trim();
            }

            var getUrlFromName = TextHelper.ToUnsignString(productItem.Name.ToLower());

            //if (string.IsNullOrWhiteSpace(productItemVm.Url) || productItemVm.Url.ToLower().Trim() == getUrlFromName)
                productItem.Url = getUrlFromName;

            //if (!string.IsNullOrWhiteSpace(productItemVm.Url) && productItemVm.Url.ToLower().Trim() != getUrlFromName)
            //    productItem.Url = productItemVm.Url.ToLower().Trim();

            //productItem.Url = TextHelper.ToUnsignString(productItem.ColorName).ToLower();
            //while (HasExistsProductItemUrl(productItem.Url))
            //{
            //    var insertedUrl = TextHelper.ToUnsignString(productItem.ColorName).ToLower();
            //    productItem.Url = $"{insertedUrl}-{CommonHelper.GenerateExtensionOfUrl()}";
            //}
            _productItemRepository.Update(productItem);
        }
        public List<ColorViewModel> GetColors()
        {
            return _colorRepository.GetAll().OrderBy(x=>x.SortOrder).ProjectTo<ColorViewModel>().ToList();
        }
        //public ProductItemViewModel PrepareProductItemModel()
        //{
        //    var model = new ProductItemViewModel();
        //    var productCategories = _productCategoryRepository.GetAll().ProjectTo<ProductCategoryViewModel>().ToList();           
        //    foreach (var item in productCategories)
        //    {
        //        model.AvailableProductCategories.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
        //    };

        //    var products = _productRepository.GetAll().ProjectTo<ProductViewModel>().ToList();
        //    foreach (var item in products)
        //    {
        //        model.AvailableProducts.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
        //    };

        //    var colors = _colorRepository.GetAll().ProjectTo<ColorViewModel>().ToList();           
        //    foreach (var item in colors)
        //    {
        //        model.AvailableColors.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
        //    };
        //    return model;
        //}        

        public string GetColor(int colorId)
        {
            return _colorRepository.GetById(colorId).Code;
        }
        public List<ProductItemViewModel> GetProductItems()
        {
            var list = _productItemRepository.GetAll();
            var model = list.ProjectTo<ProductItemViewModel>().ToList();
            return model;
        }

        public List<ProductItemViewModel> GetProductItemsByProductId(Guid id)
        {
            return _productItemRepository.GetAllIncluding(x => x.ProductId == id).OrderBy(x=>x.SortOrder).ProjectTo<ProductItemViewModel>().ToList();
        }

        public List<ProductItemViewModel> GetProductItemsByProductIdAndColorUrl(Guid id, string color)
        {
            var list = new List<ProductItemViewModel>();
            var first = _productItemRepository.FirstOrDefault(x => x.Status == Status.Actived && x.ProductId == id && x.ColorUrl == color);
            var firstVm = Mapper.Map<ProductItem, ProductItemViewModel>(first);
            list.Add(firstVm);
            var temp = _productItemRepository.GetAllIncluding(x => x.ProductId == id && x.ColorUrl != color).OrderBy(x => x.SortOrder).ProjectTo<ProductItemViewModel>().ToList();
            foreach (var item in temp)
            {
                list.Add(item);
            }
            return list;
        }

        public List<ProductItemViewModel> GetItemsByProductId(Guid id)
        {
            var query = _productItemRepository.GetAllIncluding(x => x.ProductId == id).OrderBy(x => x.SortOrder).ProjectTo<ProductItemViewModel>().ToList();
            //var product = _productRepository.GetAllIncluding(x => x.Id == id);
            //var productCategory = _productCategoryRepository.GetAll();

            //var query = (from pi in productItemList
            //            join p in product
            //            on pi.ProductId equals p.Id
            //            join pc in productCategory
            //            on p.CategoryId equals pc.Id
            //            where pi.ProductId == id
            //            select new ProductItemViewModel()
            //            {
            //                 Id = pi.Id, Code = pi.Code, ColorImage = pi.ColorImage, ColorName = pi.ColorName, HomeFlag = pi.HomeFlag, HomeOrder = pi.HomeOrder, HotFlag = pi.HotFlag, HotOrder = pi.HotOrder,
            //                  IsGood = pi.IsGood, MetaDescription = pi.MetaDescription, MetaKeywords = pi.MetaKeywords,                      
            //                Content = pi.Content,
            //                 DateCreated = pi.DateCreated, DateModified = pi.DateModified,
            //                Image1 = pi.Image1, Image2 = pi.Image2, Image3 = pi.Image3, Image4 = pi.Image4, Image5 = pi.Image5, 
            //                Image1Alt = pi.Image1Alt,
            //                Image2Alt = pi.Image2Alt,
            //                Image3Alt = pi.Image3Alt,
            //                Image4Alt = pi.Image4Alt,
            //                Image5Alt = pi.Image5Alt,
            //                Price = pi.Price,
            //                PromotionPrice = pi.PromotionPrice,
            //                Name = pi.Name, Quantity = pi.Quantity, SortOrder = pi.SortOrder, Url = pi.Url, Status = pi.Status, ProductCategoryId = pi.ProductCategoryId, ProductId = pi.ProductId,
                              
            //            }).ToList();
            return query;
        }

        public List<Product> GetProductsByTagName(string tagName)
        {
            var list = _productRepository.GetAllIncluding(x => x.Tags.Contains(tagName)).ToList();
            return list;
        }
        public void CreateTagId(ref string tagId, ref List<string> listStr, ref List<int> listDuoi)
        {
            foreach (var item in listStr)
            {
                int idx = item.LastIndexOf('-');
                if (idx != -1)
                {
                    var duoi = item.Substring(idx + 1);
                    if (int.TryParse(item, out var number))
                    {
                        listDuoi.Add(number);
                    }
                }
            }
            listDuoi.Sort();

            if (listDuoi.Count() == 0)
                tagId = tagId + "-" + 1;

            if (listDuoi.Count() == 1)
            {
                if (listDuoi[0] <= 1)
                    tagId = tagId + "-" + (listDuoi[0] + 1);
                else
                    tagId = tagId + "-" + 1;
            }

            if (listDuoi.Count() == 2)
            {
                var rs = listDuoi[1] - listDuoi[0] > 1 ? listDuoi[0] + 1 : listDuoi[1] + 1;
                tagId = tagId + "-" + rs;
            }

            if (listDuoi.Count() > 2)
            {
                for (int i = 0; i < listDuoi.Count(); i++)
                {
                    if (i <= listDuoi.Count() - 2)
                    {
                        if (listDuoi[i + 1] - listDuoi[i] > 1)
                        {
                            tagId = tagId + "-" + (listDuoi[i] + 1);
                            break;
                        }
                    }
                    if (i == listDuoi.Count() - 1)
                    {
                        tagId = tagId + "-" + i;
                        break;
                    }
                }
            }
        }

        public void CreateTagAndProductTag(Product product, ProductViewModel productVm)
        {
            var createdDdate = product.DateCreated;

            Mapper.Map(productVm, product);
            if (string.IsNullOrWhiteSpace(productVm.Url))
            {
                product.Url = TextHelper.ToUnsignString(productVm.Name);
            }
            else
            {
                product.Url = productVm.Url.ToLower();
            }
            var query = _productRepository.FirstOrDefault(x => x.Url == product.Url);
            if (query != null)
            {
                product.Url = $"{query.Url}-{product.Code.ToLower()}";
            }
            product.DateCreated = createdDdate;

            if (!string.IsNullOrWhiteSpace(product.Tags))
            {
                string[] tags = product.Tags.Split(',');
                var filteredTagList = new List<string>();
                foreach (string t in tags)
                {
                    if (!string.IsNullOrWhiteSpace(t))
                    {
                        var tagId = TextHelper.ToUnsignString(t);

                        var listStr = new List<string>();
                        var listDuoi = new List<int>();
                        for (int i = 0; i < 15; i++)
                        {
                            var check = i == 0 ? _tagRepository.FirstOrDefault(x => x.Id == tagId) : _tagRepository.FirstOrDefault(x => x.Id == tagId + "-" + i + "");
                            if (check != null)
                            {
                                listStr.Add(check.Id);
                            }
                        }

                        if (listStr.Count() > 0)
                        {
                            CreateTagId(ref tagId, ref listStr, ref listDuoi);
                        }

                        if (!_tagRepository.GetAll().Where(x => x.Id == tagId).Any())
                        {
                            var tagOrder = SetNewTagOrder(out var lstOrder);
                            var tag = new Tag
                            {
                                Id = tagId,
                                Name = t.Trim(),
                                //Type = Data.Enums.TagType.Product
                                Type = CommonConstants.ProductTag,
                                MetaTitle = t.Trim(),
                                MetaDescription = t.Trim(),
                                MetaKeywords = t.Trim(),
                                SortOrder = tagOrder,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now
                            };
                            _tagRepository.Insert(tag);
                            _unitOfWork.Commit();


                            var productTag = new ProductTag
                            {
                                Id = Guid.NewGuid(),
                                ProductId = product.Id,
                                TagId = tagId
                            };

                            _productTagRepository.Insert(productTag);
                            _unitOfWork.Commit();
                            filteredTagList.Add(t.Trim());
                        }
                    }
                }
                var filteredTagsArray = filteredTagList.ToArray();
                var filteredTagStr = string.Join(",", filteredTagsArray);
                product.Tags = filteredTagStr;
            }
            _productRepository.Update(product);
        }

       

        public int SetNewTagOrder(out List<int> list)
        {
            var order = 0;
            //list = _tagRepository.GetAll().Select(x => x.SortOrder).ToList();
            list = Tags.OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }
       
        

        public List<TagViewModel> GetAllTags()
        {
            return _tagRepository.GetAll().ProjectTo<TagViewModel>().ToList();
        }

       
        // Product
        public override void Add(ProductViewModel productVm)
        {
            var product = Mapper.Map<ProductViewModel, Product>(productVm);
            if (!string.IsNullOrWhiteSpace(productVm.Name))
            {
                product.Name = productVm.Name.Trim();
            }
            var getUrlFromName = TextHelper.ToUnsignString(product.Name);

            if (string.IsNullOrWhiteSpace(productVm.Url) || productVm.Url.ToLower().Trim() == getUrlFromName)
                product.Url = getUrlFromName;

            if (!string.IsNullOrWhiteSpace(productVm.Url) && productVm.Url.ToLower().Trim() != getUrlFromName)
                product.Url = productVm.Url.ToLower().Trim();
                

            product.Tags = !string.IsNullOrWhiteSpace(product.Tags) ? product.Tags.ToLower().Trim() : string.Empty;
            product.Code = GenerateProductCode();
            var query = _productRepository.FirstOrDefault(x => x.Url == product.Url);
            if (query != null)
            {
                product.Url = $"{query.Url}-{product.Code.ToLower()}";
            }
            product.Url = product.Url.ToLower();
            product.Id = Guid.NewGuid();
            if (!string.IsNullOrWhiteSpace(product.Tags))
            {
                var tags = product.Tags.Split(',');
                var filteredTagList = new List<string>();
                foreach (string t in tags)
                {
                    if (!string.IsNullOrWhiteSpace(t))
                    {
                        var tagId = TextHelper.ToUnsignString(t);

                        var listStr = new List<string>();
                        for (int i = 0; i < 15; i++)
                        {
                            var check = i == 0 ? _tagRepository.FirstOrDefault(x => x.Id == tagId) : _tagRepository.FirstOrDefault(x => x.Id == tagId + "-" + i + "");
                            if (check != null)
                            {
                                var tagIdIsNotNull = check.Id;
                                var productTag = new ProductTag
                                {
                                    Id = Guid.NewGuid(),
                                    ProductId = product.Id,
                                    TagId = tagIdIsNotNull
                                };
                                _productTagRepository.Insert(productTag);
                                listStr.Add(tagIdIsNotNull);
                                filteredTagList.Add(t.Trim());
                                _unitOfWork.Commit();
                            }
                        }
                        if (listStr.Count() == 0)
                        {
                            var tagOrder = SetNewTagOrder(out var lstOrder);
                            var tag = new Tag
                            {
                                Id = tagId,
                                Name = t.Trim(),
                                //Type = Data.Enums.TagType.Product
                                Type = CommonConstants.ProductTag,
                                MetaTitle = t.Trim(),
                                MetaDescription = t.Trim(),
                                MetaKeywords = t.Trim(),
                                SortOrder = tagOrder,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now
                            };
                            _tagRepository.Insert(tag);

                            var productTag = new ProductTag
                            {
                                Id = Guid.NewGuid(),
                                ProductId = product.Id,
                                TagId = tagId
                            };
                            _productTagRepository.Insert(productTag);
                            filteredTagList.Add(t.Trim());

                            _unitOfWork.Commit();
                        }
                    }
                }
                var filteredTagsArray = filteredTagList.ToArray();
                var tagsStr = string.Join(",", filteredTagsArray);
                product.Tags = tagsStr;
            }
            _productRepository.Insert(product);
        }

        public void Update(ProductViewModel productVm, string oldTags)
        {
            var product = _productRepository.GetById(productVm.Id);
            var productTemp = Products.FirstOrDefault(x => x.Id == productVm.Id);            
            var createdDate = productTemp.DateCreated;
            Mapper.Map(productVm, product);
            product.DateCreated = createdDate;
            var productId = product.Id;
            product.Tags = !string.IsNullOrWhiteSpace(productVm.Tags) ? productVm.Tags.ToLower().Trim() : string.Empty;
            var newTags = product.Tags;
          
            if (string.IsNullOrWhiteSpace(productVm.Code))
            {
                productVm.Code = GenerateProductCode();
            }
            product.Code = productVm.Code;

            if (!string.IsNullOrWhiteSpace(productVm.Name))
            {
                product.Name = productVm.Name.Trim();
            }            

            var getUrlFromName = TextHelper.ToUnsignString(product.Name);

            if (string.IsNullOrWhiteSpace(productVm.Url) || productVm.Url.ToLower().Trim() == getUrlFromName)
                product.Url = getUrlFromName;

            if (!string.IsNullOrWhiteSpace(productVm.Url) && productVm.Url.ToLower().Trim() != getUrlFromName)
                product.Url = productVm.Url.ToLower().Trim();

            var query = _productRepository.GetAllIncluding(x => x.Url == product.Url).ToList();
            if (query.Count() > 1)
            {
                product.Url = $"{query[0].Url}-{product.Code.ToLower()}";
            }

            product.Url = product.Url.ToLower();
            string[] oldTagsArr = !string.IsNullOrWhiteSpace(oldTags) ? oldTags.Trim().Split(',') : new string[] { };
            string[] newTagsArr = !string.IsNullOrWhiteSpace(newTags) ? newTags.Split(",") : new string[] { };
            var lstTag = new List<string>();
            var newTagsStr = "";

            if (oldTagsArr.Length != 0)
            {
                foreach (var oldTag in oldTagsArr)
                {
                    if (!string.IsNullOrWhiteSpace(oldTag))
                    {
                        var oldTagId = TextHelper.ToUnsignString(oldTag);
                        var pt = _productTagRepository.FirstOrDefault(x => x.TagId == oldTagId);
                        if (pt != null)
                        {
                            _productTagRepository.Delete(pt);
                            _unitOfWork.Commit();
                        }
                    }
                }
            }

            if (newTagsArr.Length != 0)
            {
                foreach (var newTag in newTagsArr)
                {
                    if (!string.IsNullOrWhiteSpace(newTag))
                    {
                        var newTagId = TextHelper.ToUnsignString(newTag);
                        var isExistTag = false;
                        var listStr = new List<string>();
                        for (int i = 0; i < 15; i++)
                        {
                            var check = i == 0 ? _tagRepository.FirstOrDefault(x => x.Id == newTagId) : _tagRepository.FirstOrDefault(x => x.Id == newTagId + "-" + i + "");
                            if (check != null)
                            {
                                var tagIdIsNotNull = check.Id;
                                //listStr.Add(tagIdIsNotNull);
                                isExistTag = true;
                                var productTag = new ProductTag
                                {
                                    Id = Guid.NewGuid(),
                                    ProductId = product.Id,
                                    TagId = tagIdIsNotNull
                                };
                                _productTagRepository.Insert(productTag);
                                _unitOfWork.Commit();
                            }
                        }
                        //if (listStr.Count() == 0)
                        if (isExistTag == false)
                        {
                            var tagOrder = SetNewTagOrder(out var lstOrder);
                            var tag = new Tag
                            {
                                Id = newTagId,
                                Name = newTag.Trim(),
                                //Type = Data.Enums.TagType.Product
                                Type = CommonConstants.ProductTag,
                                MetaTitle = newTag.Trim(),
                                MetaDescription = newTag.Trim(),
                                MetaKeywords = newTag.Trim(),
                                SortOrder = tagOrder,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now
                            };
                            _tagRepository.Insert(tag);

                            var productTag = new ProductTag
                            {
                                Id = Guid.NewGuid(),
                                ProductId = product.Id,
                                TagId = newTagId
                            };
                            _productTagRepository.Insert(productTag);
                            _unitOfWork.Commit();
                        }
                        lstTag.Add(newTag);
                    }
                }
                newTagsStr = string.Join(",", lstTag);
                product.Tags = newTagsStr;               
            }
            _productRepository.Update(product);
        }

        public ProductViewModel GetByUrl(string url)
        {
            var pro = _productRepository.GetAllIncluding(x => x.Url.Equals(url)).FirstOrDefault();
            var vm = pro != null ? Mapper.Map<Product, ProductViewModel>(pro) : new ProductViewModel();
            return vm;
        }

        public ProductItemViewModel GetProductItemByProductId(string url, Guid productId)
        {           
           return _productItemRepository.GetAllIncluding(x => x.Url == url && x.ProductId == productId).ProjectTo<ProductItemViewModel>().FirstOrDefault();
        }

        public ProductItemViewModel GetProductItemByProductId(string url, Guid productId, string getByProductIdAndColorUrl)
        {
            return _productItemRepository.GetAllIncluding(x => x.ColorUrl == url && x.ProductId == productId).ProjectTo<ProductItemViewModel>().FirstOrDefault();
        }

        //public ProductItemViewModel GetProductItemByUrl(string url)
        //{
        //    var pro = _productRepository.GetAllIncluding(x => x.Url.Equals(url)).FirstOrDefault();
        //    var productItem = _productItemRepository.Get
        //    var vm = new ProductItemViewModel();
        //    if (pro != null) {
        //        vm = new ProductItemViewModel()
        //        {
        //            Code = pro.Code, ColorCode = pro.ColorCode, ColorEnglishName = pro.ColorEnglishName, ColorId = pro.ColorId, ColorImage = pro.ColorImage, ColorName = pro.ColorName,
        //            Content = pro.Content, DateCreated = pro.DateCreated, DateModified = pro.DateModified, Id = pro.Id,
        //            Image1 = pro.Image1, Image1Alt = pro.Image1Alt, Image2 = pro.Image2, Image2Alt = pro.Image2Alt, Image3 = pro.Image3, Image3Alt = pro.Image3Alt, Image4 = pro.Image4, Image4Alt = pro.Image4Alt,
        //            Image5 = pro.Image5, Image5Alt = pro.Image5Alt, ProductCategoryId = pro.ProductCategoryId, MetaTitle = pro.MetaTitle, MetaDescription = pro.MetaDescription, MetaKeywords = pro.MetaKeywords,
        //            Name = pro.Name,  
        //        };
        //    }                           
        //    return vm;
        //}

        public override ProductViewModel GetById(Guid id)
        {
            //var query = (from pc in _productCategoryRepository.GetAll()
            //             join p in _productRepository.GetAll()
            //                 on pc.Id equals p.CategoryId
            //             where pc.Id == id && p.CategoryId == id
            //             select new { p });

            var product = (from pcat in _productRepository.GetAll() where pcat.Id == id select new { pcat }).FirstOrDefault();

            var categoryName = _productCategoryRepository.GetAllIncluding(x => x.Id == product.pcat.CategoryId).Select(x => x.Name).FirstOrDefault();

            //var productlist = query.Select(x => new PostViewModel()
            //{
            //    Name = x.p.Name,
            //    //Id = x.p.Id,
            //    CategoryId = x.p.CategoryId,
            //    Url = x.p.Url,
            //    Description = x.p.Description,
            //    Image = x.p.Image,
            //    Content = x.p.Content,
            //    Viewed = x.p.Viewed,
            //    Tags = x.p.Tags,
            //    //Unit = x.p.Unit,
            //    HomeFlag = x.p.HomeFlag,
            //    HotFlag = x.p.HotFlag,
            //    //Quantity = x.p.Quantity,

            //    Status = x.p.Status,
            //    MetaTitle = x.p.MetaTitle,
            //    MetaDescription = x.p.MetaDescription,
            //    MetaKeywords = x.p.MetaKeywords
            //}).ToList();

            var model = new ProductViewModel()
            {
                Name = product.pcat.Name,
                Code = product.pcat.Code,
                Id = product.pcat.Id,
                CategoryId = product.pcat.CategoryId,
                CategoryName = categoryName,
                Url = product.pcat.Url,
                Tags = product.pcat.Tags,
                Description = product.pcat.Description,
                Content = product.pcat.Content,
                Image = product.pcat.Image,
                MetaTitle = product.pcat.MetaTitle,
                MetaDescription = product.pcat.MetaDescription,
                MetaKeywords = product.pcat.MetaKeywords,
                HotOrder = product.pcat.HotOrder,
                HotFlag = product.pcat.HotFlag,
                HomeOrder = product.pcat.HomeOrder,
                HomeFlag = product.pcat.HomeFlag,
                SortOrder = product.pcat.SortOrder,
                DateCreated = product.pcat.DateCreated,
                DateModified = product.pcat.DateModified,
                Price = product.pcat.Price,
                PromotionPrice = product.pcat.PromotionPrice,
                OriginalPrice = product.pcat.OriginalPrice,
                Quantity = product.pcat.Quantity,
                //Products = productlist,
                Status = product.pcat.Status
            };

            return model;
        }
        #region Save

        //public override ProductViewModel GetById(Guid id)
        //{
        //    var query = (from p in _productRepository.GetAll()
        //                 join pt in _productTagRepository.GetAll() on p.Id equals pt.ProductId
        //                 join t in _tagRepository.GetAll() on pt.TagId equals t.Id
        //                 where p.Id == id && pt.ProductId == id
        //                 select new { p, pt, t }); // join product, producttag, tag
        //    var product = (from pro in _productRepository.GetAll() where pro.Id == id select new { pro }).First();
        //    string[] tags = product.pro.Tags.Split(',');
        //    var tagId = "";
        //    foreach (string tagitem in tags)
        //    {
        //        tagId = TextHelper.ToUnsignString(tagitem);
        //        var productTag = _productTagRepository.FirstOrDefault(v => v.TagId == tagId && v.ProductId == product.pro.Id);
        //        Mapper.Map<Tag, TagViewModel>(_tagRepository.GetById(tagId)); // tao moi chu ko sua
        //        Mapper.Map<ProductTag, ProductTagViewModel>(_productTagRepository.GetById(productTag.Id)); // tao moi chu ko sua
        //    }
        //    var pageAlias = TextHelper.ToUnsignString(product.pro.Name);
        //    var model = new ProductViewModel() // tao them 1 cai product, ko sua
        //    {
        //        Name = product.pro.Name,
        //        PageAlias = pageAlias,
        //        CategoryId = product.pro.CategoryId,
        //        PageTitle = product.pro.PageTitle,
        //        MetaDescription = product.pro.MetaDescription,
        //        Tags = product.pro.Tags,
        //        Price = product.pro.Price,
        //        OriginalPrice = product.pro.OriginalPrice,
        //        PromotionPrice = product.pro.PromotionPrice,
        //        UpdatedDate = product.pro.UpdatedDate,
        //        Status = product.pro.Status
        //    };
        //    return model;
        //}

        //public override List<ProductViewModel> GetAll()
        //{
        //    var query = (from pc in _productCategoryRepository.GetAll()
        //                 join p in _productRepository.GetAll()
        //                     on pc.Id equals p.CategoryId
        //                 select new { p, pc });

        //    var model = query.OrderByDescending(x => x.p.CreatedDate)
        //        .Select(x => new ProductViewModel()
        //        {
        //            Name = x.p.Name,
        //            Id = x.p.Id,
        //            CategoryId = x.p.CategoryId,
        //            CategoryName = x.pc.Name,
        //            Price = x.p.Price,
        //            ThumbnailImage = x.p.ThumbnailImage,
        //            DateCreated = x.p.DateCreated,
        //            Status = x.p.Status
        //        }).ToList();

        //    return model;
        //}

        //public override void Delete(Guid id)
        //{
        //    _productRepository.Delete(id);
        //}

        //public override ProductViewModel GetById(Guid id)
        //{
        //    return Mapper.Map<Product, ProductViewModel>(_productRepository.GetById(id));
        //}

        //public override List<ProductViewModel> GetAll()
        //{
        //    return _productRepository.GetAll().OrderBy(c => c.ProductTags)
        //    return _productRepository.FindAll(c => c.ProductCategory, c => c.ProductTags)
        //        .ProjectTo<ProductViewModel>().ToList();
        //}

        //public PagedResult<ProductViewModel> GetAllPaging(string keyword, int pageSize, int page = 1)
        //{
        //    var query = _productRepository.FindAll();
        //    if (!string.IsNullOrEmpty(keyword))
        //        query = query.Where(x => x.Name.Contains(keyword));
        //    int totalRow = query.Count();
        //    var data = query.OrderByDescending(x => x.CreatedDate)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize);
        //    var paginationSet = new PagedResult<ProductViewModel>()
        //    {
        //        Results = data.ProjectTo<ProductViewModel>().ToList(),
        //        CurrentPage = page,
        //        RowCount = totalRow,
        //        PageSize = pageSize,
        //    };
        //    return paginationSet;
        //}

        #endregion Save

        public string GetColorName(string color)
        {
            return _productItemRepository.FirstOrDefault(x => x.Url == color).ColorName.ToLower();            
        }

        // PagedResult
        public PagedResult<ProductUnitViewModel> GetAllProductsPaging(int categoryId, string keyword, int page, int pageSize, string sortBy)
        {
            var query = _productRepository.GetAllIncluding(c => c.Status == Status.Actived && c.CategoryId == categoryId);


            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword) || x.Code.Contains(keyword));

            //if (categoryId != null)
            //    query = query.where(x => x.CategoryId == categoryId);

            int totalRow = query.Count();
            switch (sortBy)
            {
                case "gia":
                    query = query.OrderByDescending(x => x.Price);
                    break;

                case "ten":
                    query = query.OrderBy(x => x.Name);
                    break;

                case "moi-nhat":
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;

                default:
                    query = query.OrderByDescending(x => x.SortOrder);
                    break;
            }
            query = query.Skip((page - 1) * pageSize)
                .Take(pageSize);


            var list = new List<ProductUnitViewModel>();
            foreach (var item in query)
            {
                var productId = item.Id;
                var productItems = GetItemsByProductId(item.Id);
                var productCartVm = new ProductUnitViewModel()
                {
                    ProductName = item.Name,
                    ProductUrl = item.Url,
                    ProductItems = productItems
                };
                list.Add(productCartVm);
            }

            var paginationSet = new PagedResult<ProductUnitViewModel>()
            {
                Results = list,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }
        public PagedResult<ProductUnitViewModel> GetAllProductsPaging(int categoryId, int page, int pageSize, string sortBy, string listProductItemByCategory)
        {
            var query = _productRepository.GetAllIncluding(c => c.Status == Status.Actived && c.CategoryId == categoryId);           

            //if (categoryId != null)
            //    query = query.where(x => x.CategoryId == categoryId);

            int totalRow = query.Count();
            switch (sortBy)
            {
                case "gia":
                    query = query.OrderByDescending(x => x.Price);
                    break;

                case "ten":
                    query = query.OrderBy(x => x.Name);
                    break;

                case "moi-nhat":
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;

                default:
                    query = query.OrderBy(x => x.SortOrder);
                    break;
            }
            query = query.Skip((page - 1) * pageSize)
                .Take(pageSize);


            var list = new List<ProductUnitViewModel>();
            foreach (var item in query)
            {
                var productId = item.Id;               
                var productItems = GetItemsByProductId(item.Id);
                var productCartVm = new ProductUnitViewModel()
                {                   
                    ProductName = item.Name,
                    ProductUrl = item.Url,
                    ProductItems = productItems
                };
                list.Add(productCartVm);
            }
            
            var paginationSet = new PagedResult<ProductUnitViewModel>()
            {
                Results = list,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }
        public PagedResult<ProductUnitViewModel> GetAllProductsPaging(string url, int? categoryId, int page, int pageSize, string sortBy)
        {            
            var query = from product in _productRepository.GetAllIncluding(x=>x.Status == Status.Actived)
                    join productTag in _productTagRepository.GetAll()
                    on product.Id equals productTag.ProductId
                    where productTag.TagId == url
                    select new { product, productTag };
            //var productItems = from productItem in _productItemRepository.GetAll()
            //        join product_tag in productUnit
            //        on productItem.ProductId equals product_tag.product.Id
            //        where productItem.ProductId == product_tag.product.Id
            //        orderby productItem.SortOrder
            //            select new { productItem, product_tag };
            //var product_Item = productItems.FirstOrDefault();

            //var query = from p in _productRepository.GetAll()
            //            join pt in _productTagRepository.GetAll()
            //            on p.Id equals pt.ProductId
            //            join pi in _productItemRepository.GetAll()
            //            on p.Id equals pi.ProductId
            //            where pt.TagId == url && p.Id == pt.ProductId && p.Id == pi.ProductId
            //            select new { p, pt, pi };           

            if (categoryId.HasValue && categoryId != 0)
                query = query.Where(x => x.product.CategoryId == categoryId);

            int totalRow = query.Count();
            switch (sortBy)
            {
                case "gia":
                    query = query.OrderByDescending(x => x.product.Price);
                    break;

                case "ten":
                    query = query.OrderBy(x => x.product.Name);
                    break;

                case "moi-nhat":
                    query = query.OrderByDescending(x => x.product.DateCreated);
                    break;

                default:
                    query = query.OrderByDescending(x => x.product.SortOrder);
                    break;
            }
            query = query.Skip((page - 1) * pageSize)
                .Take(pageSize);

            var list = new List<ProductUnitViewModel>();
            foreach (var item in query)
            {
                var getProductItems = GetProductItemsByProductId(item.product.Id);
                var productItem = getProductItems.FirstOrDefault();

                list.Add(new ProductUnitViewModel {
                    ProductName = item.product.Name,
                    ProductUrl = item.product.Url,
                    ProductItems = getProductItems                     
                });
            }
            //var list = query.ToList();

            var paginationSet = new PagedResult<ProductUnitViewModel>()
            {
                Results = list,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }        
        public PagedResult<ProductSingleViewModel> GetAllProductsPaging(string color, string keyword, int page, int pageSize, string sortBy)
        {
            //var productItems = !string.IsNullOrWhiteSpace(color)
            //            ? _productItemRepository.GetAllIncluding(x => x.Status == Status.Actived && x.ColorUrl == color)
            //            : _productItemRepository.GetAllIncluding(x => x.Status == Status.Actived);

            var query = !string.IsNullOrWhiteSpace(color)
                        ? from pi in _productItemRepository.GetAllIncluding(x => x.Status == Status.Actived && x.ColorUrl == color)
                          join p in _productRepository.GetAll()
                          on pi.ProductId equals p.Id
                          where pi.ProductId == p.Id
                          select new ProductSingleViewModel
                          {
                              Position = p.Tags,
                              ProductItem = Mapper.Map<ProductItem, ProductItemViewModel>(pi),
                              ProductUrl = p.Url
                          }
                        : from pi in _productItemRepository.GetAllIncluding(x => x.Status == Status.Actived)
                          join p in _productRepository.GetAll()
                          on pi.ProductId equals p.Id
                          where pi.ProductId == p.Id
                          //select new { pi, p };
                          select new ProductSingleViewModel
                          {
                              Position = p.Tags,
                              ProductItem = Mapper.Map<ProductItem, ProductItemViewModel>(pi),
                              ProductUrl = p.Url
                          };
          
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.ProductItem.Name.Contains(keyword) || x.ProductItem.Code.Contains(keyword));

            //if (!string.IsNullOrEmpty(position))
            //    query = query.Where(x => x.Position.Contains(position));

            int totalRow = query.Count();
            switch (sortBy)
            {
                case "gia":
                    query = query.OrderByDescending(x => x.ProductItem.Price);
                    break;

                case "ten":
                    query = query.OrderBy(x => x.ProductItem.Name);
                    break;

                case "moi-nhat":
                    query = query.OrderByDescending(x => x.ProductItem.DateCreated);
                    break;

                default:
                    query = query.OrderByDescending(x => x.ProductItem.SortOrder);
                    break;
            }
            query = query.Skip((page - 1) * pageSize)
                .Take(pageSize);
           
            //var getByProductIdAndColorUrl = "get by Product Id and Color Url";
            //var list = new List<ProductSingleViewModel>();
            //foreach (var item in query)
            //{
            //    var productItem = GetProductItemByProductId(color, item.ProductItem.ProductId, getByProductIdAndColorUrl);

            //    list.Add(new ProductSingleViewModel
            //    {
            //        ProductUrl = _productRepository.GetById(item.p.Id).Url,
            //        ProductItem = productItem
            //    });
            //}

            //if (!string.IsNullOrEmpty(keyword))
            //    query = query.Where(x => x.pi.Name.Contains(keyword) || x.pi.Code.Contains(keyword));

            //if (!string.IsNullOrEmpty(position))
            //    query = query.Where(x => x.p.Tags.Contains(position));

            //int totalRow = query.Count();
            //switch (sortBy)
            //{
            //    case "gia":
            //        query = query.OrderByDescending(x => x.pi.Price);
            //        break;

            //    case "ten":
            //        query = query.OrderBy(x => x.pi.Name);
            //        break;

            //    case "moi-nhat":
            //        query = query.OrderByDescending(x => x.pi.DateCreated);
            //        break;

            //    default:
            //        query = query.OrderByDescending(x => x.pi.SortOrder);
            //        break;
            //}
            //query = query.Skip((page - 1) * pageSize)
            //    .Take(pageSize);

            var list = query.ToList();
           
            //var getByProductIdAndColorUrl = "get by Product Id and Color Url";
            //var list = new List<ProductSingleViewModel>();
            //foreach (var item in query)
            //{
            //    var productItem = GetProductItemByProductId(color, item.p.Id, getByProductIdAndColorUrl);

            //    list.Add(new ProductSingleViewModel
            //    {
            //        ProductUrl = _productRepository.GetById(item.p.Id).Url,
            //        ProductItem = productItem
            //    });
            //}

            var paginationSet = new PagedResult<ProductSingleViewModel>()
            {
                Results = list,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

       
        public PagedResult<ProductViewModel> GetAllPaging(int categoryId, string keyword, int page, int pageSize, string sortBy)
        {
            var query = _productRepository.GetAll();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword) || x.Code.Contains(keyword));

            //if (categoryId != null)
            //    query = query.where(x => x.CategoryId == categoryId);

            int totalRow = query.Count();
            switch (sortBy)
            {
                case "gia":
                    query = query.OrderByDescending(x => x.Price);
                    break;

                case "ten":
                    query = query.OrderBy(x => x.Name);
                    break;

                case "moi-nhat":
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }
            query = query.Skip((page - 1) * pageSize)
                .Take(pageSize);

            //var list = query.ProjectTo<ProductViewModel>().ToList();
            var list = new List<ProductViewModel>();
            foreach (var item in query)
            {
                var productVm = new ProductViewModel()
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    Code = item.Code,
                    Url = item.Url,
                    Name = item.Name,
                    Content = item.Content,
                    Description = item.Description,
                    MetaTitle = item.MetaTitle,
                    MetaDescription = item.MetaDescription,
                    MetaKeywords = item.MetaKeywords,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    DateDeleted = item.DateDeleted,
                    HomeFlag = item.HomeFlag,
                    HomeOrder = item.HomeOrder,
                    HotFlag = item.HotFlag,
                    HotOrder = item.HotOrder,
                    Status = item.Status,
                    SortOrder = item.SortOrder,
                    Image = item.Image,
                    Tags = item.Tags,
                    Viewed = item.Viewed,
                    Price = item.Price,
                    PromotionPrice = item.PromotionPrice,
                    OriginalPrice = item.OriginalPrice,
                    Quantity = item.Quantity
                };
                var productCategory = _productCategoryRepository.GetById(productVm.CategoryId);

                productVm.CategoryName = productCategory == null
                    ? "Danh mục cấp 1"
                    : productCategory.Name;
                list.Add(productVm);
            }
            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = list,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public PagedResult<ProductItemViewModel> GetAllProductItemsPaging(int productCategoryId, Guid productId, int productColorId, string keyword, int page, int pageSize, string sortBy)
        {
            try
            {
                var query = _productItemRepository.GetAll();                

                if (productCategoryId != 0)
                    query = query.Where(x => x.ProductCategoryId == productCategoryId);

                if (productId.ToString() != CommonConstants.DefaultGuid)
                    query = query.Where(x => x.ProductId == productId);

                if (productColorId != 0)
                    query = query.Where(x => x.ProductColorId == productColorId);

                if (!string.IsNullOrEmpty(keyword))
                    query = query.Where(x => x.Name == keyword || x.Code == keyword);

                switch (sortBy)
                {
                    case "thu-tu-giam-dan":
                        query = query.OrderByDescending(x => x.SortOrder);
                        break;
                    case "thu-tu-tang-dan":
                        query = query.OrderBy(x => x.SortOrder);
                        break;
                    case "ngay-tao-giam-dan":
                        query = query.OrderByDescending(x => x.DateCreated);
                        break;
                    case "ngay-tao-tang-dan":
                        query = query.OrderBy(x => x.DateCreated);
                        break;
                    case "ngay-cap-nhat-giam-dan":
                        query = query.OrderByDescending(x => x.DateModified);
                        break;
                    case "ngay-cap-nhat-tang-dan":
                        query = query.OrderBy(x => x.DateModified);
                        break;
                    case "so-luong-giam-dan":
                        query = query.OrderByDescending(x => x.Quantity);
                        break;
                    case "so-luong-tang-dan":
                        query = query.OrderBy(x => x.Quantity);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.SortOrder);
                        break;
                }
                
                int totalRow = query.Count();
              
                query = query.Skip((page - 1) * pageSize)
                    .Take(pageSize);

                var list = query.ProjectTo<ProductItemViewModel>().ToList();
             
                var paginationSet = new PagedResult<ProductItemViewModel>()
                {
                    Results = list,
                    CurrentPage = page,
                    RowCount = totalRow,
                    PageSize = pageSize
                };

                return paginationSet;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public PagedResult<ProductViewModel> GetAllPaging(int categoryId, string keyword, int page, int pageSize)
        {
            var query = _productRepository.GetAll().Where(c => c.Status == Status.Actived);

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword) || x.Code.Contains(keyword));

            //if (categoryId != null)
            //    query = query.where(x => x.CategoryId == categoryId);

            int totalRow = query.Count();
            //switch (sortBy)
            //{
            //    case "price":
            //        query = query.OrderByDescending(x => x.Price);
            //        break;

            //    case "name":
            //        query = query.OrderBy(x => x.Name);
            //        break;

            //    case "lastest":
            //        query = query.OrderByDescending(x => x.DateCreated);
            //        break;

            //    default:
            //        query = query.OrderByDescending(x => x.DateCreated);
            //        break;
            //}
            query = query.OrderByDescending(x => x.DateCreated);
            query = query.Skip((page - 1) * pageSize)
                .Take(pageSize);

            //var list = query.ProjectTo<ProductViewModel>().ToList();
            var list = new List<ProductViewModel>();
            foreach (var item in query)
            {
                var productVm = new ProductViewModel()
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    Code = item.Code,
                    Url = item.Url,
                    Name = item.Name,
                    Content = item.Content,
                    Description = item.Description,
                    MetaTitle = item.MetaTitle,
                    MetaDescription = item.MetaDescription,
                    MetaKeywords = item.MetaKeywords,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    DateDeleted = item.DateDeleted,
                    HomeFlag = item.HomeFlag,
                    HomeOrder = item.HomeOrder,
                    HotFlag = item.HotFlag,
                    HotOrder = item.HotOrder,
                    Status = item.Status,
                    SortOrder = item.SortOrder,
                    Image = item.Image,
                    Tags = item.Tags,
                    Viewed = item.Viewed,
                    Price = item.Price,
                    PromotionPrice = item.PromotionPrice,
                    OriginalPrice = item.OriginalPrice,
                    Quantity = item.Quantity
                };
                var productCategory = _productCategoryRepository.GetById(productVm.CategoryId);

                productVm.CategoryName = productCategory == null
                    ? "Danh mục cấp 1"
                    : productCategory.Name;
                list.Add(productVm);
            }
            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = list,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }


        public PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize, string sortBy)
        {
            var query = _productRepository.GetAll().Where(c => c.Status == Status.Actived);

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword) || x.Code.Contains(keyword));

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            //if (categoryId != null)
            //    query = query.where(x => x.CategoryId == categoryId);

            int totalRow = query.Count();
            switch (sortBy)
            {
                case "gia":
                    query = query.OrderByDescending(x => x.Price);
                    break;

                case "ten":
                    query = query.OrderBy(x => x.Name);
                    break;

                case "moi-nhat":
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }
            query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            //var list = query.ProjectTo<ProductViewModel>().ToList();
            var list = new List<ProductViewModel>();
            foreach (var item in query)
            {
                var productVm = new ProductViewModel()
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    Code = item.Code,
                    Url = item.Url,
                    Name = item.Name,
                    Content = item.Content,
                    Description = item.Description,
                    MetaTitle = item.MetaTitle,
                    MetaDescription = item.MetaDescription,
                    MetaKeywords = item.MetaKeywords,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    DateDeleted = item.DateDeleted,
                    HomeFlag = item.HomeFlag,
                    HomeOrder = item.HomeOrder,
                    HotFlag = item.HotFlag,
                    HotOrder = item.HotOrder,
                    Status = item.Status,
                    SortOrder = item.SortOrder,
                    Image = item.Image,
                    Tags = item.Tags,
                    Viewed = item.Viewed,
                    Price = item.Price,
                    PromotionPrice = item.PromotionPrice,
                    OriginalPrice = item.OriginalPrice,
                    Quantity = item.Quantity
                };
                var productCategory = _productCategoryRepository.GetById(productVm.CategoryId);

                productVm.CategoryName = productCategory == null
                    ? "Danh mục cấp 1"
                    : productCategory.Name;
                list.Add(productVm);
            }
            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = list,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }


        public PagedResult<ProductViewModel> GetMyWishlist(Guid userId, int page, int pageSize)
        {
            var query = _productWishlistRepository.GetAll().Where(c => c.UserId == userId);

            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize)
                .Take(pageSize);

            var data = query.ProjectTo<ProductViewModel>().ToList();
            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        // List
        public List<ProductViewModel> GetListPaging(int page, int pageSize, string sort, out int totalRow)
        {
            var query = _productRepository.GetAll().Where(x => x.Status == Status.Actived);

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.Viewed);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ProductViewModel>().ToList();
        }

        public List<ProductViewModel> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _productRepository.GetAll().Where(x => x.Status == Status.Actived && x.CategoryId == categoryId);

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.Viewed);
                    break;

                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;

                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ProductViewModel>().ToList();
        }

        public List<ProductViewModel> GetListProductByCategoryId(int categoryId)
        {
            var query = _productRepository.GetAll()
                .Where(x => x.Status == Status.Actived && x.CategoryId == categoryId)
                .OrderBy(x=>x.SortOrder)
                .ProjectTo<ProductViewModel>()
                .ToList();
            return query;
        }
        public List<ProductViewModel> GetListProduct(string keyword)
        {
            IQueryable<ProductViewModel> query;
            if (!string.IsNullOrEmpty(keyword))
                query = _productRepository.GetAll().Where(x => x.Name.Contains(keyword)).ProjectTo<ProductViewModel>();
            else
                query = _productRepository.GetAll().ProjectTo<ProductViewModel>();
            return query.ToList();
        }

        public List<ProductViewModel> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow)
        {
            var query = from p in _productRepository.GetAll()
                        join pt in _productTagRepository.GetAll()
                        on p.Id equals pt.ProductId
                        where pt.TagId == tagId
                        select p;
            totalRow = query.Count();

            var model = query.OrderByDescending(x => x.DateCreated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ProjectTo<ProductViewModel>();
            return model.ToList();
        }

        public List<ProductSingleViewModel> GetLastestProducts(int top)
        {
            var query = from pi in _productItemRepository.GetAll()
                        .Where(x => x.Status == Status.Actived)
                        .OrderByDescending(x => x.DateCreated)
                        .Take(top)
                        join p in _productRepository.GetAll()
                        on pi.ProductId equals p.Id
                        where pi.ProductId == p.Id
                        select new ProductSingleViewModel
                        {
                             ProductItem = Mapper.Map<ProductItem, ProductItemViewModel>(pi),
                              ProductUrl = p.Url
                        };
            var list = query.ToList();
            return list;
        }

        public List<ProductSingleViewModel> GetNicestProducts(int top)
        {
            var query = from pi in _productItemRepository.GetAll()
                        .Where(x => x.Status == Status.Actived && x.IsGood == Status.Actived)
                        .OrderByDescending(x => x.DateCreated)
                        .Take(top)
                        join p in _productRepository.GetAll()
                        on pi.ProductId equals p.Id
                        where pi.ProductId == p.Id
                        select new ProductSingleViewModel
                        {
                            ProductItem = Mapper.Map<ProductItem, ProductItemViewModel>(pi),
                            ProductUrl = p.Url
                        };
            var list = query.ToList();
            return list;
        }

        public List<ProductSingleViewModel> GetHotProducts(int top)
        {
            var query = from pi in _productItemRepository.GetAll()
                        .Where(x => x.Status == Status.Actived && x.HotFlag == Status.Actived)
                        .OrderByDescending(x => x.HotOrder)
                        .Take(top)
                        join p in _productRepository.GetAll()
                        on pi.ProductId equals p.Id
                        where pi.ProductId == p.Id
                        select new ProductSingleViewModel
                        {
                            ProductItem = Mapper.Map<ProductItem, ProductItemViewModel>(pi),
                            ProductUrl = p.Url
                        };
            var list = query.ToList();
            return list;
        }

        public List<ProductSingleViewModel> GetItemsHaveSpecialPrice(int top)
        {
            var query = from pi in _productItemRepository.GetAll()
                        .Where(x => x.Status == Status.Actived && x.SpecialPrice > 0)
                        .OrderBy(x => x.Price)
                        .Take(top)
                        join p in _productRepository.GetAll()
                        on pi.ProductId equals p.Id
                        where pi.ProductId == p.Id
                        select new ProductSingleViewModel
                        {
                            ProductItem = Mapper.Map<ProductItem, ProductItemViewModel>(pi),
                            ProductUrl = p.Url
                        };
            var list = query.ToList();
            return list;
        }

        public List<ProductViewModel> GetReatedProducts(Guid id, int top)
        {
            var product = _productRepository.GetById(id);
            return _productRepository.GetAll().Where(x => x.Status == Status.Actived
                && x.Id != id && x.CategoryId == product.CategoryId)
            .OrderByDescending(x => x.DateCreated)
            .Take(top)
            .ProjectTo<ProductViewModel>()
            .ToList();
        }

        public List<ProductUnitViewModel> GetReatedProducts(int productCategoryId, ProductViewModel product)
        {
            var products = _productRepository.GetAllIncluding(x => x.CategoryId == productCategoryId && x.Id != product.Id && x.Status == Status.Actived).OrderBy(x=>x.SortOrder);
            var result = new List<ProductUnitViewModel>();

            foreach (var item in products)
            {                                                                          
                var productItemList = _productItemRepository.GetAllIncluding(x => x.ProductId == item.Id).OrderBy(x => x.SortOrder).ProjectTo<ProductItemViewModel>().ToList();               
                var productItemUnit = new ProductUnitViewModel() {
                    ProductUrl = item.Url, ProductItems = productItemList
                };
                result.Add(productItemUnit);
            }
            return result;         
        }

        public List<ProductViewModel> GetUpsellProducts(int top)
        {
            return _productRepository.GetAll()
                .Where(x => x.PromotionPrice != null)
                .OrderByDescending(x => x.DateModified)
                .Take(top)
                .ProjectTo<ProductViewModel>().ToList();
        }

        public List<ProductUnitViewModel> GetUpsellProductItems(int top)
        {
            var productItem = _productItemRepository.GetAll();
            var product = _productRepository.GetAll();
            var query = from pi in productItem
                        join p in product
                        on pi.ProductId equals p.Id
                        where p.PromotionPrice != null
                        orderby pi.DateModified descending
                        select new ProductUnitViewModel
                        {

                        };
            var result = query.Take(top)
                .ProjectTo<ProductUnitViewModel>().ToList();
            return result;
        }
        public List<ProductViewModel> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _productRepository.GetAll().Where(x => x.Status == Status.Actived && x.Name.Contains(keyword));

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.Viewed);
                    break;

                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;

                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ProductViewModel>()
                .ToList();
        }

        // Tag
        public List<TagViewModel> GetListTagByProductId(Guid id)
        {
            //var tags = _tagRepository.FindAll();
            //var productTags = _productTagRepository.FindAll();

            //var query = from t in tags
            //            join pt in productTags
            //            on t.Id equals pt.TagId
            //            where pt.ProductId == productId
            //            select new TagViewModel()
            //            {
            //                Id = t.Id,
            //                Name = t.Name
            //            };
            //return query.ToList();
            return _productTagRepository.GetAll().Where(x => x.ProductId == id)
                //.Select(y => y.Tag)
                .ProjectTo<TagViewModel>()
                .ToList();
        }

        public TagViewModel GetTagByUrl(string url)
        {
            var tag = _tagRepository.GetById(url);
            return Mapper.Map<Tag, TagViewModel>(tag);
        }
        public List<TagViewModel> GetListProductTag(string searchText)
        {
            return _tagRepository.GetAll().Where(x => x.Type == CommonConstants.ProductTag
            && searchText.Contains(x.Name)).ProjectTo<TagViewModel>().ToList();
        }

        public TagViewModel GetTag(string tagId)
        {
            return Mapper.Map<Tag, TagViewModel>(_tagRepository.Single(x => x.Id == tagId));
        }

        // Images
        public List<ProductImageViewModel> GetImages(Guid productId)
        {
            return _productImageRepository.GetAll().Where(x => x.ProductId == productId)
                .ProjectTo<ProductImageViewModel>().ToList();
        }

        public void AddImages(Guid productId, string[] images)
        {
            _productImageRepository.Delete(x => x.ProductId == productId);
            int order = 1;
            foreach (var image in images)
            {
                _productImageRepository.Insert(new ProductImage()
                {
                    Path = image,
                    ProductId = productId,
                    Caption = string.Empty,
                    SortOrder = order
                });
                order++;
            }
        }

        // Excel
        public void ImportExcel(string filePath, int categoryId)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];                
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    decimal.TryParse(workSheet.Cells[i, 3].Value.ToString(), out var originalPrice);
                    decimal.TryParse(workSheet.Cells[i, 4].Value.ToString(), out var price);
                    decimal.TryParse(workSheet.Cells[i, 5].Value.ToString(), out var promotionPrice);
                    bool.TryParse(workSheet.Cells[i, 9].Value.ToString(), out var hotFlag);
                    bool.TryParse(workSheet.Cells[i, 10].Value.ToString(), out var homeFlag);
                    Product product = new Product {
                    CategoryId = categoryId,
                    Name = workSheet.Cells[i, 1].Value.ToString(),
                    Description = workSheet.Cells[i, 2].Value.ToString(),
                    OriginalPrice = originalPrice,
                    PromotionPrice = promotionPrice,
                    Content = workSheet.Cells[i, 6].Value.ToString(),
                    MetaKeywords = workSheet.Cells[i, 7].Value.ToString(),
                    MetaDescription = workSheet.Cells[i, 8].Value.ToString(),
                    Price = price,
                    HotFlag = hotFlag == true ? Status.Actived : Status.InActived,                    
                    HomeFlag = homeFlag == true ? Status.Actived : Status.InActived,
                    Status = Status.Actived,
                };                   
                    _productRepository.Insert(product);
                }
            }
        }

        // WholePrice
        public List<WholePriceViewModel> GetWholePrices(Guid productId)
        {
            return _wholePriceRepository.GetAll().Where(x => x.ProductId == productId).ProjectTo<WholePriceViewModel>().ToList();
        }

        public void AddWholePrice(Guid productId, List<WholePriceViewModel> wholePrices)
        {
            _wholePriceRepository.Delete(x => x.ProductId == productId);
            //_wholePriceRepository.RemoveMultiple(_wholePriceRepository.FindAll(x => x.ProductId == productId).ToList());
            foreach (var wholePrice in wholePrices)
            {
                _wholePriceRepository.Insert(new WholePrice()
                {
                    ProductId = productId,
                    FromQuantity = wholePrice.FromQuantity,
                    ToQuantity = wholePrice.ToQuantity,
                    Price = wholePrice.Price
                });
            }
        }

        // Quantity
        public void AddQuantity(Guid productId, List<ProductQuantityViewModel> quantities)
        {
            //_productQuantityRepository.RemoveMultiple(_productQuantityRepository.FindAll(x => x.ProductId == productId).ToList());
            _productQuantityRepository.Delete(x => x.ProductId == productId);
            foreach (var quantity in quantities)
            {
                _productQuantityRepository.Insert(new ProductQuantity()
                {
                    ProductId = productId,
                    ColorId = quantity.ColorId,
                    //SizeId = quantity.SizeId,
                    Quantity = quantity.Quantity
                });
            }
        }

        public List<ProductQuantityViewModel> GetQuantities(Guid productId)
        {
            //return _productQuantityRepository.FindAll(x => x.ProductId == productId).ProjectTo<ProductQuantityViewModel>().ToList();
            return _productQuantityRepository.GetAll().Where(x => x.ProductId == productId).ProjectTo<ProductQuantityViewModel>().ToList();
        }

        // Others
        public List<string> GetListProductByName(string name)
        {
            return _productRepository.GetAll().Where(x => x.Status == Status.Actived && x.Name.Contains(name)).Select(y => y.Name).ToList();
        }

        public void IncreaseView(Guid id)
        {
            var product = _productRepository.GetById(id);
        }

        public IList<string> GetIds(int categoryId)
        {
            var list = _productRepository.GetAllIncluding(x => x.CategoryId == categoryId).Select(x => x.Id);
            var lstIds = new List<string>();
            foreach (var item in list)
            {
                lstIds.Add(item.ToString());
            }
            return lstIds;
        }

        public void DeleteProductItem(int id)
        {
            _productItemRepository.Delete(id);
        }
        public void MultiDelete(IList<string> selectedIds)
        {
            foreach (var item in selectedIds)
            {
                var product = Products.Where(r => r.Id == Guid.Parse(item)).FirstOrDefault();
                Products.Remove(product);
                var productId = product.Id;
                var lstProductTag = _productTagRepository.GetAllIncluding(x => x.ProductId == productId).ToList();
                ProductTags.RemoveRange(lstProductTag);
            }
            _unitOfWork.Commit();
        }

        public void MultiDeleteProductItem(IList<string> selectedIds)
        {            
            foreach (var item in selectedIds)
            {                
                var list = _productItemRepository.GetAllIncluding(x => x.Id == int.Parse(item)).ToList();
                _productItemRepository.DeleteMultiple(list);
            }
            //_unitOfWork.Commit();
        }


        private string GenerateProductCode()
        {
            var code = CommonHelper.GenerateRandomCode();
            while (HasExistsProductCode(code))
            {
                code = CommonHelper.GenerateRandomCode();
            }
            return code;
        }
        private string GenerateProductItemCode()
        {
            var code = $"{CommonHelper.GenerateRandomNumberCode()}";
            while (HasExistsProductItemCode(code))
            {
                code = $"{CommonHelper.GenerateRandomNumberCode()}";
            }
            return code;
        }

        /// <summary>
        /// Check exist code
        /// </summary>
        /// <param name="postCode"></param>
        /// <returns></returns>
        public virtual bool HasExistsProductCode(string code)
        {
            return _productRepository.GetAll().Any(x => x.Code == code);
        }

        public virtual bool HasExistsProductItemCode(string code)
        {
            return _productItemRepository.GetAll().Any(x => x.Code == code);
        }

        public virtual bool HasExistsProductItemUrl(string url)
        {
            return _productItemRepository.GetAll().Any(x => x.Url == url);
        }
    }
}