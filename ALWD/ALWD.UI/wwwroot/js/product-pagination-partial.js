$(document).ready(function () {
    $(document).on('click', '.page-link', function (event) {
        event.preventDefault();

        var url = $(this).attr('href'); 

        $.ajax({
            url: url,
            type: 'GET',
            success: function (result) {
                $('#productArea').html(result);
            },
            error: function (xhr, status, error) {
                console.error("Data receiving error:", error);
            }
        });
    });
});
