/*
*   Administration views
*
*   
*/

/*
    ACCOUNTS
*/
App.Views.account = {
    init: function () { },
    logon: function () {
        console.log('sdfs');
        $('input[name=email]').focus();
    }
};


/*
    ADDONS
*/
App.Views.addon = {
    init: function () { },
    create: function () {
        $('form.ajaxform').formPost();
    },
    edit: function () {
        $('form.ajaxform').formPost({
            deleteUrl: '/admin/addon/delete'
        });
    }
};

/*
    AMENITY
*/
App.Views.amenity = {
    init: function () { },
    create: function () {
        $('form.ajaxform').formPost();
    },
    edit: function () {
        $('form.ajaxform').formPost({
            deleteUrl: '/admin/amenities/delete'
        });
    }
};

/*
    CURRENCY
*/
App.Views.currency = {
    init: function () { },
    edit: function () {
        $('form.ajaxform').formPost();
    },
    refresh: function () {
        $('form.ajaxform').formPost();
    }
};

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
    edit: function () { $('form.ajaxform').formPost(); },
    photos: function () {
        // events
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

        $('#btn-remove-files').on('click', function () {
            $('#roomtype-assets li.active').remove();
        });


        // init components
        $('form.ajaxform').formPost();

        $('#model-media').modal({
            show: false,
            backdrop: true,
            keyboard: true
        }).css({
            width: '900px',
            'margin-top': function(){ return ($(this).outerHeight()/2)*-1;},
            'margin-left': function () { return -($(this).width() / 2); }
        });

        $('#model-media').mediaSelector();
    },
    details: function(){
        console.log('sdfsdf');

        // init components
        $('form.ajaxform').formPost();
    }
};

App.Views.inventory = {
    init: function () { },
    edit: function () {
            var items = [],
                authtoken = $('#view').find('input[name="__RequestVerificationToken"]').val();
            
            var $qtyEditor = $('#qty-editor'),
                $newQty = $('#new-qty');
            
            var hideEditor = function(){
                $('td.ui-selected').removeClass('ui-selected');
                $qtyEditor.hide();
                $newQty.blur();
                items = [];
            }   

            var updateInventoryCells = function(){
                for (var i = 0, item; item = items[i++]; ) {
                    item.value = $newQty.val();
                }            
            }
            
            var updateInventoryView = function(){
                var params = {
                    '__RequestVerificationToken': authtoken,
                    'hotelId': $('#hotel-selector').val(),
                    'month' : $('#month-selector').val(),
                    'year' : $('#year-selector').val()
                };
                
                $.post('/admin/inventory/list', params)
                    .success(function (response, status, xhr) {
                        console.log(response);
                        $('#room-overview').html(response);
                    })
                    .error(function (data, status, msg) {
                        App.Util.statusMessage($status, errorThrown, 'alert-error');
                    })
                    .complete(function () {
                        
                    });            
            }         
            
            $("#rooms-qty tbody").selectable({
                filter: "td",
                cancel: "select, option, a",
                start: function () {
                    items = [];
                    $qtyEditor.hide();
                    $newQty.val('');
                },
                selected: function (event, ui) {
                    items.push(ui.selected.children[0]); 
                },
                stop: function (event, ui) {
                    if (items.length == 1) {
                        items[0].focus();
                    }else{
                        var x = event.pageX - $(this).offset().left;
                        var y = event.pageY - $(this).offset().top;

                        $qtyEditor.css({
                            top: y + 'px',
                            left: x + 'px'
                        }).show();

                        $newQty.focus();
                    }
                }
            });

            $('#rooms-qty').on('change', ':input', function (e) {
                
            });

            $('#btn-accept-qty').on('click', function (e) {
                e.preventDefault();
                updateInventoryCells();
                hideEditor();
            });

            $newQty.on('keyup', function(e) {
                var keycode = (e.keyCode || e.which);
	            if(keycode == 13) {
                    updateInventoryCells();
                    hideEditor();
	            }
            });

            $('#btn-cancel-qty').on('click', function (e) {
                e.preventDefault();
                hideEditor();
            });

            $('#hotel-selector, #month-selector, #year-selector').on('change', function (e) {
                updateInventoryView();
            });


    }
};

