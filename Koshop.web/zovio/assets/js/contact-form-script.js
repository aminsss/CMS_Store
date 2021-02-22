/*==============================================================*/
// Zovio Contact Form  JS
/*==============================================================*/
(function ($) {
    "use strict"; // Start of use strict
    $("#contactForm").validator().on("submit", function (event) {
        if (event.isDefaultPrevented()) {
            // handle the invalid form...
            formError();
            submitMSG(false, "آیا فرم را به درستی پر کرده اید؟");
        } else {
            // everything looks good!
            event.preventDefault();
            submitForm();
        }
    });


    function submitForm(){
        // Initiate Variables With Form Content
        //var name = $("#name").val();
        //var email = $("#email").val();
        //var msg_subject = $("#msg_subject").val();
        //var phone_number = $("#phone_number").val();
        //var message = $("#message").val();


        $.ajax({
            url: "/Home/GetMessage",
            dataType: "json",
            type: "POST",
            //contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            data: $("#contactForm").serialize(),
            //data: {
            //    name: $("#name").val(),
            //    email: $("#email").val(),
            //    phoneNumber: $("#phone_number").val(),
            //    msgSubject: $("#msg_subject").val(),
            //    message: $("#message").val(),
            //    __RequestVerificationToken: gettoken()
            //},
            async: true,
            cache: false,
            success: function (result) {
                if (result == true) {
                    alert("پیام شما با موفقیت ارسال شد");
                }
                else if (result == false) {
                    alert("ناموفق در ارسال");
                }
            },
            error: function () {
                alert("ناموفق در ارسال");
            },
            beforeSend: function () {
            }
        });
    }

    function formSuccess(){
        $("#contactForm")[0].reset();
        submitMSG(true, "ارسال شد!")
    }

    function formError(){
        $("#contactForm").removeClass().addClass('shake animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
            $(this).removeClass();
        });
    }

    function submitMSG(valid, msg){
        if(valid){
            var msgClasses = "h4 text-center tada animated text-success";
        } else {
            var msgClasses = "h4 text-center text-danger";
        }
        $("#msgSubmit").removeClass().addClass(msgClasses).text(msg);
    }
}(jQuery)); // End of use strict