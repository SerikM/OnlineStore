
$(document).ready(function () {

    $('form').submit(

    function (e) {
        var card = $("#CardType").val();
        var month = $("#months").val();
        var year = $("#years").val();

        if (card == "Select") {
            $("#CardType").css("background-color", '#e8bcbc');
            $("#CardType").css("border-color", 'red');
            $("#CardType").css("border-style", 'double');
            $("#card").html('<li>Please select a card type</li>');

            e.preventDefault();
        }
        else {
            $("#card").empty();
            $("#CardType").css("background-color", '#FFF');
            $("#CardType").css("border-color", '#CCC');
            $("#CardType").css("border-style", 'solid');
        }

        if (month == "month") {

            $("#months").css("background-color", '#e8bcbc');
            $("#months").css("border-color", 'red');
            $("#months").css("border-style", 'double');
            $("#month").html('<li>Please select card expiry month</li>');

            e.preventDefault();
        }
        else {
            $("#month").empty();
            $("#months").css("background-color", '#FFF');
            $("#months").css("border-color", '#CCC');
            $("#months").css("border-style", 'solid');
        }


        if (year == "year") {
            $("#years").css("background-color", '#e8bcbc');
            $("#years").css("border-color", 'red');
            $("#years").css("border-style", 'double');
            $("#year").html('<li>Please select a state</li>');

            e.preventDefault();
        }
        else {
            $("#year").empty();
            $("#years").css("background-color", '#FFF');
            $("#years").css("border-color", '#CCC');
            $("#years").css("border-style", 'solid');
        }


    })
});







