var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/account/getall' },
        "columns": [
            { "data": 'fullName', "width": "25%" },
            { "data": 'faculty.name', "width": "25%" },
            { "data": 'role', "width": "20%" },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {
                        return `
                        <div class="text-center">
                             <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:120px;">
                                    <i class="bi bi-lock-fill"></i>Lock
                              </a>                              
                        </div>
                    `
                    }
                    else {
                        return `
                        <div class="text-center">
                              <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:120px;">
                                    <i class="bi bi-unlock-fill"></i>UnLock
                               </a>
                        </div>
                    `
                    }
                },
                "width": "30%"
            }
        ]
    });
}


function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/Account/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                dataTable.ajax.reload();
            }
        }
    });
}
