var updatePostController = function () {
    //var self = this;    
    this.initialize = function () {
        registerEvents();
        registerControls();
        //setInitialOrder();
        getOldTags();
    }

    function registerEvents() {
        //Init validation
        $('#frmUpdate').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                Name: { required: true },
                CategoryId: { required: true },
                MetaTitle: { required: true },
                //txtMetaKeywords: { required: true },
                MetaDescription: { required: true },
                SortOrder: {
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

        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });

        $('#btnReset').on('click', function () {
            resetFormMaintainance();
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
                    $('#Image').val(path);
                    aspnetcore.notify('Up ảnh thành công', 'success');
                },
                error: function () {
                    aspnetcore.notify('Up ảnh bị lỗi', 'error');
                }
            });
        });

        //$('.custom-file-input').on('change', function () {
        //    var fileName = $(this).val().split("\\").pop();
        //    $(this).next(".custom-file-input").html(fileName);
        //});

        //$('#btnSelectImg').on('click', function () {
        //    $('#fileInputImage').click();
        //});

        //$("#fileInputImage").on('change', function () {
        //    var fileUpload = $(this).get(0);
        //    var files = fileUpload.files;
        //    var data = new FormData();
        //    for (var i = 0; i < files.length; i++) {
        //        data.append(files[i].name, files[i]);
        //    }
        //    $.ajax({
        //        type: "POST",
        //        url: "/Admin/Upload/UploadImage",
        //        contentType: false,
        //        processData: false,
        //        data: data,
        //        success: function (path) {
        //            $('#txtImage').val(path);
        //            aspnetcore.notify('Up ảnh thành công', 'success');
        //        },
        //        error: function () {
        //            aspnetcore.notify('Up ảnh bị lỗi', 'error');
        //        }
        //    });
        //});

        $("#CategoryId").on("change", function () {
            var postCategoryId = $('#CategoryId').val();
            setValueToNewPost(postCategoryId);
        });
        //$('#btnSavePostCategory').on('click', function (e) {
        //    if ($('#frmMaintainanceSelectPostCategory').valid()) {
        //        e.preventDefault();

        //        $('#modal-select-post-category').modal('hide');
        //        $('#modal-add-edit').modal('show');
        //        var postCategoryId = $('#ddlPostCategory').combotree('getValue');

        //        //$('input[name=ddlParentPostCategory]').change(function () {
        //        //    parentId = $('#ddlParentPostCategory').val();
        //        //});               
        //        ;  
        //        //activeShowHome();
        //        //activeShowHot();
        //    }
        //});     


        $('#OrderStatus').change(function () {
            if (this.checked) {
                if ($('#hidSortOrder').val() == 0) {
                    setValueToNewPost($('#PostCategoryId').val());
                }
                else {
                    var order = $('#hidSortOrder').val();
                    $('#SortOrder').val(order);
                }
            }
            else {
                $('#SortOrder').val(0);
            }
        });

        $('#HotOrderStatus').change(function () {
            if (this.checked) {
                if ($('#hidHotOrder').val() == 0) {
                    setNewHotOrder();
                }
                else {
                    var hotOrder = $('#hidHotOrder').val();
                    $('#HotOrder').val(hotOrder);
                }
            }
            else {
                $('#HotOrder').val(0);
            }
        });

        $('#HomeOrderStatus').change(function () {
            if (this.checked) {
                if ($('#hidHomeOrder').val() == 0) {
                    setNewHomeOrder();
                }
                else {
                    var homeOrder = $('#hidHomeOrder').val();
                    $('#HomeOrder').val(homeOrder);
                }
            }
            else {
                $('#HomeOrder').val(0);
            }
        });

    };

    function resetFormMaintainance() {
        //disableFieldEdit(false);               
        $('#OrderStatus').prop('checked', true);
        $('#HotOrderStatus').prop('checked', false);
        $('#HomeOrderStatus').prop('checked', false);
        $('#Name').val('');
        $('#MetaTitle').val('');
        $('#MetaKeywords').val('');
        $('#Url').val('');
        $('#Code').val('');
        $('#Tags').val('');
        CKEDITOR.instances.MetaDescription.setData('');
        CKEDITOR.instances.Description.setData('');
        CKEDITOR.instances.Content.setData('');
        $('#Image').val('');
    }

    function setInitialOrder() {
        var postCategoryId = $('#CategoryId').val();
        setValueToNewPost(postCategoryId);
    }

    function getOldTags() {
             var oldTag = $('#Tags').val();
            $('#oldTags').val(oldTag);
    }

    function setNewHotOrder() {
        $.ajax({
            type: "POST",
            dataType: 'json',
            url: "/Admin/Post/SetNewHotOrder",
            success: function (data) {
                $('#HotOrder').val(data.hotOrder);
            },
            error: function () {
                alert('Lỗi thiết lập thứ tự HOT!');
            }
        });
    }

    function setNewHomeOrder() {
        $.ajax({
            type: "POST",
            dataType: 'json',
            url: "/Admin/Post/SetNewHomeOrder",
            success: function (data) {
                $('#HomeOrder').val(data.homeOrder);
            },
            error: function () {
                alert('Lỗi thiết lập thứ tự trang chủ!');
            }
        });
    }

    function registerControls() {
        var editorConfig = {
            filebrowserImageUploadUrl: '/Admin/Upload/UploadImageForCKEditor?type=Images'
        }
        CKEDITOR.replace('Description', editorConfig);
        CKEDITOR.replace('Content', editorConfig);
        CKEDITOR.replace('MetaDescription', editorConfig);
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

    function setValueToNewPost(postCategoryId) {
        $.ajax({
            url: "/Admin/Post/PrepareSetNewPost",
            type: 'POST',
            dataType: 'json',
            data: {
                postCategoryId: postCategoryId
            },
            success: function (response) {
                $('#PostCategoryId').val(response.postCategoryId);
                $('#SortOrder').val(response.order);
                $('#hidSortOrder').val(response.order);
                //$('#lblPostCategoryName').text(response.postCategoryName);                
                //$('#txtHomeOrder').val(response.homeOrder);
                //$('#txtHotOrder').val(response.hotOrder);                
            }
        });
    }
}