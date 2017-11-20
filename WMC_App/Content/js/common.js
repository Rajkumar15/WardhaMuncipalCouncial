var DateFormatString = '';
DateFormatString = 'dd/MM/yyyy';
var ModuleID = 0;
var urlparam = new Array();
var weekday = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thrusday", "Friday", "Saturday"];
var wMonths = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

var jStudData = new Array();
function GetParameterValues(url) {
    urlparam = new Array();
    var url = url.slice(url.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var modal = {};
        modal["stringparam"] = url[i].split('=')[0];
        modal["stringvalue"] = url[i].split('=')[1].replace("%20", ' ').replace("%20", ' ').replace("#", ' ').trim();
        urlparam.push(modal);

    }
    return urlparam;
}

function isNumber(evt, element) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    if (
        (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function getHolidayByDate(date) {
    var newDate = date.split('/')[1] + '/' + date.split('/')[0] + '/' + date.split('/')[2]
    var selectedArray = jQuery.grep(Holiday, function(element, index) {
       
        return element.eventdate == newDate// retain appropriate elements

    });
    return selectedArray;
}

var Holiday = new Array();
function getPageName(Arr) {
    var selectedArray = jQuery.grep(Arr, function(element, index) {
        // 
        return element.stringparam == "FormType" // retain appropriate elements

    });
    return selectedArray;
}

function getModuleName(Arr) {
    var selectedArray = jQuery.grep(Arr, function(element, index) {
        // 
        return element.stringparam == "type" // retain appropriate elements

    });
    return selectedArray;
}
var HostAddress = window.location.href.substr(0, window.location.href.indexOf("/", window.location.href.indexOf("/", 7) + 1));
var webPath = HostAddress + "/ws/";
var popupisOpen = 0;
function ajaxPost(url, data, success_callback, error_callback) {
    try {
        $("#backgrounddiv").show();

        $.ajax({
            type: "POST",
            url: webPath + url,
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function(value) {
                $("#backgrounddiv").hide();
               
                if (value.d == "-123456789xVyCleanNagpur") {
                    if (popupisOpen == 0) {
                        popupisOpen = popupisOpen + 1;
                        UserID = sessionStorage.getItem("UserID");
                        UpdateUserLog(UserID, function(val) {
                            if (val == true) {
                                OpenUserDefineMsgBox("alert", "You are Session has been Expired..Please Re-Login..Thank You..", function(val) {
                                    if (val == true) {
                                        popupisOpen = 0;
                                        window.location.href = "../Login.aspx";
                                    }
                                });
                            }
                        });
                    }
                }

                success_callback(value);
            },
            error: function(response) {
                
                console.log(response.responseText);
                OpenUserDefineMsgBox("alert", "Some Error Occure in System Contact your System Administrator", function(val) {
                    if (val == true) {
                        $("#backgrounddiv").hide();
                    }
                });

            }
        });
    }
    catch (ex) {      
        log(ex);
    }
}



function ajaxPostLogIn(url, data, success_callback, error_callback) {
    try {
     
        $("#backgrounddiv").show();
        $.ajax({
            type: "POST",
            url: webPath + url,
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function(value) {
                // $("#backgrounddiv").hide();
              
                if (value.d == "-123456789xVyCleanNagpur") {

                }

                success_callback(value);
            },
            error: function(response) {
              

                //                OpenUserDefineMsgBoxOnLOgIn("alert", "Some Error Occure in System Contact your System Administrator", function(val) {
                //                    if (val == true) {
                //                        $("#backgrounddiv").hide();
                //                    }
                //                });

            }
        });
    }
    catch (ex) {
      
        log(ex);
    }
}



function OpenUserDefineMsgBoxOnLOgIn(MsgType, UserMsg, OnSuccess) {
    $("#uxUserDefineMsgParent").html("");
    $("#uxUserDefineMsgParent").load("UserDefineMsgbox.htm", function(response, status, xhr) {
        xhr.complete(function() {
            var window = $("#uxUserDefinemsg")
            if (!window.data("kendoWindow")) {
                window.kendoWindow({
                    width: "300px",
                    title: MsgType.toUpperCase(),
                    modal: true,
                    resizable: false,
                    deactivate: function(e) {
                        OnSuccess(whReturn);
                        this.destroy();
                    },
                    close: OnCloseWindow
                });
                window.data("kendoWindow").open();
                $("#uxUserDefinemsg").closest(".k-window").css("top", "250px");
                $("#uxUserDefinemsg").closest(".k-window").css("left", "470px");
                $("#uxUserDefinemsg_wnd_title").next().hide();
                $(".msg").html("<p style='word-wrap: break-word;width: 200px;line-height: 20px;'>" + UserMsg + "</p>");
                if (MsgType.toLowerCase() == "alert") {
                    $("#uxUserDefinemsg_wnd_title").next(".k-window-actions");
                    $(".typemsg").html("<img src='msgimg/alert_icon.png'/>");
                    $("#btnCancel").hide();
                }
                else if (MsgType.toLowerCase() == "confirmation") {
                    $("#uxUserDefinemsg_wnd_title").next(".k-window-actions");
                    $(".typemsg").html("<img src='msgimg/confirmation.png'/>");
                    $("#btnCancel").show();
                }
                else if (MsgType.toLowerCase() == "delete") {
                    $(".typemsg").html("<img src='msgimg/delete_icon.png'/>");
                    $("#btnCancel").show();

                }
                else if (MsgType.toLowerCase() == "save") {
                    $(".typemsg").html("<img src='msgimg/save.png'/>");
                    $("#btnCancel").hide();

                }
                else if (MsgType.toLowerCase() == "saveproceed") {
                    $(".typemsg").html("<img src='msgimg/save.png'/>");
                    $("#btnCancel").show();

                }
                if (MsgType.toLowerCase() == "info") {
                    $("#uxUserDefinemsg_wnd_title").next(".k-window-actions");
                    $(".typemsg").html("<img src='msgimg/Inform_icon.png'/>");
                    $("#btnCancel").hide();
                }
                if (MsgType.toLowerCase() == "error") {
                    $("#uxUserDefinemsg_wnd_title").next(".k-window-actions");
                    $(".typemsg").html("<img src='msgimg/error.png'/>");
                    $("#btnCancel").hide();
                }
                if (MsgType.toLowerCase() == "success") {
                    $("#uxUserDefinemsg_wnd_title").next(".k-window-actions");
                    $(".typemsg").html("<img src='msgimg/success.png'/>");
                    $("#btnCancel").hide();
                }


                var whReturn = false;
                $("#btnOk").click(function() {
                    whReturn = true;
                    $("#uxUserDefinemsg").data("kendoWindow").close();
                });
                $("#btnCancel").click(function() {
                    whReturn = false;
                    $("#uxUserDefinemsg").data("kendoWindow").close();
                });

            }
        });
    });

}










function OpenUserDefineMsgBox(MsgType, UserMsg, OnSuccess) {
    $("#uxUserDefineMsgParent").html("");
    $("#uxUserDefineMsgParent").load("/JobPortal_web/UserDefineMsgbox.htm", function(response, status, xhr) {
        xhr.complete(function() {
            var window = $("#uxUserDefinemsg")
            if (!window.data("kendoWindow")) {
                window.kendoWindow({
                    width: "300px",
                    title: MsgType.toUpperCase(),
                    modal: true,
                    resizable: false,
                    deactivate: function(e) {
                        OnSuccess(whReturn);
                        this.destroy();
                    },
                    close: OnCloseWindow
                });
                window.data("kendoWindow").open();
                $("#uxUserDefinemsg").closest(".k-window").css("top", "250px");
                $("#uxUserDefinemsg").closest(".k-window").css("left", "470px");
                $("#uxUserDefinemsg_wnd_title").next().hide();
                $(".msg").html("<p style='word-wrap: break-word;width: 200px;line-height: 20px;'>" + UserMsg + "</p>");
                if (MsgType.toLowerCase() == "alert") {
                    $("#uxUserDefinemsg_wnd_title").next(".k-window-actions");
                    $(".typemsg").html("<img src='../msgimg/alert_icon.png'/>");
                    $("#btnCancel").hide();
                }
                else if (MsgType.toLowerCase() == "confirmation") {
                    $("#uxUserDefinemsg_wnd_title").next(".k-window-actions");
                    $(".typemsg").html("<img src='../msgimg/confirmation.png'/>");
                    $("#btnCancel").show();
                }
                else if (MsgType.toLowerCase() == "delete") {
                    $(".typemsg").html("<img src='../msgimg/delete_icon.png'/>");
                    $("#btnCancel").show();

                }
                else if (MsgType.toLowerCase() == "save") {
                    $(".typemsg").html("<img src='../msgimg/save.png'/>");
                    $("#btnCancel").hide();

                }
                else if (MsgType.toLowerCase() == "saveproceed") {
                    $(".typemsg").html("<img src='../msgimg/save.png'/>");
                    $("#btnCancel").show();

                }
                if (MsgType.toLowerCase() == "info") {
                    $("#uxUserDefinemsg_wnd_title").next(".k-window-actions");
                    $(".typemsg").html("<img src='../msgimg/Inform_icon.png'/>");
                    $("#btnCancel").hide();
                }
                if (MsgType.toLowerCase() == "error") {
                    $("#uxUserDefinemsg_wnd_title").next(".k-window-actions");
                    $(".typemsg").html("<img src='../msgimg/error.png'/>");
                    $("#btnCancel").hide();
                }
                if (MsgType.toLowerCase() == "success") {
                    $("#uxUserDefinemsg_wnd_title").next(".k-window-actions");
                    $(".typemsg").html("<img src='../msgimg/success.png'/>");
                    $("#btnCancel").hide();
                }
                if (MsgType.toLowerCase() == "required") {
                    $("#uxUserDefinemsg_wnd_title").next(".k-window-actions");
                    $(".typemsg").html("<img src='../msgimg/required.png'/>");
                    $("#btnCancel").hide();
                }
                var whReturn = false;
                $("#btnOk").click(function() {
                    whReturn = true;
                    $("#uxUserDefinemsg").data("kendoWindow").close();
                });
                $("#btnCancel").click(function() {
                    whReturn = false;
                    $("#uxUserDefinemsg").data("kendoWindow").close();
                });

            }
        });
    });

}
var CommonData = new Array();
var CasteData = new Array();
var Category = new Array();
function BindCommonData(OnSuccess) {
    try {
        CommonData = new Array();
        CasteData = new Array();
        Category = new Array();
        ajaxPost("ws_Common.asmx/getCommonData"
        , "{}"
        , function(value) {
            //  
            var jsonData1 = eval('(' + value.d + ')');
            if (jsonData1.length > 0) {
                for (var i = 0; i < jsonData1[0].table.length; i++) {
                    CommonData.push(jsonData1[0].table[i]);
                }
                for (var i = 0; i < jsonData1[1].table1.length; i++) {
                    CasteData.push(jsonData1[1].table1[i]);
                }
                for (var i = 0; i < jsonData1[2].table2.length; i++) {
                    Category.push(jsonData1[2].table2[i]);
                }


            }
            OnSuccess(true);
        }
        , function(errorText) { }
    );
    }
    catch (Error) { }
}

function UpdateUserLogData(UserID, OnSuccess) {
    ajaxPost("ws_Common.asmx/UpdateUserLogData"
        , "{UserID:" + UserID + "}"
        , function(value) {

            OnSuccess(true);

        }
        , function(errorText) { OnSuccess(false); }
    );

}


function UpdateUserLog(UserID, UpdateUserLog) {
    ajaxPost("ws_Common.asmx/UpdateUserLog"
        , "{UserID:" + UserID + "}"
        , function(value) {
            UpdateUserLog(true);
        }
        , function(errorText) {
            UpdateUserLog(false);
        }
    );

}

function SetSessionValueAfterLogin(UserID, Type, SetSessionAfterLogin) {
 
    ajaxPost("ws_Common.asmx/SetSessionValueAfterLogin"
        , "{UserID:" + UserID + ",Type:'" + Type + "'}"
        , function(value) {

            SetSessionAfterLogin(true);

        }
        , function(errorText) { SetSessionAfterLogin(false); }
    );

}



function UpdateUserLogTable() {
    var UserID = sessionStorage.getItem("UserID");
    OpenUserDefineMsgBox("confirmation", "Do you want to Log out the System..", function(val) {
        if (val == true) {
            ajaxPost("ws_Common.asmx/UpdateUserLogTable"
        , "{UserID:" + UserID + "}"
        , function(value) {
           
            if (value.d.toLowerCase() == "true") {
                window.location.href = "../Login.aspx";
            }

        }
        , function(errorText) { }
    );
        }
    });

}










function OnCloseWindow() {
}
function RequireField(ControlName, lblValue, returnOn) {
   
    var rtval = true;
    var cname = ControlName.split(',');
    var lbl = lblValue.split(',');
    for (var i = 0; i < cname.length; i++) {
        if ($("#" + cname[i]).get(0).tagName == "INPUT") {
            var textlbl = $("#" + lbl[i]).html();
            textlbl = textlbl.trim();
            if ($("#" + cname[i]).attr('type') == "text") {
                if ($("#" + cname[i]).val().trim().length == 0) {
                    var swt = $("#" + cname[i]).width();
                    if (swt > 150) {
                        swt = swt - 100;
                    }
                    var str = "<div class='ErrorMsg'  style='margin-left:" + swt + "px'><div><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " Required</span></div></div>";
                    $("#" + cname[i]).before(str);
                    rtval = false;
                }
                if ($("#" + cname[i]).val() == "0") {
                    var swt = $("#" + cname[i]).width();
                    if (swt > 150) {
                        swt = swt - 100;
                    }
                    var str = "<div class='ErrorMsg'  style='margin-left:" + swt + "px'><div><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " should not be zero</span></div></div>";
                    $("#" + cname[i]).before(str);
                    rtval = false;
                }
            }
            if ($("#" + cname[i]).attr('type') == "password") {
                if ($("#" + cname[i]).val().trim().length == 0) {
                    var swt = $("#" + cname[i]).width();
                    if (swt > 150) {
                        swt = swt - 100;
                    }
                    var str = "<div class='ErrorMsg'  style='margin-left:" + swt + "px'><div><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " Required</span></div></div>";
                    $("#" + cname[i]).before(str);
                    rtval = false;
                }
            }
        }
        else if ($("#" + cname[i]).get(0).tagName == "SELECT") {
            // 


            if ($("#" + cname[i] + "").text().toLowerCase() != "select one") {
                if ($("#" + cname[i] + " option:selected").val().trim().length == 0 || $("#" + cname[i] + " option:selected").text().toLowerCase() == "Selectone"
              || $("#" + cname[i] + "").val() == "Selectone" || $("#" + cname[i] + " option:selected").text().toLowerCase() == "select one") {
                    var textlbl = $("#" + lbl[i]).html();
                    var swt = $("#" + cname[i]).width();
                    if (swt > 150) {
                        swt = swt - 100;
                    }
                    var str = "<div class='ErrorMsg'  style='margin-left:" + swt + "px'><div><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " Required</span></div></div>";
                    $("#" + cname[i]).parent().before(str);
                    rtval = false;
                }
            }
            else {
                var textlbl = $("#" + lbl[i]).html();
                var swt = $("#" + cname[i]).width();
                if (swt > 150) {
                    swt = swt - 150;
                }
                var str = "<div class='ErrorMsg'  style='margin-left:" + swt + "px'><div><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " Required</span></div></div>";
                $("#" + cname[i]).parent().before(str);
                rtval = false;
            }
        }
        else if ($("#" + cname[i]).get(0).tagName == "TEXTAREA") {
            var textlbl = $("#" + lbl[i]).html();
            if ($("#" + cname[i]).attr('type') == "text") {
                if ($("#" + cname[i]).val().trim().length == 0) {
                    var swt = $("#" + cname[i]).width();
                    if (swt > 150) {
                        swt = swt - 100;
                    }
                    var str = "<div class='ErrorMsg'  style='margin-left:" + swt + "px'><div><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " Required</span></div></div>";
                    $("#" + cname[i]).before(str);
                    rtval = false;
                }
            }
        }
        else if ($("#" + cname[i]).get(0).tagName == "SPAN") {
           
            var textlbl = $("#" + lbl[i]).html();
            textlbl = textlbl.trim();
            if ($("#" + cname[i] + " input[type=text]").val().trim().length == 0) {
                var swt = $("#" + cname[i]).width();

                var str = "<div class='ErrorMsg'  style='margin-left:" + swt + "px'><div><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " Required</span></div></div>";
                $("#" + cname[i]).before(str);
                rtval = false;
            }
        }
        else if ($("#" + cname[i]).get(0).tagName == "IMG") {
            var textlbl = $("#" + lbl[i]).html();
            textlbl = textlbl.trim();
            if ($("#" + cname[i]).attr("src") == undefined || $("#" + cname[i]).attr("src") == "") {
                var swt = $("#" + cname[i]).width();

                var str = "<div class='ErrorMsg'  style='margin-left:" + swt + "px'><div><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " Required</span></div></div>";
                $("#" + cname[i]).before(str);
                rtval = false;
            }
        }
    }
    returnOn(rtval);

    $('.ErrorMsg').delay(2000).fadeOut('slow');


}

function ShowInLineMsg(Msg, controlname) {
   
    var swt = $("#" + controlname).width();
    if (swt > 150) {
        swt = swt - 100;
    }
    var str = "<div class='ErrorMsg'  style='margin-left:" + swt + "px'><div><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + Msg + "</span></div></div>";
    $("#" + controlname).before(str);
    $('.ErrorMsg').delay(6000).fadeOut('slow');
}


function DisplayNotice() {
    ajaxPost("ws_MasterRecord.asmx/GetNoticeboardData"
        , "{}"
        , function(value) {
            jsondata = eval('(' + value.d + ')');
            NoticeBoard = new Array();
            if (jsondata[0].table.length > 0) {
                if (jsondata[0].table.length > 0) {
                    for (var i = 0; i < jsondata[0].table.length; i++) {
                        NoticeBoard.push(jsondata[0].table[i]);
                    }

                }
            }
            PrintNotices();
        }
        , function(errorText) {



        });
}
function PrintNotices() {
    var str = "<div style='height:540px;padding-top:30px;background:url(../Images/noticeboard_1.png);background-repeat: no-repeat;'>";
    if (NoticeBoard.length > 0) {
        str = str + "<div id='marqueetext' style='width: 90%;margin: 10px auto 0px auto;'><ul style='width: 100%;'>"
        for (var i = 0; i < NoticeBoard.length; i++) {
            if (NoticeBoard[i].isnew == false) {
                str = str + "<li><p>" + NoticeBoard[i].notice + "</p><p class='noticeby'>" + NoticeBoard[i].noticeby + "</p></li>";
            }
            else {
                str = str + "<li><p>" + NoticeBoard[i].notice + "<img src='../Images/new.gif' alt='New' style='margin-left:5px'></p><p class='noticeby'>" + NoticeBoard[i].noticeby + "</p></li>";
            }
        }
        str = str + "</ul></div>";
    }
    str = str + "</div>";
    $("#MenuPrint").html(str);
    var dd = $('#marqueetext').easyTicker({
        direction: 'up',
        easing: 'easeInOutBack',
        speed: 'slow',
        interval: 6000,
        height: '540px',
        visible: 1,
        mousePause: 1,
        controls: {
            up: '.up',
            down: '.down',
            toggle: '.toggle',
            stopText: 'Stop !!!'


        }
    }).data('easyTicker');
}

function DisplayModule() {
    var ModuleData = getNotDefualtModule();
    var rows = ModuleData.length / 2;
    var totalRows = (Math.round(rows) + 1) * 2;
    var k = 0;
    var g = 0;

    var p = 0;
    var q = 0;
    var mString = '<table cellpadding="1" cellspacing="1" style="width: 98%;">'
    for (var i = 0; i < totalRows; i++) {
        if (i % 2 == 0) {
            k = 0;
            mString = mString + '<tr>'
            while (k < 2) {
                if (ModuleData.length != g) {
                    mString = mString + '<td class="ModuleImage" id=' + ModuleData[g].pk_id + ' onClick="tranferToPage(this)">'
                    mString = mString + '<img src="../Images/' + ModuleData[g].imagename + '" alt=' + ModuleData[g].modulename + ' /></td>'

                    g = g + 1;
                }
                k = k + 1;
            }
            mString = mString + '</tr>'
        } else {
            mString = mString + '<tr>'
            p = 0;
            while (p < 2) {
                if (ModuleData.length != q) {
                    mString = mString + '<td style="height: 30px" class="ModuleList" id=' + ModuleData[q].pk_id + '  onClick="tranferToPage(this)">';
                    mString = mString + ModuleData[q].modulename + '</td>'

                    q = q + 1;
                }
                p = p + 1;
            }
            mString = mString + '</tr>'
        }

    }
    mString = mString + "</table>"
    $("#uxModuleList").html(mString);

    $(".LogoutText:eq(2)").hide()


}

function DisplayMenu() {
    $("#MenuPrint").html("");
    var ModuleID = sessionStorage.getItem("ModuleID");
    var _ParentMenu = getParentMenu(ModuleID);
    var strMenu = "<ul>";
    for (var i = 0; i < _ParentMenu.length; i++) {
        strMenu = strMenu + '<li><span class="MenuHeader MenuColor"><a href="#" id=' + _ParentMenu[i].pk_id + ' onclick=transferFromMenu(this,"MainMenu")>' + _ParentMenu[i].menuname;
        var _menu = getMenu(_ParentMenu[i].pk_id)
        if (_menu.length > 0) {
            strMenu = strMenu + '<div style="width:10;height:10px;margin-right:10px;margin-top:8px;float:right;" id=img' + _ParentMenu[i].pk_id + '><img src="../Images/expand.png"/></div></a></span>'
            strMenu = strMenu + '<div id="menudiv' + _ParentMenu[i].pk_id + '" class="hideSubMenu"><ul class="toggle">'
            for (var j = 0; j < _menu.length; j++) {
                strMenu = strMenu + '<li class="icn_add_user"><a href="#" id=' + _menu[j].pk_id + ' onclick=transferFromMenu(this,"SubMenu")>' + _menu[j].menuname + '</a></li>'
            }
            strMenu = strMenu + '</ul></div>'
            strMenu = strMenu + "</li>"
        }
        else {
            strMenu = strMenu + '</a></span>'
            strMenu = strMenu + "</li>"
        }

    }
    strMenu = strMenu + "</ul>"
    $("#MenuPrint").html(strMenu);
}







var SaveMsg = "Record Saved Successfully";
var UpdateMsg = "Record Updated Successfully";
var DeleteMsg = "Record Delete Successfully";
function RequireOnPageField(ControlName, lblValue, Control, returnOn) {

    
    var str = "<div id='uxErrorData'>";
    var rtval = true;
    var cname = ControlName.split(',');
    var lbl = lblValue.split(',');
    for (var i = 0; i < cname.length; i++) {
        var textlbl = $("#" + lbl[i]).html();
        textlbl = textlbl.trim();
        if ($("#" + cname[i]).get(0).tagName == "INPUT") {

            if ($("#" + cname[i]).attr('type') == "text") {
                if ($("#" + cname[i]).val().trim().length == 0) {
                    str = str + "<span class='onpageerror k-error-colored' ><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " Required</span></span><br/>";
                    rtval = false;
                }
               
                if ($("#" + cname[i]).val() == "0") {
                    str = str + "<span class='onpageerror k-error-colored' ><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " shuold not zero</span></span><br/>";
                    rtval = false;
                }
            }
            if ($("#" + cname[i]).attr('type') == "password") {
                if ($("#" + cname[i]).val().trim().length == 0) {
                    str = str + "<span class='onpageerror k-error-colored'><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " Required</span></span><br/>";
                    rtval = false;
                }
            }
        }
        else if ($("#" + cname[i]).get(0).tagName == "SELECT") {
            // 
            if ($("#" + cname[i] + " option:selected").val().trim().length == 0 || $("#" + cname[i] + " option:selected").text().toLowerCase() == "Selectone"
              || $("#" + cname[i] + "").val() == "Selectone") {
                str = str + "<span class='onpageerror k-error-colored'><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " Required</span></span><br/>";
                rtval = false;
            }
        }
        else if ($("#" + cname[i]).get(0).tagName == "TEXTAREA") {
            var textlbl = $("#" + lbl[i]).html();
            if ($("#" + cname[i]).attr('type') == "text") {
                if ($("#" + cname[i]).val().trim().length == 0) {
                    str = str + "<span class='onpageerror k-error-colored'><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " Required</span></span><br/>";

                    rtval = false;
                }
            }
        }
        else if ($("#" + cname[i]).get(0).tagName == "SPAN") {
            var textlbl = $("#" + lbl[i]).html();
            textlbl = textlbl.trim();
            if ($("#" + cname[i] + " input[type=text]").val().trim().length == 0) {
                str = str + "<span class='onpageerror k-error-colored'><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " Required</span></span><br/>";

                rtval = false;
            }
        }
        else if ($("#" + cname[i]).get(0).tagName == "IMG") {
            var textlbl = $("#" + lbl[i]).html();
            textlbl = textlbl.trim();
            if ($("#" + cname[i]).attr("src") == undefined || $("#" + cname[i]).attr("src") == "") {
                str = str + "<span class='onpageerror k-error-colored'><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + textlbl + " Required</span></span><br/>";

                rtval = false;
            }
        }

    }
    str = str + "</div>";
    if (rtval == false) {
        $('#' + Control).html(str);
    }
    else {
        $('#' + Control).html("");
    }


    var window = $('#uxErrorData')
    if (!window.data("kendoWindow")) {
        window.kendoWindow({
            width: "300px",
            height: "300px",
            title: false,
            modal: true,
            resizable: false,

            deactivate: function(e) {

                this.destroy();
            }
        });
    }
    $("#uxErrorData").closest(".k-window").css({ top: "10px", left: "800px" });

    setTimeout(function() {
        $('#uxErrorData').data("kendoWindow").close();
    }, 3000);


    returnOn(rtval);




}




function PopupErrorMsg(Msg) {
    $("#uxMsgDiv").show();
    $("#uxMsgDiv").addClass("k-header");

    $("#uxMsgDiv").html(Msg);
    $('#uxMsgDiv').delay(5000).fadeOut('slow');

}




function ConvertDate(dt) {   
    if (dt != '') {
        var today = new Date(dt);
        var dd = today.getDate() + ""; if (dd.length == 1) dd = "0" + dd; ;
        var mm = (today.getMonth() + 1) + ""; if (mm.length == 1) mm = "0" + mm; ;         //January is 0!
        var yyyy = today.getFullYear();
        var dtdate = (dd + "/" + mm + "/" + yyyy);
        return dtdate;
    }
}

function validateEmail(e) {
 

    var n = $(e).val();
    

    if (n.length > 0) {

        var atpos = n.indexOf("@");
        var dotpos = n.lastIndexOf(".");
        if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= n.length) {
       
            // var swt = $("#" + t.id).width();
            // var str = "<div class='ErrorMsg'  style='margin-left:" + swt + "px'><div><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>Enter Valid Email Id</span></div></div>";
            // $("#" + t.id).before(str);
            // $('.ErrorMsg').delay(7000).fadeOut('slow');
            return false;

        }
        else {
            return true
        }
    }

}


function ConvertDate(dt) {
    //  ;
    if (dt != '') {
        var today = new Date(dt);
        var dd = today.getDate() + ""; if (dd.length == 1) dd = "0" + dd;
        var mm = (today.getMonth() + 1) + ""; if (mm.length == 1) mm = "0" + mm;          //January is 0!
        var yyyy = today.getFullYear();
        var dtdate = (dd + "/" + mm + "/" + yyyy);
        return dtdate;
    }
}

function ConvertDatetoMMDDYYYY(dt) {
    //    ;
    if (dt != '') {

        var dd = dt.split('/')[0];
        var mm = dt.split('/')[1];         //January is 0!
        var yyyy = dt.split('/')[2];
        var dtdate = (mm + "/" + dd + "/" + yyyy);
        return dtdate;
    }
}


function mask(Controlid) {
    //    ;
    $(Controlid).each(function() {
        $(this).mask('99/99/9999');
    });
}

function checkEmpty(data)
{
    
    if ( data==null)
    {
        return '';
    }
    else {
        return data;
    }

}
function checkEmptyfile(data) {
    
    if (data == null) {
        return '';
    }
    else {
        return data.substring(54);
    }

}
function checkdropdowndata(data) {
    
    if (data == null) {
        return 'NA';
    }
    else {
        return data;
    }

}

function onlyAlpha(e) {
    
    var t = event || e;
    var n = t.which || t.keyCode;
    if (n > 31 && (n < 48 || n > 57)) return true;
    return false
}

function onlyAlphaNumeric(e) {
    
    var t = event || e;
    var n = t.which || t.keyCode;
    if ((n < 65 || n > 90) && (n < 48 || n > 57) && (n < 97 || n > 122)) return false;
    return true
}

function GetSqlDateformat(obj) {
    try {
        if (obj != undefined && obj != null) {
            SqlDate = obj.toString().split('/')[1] + '/';
            SqlDate += obj.toString().split('/')[0] + '/';
            SqlDate += obj.toString().split('/')[2];
            return SqlDate;
        }
    }
    catch (ex) {
        log(ex);
    }
}

function PopupDivErrorMsg(Msg, divID) {
    //    
    $(divID).show();
    $(divID).addClass("k-header");

    $(divID).html(Msg);
    $(divID).css("color", "Red");
    var ht = $(divID).height();
    $(divID).css("line-height", ht + "px");
    $(divID).css("padding-left", "10px");
    $(divID).css("font-weight", "bold");
    $(divID).delay(5000).fadeOut('slow');

}

function BindXML(filename, controlname, onSelect, onChange, onOpen, onClose, datafield, valuefield, OpLableRequire, OnSuccess) {
    //    
    var jsondata;
    var opLabel = "";
    if (OpLableRequire == true) {
        opLabel = "Select One"
    }
    ajaxPost("Comman.asmx/ReadXml"
        , "{fileName:" + JSON.stringify(filename) + "}"
        , function(value) {
            if (value.d != null) {
                jsondata = eval('(' + value.d + ')');
                //                
                var dataSource = new kendo.data.DataSource({
                    data: jsondata[0].table
                });
                if (dataSource.options.data.length > 0) {
                    $(controlname).kendoDropDownList({
                        dataTextField: datafield,
                        dataValueField: valuefield,
                        dataSource: dataSource,
                        select: onSelect,
                        change: onChange,
                        close: onClose,
                        open: onOpen,
                        optionLabel: opLabel
                    });
                    OnSuccess(true);
                }
            }
        }
        , function(errorText) {



        });

}

var userID = "";
function GetSessionValue(OnSetSession) {
    ajaxPost("ws_Common.asmx/GetSessionValue"
        , "{}"
        , function(value) {
            
            jsondata = eval('(' + value.d + ')');
            if (jsondata[0].table.length > 0) {
                if (parseInt(jsondata[0].table[0].logusersession) == -1) {
                    OpenUserDefineMsgBox("alert", "You are Session has been Expaired..Please Re-Login..Thank You..", function(val) {
                        if (val == true) {
                            window.location.href = "../Login.aspx";
                        }
                    });
                }
                else if (parseInt(jsondata[0].table[0].logusersession) == 0) {
                    OpenUserDefineMsgBox("alert", "No Session was defined..Please Contact to your System Administrator..", function(val) {
                        if (val == true) {
                            window.location.href = "../Login.aspx";
                        }
                    });
                }
                else {
                    $("#showusername").html("<h3>" + jsondata[0].table[0].loginfullname.toUpperCase() + "</h3>");
                    $("#ulLogInTime").html(jsondata[0].table[0].logindatetime);
                    userID = jsondata[0].table[0].loginuserid;
                    sessionStorage.setItem("SessionID", jsondata[0].table[0].logusersession);
                    sessionStorage.setItem("InstID", jsondata[0].table[0].loguserinst);
                }
            }
            else {

            }
            OnSetSession(true);
        }
        , function(errorText) {

            OnSetSession(false);

        });
}

function IsSessionExpire(OnSetSession) {
    ajaxPost("ws_Common.asmx/GetSessionValue"
        , "{}"
        , function(value) {
            if (parseInt(jsondata[0].table[0].logusersession) == -1) {
                OnSetSession(false);
            }
            else {
                OnSetSession(true);
            }
        }
        , function(errorText) {

            OnSetSession(false);

        });
}



function getURL(OnSuccess) {
    var HostAddress = window.location.href.substr(0, window.location.href.indexOf("/", window.location.href.indexOf("/", 7) + 1));
    var s = window.location.href.lastIndexOf('/') + 1;
    var url = window.location.href.substr(s, window.location.href.length);
    //        
    $("#panelbar li").removeClass("k-state-active");
    $("#panelbar li[navurl='" + url + "']").parent().parent().addClass("k-state-active");
    var panelbar = $("#panelbar").data("kendoPanelBar");
    panelbar.expand($("li.k-state-active", panelbar.element));
    $("#panelbar li[navurl='" + url + "'] span").addClass("k-state-hover");
    OnSuccess(true);
}



function onlyNumbers(e) {
    
    var t = event || e;
    var n = t.which || t.keyCode;
    if (n > 31 && (n < 48 || n > 57)) {
        if (n == 45 || n == 95)
        {
            return true;
        }
        return false;
    }
    else {
        return true;
    }
 
}

function GetSqlDateformat(obj) {
    try {
        if (obj != undefined && obj != null) {
            SqlDate = obj.toString().split('/')[1] + '/';
            SqlDate += obj.toString().split('/')[0] + '/';
            SqlDate += obj.toString().split('/')[2];
            return SqlDate;
        }
    }
    catch (ex) {
        log(ex);
    }
}



function IsSMSEmailSend(MenuID, isSend) {
    try {
        // 
        ajaxPost("ws_Common.asmx/IsSMSEmailSend"
                    , "{MenuID:" + MenuID + "}"
                    , function(value) {
                        var jsonData = eval('(' + value.d + ')');
                        isSend(jsonData);
                    }
                    , function(errorText) {
                        isSend(false);
                    }
            );
    }
    catch (Error) {
        isSend(false);
    }
}



function GetMaxRecordWithCondition(TableName, FieldName, Condition, onMaxRec) {
    try {
        // 
        ajaxPost("ws_Common.asmx/GetMaxRecordWithCondition"
                    , "{TableName:'" + TableName + "',FieldName:'" + FieldName + "',Condition:'" + Condition + "'}"
                    , function(value) {
                        jsonData = eval('(' + value.d + ')');
                        if (jsonData.length > 0) {
                            //  
                            onMaxRec(jsonData[0].table[0].countid);
                        }
                    }
                    , function(errorText) {
                    }
            );
    }
    catch (Error) {
    }
}


function restrictSpecialChar(e) {

    var regex = new RegExp("^[a-zA-Z0-9-]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str) || e.charCode == 32) {
        return true;
    }

    e.preventDefault();
    return false;
}


function chknumwithDot(e, t, n, r, i) {

    var charCode = (i.which) ? i.which : i.keyCode

    if (charCode != 45 && (charCode != 46 || n.value.indexOf('.') != -1) && (charCode < 48 || charCode > 57)) {
        return false;
    }
    else {
        if (document.getSelection) {
            s = document.getSelection();
            if (s != "") {
                var startPos = n.selectionStart;
                var endPos = n.selectionEnd;
                var str = n.value.slice(startPos, endPos)
                n.value.replace(slicedText, n.value);
            }
        }
        if (t == 0) {
            if (charCode == 46) {
                return false;
            }
            if ((n.value.length + 1) > e) {
                return false;
            }
        }
        else {
            if (charCode != 46) {
                if (n.value.split(".").length == 1) {
                    if ((n.value.split(".")[0].length + 1) > e) {
                        return false;
                    }
                }
                if (n.value.split(".").length > 1) {
                    if ((n.value.split(".")[0].length) > e) {
                        return false;
                    }
                    if (n.value.split(".")[1].length + 1 > t) {
                        return false;
                    }
                }
            }
        }
    }

    return true;
}



function GetMasterData(MasterType) { 
    var selectedArray = jQuery.grep(CommonData, function(element, index) {
        return element.mastertype == MasterType // retain appropriate elements

    });
    return selectedArray;
}

function GetCategorywiseCaste(CatID) {
    var selectedArray = jQuery.grep(CasteData, function(element, index) {
        return element.parentcasteid == 0 && element.fk_categoryid == CatID // retain appropriate elements

    });
    return selectedArray;
}


function GetCategoryID(CasteID) {
    
    var selectedArray = jQuery.grep(CasteData, function(element, index) {
        return element.pk_id == CasteID // retain appropriate elements

    });
    if (selectedArray.length > 0) {
        return selectedArray[0].fk_categoryid;
    }
    else {
        return 0;
    }
}


function GetCaste() {
    var selectedArray = jQuery.grep(CasteData, function(element, index) {
        return element.parentcasteid == 0 // retain appropriate elements

    });
    return selectedArray;
}

function GetSubCaste(parentcasteid) {
    var selectedArray = jQuery.grep(CasteData, function(element, index) {
        return element.parentcasteid == parentcasteid // retain appropriate elements

    });
    return selectedArray;
}

var Module = new Array();
var ParentMenu = new Array();
var MenuMaster = new Array();
var NoticeBoard = new Array();

function GetMenuAndModule(OnSuccessVal) {

    ajaxPost("ws_Common.asmx/getModuleAndMenu"
        , "{}"
        , function(value) {
            //            
            Module = new Array();
            ParentMenu = new Array();
            MenuMaster = new Array();

            jsondata = eval('(' + value.d + ')');
            if (jsondata[0].table.length > 0) {
                for (var i = 0; i < jsondata[0].table.length; i++) {
                    Module.push(jsondata[0].table[i]);
                }
            }

            if (jsondata[1].table1.length > 0) {
                for (var i = 0; i < jsondata[1].table1.length; i++) {
                    ParentMenu.push(jsondata[1].table1[i]);
                }
            }
            if (jsondata[2].table2.length > 0) {
                for (var i = 0; i < jsondata[2].table2.length; i++) {
                    MenuMaster.push(jsondata[2].table2[i]);
                }
            }
            Holiday = new Array();
            if (jsondata[3].table3.length > 0) {
                for (var i = 0; i < jsondata[3].table3.length; i++) {
                    Holiday.push(jsondata[3].table3[i]);
                }
            }

            OnSuccessVal(true);
        }
        , function(errorText) {



        });
}
var TableData = new Array();
function GetTableWiseData(TableName, Criteria, OnSuccessVal) {

    ajaxPost("ws_Common.asmx/GetTableWiseData"
        , "{TableName:'" + TableName + "',Criteria:'" + Criteria + "'}"
        , function(value) {
            TableData = new Array();
            jsondata = eval('(' + value.d + ')');
            OnSuccessVal(jsondata[0].table);
            //            for (var i = 0; i < jsondata[0].table.length; i++) {
            //                TableData.push(jsondata[0].table[i]);
            //            }
        }
        , function(errorText) {



        });
}

function getMenuByName(ArrName, MenuName) {
    var selectedArray = jQuery.grep(ArrName, function(element, index) {
        return element.menuname == MenuName // retain appropriate elements

    });
    return selectedArray;
}

function getNotDefualtModule() {
    var selectedArray = jQuery.grep(Module, function(element, index) {
        return element.isdefualt == false // retain appropriate elements

    });
    return selectedArray;
}
function getParentMenu(ModuleID) {
    var selectedArray = jQuery.grep(ParentMenu, function(element, index) {
        return element.moduleid == ModuleID // retain appropriate elements

    });
    return selectedArray;
}


function getMenu(PMenuID) {
    var selectedArray = jQuery.grep(MenuMaster, function(element, index) {
        return element.parentmenuid == PMenuID // retain appropriate elements

    });
    return selectedArray;
}



function getModule(ModuleName) {

    var selectedArray = jQuery.grep(Module, function(element, index) {
        return element.modulename == ModuleName // retain appropriate elements

    });
    return selectedArray;
}

function getModuleDetailByID(ModuleID) {
    var selectedArray = jQuery.grep(Module, function(element, index) {
        return element.pk_id == ModuleID // retain appropriate elements

    });
    return selectedArray;
}

function RequireControls(ControlName, MsgOn, lblValue, returnOn) {
    //
    var rtval = true;
    var cname = ControlName.split(',');
    var mcontrol = MsgOn.split(',');
    var lbl = lblValue.split(',');
    for (var i = 0; i < cname.length; i++) {
        if ($("#" + cname[i]).attr('type') == "file") {
            if ($("#" + cname[i]).closest(".k-dropzone").next().hasClass("k-upload-files") == false) {

                var str = "<div class='ErrorMsg'  style='margin-left:100px'><div><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + lbl + " Required</span></div></div>";
                $("#" + mcontrol[i]).before(str);
                rtval = false;
            }
        }
    }
    $('.ErrorMsg').delay(5000).fadeOut('slow');
    returnOn(rtval)
}


function RequirePhoto(ControlName, MsgOn, lblValue, returnOn) {
    //
    var rtval = true;
    var cname = ControlName.split(',');
    var mcontrol = MsgOn.split(',');
    var lbl = lblValue.split(',');
    for (var i = 0; i < cname.length; i++) {

        if ($("#" + cname[i]).attr("src") == undefined || $("#" + cname[i]).attr("src") == "") {

            var str = "<div class='ErrorMsg'  style='margin-left:100px'><div><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + lbl + " Required</span></div></div>";
            $("#" + mcontrol[i]).before(str);
            rtval = false;
        }

    }
    $('.ErrorMsg').delay(5000).fadeOut('slow');
    returnOn(rtval)
}

function ConvertDateInWord(datevale, ControlName) {
    
    if (datevale.length > 0) {
        var wDays = ['First', 'Second', 'Third', 'Fourth', 'Fifth', 'Sixth', 'Seventh', 'Eighth', 'Ninth', 'Tenth', 'Eleventh', 'Twelfth', 'Thirteenth', 'Fourteenth', 'Fifteenth', 'Sixteenth', 'Seventeenth', 'Eighteenth', 'Nineteenth', 'Twentieth', 'Twenty-First', 'Twenty-Second', 'Twenty-Third', 'Twenty-Fourth', 'Twenty-Fifth', 'Twenty-Sixth', 'Twenty-Seventh', 'Twenty-Eighth', 'Twenty-Ninth', 'Thirtieth', 'Thirty-First']

        
        var wNumbers = ['One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine', 'Ten', 'Eleven', 'Twelve', 'Thirteen', 'Fourteen', 'Fifteen', 'Sixteen', 'Seventeen', 'Eighteen', 'Nineteen', 'Two Thousand', 'Twentyone']
        var wLastYear = ["Ten", 'Twenty', 'Thirty', 'Fourty', 'Fivety', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];
        var date = new Date(datevale.split('/')[2], (datevale.split('/')[1] - 1), datevale.split('/')[0])
        
        var day = 0;
        var month = 0;
        var year = 0;
        if (date.getDate() > 1) {

            day = parseInt(date.getUTCDate());
            month = parseInt(date.getUTCMonth());
            year = date.getUTCFullYear().toString();
        }
        else {
            day = parseInt(date.getDate()) - 1;
            month = parseInt(date.getMonth());
            year = (date.getFullYear()).toString();
        }



        var x = year.charAt(0)
        var xx = year.charAt(1)
        var xxx = year.charAt(2)
        var xxxx = year.charAt(3)


        var a = parseInt(x + xx) - 1
        var b = parseInt(xxx + xxxx)
        var printyear = "";
        if (b <= 19) {
            printyear = wNumbers[b - 1];
        }
        else {
            var c = parseInt(b % 10);
            var d = parseInt(b / 10);
            if (c > 0) {
                printyear = wLastYear[d - 1] + ' ' + wNumbers[c - 1];
            }
            else {
                printyear = wLastYear[d - 1]
            }
        }
        if (printyear == undefined) {
            printyear = "";
        }
        return wDays[day] + ' ' + wMonths[month] + ' ' + wNumbers[a] + ' ' + printyear;
    }
    else {
        return "";
    }
}
function ConvertDateInYYYYMMDD(dt) {

    var formatedDate = "";
    if (dt != '') {
        var today = new Date(dt);
        var dd = today.getDate();
        var mm = today.getMonth(); //January is 0!
        var yyyy = today.getFullYear();
        var dtdate = yyyy + "/" + mm + "/" + dd;
        return dtdate;
    }
}

function getMonthNo(monthName) {
    var monthNo;
    switch (monthName) {
        case "Jan":
            {
                monthNo = 1;
                break;
            }
        case "Feb":
            {
                monthNo = 2;
                break;
            }
        case "Mar":
            {
                monthNo = 3;
                break;
            }
        case "Apr":
            {
                monthNo = 4;
                break;
            }
        case "May":
            {
                monthNo = 5;
                break;
            }
        case "Jun":
            {
                monthNo = 6;
                break;
            }
        case "Jul":
            {
                monthNo = 7;
                break;
            }
        case "Aug":
            {
                monthNo = 8;
                break;
            }
        case "Sep":
            {
                monthNo = 9;
                break;
            }
        case "Oct":
            {
                monthNo = 10;
                break;
            }
        case "Nov":
            {
                monthNo = 11;
                break;
            }
        case "Dec":
            {
                monthNo = 12;
                break;
            }
    }
    return monthNo;
}

function getHolidaysDate(selectedmonth, selectedyear) {
    
    var edtArray = jQuery.grep(Holiday, function(element, index) {
        return element.eventmonth == selectedmonth
    });
    return edtArray;
}

function butionHideShow(New, Save, Delete, Serch, ViewRpt, GeneRpt, EE, EP, Print, Clear) {
    
    if (New == 1) {
        $("#uxMainNew").hide();
    }
    else if (New == 0) {
        $("#uxMainNew").show();
    }

    if (Save == 1) {
        $("#uxMainSave").hide();
    }
    else if (Save == 0) {
        $("#uxMainSave").show();
    }

    if (Delete == 1) {
        $("#btnDeleteMain").hide();
    }
    else if (Delete == 0) {
        $("#btnDeleteMain").show();
    }

    if (Serch == 1) {
        $("#uxMainSearch").hide();
    }
    else if (Serch == 0) {
        $("#uxMainSearch").show();
    }

    if (ViewRpt == 1) {
        $("#uxMainViewReport").hide();
    }
    else if (ViewRpt == 0) {
        $("#uxMainViewReport").show();
    }


    if (GeneRpt == 1) {
        $("#uxMainGenerateReport").hide();
    }
    else if (GeneRpt == 0) {
        $("#uxMainGenerateReport").show();
    }


    if (EE == 1) {
        $("#uxMainExportToExcel").hide();
    }
    else if (EE == 0) {
        $("#uxMainExportToExcel").show();
    }

    if (EP == 1) {
        $("#uxMainExportToPDF").hide();
    }
    else if (EP == 0) {
        $("#uxMainExportToPDF").show();
    }

    if (Print == 1) {
        $("#uxPrintMain").hide();
    }
    else if (Print == 0) {
        $("#uxPrintMain").show();
    }
    if (Clear == 1) {
        $("#uxClear").hide();
    }
    else if (Clear == 0) {
        $("#uxClear").show();
    }
}

function butEnableDisable(New, Save, Delete, Serch, ViewRpt, GeneRpt, EE, EP, Print, Clear) {
    if (New == 1) {
        $("#uxMainNew").attr("disabled", "disabled");
        $("#uxMainNew").addClass("k-state-disabled");
    }
    else if (New == 0) {
        $("#uxMainNew").removeAttr("disabled");
        $("#uxMainNew").removeClass("k-state-disabled");
    }

    if (Save == 1) {
        $("#uxMainSave").attr("disabled", "disabled");
        $("#uxMainSave").addClass("k-state-disabled");
    }
    else if (Save == 0) {
        $("#uxMainSave").removeAttr("disabled");
        $("#uxMainSave").removeClass("k-state-disabled");
    }

    if (Delete == 1) {
        $("#btnDeleteMain").attr("disabled", "disabled");
        $("#btnDeleteMain").addClass("k-state-disabled");
    }
    else if (Delete == 0) {
        $("#btnDeleteMain").removeAttr("disabled");
        $("#btnDeleteMain").removeClass("k-state-disabled");
    }

    if (Serch == 1) {
        $("#uxMainSearch").attr("disabled", "disabled");
        $("#uxMainSearch").addClass("k-state-disabled");
    }
    else if (Serch == 0) {
        $("#uxMainSearch").removeAttr("disabled");
        $("#uxMainSearch").removeClass("k-state-disabled");
    }


    if (ViewRpt == 1) {
        $("#uxMainViewReport").attr("disabled", "disabled");
        $("#uxMainViewReport").addClass("k-state-disabled");
    }
    else if (ViewRpt == 0) {

        $("#uxMainViewReport").removeAttr("disabled");
        $("#uxMainViewReport").removeClass("k-state-disabled");
    }


    if (GeneRpt == 1) {

        $("#uxMainGenerateReport").attr("disabled", "disabled");
        $("#uxMainGenerateReport").addClass("k-state-disabled");
    }
    else if (GeneRpt == 0) {

        $("#uxMainGenerateReport").removeAttr("disabled");
        $("#uxMainGenerateReport").removeClass("k-state-disabled");
    }


    if (EE == 1) {

        $("#uxMainExportToExcel").attr("disabled", "disabled");
        $("#uxMainExportToExcel").addClass("k-state-disabled");
    }
    else if (EE == 0) {

        $("#uxMainExportToExcel").removeAttr("disabled");
        $("#uxMainExportToExcel").removeClass("k-state-disabled");
    }

    if (EP == 1) {

        $("#uxMainExportToPDF").attr("disabled", "disabled");
        $("#uxMainExportToPDF").addClass("k-state-disabled");
    }
    else if (EP == 0) {

        $("#uxMainExportToPDF").removeAttr("disabled");
        $("#uxMainExportToPDF").removeClass("k-state-disabled");
    }

    if (Print == 1) {

        $("#uxPrintMain").attr("disabled", "disabled");
        $("#uxPrintMain").addClass("k-state-disabled");
    }
    else if (Print == 0) {

        $("#uxPrintMain").removeAttr("disabled");
        $("#uxPrintMain").removeClass("k-state-disabled");
    }
    if (Clear == 1) {

        $("#uxClear").attr("disabled", "disabled");
        $("#uxClear").addClass("k-state-disabled");
    }
    else if (Clear == 0) {

        $("#uxClear").removeAttr("disabled");
        $("#uxClear").removeClass("k-state-disabled");
    }

}

function disbledbuttonControl(New, Save, Delete, Search) {
    if (New == true) {
        $("#uxMainNew").addClass("k-state-disabled");
        $("#uxMainNew").attr("disabled", true);
    }
    else {
        $("#uxMainNew").removeClass("k-state-disabled");
        $("#uxMainNew").removeAttr("disabled");
    }
    if (Save == true) {
        $("#uxMainSave").addClass("k-state-disabled");
        $("#uxMainSave").attr("disabled", true);
    }
    else {
        $("#uxMainSave").removeClass("k-state-disabled");
        $("#uxMainSave").removeAttr("disabled");
    }
    if (Delete == true) {
        $("#btnDeleteMain").addClass("k-state-disabled");
        $("#btnDeleteMain").attr("disabled", true);
    }
    else {
        $("#btnDeleteMain").removeClass("k-state-disabled");
        $("#btnDeleteMain").removeAttr("disabled");
    }
    if (Search == true) {
        $("#uxMainSearch").addClass("k-state-disabled");
        $("#uxMainSearch").attr("disabled", true);
    }
    else {
        $("#uxMainSearch").removeClass("k-state-disabled");
        $("#uxMainSearch").removeAttr("disabled");
    }

}

function validateGrid(CName, msg) {
    var str = "<div class='ErrorMsg'  style='margin-left:300px;z-index:1000;position: fixed;'><div><span class='k-icon k-i-note' ></span><span style='margin-right:5px'>" + msg + " Required</span></div></div>";
    $(CName).append(str);
    $('.ErrorMsg').delay(6000).fadeOut('slow');
}
var LibConfigds = new Array();
function getLibConfig(LibSuccess) {

    try {
        ajaxPost("ws_Common.asmx/GetLibConfig"
        , "{}"
        , function(value) {
            LibConfigds = new Array();
            jsondata = eval('(' + value.d + ')');
            for (var i = 0; i < jsondata[0].table.length; i++) {
                LibConfigds.push(jsondata[0].table[i]);
            }
            LibSuccess(true);

        }
        , function(errorText) {



        });
    }
    catch (ex) {
    }

}

function CheckDuplicateRecord(TableName, Condition, SessionValue, OnSuccess) {
    try {
        ajaxPost("ws_Common.asmx/CheckDuplicateRecord"
        , "{TableName:'" + TableName + "',Condition:'" + Condition + "',SessionValue:'" + SessionValue + "'}"
        , function(value) {

            jsondata = eval('(' + value.d + ')');
            
            if (jsondata[0].table.length > 0) {
                if (parseInt(jsondata[0].table[0].totalrow) > 0) {
                    OnSuccess(true);
                }
                else {
                    OnSuccess(false);
                }
            }
            else {
                OnSuccess(false);
            }


        }
        , function(errorText) {



        });
    }
    catch (ex) {
    }
}


function getMenuDetailByID(MenuID) {
    
    var selectedMenu = jQuery.grep(ParentMenu, function(element, index) {
        return element.pk_id == MenuID
    });
    if (selectedMenu.length == 0) {
        selectedMenu = jQuery.grep(MenuMaster, function(element, index) {
            return element.pk_id == MenuID
        });
    }
    return selectedMenu;
}

function GetSessionFromToDate(SessionID) {
    var selectedArray = jQuery.grep(Session, function(element, index) {
        return element.pkid == SessionID // retain appropriate elements

    });
    return selectedArray;
}


function GetRptMsg(dsValue) {
    try {
        ajaxPost("ws_Common.asmx/GetRptMsg"
        , "{}"
        , function(value) {
            
            dsValue = value.d;
            var dvalue = dsValue.split(',');
            if (dvalue[1].length > 0) {
                OpenUserDefineMsgBox(dvalue[0], dvalue[1], function(val) {
                    if (val == true) {

                    }
                });
            }
        }
        , function(errorText) {



        });
    }
    catch (ex) {
        alert("GI")
    }
}




function CheckSeriesExist(SeriesName, SessionValue, OnSuccess) {
    try {
        ajaxPost("ws_Common.asmx/CheckSeriesExist"
        , "{SeriesName:'" + SeriesName + "',SessionValue:'" + SessionValue + "'}"
        , function(value) {

            jsondata = value.d;
            
            OnSuccess(value.d);



        }
        , function(errorText) {



        });
    }
    catch (ex) {
    }
}

///// Raj Code Start Here //////
$(".Rajstring").keypress(function (e) {
    if (!onlyAlpha("Rajstring")) {
        e.preventDefault();
    }
});

$(".Rajalph").keypress(function (e) {
    if (!onlyAlphaNumeric("Rajalph")) {
        e.preventDefault();
    }
});

$('.Rajdecimal').keypress(function (e) {
    if (!isNumber("Rajdecimal")) {
        e.preventDefault();
    }
});

$('.Rajnumber').keypress(function (e) {
    if (!isNumber("Rajnumber")) {
        e.preventDefault();
    }
});

function formatJsonDate(jsonDate) {
    if (jsonDate == null) {
        return "";
    }
    else {
        var dateString = jsonDate.substr(6);
        var currentTime = new Date(parseInt(dateString));
        var month = currentTime.getMonth() + 1;
        var day = currentTime.getDate();
        var year = currentTime.getFullYear();
        var date = day + "/" + month + "/" + year;
        return date;
    }
};

function JsonActiveString(no) {
    if (no == null || no=="") {
        return "NO";
    }
    else {
        if (no == "1") {
            return "YES";
        }
        else {
            return "NO";
        }
    }
};

function DisplayImageaUsingPath(Path)
{
    if (Path == null || Path == "") {
        return "";
    }
    else {
       return "<div><img src="+Path+" name='noticeimage'/></div>"
    }
}
