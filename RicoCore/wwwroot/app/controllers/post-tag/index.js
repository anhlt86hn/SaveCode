var tagController = function () {
    //var self = this;   
    this.initialize = function () {            
        registerEvents();
        registerControls();
        loadData();
        clearSelectedCheckboxes();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtName: { required: true },                       
                txtMetaTitle: { required: true },
                txtMetaKeywords: { required: true },
                txtMetaDescription: { required: true },                
                txtSortOrder: {
                    required: true,
                    number: true
                },              
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
            $('#modal-add-edit').modal('show');
            setNewOrder();
        });       

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                save();
            }    
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            //var that = $('#hidTagId').val();
            fillData(that);
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            //var that = $('#hidTagId').val();
            aspnetcore.confirm('Bạn có chắc chắn muốn xoá?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/PostTag/Delete",
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

        $('#btnCancel').on('click', function () {
            resetFormMaintainance();
        });

        $('.close').on('click', function () {
            resetFormMaintainance();
        });

        $('#btnSave').keypress(function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                save();
            }
        });
        
        function updateMasterCheckbox() {
            var numChkBoxes = $('#tbl-content input[type=checkbox][id!=mastercheckbox]').length;
            var numChkBoxesChecked = $('#tbl-content input[type=checkbox][id!=mastercheckbox]:checked').length;
            $('#mastercheckbox').prop('checked', numChkBoxes == numChkBoxesChecked && numChkBoxes > 0);
        }

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
                    url: "/Admin/PostTag/MultiDelete",
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

    function save() {
        //var that = $('#hidTagId').val();
        //var id = parseInt($('#hidTagId').val());

        var id = $('#hidTagId').val();
        //var tagId = $('#hidTagId').val();
        var name = $('#txtName').val();             
        var metaTitle = $('#txtMetaTitle').val();
        var metaDescription = $('#txtMetaDescription').val();
        var metaKeywords = $('#txtMetaKeywords').val();                      
        var sortOrder = $('#txtSortOrder').val();       

        $.ajax({
            type: "POST",
            url: "/Admin/PostTag/SaveEntity",
            data: {
                //Id: that,
                Id: id,
                Name: name,                           
                MetaTitle: metaTitle,
                MetaDescription: metaDescription,
                MetaKeywords: metaKeywords,              
                SortOrder: sortOrder
                //tagId: tagId
            },
            dataType: "json",
            beforeSend: function () {
                aspnetcore.startLoading();
            },
            success: function () {
                aspnetcore.notify('Cập nhật Tag thành công', 'success');
                $('#modal-add-edit').modal('hide');
                resetFormMaintainance();
                aspnetcore.stopLoading();
                loadData(true);
            },
            error: function () {
                aspnetcore.notify('Cập nhật Tag có lỗi xảy ra', 'error');
                aspnetcore.stopLoading();
            }
        });

        return false;
    }


    //function disableFieldEdit(disabled) {
    //    $('#txtUserName').prop('disabled', disabled);
    //    $('#txtPassword').prop('disabled', disabled);
    //    $('#txtConfirmPassword').prop('disabled', disabled);

    //}

    function clearSelectedCheckboxes() {
        $(document).ready(function () {
            var selectedIds = [];
            //clear selected checkboxes
            $('.checkboxGroups').prop('checked', false).change();
            selectedIds = [];
            return false;
        });
    }

    function setNewOrder() {
        $.ajax({
            url: "/Admin/PostTag/SetNewTagOrder",
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                $('#txtSortOrder').val(response.order);
            }
        });
    }

    function resetFormMaintainance() {
        //disableFieldEdit(false);
        $('#hidTagId').val('00000000-0000-0000-0000-000000000000');       
        $('#txtName').val('');      
        $('#txtMetaTitle').val('');
        $('#txtMetaDescription').val('');
        $('#txtMetaKeywords').val('');      
        $('#txtSortOrder').val(0);       
    }

    function registerControls() {
        var editorConfig = {
            filebrowserImageUploadUrl: '/Admin/Upload/UploadImageForCKEditor?type=Images'
        }
        CKEDITOR.replace('txtMetaDescription', editorConfig);       
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

    function fillData(that) {
        $.ajax({
            type: "GET",
            url: "/Admin/PostTag/GetById",
            data: {
                id: that
            },
            dataType: "json",
            beforeSend: function () {
                aspnetcore.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidTagId').val(data.Id);
                $('#txtName').val(data.Name);               
                $('#txtMetaKeywords').val(data.MetaKeywords);
                $('#txtMetaDescription').val(data.MetaDescription);
                $('#txtMetaTitle').val(data.MetaTitle);             
                $('#txtSortOrder').val(data.SortOrder);              
                $('#modal-add-edit').modal('show');
                aspnetcore.stopLoading();
            },
            error: function () {
                aspnetcore.notify('Có lỗi xảy ra', 'error');
                aspnetcore.stopLoading();
            }
        });
    }     

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/Admin/PostTag/GetAllPaging",
            data: {                
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
                            Name: item.Name,                           
                            SortOrder: item.SortOrder,
                            MetaTitle: item.MetaTitle,
                            MetaDescription: item.MetaDescription,
                            MetaKeywords: item.MetaKeywords,                           
                            DateModified: aspnetcore.dateTimeFormatJson(item.DateModified)                                                      
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