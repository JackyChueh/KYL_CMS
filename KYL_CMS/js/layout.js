$(document).ready(function () {
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });

    $('#quit').click(function () {
        document.location.href = '/Main/Login'
    });
});
