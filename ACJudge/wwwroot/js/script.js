
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
    
    /*
     Eye To Show Password
    */
    var PassFiled = $('.password-eye input[type="password"]');
    $('.password-eye .show-pass-lg').hover(function(){
        PassFiled.attr('type','text');
    }, function(){
        PassFiled.attr('type','password');
    });
    
    $('.password-eye .show-pass').hover(function(){
        PassFiled.attr('type','text');
    }, function(){
        PassFiled.attr('type','password');
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
        }else if(Role == '-2'){
            btnStatus.val('-2');
            btnStatus.css('display','none');
        }
  
    });
}


function Get_Submision(URL, SubID, ElementShow){
    
    $.ajax({
        method: "POST",
        cache: false,
        url: URL ,
        data: { submissionId: SubID },
        success: function (data, status) {
            var result = '';
            for(var i=0;i < data.length; ++i){
                if(data[i] === '<')
                    result += '&lt;';
                else if(data[i] === '>')
                    result += '&gt;';
                else
                    result += data[i];
            }
            
            ElementShow.html(PR.prettyPrintOne(result));
        },
        error: function(xhr, status, error){
            console.log(error);
        }
    });
    
}

function Submision_Status_Page(){
    var OpenModal = $('.status-page #ShowSubmisionStatusModal');
    OpenModal.on('show.bs.modal', function (event) {        
        var button = $(event.relatedTarget);
        var UserName = button.data('user');
        var SubID = button.data('id');
        
        
        var modal = $(this);
        modal.find('.modal-title span').text(UserName);
        
        var AllSiblingsButton = button.parent().nextAll();
        var TableRowSet = modal.find('.modal-body .table tbody tr td');
        
        for(var i=2,j=0;i<AllSiblingsButton.length-1 && j<TableRowSet.length;++i,++j){
            var From = $(AllSiblingsButton[i]);
            var To = $(TableRowSet[j]);
            To.text(From.text());
            To.attr('class',From.attr('class'));
        }
        
        var SetID =  modal.find('.modal-body .table tbody tr input[name="SubmisionID"]');
        SetID.val(SubID);
        
        var URL = button.data('link');
        var SetCode = modal.find('.modal-body .submision pre');
        console.log(URL);
        console.log(SubID);
        console.log(SetCode);
        Get_Submision(URL, SubID, SetCode);
    });
    
    var Copied = $('.status-page .submision-modal .submision .btn');
    Copied.click(function(){
        var PreCode = $(this).next('pre');
        let Txt = PreCode.text();
        
        var TextareaTemp = $('<textarea />');
        $(this).append(TextareaTemp);
        
        TextareaTemp.val(Txt).select();
        
        document.execCommand("copy");
        TextareaTemp.remove();

    });
}
function Status_Page_Contest(){
    var OpenModal = $('.show-contest-page #StatusCotntest');
    OpenModal.on('show.bs.modal', function (event) {        
        var button = $(event.relatedTarget);
        var UserName = button.data('user');
        var SubID = button.data('id');
        
        
        var modal = $(this);
        modal.find('.modal-title span').text(UserName);
        
        var AllSiblingsButton = button.parent().nextAll();
        var TableRowSet = modal.find('.modal-body .table tbody tr td');
        
        for(var i=1,j=0;i<AllSiblingsButton.length-1 && j<TableRowSet.length;++i,++j){
            var From = $(AllSiblingsButton[i]);
            var To = $(TableRowSet[j]);
            To.text(From.text());
            To.attr('class',From.attr('class'));
        }
        
        var SetID =  modal.find('.modal-body .table tbody tr input[name="SubmisionID"]');
        SetID.val(SubID);
        
        var URL = button.data('link');
        var SetCode = modal.find('.modal-body .submision pre');
        Get_Submision(URL, SubID, SetCode);
    });
    
    var Copied = $('.show-contest-page .submision-modal .submision .btn');
    Copied.click(function(){
        var PreCode = $(this).next('pre');
        let Txt = PreCode.text();
        
        var TextareaTemp = $('<textarea />');
        $(this).append(TextareaTemp);
        
        TextareaTemp.val(Txt).select();
        
        document.execCommand("copy");
        TextareaTemp.remove();

    });
}

