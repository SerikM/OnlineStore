$(document).ready(function () {


    var endOfCatalogue = false;
    var addSize = 4;
    var callingBack = false;

    $("#PageSize").val(addSize);

    $("#loading-photos-indic").click(function () {

        ////////////////////////////////////
        if ((!endOfCatalogue) && (!callingBack)) {
            callingBack = true;

            $("#loading-photos-indic").html('<img src="/Content/MyImages/spinner.gif" />');

            var myUrl = "/Products/AjaxList";
            var cat = $("#Category").val();
            var subCat = $("#SubCategory").val();
            var skip = parseInt($("#PageSize").val());

            $.ajax({
                url: myUrl,
                data: { "skipSize": skip, "currentCategory": cat, "addSize": addSize, "subCategory": subCat },
                contentType: "application/json; charset=utf-8",
                dataType: "html"
            })


                .done(function (msg) {
                    var data = 1;
                    try {
                        data = parseInt(msg);
                    }
                    catch (Exception) { }

                    if (data < 1) {
                        endOfCatalogue = true;
                        $("#loading-photos-indic").html("end of catalogue");
                    }
                    else {
                        $("#products-div").append(msg);
                        skip += addSize;
                        $("#PageSize").val(skip);
                        $("#loading-photos-indic").html("load more");
                        $("body").addClass("scroll");
                    }
                })

                .fail(function (msg) { $("#loading-photos-indic").html("error") })

                .always(function () { callingBack = false; });
        }
        ///////////////////////////////////     
    });






    $(window).scroll(function () {

        if ($(window).scrollTop() == $(document).height() - $(window).height()) {

            ////////////////////////////////
            if ((!endOfCatalogue) && (!callingBack)) {

                // $("#loading-photos-indic").hide();
                callingBack = true;
                $("#loading-photos-indic").html('<img src="/Content/MyImages/spinner.gif" />');

                var myUrl = "/Products/AjaxList";
                var cat = $("#Category").val();
                var subCat = $("#SubCategory").val();
                var skip = parseInt($("#PageSize").val());

                $.ajax({
                    url: myUrl,
                    data: { "skipSize": skip, "currentCategory": cat, "addSize": addSize, "subCategory": subCat },
                    contentType: "application/json; charset=utf-8",
                    dataType: "html"
                })


                    .done(function (msg) {
                        var data = 1;
                        try {
                            data = parseInt(msg);
                        }
                        catch (Exception) { }

                        if (data < 1) {
                            endOfCatalogue = true;
                            $("#loading-photos-indic").html("end of catalogue");

                        }
                        else {
                            $("#products-div").append(msg);
                            skip += addSize;
                            $("#PageSize").val(skip);
                            $("#loading-photos-indic").html("load more");
                            $("body").addClass("scroll");
                        }
                    })

                    .fail(function (msg) { $("#loading-photos-indic").html("error") })

                    .always(function () {
                        callingBack = false;

                    });
            }
            ///////////////////////////////////

        }
    });
});











