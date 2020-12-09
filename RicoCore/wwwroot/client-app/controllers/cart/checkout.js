var CartController = function () {
    var cachedObj = {
        colors: []
    }
    this.initialize = function () {
        
        //$.when(loadColors(),
        //    loadSizes())
        //    .then(function () {
        //        loadData();
        //    });
       
        registerEvents();        
    }

    function registerEvents() {

        $('body').on('click', '.items-count', function () {
            var quantity = 0;
            var temp = parseInt($('#txtQuantity').val());
            if ($(this).hasClass('increase'))
                quantity = temp + 1;

            if ($(this).hasClass('reduced'))
                quantity = temp - 1;

            $('#txtQuantity').val(quantity);

            var productItemPrice = $(this).parent().parent().find('.product-item-price span').text().replace(',', '');
            var price = parseInt(productItemPrice);
            var total = quantity * price;
            var showTotal = $(this).parent().parent().find('.product-item-total span').first().text(aspnetcore.formatNumber(total, 0));
            $('.product-item-total span').text(showTotal);
        });

        //$('body').on('keyup', '.txtQuantity', function (e) {
        //    e.preventDefault();
        //    var id = $(this).data('id');
        //    var q = $(this).val();
        //    if (q > 0) {
        //        $.ajax({
        //            url: '/Cart/UpdateCart',
        //            type: 'post',
        //            data: {
        //                productItemId: id,
        //                quantity: q
        //            },
        //            success: function () {
        //                aspnetcore.notify('Cập nhật số lượng sản phẩm thành công', 'success');
        //                loadHeaderCart();
        //                loadData();
        //            }
        //        });
        //    } else {
        //        aspnetcore.notify('Số lượng sản phẩm không hợp lệ', 'error');
        //    }

        //});


        //    $('body').on('click', '.btn-delete', function (e) {
        //        e.preventDefault();
        //        var id = $(this).data('id');
        //        $.ajax({
        //            url: '/Cart/RemoveFromCart',
        //            type: 'post',
        //            data: {
        //                productItemId: id
        //            },
        //            success: function () {
        //                aspnetcore.notify('Xoá sản phẩm thành công', 'success');
        //                loadHeaderCart();
        //                loadData();
        //            }
        //        });
        //    });
        //    $('body').on('keyup', '.txtQuantity', function (e) {
        //        e.preventDefault();
        //        var id = $(this).data('id');
        //        var q = $(this).val();
        //        if (q > 0) {
        //            $.ajax({
        //                url: '/Cart/UpdateCart',
        //                type: 'post',
        //                data: {
        //                    productItemId: id,
        //                    quantity: q
        //                },
        //                success: function () {
        //                    aspnetcore.notify('Cập nhật số lượng sản phẩm thành công', 'success');
        //                    loadHeaderCart();
        //                    loadData();
        //                }
        //            });
        //        } else {
        //            aspnetcore.notify('Số lượng sản phẩm không hợp lệ', 'error');
        //        }

        //    });

        //    $('body').on('change', '.ddlColorId', function (e) {
        //        e.preventDefault();
        //        var id = parseInt($(this).closest('tr').data('id'));
        //        var colorId = $(this).val();
        //        var q = $(this).closest('tr').find('.txtQuantity').first().val();            

        //        if (q > 0) {
        //            $.ajax({
        //                url: '/Cart/UpdateCart',
        //                type: 'post',
        //                data: {
        //                    productId: id,
        //                    quantity: q,
        //                    color: colorId,
        //                    //size: sizeId
        //                },
        //                success: function () {
        //                    aspnetcore.notify('Update quantity is successful', 'success');
        //                    loadHeaderCart();
        //                    loadData();
        //                }
        //            });
        //        } else {
        //            aspnetcore.notify('Your quantity is invalid', 'error');
        //        }

        //    });

        //    $('body').on('change', '.ddlSizeId', function (e) {
        //        e.preventDefault();
        //        var id = parseInt($(this).closest('tr').data('id'));
        //        var sizeId = $(this).val();
        //        var q = parseInt($(this).closest('tr').find('.txtQuantity').first().val());
        //        var colorId = parseInt($(this).closest('tr').find('.ddlColorId').first().val());
        //        if (q > 0) {
        //            $.ajax({
        //                url: '/Cart/UpdateCart',
        //                type: 'post',
        //                data: {
        //                    productId: id,
        //                    quantity: q,
        //                    color: colorId,
        //                    size: sizeId
        //                },
        //                success: function () {
        //                    aspnetcore.notify('Update quantity is successful', 'success');
        //                    loadHeaderCart();
        //                    loadData();
        //                }
        //            });
        //        } else {
        //            aspnetcore.notify('Your quantity is invalid', 'error');
        //        }

        //    });

        //    $('#btnClearAll').on('click', function (e) {
        //        e.preventDefault();
        //        $.ajax({
        //            url: '/Cart/ClearCart',
        //            type: 'post',
        //            success: function () {
        //                aspnetcore.notify('Xoá giỏ hàng thành công', 'success');
        //                loadHeaderCart();
        //                loadData();
        //            }
        //        });
        //    });
        //}

        //function loadColors() {
        //    return $.ajax({
        //        type: "GET",
        //        url: "/Cart/GetColors",
        //        dataType: "json",
        //        success: function (response) {
        //            cachedObj.colors = response;
        //        },
        //        error: function () {
        //            aspnetcore.notify('Has an error in progress', 'error');
        //        }
        //    });
        //}

        //function loadSizes() {
        //    return $.ajax({
        //        type: "GET",
        //        url: "/Cart/GetSizes",
        //        dataType: "json",
        //        success: function (response) {
        //            cachedObj.sizes = response;
        //        },
        //        error: function () {
        //            aspnetcore.notify('Has an error in progress', 'error');
        //        }
        //    });
        //}
        //function getColorOptions(selectedId) {
        //    var colors = "<select class='form-control ddlColorId'><option value='0'></option>";
        //    $.each(cachedObj.colors, function (i, color) {
        //        if (selectedId === color.Id)
        //            colors += '<option value="' + color.Id + '" selected="select">' + color.Name + '</option>';
        //        else
        //            colors += '<option value="' + color.Id + '">' + color.Name + '</option>';
        //    });
        //    colors += "</select>";
        //    return colors;
        //}

        //function getSizeOptions(selectedId) {
        //    var sizes = "<select class='form-control ddlSizeId'>";
        //    $.each(cachedObj.sizes, function (i, size) {
        //        if (selectedId === size.Id)
        //            sizes += '<option value="' + size.Id + '" selected="select">' + size.Name + '</option>';
        //        else
        //            sizes += '<option value="' + size.Id + '">' + size.Name + '</option>';
        //    });
        //    sizes += "</select>";
        //    return sizes;
        //}

        //function loadHeaderCart() {
        //    $("#headerCart").load("/AjaxContent/HeaderCart");
        //}

        //function loadData() {
        //    $.ajax({
        //        url: '/Cart/GetCart',
        //        type: 'GET',
        //        dataType: 'json',
        //        success: function (response) {
        //            var template = $('#template-cart').html();
        //            var render = "";
        //            var totalAmount = 0;
        //            $.each(response, function (i, item) {
        //                render += Mustache.render(template,
        //                    {
        //                        ProductItemId: item.ProductItem.Id,
        //                        ProductItemName: item.ProductItem.Name,
        //                        Image: item.ProductItem.Image1,
        //                        Price: aspnetcore.formatNumber(item.Price, 0),
        //                        Quantity: item.Quantity,
        //                        Code: item.ProductItem.Code,
        //                        ColorName: item.Color,
        //                        ColorImage: item.ProductItem.ColorImage,
        //                        //Colors: getColorOptions(item.Color == null ? 0 : item.Color.Id),
        //                        //Sizes: getSizeOptions(item.Size == null ? "" : item.Size.Id),
        //                        Amount: aspnetcore.formatNumber(item.Price * item.Quantity, 0),
        //                        Url: '/' + item.ProductUrl + '/mau-rem=' + item.ProductItem.Url
        //                    });
        //                totalAmount += item.Price * item.Quantity;
        //            });
        //            $('#lblTotalAmount').text(aspnetcore.formatNumber(totalAmount, 0));
        //            if (render !== "")
        //                $('#table-cart-content').html(render);
        //            else
        //                $('#table-cart-content').html('Bạn chưa chọn sản phẩm nào');
        //        }
        //    });
        //    return false;
        //}
    }


    function loadData() {
        $.ajax({
            url: '/Cart/GetCart',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var template = $('#template-cart').html();
                var render = "";
                var totalAmount = 0;
                $.each(response, function (i, item) {
                    render += Mustache.render(template,
                        {
                            ProductItemId: item.ProductItem.Id,
                            ProductItemName: item.ProductItem.Name,
                            Image: item.ProductItem.Image1,
                            Price: aspnetcore.formatNumber(item.Price, 0),
                            Quantity: item.Quantity,
                            Code: item.ProductItem.Code,
                            ColorName: item.Color,
                            ColorImage: item.ProductItem.ColorImage,
                            //Colors: getColorOptions(item.Color == null ? 0 : item.Color.Id),
                            //Sizes: getSizeOptions(item.Size == null ? "" : item.Size.Id),
                            Amount: aspnetcore.formatNumber(item.Price * item.Quantity, 0),
                            Url: '/' + item.ProductUrl + '/mau-rem=' + item.ProductItem.Url
                        });
                    totalAmount += item.Price * item.Quantity;
                });
                $('#lblTotalAmount').text(aspnetcore.formatNumber(totalAmount, 0));
                if (render !== "")
                    $('#table-cart-content').html(render);
                else
                    $('#table-cart-content').html('Bạn chưa chọn sản phẩm nào');
            }
        });
        return false;
    }
}