function Insert_Problem_Table_Create_Contest(MainClass) {

    var MainTableObj = $(MainClass + ' .table');
    var AddProblemPlus = MainClass + ' .table thead i.fa-plus';
    var AddProblemMinus = MainClass + ' .table thead i.fa-minus';
    var DoneProblem = MainClass + ' .table i.fa-check';
    var RemProblem = MainClass + ' .table i.fa-times';
    var StaticRowObj = $(MainClass + ' .table tbody tr:first-child').clone();
    var ID = StaticRowObj.find('td:first-child').text();

    $(document).on('click', DoneProblem, function () {
        var CurRow = $(this).parent().parent();
        var ParCurRow = CurRow.parent();

        $(AddProblemMinus).removeClass('fa-minus').addClass('fa-plus');

        // Work In Clicked Element
        $(this).removeClass('fa-check text-success').addClass('fa-times text-danger');

        var AllInputFiled = CurRow.find("input, select");
        AllInputFiled.each(function () {
            $(this).attr('disabled', 'disabled');
        });

    });
    $(document).on('click', AddProblemPlus, function () {
        if (ID === 'Z')
            return;
        ID = String.fromCharCode(ID.charCodeAt(0) + 1); //Increase ID

        var CurObj = StaticRowObj.clone();
        CurObj.find('td:first-child').text(ID);
        var TableBody = MainTableObj.find('tbody');
        TableBody.append(CurObj);

        $(this).removeClass('fa-plus').addClass('fa-minus');
    });

    $(document).on('click', RemProblem, function () {
        var CurRow = $(this).parent().parent();

        var AllNextRow = CurRow.nextAll();
        AllNextRow.each(function () {
            var Temp = $(this).find('td:first-child');
            Temp.text(String.fromCharCode(Temp.text().charCodeAt(0) - 1));

        });

        if (CurRow.siblings().length != 0) {
            CurRow.remove();
            ID = String.fromCharCode(ID.charCodeAt(0) - 1);
        }
    });

    // Add All names in fileds and check all problem closed
    var SubmitBtn = $(MainClass + ' form [type="submit"]');
    SubmitBtn.on('click', function (e) {
        if ($(DoneProblem).length > 0) {
            alert("Please Close Last Problem Check !");
            e.preventDefault();
            return;
        }
        let AllInputProblemId = $(MainClass + ' [data-target="problemId"]');

        AllInputProblemId.each(function (idx) {
            var str = "problems[" + idx + "]." + ($(AllInputProblemId[idx]).data('target'));
            $(AllInputProblemId[idx]).removeAttr('disabled').attr('name', str);

        });

        let AllSelectBox = $(MainClass + ' [data-target="PlatForm"]');

        AllSelectBox.each(function (idx) {
            var str = "problems[" + idx + "]." + ($(AllSelectBox[idx]).data('target'));
            $(AllSelectBox[idx]).removeAttr('disabled').attr('name', str);
        });

        let AllInputAlias = $(MainClass + ' [data-target="Alias"]');

        AllInputAlias.each(function (idx) {
            var str = "problems[" + idx + "]." + ($(AllInputAlias[idx]).data('target'));
            $(AllInputAlias[idx]).removeAttr('disabled').attr('name', str);

        });
        
    });

}


function Create_Contest_Page() {

    var ButtonTogle = $('.create-contest-page #CreateContesteGeneric .toogle-button button');
    ButtonTogle.click(function () {
        var Temp = 'btn-default';

        if ($(this).hasClass(Temp)) {
            $(this).removeClass(Temp);
            $(this).addClass($(this).data('toggle'));
        }

        var NxtElements = $(this).siblings('button');
        NxtElements.each(function () {
            var Cur = $(this).data('toggle');
            if ($(this).hasClass(Cur)) {
                $(this).removeClass(Cur);
                $(this).addClass(Temp);
            }
        });

    });

    var AlertVisible = $('.create-contest-page .alert');
    AlertVisible.each(function () {
        if ($(this).is(':empty')) {
            $(this).css('display', 'none');
        } else {
            $(this).css('display', 'block');
        }
    });


    // Add Problem To Table  contest-classical , contest-group
    Insert_Problem_Table_Create_Contest('.create-contest-page .contest-classical');
    Insert_Problem_Table_Create_Contest('.create-contest-page .contest-group');


}


