 <script>
        $("#SendButton").on("click", function (e) {
            e.preventDefault();
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
                },
            });
        });
    </script>
