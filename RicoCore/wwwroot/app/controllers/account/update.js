var accountController = function () {
    //var self = this;    
    this.initialize = function () {
        registerEvents();     
    }

    function registerEvents() {
        //Init validation
        $('#frmUpdate').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                Level: { required: true },
                Domain: { required: true },
                UserName: { required: true },
                PasswordId: { required: true },
                Order: {
                    required: true,
                    number: true
                },
            }
        });

        $('#btnReset').on('click', function () {
            resetFormMaintainance();
        });


    };


    function resetFormMaintainance() {
        //disableFieldEdit(false);                     
        $('#Level').val('');
        $('#Domain').val('');
        $('#UserName').val('');
        $('#PasswordId').val('');
        $('#UserName').val('');
        $('#Phone').val('');
        $('#SecurityEmail').val('');
        $('#Url').val('');
        $('#Note').val('');


    function setInitialOrder() {
        setValueToNewPassword();
    }

    function setValueToNewPassword() {
        $.ajax({
            url: "/Admin/Account/SetNewOrder",
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                $('#Order').val(response.order);
            }
        });
    }

}