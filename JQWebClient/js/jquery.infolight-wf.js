function createAndOpenWorkflowDialog(winId, title, width, height, formUrl, selectedRow, onLoadHandler, dataFormId) {
    if (isSubPath) {
        formUrl = "../" + formUrl;
    }
    $(document.body).find('#' + winId).remove();
    $('<div id="' + winId + '"/>').appendTo('body').dialog({
        title: title,
        width: width,
        height: height,
        modal: true,
        draggable: true,
        closed: true,
        maximized: false,
        minimizable: false,
        collapsible: true,
        'class': "easyui-dialog",
        inline: false,
        zIndex: 9001,
        onLoad: function () {
            $(this).dialog('open');
            if (onLoadHandler != undefined) {
                if (dataFormId != undefined)
                    onLoadHandler(selectedRow, dataFormId, winId);
                else
                    onLoadHandler(selectedRow, undefined, winId);
            }
        },
        onClose: function () {
            var isDone = $("#" + winId).attr("isDone");
            if (isSubPath == true && isDone == "true") {
                var isAutoPageClose = getInfolightOption($("#" + dataFormId)).isAutoPageClose;
                if (isAutoPageClose) {
                    self.parent.closeCurrentTab();
                }
            }
            $(this).remove();
        }
        /*
        buttons: [{
        text: '确定',
        handler: onOkHandler
        }, {
        text: '取消',
        handler: function () {
        $('#' + winId).dialog({
        onClose: function () {
        if (onCancelHandler != undefined) {
        onCancelHandler();
        }
        }
        }).dialog('close');
        }
        }]*/
    }).dialog('refresh', formUrl);



    return;
    $('#' + winId).dialog({
        title: title,
        left: 100,
        top: 50,
        width: width,
        height: height,
        modal: true,
        draggable: true,
        closed: true,
        maximized: false,
        minimizable: false,
        collapsible: true,
        'class': "easyui-dialog",
        inline: false,
        zIndex: 9001,
        onLoad: function () {
            $('#' + winId).dialog('open');
            if (onLoadHandler != undefined) {
                if (dataFormId != undefined)
                    onLoadHandler(selectedRow, dataFormId, winId);
                else
                    onLoadHandler(selectedRow, undefined, winId);
            }
        },
        onClose: function () {
            var isDone = $("#" + winId).attr("isDone");
            if (isSubPath == true && isDone == "true") {
                var isAutoPageClose = getInfolightOption($("#" + dataFormId)).isAutoPageClose;
                if (isAutoPageClose) {
                    self.parent.closeCurrentTab();
                }
            }
        }
        /*
        buttons: [{
            text: '确定',
            handler: onOkHandler
        }, {
            text: '取消',
            handler: function () {
                $('#' + winId).dialog({
                    onClose: function () {
                        if (onCancelHandler != undefined) {
                            onCancelHandler();
                        }
                    }
                }).dialog('close');
            }
        }]*/
    });
    if (isSubPath) {
        formUrl = "../" + formUrl;
    }
    $('#' + winId).dialog('refresh', formUrl);
    $('#' + winId).dialog('open');
}

