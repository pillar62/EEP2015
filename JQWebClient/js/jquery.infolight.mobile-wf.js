var isSubPath = false;

var flowDeleteText = "是否要删除这笔工作流";
var flowRejectText = "是否要作废这笔工作流";
var flowPauseText = "是否要暂停这笔工作流";
$(document).live('pagecreate', function (e) {
    if ($.fn.flow) {
        var path = window.location.pathname;
        path = path.replace("/JQWebClient", "");
        path = path.substring(1, path.length);
        if (path.indexOf("/") != -1) {
            isSubPath = true;
        }
        else {
            path = "/";
            isSubPath = false;
        }

        var DeleteSure = $.sysmsg('getValue', 'FLClientControls/FLNavigator/FlowDeleteConfirm');
        var NavText = $.sysmsg('getValue', 'FLClientControls/FLNavigator/NavText');
        var NavTexts = NavText.split(";");
        flowDeleteText = String.format(DeleteSure, NavTexts[20]);
        flowRejectText = $.sysmsg('getValue', 'FLClientControls/FLNavigator/FlowRejectConfirm');
        flowPauseText = $.sysmsg('getValue', 'FLClientControls/FLNavigator/FlowPauseConfirm');


        $("." + $.fn.flow.class).each(function () {
            if (e.target.id == "MobileFormSubmit") {
                $(this).flow("initializeMobileFormSubmit");
            }
            else if (e.target.id == "MobileFormApprove") {
                $(this).flow("initializeMobileFormApprove");
            }
            else if (e.target.id == "MobileFormReturn") {
                $(this).flow("initializeMobileFormReturn");
            }
            else if (e.target.id == "MobileFormNotify") {
                $(this).flow("initializeMobileFormNotify");
            }
            else if (e.target.id == "MobileFormPlusApprove") {
                $(this).flow("initializeMobileFormPlusApprove");
            }
            else if (e.target.id == "MobileFormComment") {
                $(this).flow("initializeMobileFormComment");
            }
            else if (e.target.id == "MobileFormHasten") {
                $(this).flow("initializeMobileFormHasten");
            }
            else if (e.target.id == "MobileFlowPreview") {
                $(this).flow("initializeMobileFlowPreview");
            }
        });
    }
});

function doSubmit(dataFormId, caption) {
    if ($('.info-form', '#' + dataFormId).form('status') != "view") {
        var message = $.sysmsg('getValue', 'JQWebClient/CheckHasSaved');
        alert(message);
        return;
    }
    var itemParams = Request.getQueryString().split(",");
    var itemParam = "";
    for (var i = 0; i < itemParams.length; i++) {
        if (itemParams[i] != "")
            itemParam += "&" + itemParams[i];
    }

    var url = "../InnerPages/MobileFormSubmit.aspx?DataFormId=" + dataFormId + itemParam;
    //location.href = url;
    window.localStorage.setItem("DataFormId", dataFormId);
    setTimeout(function () {
        $.mobile.changePage(url, { transition: "pop", role: "page" })
    }, 100);

    //alert("MobileSubmit");
}

function doApprove(dataFormId, caption, urlParam) {
    if ($('.info-form', '#' + dataFormId).form('status') != "view") {
        var message = $.sysmsg('getValue', 'JQWebClient/CheckHasSaved');
        alert(message);
        return;
    }
    var itemParams = Request.getQueryString().split(",");
    var itemParam = "";
    if (urlParam != undefined) {
        itemParam = urlParam;
    }
    else {
        for (var i = 0; i < itemParams.length; i++) {
            if (itemParams[i] != "")
                itemParam += "&" + itemParams[i];
        }
    }
    var urlPrdfix = "";
    if (isSubPath)
        urlPrdfix = "../";
    var url = urlPrdfix + "InnerPages/MobileFormApprove.aspx?DataFormId=" + dataFormId + itemParam;
    //location.href = url;
    window.localStorage.setItem("DataFormId", dataFormId);
    setTimeout(function () {
        $.mobile.changePage(url, { transition: "pop", role: "page" })
    }, 100);
}

