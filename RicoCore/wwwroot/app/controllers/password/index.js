var passwordController = function () {
    //var self = this;   
    this.initialize = function () {
        registerEvents();
        //registerControls();     
        clearSelectedCheckboxes();        
    }

    function registerEvents() {
        //Init validation
        $('#frmAdd').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                Content: { required: true },               
                Level: { required: true },              
                Order: { required: true },  
                txtImage: { required: true },           
            }
        });      

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
               
            }
        });

        $("#btn-search").on('click', function () {
          
        });

              

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            //var that = $('#hidId').val();
            aspnetcore.confirm('Bạn có chắc chắn muốn xoá?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Password/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        aspnetcore.startLoading();
                    },
                    success: function () {
                        aspnetcore.notify('Xoá thành công', 'success');
                        aspnetcore.stopLoading();
                        location.reload();
                        resetFormMaintainance();
                    },
                    error: function () {
                        aspnetcore.notify('Xoá không thành công', 'error');
                        aspnetcore.stopLoading();
                    }
                });
            });
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
                url: '/Admin/Password/ImportExcel',
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
                url: "/Admin/Password/ExportExcel",
                beforeSend: function () {
                    tedu.startLoading();
                },
                success: function (response) {
                    window.location.href = response;
                    tedu.stopLoading();
                },
                error: function () {
                    tedu.notify('Has an error in progress', 'error');
                    tedu.stopLoading();
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
                    url: "/Admin/Password/MultiDelete",
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
        $('#Content').val('');
        $('#Level').val('');   
        $('#Order').val('');        
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
}