function reunionSYS_TODOLIST() {
    var newSYS_TODOLIST = {};
    newSYS_TODOLIST["LISTID"] = Request.getQueryStringByName("LISTID");
    newSYS_TODOLIST["FLOW_ID"] = Request.getQueryStringByName("FLOW_ID");
    newSYS_TODOLIST["FLOW_DESC"] = Request.getQueryStringByName("FLOW_DESC");
    newSYS_TODOLIST["APPLICANT"] = Request.getQueryStringByName("APPLICANT");
    newSYS_TODOLIST["S_USER_ID"] = Request.getQueryStringByName("S_USER_ID");
    newSYS_TODOLIST["S_STEP_ID"] = Request.getQueryStringByName("S_STEP_ID");
    newSYS_TODOLIST["S_STEP_DESC"] = Request.getQueryStringByName("S_STEP_DESC");
    newSYS_TODOLIST["D_STEP_ID"] = Request.getQueryStringByName("D_STEP_ID");
    newSYS_TODOLIST["D_STEP_DESC"] = Request.getQueryStringByName("D_STEP_DESC");
    newSYS_TODOLIST["EXP_TIME"] = Request.getQueryStringByName("EXP_TIME");
    newSYS_TODOLIST["URGENT_TIME"] = Request.getQueryStringByName("URGENT_TIME");
    newSYS_TODOLIST["TIME_UNIT"] = Request.getQueryStringByName("TIME_UNIT");
    newSYS_TODOLIST["USERNAME"] = Request.getQueryStringByName("USERNAME");
    newSYS_TODOLIST["FORM_NAME"] = Request.getQueryStringByName("FORM_NAME");
    newSYS_TODOLIST["NAVIGATOR_MODE"] = Request.getQueryStringByName("NAVIGATOR_MODE");
    newSYS_TODOLIST["FLNAVIGATOR_MODE"] = Request.getQueryStringByName("FLNAVIGATOR_MODE");
    newSYS_TODOLIST["PARAMETERS"] = Request.getQueryStringByName("PARAMETERS");
    newSYS_TODOLIST["SENDTO_KIND"] = Request.getQueryStringByName("SENDTO_KIND");
    newSYS_TODOLIST["SENDTO_ID"] = Request.getQueryStringByName("SENDTO_ID");
    newSYS_TODOLIST["FLOWIMPORTANT"] = Request.getQueryStringByName("FLOWIMPORTANT");
    newSYS_TODOLIST["FLOWURGENT"] = Request.getQueryStringByName("FLOWURGENT");
    newSYS_TODOLIST["STATUS"] = Request.getQueryStringByName("STATUS");
    newSYS_TODOLIST["FORM_TABLE"] = Request.getQueryStringByName("FORM_TABLE");
    newSYS_TODOLIST["FORM_KEYS"] = Request.getQueryStringByName("FORM_KEYS");
    newSYS_TODOLIST["FORM_PRESENTATION"] = Request.getQueryStringByName("FORM_PRESENTATION");
    newSYS_TODOLIST["FORM_PRESENT_CT"] = Request.getQueryStringByName("FORM_PRESENT_CT");
    newSYS_TODOLIST["REMARK"] = Request.getQueryStringByName("REMARK");
    newSYS_TODOLIST["PROVIDER_NAME"] = Request.getQueryStringByName("PROVIDER_NAME");
    newSYS_TODOLIST["VERSION"] = Request.getQueryStringByName("VERSION");
    newSYS_TODOLIST["EMAIL_ADD"] = Request.getQueryStringByName("EMAIL_ADD");
    newSYS_TODOLIST["EMAIL_STATUS"] = Request.getQueryStringByName("EMAIL_STATUS");
    newSYS_TODOLIST["VDSNAME"] = Request.getQueryStringByName("VDSNAME");
    newSYS_TODOLIST["SENDBACKSTEP"] = Request.getQueryStringByName("SENDBACKSTEP");
    newSYS_TODOLIST["LEVEL_NO"] = Request.getQueryStringByName("LEVEL_NO");
    newSYS_TODOLIST["WEBFORM_NAME"] = Request.getQueryStringByName("WEBFORM_NAME");
    newSYS_TODOLIST["UPDATE_DATE"] = Request.getQueryStringByName("UPDATE_DATE");
    newSYS_TODOLIST["UPDATE_TIME"] = Request.getQueryStringByName("UPDATE_TIME");
    newSYS_TODOLIST["FLOWPATH"] = Request.getQueryStringByName("FLOWPATH");
    newSYS_TODOLIST["PLUSAPPROVE"] = Request.getQueryStringByName("PLUSAPPROVE");
    newSYS_TODOLIST["PLUSROLES"] = Request.getQueryStringByName("PLUSROLES");
    newSYS_TODOLIST["MULTISTEPRETURN"] = Request.getQueryStringByName("MULTISTEPRETURN");
    newSYS_TODOLIST["SENDTO_NAME"] = Request.getQueryStringByName("SENDTO_NAME");
    newSYS_TODOLIST["ATTACHMENTS"] = Request.getQueryStringByName("ATTACHMENTS");
    newSYS_TODOLIST["CREATE_TIME"] = Request.getQueryStringByName("CREATE_TIME");
    return newSYS_TODOLIST;
}

