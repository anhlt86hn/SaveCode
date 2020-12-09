var passwordController = function () {
    //var self = this;    
    this.initialize = function () {
        registerEvents();             
        setInitialOrder();
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
                //txtMetaKeywords: { required: true },               
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

    function setInitialOrder() {       
        setValueToNewPassword();
    }

    function setValueToNewPassword() {
        $.ajax({
            url: "/Admin/Password/SetNewOrder",
            type: 'POST',
            dataType: 'json',            
            success: function (response) {
                $('#Order').val(response.order);                           
            }
        });
    }   
   
}