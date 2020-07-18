var Force = {
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
        $('#PASSWORD').keyup(function () {
            $("#PASSWORD").val(($("#PASSWORD").val()).toUpperCase());
        });
        $('#PASSWORD2').keyup(function () {
            $("#PASSWORD2").val(($("#PASSWORD2").val()).toUpperCase());
        });
    },

    DataValidate: function () {
        $("#force").validate({
            rules: {
                // simple rule, converted to {required:true}
                PASSWORD: {
                    minlength: 4
                },
                PASSWORD2: {
                    minlength: 4,
                    equalTo: "#PASSWORD"
                }
            },
            //messages: {
            //    account: "帳號要輸入喔!"
            //}
        });
    }

};