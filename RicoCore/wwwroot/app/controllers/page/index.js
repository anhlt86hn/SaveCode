var PageController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
        registerControls();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtName: { required: true },
                txtUrl: { required: true },
                txtMetaTitle: { required: true },
                txtMetaKeywords: { required: true },
                txtMetaDescription: { required: true }            
            }
        });

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });
        $("#btn-search").on('click', function () {
            loadData();
        });

        $("#ddl-show-page").on('change', function () {
            aspnetcore.configs.pageSize = $(this).val();
            aspnetcore.configs.pageIndex = 1;
            loadData(true);
        });

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Page/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    aspnetcore.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtName').val(data.Name);
                    $('#txtMetaKeywords').val(data.MetaKeywords);
                    $('#txtMetaDescription').val(data.MetaDescription);
                    $('#txtMetaTitle').val(data.MetaTitle);
                    $('#txtUrl').val(data.Url);
                    $('#txtCode').val(data.UniqueCode);
                    $('#txtCode').prop('readonly', true);
                    CKEDITOR.instances.txtContent.setData(data.Content);
                    $('#ckStatus').prop('checked', data.Status === 1);

                    $('#modal-add-edit').modal('show');
                    aspnetcore.stopLoading();

                },
                error: function () {
                    aspnetcore.notify('Có lỗi xảy ra', 'error');
                    aspnetcore.stopLoading();
                }
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidId').val();
                var name = $('#txtName').val();
                var url = $('#txtUrl').val();
                var metaTitle = $('#txtMetaTitle').val();
                var metaDescription = $('#txtMetaDescription').val();
                var metaKeywords = $('#txtMetaKeywords').val();
                var content = CKEDITOR.instances.txtContent.getData();
                var status = $('#ckStatus').prop('checked') === true ? 1 : 0;
                var code = $('#txtCode').val();

                $.ajax({
                    type: "POST",
                    url: "/Admin/Page/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Content: content,
                        Status: status,
                        Url: url,
                        MetaTitle: metaTitle,
                        MetaDescription: metaDescription,
                        MetaKeywords: metaKeywords,
                        Code: code
                    },
                    dataType: "json",
                    beforeSend: function () {
                        aspnetcore.startLoading();
                    },
                    success: function () {
                        aspnetcore.notify('Thêm mới/Cập nhật trang đơn thành công', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();

                        aspnetcore.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        aspnetcore.notify('Đã có lỗi trong quá trình thêm mới/cập nhật trang đơn', 'error');
                        aspnetcore.stopLoading();
                    }
                });
                return false;
            }
            return false;
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            aspnetcore.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Page/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        aspnetcore.startLoading();
                    },
                    success: function () {
                        aspnetcore.notify('Xóa trang đơn thành công', 'success');
                        aspnetcore.stopLoading();
                        loadData();
                    },
                    error: function () {
                        aspnetcore.notify('Đã có lỗi trong quá trình xóa trang đơn', 'error');
                        aspnetcore.stopLoading();
                    }
                });
            });
        });
    };

    function resetFormMaintainance() {
        //$('#hidId').val(0);
        //$('#hideId').val('');   
        $('#hidId').val("00000000-0000-0000-0000-000000000000");
        $('#txtName').val('');
        $('#txtUrl').val('');
        CKEDITOR.instances.txtContent.setData('');
        $('#ckStatus').prop('checked', true);
        $('#txtMetaTitle').val('');
        $('#txtMetaDescription').val('');
        $('#txtMetaKeywords').val('');
        $('#txtCode').val('');
    }

    function registerControls() {
        var editorConfig = {
            filebrowserImageUploadUrl: '/Admin/Upload/UploadImageForCKEditor?type=Images'
        }
        CKEDITOR.replace('txtContent', editorConfig);
        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/page/GetAllPaging",
            data: {
                keyword: $('#txt-search-keyword').val(),
                page: aspnetcore.configs.pageIndex,
                pageSize: aspnetcore.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                aspnetcore.startLoading();
            },
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            Name: item.Name,
                            Url: item.Url,
                            Id: item.Id,
                            Code: item.Code,
                            Status: aspnetcore.getStatus(item.Status),
                            DateCreated: aspnetcore.dateTimeFormatJson(item.DateCreated),
                        });
                    });
                    $("#lbl-total-records").text(response.RowCount);
                    if (render != undefined) {
                        $('#tbl-content').html(render);

                    }
                    wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);


                }
                else {
                    $('#tbl-content').html('');
                }
                aspnetcore.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / aspnetcore.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, p) {
                aspnetcore.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
}