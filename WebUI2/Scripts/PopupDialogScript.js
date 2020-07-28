
$(document).ready(function () { 
$().click(function (e) {

     ShowDialog(true);
     e.preventDefault();
});



$("#confirm").click(function (e) {

    $("#shipmentForm").submit();
    HideDialog();
});


$("#decline").click(function (e) {

    $("#shipmentForm").submit();
    HideDialog();
});



function ShowDialog() {

    $("#overlay").show();
    $("#dialog").fadeIn(300)
    $("overlay").unbind("click")
}

function HideDialog() {

    $("#overlay").hide();
    $("#dialog").fadeOut(300);
}

});
