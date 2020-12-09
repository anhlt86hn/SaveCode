var ProductDetailController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {

        //$('.pick-color').on('click', function (e) {
        //    e.preventDefault();
            //var id = parseInt($(this).data('id'));
            //var bigImg = $('.image-detail');
            //var linkImg1 = $('#mini-img-1');
            //var img1 = linkImg1.find('img');
            //var linkImg2 = $('#mini-img-2');
            //var img2 = linkImg2.find('img');
            //var linkImg3 = $('#mini-img-3');
            //var img3 = linkImg3.find('img');
            //var linkImg4 = $('#mini-img-4');
            //var img4 = linkImg4.find('img');
            //var linkImg5 = $('#mini-img-5');
            //var img5 = linkImg5.find('img');
            //var breadcrumbName = $('.breadcrumbs').find('li:last-child strong');
            
            //var namelink = $('.product-name a');
            //var name = $('.product-name h1');
            //var code = $('.info-other').find('span');
            //var content = $('.short-description p');
            //$(this).parent().siblings().find('.pick-color').removeClass('borderimg');
            //$(this).addClass('borderimg');            
            //$.ajax({
            //    type: 'GET',
            //    url: "/Product/Detail",
            //    data: {
            //        url,
            //        productUrl
            //    },
            //    dataType: 'json',
            //    beforeSend: function () {
            //        aspnetcore.startLoading();
            //    },
            //    success: function (response) {                   
            //    },
            //        error: function () {
            //        aspnetcore.notify('Có loi xay ra', 'error');
            //        aspnetcore.stopLoading();
            //    }
            //});

            //$.ajax({
            //    type: "POST",
            //    url: "/Product/GetById",
            //    data: {
            //        id: id
            //    },
            //    dataType: "json",
            //    beforeSend: function () {
            //        aspnetcore.startLoading();
            //    },
            //    success: function (response) {
            //        var data = response;                    
            //        $('#btnAddToCart').attr('data-id', data.id);
            //        breadcrumbName.text(data.name);
            //        name.text(data.name);
            //        namelink.attr({
            //            'href': '/' + productUrl + '/mau-rem=' + data.url,
            //            'title': data.name
            //        });
            //        bigImg.attr({
            //                'src': data.image1,
            //                'alt': data.image1Alt,
            //                'data-zoom-image': data.image1,
            //                'data-image': data.image1
            //        });   
            //        linkImg1.attr({
            //            'data-image': data.image1,
            //            'data-zoom-image': data.image1
            //        });
            //        img1.attr({
            //            'src': data.image1,
            //            'alt': data.image1Alt
            //        });
            //        linkImg2.attr({
            //            'data-image': data.image2,
            //            'data-zoom-image': data.image2
            //        });
            //        img2.attr({
            //            'src': data.image2,
            //            'alt': data.image2Alt
            //        });
            //        linkImg3.attr({
            //            'data-image': data.image3,
            //            'data-zoom-image': data.image3
            //        });
            //        img3.attr({
            //            'src': data.image3,
            //            'alt': data.image3Alt
            //        });
            //        linkImg4.attr({
            //            'data-image': data.image4,
            //            'data-zoom-image': data.image4
            //        });
            //        img4.attr({
            //            'src': data.image4,
            //            'alt': data.image4Alt
            //        });
            //        linkImg5.attr({
            //            'data-image': data.image5,
            //            'data-zoom-image': data.image5
            //        });
            //        img5.attr({
            //            'src': data.image5,
            //            'alt': data.image5Alt
            //        });
            //        code.text(data.code);
            //        content.text(data.content);
            //    },
            //    error: function () {
            //        aspnetcore.notify('Có loi xay ra', 'error');
            //        aspnetcore.stopLoading();
            //    }
            //}).then(function () {
           
            //});
        //});                  

       
    }    
}