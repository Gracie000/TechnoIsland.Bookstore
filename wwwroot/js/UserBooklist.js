var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/books/getall/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "title", "width": "20%" },
            { "data": "author", "width": "20%" },
            { "data": "publisher", "width": "20%" },
            {
                "data": "imageName", "render": function (data) {
                    return '<img src="../images/' + data + '" width = "100" height = "100" /> '
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/BorrowHistories/Create?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Borrow
                         </a>
                         &nbsp;
                         <a href="/BorrowHistories/Edit?id='+${data}" class='btn btn-primary text-white' style='cursor:pointer; width:70px;'>
                            Return
                         </a>
                         </div>`;
                }, "width": "40%"
            }
        ],
        "width": "100%"
    });
}
