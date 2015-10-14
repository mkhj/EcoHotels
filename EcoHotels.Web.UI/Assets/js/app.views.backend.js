/*
*   Backend views
*
*   
*/

/*
HOTEL
*/
App.Views.hotel = {
    init: function () { },
    create: function () {
        $('form.ajaxform').formPost({
            success: function (response) {
                location.href = '/admin/hotel/photos?id=' + response.data.id;
            }
        });
    },
    edit: function () {

        $('#roomtype-assets').on('click', 'li', function () {
            var $file = $(this);
            if ($file.hasClass('active')) {
                $file.removeClass('active');
            } else {
                $file.addClass('active');
            }
        });

        $('#btn-remove-files').on('click', function () {
            $('#roomtype-assets li.active').remove();
        });

        $('form.ajaxform').formPost(); 
    }
};

/*
USER
*/
App.Views.user = {
    init: function () {
        $('#btn-generate-password').on('click', function (e) {
            e.preventDefault();
            $('#Password').val(App.Util.password(8).toLowerCase())
        });
    },
    create: function () {
        $('form.ajaxform').formPost();
    },
    edit: function () {
        $('form.ajaxform').formPost({
            deleteUrl: '/backend/user/delete',
            editUrl: '/backend/user/edit'
        });
    }
}