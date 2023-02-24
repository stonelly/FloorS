$(document).ready(function () {

   
    $('#txtfrom').datepicker({
        changeMonth: true,
        changeYear: true,
        showOn: "button",
        buttonImage: "/images/calendar.png",
        buttonImageOnly: true,
        dateFormat: 'dd/mm/yy',
        maxDate: new Date(),
        beforeShow: function () {
            $(".ui-datepicker").css('font-size', 12)
        },
        onClose: function (selectedDate) {
            $("#txtTo").datepicker("option", "minDate", selectedDate);
        }
    });


    $('#txtTo').datepicker({
        changeMonth: true,
        changeYear: true,
        showOn: "button",
        buttonImage: "/images/calendar.png",
        buttonImageOnly: true,
        dateFormat: 'dd/mm/yy',
        maxDate: new Date(),
        beforeShow: function () {
            $(".ui-datepicker").css('font-size', 12)
        },
        onClose: function (selectedDate) {
            $("#txtfrom").datepicker("option", "maxDate", selectedDate);
        }
    });

    $("tr:last").addClass("footerGrid");


});

function validateSub() {
    var mes = '';
    var error = 0;
    if ($('#txtfrom').val() == '') {
        error = 1;
        mes = "Please select Start date<br/>";
    }
    else {
        if (!validatedate($('#txtfrom').val())) {
            error = 1;
            mes = mes + "Invalid Start date<br/>";
        }
    }
    if ($('#txtTo').val() == '') {
        error = 1;
        mes = mes + " Please select End date";
    }
    else {
        if (!validatedate($('#txtTo').val())) {
            error = 1;
            mes = mes + "Invalid End date<br/>";
        }
    }

    if (error) {
        $('#validation_msg').html(mes);
        return false;
    } else {
        $('#validation_msg').empty();
        return true;
    }
}

function validatedate(intdate) {
    var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
    var Val_date = intdate
    if (Val_date.match(dateformat)) {
        var seperator1 = Val_date.split('/');
        var seperator2 = Val_date.split('-');

        if (seperator1.length > 1) {
            var splitdate = Val_date.split('/');
        }
        else if (seperator2.length > 1) {
            var splitdate = Val_date.split('-');
        }
        var dd = parseInt(splitdate[0]);
        var mm = parseInt(splitdate[1]);
        var yy = parseInt(splitdate[2]);
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (mm == 1 || mm > 2) {
            if (dd > ListofDays[mm - 1]) {
                //alert('Invalid date format!');
                return false;
            }
            return true;
        }
        if (mm == 2) {
            var lyear = false;
            if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
                lyear = true;
            }
            if ((lyear == false) && (dd >= 29)) {
                //alert('Invalid date format!');
                return false;
            }
            if ((lyear == true) && (dd > 29)) {
                // alert('Invalid date format!');
                return false;
            }
        }
        return true;
    }
    else {
        // alert("Invalid date format!");

        return false;
    }
}