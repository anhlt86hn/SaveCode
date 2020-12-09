var customController = function () {
    
    this.initialize = function () {
        registerEvents();    
        clearSelectedCheckboxes();
        loadData();
    }

    function registerEvents() {
        //Init validation
        $('#frmAddOrUpdate').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtName: { required: true },               
                txtCode: { required: true },
                //txtUrl: { required: true },                
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
            //resetFormMaintainance();           
            $('#modal-add-edit').modal('show');
            
        });
       

        $('#btnSave').on('click', function (e) {
            if ($('#frmAddOrUpdate').valid()) {
                e.preventDefault();
                save();
            }            
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            //var that = $('#hidId').val();
            fillData(that);
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            //var that = $('#hidId').val();
            aspnetcore.confirm('Bạn có chắc chắn muốn xoá?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/CustomConfig/Delete",
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

        $('.close').on('click', function () {
            resetFormMaintainance();
        });

        $('#btnSave').keypress(function (e) {
            if ($('#frmAddOrUpdate').valid()) {
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
                    url: "/Admin/CustomConfig/MultiDelete",
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
        //var that = $('#hidId').val();
        //var id = parseInt($('#hidId').val());

        var id = $('#hidId').val();
        var name = $('#txtName').val();
        var code = $('#txtCode').val();        
        var url = $('#txtUrl').val();
        var stringValue = $('#txtStringValue').val();
        var integerValue = $('#txtIntegerValue').val();
        var boolVal = parseInt($('#ddlBooleanValue').val());
        var booleanValue = boolVal == 1 ? true : (boolVal = NaN ? null : false);
        //var booleanValue = $('#ckBooleanValue').prop('checked') === true ? 1 : 0; 
        var dateValue = $('#txtDateValue').val();
        var decimalValue = $('#txtDecimalValue').val();
       
        var status = $('#ckStatus').prop('checked') === true ? 1 : 0;        

        $.ajax({
            type: "POST",
            url: "/Admin/CustomConfig/SaveEntity",
            data: {               
                Id: id,
                Name: name,
                UniqueCode: code,
                Url: url,
                TextValue: stringValue,
                IntegerValue: integerValue,
                BooleanValue: booleanValue,
                DateValue: dateValue,
                DecimalValue: decimalValue,
                Status: status
            },
            dataType: "json",
            beforeSend: function () {
                aspnetcore.startLoading();
            },
            success: function () {
                aspnetcore.notify('Cập nhật thành công', 'success');
                $('#modal-add-edit').modal('hide');
                resetFormMaintainance();
                aspnetcore.stopLoading();
                loadData(true);
            },
            error: function () {
                aspnetcore.notify('Cập nhật có lỗi xảy ra', 'error');
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
        $('#txtName').val('');
        $('#txtUrl').val('');
        $('#txtCode').val('');   
        $('#txtStringValue').val('');
        $('#txtIntegerValue').val();
        $('#txtDecimalValue').val();
        $('#txtDateValue').val();
        $('#ckBooleanValue').prop('checked', true);
        $('#ckStatus').prop('checked', true);
    }    

    function fillData(that) {        
        $.ajax({
            type: "GET",
            url: "/Admin/CustomConfig/GetById",
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
                $('#txtName').val(data.Name);                          
                $('#txtCode').val(data.UniqueCode);                               
                $('#txtUrl').val(data.Url); 
                $('#txtStringValue').val(data.TextValue);
                $('#txtIntegerValue').val(data.IntegerValue);
                $('#txtDateValue').val(data.DateValue);
                $('#txtDecimalValue').val(data.DecimalValue);
                $('#txtBooleanValue').prop('checked', data.BooleanValue === 1);
                $('#ckStatus').prop('checked', data.Status === 1);              
                //disableFieldEdit(true);
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
            url: "/Admin/CustomConfig/GetAllPaging",
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
                            Url: item.Url,
                            UniqueCode: item.UniqueCode,                                            
                            Status: aspnetcore.getStatus(item.Status)
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