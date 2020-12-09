var passwordController = function () {
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
                Content: { required: true },
                Level: { required: true },
                Order: { required: true },                
            }
        });

        $('#btnReset').on('click', function () {
            resetFormMaintainance();
        });

    };

    function resetFormMaintainance() {
        //disableFieldEdit(false);               
       
        $('#Content').val('');
        $('#Level').val('');
        $('#Order').val('');
      
    }
      
}