function CheckHasSaved(dataFormId) {
    if (dataFormId != undefined) {
        var mode = getEditMode($("#" + dataFormId));
        if (mode != "viewed") {
            //var message = $.sysmsg('getValue', 'JQWebClient/CheckHasSaved');
            //alert(message);
            return false;
        }
    }

    return true;
}

function doSubmit(winId, dataFormId, winTitle, selectedRow) {
    //if (CheckHasSaved(dataFormId)) {
    //    if (selectedRow == undefined)
    //        selectedRow = reunionSYS_TODOLIST();
    //    createAndOpenWorkflowDialog(winId, winTitle, 550, 400, "InnerPages/FormSubmit.html", selectedRow, formSubmitLoaded, dataFormId);
    //}

    if (CheckHasSaved(dataFormId)) {
        if (selectedRow == undefined)
            selectedRow = reunionSYS_TODOLIST();
        createAndOpenWorkflowDialog(winId, winTitle, 550, 400, "InnerPages/FormSubmit.html", selectedRow, formSubmitLoaded, dataFormId);
    }
    else {
        var did = "";
        if ($("#" + dataFormId).closest(".easyui-dialog").length == 0)
            did = $("#" + dataFormId).closest(".info-dialog").attr("id");
        else
            did = $("#" + dataFormId).closest(".easyui-dialog").attr("id");
        submitForm("#" + did, undefined, undefined, doSubmit, winId, dataFormId, winTitle, selectedRow);
    }
}

function doApprove(winId, dataFormId, winTitle, selectedRow) {
    if (CheckHasSaved(dataFormId)) {
        if (selectedRow == undefined)
            selectedRow = reunionSYS_TODOLIST();
        createAndOpenWorkflowDialog(winId, winTitle, 550, 400, "InnerPages/FormApprove.html", selectedRow, formApproveLoaded, dataFormId);
    }
    else {
        var did = "";
        if ($("#" + dataFormId).closest(".easyui-dialog").length == 0)
            did = $("#" + dataFormId).closest(".info-dialog").attr("id");
        else
            did = $("#" + dataFormId).closest(".easyui-dialog").attr("id");
        submitForm("#" + did, undefined, undefined, doApprove, winId, dataFormId, winTitle, selectedRow);
    }
}

function doReturn(winId, dataFormId, winTitle, selectedRow) {
    if (CheckHasSaved(dataFormId)) {
        if (selectedRow == undefined)
            selectedRow = reunionSYS_TODOLIST();
        createAndOpenWorkflowDialog(winId, winTitle, 550, 400, "InnerPages/FormReturn.html", selectedRow, formReturnLoaded, dataFormId);
    }
    else {
        var did = "";
        if ($("#" + dataFormId).closest(".easyui-dialog").length == 0)
            did = $("#" + dataFormId).closest(".info-dialog").attr("id");
        else
            did = $("#" + dataFormId).closest(".easyui-dialog").attr("id");
        submitForm("#" + did, undefined, undefined, doReturn, winId, dataFormId, winTitle, selectedRow);
    }
    //if (CheckHasSaved(dataFormId)) {
    //    if (selectedRow == undefined)
    //        selectedRow = reunionSYS_TODOLIST();
    //    createAndOpenWorkflowDialog(winId, winTitle, 550, 400, "InnerPages/FormReturn.html", selectedRow, formReturnLoaded, dataFormId);
    //}
}

