var postCategoryController = function () {
    //var self = this;
    this.initialize = function () {
        loadData();
        registerEvents();
        registerControls();
        clearSelectedCheckboxes();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                Name: { required: true },
                //txtCode: { required: true },
                //txtUrl: { required: true },
                //ddlPostCategoryId: { required: true },
                MetaTitle: { required: true },
                MetaKeywords: { required: true },
                MetaDescription: { required: true },
                ckStatus: { required: true },
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

        $('#frmMaintainanceSelectPostCategory').valid({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                ddlPostCategoryId: { required: true },
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
                var parentId = $('#ddlPostCategoryId').combotree('getValue');

                //$('input[name=ddlParentPostCategory]').change(function () {
                //    parentId = $('#ddlParentPostCategory').val();
                //});               
                setValueToNewPostCategory(parentId);   
                //activeShowHome();
                //activeShowHot();
            }
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                save();
            }
           
        });

        $('body').on('click', '#btnEdit', function (e) {
            e.preventDefault();
            //var that = $(this).data('id');
            var that = $('#hidId').val();
            fillData(that);
        });

        $('body').on('click', '#btnDelete', function (e) {
            e.preventDefault();
            //var that = $(this).data('id');
            var that = $('#hidId').val();
            aspnetcore.confirm('Bạn có chắc chắn muốn xoá?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/PostCategory/Delete",
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

    }

    function save() {
        //var that = $('#hidId').val();
        //var id = parseInt($('#hidId').val());        

        var id = $('#hidId').val();
        var name = $('#txtName').val();
        var code = $('#txtCode').val();
        var parentId = $('#txtParentCategoryId').val();        
        var metaTitle = $('#txtMetaTitle').val();
        var metaDescription = $('#txtMetaDescription').val();
        var metaKeywords = $('#txtMetaKeywords').val();
        var image = $('#txtImage').val();
        //var description = $('#txtDescription').val();
        var description = CKEDITOR.instances.txtDescription.getData();
        var url = $('#txtUrl').val();
        //var relCanonical = $('#txtRelCanonical').val();                 
        var sortOrder = $('#txtSortOrder').val();
        var homeOrder = $('#txtHomeOrder').val();
        var hotOrder = $('#txtHotOrder').val();  
        //var order = $('#txtOrder').val();
        //if (homeOrder == "" || homeOrder == "0") {
        //    homeOrder = 100;
        //}                
        var status = $('#ckStatus').prop('checked') === true ? 1 : 0;
        var homeFlag = $('#ckShowHome').prop('checked') === true ? 1 : 0;
        var hotFlag = $('#ckShowHot').prop('checked') === true ? 1 : 0;

        $.ajax({
            type: "POST",
            url: "/Admin/PostCategory/SaveEntity",
            data: {
                //Id: that,
                Id: id,
                Name: name,
                Code: code,
                ParentId: parentId,               
                MetaTitle: metaTitle,
                MetaDescription: metaDescription,
                MetaKeywords: metaKeywords,
                Image: image,
                Description: description,
                //RelCanonical: relCanonical,
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
        $('#hidId').val("0");      
        $('#txtParentCategoryId').val("0");       
        $('#lblParentPostCategoryName').text('');
        $('#txtName').val('');
        $('#txtUrl').val('');
        $('#txtCode').val('');        
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
        $('#ckShowHome').prop('checked', false);
        $('#txtHomeOrder').val(0);
        $('#ckShowHot').prop('checked', false);
        $('#txtHotOrder').val(0);
        //$('#txtOrder').val('');
        $('#ckStatus').prop('checked', true);
    }

    function registerControls() {
        var editorConfig = {
            filebrowserImageUploadUrl: '/Admin/Upload/UploadImageForCKEditor?type=Images'
        }
        CKEDITOR.replace('txtDescription', editorConfig);
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
            url: "/Admin/PostCategory/GetById",
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
                $('#txtParentCategoryId').val(data.ParentId);        
                $('#lblParentPostCategoryName').text(data.ParentPostCategoryName);                                
                $('#txtCode').val(data.Code);
                $('#txtCode').prop('readonly', true);
                //initTreeDropDownCategory(data.CategoryId);
                //$('#txtDescription').val(data.Description);
                CKEDITOR.instances.txtDescription.setData(data.Description);
                $('#txtImage').val(data.Image);
                $('#txtMetaKeywords').val(data.MetaKeywords);
                $('#txtMetaDescription').val(data.MetaDescription);
                $('#txtMetaTitle').val(data.MetaTitle);
                $('#txtUrl').val(data.Url);
                //$('#txtRelCanonical').val(data.RelCanonical);
                $('#ckStatus').prop('checked', data.Status === 1);
                $('#ckShowHome').prop('checked', data.HomeFlag);
                $('#ckShowHot').prop('checked', data.HotFlag);
                $('#txtSortOrder').val(data.SortOrder);
                $('#txtHomeOrder').val(data.HomeOrder);
                $('#txtHotOrder').val(data.HotOrder);               
                //$('#txtOrder').val(data.Order);
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

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/PostCategory/GetAll",
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
                $('#ddlPostCategoryId').combotree({
                    data: arr
                });
                if (selectedId != undefined) {
                    $('#ddlPostCategoryId').combotree('setValue', selectedId);
                }
            }
        });
    }

    function setValueToNewPostCategory(parentId) {
        $.ajax({
            url: "/Admin/PostCategory/PrepareSetNewPostCategory",
            type: 'POST',
            dataType: 'json',
            data: {
                parentId: parentId
            },
            success: function (response) {
                $('#txtParentCategoryId').val(response.parentId);
                $('#txtSortOrder').val(response.sortOrder);
                $('#lblParentPostCategoryName').text(response.parentName);                
                $('#txtHomeOrder').val(response.homeOrder);
                $('#txtHotOrder').val(response.hotOrder); 
                //$('#txtOrder').val(response.order);
            }
        });
    }   

    function activeShowHome() {
        $('#ckShowHome').change(function () {
            if (this.checked) {
                $.ajax({
                    url: "/Admin/PostCategory/SetNewHomeOrder",
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
                    url: "/Admin/PostCategory/SetNewHotOrder",
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

    function loadData() {
        $.ajax({
            url: "/Admin/PostCategory/GetAll",
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

                arr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });

                $('#treePostCategory').tree({
                    data: arr,
                    dnd: true,
                    onDblClick: function (node) {
                        fillData(node.id);
                    },
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        // select the node
                        //$('#tt').tree('select', node.target);
                        $('#hidId').val(node.id);
                        // display context menu
                        $('#contextMenu').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    },
                    onDrop: function (target, source, point) {
                        console.log(target);
                        console.log(source);
                        console.log(point);
                        var targetNode = $(this).tree('getNode', target);
                        if (point === 'append') {
                            var children = [];
                            $.each(targetNode.children, function (i, item) {
                                children.push({
                                    key: item.id,
                                    value: i
                                });
                            });

                            $.ajax({
                                url: '/Admin/PostCategory/UpdateParentId',
                                type: 'post',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: children
                                },
                                success: function () {
                                    loadData();
                                }
                            });
                        }
                        else if (point === 'top' || point === 'bottom') {
                            $.ajax({
                                url: '/Admin/PostCategory/ReOrder',
                                type: 'post',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id
                                },
                                success: function () {
                                    loadData();
                                }
                            });
                        }
                    }
                });
            }
        });
    }

     
}