/*
MEDIA
*/
App.Views.media = {
    init: function () { },
    create: function () {
        $('form.ajaxform').formPost({
            editUrl: '/admin/media/folder'
        });
    },
    edit: function () {
        $('form.ajaxform').formPost({
            deleteUrl: '/admin/media/deletefolder'
        });
    },
    view: function () {
        var authtoken = $('#view').find('input[name="__RequestVerificationToken"]').val();
        var catId = $('#CategoryId').val();

        // The localization script
        window.locale = {
            "fileupload": {
                "errors": {
                    "maxFileSize": "File is too big",
                    "minFileSize": "File is too small",
                    "acceptFileTypes": "Filetype not allowed",
                    "maxNumberOfFiles": "Max number of files exceeded",
                    "uploadedBytes": "Uploaded bytes exceed file size",
                    "emptyResult": "Empty file upload result"
                },
                "error": "Error",
                "start": "Start",
                "cancel": "Cancel",
                "destroy": "Delete"
            }
        };
        // The main application script
        $(function () {
            'use strict';

            // Initialize the jQuery File Upload widget:
            $('#fileupload').fileupload();

            $('#fileupload').fileupload('option', {
                maxFileSize: 1048576, // = 1mb
                resizeMaxWidth: 1920,
                resizeMaxHeight: 1200,
                autoUpload: true,
                limitConcurrentUploads: 1,
                acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i
            }).bind('fileuploaddone', function (e, data) {});

        });

        // events
        $('#assets').on('click', 'li', function () {
            var $file = $(this);
            if ($file.hasClass('active')) {
                $file.removeClass('active');
            } else {
                $file.addClass('active');
            }
        });

        $('#btn-remove-files').on('click', function () {
            var $this = $(this);
            $status = $('div.alert');
            asset = { categoryId: catId, ids: [] };
            elements = $('#assets li.active');

            $this.button('loading');

            elements.each(function () {
                asset.ids.push($(this).data('id'));
            });

            var params = decodeURIComponent($.param(asset, true));
            var authParam = $.param({ "__RequestVerificationToken": authtoken });

            $.post('/admin/media/delete', params + '&' + authParam)
                .success(function (response, status, xhr) {
                    switch (response.status) {
                        case 'success':
                            elements.remove();
                            App.Util.statusMessage($status, response.message, 'alert-success');
                            break;
                        case 'error':
                            App.Util.statusMessage($status, response.message, 'alert-error');
                            break;
                    }
                })
                .error(function (data, status, msg) {
                    App.Util.statusMessage($status, errorThrown, 'alert-error');
                })
                .complete(function () {
                    $this.button('complete');
                });
        });

    }
};

/*
    ORGANIZATION
*/
App.Views.organization = {
    init: function () { },
    create: function () {
        $('#btn-generate-password').on('click', function (e) {
            e.preventDefault();
            $('#Password').val(App.Util.password(8).toLowerCase())
        });

        $('form.ajaxform').formPost({
            success: function(){}
        });
    },
    edit: function () {
        $('form.ajaxform').formPost({
            deleteUrl: '/admin/organization/delete',
        });
    }
};

/*
    ROOMTYPE
*/
App.Views.roomtype = {
    init: function () {
        // events
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

        // init components
        $('#model-media').modal({
            show: false,
            backdrop: true,
            keyboard: true
        }).css({
            width: '900px',
            'margin-top': function(){ return ($(this).outerHeight()/2)*-1;},
            'margin-left': function () { return -($(this).width() / 2); }
        });

        $('#model-media').mediaSelector({
            containerLimit: 3
        });
    },
    create: function () {
        $('form.ajaxform').formPost({
            success: function(response){
                $('<li><a href="' + response.data.url + '">' + response.data.name + '</a></li>').insertBefore('#sidebar li.divider');
            }
        }); 
    },
    edit: function () {
        // events
        $('#btn-delete-ok').on('click', function (e) {
            e.preventDefault();

            var $form = $('#form'),
                $status = $('div.alert');
                authtoken = $form.find('input[name="__RequestVerificationToken"]').val();
                hotelId = $form.find('input[name="HotelId"]').val(),
                roomTypeId = $form.find('input[name="RoomTypeId"]').val();

            $.post('/admin/roomtype/delete', { "hotelId": hotelId, "roomTypeId": roomTypeId, "__RequestVerificationToken": authtoken })
                        .success(function (response, status, xhr) {
                            if (response.status == 'success') {
                                $form.hide();
                                $('#sidebar li.active').remove();
                            }
                            $('#model-delete').modal('hide');
                        })
                        .error(function (data, status, errorThrown) {
                            $('#model-delete').modal('hide');
                            App.Util.statusMessage($status, errorThrown, 'alert-error');
                        });
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
        $('form.ajaxform').formPost({
            editUrl: '/admin/user/edit'
        });
    },
    edit: function () {
        $('form.ajaxform').formPost({
            deleteUrl: '/admin/user/delete',
            editUrl: '/admin/user/edit'
        });
    }
}