function doNotify(winId, dataFormId, winTitle, selectedRow) {
    if (CheckHasSaved(dataFormId)) {
        if (selectedRow == undefined)
            selectedRow = reunionSYS_TODOLIST();
        createAndOpenWorkflowDialog(winId, winTitle, 475, 455, "InnerPages/FormNotify.html", selectedRow, formNotifyLoaded, dataFormId);
    }
}

function doPlusApprove(winId, dataFormId, winTitle, selectedRow) {
    if (CheckHasSaved(dataFormId)) {
        if (selectedRow == undefined)
            selectedRow = reunionSYS_TODOLIST();
        createAndOpenWorkflowDialog(winId, winTitle, 485, 465, "InnerPages/FormPlusApprove.html", selectedRow, formPlusLoaded, dataFormId);
    }
}

function doFlowDelete(dg, dataFormId, winTitle, selectedRow) {
    if (CheckHasSaved(dataFormId)) {
        if (confirm(flowDeleteText)) {
            var urlPrdfix = "";
            if (isSubPath) {
                urlPrdfix = "../";
            }

            if (selectedRow == undefined)
                selectedRow = reunionSYS_TODOLIST();
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
                async: false,
                success: function (message) {

                },
                error: function () {
                    return false;
                }
            });

            window.top.FlowRefreshNotify.call();
            try {
                self.parent.closeCurrentTab();
            } catch (ex) { }
        }
    }
}

function doReject(dg, dataFormId, winTitle, selectedRow, notify) {
    //if (CheckHasSaved(dataFormId)) {
    $.messager.prompt('Confrim', flowRejectText, function (r) {
        if (r != undefined) {
            var urlPrdfix = "";
            if (isSubPath) {
                urlPrdfix = "../";
            }

            if (selectedRow == undefined)
                selectedRow = reunionSYS_TODOLIST();
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
            urlParam.suggest = encodeURIComponent(r);

            if (!notify) {
                if (dataFormId) {
                    var dataForm = $('#' + dataFormId);
                    var infolightOptions = getInfolightOption(dataForm);
                    notify = infolightOptions.rejectNotify
                }
            }
            if (notify) {
                urlParam.NotifyAllRoles = "true";
            }

            $.ajax({
                type: "POST",
                url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
                data: urlParam,
                cache: false,
                async: false,
                success: function (message) {
                    alert(message);
                    $("#FlowReject", "#" + dataFormId).linkbutton('disable');
                    var did = "";
                    if ($("#" + dataFormId).closest(".easyui-dialog").length == 0)
                        did = $("#" + dataFormId).closest(".info-dialog").attr("id");
                    else
                        did = $("#" + dataFormId).closest(".easyui-dialog").attr("id");
                    window.top.FlowRefreshInbox.call();
                    //closeForm("#" + did);
                },
                error: function () {
                    return false;
                }
            });
            var isAutoPageClose = getInfolightOption($("#" + dataFormId)).isAutoPageClose;
            if (isAutoPageClose) {
                self.parent.closeCurrentTab();
            }
        }
    });
    //}
}

