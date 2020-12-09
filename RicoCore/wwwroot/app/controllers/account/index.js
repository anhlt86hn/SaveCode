var accountController = function () {
    //var self = this;   
    this.initialize = function () {
        registerEvents();
        //registerControls();     
        clearSelectedCheckboxes();            
    }

    function registerEvents() {
        //Init validation
       
        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
               
            }
        });

        $("#btn-search").on('click', function () {
          
        });


        $('.display-or-hidden-password').change(function () {
            if (this.checked) {
                var password = $(this).next().next().val();
                $(this).parent().prev().find("span").text(password);
            }
            else {
                var hiddenPassword = $(this).next().val();
                $(this).parent().prev().find("span").text(hiddenPassword);
            }
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            //var that = $('#hidId').val();
            aspnetcore.confirm('Bạn có chắc chắn muốn xoá?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Account/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        aspnetcore.startLoading();
                    },
                    success: function () {
                        aspnetcore.notify('Xoá thành công', 'success');
                        aspnetcore.stopLoading();
                        location.reload();                       
                    },
                    error: function () {
                        aspnetcore.notify('Xoá không thành công', 'error');
                        aspnetcore.stopLoading();
                    }
                });
            });
        });
        

        $('#btn-import').on('click', function () {          
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
            
            $.ajax({
                url: '/Admin/Account/ImportExcel',
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
                url: "/Admin/Account/ExportExcel",
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

            $.each($('input[type=checkbox][id!=mastercheckbox][id!=ckStatus][class!=display-or-hidden-password]'), function (i, item) {
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
                    url: "/Admin/Account/MultiDelete",
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
      
}