var BaseController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {

        $('body').on('click', '.add-to-cart', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $.ajax({
                url: '/Cart/AddProductItemToCart',
                type: 'post',
                data: {
                    productItemId: id,
                    quantity: 1,
                    //color: 0,
                    //size: 0
                },
                success: function (response) {
                    aspnetcore.notify('Sản phẩm đã được thêm vào giỏ hàng', 'success');
                    loadHeaderCart();
                    //loadMyCart();
                }
            });
        });

        $('body').on('click', '.clear-all', function (e) {
            e.preventDefault();
            $.ajax({
                url: '/Cart/ClearCart',
                type: 'post',
                success: function () {
                    aspnetcore.notify('Xoá giỏ hàng thành công', 'success');
                    loadHeaderCart();
                    loadData();
                }
            });
        });

        $('body').on('click', '.remove-cart', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $.ajax({
                url: '/Cart/RemoveFromCart',
                type: 'post',
                data: {
                    productItemId: id
                },
                success: function (response) {
                    aspnetcore.notify('Sản phẩm đã được xóa khỏi giỏ hàng', 'success');
                    //aspnetcore.notify(resources["RemoveCartOK"], 'success');
                    loadHeaderCart();
                    loadData();
                    //loadMyCart();
                }
            });
        });       

        $('#AddDetailToCart').on('click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var quantity = parseInt($('#txtQuantity').val());
            $.ajax({
                url: '/Cart/AddProductItemToCart',
                type: 'post',
                dataType: 'json',
                data: {
                    productItemId: id,
                    quantity: quantity,
                    //color: colorId,
                    //size: sizeId
                },
                success: function () {
                    aspnetcore.notify('Sản phẩm đã được thêm vào giỏ hàng thành công', 'success');
                    loadHeaderCart();
                    //alert(1);
                }
            });
        });  

        $('body').on('click', '.items-count', function () {
            var quantity = 0;
            var temp = parseInt($('#txtQuantity').val());
            if ($(this).hasClass('increase'))
                quantity = temp + 1;

            if ($(this).hasClass('reduced'))
                quantity = temp - 1;

            $('#txtQuantity').val(quantity);            
        });

        $('.product-image').on('mouseenter', function (e) {
            e.preventDefault();
            var hidId = $(this).parent().find('.hid-id');
            var hidUrl = $(this).parent().find('.hid-url');
            var id = hidId.val();
            var url = hidUrl.val();
            var img = $(this).find('img').first();
            $.ajax({
                type: "POST",
                url: "/Product/GetByUrl",
                data: {
                    id: id,
                    url: url
                },
                dataType: "json",
                beforeSend: function () {
                    aspnetcore.startLoading();
                },
                success: function (response) {
                    var data = response;
                    img.attr({
                        'alt': data.image2Alt,
                        'src': data.image2
                    });
                },
                error: function () {
                    aspnetcore.notify('Có loi xay ra', 'error');
                    aspnetcore.stopLoading();
                }
            }).then(function () {
                hidId.val(id);
                hidUrl.val(url);
            });
        });

        $('.product-image').on('mouseleave', function (e) {
            e.preventDefault();
            var hidId = $(this).parent().find('.hid-id');
            var hidUrl = $(this).parent().find('.hid-url');
            var id = hidId.val();
            var url = hidUrl.val();
            var img = $(this).find('img').first();
            $.ajax({
                type: "POST",
                url: "/Product/GetByUrl",
                data: {
                    id: id,
                    url: url
                },
                dataType: "json",
                beforeSend: function () {
                    aspnetcore.startLoading();
                },
                success: function (response) {
                    var data = response;
                    img.attr({
                        'alt': data.image1Alt,
                        'src': data.image1
                    });
                },
                error: function () {
                    aspnetcore.notify('Có loi xay ra', 'error');
                    aspnetcore.stopLoading();
                }
            }).then(function () {
                hidId.val(id);
                hidUrl.val(url);
            });
        });
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");
    }

    //function loadMyCart() {
    //    $("#sidebarCart").load("/AjaxContent/MyCart");
    //}

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