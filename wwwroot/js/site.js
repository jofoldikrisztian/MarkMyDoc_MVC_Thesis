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


function CreateOut(rating, ratingAspect) {
    for (var i = 1; i <= rating; i++) {
        $("#" + ratingAspect + i).attr('class', 'far fa-star');
    }
}

function CreateOver(rating, ratingAspect) {
    for (var i = 1; i <= rating; i++) {
        $("#" + ratingAspect + i).attr('class', 'fas fa-star');
    }
}

function CreateClick(rating, ratingAspect) {
    $("#" + ratingAspect + "Rating").val(rating);
    for (var i = 1; i <= rating; i++) {
        $("#" + ratingAspect + i).attr('class', 'fas fa-star');
    }

    for (var i = rating + 1; i <= 5; i++) {
        $("#" + ratingAspect + i).attr('class', 'far fa-star');
    }
}

function CreateSelected(ratingAspect) {
    var rating = $("#" + ratingAspect + "Rating").val();
    for (var i = 1; i <= rating; i++) {
        $("#" + ratingAspect + i).attr('class', 'fas fa-star');
    }
}

function VerifyRating(ratingAspect) {
    var ProfessionalismRating = $("#ProfessionalismRating").val();
    var CommunicationRating = $("#CommunicationRating").val();
    var EmpathyRating = $("#EmpathyRating").val();
    var HumanityRating = $("#HumanityRating").val();
    var TrustAtmosphereRating = $("#TrustAtmosphereRating").val();
    if (ProfessionalismRating == "0" || CommunicationRating == "0" || EmpathyRating == "0" || HumanityRating == "0" || TrustAtmosphereRating == "0") {
        alert("Kérlek tölts ki minden értékelést!");
        return false;
    }
}


/* TextArea autosize*/


!function (t, e, i, n) {
    function s(e, i)
    {
        this.element = e, this.$element = t(e), this.init()
    }
    var h = "textareaAutoSize", o = "plugin_" + h, r = function (t)
    {
        return t.replace(/\s/g, "").length > 0
    };
    s.prototype = {
        init: function () {
            var i = parseInt(this.$element.css("paddingBottom")) + parseInt(this.$element.css("paddingTop")) + parseInt(this.$element.css("borderTopWidth")) + parseInt(this.$element.css("borderBottomWidth")) || 0;
            r(this.element.value) && this.$element.height(this.element.scrollHeight - i),
                this.$element.on("input keyup", function (n) {
                    var s = t(e), h = s.scrollTop();
                    t(this).height(0).height(this.scrollHeight - i), s.scrollTop(h)
                })
        }
    }, t.fn[h] = function (e) { return this.each(function () { t.data(this, o) || t.data(this, o, new s(this, e)) }), this }
}(jQuery, window, document);

// Initialize Textarea
$('.textarea-autosize').textareaAutoSize();





/* end TextArea autosize*/