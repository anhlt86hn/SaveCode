var jsController = function () {
    //var self = this;
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        //Init validation       

        $('body').on('click', '#btnEdit', function (e) {
            e.preventDefault();
            //var that = $(this).data('id');
            var that = $('#hidId').val();
            window.location.href = "/admin/function/update?id=" + that;
            //fillData(that);
        });

        $('body').on('click', '#btnDelete', function (e) {
            e.preventDefault();
            //var that = $(this).data('id');
            var that = $('#hidId').val();
            aspnetcore.confirm('Bạn có chắc chắn muốn xoá?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Function/Delete",
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
    }

    function loadData() {
        $.ajax({
            url: "/Admin/Function/GetAll",
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

                $('#treeList').tree({
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
                                url: '/Admin/Function/UpdateParentId',
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
                                url: '/Admin/Function/ReOrder',
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