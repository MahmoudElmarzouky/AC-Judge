
$(document).ready(function(){
    'use strict';
        
});

function Main_Rule(){
    /*
     Favorite Item [ select and unselect favorite items]
    */
    var IconFavI = $('i[data-favorite="IconFav"]');
    var IconFavA = $('a[data-favorite="IconFav"]');
    IconFavA.click(function(){
        $(this).toggleClass('active');
    });
    IconFavI.click(function(){
        $(this).toggleClass('active');
    });
    
    /*
     Open and Close Panel contain plus icon
    */
    var IconOpenClose = $('.panel .panel-heading i[data-toggle="OpenClosePanel"]');
    IconOpenClose.click(function(){
       $(this).parent().next().fadeToggle(150);
       if($(this).hasClass('fa-plus')){
        $(this).removeClass('fa-plus');
        $(this).addClass('fa-minus');
       }else{
        $(this).removeClass('fa-minus');
        $(this).addClass('fa-plus');
       }
    });
    
    /*
     Open and Close Filter Panel In Icon
    */
    var IconOpenClose = $('.panel .panel-heading i[data-toggle="OpenFilterPanel"]');
    IconOpenClose.click(function(){
       $(this).parent().next('.panel-body').slideToggle(150);
       if($(this).hasClass('fa-angle-down')){
        $(this).removeClass('fa-angle-down');
        $(this).addClass('fa-angle-up');
       }else{
        $(this).removeClass('fa-angle-up');
        $(this).addClass('fa-angle-down');
       }
    });
    
    /*
     Confirm Message on Click button
    */
    var ConfirmMsg = $('.confirm');
    ConfirmMsg.click(function(){
       return confirm('Are You Sure ?'); 
    });
    
    /*
     Switch radio button when click
    */
    var RadioBtnChange = $('.radio-btn-change .btn');
    RadioBtnChange.click(function(){
       $(this).addClass('active').siblings().removeClass('active');
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

function Modal_Edit_Member_Group(){
    var Element = $('.details-group-page .group-member a[data-call="x-modal"]');
    var RowTable = '.details-group-page #EditMemberGroupModal';
    Element.click(function(){
        var UserId = $(this).data('id');
        var UserName = $(this).data('username');
        var FullName = $(this).data('name');
        var Role = $(this).data('role');
        
        $(RowTable + ' [data-id="UserId"]').val(UserId);
        $(RowTable + ' [data-id="UserName"]').text(UserName);
        $(RowTable + ' [data-id="FullName"]').text(FullName);
        
        var btnStatus = $(RowTable + ' [data-id="UserStatus"]');
        if(Role == '-1'){
            btnStatus.val('0');
            btnStatus.html('<i class="fas fa-edit"></i> Set Manager');
        }else if(Role == '0'){
            btnStatus.val('-1');
            btnStatus.html('<i class="fas fa-edit"></i> Set Participant');
        }
  
    });
}

$(function(){
    
    'use strict';
    
    /* Start Main Rule */
    
    Main_Rule();
    
    /* End Main Rule */
    
    /* Start User Sign In, Sign Up */
    Show_User_Error_Message('.user-signin');
    Show_User_Error_Message('.user-signup');
    
    /* End User Sign In, Sign Up */
    
    /* Start Group Page */
    Show_User_Error_Message('.create-group-page');
    Show_User_Error_Message('.edit-group-page');
    Modal_Edit_Member_Group();
    /* End Group Page */
    
    
});