var postDetailController = function () {
    this.initialize = function () {
        registerEvents();
        clearSelectedCheckboxes();
    }

    function registerEvents() {        
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
                        location.reload();
                    },
                    error: function (status) {
                        aspnetcore.notify('Xoá không thành công', 'error');
                        aspnetcore.stopLoading();
                    }
                });
            });
        });

        $('#btn-import').on('click', function () {
            var selectId = $('#hiddenCategoryId').val();
            $('#modal-import-excel').modal('show');
            initTreeDropDownCategory(selectId);
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
                    location.reload();
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

    };

    function updateMasterCheckbox() {
        var numChkBoxes = $('#tbl-content input[type=checkbox][id!=mastercheckbox]').length;
        var numChkBoxesChecked = $('#tbl-content input[type=checkbox][id!=mastercheckbox]:checked').length;
        $('#mastercheckbox').prop('checked', numChkBoxes == numChkBoxesChecked && numChkBoxes > 0);
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

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/PostCategory/GetCategories",
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