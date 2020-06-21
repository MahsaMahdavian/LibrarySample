
var dataTable;

$(document).ready(function () {
    //loadDataTable();
    LoadTabaleManually();
    SarchWithAllItem();


});

function LoadTabaleManually() {
    $.ajax({
        type: "Get",
        url: "Book/GetAllBooks",
        datatype: "Json",
        contentType: "application/json",
        success: function (result) {
            var trHTML = '';
            $.each(result.data, function (i, item) {
                var $tr = $('<tr>').append(
                    $('<td>').text(item.title),
                    $('<td>').text(item.summary),
                    $('<td>').text(item.numOfPages),
                    $('<td>').text(item.isbn),
                    $('<td>').text(item.insertDate),
                    $('<td>').text(item.language),
                    $('<td>').text(item.author),
                    $('<td>').text(item.translator),
                    $('<td>').append(function () {
                        return `<div class="text-center">
                              
                                <a  href="Book/Details/${item.id}" class='btn btn-primary text-white' style='cursor:pointer; width:100px;'>
                                    <i class='fas fa-info'></i> جزئیات
                                </a>
                            </div>
                            `;
                    })                
                ).appendTo('#myTable');
            });
        }
    });
}

function SarchWithAllItem() {
    $("#txtSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
}
function loadDataTable() {

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "Book/GetAllBooks",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "title", "width": "50%" },
            { "data": "summary", "width": "50%" },
            { "data": "numOfPages", "width": "20%" },
            { "data": "isbn", "width": "50%" },
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