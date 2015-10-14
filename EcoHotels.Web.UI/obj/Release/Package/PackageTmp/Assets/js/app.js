/*
*
*
*
*/

$(document).ready(function () {
    setInterval(function () {
        $.post("/admin/app/ping");
    }, 1000 * 60 * 5);

    $('#language-selector a').click(function(e){
        e.preventDefault();
        
        var $this = $(this);
        var $selector = $this.parent();
        var $form = $this.parents('form');
        var selectedLanguage = $this.children().attr("hreflang");

        $form.find('span.localized').hide();
        $form.find('span.localized').has('i[hreflang=' + selectedLanguage + ']').removeClass('hidden').show();

        $selector.children().toggleClass('active');
    });

    App.ViewEngine.init();    
});


var App = {};

App.Util = {
    statusMessage: function (elm, msg, status) {
        $(document).scrollTop(10);        
        elm.addClass(status)
            .text(msg)
            .removeClass('hidden') //
            .show()
            .delay(8000)
            .fadeOut(600, function () { $(this).removeClass(status); });
    },
    password: function password(length, special) {
        var iteration = 0;
        var password = "";
        var randomNumber;
        if (special == undefined) {
            var special = false;
        }
        while (iteration < length) {
            randomNumber = (Math.floor((Math.random() * 100)) % 94) + 33;
            if (!special) { 
                if ((randomNumber >= 33) && (randomNumber <= 47)) { continue; }
                if ((randomNumber >= 58) && (randomNumber <= 64)) { continue; }
                if ((randomNumber >= 91) && (randomNumber <= 96)) { continue; }
                if ((randomNumber >= 123) && (randomNumber <= 126)) { continue; }
            }
            iteration++;
            password += String.fromCharCode(randomNumber);
        }
        return password;
    }
};

/*
    View Engine
*/

App.ViewEngine = {
    fire: function (view, action, args) {   
        var namespace = App.Views;
        action = (action === undefined) ? 'init' : action;
        if (action !== '' && namespace[view] && typeof namespace[view][action] == 'function') {
            namespace[view][action](args);
        }
    },
    init: function () {
        App.ViewEngine.fire('common');
        
        var container = $('#view');
        if (container.length == 0) return;

        var view = container.data('view'),
            action = container.data('action');

        App.ViewEngine.fire(view); // fires 'init'
        App.ViewEngine.fire(view, action); // fires view -> action
    }
};

/*
    Default View
*/
App.Views = {};
App.Views.common = {
    init: function () {
    }
};



/* 
*   Setup Custom Validation rules
*   http://docs.jquery.com/Plugins/Validation/Validator/addClassRules#rules
*   http://docs.jquery.com/Plugins/Validation/validate
*/
jQuery.validator.addClassRules({
    'required': { required: true },
    'number-required': { required: true, number: true },
    'number': { number: true },
    'email-required': { required: true, email: true }
});

