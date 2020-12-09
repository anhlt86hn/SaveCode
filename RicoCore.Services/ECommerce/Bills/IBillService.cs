using RicoCore.Data.Entities;
using RicoCore.Data.Enums;
using RicoCore.Services.ECommerce.Bills.Dtos;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Utilities.Dtos;
using System;
using System.Collections.Generic;

namespace RicoCore.Services.ECommerce.Bills
{
    public interface IBillService : IWebServiceBase<Bill, Guid, BillViewModel>
    {
        bool HasExistsCode(string code);
        PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword,
            int pageIndex, int pageSize);
        BillViewModel GetDetail(Guid billId);
        void UpdateStatus(Guid orderId, BillStatus status);


        List<ColorViewModel> GetColors();

        List<SizeViewModel> GetSizes();
        ColorViewModel GetColor(int id);

        SizeViewModel GetSize(Guid id);

        List<BillDetailViewModel> GetBillDetails(Guid billId);

        BillDetailViewModel AddDetail(BillDetailViewModel billDetailVm);
        BillDetailViewModel UpdateDetail(BillDetailViewModel billDetailVm);
        void DeleteDetail(int productItemId, Guid billId);
        Guid Add(BillViewModel billVm, out Guid billId);
        void Update(BillViewModel billVm, List<BillDetailViewModel> BillDetails);
    }
}