
$(document).ready(function () {
    $("#reviews").DataTable({
        "processing": true, // töltőcsík
        "serverSide": true, // szerver oldali feldolgozás
        "filter": true, // keresés
        "ordering": false,
        "bLengthChange": false,
        "ajax": {
            "url": "/Reviews/LoadUnApprovedReviews",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": true
        }],
        "columns": [
            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "userName", "name": "userName", "autoWidth": true },
            { "data": "title", "name": "title", "autoWidth": true },
            { "data": "doctor", "name": "doctor", "autoWidth": true },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Reviews/Approve/' + full.id + '">Jóváhagyás</a>'; }
            },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-danger" href="/Reviews/Delete/' + full.id + '">Törlés</a>'; }
            },
        ]

    });
});

