
$(document).ready(function () {
    $("#users").DataTable({
        "processing": true, // töltőcsík
        "serverSide": true, // szerver oldali feldolgozás
        "filter": true, // keresés
        "ordering": false,
        "bLengthChange": false,
        "ajax": {
            "url": "/Users/LoadUsers",
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
            { "data": "email", "name": "email", "autoWidth": true },
            { "data": "roles", "name": "roles", "autoWidth": true },
            { "data": "isLockedOut", "name": "isLockedOut", "autoWidth": true },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Users/Manage/' + full.id + '">Szerepkörök kezelése</a>'; }
            },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Users/Edit/' + full.id + '">Szerkesztés</a>'; }
            },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-danger" href="/Users/LockoutUser/' + full.id + '">Tiltás</a>'; }
            },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-danger" href="/Users/Delete/' + full.id + '">Törlés</a>'; }
            }
        ]

    });
});

