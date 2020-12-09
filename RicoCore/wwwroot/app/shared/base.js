var BaseController = function () {  
    this.initialize = function () {
        loadAnnouncement();
        //registerEvents();
    }

    function registerEvents() {
        //Init validation      
        $('body').on('click', '.btnEdit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Role/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    aspnetcore.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtName').val(data.Name);
                    $('#txtDescription').val(data.Description);
                    $('#modalAddEdit').modal('show');
                    aspnetcore.stopLoading();
                },
                error: function (status) {
                    aspnetcore.notify('Có lỗi xảy ra', 'error');
                    aspnetcore.stopLoading();
                }
            });
        });
    };

    function loadAnnouncement() {
        $.ajax({
            type: "GET",
            url: "/Admin/Announcement/GetAllPaging",
            data: {               
                page: aspnetcore.configs.pageIndex,
                pageSize: aspnetcore.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                aspnetcore.startLoading();
            },
            success: function (response) {
                var template = $('#announcement-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    $('#announcementArea').show();
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            Content: item.Content,
                            Id: item.Id,
                            Title: item.Title,
                            FullName: item.Avatar
                        });
                    });
                    render += $('#announcement-tag-template').html();
                    $("#totalAnnouncement").text(response.RowCount);
                    if (render != undefined) {
                        $('#announcementList').html(render);

                    }                  
                }
                else {
                    $('#announcementArea').hide();
                    $('#announcementList').html('');
                }
                aspnetcore.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };  
}
