using AutoMapper;
using AutoMapper.QueryableExtensions;
using RicoCore.Data.Entities;
using RicoCore.Data.Entities.ECommerce;
using RicoCore.Data.Enums;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Infrastructure.SharedKernel;
using RicoCore.Services.ECommerce.Bills.Dtos;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Utilities.Constants;
using RicoCore.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RicoCore.Services.ECommerce.Bills
{
    public class BillService : WebServiceBase<Bill, Guid, BillViewModel>, IBillService
    {
        private readonly IRepository<Bill, Guid> _orderRepository;
        private readonly IRepository<BillDetail, Guid> _orderDetailRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<ProductItem, int> _productItemRepository;
        private readonly IRepository<Color, int> _colorRepository;
        private readonly IRepository<Size, Guid> _sizeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BillService(IRepository<Bill, Guid> orderRepository,
            IRepository<BillDetail, Guid> orderDetailRepository,            
            IRepository<Product, Guid> productRepository,
            IRepository<ProductItem, int> productItemRepository,
            IRepository<Color, int> colorRepository,
            IRepository<Size, Guid> sizeRepository,
            IUnitOfWork unitOfWork) : base(orderRepository, unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _productRepository = productRepository;
            _productItemRepository = productItemRepository;
            _unitOfWork = unitOfWork;
        }

        public virtual bool HasExistsCode(string code)
        {
            return _orderRepository.GetAll().Any(x => x.Code == code);
        }

        private string GenerateCode()
        {
            var code = CommonHelper.GenerateRandomCode();
            while (HasExistsCode(code))
            {
                code = CommonHelper.GenerateRandomCode();
            }
            return code;
        }


        public Guid Add(BillViewModel billVm, out Guid billId)
        {
            var order = Mapper.Map<BillViewModel, Bill>(billVm);
            //var orderDetails = Mapper.Map<List<BillDetailViewModel>, List<BillDetail>>(billVm.BillDetails);
            //foreach (var detail in orderDetails)
            //{
            //    var product = _productRepository.GetById(detail.ProductId);
            //    detail.Price = product.Price;
            //}
            var code = GenerateCode();
            if (string.IsNullOrWhiteSpace(code))
            {
                do
                {
                    code = CommonMethods.GenerateCode();
                } while (!HasExistsCode(code));                
            }
            order.Code = code;
            //order.BillDetails = orderDetails;
            _orderRepository.Insert(order);
            billId = order.Id;
            return billId;
        }
        public BillDetailViewModel UpdateDetail(BillDetailViewModel billDetailVm)
        {
            var billDetail = Mapper.Map<BillDetailViewModel, BillDetail>(billDetailVm);
            _orderDetailRepository.Update(billDetail);
            return billDetailVm;
        }
        public void Update(BillViewModel billVm, List<BillDetailViewModel> BillDetails)
        {
            //Mapping to order domain
            var order = _orderRepository.GetById(billVm.Id);

            if(BillDetails.Count > 0)
            {
                //Get order Detail
                var newDetails = Mapper.Map<List<BillDetailViewModel>, List<BillDetail>>(BillDetails);

                // new details added
                var addedDetails = newDetails.Where(x => x.Id == Guid.Parse(CommonConstants.DefaultGuid)).ToList();
                foreach (var item in addedDetails)
                {
                    _orderDetailRepository.Insert(item);
                }

                // get updated details
                var updatedDetails = newDetails.Where(x => x.Id != Guid.Parse(CommonConstants.DefaultGuid)).ToList();
                foreach (var item in updatedDetails)
                {
                    _orderDetailRepository.Update(item);
                }

                //Existed details
                var existedDetails = _orderDetailRepository.GetAllIncluding(x => x.BillId == billVm.Id);

                _orderDetailRepository.DeleteMultiple(existedDetails.Except(updatedDetails).ToList());
            }           

            if (order.BillStatus != BillStatus.Completed && billVm.BillStatus == BillStatus.Completed)
            {
                ConfirmBill(order.Id);
            }
            if (order.BillStatus != BillStatus.Cancelled && billVm.BillStatus == BillStatus.Cancelled)
            {
                CancelBill(order.Id);
            }
            if (string.IsNullOrWhiteSpace(billVm.Code))
            {
                order.Code = GenerateCode();
            }
            order.CustomerName = billVm.CustomerName;
            order.CustomerAddress = billVm.CustomerAddress;
            order.CustomerFacebook = billVm.CustomerFacebook;
            order.CustomerMessage = billVm.CustomerMessage;
            order.CustomerMobile = billVm.CustomerMobile;
            order.ShipName = billVm.ShipName;
            order.ShipAddress = billVm.ShipAddress;                     
            order.ShipMobile = billVm.ShipMobile;
            order.BillStatus = billVm.BillStatus;
            order.PaymentMethod = billVm.PaymentMethod;
            order.ShippingFee = billVm.ShippingFee;

            _orderRepository.Update(order);           
        }

        public void UpdateStatus(Guid billId, BillStatus status)
        {
            var order = _orderRepository.GetById(billId);
            order.BillStatus = status;
            _orderRepository.Update(order);
        }

        public void ConfirmBill(Guid id)
        {
            var bill = _orderRepository.GetById(id);
            var billDetails = _orderDetailRepository.GetAll().Where(x => x.BillId == id);
            if (bill.BillStatus != BillStatus.Completed)
            {
                bill.BillStatus = BillStatus.Completed;
                foreach (var detail in billDetails)
                {
                    var product = _productItemRepository.GetById(detail.ProductItemId);
                    if (product.Quantity >= detail.Quantity)
                    {
                        product.Quantity -= detail.Quantity;
                    }
                    else
                        throw new Exception($"Sản phẩm {product.Name}-{product.Code} không đủ số lượng. Hiện tại chỉ còn {product.Quantity} trong kho.");
                }
            }
            else
            {
                throw new Exception("Đơn hàng này đã được xác nhận trước đó.");
            }
        }

        public void CancelBill(Guid id)
        {
            var bill = _orderRepository.GetById(id);
            var billDetails = _orderDetailRepository.GetAll().Where(x => x.BillId == id);
            if (bill.BillStatus != BillStatus.Cancelled)
            {
                bill.BillStatus = BillStatus.Cancelled;
                foreach (var detail in billDetails)
                {
                    var product = _productItemRepository.GetById(detail.ProductItemId);
                    product.Quantity += detail.Quantity;
                }
            }
            else
            {
                throw new Exception("Đơn này đã huỷ trước đó rồi.");
            }
        }

        public void PendingBill(Guid id)
        {
            var bill = _orderRepository.GetById(id);
            if (bill.BillStatus != BillStatus.Pending)
            {
                bill.BillStatus = BillStatus.Pending;
            }
            else
            {
                throw new Exception("Đơn hàng này đã bị hoãn trước đó.");
            }
        }
        public List<SizeViewModel> GetSizes()
        {
            return _sizeRepository.GetAll().ProjectTo<SizeViewModel>().ToList();
        }

        public PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword
            , int pageIndex, int pageSize)
        {
            var query = _orderRepository.GetAll();
            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated >= start);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated <= end);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.CustomerName.Contains(keyword) || x.CustomerMobile.Contains(keyword));
            }
            var totalRow = query.Count();
            var data = query.OrderByDescending(x => x.DateCreated)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<BillViewModel>()
                .ToList();
            return new PagedResult<BillViewModel>()
            {
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Results = data,
                RowCount = totalRow
            };
        }

        public BillViewModel GetDetail(Guid billId)
        {
            var bill = _orderRepository.SingleIncluding(x => x.Id == billId);
            var billVm = Mapper.Map<Bill, BillViewModel>(bill);
            //var billDetailVm = _orderDetailRepository.GetAllIncluding(x => x.BillId == billId).ProjectTo<BillDetailViewModel>().ToList();
            var billDetailVm = (from o in _orderDetailRepository.GetAllIncluding(x => x.BillId == billId)
                               join pi in _productItemRepository.GetAll()
                               on o.ProductItemId equals pi.Id
                               where o.ProductItemId == pi.Id
                               select new BillDetailViewModel
                               {
                                    Id = o.Id,
                                     BillId = billId,
                                      Code = pi.Code,
                                      ProductItemName = pi.Name,
                                       Quantity = o.Quantity,
                                       Price = o.Price,
                                        Color = o.Color, 
                                         ProductItemId = pi.Id
                               }).ToList();

            billVm.BillDetails = billDetailVm;
            return billVm;
        }

        public List<BillDetailViewModel> GetBillDetails(Guid billId)
        {
            return _orderDetailRepository
                .GetAllIncluding(x => x.BillId == billId)
                .ProjectTo<BillDetailViewModel>().ToList();
        }

        public List<ColorViewModel> GetColors()
        {
            return _colorRepository.GetAll().ProjectTo<ColorViewModel>().ToList();
        }

        public BillDetailViewModel AddDetail(BillDetailViewModel billDetailVm)
        {
            var billDetail = Mapper.Map<BillDetailViewModel, BillDetail>(billDetailVm);
            _orderDetailRepository.Insert(billDetail);
            return billDetailVm;
        }

        public void DeleteDetail(int productItemId, Guid billId)
        {
            var detail = _orderDetailRepository.SingleIncluding(x => x.ProductItemId == productItemId
           && x.BillId == billId);
            _orderDetailRepository.Delete(detail);
        }

        public ColorViewModel GetColor(int id)
        {
            return Mapper.Map<Color, ColorViewModel>(_colorRepository.GetById(id));
        }

        public SizeViewModel GetSize(Guid id)
        {
            return Mapper.Map<Size, SizeViewModel>(_sizeRepository.GetById(id));
        }
    }
}