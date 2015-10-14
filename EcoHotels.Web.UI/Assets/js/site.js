/*
*
*
*
*/

if (!window.console) { window.console = {log: function(){}} };

$(document).ready(function () {
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

App.Views.home = {
    init: function () {
        var $window = $(window),
            $form = $('#search-form'),
            $hero = $('#hero'),
            $images = $hero.find('#slideshow img');

        $("#arrival").datepicker({
            minDate: 0,
            maxDate: '-1D +1Y',
            dateFormat: 'mm/dd/yy',
            onSelect: function (dateText, inst) {
                var nextDate = new Date(inst.selectedYear, inst.selectedMonth, inst.selectedDay);
                nextDate.setDate(nextDate.getDate() + 1);
                $('#datepicker2').datepicker('option', 'minDate', nextDate);
                $('#check-out').focus();
            }
        });

        $("#departure").datepicker({
            minDate: 1,
            maxDate: '+1Y',
            dateFormat: 'mm/dd/yy'
        });


        var validator = $form.validate({
            meta: "validate",
            errorClass: "error",
            validClass: "",
            errorElement: "span",
            errorPlacement: function (error, element) {
                return false;
            },
            highlight: function (element, errorClass, validClass) {
                $(element).parents("div[class='control-group']").addClass("error");
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).parents(".error").removeClass("error");
            },
            submitHandler: function (form) {
                form.submit();  
            }
        });

        $window.on('resize', function () {
            adjustHero();
        });

        var adjustHero = function () {
            var height = $window.height();
            if (500 < height && height < 700) {
                $hero.css({ 'height': height + 'px' });
                $images.css({ 'top': height - 700 + 'px' });
            }
        };

        adjustHero();

        var current = $('#slideshow li.active'),
            next = $('#slideshow li.active').next();

        var changePhoto = function () {
            $(next).addClass('next');

            $(current).fadeOut('slow', function () {
                $(current).removeClass('active').show();
                $(next).addClass('active').removeClass('next');
                
                current = next;

                next = $('#slideshow li.active').next();
                if (next.length == 0) {
                    next = $('#slideshow li:first');
                }                
            });        
        }

        setInterval(changePhoto, 6000);
    }
};

App.Views.account = {
    init: function () {
    },
    logon: function () {
        $('input[name=email]').focus();
    }
};

App.Views.checkout = {
    init: function () {
    },
    step1: function () {
    
        var currency = $('#room-information').data('currency');
        var numberOfDays = $('#room-information').data('days');
        var addons = $('tr.addonline');
        var bookinglines = $('tr.bookingline');

        var updateAddonPrices = function () {
            $.each(addons, function () {
                var $this = $(this);
                var $checkbox = $this.find('input[type="checkbox"]');


                var price = $checkbox.data('price');
                var postingrhythm = +$checkbox.data('postingrhythm');               // days = 1, entire = 5
                var calculationrule = +$checkbox.data('calculationrule');           // person = 1, room = 5

                var quests = $this.prev('tr.bookingline').find("option:selected").val() || 0;
                $this.find('span.addon-item-guests').text(quests);                 // Update number of guest for addon if calculationrule = 1

                var addonTotal = 0;

                if (calculationrule == 1) { // Per Guest
                    addonTotal = price * quests;
                } else { // Per Room
                    addonTotal = price;
                }

                if (postingrhythm == 1) { // Per Day
                    addonTotal = addonTotal * numberOfDays;
                } else { // Entire stay
                    addonTotal = addonTotal;
                }

                // Update price                
                if (!$checkbox.is(':checked')) {
                    $this.data('addon-item-totalprice', 0);
                } else {
                    $this.data('addon-item-totalprice', addonTotal);
                }

                $this.find('span.addon-item-total').text(addonTotal);
            });

        };

        var updateRoomPrices = function () {
            $.each(bookinglines, function () {
                var $this = $(this);

                var $roomprice = $this.find('span.room-total');

                var roomprice = $roomprice.data('roomprice');
                var addontotal = $this.next('tr.addonline').data('addon-item-totalprice');
                var total = roomprice + addontotal;

                $this.data('bookingline-total', total);
                $roomprice.text(total);
            });
        };

        var updateTotalPrice = function () {
            var total = 0;            
            $.each(bookinglines, function () {
                total = total + $(this).data('bookingline-total');            
            });
            $('td.booking-total').text(total);
        };


        $('#Firstname').focus();
        $('form.ajaxform').formPost({
            success: function(){
                document.location = $('#next-step').val();
            }
        });

        $('#tos').tooltip({
            animation: false,
            placement: 'right',
            title: function () {
                return $('#test-tos').html();
            }
        });


        var $rooms = $('#room-information tbody tr');
        if ($rooms.length == 1) {
            var $names = $('#Firstname, #Lastname');
            var $primaryGuestName = $('#Rooms_0__NameOfPrimaryGuest');

            $names.on('blur', function () {
                var fullname = '';
                $primaryGuestName.val(fullname);

                $names.each(function () {
                    fullname += ' ' + $(this).val();
                });

                $primaryGuestName.val($.trim(fullname));
            });
        }

        $("input.addon").on('change', function (e) {
            updateAddonPrices();
            updateRoomPrices();
            updateTotalPrice();
        });

        $('select.number-of-guests').change(function () {
            updateAddonPrices();
            updateRoomPrices();
            updateTotalPrice();
        });



    },
    complete: function () {   
    }
};

App.Views.reservation = {
    init: function () {
    },
    show: function () {
        $('#btn-cancel-ok').on('click', function (e) {
            e.preventDefault();

            var $status = $('div.alert');

            var id = $('input[name="Id"]').val();
            var authtoken = $('input[name="__RequestVerificationToken"]').val();

            $.post('/en/reservation/cancel', { "id": id, "__RequestVerificationToken": authtoken })
                    .success(function (response, status, xhr) {
                        if (response.status == 'success') {

                        }

                        App.Util.statusMessage($status, response.message, 'alert-success');
                    })
                    .error(function (data, status, errorThrown) {
                        App.Util.statusMessage($status, errorThrown, 'alert-error');
                    }).
                    complete(function () {
                        $('#model-cancel').modal('hide');
                    });
        });
    }
};

App.Views.settings = {
    init: function () {
    },
    details: function () {
        $('form.ajaxform').formPost();
    }
};

App.Views.search = {
    init: function () {
    },
    result: function () {
        $('#calendars').multidatepicker();
        $('#myCarousel, #myCarousel2').gallery();
    }
};

App.Views.hotel = {
    init: function () {
    },
    view: function () {
        $('#myTab a').click(function (e) {  
            e.preventDefault();
            $(this).tab('show');
        });

        $('#maps').on('shown', function (e) {
            e.preventDefault();
            $('#google-map').googlemap();

        });

        $('#thumbnails img').on('click', function () {
            var url = $(this).data('large-photo-url');
            $('#large-hotel-photo').attr('src', url);
        });        

        $('#search-result select').on('change', function(){
            var total = 0;
            $("#search-result select option:selected").each(function () {
                total += +$(this).data('price');
            });

            var isRoomSelected = false;
            $.each($("#search-result select option:selected"), function (i, elm) {
                console.log(+$(elm).val());
                if (+$(elm).val() >= 1) {
                    isRoomSelected = true;
                }                
            });

            if (isRoomSelected) {
                $('#btn-reservate').removeClass('disabled');
            } else {
                $('#btn-reservate').addClass('disabled');
            }

            $('#total-reservation-price').text(total);
        });

        $('#btn-reservate').on('click', function(e){
            e.preventDefault();
            if($(this).hasClass('disabled'))
            {
                return;
            }

            $('#arrival').val($('#check-in').val());
            $('#departure').val($('#check-out').val());

            $('#reservate-form').submit();
        });

        $('#calendars').multidatepicker();

        $('div.roomtype').tooltip({
            animation: false,
            placement: 'left',
            html: true,
            title: function () {
                return $(this).find('div.roomtype-description-tooltip').html();
            }
        }); 
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
    "use strict";

    $.fn.extend({
        gallery: function (options) {
            return this.each(function(){
                var $this = $(this),
                    dots = $this.find('.gallery-dots');

                $this.carousel({ interval: 5000, pause: false });
                $this.carousel('pause');

                $this.on('mouseenter', function(){
                    var $element = $(this);
                    $element.carousel('cycle');
                    dots.show();
                    $element.find('.carousel-control').show();
                });

                $this.on('mouseleave', function(){
                    var $element = $(this);
                    $element.carousel('pause');
                    dots.hide();
                    $element.find('.carousel-control').hide();
                });

                $this.on('slide', function(){                    
                    var last = dots.find("a.selected:last-child");
                    if(last.length > 0){
                        last.removeClass('selected');
                        dots.find('a:first-child').addClass('selected')
                    }else{
                        dots.find('a.selected').removeClass('selected').next().addClass('selected');
                    }                   
                });  
            
            });
        }
    });
} (window.jQuery));


/* 
*   
*   https://developers.google.com/maps/documentation/javascript/tutorial
*/
(function($) {
    $.fn.extend({
        googlemap: function(options){
            // elements
            var $this = $(this);

            var defaults = {
                zoom: 13,
                mapTypeId: google.maps.MapTypeId.TERRAIN
            };
            
            var o = $.extend(defaults, options);
            var map = new google.maps.Map($this[0], o);

            var address = $this.data('hotel-address');
            var region = $this.data('hotel-region');
            
            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'address': address, 'region': region }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    map.setCenter(results[0].geometry.location);
                    var marker = new google.maps.Marker({
                        map: map,
                        position: results[0].geometry.location
                    });
                } else {
                    //console.log(results, status);
                    //alert("Geocode was not successful for the following reason: " + status);
                }
            });
        }
    });
} (jQuery));


