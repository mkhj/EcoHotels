/*
*
*
*
*/
$(document).ready(function () {
    $('a.twitter').twitterLogon();
});

/* 
*   
*   
*/
(function ($) {
    $.fn.extend({
        twitterLogon: function (options) {
            var $this = $(this);
            url = $this.attr('href');
            $this.on('click', function (e) {
                e.preventDefault();
                var popup = window.open(url, '_blank', 'height=400,width=800,left=250,top=100,resizable=yes', true);

                var wait = function () {
                    setTimeout(function () {
                        if (popup.closed) {
                            location.href = '/';
                        } else { wait(); }
                    }, 25);
                };
                wait();
            });
        }
    });
} (jQuery));

(function($){
    var autoLogin = function(response){
        if (response.status === 'connected') {
            login(response); // app has been authorized
        } else if (response.status === 'not_authorized') {
                // app has NOT been authorized
        } else {
            // user is not conntected to facebook
        }
    }

    var login = function (response) {
        FB.api('/me', function (response) {
            var authtoken = $('input[name="__RequestVerificationToken"]').val();
            var data = $.extend(response, {'__RequestVerificationToken': authtoken});

            $.post('/en/facebook/logon', data)
                .success(function() { document.location = '/'; })
                .error(function() { console.log("error logging in to your facebook account."); });
        });
    }

    $('a.facebook').on('click', function (e) { 
        e.preventDefault();
        FB.login(function (response) {
            if (response.authResponse) {
                login(response);
            } else {
                // User cancelled login or did not fully authorize.
            }
        }, { scope: 'email, user_birthday' });
    });

    /*
        http://developers.facebook.com/docs/reference/javascript/FB.init/
        http://developers.facebook.com/docs/reference/javascript/FB.login/
        http://developers.facebook.com/docs/reference/javascript/FB.getLoginStatus/
    */

    window.fbAsyncInit = function () {
        FB.init({
            appId: '252482554872325',
            status: true,
            cookie: true,
            xfbml: true
        });

        // Rather than calling FB.getLoginStatus explicitly, it is possible to check the user's status by setting status: true when you call FB.init.
        FB.Event.subscribe('auth.statusChange', autoLogin);
    };

}(jQuery));

// Load the SDK Asynchronously
(function (d) {
    var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement('script'); js.id = id; js.async = true;
    js.src = "//connect.facebook.net/en_US/all.js";
    ref.parentNode.insertBefore(js, ref);
} (document));