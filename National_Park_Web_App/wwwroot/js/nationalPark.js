var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tbldata').DataTable({
        "ajax": {
            "url": "NationalPark/GetAll",
            "type": "GET",
            "dataSrc":"data"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "state", "width": "15%" },
           
            
              

            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class ="text-center">
                    <a href ="NationalPark/Upsert/${data}" class ="btn btn-info">
                    <i class = "fas fa-edit"></i>
                    </a>
                    <a class= "btn btn-danger" onclick = Delete('NationalPark/Delete/${data}')>
                    <i class = "fas fa-trash-alt "></i>
                    </a>
                    </div>  
                    `;
                }
            }
        ]
    })
}

function Delete(url) {
    swal({
        title: "Want To Delete Data?",
        text: "Delete Information !!!",
        icon: "warning",
        dangerModel: true,
        buttons: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else
                        toastr.error(data.message);
                }
            })
        }
    }
    )
}