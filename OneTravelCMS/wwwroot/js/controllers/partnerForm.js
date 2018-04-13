$(function () {

    $("#example").DataTable({
        'lengthMenu': [[10, 20, 30, 50, -1], [10, 20, 30, 50, "Tất cả"]],
        "paging": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        "language": {
            sProcessing: "Đang xử lý...",
            sLengthMenu: "Xem _MENU_ mục",
            sZeroRecords: "Không tìm thấy dòng nào phù hợp",
            sInfo: "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
            sInfoEmpty: "Đang xem 0 đến 0 trong tổng số 0 mục",
            sInfoFiltered: "(được lọc từ _MAX_ mục)",
            sInfoPostFix: "",
            sSearch: "Tìm:",
            sUrl: "",
            oPaginate: {
                sFirst: "Đầu",
                sPrevious: "Trước",
                sNext: "Tiếp",
                sLast: "Cuối"
            }
        }
    });

    // Add event listener for opening and closing details
    $('#example tbody').on('click',
        'td.details-control',
        function () {
            var tr = $(this).closest('tr');
            var row = table.row(tr);

            if (row.child.isShown()) {
                // This row is already open - close it
                row.child.hide();
                tr.removeClass('shown');
            } else {
                // Open this row
                row.child(format(row.data())).show();
                tr.addClass('shown');
            }
        });

    $("#example").on("click", ".add-partner-contact",
        function (e) {
            const el = $(e.target);
            $(".partner-contact-form-title").text("Thêm mới liên hệ");
            $("#idPartner").val(el.data("partner"));
            $("#id").val(0);
            $("#contactName").val("");
            $("#email").val("");
            $("#mobile").val("");
            $("#positionTitle").val("");
            $("#note").val("");
            $("#partner-contact-modal").modal();
        });

    $(".add-partner-contact").on("click",
        function (e) {
            const el = $(e.target);
            console.log(el.data("partner"));
            $(".partner-contact-form-title").text("Thêm mới liên hệ");
            $("#idPartner").val(el.data("partner"));
            $("#id").val(0);
            $("#contactName").val("");
            $("#email").val("");
            $("#mobile").val("");
            $("#positionTitle").val("");
            $("#note").val("");
            $("#partner-contact-modal").modal();
        });

    var fillDataOnEdit = function (callback) {
        $.ajax({
            url: `/OneOperator/PartnerContact/Get/${$("#id").val()}`,
            type: "get",
            success: (res) => {
                callback(res.data);
            },
            error: (res) => {
                console.log(res);
                swal("Lỗi!", `Đã xảy ra lỗi.`, "error");
            }
        });
    };

    $("#example").on("click", ".table-edit",
        function (e) {
            e.preventDefault();
            const el = $(e.target);
            $(".partner-contact-form-title").text("Cập nhật liên hệ");
            $("#idPartner").val(el.data("partner"));
            $("#id").val(parseInt(el.data("id")));
            $("#partner-contact-modal").modal();
            fillDataOnEdit((res) => {
                $("#contactName").val(res.contactName);
                $("#email").val(res.email);
                $("#mobile").val(res.mobile);
                $("#positionTitle").val(res.positionTitle);
                $("#note").val(res.note);
            });
        });

    $("#example").on("click", ".table-delete",
        function (e) {
            const el = $(e.target);
            swal({
                title: "Bạn có chắc chắn xóa?",
                text: "Bạn sẽ không thể phục hồi lại khi xóa!",
                type: "warning",
                cancelButtonText: "Hủy",
                showCancelButton: true,
                confirmButtonColor: "#d9534f",
                confirmButtonText: "Có, Tôi muốn xóa!",
                closeOnConfirm: false
            }, function () {

                $.ajax({
                    url: `/OneOperator/PartnerContact/Delete/${el.attr("data-id")}`,
                    method: "delete",
                    success: function (res) {
                        el.parents("tr:first").fadeOut(function () {
                            $(this).remove();
                        });
                        swal(`${res.title}`, `${res.message}`, "success");
                    },
                    error: function (error) {
                        console.log(error);
                        swal("Opps!", "Đã xảy ra lỗi trong quá trình thực thi yêu cầu.", "error");
                    }
                });


            });
        });

    $("#example").on("click", ".delete-item",
        function (e) {
            const el = $(e.target);
            swal({
                title: "Bạn có chắc chắn xóa?",
                text: "Bạn sẽ không thể phục hồi lại khi xóa!",
                type: "warning",
                cancelButtonText: "Hủy",
                showCancelButton: true,
                confirmButtonColor: "#d9534f",
                confirmButtonText: "Có, Tôi muốn xóa!",
                closeOnConfirm: false
            }, function () {

                $.ajax({
                    url: `/OneOperator/Partner/Delete/${el.attr("data-id")}`,
                    method: "delete",
                    success: function (res) {
                        if (res.status) {
                            el.parents("tr").fadeOut(function () {
                                $(this).remove();
                            });
                            swal(`${res.title}`, `${res.message}`, "success");
                        } else {
                            swal(`${res.title}`, `${res.message}`, "error");
                        }

                    },
                    error: function (error) {
                        console.log(error);
                        swal("Opps!", "Đã xảy ra lỗi trong quá trình thực thi yêu cầu.", "error");
                    }
                });


            });
        });

    $("#contactName").on("change",
        function () {
            if ($(this).val() === "") {
                $("#contactNameEmplty").removeClass("hidden");
            }
            else $("#contactNameEmplty").addClass("hidden");
        });

    $("#partner-contact-save").on("click",
        function () {
            var formData = {
                id: parseInt($("#id").val()),
                contactName: $("#contactName").val(),
                positionTitle: $("#positionTitle").val(),
                mobile: $("#mobile").val(),
                email: $("#email").val(),
                note: $("#note").val(),
                idPartner: $("#idPartner").val()
            };
            console.log(formData);

            if (formData.contactName === "") {
                $("#contactNameEmplty").removeClass("hidden");
                return;
            }

            $.ajax({
                url: "/OneOperator/PartnerContact/Create",
                type: "post",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(formData),
                success: (res) => {
                    console.log(res.data);
                    swal(`${res.title}`, `${res.message}`, "success");
                    const html = `<tr><td></td><td></td>
                            <td class="contactName_${res.data.id}">${formData.contactName}</td>
                            <td class="positionTitle_${res.data.id}">${formData.positionTitle}</td>
                            <td class="mobile_${res.data.id}">${formData.mobile}</td>
                            <td class="email_${res.data.id}">${formData.email}</td>
                            <td><a class="table-edit" href="javascript:void(0)"
                                    data-partner="${formData.idPartner}"
                                    data-id="${res.data.id}">
                                    <i class="fa fa-edit text-success"></i> Sửa</a>
                                    | <a class="table-delete" href="javascript:void(0)"
                                        data-id="${res.data.id}">
                                    <i class="fa fa-trash text-danger"></i> Xóa</a></td>
                       </tr>`;
                    if (formData.id === 0)
                        $("#partnerContactBody").append(html);
                    else {
                        $(`.contactName_${res.data.id}`).text(res.data.contactName);
                        $(`.positionTitle_${res.data.id}`).text(res.data.positionTitle);
                        $(`.mobile_${res.data.id}`).text(res.data.mobile);
                        $(`.email_${res.data.id}`).text(res.data.email);
                    }

                    $("#partner-contact-modal").modal("hide");
                },
                error: (res) => {
                    console.log(res);
                    swal("Lỗi!", `Đã xảy ra lỗi khi thêm liên hệ.`, "error");
                }
            });
        });
});