function doPause(dg, dataFormId, winTitle, selectedRow, approve) {
    if ($("#FlowSubmit").length > 0 && $("#FlowSubmit").attr('onclick') == undefined) {
        return;
    }

    if (CheckHasSaved(dataFormId)) {
        //if (confirm(flowPauseText)) {
        if (true) {
            var urlPrdfix = "";
            if (isSubPath) {
                urlPrdfix = "../";
            }

            if (selectedRow == undefined)
                selectedRow = reunionSYS_TODOLIST();
            var dataForm = $('#' + dataFormId);
            var infolightOptions = getInfolightOption(dataForm);
            var keys = "";
            var dialoggrid = dataForm.attr('dialogGrid');
            if (dialoggrid == undefined) dialoggrid = dataForm.attr('switchGrid');
            if (dialoggrid == undefined) dialoggrid = dataForm.attr('continueGrid');
            var key = $(dialoggrid).attr('keyColumns');
            var keyValues = "";
            var keys = key.split(",");
            for (var i = 0; i < keys.length; i++) {
                if (keys[i] != "") {
                    var control = $("#" + dataForm.attr("id") + keys[i]);
                    var controlClass = control.attr('class');
                    if (controlClass != undefined) {
                        if (controlClass.indexOf('easyui-datebox') == 0) {
                            value = control.datebox('getBindingValue');
                        }
                        else if (controlClass.indexOf('easyui-combobox') == 0) {
                            value = control.combobox('getValue');
                        }
                        else if (controlClass.indexOf('easyui-datetimebox') == 0) {
                            value = control.datetimebox('getBindingValue');
                        }
                        else if (controlClass.indexOf('easyui-combogrid') == 0) {
                            value = control.combogrid('getValue');
                        }
                        else if (controlClass.indexOf('info-combobox') == 0) {
                            value = control.combobox('getValue');
                        }
                        else if (controlClass.indexOf('info-combogrid') == 0) {
                            value = control.combogrid('getValue');
                        }
                        else if (controlClass.indexOf('info-refval') == 0) {
                            value = control.refval('getValue');
                        }
                        else {
                            value = control.val();
                        }
                    }
                    else {
                        value = control.val();
                    }

                    if (value != "") {
                        //keyValues += tableName + "." + keys[i] + "='" + value + "';";
                        keyValues += keys[i] + "='" + value + "';";
                    }
                }
            }
            keyValues = keyValues.substring(0, keyValues.lastIndexOf(";"));
            var urlParam = {};
            urlParam.Type = "Workflow";
            urlParam.Active = "Pause";
            urlParam.LISTID = selectedRow.LISTID;
            urlParam.FLOWFILENAME = Request.getQueryStringByName("FLOWFILENAME");
            urlParam.PROVIDER_NAME = infolightOptions.remoteName;
            urlParam.FORM_KEYS = key;
            urlParam.FORM_PRESENTATION = keyValues;
            urlParam.FLOWPATH = selectedRow.FLOWPATH;
            urlParam.STATUS = selectedRow.STATUS;
            urlParam.SENDTO_ID = selectedRow.SENDTO_ID;

            $.messager.progress({ title: 'Please waiting', msg: 'Pausing...' });
            $.ajax({
                type: "POST",
                url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
                data: urlParam,
                cache: false,
                async: true,
                success: function (data) {
                    $('#FlowPause').hide();
                    //$('#FlowSubmit').hide();

                    var listID = data;
                    $.ajax({
                        type: "POST",
                        dataType: 'json',
                        url: '../handler/SystemHandle_Flow.ashx',
                        data: { listID: listID, Type: "ToDoList" },
                        cache: false,
                        async: true,
                        success: function (data) {
                            if (data.rows.length > 0) {
                                var selectedRow = data.rows[0]
                                var jqMobileUIText = $.sysmsg('getValue', 'Web/JQMobile/UIText');
                                var jqMobileUITexts = jqMobileUIText.split(';');
                                var title = jqMobileUITexts[6];
                                $("#FlowSubmit").removeAttr('onclick');
                                $("#FlowSubmit").unbind('click').bind('click', function () {
                                    doApprove('winApprove', dataFormId, title, selectedRow);
                                });
                                if (approve) {
                                    doApprove('winApprove', dataFormId, title, selectedRow);
                                }
                                $("#FlowReject").removeAttr('onclick');
                                $("#FlowReject").unbind('click').bind('click', function () {
                                    doReject('winNotify', dataFormId, $("#FlowReject").attr(title), selectedRow);
                                });

                                var sumbitButton = dataForm.parent().next('div').children('#DialogSubmit');
                                if (sumbitButton.length == 0) {
                                    sumbitButton = $('#DialogSubmit');
                                }
                                setTimeout(function () {
                                    sumbitButton.attr('onclick', '');
                                    sumbitButton.unbind('click').bind('click', function () {
                                        submitForm('#' + dataForm.closest('.info-dialog,.easyui-dialog').attr('id'), undefined, function () { doApprove('winApprove', dataFormId, title, selectedRow); });
                                    });
                                }, 500);
                            }
                        }
                    });
                    if (!approve) {
                        var message = $.sysmsg('getValue', 'FLClientControls/FLNavigator/PauseSucceed');
                        alert(message);
                    }
                    //$(dg).datagrid('loadData', []);
                },
                error: function () {
                    return false;
                },
                complete: function () {
                    $.messager.progress('close');
                }
            });
        }
    }
}

