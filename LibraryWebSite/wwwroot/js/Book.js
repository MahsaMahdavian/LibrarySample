
var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Book/GetAllBooks",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "title", "width": "50%" },
            { "data": "summary", "width": "50%" },
            { "data": "numOfPages", "width": "20%" },
            { "data": "iSBN", "width": "50%" },
            { "data": "insertDate", "width": "20%" },          
            { "data": "language", "width": "20%" },
            { "data": "author", "width": "20%" },
            { "data": "translator", "width": "20%" },
           
        ],
        "language": {
            "emptyTable": "No records found."
        },
        "width": "100%"
    });
}