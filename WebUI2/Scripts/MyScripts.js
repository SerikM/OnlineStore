// selecting size of product
    $(document).ready(function () {
        $(".size").click(
            function ()
            {
                $(".size").removeClass("size-selected");
                $(this).addClass("size-selected");
            }
            );
    });

    // selecting color of product
    $(document).ready(function () {
        $(".color").click(
            function () {
                $(".color").removeClass("color-selected");
                $(this).addClass("color-selected");
            }
            );
    });



// adds an item to favourites
$(document).ready(function () {

    $("#products-div").on("click", ".add-to-fav", 
        function () {

            var id = $(this).children().first().attr("id");
            var myUrl = "/Products/AddFavourite";

            $.ajax(
                {
                    data: { "productId": id },
                    url: myUrl,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json"
                }
                )
                .done(function (msg)
                {
                    var userName = msg.UserName;
                    var result = msg.Result;
                    var link = '<a class="more-detail-link" href="/Products/WishList?UserName=' + userName + '&returnUrl=/Products/List">view my wish list</a>';
                    $('#' + id + 'link').replaceWith(link);
                })

                .fail(function (msg)
                {
                    alert(msg);
                })

                .always(function (msg)
                {

               });




        });
});




// displays the pop-up image of the product

$(document).ready(function () {

    $("#products-div").on("mouseenter", ".details-link",
        function() {
            var id = $(this).attr('id');
            var imId = id + "product-image-pop";
            $('#' + imId).fadeIn("slow", null);;
        });


    $("#products-div").on("mouseleave", ".details-link",
    function () {
        var id = $(this).attr('id');
        var imId = id + "product-image-pop";
        $('#' + imId).hide();

    });
});





$(document).ready(function () {

    $(".account-tab").click(function () {

        $(this).closest("tr")
            .next()
            .find(".account-tab-lower")
            .slideToggle("slow");
    })
});





// getting info from the database for the user's past orders
$(document).ready(
    function () {
        $("#past-orders").click(function () {

            $("#loading-img").show();
            var myUrl = "/Account/GetPastOrders";

            $.ajax({
                data: "",
                url: myUrl,
                contentType: "application/json; charset=utf-8",
                dataType: "html"
            })
              .done(function (msg) {
                  $("#loading-img").hide();
                  $("#past-orders-display").html(msg);
              })
              .fail(function () {
                  alert("fail");
              })
            .always(function () {
            });
        });
    });




// getting wish list for user personal page
$(document).ready(function () {
    $("#fav-list").click(function () {

        $("#loading-img-fav").show();
        var myurl = "/Products/GetWishListString";

        $.ajax(
            {
                data: "",
                url: myurl,
                contentType:"application/json; charset=utf-8",
                dataType:"html"
            }
            ).done(function (msg)
            {
                $("#loading-img-fav").hide();
                $("#fav-list-display").html(msg);
             }).fail().always();

    });

});




/// ajax- add to cart functionality

$(document).ready(
function () {
    $("#products-div").on("click", ".add-to-cart", function () {
        
        var size = $(".size-selected").html();
        var color = $(".color-selected").attr('id');
        var buttonId = $(this).attr('id');

        var butSel = "input#" + buttonId + ".add-to-cart";
        var imgId = buttonId + "loading-image";        
        $('#' + imgId).show();
        $(butSel).hide();
        var myUrl = "/Cart/AddItemAjax";

        $.ajax({
            data: { "ProductID": buttonId,"size": size , "color": color},
            url: myUrl,
            contentType: "application/json; charset=utf-8",
            dataType: "html"
        })

          .done(function (msg) {
              if (msg == "error") alert(msg);
              else {
                  $("#" + imgId).hide();
                  $(butSel).show();
                  $("#cart-values").html(msg);
               }
          })



          .fail(function (msg) {
              alert(msg);
              $("#" + imgId).hide();
              $(butSel).show();
          })



        .always(function () {

        });
    });
});