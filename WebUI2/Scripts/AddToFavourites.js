$(document).ready(function () {

    function doneFunc() {
        alert("done");
    }



    $("#favourite").click(function () {

        var myUrl = "/Products/AddFavourite";
        var prodId = $("#favourite").val();

        $.ajax({

            url: myUrl,
            data: { "productId": prodId },
            dataType: "application/json, encoding=utf-8",
            contentType: "html"



        }).fail(function (message) { }).done(function (message) { alert(message); }).always(function (message) { });


    });

});