/* 
*   
*   
*/
(function($) {
    $.fn.extend({
        multidatepicker: function(options){

            // elements
            var $form = $('#calendars-form');

            $('#calendars input').on('focus', function () {
                $('#calendars div.active').removeClass('active');
                $(this).closest('div.calendar-text').next().addClass('active');
            });


            //$('#check-in').focus();

            var checkinDate = $('#check-in').data('date');
            var checkoutDate = $('#check-out').data('date');

            $("#datepicker").datepicker({
                minDate: 0,
                maxDate: '-1D +1Y',
                altField: '#check-in',
                altFormat: 'dd MM, yy',
                defaultDate: checkinDate,
                onSelect: function (dateText, inst) {
                    var nextDate = new Date(inst.selectedYear, inst.selectedMonth, inst.selectedDay);
                    nextDate.setDate(nextDate.getDate() + 1);
                    $('#datepicker2').datepicker('option', 'minDate', nextDate);
                    $('#check-out').focus();
                }
            });

            $("#datepicker2").datepicker({
                minDate: 1,
                maxDate: '+1Y',
                altField: '#check-out',
                altFormat: "dd MM, yy",
                defaultDate: checkoutDate,
                onSelect: function (dateText, inst) {

                    $('#calendars div.active').removeClass('active');

                    var data = {
                        arrival: $('#check-in').val(),
                        departure: $('#check-out').val()
                    };

                    $('#calendars-form').ajaxSubmit({
                        data: data,
                        success: function (response, status, xhr, elm) {
                            $('#search-result').html(response);
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                        },
                        complete: function () {
                        }
                    });
                }
            });


        }
    });
} (jQuery));

/* 
*   
*   
*/
(function ($) {
    $.fn.extend({
        formPost: function (options) {

            var defaults = {
                success: function (response) {
                }
            };

            var options = $.extend(defaults, options);

            // elements
            var $this = $(this);
            var $form = $this;
            var $status = $('div.alert');

            var id = $form.find('input[name="Id"]').val();
            var authtoken = $this.find('input[name="__RequestVerificationToken"]').val();

            
            $('button.btn').click(function () {
                $(this).button('loading');
            });

            // validation
            var validator = $form.validate({
                meta: "validate",
                errorClass: "error",
                validClass: "",
                errorElement: "span",
                errorPlacement: function (error, element) {
                    if($(element).hasClass('no-error-message') == false){
                        $(error).addClass('help-inline').insertAfter(element);
                    }                    
                },
                invalidHandler: function (form, validator) {
                    $('button.btn').button('complete');
                },
                highlight: function (element, errorClass, validClass) {
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
                            $('button.btn').button('complete');
                        }
                    });
                }
            });

            return $this;
        }
    });
} (jQuery));