function doReturn(dataFormId, caption, urlParam) {
    if ($('.info-form', '#' + dataFormId).form('status') != "view") {
        var message = $.sysmsg('getValue', 'JQWebClient/CheckHasSaved');
        alert(message);
        return;
    }
    var itemParams = Request.getQueryString().split(",");
    var itemParam = "";
    if (urlParam != undefined) {
        itemParam = urlParam;
    }
    else {
        for (var i = 0; i < itemParams.length; i++) {
            if (itemParams[i] != "")
                itemParam += "&" + itemParams[i];
        }
    }
    var urlPrdfix = "";
    if (isSubPath)
        urlPrdfix = "../";
    var url = urlPrdfix + "InnerPages/MobileFormReturn.aspx?DataFormId=" + dataFormId + itemParam;
    //location.href = url;
    window.localStorage.setItem("DataFormId", dataFormId);
    setTimeout(function () {
        $.mobile.changePage(url, { transition: "pop", role: "page" })
    }, 100);
}

function doNotify(dataFormId, caption, urlParam) {
    var itemParams = Request.getQueryString().split(",");
    var itemParam = "";
    if (urlParam != undefined) {
        itemParam = urlParam;
    }
    else {
        for (var i = 0; i < itemParams.length; i++) {
            if (itemParams[i] != "")
                itemParam += "&" + itemParams[i];
        }
    }
    var urlPrdfix = "";
    if (isSubPath)
        urlPrdfix = "../";
    var url = urlPrdfix + "InnerPages/MobileFormNotify.aspx?DataFormId=" + dataFormId + itemParam;
    //location.href = url;
    window.localStorage.setItem("DataFormId", dataFormId);
    setTimeout(function () {
        $.mobile.changePage(url, { transition: "pop", role: "page" })
    }, 100);
}

function doComment(dataFormId, caption, urlParam) {
    var itemParams = Request.getQueryString().split(",");
    var itemParam = "";
    if (urlParam != undefined) {
        itemParam = urlParam;
    }
    else {
        for (var i = 0; i < itemParams.length; i++) {
            if (itemParams[i] != "")
                itemParam += "&" + itemParams[i];
        }
    }
    var urlPrdfix = "";
    if (isSubPath)
        urlPrdfix = "../";
    var url = urlPrdfix + "InnerPages/MobileFormComment.aspx?DataFormId=" + dataFormId + itemParam;
    //location.href = url;
    window.localStorage.setItem("DataFormId", dataFormId);
    setTimeout(function () {
        $.mobile.changePage(url, { transition: "pop", role: "page" })
    }, 100);
}

function commentGoBack() {
    //var dataFormId = Request.getQueryStringByName("DataFormId");
    var dataFormId = window.localStorage.getItem("DataFormId");
    if (!dataFormId) {
        history.go(-1);
        //$.mobile.changePage("../MobileMainFlowPage.aspx", { transition: "pop", role: "page" })
    }
    else {
        var jqTab = $("#" + dataFormId).closest('.ui-dialog');
        if (jqTab.length == 0) {
            $.mobile.changePage($("#" + dataFormId), { transition: "pop", role: "page" })
        }
        else {
            var jqTabId = jqTab.attr('id');
            $.mobile.changePage($("#" + jqTabId), { transition: "pop", role: "page" })
        }
    }
}

function doPlusApprove(dataFormId, caption, urlParam) {
    var itemParams = Request.getQueryString().split(",");
    var itemParam = "";
    if (urlParam != undefined) {
        itemParam = urlParam;
    }
    else {
        for (var i = 0; i < itemParams.length; i++) {
            if (itemParams[i] != "")
                itemParam += "&" + itemParams[i];
        }
    }
    var urlPrdfix = "";
    if (isSubPath)
        urlPrdfix = "../";
    var url = urlPrdfix + "InnerPages/MobileFormPlusApprove.aspx?DataFormId=" + dataFormId + itemParam;
    //location.href = url;
    window.localStorage.setItem("DataFormId", dataFormId);
    setTimeout(function () {
        $.mobile.changePage(url, { transition: "pop", role: "page" })
    }, 100);
}

function doFlowDelete(dataFormId, caption) {
    if (confirm(flowDeleteText)) {
        var urlPrdfix = "";
        if (isSubPath) {
            urlPrdfix = "../";
        }

        var selectedRow = reunionSYS_TODOLIST();
        var urlParam = {};
        urlParam.Type = "Workflow";
        urlParam.Active = "FlowDelete";
        urlParam.LISTID = selectedRow.LISTID;
        urlParam.PROVIDER_NAME = selectedRow.PROVIDER_NAME;
        urlParam.FORM_KEYS = selectedRow.FORM_KEYS;
        urlParam.FORM_PRESENTATION = selectedRow.FORM_PRESENTATION;
        urlParam.FLOWPATH = selectedRow.FLOWPATH;
        urlParam.STATUS = selectedRow.STATUS;
        urlParam.SENDTO_ID = selectedRow.SENDTO_ID;

        $.ajax({
            type: "POST",
            url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
            data: urlParam,
            cache: false,
            async: true,
            success: function (message) {
                gotoInbox(true, 2);
                //alert(message);
                //$(dg).datagrid('loadData', []);
            },
            error: function () {
                return false;
            }
        });
    }
}

