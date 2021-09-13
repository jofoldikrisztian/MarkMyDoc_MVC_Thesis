// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var x = document.getElementById("search-box");

if (x != null) {

    x.addEventListener("focus", addFocusClass, true);
    x.addEventListener("blur", removeFocusClass, true);

    function addFocusClass() {
        x.className += "onFocus";
    }

    function removeFocusClass() {
        x.className = "";
    }
}


$(function () {
    $("#input-text").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Home/Search/',
                data: { "toSearch": request.term },
                type: "POST",
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });

        },
        select: function (e, i) {
            $("#hfCity").val(i.item.val);
        },
        minLength: 2,
        appendTo: '#search-box',
    });
});

//#region Oldal töltő 

$('#btnSubmit, #btnDoctors, .buttons, #navButton').click(function () {
    $('.spinner').css('display', 'block');
});

//#endregion

$(document).ready(function () {
    $('.select2-mmd').select2(
        {
            placeholder: "Válassz szakterületeket!",
            allowClear: true
        }
    );
});
