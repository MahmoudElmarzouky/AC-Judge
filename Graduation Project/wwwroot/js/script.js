
$(document).ready(function(){
    'use strict';
        
});

function Main_Rule(){
    var IconFav = $('i[data-favorite="IconFav"]');
    IconFav.click(function(){
        $(this).toggleClass('active');
    });
    
}

function Show_User_Error_Message(element){
    var alt = element + " div[id = 'user-error-msg']";
    $(alt).each(function(){
        var Msg = $(this).children('span');
        if($(Msg).is(':empty')){
            $(this).css('display','none');
        }else{
            $(this).css('display','block');
        }
    });
}

$(function(){
    
    'use strict';
    
    /* Start Main Rule */
    
    Main_Rule();
    
    /* End Main Rule */
    
    /* Start User Sign In, Sign Up */;
    Show_User_Error_Message('.user-signin');
    Show_User_Error_Message('.user-signup');
    
    /* End User Sign In, Sign Up */
     
});