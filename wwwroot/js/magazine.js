var dataTable
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {

    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/manager/magazine/getall'
        },
        "columns": [
            { data: 'name', "width": "15%", className: "text-center" },
            {
                data: 'startDate', "width": "10%", className: "text-center"
                render: function (data) {
                    var date = new Date(data);
                    var datestring = date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
                    return datestring;
                }
            },
            {
                data: 'endDate', "width": "10%", className: "text-center"
                render: function (data) {
                    var date = new Date(data);
                    var datestring = date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
                    return datestring;
                }
            },
            { data: 'faculty.name', "width": "20%", className: "text-center" },
            { data: 'semester.name', "width": "20%" className: "text-center" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="text-center">
                            <a href="/manager/magazine/edit/${data}" class="btn btn-success text-white" style="cursor:pointer; width:50px;">
                                <i class="fas fa-edit"></i> Edit
                            </a>
                            <a href="/manager/magazine/delete/${data}" class="btn btn-danger text-white" style="cursor:pointer; width:50px;">
                                <i class="fas fa-trash-alt"></i> Delete
                            </a>
                        </div>
                    `
                },
                "width": "15%"
            },
        ],
    });
}