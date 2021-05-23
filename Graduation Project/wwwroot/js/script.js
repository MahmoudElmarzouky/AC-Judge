function Height_User_SignIn(){
    var Hi_Lower = 580;
    var Hi_Upper = 330;
    var Max_Media_Md = 992;
   
    var Hi_User = ($(document).width() <= Max_Media_Md) ? Hi_Lower : Hi_Upper;
    var Max_Hi_User = $(window).height() - $('header').height() - $('footer').height();
    Hi_User = Math.max(Hi_User,Max_Hi_User);
    $('.user-sign-in-up').height(Hi_User);
}

$(document).ready(function(){
    /* Start User Sign In, Sign Up */
    
    var User_Selected_Class = $('.user-sign-in-up h1 span.selected');
    $('.user-sign-in-up form').css('display', 'none');
    $('.user-sign-in-up ' + '.' + User_Selected_Class.data('class')).css('display', 'block');
    
    Height_User_SignIn();
    
    /* End User Sign In, Sign Up */
});

$(function(){
    
    'use strict';
    /* Start User Sign In, Sign Up */
    
    // Switch Signin Signup
    
    $('.user-sign-in-up h1 span').click(function(){
        $(this).addClass('selected').siblings().removeClass('selected');
        $('.user-sign-in-up form').hide();
        $('.user-sign-in-up ' + '.' + $(this).data('class')).fadeIn(100);
    });
    
    // Resize User Sign In Height Full Height

    $(window).resize(function(){
        Height_User_SignIn();
    });
    
    /* End User Sign In, Sign Up */
     
});