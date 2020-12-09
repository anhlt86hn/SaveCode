var ChangePasswordController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        //Init validation       

        $('#frmChangePassword').valid({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtPassword: { required: true, minlength: 6 },
                txtConfirmPassword: { required: true, minlength: 6, equalTo: "#txtPassword" },
            },
            message: {
                txtPassword: { required: "Phải nhập mật khẩu", minlength: "Mật khẩu phải ít nhất 6 ký tự" },
                txtConfirmPassword: { required: "Phải nhập Xác nhận mật khẩu", minlength: "Mật khẩu phải ít nhất 6 ký tự", equalTo: "Phải nhập mật khẩu và Xác nhận mật khẩu giống nhau" },
            }
        });


        //$('a[href="#changePassword"]').click(function () {
        //    resetChangePassword();
        //    $('#modal-change-password').modal('show');
        //});

        $('#btnCancelPassword').on('click', function () {
            resetChangePassword();
        });

        $('#btnSaveChangePassword').on('click', function (e) {
            if ($('#frmChangePassword').valid()) {
                e.preventDefault();
                var id = $('#hidIdPassword').val();
                var userName = $('#hidUserName').val();
                var password = $('#txtPassword').val();
                var confirmPassword = $('#txtConfirmPassword').val();
                $.ajax({
                    type: "POST",
                    url: "/Admin/User/ChangePassword",
                    data: {
                        Id: id,
                        Password: password,
                        UserName: userName
                    },
                    dataType: "json",
                    beforeSend: function () {
                        aspnetcore.startLoading();
                    },
                    success: function () {
                        aspnetcore.notify('Đổi mật khẩu tài khoản thành công', 'success');
                        $('#modal-change-password').modal('hide');
                        resetChangePassword();
                        aspnetcore.stopLoading();
                    },
                    error: function () {
                        aspnetcore.notify('Đổi mật khẩu tài khoản không thành công', 'error');
                        aspnetcore.stopLoading();
                    }

                });
            }
            return false;
        });

        function resetChangePassword() {
            $('#hidIdPassword').val("00000000-0000-0000-0000-000000000000");
            $('#txtPassword').val('');
            $('#txtConfirmPassword').val('');
        }
    };
}