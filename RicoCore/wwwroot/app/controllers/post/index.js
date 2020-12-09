var postController = function () {
    //var self = this;
    var imageManagement = new ImageManagement(self);
    this.initialize = function () {
        registerEvents();
        //registerControls();
        imageManagement.initialize();
        clearSelectedCheckboxes();
        $.when(initTreeDropDownCategory()).then(function () {
            loadData();
        });        
        
    }

    function registerEvents() {
        //Init validation
        $('#frmAddOrUpdate').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtName: { required: true },               
                txtMetaTitle: { required: true },
                //txtMetaKeywords: { required: true },
                txtMetaDescription: { required: true },  
                txtImage: { required: true },
                txtSortOrder: {
                    required: true,
                    number: true
                },               
                //txtHomeOrder: {
                //    required: true,
                //    number: true
                //},
                //txtHotOrder: {
                //    required: true,
                //    number: true
                //},
                //txtRelCanonical: { required: true },

                //txtHomeOrder: { number: true }
                //txtPassword: {
                //    required: true,
                //    minlength: 6
                //},
                //txtConfirmPassword: {
                //    equalTo: "#txtPassword"
                //},
                //txtEmail: {
                //    required: true,
                //    email: true
                //}
            }
        });

        $('#frmMaintainanceSelectPostCategory').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                ddlPostCategory: { required: true },
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

        $("#btnCreate").on('click', function () {
            //initRoleList();    
            resetFormMaintainance();   
            initTreeDropDownCategory();                    
            $('#modal-select-post-category').modal('show');
        });

        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
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
                    aspnetcore.notify('Up ảnh thành công', 'success');
                },
                error: function () {
                    aspnetcore.notify('Up ảnh bị lỗi', 'error');
                }
            });
        });

        $('#btnSavePostCategory').on('click', function (e) {
            if ($('#frmMaintainanceSelectPostCategory').valid()) {
                e.preventDefault();

                $('#modal-select-post-category').modal('hide');
                $('#modal-add-edit').modal('show');
                var postCategoryId = $('#ddlPostCategory').combotree('getValue');

                //$('input[name=ddlParentPostCategory]').change(function () {
                //    parentId = $('#ddlParentPostCategory').val();
                //});               
                setValueToNewPost(postCategoryId);  
                //activeShowHome();
                //activeShowHot();
            }
        });     
        

        $('#ckStatus').change(function () {
            if (this.checked) {
                if ($('#hidSortOrder').val() == 0) {                   
                    setValueToNewPost($('txtPostCategoryId').val());
                }
                else {
                    var order = $('#hidSortOrder').val();
                    $('#txtSortOrder').val(order);
                }
            }
            else {
                $('#txtSortOrder').val(0);
            }
        });

        $('#ckShowHot').change(function () {
            if (this.checked) {
                if ($('#hidHotOrder').val() == 0) {
                    setNewHotOrder();
                }
                else {
                    var hotOrder = $('#hidHotOrder').val();
                    $('#txtHotOrder').val(hotOrder);
                }
            }
            else {
                $('#txtHotOrder').val(0);
            }
        });

        $('#ckShowHome').change(function () {
            if (this.checked) {
                if ($('#hidHomeOrder').val() == 0) {
                    setNewHomeOrder();
                }
                else {
                    var homeOrder = $('#hidHomeOrder').val();
                    $('#txtHomeOrder').val(homeOrder);
                }
            }
            else {
                $('#txtHomeOrder').val(0);
            }
        });


        $('#btnSave').on('click', function (e) {
            if ($('#frmAddOrUpdate').valid()) {
                e.preventDefault();
                save();
            }
            //    //var that = $('#hidId').val();
            //    //var id = parseInt($('#hidId').val());

            //    //var id = $('#hidId').val();
            //    var name = $('#txtName').val();
            //    //var code = $('#txtCode').val();

            //    var parentId = $('#parentPostCategoryId').val();

            //    var metaTitle = $('#txtMetaTitle').val();
            //    var metaDescription = $('#txtMetaDescription').val();
            //    var metaKeywords = $('#txtMetaKeywords').val();
            //    var image = $('#txtImage').val();
            //    var description = $('#txtDescription').val();
            //    //var description = CKEDITOR.instances.txtDescription.getData();
            //    var url = $('#txtUrl').val();
            //    //var relCanonical = $('#txtRelCanonical').val();                 
            //    var sortOrder = $('#txtSortOrder').val();

            //    var homeOrder = $('#txtHomeOrder').val();
            //    var hotOrder = $('#txtHotOrder').val();
            //    //if (homeOrder == "" || homeOrder == "0") {
            //    //    homeOrder = 100;
            //    //}                
            //    var status = $('#ckStatus').prop('checked') === true ? 1 : 0;
            //    var homeFlag = $('#ckShowHome').prop('checked') === true ? 1 : 0;
            //    var hotFlag = $('#ckShowHot').prop('checked') === true ? 1 : 0;

            //    $.ajax({
            //        type: "POST",
            //        url: "/Admin/PostCategory/SaveEntity",
            //        data: {
            //            //Id: that,
            //            //Id: id,
            //            Name: name,
            //            //Code: code,

            //            ParentId: parentId,
            //            MetaTitle: metaTitle,
            //            MetaDescription: metaDescription,
            //            MetaKeywords: metaKeywords,
            //            Image: image,
            //            Description: description,
            //            //RelCanonical: relCanonical,
            //            Url: url,
            //            SortOrder: sortOrder,
            //            HomeFlag: homeFlag,
            //            HomeOrder: homeOrder,
            //            HotFlag: hotFlag,
            //            HotOrder: hotOrder,
            //            Status: status
            //        },
            //        dataType: "json",
            //        beforeSend: function () {
            //            aspnetcore.startLoading();
            //        },
            //        success: function () {
            //            aspnetcore.notify('Cập nhật sản phẩm thành công', 'success');
            //            $('#modal-add-edit').modal('hide');
            //            resetFormMaintainance();
            //            aspnetcore.stopLoading();
            //            loadData(true);
            //        },
            //        error: function () {
            //            aspnetcore.notify('Cập nhật sản phẩm có lỗi xảy ra', 'error');
            //            aspnetcore.stopLoading();
            //        }
            //    });

            //    return false;
            //}
        });

        //$('body').on('click', '.btn-edit', function (e) {
        //    e.preventDefault();
        //    var that = $(this).data('id');
        //    //var that = $('#hidId').val();
        //    fillData(that);
        //});

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            //var that = $('#hidId').val();
            aspnetcore.confirm('Bạn có chắc chắn muốn xoá?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Post/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        aspnetcore.startLoading();
                    },
                    success: function () {
                        aspnetcore.notify('Xoá thành công', 'success');
                        aspnetcore.stopLoading();
                        loadData();
                        resetFormMaintainance();
                    },
                    error: function () {
                        aspnetcore.notify('Xoá không thành công', 'error');
                        aspnetcore.stopLoading();
                    }
                });
            });
        });

        $('#btnCancel').on('click', function () {
            resetFormMaintainance();
        });

        $('.btn-cancel').on('click', function () {
            resetFormMaintainance();
        });

        $('#btnSave').keypress(function (e) {
            if ($('#frmAddOrUpdate').valid()) {
                e.preventDefault();
                save();
            }
        });

        $('#btn-import').on('click', function () {
            initTreeDropDownCategory();
            $('#modal-import-excel').modal('show');
        });

        $('#btnImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            // Adding one more key to FormData object  
            fileData.append('categoryId', $('#ddlCategoryImportExcel').combotree('getValue'));
            $.ajax({
                url: '/Admin/Post/ImportExcel',
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    $('#modal-import-excel').modal('hide');
                    loadData();
                }
            });
            return false;
        });

        $('#btn-export').on('click', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Post/ExportExcel",
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

       

        $('#mastercheckbox').on('click', function () {
            //$('#mastercheckbox').click(function () {
            $('.checkboxGroups').prop('checked', $(this).prop('checked'));
            //$('.checkboxGroups').prop('checked', $(this).is(':checked')).change();
        });

        $('body').on('click', '#delete-selected', function (e) {
            //$('#delete-selected').click(function (e) {
            e.preventDefault();
            var selectedIds = [];

            updateMasterCheckbox();

            $.each($('input[type=checkbox][id!=mastercheckbox][id!=ckStatus]'), function (i, item) {
                if ($(item).prop('checked') === true)
                    selectedIds.push($(item).prop('value'));
            });

            var postData = {
                selectedIds: selectedIds
            };
            //addAntiForgeryToken(postData);
            aspnetcore.confirm('Bạn có chắc chắn muốn xoá?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Post/MultiDelete",
                    data: postData,
                    beforeSend: function () {
                        aspnetcore.startLoading();
                    },
                    success: function (response) {
                        aspnetcore.notify('Xoá thành công', 'success');
                        aspnetcore.stopLoading();
                        loadData();
                    },
                    error: function (status) {
                        aspnetcore.notify('Xoá không thành công', 'error');
                        aspnetcore.stopLoading();
                    }
                });
            });
        });
    };

    function updateMasterCheckbox() {
        var numChkBoxes = $('#tbl-content input[type=checkbox][id!=mastercheckbox]').length;
        var numChkBoxesChecked = $('#tbl-content input[type=checkbox][id!=mastercheckbox]:checked').length;
        $('#mastercheckbox').prop('checked', numChkBoxes == numChkBoxesChecked && numChkBoxes > 0);
    }

    function activeShowHome() {
        $('#ckShowHome').change(function () {
            if (this.checked) {
                $.ajax({
                    url: "/Admin/Post/SetNewPostHomeOrder",
                    type: 'GET',
                    dataType: 'json',
                    success: function (response) {
                        $('#txtHomeOrder').val(response.order);
                    }
                });
            }
            else {
                $('#txtHomeOrder').val(0);
            }
        });
    }

    function activeShowHot() {
        $('#ckShowHot').change(function () {
            if (this.checked) {
                $.ajax({
                    url: "/Admin/Post/SetNewPostHotOrder",
                    type: 'GET',
                    dataType: 'json',
                    success: function (response) {
                        $('#txtHotOrder').val(response.order);
                    }
                });
            }
            else {
                $('#txtHotOrder').val(0);
            }
        });
    }

    function save() {
        //var that = $('#hidId').val();
        //var id = parseInt($('#hidId').val());

        var id = $('#hidId').val();
        var name = $('#txtName').val();
        var code = $('#txtCode').val();
        var oldTags = $('#oldTags').val();
        var postCategoryId = $('#txtPostCategoryId').val();        
        var metaTitle = $('#txtMetaTitle').val();
        var metaDescription = $('#txtMetaDescription').val();
        var metaKeywords = $('#txtMetaKeywords').val();
        var image = $('#txtImage').val();
        //var description = $('#txtDescription').val();
        //var content = $('#txtContent').val();
        var content = CKEDITOR.instances.txtContent.getData();
        var description = CKEDITOR.instances.txtDescription.getData();
        var url = $('#txtUrl').val();
        //var relCanonical = $('#txtRelCanonical').val();                 
        var sortOrder = $('#txtSortOrder').val();
        var tags = $('#txtTag').val();
        var homeOrder = $('#txtHomeOrder').val();
        var hotOrder = $('#txtHotOrder').val();
        //if (homeOrder == "" || homeOrder == "0") {
        //    homeOrder = 100;
        //}                
        var status = $('#ckStatus').prop('checked') === true ? 1 : 0;
        var homeFlag = $('#ckShowHome').prop('checked') === true ? 1 : 0;
        var hotFlag = $('#ckShowHot').prop('checked') === true ? 1 : 0;

        $.ajax({
            type: "POST",
            url: "/Admin/Post/SaveEntity",
            data: {
                oldTags: oldTags,
                Id: id,
                Name: name,
                Code: code,
                CategoryId: postCategoryId,                
                MetaTitle: metaTitle,
                MetaDescription: metaDescription,
                MetaKeywords: metaKeywords,
                Image: image,
                Description: description,
                Content: content,
                //RelCanonical: relCanonical,
                Tags: tags,
                Url: url,
                SortOrder: sortOrder,
                HomeFlag: homeFlag,
                HomeOrder: homeOrder,
                HotFlag: hotFlag,
                HotOrder: hotOrder,
                Status: status
            },
            dataType: "json",
            beforeSend: function () {
                aspnetcore.startLoading();
            },
            success: function () {
                aspnetcore.notify('Cập nhật bài viết thành công', 'success');
                $('#modal-add-edit').modal('hide');
                resetFormMaintainance();
                aspnetcore.stopLoading();
                loadData(true);
            },
            error: function () {
                aspnetcore.notify('Cập nhật bài viết có lỗi xảy ra', 'error');
                aspnetcore.stopLoading();
            }
        });

        return false;
    }

    function disableFieldEdit(disabled) {
        $('#txtUserName').prop('disabled', disabled);
        $('#txtPassword').prop('disabled', disabled);
        $('#txtConfirmPassword').prop('disabled', disabled);
    }

    function clearSelectedCheckboxes() {
        $(document).ready(function () {
            var selectedIds = [];
            //clear selected checkboxes
            $('.checkboxGroups').prop('checked', false).change();
            selectedIds = [];
            return false;
        });
    }

    function resetFormMaintainance() {
        //disableFieldEdit(false);
        $('#hidId').val("00000000-0000-0000-0000-000000000000");
        $('#oldTags').val('');
        $('#txtPostCategoryId').val("0");       
        $('#lblPostCategoryName').text('');
        $('#txtName').val('');
        $('#txtUrl').val('');
        $('#txtCode').val('');   
        $('#txtTag').val('');
        //$('#txtContent').val('');
        CKEDITOR.instances.txtContent.setData('');
        initTreeDropDownCategory('');
        //$('input[name="ckRoles"]').removeAttr('checked');
        $('#txtMetaTitle').val('');
        $('#txtMetaDescription').val('');
        $('#txtMetaKeywords').val('');
        $('#txtImage').val('');
        //$('#txtDescription').val('');
        CKEDITOR.instances.txtDescription.setData('');
        $('#txtRelCanonical').val('');
        $('#txtSortOrder').val('');
        $('#hidSortOrder').val(0);
        $('#ckShowHome').prop('checked', false);
        $('#txtHomeOrder').val(0);
        $('#hidHomeOrder').val(0);
        $('#ckShowHot').prop('checked', false);
        $('#txtHotOrder').val(0);
        $('#hidHotOrder').val(0);
        $('#ckStatus').prop('checked', true);
    }

    //function registerControls() {
    //    var editorConfig = {
    //        filebrowserImageUploadUrl: '/Admin/Upload/UploadImageForCKEditor?type=Images'
    //    }
    //    CKEDITOR.replace('txtDescription', editorConfig);
    //    CKEDITOR.replace('txtContent', editorConfig);
    //    //Fix: cannot click on element ck in modal
    //    $.fn.modal.Constructor.prototype.enforceFocus = function () {
    //        $(document)
    //            .off('focusin.bs.modal') // guard against infinite focus loop
    //            .on('focusin.bs.modal', $.proxy(function (e) {
    //                if (
    //                    this.$element[0] !== e.target && !this.$element.has(e.target).length
    //                    // CKEditor compatibility fix start.
    //                    && !$(e.target).closest('.cke_dialog, .cke').length
    //                    // CKEditor compatibility fix end.
    //                ) {
    //                    this.$element.trigger('focus');
    //                }
    //            }, this));
    //    };
    //}

    function fillData(that) {        
        $.ajax({
            type: "GET",
            url: "/Admin/Post/GetById",
            data: {
                id: that
            },
            dataType: "json",
            beforeSend: function () {
                aspnetcore.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidId').val(data.Id);
                $('#oldTags').val(data.Tags);
                $('#txtName').val(data.Name);
                $('#lblPostCategoryName').text(data.CategoryName);
                $('#txtPostCategoryId').val(data.CategoryId);               
                $('#txtCode').val(data.Code);
                $('#txtCode').prop('readonly', true);
                //initTreeDropDownCategory(data.CategoryId);
                //$('#txtContent').val(data.Content);
                //$('#txtDescription').val(data.Description);
                CKEDITOR.instances.txtContent.setData(data.Content);
                CKEDITOR.instances.txtDescription.setData(data.Description);
                $('#txtImage').val(data.Image);
                $('#txtMetaKeywords').val(data.MetaKeywords);
                $('#txtMetaDescription').val(data.MetaDescription);
                $('#txtMetaTitle').val(data.MetaTitle);
                $('#txtUrl').val(data.Url);
                $('#txtTag').val(data.Tags);
                //$('#txtRelCanonical').val(data.RelCanonical);
                $('#ckStatus').prop('checked', data.Status === 1);
                $('#ckShowHome').prop('checked', data.HomeFlag);
                $('#ckShowHot').prop('checked', data.HotFlag);
                $('#txtSortOrder').val(data.SortOrder);
                $('#hidSortOrder').val(data.SortOrder);
                $('#txtHomeOrder').val(data.HomeOrder);
                $('#hidHomeOrder').val(data.HomeOrder);
                $('#txtHotOrder').val(data.HotOrder);
                $('#hidHotOrder').val(data.HotOrder);
                //disableFieldEdit(true);
                $('#modal-add-edit').modal('show');
                aspnetcore.stopLoading();

            },
            error: function () {
                aspnetcore.notify('Có lỗi xảy ra', 'error');
                aspnetcore.stopLoading();
            }
        });
        //activeShowHome();
        //activeShowHot();
    }

    function activeShowHome() {
        $('#ckShowHome').change(function () {
            if (this.checked) {
                $.ajax({
                    url: "/Admin/Post/SetNewHomeOrder",
                    type: 'GET',
                    dataType: 'json',
                    success: function (response) {
                        $('#txtHomeOrder').val(response.homeOrder);
                    }
                });
            }
            else {
                $('#txtHomeOrder').val(0);
            }
        });
    }

    function activeShowHot() {
        $('#ckShowHot').change(function () {
            if (this.checked) {
                $.ajax({
                    url: "/Admin/Post/SetNewHotOrder",
                    type: 'GET',
                    dataType: 'json',
                    success: function (response) {
                        $('#txtHotOrder').val(response.hotOrder);
                    }
                });
            }
            else {
                $('#txtHotOrder').val(0);
            }
        });
    }

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/PostCategory/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                //data.push({
                //    id: 0,
                //    text: "Tất cả",
                //    parentId: 0,
                //    sortOrder: 0
                //});
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = aspnetcore.unflattern(data);                
                $('#ddlPostCategoryFilter').combotree({
                    data: arr
                }); 
                $('#ddlPostCategory').combotree({
                    data: arr
                });     
                $('#ddlCategoryImportExcel').combotree({
                    data: arr
                });
                if (selectedId != undefined) {
                    $('#ddlPostCategory').combotree('setValue', selectedId);
                }
                if (selectedId != undefined) {
                    $('#ddlPostCategoryFilter').combotree('setValue', selectedId);
                }
                if (selectedId != undefined) {
                    $('#ddlCategoryImportExcel').combotree('setValue', selectedId);
                }
            }
        });
    }

    function setValueToNewPost(postCategoryId) {
        $.ajax({
            url: "/Admin/Post/PrepareSetNewPost",
            type: 'POST',
            dataType: 'json',
            data: {
                postCategoryId: postCategoryId
            },
            success: function (response) {
                $('#txtPostCategoryId').val(response.postCategoryId);
                $('#txtSortOrder').val(response.order);
                $('#hidSortOrder').val(response.order);
                $('#lblPostCategoryName').text(response.postCategoryName);                
                //$('#txtHomeOrder').val(response.homeOrder);
                //$('#txtHotOrder').val(response.hotOrder);                
            }
        });
    }   

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",            
            url: "/Admin/Post/GetAllPaging",
            data: {
                categoryId: $('#ddlPostCategoryFilter').combotree('getValue'),
                keyword: $('#txtSearch').val(),
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
                            Id: item.Id,
                            Code: item.Code,
                            Name: item.Name,
                            CategoryName: item.CategoryName,
                            Status: aspnetcore.getStatus(item.Status),
                            SortOrder: item.SortOrder,
                            Tags: item.Tags,
                            HotFlag: aspnetcore.getStatus(item.HotFlag),                            
                            HotOrder: item.HotOrder,
                            HomeFlag: aspnetcore.getStatus(item.HomeFlag), 
                            HomeOrder: item.HomeOrder,
                            Image: item.Image == null ? '<img src="/admin-side/images/user.png" width=100' : '<img src="' + item.Image + '" width=100 />',
                            Url: item.Url,
                            DateCreated: aspnetcore.dateTimeFormatJson(item.DateCreated), 
                            DateModified: aspnetcore.dateTimeFormatJson(item.DateModified),
                            //OrderStatus: aspnetcore.displayStatus(item.OrderStatus),
                            //HotOrderStatus: aspnetcore.displayStatus(item.HotOrderStatus),
                            //HomeOrderStatus: aspnetcore.displayStatus(item.HomeOrderStatus)                            
                        });
                    });
                    $("#lbl-total-records").text(response.RowCount);
                    if (render !== undefined) {
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