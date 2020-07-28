
    $(document).ready(function () {

        $("#search-glass").click(function () {

            var cont = $("#search-box").val();
            if (cont == '') {
                $("#search-box").show();
                $("#search-box").animate({

                    width: "100%"
                }, 2000, null);
            }
            else
            {
                $("#loading-img").show();

                var myUrl = "/Products/SearchDatabase";
                $.ajax({
                   
                    url: myUrl,
                    data: { searchWord: $("#search-box").val() },
                    contentType: "application/json; charset=utf-8",
                    dataType:"html"
                })
                  .done(function (msg) {

                      $("#loading-img").hide();
                      $(".pager").hide();
                      $(".products").hide();
                      $("#loading-photos-indic").hide();
                     
                      $("#search-results").html(msg);
                      $("#search-results").slideDown();
                      




                  })
                  .fail(function (msg) { alert("fail" + msg) })
                  .always(function (msg) { $("#search-box").val('') });
           
            }
        });


    });

////////////////////////////////
// enter press event handler

    $(document).ready(function () {

        $(document).keypress(function (e) {

            if (e.which == 13) {


    var cont = $("#search-box").val();
    if (cont == '') {
        $("#search-box").show();
        $("#search-box").animate({

            width: "100%"
        }, 2000, null);
    }
    else {
        $("#loading-img").show();

        var myUrl = "/Products/SearchDatabase";
        $.ajax({

            url: myUrl,
            data: { searchWord: $("#search-box").val() },
            contentType: "application/json; charset=utf-8",
            dataType: "html"
        })
          .done(function (msg) {

              $("#loading-img").hide();
              $(".pager").hide();
              $(".products").hide();
              $("#loading-photos-indic").hide();
              $("#search-results").html(msg);
              $("#search-results").slideDown();





          })
          .fail(function (msg) { alert("fail" + msg) })
          .always(function (msg) { $("#search-box").val('') });

    }


            }
        });
    });




















