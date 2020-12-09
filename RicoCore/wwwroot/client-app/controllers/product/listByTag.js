var ProductController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        //Init validation

        $('.pick-color').on('click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var url = $(this).data('url');            
            var productUrl = $(this).parent().parent().find('.product-url').data('producturl');
            $(this).parent().siblings().find('.pick-color').removeClass('borderimg');          
            $(this).addClass('borderimg');
            var imglink = $(this).parent().parent().parent().parent().parent().prev().find('a.product-image');
            var img = $(this).parent().parent().parent().parent().parent().prev().find('img');
            var code = $(this).parent().parent().next().find('span.product-item-code');
            var name = $(this).parent().parent().parent().prev().find('h3');
            var namelink = $(this).parent().parent().parent().prev().find('a');   
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
                    code.text(data.code);
                    $('.add-to-cart').attr('data-id', data.id);
                    namelink.attr({
                        'title': data.name,
                        'href': "/" + productUrl + "/mau-rem=" + data.url
                    });
                    name.text(data.name);
                    img.attr({
                        'data-id': data.id,
                        'data-url': data.url,
                        'alt': data.image1Alt,
                        'src': data.image1
                    });
                    imglink.attr({
                        'title': data.name,
                        'href': '/' + productUrl + "/mau-rem=" + data.url
                    });                   
                },
                error: function () {
                    aspnetcore.notify('Có loi xay ra', 'error');
                    aspnetcore.stopLoading();
                }
            }).then(function () {     
                imglink.parent().find('.hid-id').val(id);  
                imglink.parent().find('.hid-url').val(url);                 
            });          
        });        
    }    
}