//function doReject(dg, dataFormId, winTitle) {
//    if (confirm(flowRejectText)) {
//        var urlPrdfix = "";
//        if (isSubPath) {
//            urlPrdfix = "../";
//        }

//        var selectedRow = reunionSYS_TODOLIST();
//        var urlParam = "?type=Workflow&active=Reject";
//        urlParam += "&LISTID=" + selectedRow.LISTID;
//        urlParam += "&PROVIDER_NAME=" + selectedRow.PROVIDER_NAME;
//        urlParam += "&FORM_KEYS=" + selectedRow.FORM_KEYS;
//        urlParam += "&FORM_PRESENTATION=" + selectedRow.FORM_PRESENTATION;
//        urlParam += "&FLOWPATH=" + selectedRow.FLOWPATH;
//        urlParam += "&STATUS=" + selectedRow.STATUS;
//        urlParam += "&SENDTO_ID=" + selectedRow.SENDTO_ID;

//        $.ajax({
//            type: "POST",
//            url: urlPrdfix + 'handler/SystemHandle_Flow.ashx' + encodeURI(urlParam),
//            cache: false,
//            async: true,
//            success: function (message) {
//                //alert(message);
//                $(dg).datagrid('loadData', []);
//            },
//            error: function () {
//                return false;
//            }
//        });
//    }
//}

function doComment(winId, dataFormId, winTitle, selectedRow) {
    if (selectedRow == undefined)
        selectedRow = reunionSYS_TODOLIST();
    createAndOpenWorkflowDialog(winId, winTitle, 720, 400, "InnerPages/FormComment.html", selectedRow, formCommentLoaded, dataFormId);
}

function doHasten(winId, dataFormId, winTitle, selectedRow) {
    if (CheckHasSaved(dataFormId)) {
        if (selectedRow == undefined)
            selectedRow = reunionSYS_TODOLIST();
        createAndOpenWorkflowDialog(winId, winTitle, 475, 200, "InnerPages/FormHasten.html", selectedRow, formHastenLoaded);
    }
}

function doFlowQuery(winId, dataFormId, winTitle, selectedRow) {
    if (selectedRow == undefined)
        selectedRow = reunionSYS_TODOLIST();
    createAndOpenWorkflowDialog(winId, winTitle, 610, 200, "InnerPages/FormFlowQuery.html", "dgInbox", formFlowQueryLoaded);
}


function openPreview(url) {
    var uiText = $.sysmsg('getValue', 'FLClientControls/SubmitConfirm/UIText');
    var uiTexts = uiText.split(',');

    var height = $(window).height() - 20;
    var width = $(window).width() - 20;
    var dialog = $('<div/>').html('<iframe style="border: 0px;" src="' + url + '" width="100%" height="100%"></iframe>').appendTo('body')
    .dialog({
        draggable: false,
        modal: true,
        height: height,
        width: width,
        title: uiTexts[14]//,
        //maximizable: true
    });
    dialog.find('.panel-body').css('overflow-y', 'hidden');
    dialog.dialog('open');
}