var loginController = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {

        $('#frmLogin').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            //lang: 'en',
            rules: {
                UserName: {
                    required: true
                },
                Password: {
                    required: true
                }
            }
        });

        //$('#btnLogin').keypress(function (e) {
        //    if (e.which == 13) {
        //        e.preventDefault();
        //        var user = $('#txtUserName').val();
        //        var password = $('#txtPassword').val();
        //        login(user, password);
        //    }
        //});

        //$('#btnLogin').on('click', function (e) {
        //    if ($('#frmLogin').valid()) {
        //        e.preventDefault();
        //        var user = $('#txtUserName').val();
        //        var password = $('#txtPassword').val();
        //        login(user, password);
        //    }
        //});

        $('#frmLogin').on('submit', (e) => {
            e.preventDefault();
            var user = $('#txtUserName').val();
            var password = $('#txtPassword').val();
            login(user, password);
        });

        $('#btnLogin').keypress(function (e) {
            if (e.which == 13) {
                e.preventDefault();

                login(user, password);
            }
        });
    }
    

    var login = function (user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                Email: user,
                Password: pass
            },
            dateType: 'json',
            url: '/admin/login/authen',
            success: function (res) {
                if (res.Success) {                   
                    window.location.href = "/Admin/Home/Index";
                }
                else {                    
                    aspnetcore.notify('Login failed', 'error');                    
                }
            }
        })
    }
}