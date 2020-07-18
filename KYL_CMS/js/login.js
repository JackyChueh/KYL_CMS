var Login = {
    Action: null,
    FUNCTIONS: null,

    Page_Init: function () {
        this.Event_Binding();
        this.DataValidate();
    },

    Event_Binding: function () {
        $(".transparent").change(function () {
            $("#msg").hide();
        });
        $('#account').keyup(function () {
            $("#account").val(($("#account").val()).toUpperCase());
        });
        $('#password').keyup(function () {
            $("#password").val(($("#password").val()).toUpperCase());
        });
    },

    DataValidate: function () {
        $("#login").validate({
            rules: {
                // simple rule, converted to {required:true}
                account: "required",
                password: "required"
            },
            //messages: {
            //    account: "帳號要輸入喔!"
            //}
        });
    }
    

};