function doReject(dataFormId, caption) {
    if ($('.info-form', '#' + dataFormId).form('status') != "view") {
        var message = $.sysmsg('getValue', 'JQWebClient/CheckHasSaved');
        alert(message);
        return;
    }
    if (confirm(flowRejectText)) {
        var urlPrdfix = "";
        if (isSubPath) {
            urlPrdfix = "../";
        }

        var selectedRow = reunionSYS_TODOLIST();
        var urlParam = {};
        urlParam.Type = "Workflow";
        urlParam.Active = "Reject";
        urlParam.LISTID = selectedRow.LISTID;
        urlParam.PROVIDER_NAME = selectedRow.PROVIDER_NAME;
        urlParam.FORM_KEYS = selectedRow.FORM_KEYS;
        urlParam.FORM_PRESENTATION = selectedRow.FORM_PRESENTATION;
        urlParam.FLOWPATH = selectedRow.FLOWPATH;
        urlParam.STATUS = selectedRow.STATUS;
        urlParam.SENDTO_ID = selectedRow.SENDTO_ID;

        $.ajax({
            type: "POST",
            url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
            data: urlParam,
            cache: false,
            async: true,
            success: function (message) {
                alert(message);
                gotoInbox(true);
                //$("#FlowReject", "#" + dataFormId).linkbutton('disable');
                //window.top.FlowRefreshInbox.call();
                //$(dg).datagrid('loadData', []);
            },
            error: function () {
                return false;
            }
        });
    }
}

function doPause(dataFormId, caption, callback) {
    if ($('.info-form', '#' + dataFormId).form('status') != "view") {
        var message = $.sysmsg('getValue', 'JQWebClient/CheckHasSaved');
        alert(message);
        return;
    }
    //if (confirm(flowPauseText)) {
    if (true) {
        var urlPrdfix = "";
        if (isSubPath) {
            urlPrdfix = "../";
        }

        var selectedRow = reunionSYS_TODOLIST();
        //var dataFormId = Request.getQueryStringByName("DataFormId");
        var dataGrid = $(".info-form", "#" + dataFormId).form('options').viewPage;
        var keys = $(".info-datagrid", dataGrid).datagrid('options').keys;
        var keyColumns = "";
        var keyValues = "";
        for (var i = 0; i < keys.length; i++) {
            if (keys[i] != "") {
                var value = $("#" + dataFormId + "_" + keys[i]).val();
                if (value != "") {
                    keyValues += keys[i] + "='" + value + "';";
                }
                keyColumns += keys[i] + ";";
            }
        }
        keyValues = keyValues.substring(0, keyValues.lastIndexOf(";"));
        var urlParam = {};
        urlParam.Type = "Workflow";
        urlParam.Active = "Pause";
        urlParam.LISTID = selectedRow.LISTID;
        urlParam.FLOWFILENAME = Request.getQueryStringByName("FLOWFILENAME");
        urlParam.PROVIDER_NAME = $(".info-form", "#" + dataFormId).form('options').remoteName;
        urlParam.FORM_KEYS = keyColumns;
        urlParam.FORM_PRESENTATION = keyValues;
        urlParam.FLOWPATH = selectedRow.FLOWPATH;
        urlParam.STATUS = selectedRow.STATUS;
        urlParam.SENDTO_ID = selectedRow.SENDTO_ID;
        $.ajax({
            type: "POST",
            url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
            data: urlParam,
            cache: false,
            async: true,
            success: function (message) {
                var message = $.sysmsg('getValue', 'FLClientControls/FLNavigator/PauseSucceed');
                alert(message);
                //$(dg).datagrid('loadData', []);
                if (callback) {
                    callback.call(this);
                }
                gotoInbox(true);
            },
            error: function () {
                return false;
            }
        });
    }
}

function closeSubmit() {
    $("#MobileFormSubmit").dialog("close");
}

String.format = function () {
    if (arguments.length == 0)
        return null;

    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}