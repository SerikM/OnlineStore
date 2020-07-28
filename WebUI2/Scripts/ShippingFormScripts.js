


$(document).ready(function () {

    $("#ShipDet_Country").change(function () {

        var country = $("#ShipDet_Country").val();
      
        if (country === "Australia")
        {
            $('#ShipDet_State').replaceWith('<select class="state-drop" id="ShipDet_State" style="display:block;" name="ShipDet.State" data-val-required="Please select a state name" data-val="true"><option selected="selected">State Name</option><option>NSW</option><option>QLD</option><option>ACT</option><option>SA</option><option>WA</option><option>VIC</option><option>TAS</option><option>NT</option></select>');
                       
        }
        else 
        {
            $('#ShipDet_State').replaceWith('<input id="ShipDet_State" class="text-box single-line" type="text" value="" name="ShipDet.State" data-val-required="Please enter a state name" data-val="true"></input>');
            $('#auto-complete-div').hide();
            $("#ShipDet_StreetName").off('keyup');
            $("#ShipDet_PostCode").off('keyup');
            $("#ShipDet_Town").off('keyup');
            $("#ShipDet_StreetNumber").off('keyup');
            $("#ShipDet_StreetType").off('change');


        }
    });

});



////////////////////////
$(document).ready(function () {

    $("#editor-long").on('change', ".state-drop", function () {
   
        if ($('#ShipDet_State').val() === "NSW") {

            $("#ShipDet_StreetName").keyup(AutoCompleteAddress);
            $("#ShipDet_PostCode").keyup(AutoCompleteAddress);
            $("#ShipDet_Town").keyup(AutoCompleteAddress);
            $("#ShipDet_StreetNumber").keyup(AutoCompleteAddress);
            $("#ShipDet_StreetType").change(AutoCompleteAddress);
        }
        else {

            $("#ShipDet_StreetName").off('keyup');
            $("#ShipDet_PostCode").off('keyup');
            $("#ShipDet_Town").off('keyup');
            $("#ShipDet_StreetNumber").off('keyup');
            $("#ShipDet_StreetType").off('change');
            $('#auto-complete-div').hide();
        }
    });
});


function AutoCompleteAddress() {
    $('#auto-complete-div').slideDown("slow");

    var StreetName = $("#ShipDet_StreetName").val();
    var PostCode = $("#ShipDet_PostCode").val();
    var Town = $("#ShipDet_Town").val();
    var myUrl = '/Cart/GetShipAddr';
    var State = $('#ShipDet_State').val();
    var Country = $("#ShipDet_Country").val();
    var StreetType = $("#ShipDet_StreetType").val();
    var StreetNumber = $("#ShipDet_StreetNumber").val();

    $('#address-loading').show();

    $.ajax
        ({
            url: myUrl,
            contentType: 'application/json; charset=utf-8',
            data: {
                'StreetNumber': StreetNumber, 'StreetName': StreetName,
                'StreetType': StreetType, 'PostCode': PostCode, 'Town': Town, 'State': State, 'Country': Country
            },
            dataType: 'html'
        })

        .done(function (msg) {

            $('#status').removeClass();
            $('#status').html('Verify by clicking your address');
            $('#status').addClass("status");
            $('#addr-records').html(msg);
        })

        .fail(function () {
            $("#status").html("Connection failed");
            $("#status").addClass('fail-red').removeClass('status');
        })

        .always(function () { $('#address-loading').hide(); });
}






$(document).ready(function () {

    $("#auto-complete-div").on('click', ".address-record", function () {

        var id = $(this).attr('id');
        var Country = $('input[name=' + id + 'Country]').val();
        var State = $('input[name=' + id + 'State]').val();
        var StreetNumber = $('input[name=' + id + 'StreetNumber]').val();
        var StreetName = $('input[name=' + id + 'StreetName]').val();
        var PostCode = $('input[name=' + id + 'PostCode]').val();
        var Town = $('input[name=' + id + 'Town]').val();
        var StreetType = $('input[name=' + id + 'StreetType]').val();



        $('select[id = "ShipDet_StreetType"]').val(StreetType);
        $("#ShipDet_Country").val(Country);
        $('#ShipDet_State').val(State);
        $("#ShipDet_StreetNumber").val(StreetNumber);
        $("#ShipDet_StreetName").val(StreetName);
        $("#ShipDet_PostCode").val(PostCode);
        $("#ShipDet_Town").val(Town);


        $("#status").html("Your address has been verified");
        $("#status").removeClass();
        $("#status").addClass('confirm-green');
    });
});




$(document).ready(function () {

    $('form').submit(
       
    function(e)
    {    
        var country =   $("#ShipDet_Country").val();
        var state = $("#ShipDet_State").val();
        var strType = $("#ShipDet_StreetType").val();

        if (country === "Country Name") {
            $('#ShipDet_Country').css("background-color", '#e8bcbc');
            $('#ShipDet_Country').css("border-color", 'red');
            $('#ShipDet_Country').css("border-style", 'double');            
            $("#country").html('<li>Please select a country</li>');
            
            e.preventDefault();
        }
        else {
            $("#country").empty();
            $('#ShipDet_Country').css("background-color", '#FFF');
            $('#ShipDet_Country').css("border-color", '#CCC');
            $('#ShipDet_Country').css("border-style", 'solid');
             }

        if (strType === "Street Type") {

            $('#ShipDet_StreetType').css("background-color", '#e8bcbc');
            $('#ShipDet_StreetType').css("border-color", 'red');
            $('#ShipDet_StreetType').css("border-style", 'double');            
            $("#street").html('<li>Please select a street type</li>');          

            e.preventDefault();
        }
        else {
            $("#street").empty();
            $('#ShipDet_StreetType').css("background-color", '#FFF');
            $('#ShipDet_StreetType').css("border-color", '#CCC');
            $('#ShipDet_StreetType').css("border-style", 'solid');
             }


        if ((state === "State Name") || (!$('#ShipDet_State').val())) {
            $('#ShipDet_State').css("background-color", '#e8bcbc');
            $('#ShipDet_State').css("border-color", 'red');
            $('#ShipDet_State').css("border-style", 'double');          
            $("#state").html('<li>Please select a state</li>');          

            e.preventDefault();
        }
        else {
               $("#state").empty();      
               $('#ShipDet_State').css("background-color", '#FFF');
               $('#ShipDet_State').css("border-color", '#CCC');
               $('#ShipDet_State').css("border-style", 'solid');
             }


    })   
});







