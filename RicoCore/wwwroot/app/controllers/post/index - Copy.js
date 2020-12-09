var productController = function () {
    var imageManagement = new ImageManagement(self);

    this.initialize = function () {
        $.when(initTreeDropDownCategory()).then(function () {
            loadData();  
            loadCategories();
        });
        registerEvents();
        registerControls();
        imageManagement.initialize();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtName: { required: true },
                ddlAddUpdateCategory: { required: true },
                txtPrice: {
                    required: true,
                    number: true
                },
                txtPromotionPrice: { required: true },
                txtQuantity: { required: true }
            }
        });

        $('#txtSearch').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });

        $("#btnSearch").on('click', function () {
            loadData();
        });

        $("#fileInputImage").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    $('#txtImage').val(path);
                    aspnetcore.notify('Đã tải ảnh lên thành công!', 'success');

                },
                error: function () {
                    aspnetcore.notify('There was error uploading files!', 'error');
                }
            });
        });

        $("#btnSelectImg").on('click', function () {
            $('#fileInputImage').click();
        });

        $("#ddlShowPage").on('change', function () {
            aspnetcore.configs.pageSize = $(this).val();
            aspnetcore.configs.pageIndex = 1;
            loadData(true);
        });

        $("#btnCreate").on('click', function () {
            resetFormMaintainance();
            initTreeDropDownCategory();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btnEdit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            getDetail(that);
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                saveProduct();
                return false;
            }
            return false;
        });

        $('body').on('click', '.btnDelete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            aspnetcore.confirm('Bạn có chắc chắn muốn xoá?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Product/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        aspnetcore.startLoading();
                    },
                    success: function () {
                        aspnetcore.notify('Xoá thành công', 'success');
                        aspnetcore.stopLoading();
                        loadData();
                    },
                    error: function () {
                        aspnetcore.notify('Xoá không thành công', 'error');
                        aspnetcore.stopLoading();
                    }
                });
            });
        });

        $('body').on('click', '#btnDeleteAll', function (e) {
            e.preventDefault();
            //var selectLst = '';
            var selectLst = [];

            $.each($('input[name="ckSelect"]'), function (i, item) {
                if ($(item).prop('checked') === true)
                    //selectLst += ($(item).prop('value'));
                selectLst.push($(item).prop('value'));
                //selectLst+=...
            });
            $('#ckCheckAllDelete').on('click', function () {
                $('.ckDelete').prop('checked', $(this).prop('checked'));
            });
            $('.ckDelete').on('click', function () {
                if ($('.ckDelete:checked').length == response.length) {
                    $('#ckCheckAllDelete').prop('checked', true);
                } else {
                    $('#ckCheckAllDelete').prop('checked', false);
                }
            });

            aspnetcore.confirm('Bạn có chắc chắn muốn xoá nhieu?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Product/DeleteAll",
                    data: {
                        deleteAll: selectLst
                    },
                    beforeSend: function () {
                        aspnetcore.startLoading();
                    },
                    success: function () {
                        aspnetcore.notify('Xoá thành công', 'success');
                        aspnetcore.stopLoading();
                        loadData();
                    },
                    error: function () {
                        aspnetcore.notify('Xoá không thành công', 'error');
                        aspnetcore.stopLoading();
                    }
                });
            });
        });

        $("#btnImport").on('click', function () {
            initTreeDropDownCategory();
            $('#modal-import-excel').modal('show');

        });

        $('#btnImportExcel').on('click', function (e) {
            e.preventDefault();
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            // Adding one more key to FormData object  
            fileData.append('categoryId', $('#ddlCategoryIdImportExcel').combotree('getValue'));
            $.ajax({
                url: '/Admin/Product/ImportExcel',
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function () {
                    $('#modal-import-excel').modal('hide');
                    loadData();

                }
            });
            return false;
        });

        $("#btnExport").on('click', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Product/ExportExcel",
                beforeSend: function () {
                    aspnetcore.startLoading();
                },
                success: function (response) {
                    window.location.href = response;
                    aspnetcore.stopLoading();
                },
                error: function () {
                    aspnetcore.notify('Has an error in progress', 'error');
                    aspnetcore.stopLoading();
                }
            });
        });
    };

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/ProductCategory/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = aspnetcore.unflattern(data);

                $('#ddlLoadCategory').combotree({
                    data: arr
                });

                $('#ddlCategoryIdImportExcel').combotree({
                    data: arr
                });

                $('#ddlAddUpdateCategory').combotree({
                    data: arr
                });

                if (selectedId != undefined) {
                    $('#ddlAddUpdateCategory').combotree('setValue', selectedId);
                }
            }
        });
    }

    function resetFormMaintainance() {
        $('#hidId').val('');
        $('#txtName').val('');
        $('#txtCode').val('');
        initTreeDropDownCategory('');

        $('#txtDescription').val('');
        //$('#txtUnit').val('');

        $('#txtPrice').val('0');
        $('#txtOriginalPrice').val('');
        $('#txtPromotionPrice').val('');
        //$('#txtImage').val('');
        $('#txtContent').val('');
        $('#txtTag').val('');
        $('#txtMetakeywords').val('');
        $('#txtMetaDescription').val('');
        $('#txtPageTitle').val('');
        $('#txtPageAlias').val('');
    
        //CKEDITOR.instances.txtContentM.setData('');
        $('#ckStatus').prop('checked', true);
        $('#ckHot').prop('checked', false);
        $('#ckShowHome').prop('checked', false);

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

    function loadCategories() {
        $.ajax({
            type: 'GET',
            url: '/admin/product/GetAllCategories',
            dataType: 'json',
            success: function (response) {
                //var render = "<option value=''>--Chọn danh mục sản phẩm--</option>";
                var render = "";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Name + "</option>"
                });
                $('#ddlLoadCategory').html(render);
            },
            error: function (status) {
                console.log(status);
                aspnetcore.notify('Cannot loading product category data', 'error');
            }
        });
    }

    function loadData(isPageChanged) {      
        $.ajax({
            type: 'GET',
            url: '/Admin/Product/GetAllPaging',
            data: {
                categoryId: $('#ddlLoadCategory').combotree('getValue'),
                keyword: $('#txtSearch').val(),
                page: aspnetcore.configs.pageIndex,
                pageSize: aspnetcore.configs.pageSize
            },
            dataType: 'Json',
            beforeSend: function () {
                aspnetcore.startLoading();
            },
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                if (response.RowCount > 0) {
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Code: item.Code,
                        //Image: item.Image,
                        CategoryName: item.ProductCategory.Name,
                        Image: item.Image == null ? '<img src="/admin-side/images/user.png" width=25' : '<img src="' + item.Image + '" width=25 />',
                        Price: item.Price,
                        PromotionPrice: item.PromotionPrice,
                        //Price: aspnetcore.formatNumber(item.Price, 0),
                        //PromotionPrice: aspnetcore.formatNumber(item.PromotionPrice, 0),
                        Quantity: item.Quantity,
                        CreatedDate: aspnetcore.dateTimeFormatJson(item.CreatedDate),
                        Status: aspnetcore.getStatus(item.Status),
                    });
                });
                    $('#label-total-records').text(response.RowCount);
                    if (render != '') {
                        $('#table-content').html(render);
                    }
                    wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);
            }
                else {
                $('#table-content').html('');
            }
                aspnetcore.stopLoading();
            },
            error: function (status) {
                console.log(status);
                aspnetcore.notify('Cannot loading data', 'error');
            }
        });
    };

    function saveProduct() {
        var id = $('#hidId').val();
        var name = $('#txtName').val();
        var code = $('#txtCode').val();
        var categoryId = $('#ddlAddUpdateCategory').combotree('getValue');

        var description = $('#txtDescription').val();
        //var unit = $('#txtUnit').val();

        var price = $('#txtPrice').val();
        var originalPrice = $('#txtOriginalPrice').val();
        var promotionPrice = $('#txtPromotionPrice').val();

        var image = $('#txtImage').val();

        var tags = $('#txtTag').val();
        var metaKeywords = $('#txtMetakeywords').val();
        var metaDescription = $('#txtMetaDescription').val();
        var pageTitle = $('#txtPageTitle').val();
        var pageAlias = $('#txtPageAlias').val();
        var quantity = $('#txtQuantity').val();

        var content = CKEDITOR.instances.txtContent.getData();
        var status = $('#ckStatus').prop('checked') === true ? 1 : 0;
        var hot = $('#ckHot').prop('checked');
        var showHome = $('#ckShowHome').prop('checked');

        $.ajax({
            type: "POST",
            url: "/Admin/Product/SaveEntity",
            data: {
                Id: id,
                Name: name,
                Code: code,
                Quantity: quantity,
                CategoryId: categoryId,
                Image: image,
                Price: price,
                OriginalPrice: originalPrice,
                PromotionPrice: promotionPrice,
                Description: description,
                Content: content,
                HomeFlag: showHome,
                HotFlag: hot,
                Tags: tags,
                //Unit: unit,
                Status: status,
                PageTitle: pageTitle,
                PageAlias: pageAlias,
                MetaKeywords: metaKeywords,
                MetaDescription: metaDescription
            },
            dataType: "json",
            beforeSend: function () {
                aspnetcore.startLoading();
            },
            success: function () {
                aspnetcore.notify('Cập nhật sản phẩm thành công', 'success');
                $('#modal-add-edit').modal('hide');
                resetFormMaintainance();

                aspnetcore.stopLoading();
                loadData(true);
            },
            error: function () {
                aspnetcore.notify('Cập nhật sản phẩm có lỗi xảy ra', 'error');
                aspnetcore.stopLoading();
            }
        });
    }

    function getDetail(that) {
        $.ajax({
            type: "GET",
            url: "/Admin/Product/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                aspnetcore.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidId').val(data.Id);
                $('#txtName').val(data.Name);
                $('#txtCode').val(data.Code);
                initTreeDropDownCategory(data.CategoryId);

                $('#txtDescription').val(data.Description);
                $('#txtUnit').val(data.Unit);

                $('#txtPrice').val(data.Price);
                $('#txtOriginalPrice').val(data.OriginalPrice);
                $('#txtPromotionPrice').val(data.PromotionPrice);

                $('#txtImage').val(data.Image);

                $('#txtTag').val(data.Tags);
                $('#txtMetaKeywords').val(data.MetaKeywords);
                $('#txtMetaDescription').val(data.MetaDescription);
                $('#txtPageTitle').val(data.PageTitle);
                $('#txtPageAlias').val(data.PageAlias);

                CKEDITOR.instances.txtContent.setData(data.Content);
                $('#ckStatus').prop('checked', data.Status === 1);
                $('#ckHot').prop('checked', data.HotFlag);
                $('#ckShowHome').prop('checked', data.HomeFlag);
                $('#txtQuantity').val(data.Quantity);
                $('#modal-add-edit').modal('show');
                aspnetcore.stopLoading();

            },
            error: function () {
                aspnetcore.notify('Có lỗi xảy ra', 'error');
                aspnetcore.stopLoading();
            }
        });
    }

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