function Flib_Status_Submision(URL, SubID){
    $.ajax({
        method: "POST",
        cache: false,
        url: URL ,
        data: { SubmisionId: SubID },
        success: function (data, status) {
            // Done
        },
        error: function(xhr, status, error){
            console.log(error);
        }
    });
}
function User_View_Page(){
    /*
     Show My Submision
    */
    var BtnToogleSubmision = $('.main-user-view .my-submision [data-target="flibShareSubmision"]');
    
    BtnToogleSubmision.on('click', function(){
        var ToogleTempIcon = $(this).attr('class');
        $(this).attr('class', $(this).attr('data-toogle'));
        $(this).attr('data-toogle', ToogleTempIcon);
        
        var ParTd = $(this).parent();
        var URL = ParTd.data('link');
        var SubID = ParTd.data('id');
        
        Flib_Status_Submision(URL, SubID);
        
    });
    
}


function Divide_And_Merge_Codeforcess_Problem(){
    if($('#CodeforcesProblemContent').length < 1)
        return;

    let ProblmeStatment = $('#CodeforcesProblemContent .problem-statement');
    var MainProblemTitle = $('#CodeforcesProblemContent .header .title');
    
    // Edit Problem Name
    if($('.show-problem-page').length > 0){
        var GetIndexOfDot = MainProblemTitle.text().indexOf('.');
        var NewTitleText = MainProblemTitle.text().substr(GetIndexOfDot + 1).trim();
        MainProblemTitle.text(NewTitleText);
    }else if($('.show-contest-page').length > 0){
        var Alias = $('.show-problem .panel [data-refere="ProblemAlias"]');
        MainProblemTitle.text(Alias.text())
    }
    
    
    
    // Edit Sections 
    let ProbelmSections = ProblmeStatment.children();
    
    ProbelmSections.each(function(idx){
        var Section = $(ProbelmSections[idx]);
        if(idx > 0){
            var SectionTitle = Section.children('.section-title');
            if(SectionTitle.length > 0){
                var InsertDiv = $('<div />');
                InsertDiv.attr('data-section', 'problemsection');
                InsertDiv.append(SectionTitle.nextAll()); 
                InsertDiv.insertAfter(SectionTitle);
            }else{
                Section.attr('data-section', 'problemsection');
            }
        }
    });
    
    
    function SetInfo(Get, Set){
       var Element = MainProblemTitle.siblings(Get);
        var Text = Element.clone().children().remove().end().text();
        $('.all-data-problem .problem-info ' + Set).text(Text);
        Element.remove(); 
    }
    
    // Edit Info
    SetInfo('.time-limit', '[data-refere="TimeLimit"]');
    SetInfo('.memory-limit', '[data-refere="MemoryLimit"]');
    SetInfo('.input-file', '[data-refere="InputFile"]');
    SetInfo('.output-file', '[data-refere="OutputFile"]');
    
    // Modal Name
    $('#SubmitProblmeMain [data-target="ProblemName"]').text(MainProblemTitle.text());
    
    ProblmeStatment.css('display', 'block');
   
}
function VoteBlog(blogId, voteValue){
    // TODO Fix Url to be relative
    var URL = "https://localhost:5021/Blog/" + (voteValue === 1? "UpVote": "DownVote") + "/" + blogId;
    $.ajax({
        method: "GET",
        cache: false,
        url: URL ,
        success: function (data, status) {
            document.getElementById("voteValue").innerText = data.toString();
        },
        error: function(xhr, status, error){
            console.log(error);
        }
    });
}
$(function(){
    
    'use strict';
    
    /* Start Call All Libarary */
    
    PR.prettyPrint();
    
    /* End Call All Libarary */
    
    /* Start Call BootStrap Comp Used */
    $('[data-toggle="tooltip"]').tooltip();
    /* End Call BootStrap Comp Used */
    
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
    
    /* Start Status Submision */
    Submision_Status_Page();
    /* End Status Submision */
    
    /* Start Create Contest */
    Create_Contest_Page();
    /* End Create Contest*/
    
    /* Start User View Page */
    User_View_Page();
    /* End User View Page */
    
    /* Start Codeforces Edit Problem */
    Divide_And_Merge_Codeforcess_Problem();
    /* End Codeforces Edit Problem */
    
    
    /* Start Contest Page */
    Status_Page_Contest();
    /* End Contest Page  */
    
});