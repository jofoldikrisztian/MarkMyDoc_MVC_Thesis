
$(document).ready(function () {
    $("#doctors").DataTable({
        "processing": true, // töltőcsík
        "serverSide": true, // szerver oldali feldolgozás
        "filter": true, // keresés
        "ordering": false,
        "bLengthChange": false,
        "ajax": {
            "url": "/Doctors/LoadDoctors",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": true
        }],
        "columns": [
            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "name", "name": "name", "autoWidth": true },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Doctors/Edit/' + full.id + '">Szerkesztés</a>'; }
            },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-danger" href="/Doctors/Delete/' + full.id + '">Törlés</a>'; }
            },
        ]

    });
});

