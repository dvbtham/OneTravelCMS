$(function() {
    alertShow();
    stickyPanel();
    datetimePicker();
    $(".select2").select2({
        "language": "vi-VN"
    });
});

function alertShow() {
    const alert = $("#alert");
    if (alert.data("val")) {
        $.toast().reset("all");
        $("body").removeAttr("class").removeClass("bottom-center-fullwidth").addClass("top-center-fullwidth");
        $.toast({
            heading: "Thành công",
            text: "Dữ liệu đã được lưu.",
            position: "top-center",
            loaderBg: "#4dd4a8",
            icon: "success",
            hideAfter: 3500
        });
    }
}

function datetimePicker() {

    $('.input-daterange-datepicker').daterangepicker({
        buttonClasses: ['btn', 'btn-sm'],
        applyClass: 'btn-primary',
        cancelClass: 'btn-danger',
        locale: {
            applyLabel: "Áp dụng",
            cancelLabel: "Hủy",
            format: 'DD/MM/YYYY'
        }
    });
    $("#datetimepicker").datetimepicker({
        format: "DD/MM/YYYY",
        useCurrent: true,
        ignoreReadonly: true,
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down"
        }
    }).on("dp.show", function () {
        if ($(this).data("DateTimePicker").date() === null)
            $(this).data("DateTimePicker").date(moment());
    });
}

function stickyPanel() {
    const panelHeading = $("#panel-heading");
    if (!panelHeading) return;

    panelHeading.sticky({ topSpacing: 65, responsiveWidth: true });
    panelHeading.on("sticky-start", function () {
        $(this).addClass("sticky-heading");
    });
    panelHeading.on("sticky-end", function () {
        $(this).removeClass("sticky-heading");
    });
}