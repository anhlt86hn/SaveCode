var HomeController = function () {
    this.initialize = function () {
        loadData();
    //registerEvents();
    jQuery('#jtv-rev_slider').show().revolution({
                dottedOverlay: 'none',
                delay: 5000,
                startwidth: 850,
                startheight: 450,
                hideThumbs: 200,
                thumbWidth: 200,
                thumbHeight: 50,
                thumbAmount: 2,
                navigationType: 'thumb',
                navigationArrows: 'solo',
                navigationStyle: 'round',
                touchenabled: 'on',
                onHoverStop: 'on',
                swipe_velocity: 0.7,
                swipe_min_touches: 1,
                swipe_max_touches: 1,
                drag_block_vertical: false,
                spinner: 'spinner0',
                keyboardNavigation: 'off',
                navigationHAlign: 'center',
                navigationVAlign: 'bottom',
                navigationHOffset: 0,
                navigationVOffset: 20,
                soloArrowLeftHalign: 'left',
                soloArrowLeftValign: 'center',
                soloArrowLeftHOffset: 20,
                soloArrowLeftVOffset: 0,
                soloArrowRightHalign: 'right',
                soloArrowRightValign: 'center',
                soloArrowRightHOffset: 20,
                soloArrowRightVOffset: 0,
                shadow: 0,
                fullWidth: 'on',
                fullScreen: 'off',
                stopLoop: 'off',
                stopAfterLoops: -1,
                stopAtSlide: -1,
                shuffle: 'off',
                autoHeight: 'off',
                forceFullWidth: 'on',
                fullScreenAlignForce: 'off',
                minFullScreenHeight: 0,
                hideNavDelayOnMobile: 1500,
                hideThumbsOnMobile: 'off',
                hideBulletsOnMobile: 'off',
                hideArrowsOnMobile: 'off',
                hideThumbsUnderResolution: 0,
                hideSliderAtLimit: 0,
                hideCaptionAtLimit: 0,
                hideAllCaptionAtLilmit: 0,
                startWithSlide: 0,
                fullScreenOffsetContainer: ''
            });        
    }

    //function registerEvents() {
    //    $('.product-image').on('mouseenter', function (e) {
    //        e.preventDefault();
    //        var hidId = $(this).parent().find('.hid-id');
    //        var hidUrl = $(this).parent().find('.hid-url');
    //        var id = hidId.val();
    //        var url = hidUrl.val();
    //        var img = $(this).find('img').first();
    //        $.ajax({
    //            type: "POST",
    //            url: "/Product/GetByUrl",
    //            data: {
    //                id: id,
    //                url: url
    //            },
    //            dataType: "json",
    //            beforeSend: function () {
    //                aspnetcore.startLoading();
    //            },
    //            success: function (response) {
    //                var data = response;
    //                img.attr({
    //                    'alt': data.image2Alt,
    //                    'src': data.image2
    //                });
    //            },
    //            error: function () {
    //                aspnetcore.notify('Có loi xay ra', 'error');
    //                aspnetcore.stopLoading();
    //            }
    //        }).then(function () {
    //            hidId.val(id);
    //            hidUrl.val(url);
    //        });
    //    });

    //    $('.product-image').on('mouseleave', function (e) {
    //        e.preventDefault();
    //        var hidId = $(this).parent().find('.hid-id');
    //        var hidUrl = $(this).parent().find('.hid-url');
    //        var id = hidId.val();
    //        var url = hidUrl.val();
    //        var img = $(this).find('img').first();
    //        $.ajax({
    //            type: "POST",
    //            url: "/Product/GetByUrl",
    //            data: {
    //                id: id,
    //                url: url
    //            },
    //            dataType: "json",
    //            beforeSend: function () {
    //                aspnetcore.startLoading();
    //            },
    //            success: function (response) {
    //                var data = response;
    //                img.attr({
    //                    'alt': data.image1Alt,
    //                    'src': data.image1
    //                });
    //            },
    //            error: function () {
    //                aspnetcore.notify('Có loi xay ra', 'error');
    //                aspnetcore.stopLoading();
    //            }
    //        }).then(function () {
    //            hidId.val(id);
    //            hidUrl.val(url);
    //        });
    //    });

    //    $('.add-to-cart').on('click', function (e) {
    //        e.preventDefault();
    //        var id = $(this).data('id');            
    //        $.ajax({
    //            url: '/Cart/AddProductItemToCart',
    //            type: 'post',
    //            dataType: 'json',
    //            data: {
    //                productItem: id,
    //                quantity: 1,                   
    //            },
    //            success: function () {
    //                aspnetcore.notify('Sản phẩm đã được thêm vào giỏ hàng thành công', 'success');
    //                loadHeaderCart();                   
    //            }
    //        });
    //    });
    //}    

    function loadData() {
        $.ajax({
            Url: "http://localhost:63452/api/ket-qua-truyen-thong",
            Type: 'GET',
            data: {
                page: 1,
                pageSize: 2
            },
            dataType: "json",
            success: function (response) {
                alert("Successfully Get data");
            }
        });
    }

}