/* 
*   
*   
*/
(function ($) {
    $.fn.extend({
        formPost: function (options) {

            var defaults = {
                deleteUrl: '',
                editUrl: '',
                success: function (response) {
                    $('<li><a href="' + options.editUrl + '/' + response.data.id + '">' + response.data.name + '</a></li>').insertBefore('#sidebar li.divider');
                    /*
                    $("#sidebar li").sort(function asc_sort(a, b){
                    return ($(b).text().toLowerCase()) < ($(a).text().toLowerCase());    
                    }).appendTo('#sidebar');    
                    */
                }
            };

            var options = $.extend(defaults, options);

            // elements
            var $this = $(this);
            var $form = $this;
            var $status = $('div.alert');
            var $btnDelete = $form.find('#btn-delete');

            var id = $form.find('input[name="Id"]').val();
            var authtoken = $this.find('input[name="__RequestVerificationToken"]').val();

            // events
            $('#btn-delete-ok').on('click', function (e) {
                e.preventDefault();

                if (options.deleteUrl == '') {
                    return;
                }
                $.post(options.deleteUrl, { "id": id, "__RequestVerificationToken": authtoken })
                        .success(function (response, status, xhr) {
                            if (response.status == 'success') {
                                $form.hide();
                                $('#sidebar li.active').remove();
                            }
                        })
                        .error(function (data, status, errorThrown) {
                            App.Util.statusMessage($status, errorThrown, 'alert-error');
                        }).
                        complete(function () {
                            $('#model-delete').modal('hide');
                        });
            });

            $('button.btn-primary').click(function () {
                $(this).button('loading');
            });

            // validation
            var validator = $form.validate({
                meta: "validate",
                errorClass: "error", // "error vanadium-advice vanadium-invalid", //note: can not remove the error class, it gives a bug
                validClass: "",
                errorElement: "span",
                errorPlacement: function (error, element) {
                    $(error).addClass('help-inline').insertAfter(element);
                },
                invalidHandler: function (form, validator) {
                    $('button.btn-primary').button('complete');
                },
                highlight: function (element, errorClass, validClas) {
                    $(element).parents("div[class='control-group']").addClass("error");
                },
                unhighlight: function (element, errorClass, validClass) {
                    $(element).parents(".error").removeClass("error");
                },
                submitHandler: function (form) {
                    $(form).ajaxSubmit({
                        success: function (response, status, xhr, elm) {
                            switch (response.status) {
                                case 'success':
                                    if (response.data && response.data.created) {
                                        validator.resetForm()

                                        options.success(response);
                                    }

                                    App.Util.statusMessage($status, response.message, 'alert-success');
                                    break;
                                case 'warning':
                                    App.Util.statusMessage($status, response.message, 'alert-block');
                                    break;
                                case 'error':
                                    App.Util.statusMessage($status, response.message, 'alert-error');
                                    break;
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            App.Util.statusMessage($status, errorThrown, 'alert-error');
                        },
                        complete: function () {
                            $('button.btn-primary').button('complete');
                        }
                    });
                }
            });

            return $this;
        }
    });
} (jQuery));


/*
*
*
*
*/
(function ($) {
    $.fn.extend({
        mediaSelector: function (options) {

            var defaults = {
                ok: function () { },
                aspectRatio: 1.6180,
                containerLimit: 8
            };

            var options = $.extend(defaults, options),
                imageData = {id:'', name:'', x:0, y:0, x2:0, y2:0, cropxunits:0, cropyunits:0};

            // elements
            var $this = $(this),             
                $containers = $('#asset-containers'),
                $targetContainer = null;


            var jCrop = null,
                $foldViewer = $('#media-folder-view'),
                $cropViewer = $('#media-crop-view'),
                $mediaFiles = $('#media-files'),
                $selectedImg = $('#image-to-crop'),
                $btnAcceptCrop = $this.find('#btn-add-ok');

            // methods
            var initMediaSelector = function(){
                $targetContainer = $(this).parent().parent();
                $('#media-folder-view').show();
                $('#media-crop-view').hide();
                $mediaFiles.find('li.active').removeClass('active');         
            };

            var initImageCropper = function(data){
                var $file = $(this);
                imageData = $.extend(imageData, data);

                $selectedImg.attr('src', '/img/' + imageData.id + '/800x600/' + imageData.name);
                $selectedImg.Jcrop({
                    aspectRatio: options.aspectRatio,
                    setSelect: [imageData.x, imageData.y, imageData.x2, imageData.y2],
                    onSelect: function(coords){ 
                        imageData.x = coords.x;
                        imageData.y = coords.y;
                        imageData.x2 = coords.x2;
                        imageData.y2 = coords.y2;
                        imageData.cropxunits = $selectedImg.width();
                        imageData.cropyunits = $selectedImg.height();
                    },
                    onChange: function (coords){}
                });

                $('#media-folder-view').hide();
                $('#media-crop-view').show();
            };

            var insertCroppingIntoContainer = function($target){
                var path = '/img/' + imageData.id + '/200x150/' + imageData.name;
                var url = path + '?width=200&height=150&crop=(' + imageData.x + ',' + imageData.y + ',' + imageData.x2 + ',' + imageData.y2 + ')&cropxunits=' + imageData.cropxunits + '&cropyunits=' + imageData.cropyunits;

                var img = $target.children().get(0);
                $(img).attr('src', url);

                $target.children('.data').remove();

                var content = '<input class="data" name="Assets.Index" type="hidden" value="' + imageData.id + '">' +
                          '<input class="data" name="Assets[' + imageData.id + '].Id" type="hidden" value="' + imageData.id + '">' +
                          '<input class="data" name="Assets[' + imageData.id + '].Name" type="hidden" value="' + imageData.name + '">' +
                          '<input class="data" name="Assets[' + imageData.id + '].TopX" type="hidden" value="' + parseInt(imageData.x) + '">' +
                          '<input class="data" name="Assets[' + imageData.id + '].TopY" type="hidden" value="' + parseInt(imageData.y) + '">' +
                          '<input class="data" name="Assets[' + imageData.id + '].BottomX" type="hidden" value="' + parseInt(imageData.x2) + '">' +
                          '<input class="data" name="Assets[' + imageData.id + '].BottomY" type="hidden" value="' + parseInt(imageData.y2) + '">' +
                          '<input class="data" name="Assets[' + imageData.id + '].CropXUnits" type="hidden" value="' + imageData.cropxunits + '">' +
                          '<input class="data" name="Assets[' + imageData.id + '].CropYUnits" type="hidden" value="' + imageData.cropyunits + '">';

                $(content).appendTo($target); 
                
                $targetContainer.data('has-image', true);           
            };

            var acceptImageCrop = function(e){
                e.preventDefault();
                $this.modal('hide');

                insertCroppingIntoContainer($targetContainer);
                                
                var totalContainers = $containers.children().length,
                    $lastContainer = $containers.children(':last');                           

                if($lastContainer.data('has-image') && totalContainers < options.containerLimit){
                    addNewContainer();
                }
            };

            var removePhoto = function(e){
                e.preventDefault();
                
                $(this).parent().parent().remove();

                var totalContainers = $containers.children().length,
                    $lastContainer = $containers.children(':last');

                if($lastContainer.data('has-image') && totalContainers >= (options.containerLimit-1)){
                    addNewContainer();
                }
            };

            var resizePhoto = function(e){
                e.preventDefault();
                $targetContainer = $(this).parent().parent();

                var dataElements = $targetContainer.children('.data');

                var cropdata = {
                    id: dataElements[1].value, 
                    name: dataElements[2].value, 
                    x: dataElements[3].value, 
                    y: dataElements[4].value, 
                    x2: dataElements[5].value, 
                    y2: dataElements[6].value, 
                    cropxunits: dataElements[7].value, 
                    cropyunits: dataElements[8].value
                };

                initImageCropper(cropdata)
            };

            var addNewContainer = function(){
                var content = '<li data-has-image="false">'+
                    '<img src="http://placehold.it/200x150&text=Add%20photo" class="img-polaroid">' +
                    '<ul class="container-new unstyled" style="display: none; ">' +
                        '<li class="add"><a class="btn" data-toggle="modal" href="#model-media">Add</a></li>' +
                    '</ul>'+
                    '<ul class="container-edit unstyled" style="display: none; ">' +
	                    '<li class="remove"><a href="#" class="btn">Remove</a></li>'+
	                    '<li class="resize"><a data-toggle="modal" href="#model-media" class="btn">Resize</a></li>'+
                    '</ul>'+
                '</li>';

                $(content).appendTo($containers);
            };


            // events
            $('#asset-containers').on('click', 'li.add', initMediaSelector);

            $('#asset-containers').on('click', 'li.remove', removePhoto);

            $('#asset-containers').on('click', 'li.resize', resizePhoto);
            
            $("#asset-containers").on({
                mouseenter: function () {
                    var $this = $(this);
                    var hasData = $this.data('has-image');
                    var selector = (hasData)? 'ul.container-edit' : 'ul.container-new';

                    $this.find(selector)
                        .css('display', 'none')
                        .stop(true, true)
                        .fadeIn('fast')
                        .css('display', 'block');  
                },
                mouseleave: function () {
                    $(this).find('ul')
                        .stop(true, true)
                        .fadeOut(100); 
                }
            }, 'li');


            $mediaFiles.on('click', 'li', function(){ 
                var $this = $(this);
                var data = {id:$this.data('id'), name:$this.data('filename'), x:10, y:10, x2:620, y2:320, cropxunits: 0, cropyunits: 0};
                initImageCropper(data);
            });

            $btnAcceptCrop.on('click', acceptImageCrop);

            $this.on('hide', function(){
                if($selectedImg.data('Jcrop')){
                    $selectedImg.data('Jcrop').destroy();
                }                
            });
        }
    });
})(jQuery);