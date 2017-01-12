$.fn.flow = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.flow.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            $(this).flow('initialize');
            if (!$(this).hasClass($.fn.flow.class)) {
                $(this).addClass($.fn.flow.class)
            }
        });
    }
};

$.fn.flow.class = 'info-flow';

$.fn.flow.defaults = {
    ToDoListText: "Todo",
    ToDoHisText: "History",
    NotifyText: "Notify",
    DelayText: "Delay",
    SubmitText: 'Submit',
    ApproveText: 'Approve',
    ReturnText: 'Return',
    RejectText: 'Reject',
    PlusText: 'Plus',
    NotifyText: 'Notify',
    FlowDeleteText: 'Delete',
    PauseText: 'Pause',
    CommentText: 'Comment',
    UploadText: 'Upload File',

    FLOW_DESC: 'flow',
    D_STEP_ID: 'work name',
    S_STEP_ID: 'work name',
    USERNAME: 'sender',
    SENDTO_NAME: 'send to',
    STATUS: 'status',
    FORM_PRESENT_CT: 'flow condition',
    REMARK: 'infomation',
    UPDATE_WHOLE_TIME: 'update date',
    URGENT: 'Urgent',
    IMPORTANT: 'Important',

    Info:' '
};


$.fn.flow.methods = {
    initialize: function (jq) {
        jq.each(function () {
            $("a.menu").setText($.fn.main.defaults.menuText);
            $("a.flow").setText($.fn.main.defaults.flowText);
            $("a.logout").setText($.fn.main.defaults.logoutText);
            $("a.message").setText($.fn.main.defaults.messageText);
            $("a.password").setText($.fn.main.defaults.changePasswordText);
            $('a.todolist').addClass("ui-btn-active");

            $.ajax({
                type: "POST",
                dataType: 'text',
                url: $.getSystemUrl(),
                data: { mode: 'getMessageCount' },
                cache: false,
                async: true,
                success: function (data) {
                    if (data != '0') {
                        $('<span class="ui-li-count ui-body-inherit" style="right:inherit;left:150px;top:10px;background-color:#1b97d7;color:#ffffff;border-color:#08acdb">' + data + '</span>').appendTo('.btn_message');
                    }
                }
            });
            $(this).flow('refresh', 'ToDoList');

            var NavText = $.sysmsgCordova('getValue', 'Web/webClientMainFlow/UIText');
            var NavTexts = NavText.split(';');

            $('a.todolist', this).setText(NavTexts[2]);
            $('a.todohis', this).setText(NavTexts[3]);
            $('a.notify', this).setText(NavTexts[16]);
            $('a.delay', this).setText(NavTexts[4]);

            $('.open', '.ToDoListPopUp').setText(NavTexts[9]);
            $('.approve', '.ToDoListPopUp').setText(NavTexts[6]);
            $('.return', '.ToDoListPopUp').setText(NavTexts[8]);

            $('.open', '.ToDoHisPopUp').setText(NavTexts[9]);
            $('.retake', '.ToDoHisPopUp').setText(NavTexts[14]);
            $('.hasten', '.ToDoHisPopUp').setText(NavTexts[18]);

            $('.open', '.NotifyPopUp').setText(NavTexts[9]);
            $('.delete', '.NotifyPopUp').setText(NavTexts[15]);

            $('.open', '.DelayPopUp').setText(NavTexts[9]);


            var ToDoListColumn = $.sysmsgCordova('getValue', 'EEPNetClient/FrmClientMain/ToDoListColumns');
            var ToDoListColumns = ToDoListColumn.split(',');
            var columns = ['S_STEP_ID', 'USERNAME', 'STATUS', 'REMARK', 'UPDATE_WHOLE_TIME'];
            $.fn.flow.defaults[columns[0]] = ToDoListColumns[1];
            $.fn.flow.defaults[columns[1]] = ToDoListColumns[3];
            $.fn.flow.defaults[columns[2]] = ToDoListColumns[6];
            $.fn.flow.defaults[columns[3]] = ToDoListColumns[4];
            $.fn.flow.defaults[columns[4]] = ToDoListColumns[5];

            var NavText = $.sysmsgCordova('getValue', 'FLClientControls/FLNavigator/NavText');
            var NavTexts = NavText.split(';');
            $('h1.SubmitTitle').setText(NavTexts[16]);
            $('h1.ApproveTitle').setText(NavTexts[17]);
            $('h1.ReturnTitle').setText(NavTexts[18]);
            $('h1.NotifyTitle').setText(NavTexts[20]);
            $('h1.PlusTitle').setText(NavTexts[22]);



        });
    },
    refresh: function (jq, type) {
        jq.each(function () {
            $('ul.flow').empty();
            var flow = $(this);
            var url = webSiteUrl + '/handler/SystemHandle_Flow2.ashx';
            var data = { type: type };
            if (type == 'Delay') {
                data.Level = '0';
            }
            //loading
            $.mobile.loading('show', { theme: 'b', text: $.fn.datagrid.defaults.loadingMessage, textVisible: true });
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                cache: false,
                async: true,
                success: function (data) {
                    var rows = eval('(' + data + ')');
                    var titleLi = '<li data-role="list-divider">' + $.fn.flow.defaults[type + 'Text'] + '<span class="ui-li-count">' + rows.length + '</span></li>';
                    $(titleLi).appendTo('ul.flow');
                    for (var i = 0; i < rows.length; i++) {
                        if (type == "ToDoList") {
                            $(flow.flow('createItem', { row: rows[i], titleColumn: 'FLOW_DESC', columns: ['D_STEP_ID', 'USERNAME', 'STATUS', 'FORM_PRESENT_CT', 'REMARK', 'UPDATE_WHOLE_TIME'] })).appendTo('ul.flow')
                            .data('row', rows[i]);
                        }
                        else if (type == "ToDoHis") {
                            rows[i].FLNAVMODE = 6;
                            rows[i].NAVMODE = 0;
                            $(flow.flow('createItem', { row: rows[i], titleColumn: 'FLOW_DESC', columns: ['D_STEP_ID', 'SENDTO_NAME', 'STATUS', 'FORM_PRESENT_CT', 'REMARK', 'UPDATE_WHOLE_TIME'] })).appendTo('ul.flow')
                            .data('row', rows[i]);
                        }
                        else {
                            $(flow.flow('createItem', { row: rows[i], titleColumn: 'FLOW_DESC', columns: ['D_STEP_ID', 'SENDTO_NAME', 'STATUS', 'FORM_PRESENT_CT', 'REMARK', 'UPDATE_WHOLE_TIME'] })).appendTo('ul.flow')
                            .data('row', rows[i]);
                        }
                    }
                    $("ul.flow").listview("refresh");
                    $("ul.flow").find('.popupSource').click(function () {
                        var popUp = $('.' + type + 'PopUp');
                        var selectedRow = $(this).closest('li').data('row');
                        if (popUp.find(".return").length > 0) {
                            if (selectedRow.FLNAVIGATOR_MODE == "0" || selectedRow.FLNAVIGATOR_MODE == "5")
                            {
                                popUp.find(".return").css({ "display": "none" });
                               
                            }
                            else if (selectedRow.PLUSROLES != "") {
                                popUp.find(".return").css({ "display": "none" });
                            }
                            else if (selectedRow.STATUS == "NP") {
                                popUp.find(".return").css({ "display": "none" });
                            }
                            else {
                                popUp.find(".return").css({ "display": "inherit" });
                            }
                        }

                        if (selectedRow.PLUSROLES == "") {
                            popUp.find(".approve").css({ "display": "inherit" });
                            //popUp.find(".return").css({ "display": "inherit" });
                        }
                        else {
                            popUp.find(".approve").css({ "display": "none" });
                            //popUp.find(".return").css({ "display": "none" });
                        }
                       

                        if (popUp.find(".retake").length > 0) {
                            if (RetakeVisible(selectedRow) == false) {
                                popUp.find(".retake").css({ "display": "none" });
                            }
                            else
                            {
                                popUp.find(".retake").css({ "display": "inherit" });
                            }
                        }
                        popUp.popup('open', { positionTo: $(this) });
                        var urlParam = "?IsWorkflow=1";
                        for (var field in selectedRow) {
                            urlParam += "&" + field + "=" + selectedRow[field];
                        }
                        var url = selectedRow.WEBFORM_NAME.replace(".", "/") + ".html" + urlParam;
                        popUp.find('a.open').unbind().click(function () {
                            window.sessionStorage.setItem('flowrow', $.toJSONString(selectedRow));
                            window.location.href = url;
                        });
                        popUp.find('a.approve').unbind().click(function () {
                            window.sessionStorage.setItem('flowrow', $.toJSONString(selectedRow));
                            $.mobile.changePage("approve.html", { transition: "slide" });
                        });
                        popUp.find('a.return').unbind().click(function () {
                            window.sessionStorage.setItem('flowrow', $.toJSONString(selectedRow));
                            $.mobile.changePage("return.html", { transition: "slide" });
                        });
                        popUp.find('a.hasten').unbind().click(function () {
                            window.sessionStorage.setItem('flowrow', $.toJSONString(selectedRow));
                            $.mobile.changePage("hasten.html", { transition: "slide" });
                        });
                        popUp.find('a.retake').unbind().click(function () {
                            flow.flow('retakeFlow', selectedRow);
                        });
                        popUp.find('a.delete').unbind().click(function () {
                            flow.flow('deleteFlow', selectedRow);
                        });
                    });
                },
                complete: function () {
                    $.mobile.loading('hide');
                }
            });
        });
    },
    createItem: function (jq, parameters) {
        if (jq.length > 0) {
            var itemLi = '<li data-role="collapsible" data-iconpos="right" data-shadow="false" data-corners="false">';
            itemLi += '<a class="popupSource">';
            itemLi += '<h2>';
            itemLi += '<p style="margin-top:2px;margin-bottom:2px">' + $.fn.flow.defaults[parameters.titleColumn] + ': ' + parameters.row[parameters.titleColumn] + '</p>';
            itemLi += '</h2>';
            itemLi += '<table style="width: 100%" class=\"popupSource\">';
            for (var i = 0; i < parameters.columns.length; i++) {
                if (i % 2 == 0) {
                    itemLi += '<tr>';
                }
                var caption = $.fn.flow.defaults[parameters.columns[i]];
                if (!caption) {
                    caption = parameters.columns[i];
                }

                itemLi += '<td style="width: 50%"><p class="ui-li-desc" style="margin-top:2px;margin-bottom:2px">' + caption + ': ' + parameters.row[parameters.columns[i]] + '</p></td>';
                if (i % 2 == 1) {
                    itemLi += '</tr>'
                }
            }
            itemLi += '</table>';
            itemLi += '</a>';
            itemLi += '</li>';
            return itemLi
        }
    },
    pauseFlow: function (jq, selectedRow) {
        if (jq.length > 0) {
            var data = { type: 'Workflow', active: 'Pause' };

            data.FLOWFILENAME = selectedRow.FLOWFILENAME;
            data.PROVIDER_NAME = selectedRow.PROVIDER_NAME;
            data.FORM_PRESENTATION = selectedRow.FORM_PRESENTATION.replace(/''/g, "'");;
            data.FLOWPATH = selectedRow.FLOWPATH;
            data.STATUS = selectedRow.STATUS;
            data.SENDTO_ID = selectedRow.SENDTO_ID;
            data.FORM_KEYS = selectedRow.FORM_KEYS;
            $.ajax({
                type: "POST",
                url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                data: data,
                cache: false,
                async: true,
                success: function (message) {
                    var PauseSucceed = $.sysmsgCordova('getValue', 'FLClientControls/FLNavigator/PauseSucceed');
                    if (navigator.notification) {
                        navigator.notification.alert(
                            PauseSucceed,
                            function () {  location.href = '../mainflow.html'; },
                            $.fn.flow.defaults["Info"],
                            $.fn.form.defaults.okText
                        );
                    }
                    else {
                        window.alert(PauseSucceed);
                         location.href = '../mainflow.html';
                    }
                },
                error: function () {
                    return false;
                }
            });
        }
    },
    retakeFlow: function (jq, selectedRow) {
        if (jq.length > 0) {
            //var urlParam = '?type=Workflow&active=Retake&LISTID=' + selectedRow.LISTID + '&D_STEP_ID=' + selectedRow.D_STEP_ID;
            $.ajax({
                type: "POST",
                url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                data: { type: 'Workflow', active: 'Retake', LISTID: selectedRow.LISTID, D_STEP_ID: selectedRow.D_STEP_ID },
                cache: false,
                async: true,
                success: function (message) {
                    if (navigator.notification) {
                        navigator.notification.alert(
                            message,
                            function () { },
                            $.fn.flow.defaults["Info"],
                            $.fn.form.defaults.okText
                        );
                    }
                    else {
                        window.alert(message);
                    }
                    $('.ToDoHisPopUp').popup('close');
                    $(jq).flow('refresh', 'ToDoHis');
                },
                error: function () {
                    return false;
                }
            });
        }
    },
    deleteFlow: function (jq, selectedRow) {
        if (jq.length > 0) {
            var data = { type: 'Workflow', active: 'FlowDelete' };

            data.LISTID = selectedRow.LISTID;
            data.PROVIDER_NAME = selectedRow.PROVIDER_NAME;
            data.FORM_PRESENTATION = selectedRow.FORM_PRESENTATION.replace(/''/g, "'");;
            data.FLOWPATH = selectedRow.FLOWPATH;
            data.STATUS = selectedRow.STATUS;
            data.SENDTO_ID = selectedRow.SENDTO_ID;
            data.FORM_KEYS = selectedRow.FORM_KEYS;

            $.ajax({
                type: "POST",
                url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                data: data,
                cache: false,
                async: true,
                success: function (message) {
                    $('.NotifyPopUp').popup('close');
                    $(jq).flow('refresh', 'Notify');
                },
                error: function (er) {
                    return false;
                }
            });
        }
    },
    rejectFlow: function (jq, selectedRow) {
        var data = { type: 'Workflow', active: 'Reject' };

        data.LISTID = selectedRow.LISTID;
        data.PROVIDER_NAME = selectedRow.PROVIDER_NAME;
        data.FORM_PRESENTATION = selectedRow.FORM_PRESENTATION.replace(/''/g, "'");;
        data.FLOWPATH = selectedRow.FLOWPATH;
        data.STATUS = selectedRow.STATUS;
        data.SENDTO_ID = selectedRow.SENDTO_ID;
        data.FORM_KEYS = selectedRow.FORM_KEYS;

        $.ajax({
            type: "POST",
            url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
            data: data,
            cache: false,
            async: true,
            success: function (message) {
                alert(message);
                 location.href = '../mainflow.html';
            },
            error: function (er) {
                return false;
            }
        });
    }
};


$.fn.flowpage = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.flowpage.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            $(this).flowpage('initialize');
            if (!$(this).hasClass($.fn.flowpage.class)) {
                $(this).addClass($.fn.flowpage.class)
            }
        });
    }
};

$.fn.flowpage.class = 'info-flowpage';

$.fn.flowpage.methods = {
    initialize: function (jq) {
        jq.each(function () {


            //$("a.menu").setText($.fn.main.defaults.menuText);
            //$("a.flow").setText($.fn.main.defaults.flowText);
            //$('a.todolist').addClass("ui-btn-active");

            //$('a.todolist', this).setText($.fn.flow.defaults.ToDoListText);
            //$('a.todohis', this).setText($.fn.flow.defaults.ToDoHisText);
            //$('a.notify', this).setText($.fn.flow.defaults.NotifyText);
            //$('a.delay', this).setText($.fn.flow.defaults.DelayText);
            $('label.suggest', this).setText($.fn.flow.defaults.REMARK);
            $('label.important', this).setText($.fn.flow.defaults.IMPORTANT);
            $('label.urgent', this).setText($.fn.flow.defaults.URGENT);
            $('a.ok', this).setText($.fn.form.defaults.okText);
            $('a.close', this).setText($.fn.form.defaults.cancelText);

            var selectedRow = $.parseJSON(window.sessionStorage.getItem('flowrow'));
            $(this).flowpage('load', selectedRow);

            var UIText = $.sysmsgCordova('getValue', 'FLClientControls/SubmitConfirm/UIText');
            var UITexts = UIText.split(',');
            $('label.suggest', this).setText(UITexts[3]);
            $('label.important', this).setText(UITexts[1]);
            $('label.urgent', this).setText(UITexts[2]);
            $('label.remark', this).setText(UITexts[3]);
            $('label.role', this).setText(UITexts[5]);
            $('label.returnTo', this).setText(UITexts[10]);
            $('a.ok', this).setText(UITexts[6]);
            $('a.close', this).setText(UITexts[7]);
            $('a.preview', this).setText(UITexts[14]);
            $('a.file', this).setText(UITexts[12]);

            var NavText = $.sysmsgCordova('getValue', 'FLClientControls/FLNavigator/NavText');
            var NavTexts = NavText.split(';');
            $('h1.SubmitTitle').setText(NavTexts[16]);
            $('h1.ApproveTitle').setText(NavTexts[17]);
            $('h1.ReturnTitle').setText(NavTexts[18]);
            $('h1.NotifyTitle').setText(NavTexts[20]);
            $('h1.PlusTitle').setText(NavTexts[22]);
        });
    },
    load: function (jq, selectedRow) {
        jq.each(function () {
            var page = $(this);
            $('input.important', this).each(function () {
                if (selectedRow.FLOWIMPORTANT == "1") {
                    $(this).prop('checked', 'checked');
                }
                else {
                    $(this).prop('checked', false);
                }

            });
            $('input.urgent', this).each(function () {
                if (selectedRow.FLOWURGENT == "1") {
                    $(this).prop('checked', 'checked');
                }
                else {
                    $(this).prop('checked', false);
                }

            });
            $('select.role', this).each(function () {
                var roleSelect = $(this);
                var data = { type: 'ddlRoles' };
                var url = webSiteUrl + '/handler/SystemHandle_Flow2.ashx';
                if (selectedRow.LISTID) {
                    data.LISTID = selectedRow.LISTID
                }
                $.ajax({
                    type: "POST",
                    url: url,
                    data: data,
                    cache: false,
                    async: true,
                    success: function (data) {
                        var roles = $.parseJSON(data);
                        roleSelect.empty();
                        for (var i = 0; i < roles.length; i++) {
                            roleSelect.append($.createOption(roles[i].GROUPNAME, roles[i].GROUPID));
                        }
                        roleSelect.selectmenu("refresh");
                        if (selectedRow.SENDTO_KIND == "1") {
                            roleSelect.val(selectedRow.SENDTO_ID);
                            roleSelect.selectmenu('disable');
                        }
                    },
                    error: function (data) { }
                });
            });
            $('ul.role', this).each(function () {
                var roleUl = $(this);
                $.ajax({
                    type: "POST",
                    url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                    data: { type: 'lstRolesFrom' },
                    cache: false,
                    async: true,
                    success: function (data) {
                        var roles = $.parseJSON(data);
                        for (var i = 0; i < roles.length; i++) {
                            roleUl.append("<li><input class=\"info-checkbox\" type=\"checkbox\" id=\"" + roles[i].GROUPID + "\" /><a>" + roles[i].GROUPID + "(" + roles[i].GROUPNAME + ")</a></li>");
                        }
                        roleUl.listview("refresh");
                    },
                    error: function (data) {

                    }
                });
            });
            $('ul.user', this).each(function () {
                var userUl = $(this);
                $.ajax({
                    type: "POST",
                    url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                    data: { type: 'lstUsersFrom' },
                    cache: false,
                    async: true,
                    success: function (data) {
                        var users = $.parseJSON(data);
                        for (var i = 0; i < users.length; i++) {
                            userUl.append("<li><input class=\"info-checkbox\" type=\"checkbox\" id=\"" + users[i].USERID + "\" /><a>" + users[i].USERID + "(" + users[i].USERNAME + ")</a></li>");
                        }
                        userUl.listview("refresh");
                    },
                    error: function (data) { }
                });
            });
            $('select.step', this).each(function () {
                var stepSelect = $(this);
                $.ajax({
                    type: "POST",
                    url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                    data: { type: 'ddlReturnStep', LISTID: selectedRow.LISTID },
                    cache: false,
                    async: true,
                    success: function (data) {
                        var steps = $.parseJSON(data);
                        stepSelect.empty();
                        for (var i = 0; i < steps.length; i++) {
                            stepSelect.append($.createOption(steps[i].RERURNSTEPNAME, steps[i].RERURNSTEPID));
                        }
                        stepSelect.selectmenu("refresh");
                    },
                    error: function (data) { }
                });
            });

            $('div.history', this).each(function () {
                var historyDiv = $(this);
                $.ajax({
                    type: "POST",
                    url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                    data: { type: 'gdvHis', LISTID: selectedRow.LISTID },
                    cache: false,
                    async: true,
                    success: function (data) {
                        var rows = $.parseJSON(data);
                        var columns = ['S_STEP_ID', 'USERNAME', 'STATUS', 'REMARK', 'UPDATE_WHOLE_TIME'];
                        var table = '<table class="info-datagrid table-stripe infolight-breakpoint ui-table ui-table-reflow" data-role="table"  data-mode="reflow">';
                        var thead = '<thead><tr>';
                        for (var i = 0; i < columns.length; i++) {
                            thead += '<th infolight-options="field:\'' + columns[i] + '\',width:120,align:\'\'">' + columns[i] + '</th>';
                        }
                        thead += '</tr></thead>';
                        table += thead;
                        var tbody = '<tbody>';
                        for (var i = 0; i < rows.length; i++) {
                            tbody += '<tr>';
                            for (var j = 0; j < columns.length; j++) {

                                var caption = $.fn.flow.defaults[columns[j]];
                                if (!caption) {
                                    caption = columns[j];
                                }

                                tbody += '<td style="width: auto;padding:2px" field="' + columns[j] + '"><b class=\"ui-table-cell-label\">' + caption + '</b>' + rows[i][columns[j]] + '</td>';
                            }
                            tbody += "</tr>"
                        }
                        tbody += '</tbody>';
                        table += tbody;
                        table += '</table>';
                        $(table).appendTo(historyDiv);
                        $(table).datagrid({});
                    },
                    error: function (data) { }
                });
            });
            $('ul.files', this).each(function () {
                var attachments = selectedRow.ATTACHMENTS;
                if (attachments) {
                    var fileUl = $(this);
                    var list = attachments.split(';');
                    for (var i = 0; i < list.length; i++) {
                        if (list[i]) {
                            $('<li data-icon="delete" ><a>' + list[i] + '</a></li>').appendTo($('ul.files', jq[0])).click(function () {
                                if (event.x < $(this).width() - 25) {
                                    //window.sessionStorage.setItem('previewUrl', webSiteUrl + '/WorkflowFiles/' + $(this).find('a').html());
                                    //$.mobile.changePage('preview.html', { transition: "pop" });
                                    $('img.preview').attr('src', webSiteUrl + '/WorkflowFiles/' + $(this).find('a').html());
                                    $('#popupDialog').css('width', $(this).width() - 25);
                                    $('#popupDialog').popup('open');
                                }
                                else {
                                    $(this).remove();
                                    fileUl.listview('refresh');
                                }
                            });
                        }
                    }
                    fileUl.listview('refresh');
                }
            });
            $('img.preview', this).each(function () {
                $(this).attr('src', window.sessionStorage.getItem('previewUrl'));
            });
        });
    },
    submitFlow: function (jq) {
        if (jq.length > 0) {
            var page = $(jq[0]);
            var selectedRow = $.parseJSON(window.sessionStorage.getItem('flowrow'));
            $.mobile.loading('show', { theme: 'b', text: $.fn.form.defaults.updatingMessage, textVisible: true });
            var data = { type: 'Workflow', active: 'Submit' };

            data.roles = $('select.role', page).val();
            data.important = $('input.important', page).is(':checked');
            data.urgent = $('input.urgent', page).is(':checked');

            var attachments = "";
            $('ul.files li').each(function () {
                attachments += $(this).find("a").html() + ";";
            });
            data.ATTACHMENTS = attachments;
            //urlParam += "&LISTID=" + selectedRow.LISTID;
            data.FLOWFILENAME = selectedRow.FLOWFILENAME;
            data.PROVIDER_NAME = selectedRow.PROVIDER_NAME;
            data.FORM_PRESENTATION = selectedRow.FORM_PRESENTATION.replace(/''/g, "'");;
            data.FLOWPATH = selectedRow.FLOWPATH;
            data.STATUS = selectedRow.STATUS;
            data.SENDTO_ID = selectedRow.SENDTO_ID;
            data.FORM_KEYS = selectedRow.FORM_KEYS;
            data.suggest = $('textarea.suggest').val();
            $.ajax({
                type: "POST",
                url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                data: data,
                cache: false,
                async: true,
                success: function (message) {
                    if (navigator.notification) {
                        navigator.notification.alert(
                            message.replace(/<\/br>/g, '\r\n').replace(/<br\/>/g, '\r\n'),
                            function () {
                                 location.href = 'mainflow.html';
                            },
                            $.fn.flow.defaults["Info"],
                            $.fn.form.defaults.okText
                        );
                    }
                    else {
                        window.alert(message.replace(/<\/br>/g, '\r\n').replace(/<br\/>/g, '\r\n'));
                         location.href = 'mainflow.html';
                    }
                    page.find('a.preview').hide();
                    page.find('a.file').hide();
                    page.find('a.ok').hide();
                },
                error: function (message) {

                },
                complete: function () {
                    $.mobile.loading('hide');
                }
            });
        }
    },
    approveFlow: function (jq) {
        if (jq.length > 0) {
            var page = $(jq[0]);
            var selectedRow = $.parseJSON(window.sessionStorage.getItem('flowrow'));
            $.mobile.loading('show', { theme: 'b', text: $.fn.form.defaults.updatingMessage, textVisible: true });
            var data = { type: 'Workflow', active: 'Approve' };
            data.roles = $('select.role', page).val();
            data.important = $('input.important', page).is(':checked');
            data.urgent = $('input.urgent', page).is(':checked');

            var attachments = "";
            $('ul.files li').each(function () {
                attachments += $(this).find("a").html() + ";";
            });
            data.ATTACHMENTS = attachments;
            data.LISTID = selectedRow.LISTID;
            data.PROVIDER_NAME = selectedRow.PROVIDER_NAME;
            data.FORM_PRESENTATION = selectedRow.FORM_PRESENTATION.replace(/''/g, "'");;
            data.FLOWPATH = selectedRow.FLOWPATH;
            data.STATUS = selectedRow.STATUS;
            data.SENDTO_ID = selectedRow.SENDTO_ID;
            data.FORM_KEYS = selectedRow.FORM_KEYS;
            data.suggest = $('textarea.suggest').val();

            $.ajax({
                type: "POST",
                url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                data: data,
                cache: false,
                async: true,
                success: function (message) {
                    if (navigator.notification) {
                        navigator.notification.alert(
                            message.replace(/<\/br>/g, '\r\n').replace(/<br\/>/g, '\r\n'),
                            function () {  location.href = 'mainflow.html'; },
                            $.fn.flow.defaults["Info"],
                            $.fn.form.defaults.okText
                        );
                    }
                    else {
                        window.alert(message.replace(/<\/br>/g, '\r\n').replace(/<br\/>/g, '\r\n'));
                         location.href = 'mainflow.html';
                    }
                    page.find('a.preview').hide();
                    page.find('a.file').hide();
                    page.find('a.ok').hide();
                },
                error: function (message) {

                },
                complete: function () {
                    $.mobile.loading('hide');
                }
            });
        }
    },
    previewFlow: function (jq, keyValues) {
        if (jq.length > 0) {
            var page = $(jq[0]);
            var selectedRow = $.parseJSON(window.sessionStorage.getItem('flowrow'));
            $.mobile.loading('show', { theme: 'b', text: $.fn.form.defaults.updatingMessage, textVisible: true });
            var data = { type: 'Workflow', active: 'Preview' };
            data.roles = $('select.role', page).val();
            data.important = $('input.important', page).is(':checked');
            data.urgent = $('input.urgent', page).is(':checked');


            data.LISTID = selectedRow.LISTID;
            data.FLOWFILENAME = selectedRow.FLOWFILENAME;
            data.PROVIDER_NAME = selectedRow.PROVIDER_NAME;
            data.FORM_PRESENTATION = selectedRow.FORM_PRESENTATION.replace(/''/g, "'");;
            data.FLOWPATH = selectedRow.FLOWPATH;
            data.STATUS = selectedRow.STATUS;
            data.SENDTO_ID = selectedRow.SENDTO_ID;
            data.FORM_KEYS = selectedRow.FORM_KEYS;
            data.suggest = $('textarea.suggest').val();

            $.ajax({
                type: "POST",
                url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                data: data,
                cache: false,
                async: true,
                success: function (fileName) {
                    $('img.preview').attr('src', webSiteUrl + '/WorkflowFiles/PreView/' + fileName);
                    $('#popupDialog').css('width', $(window).width() - 25);
                    $('#popupDialog').popup('open');
                },
                error: function (message) {

                },
                complete: function () {
                    $.mobile.loading('hide');
                }
            });

        }
    },
    returnFlow: function (jq) {
        if (jq.length > 0) {
            var page = $(jq[0]);
            var selectedRow = $.parseJSON(window.sessionStorage.getItem('flowrow'));
            $.mobile.loading('show', { theme: 'b', text: $.fn.form.defaults.updatingMessage, textVisible: true });

            var data = { type: 'Workflow', active: 'Return' };
            data.roles = $('select.role', page).val();
            data.returnstep = $('select.step', page).val();
            data.returnsteptext = $('select.step', page).val();
            data.important = $('input.important', page).is(':checked');
            data.urgent = $('input.urgent', page).is(':checked');

            var attachments = "";
            $('ul.files li').each(function () {
                attachments += $(this).find("a").html() + ";";
            });
            data.ATTACHMENTS = attachments;
            data.LISTID = selectedRow.LISTID;
            data.PROVIDER_NAME = selectedRow.PROVIDER_NAME;
            data.FORM_PRESENTATION = selectedRow.FORM_PRESENTATION.replace(/''/g, "'");;
            data.FLOWPATH = selectedRow.FLOWPATH;
            data.STATUS = selectedRow.STATUS;
            data.SENDTO_ID = selectedRow.SENDTO_ID;
            data.FORM_KEYS = selectedRow.FORM_KEYS;
            data.MULTISTEPRETURN = selectedRow.MULTISTEPRETURN;
            data.suggest = $('textarea.suggest').val();

            $.ajax({
                type: "POST",
                url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                data: data,
                cache: false,
                async: true,
                success: function (message) {
                    if (navigator.notification) {
                        navigator.notification.alert(
                            message.replace(/<\/br>/g, '\r\n').replace(/<br\/>/g, '\r\n'),
                            function () {  location.href = 'mainflow.html'; },
                            $.fn.flow.defaults["Info"],
                            $.fn.form.defaults.okText
                        );
                    }
                    else {
                        window.alert(message.replace(/<\/br>/g, '\r\n').replace(/<br\/>/g, '\r\n'));
                         location.href = 'mainflow.html';
                    }
                    page.find('a.preview').hide();
                    page.find('a.file').hide();
                    page.find('a.ok').hide();
                },
                error: function (message) {

                },
                complete: function () {
                    $.mobile.loading('hide');
                }
            });
        }
    },
    plusFlow: function (jq) {
        if (jq.length > 0) {
            var page = $(jq[0]);
            var selectedRow = $.parseJSON(window.sessionStorage.getItem('flowrow'));
            $.mobile.loading('show', { theme: 'b', text: $.fn.form.defaults.updatingMessage, textVisible: true });

            var dataUsers = $(".info-checkbox", "ul.user");
            var dataRoles = $(".info-checkbox", "ul.role");
            var users = "";
            var users1 = "";
            for (var i = 0; i < dataUsers.length; i++) {
                if (dataUsers[i].checked) {
                    users += "U:" + dataUsers[i].id + ";";
                    users1 += dataUsers[i].id + ":UserId;";
                }
            }
            var roles = "";
            for (var i = 0; i < dataRoles.length; i++) {
                if (dataRoles[i].checked)
                    roles += dataRoles[i].id + ";";
            }

            var data = { type: 'Workflow', active: 'Plus' };

            var urlParam = "?type=Workflow&active=Plus";
            data.users = users;
            data.users1 = users1;
            data.roles = roles;
            data.LISTID = selectedRow.LISTID;
            data.PROVIDER_NAME = selectedRow.PROVIDER_NAME;
            data.FORM_KEYS = selectedRow.FORM_KEYS;
            data.FORM_PRESENTATION = selectedRow.FORM_PRESENTATION;
            data.FLOWPATH = selectedRow.FLOWPATH;
            data.STATUS = selectedRow.STATUS;
            data.SENDTO_ID = selectedRow.SENDTO_ID;
            data.suggest = $('textarea.suggest').val();

            $.ajax({
                type: "POST",
                url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                data: data,
                cache: false,
                async: true,
                success: function (message) {
                    if (navigator.notification) {
                        navigator.notification.alert(
                            message.replace(/<\/br>/g, '\r\n').replace(/<br\/>/g, '\r\n'),
                            function () {
                            },
                            $.fn.flow.defaults["Info"],
                            $.fn.form.defaults.okText
                        );
                    }
                    else {
                        window.alert(message.replace(/<\/br>/g, '\r\n').replace(/<br\/>/g, '\r\n'));
                    }
                    page.find('a.ok').hide();
                     location.href = '../mainflow.html';
                },
                error: function (message) {

                },
                complete: function () {
                    $.mobile.loading('hide');
                }
            });
        }
    },
    notifyFlow: function (jq) {
        if (jq.length > 0) {
            var page = $(jq[0]);
            var selectedRow = $.parseJSON(window.sessionStorage.getItem('flowrow'));
            $.mobile.loading('show', { theme: 'b', text: $.fn.form.defaults.updatingMessage, textVisible: true });

            var dataUsers = $(".info-checkbox", "ul.user");
            var dataRoles = $(".info-checkbox", "ul.role");
            var users = "";
            var users1 = "";
            for (var i = 0; i < dataUsers.length; i++) {
                if (dataUsers[i].checked) {
                    users += "U:" + dataUsers[i].id + ";";
                    users1 += dataUsers[i].id + ":UserId;";
                }
            }
            var roles = "";
            for (var i = 0; i < dataRoles.length; i++) {
                if (dataRoles[i].checked)
                    roles += dataRoles[i].id + ";";
            }

            var data = { type: 'Workflow', active: 'Notify' };

            var urlParam = "?type=Workflow&active=Plus";
            data.users = users;
            data.users1 = users1;
            data.roles = roles;
            data.LISTID = selectedRow.LISTID;
            data.PROVIDER_NAME = selectedRow.PROVIDER_NAME;
            data.FORM_KEYS = selectedRow.FORM_KEYS;
            data.FORM_PRESENTATION = selectedRow.FORM_PRESENTATION;
            data.FLOWPATH = selectedRow.FLOWPATH;
            data.STATUS = selectedRow.STATUS;
            data.SENDTO_ID = selectedRow.SENDTO_ID;
            data.suggest = $('textarea.suggest').val();

            $.ajax({
                type: "POST",
                url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                data: data,
                cache: false,
                async: true,
                success: function (message) {
                    if (navigator.notification) {
                        navigator.notification.alert(
                        message.replace(/<\/br>/g, '\r\n').replace(/<br\/>/g, '\r\n'),
                                function () {
                                },
                                $.fn.flow.defaults["Info"],
                                $.fn.form.defaults.okText
                        );
                    }
                    else {
                        window.alert(message.replace(/<\/br>/g, '\r\n').replace(/<br\/>/g, '\r\n'));
                    }
                    page.find('a.ok').hide();
                },
                error: function (message) {

                },
                complete: function () {
                    $.mobile.loading('hide');
                }
            });
        }
    },
    hastenFlow: function (jq) {
        if (jq.length > 0) {
            var page = $(jq[0]);
            var selectedRow = $.parseJSON(window.sessionStorage.getItem('flowrow'));
            $.mobile.loading('show', { theme: 'b', text: $.fn.form.defaults.updatingMessage, textVisible: true });

            var data = { type: 'Workflow', active: 'Hasten' };

            data.LISTID = selectedRow.LISTID;
            data.PROVIDER_NAME = selectedRow.PROVIDER_NAME;
            data.FORM_KEYS = selectedRow.FORM_KEYS;
            data.FORM_PRESENTATION = selectedRow.FORM_PRESENTATION;
            data.FLOWPATH = selectedRow.FLOWPATH;
            data.STATUS = selectedRow.STATUS;
            data.SENDTO_ID = selectedRow.SENDTO_ID;
            data.suggest = $('textarea.suggest').val();
            $.ajax({
                type: "POST",
                url: webSiteUrl + '/handler/SystemHandle_Flow2.ashx',
                data: data,
                cache: false,
                async: true,
                success: function (message) {
                    if (navigator.notification) {
                        navigator.notification.alert(
                           message.replace(/<\/br>/g, '\r\n').replace(/<br\/>/g, '\r\n'),
                            function () { },
                            $.fn.flow.defaults["Info"],
                            $.fn.form.defaults.okText
                        );
                    }
                    else {
                        window.alert(message.replace(/<\/br>/g, '\r\n').replace(/<br\/>/g, '\r\n'));
                    }
                    page.find('a.ok').hide();
                },
                error: function (message) {

                },
                complete: function () {
                    $.mobile.loading('hide');
                }
            });
        }
    },
    uploadFile: function (jq) {
        if (jq.length > 0) {
            navigator.camera.getPicture(
                function (imageURI) {
                    var options = new FileUploadOptions();
                    options.fileKey = "file";
                    options.fileName = imageURI;
                    options.mimeType = "image/jpeg";

                    var params = {};
                    params.type = "flow";

                    options.params = params;
                    $.mobile.loading('show', { theme: 'b', text: $.fn.flow.defaults.UploadText, textVisible: true });
                    var uploader = new FileTransfer();
                    uploader.upload(imageURI, webSiteUrl + '/handler/file_upload.ashx', function (r) {
                        $.mobile.loading('hide');
                        //$('ul.files', jq[0]).append('<li><a href="#">' + r.response + '</a></li>');
                        $('<li data-icon="delete" ><a>' + r.response + '</a></li>').appendTo($('ul.files', jq[0])).click(function () {
                            if (event.x < $(this).width() - 25) {
                                $('img.preview').attr('src', webSiteUrl + '/WorkflowFiles/' + $(this).find('a').html());
                                $('#popupDialog').css('width', $(this).width() - 25);
                                $('#popupDialog').popup('open');
                            }
                            else {
                                $(this).remove();
                                $('ul.files', jq[0]).listview('refresh');
                            }
                        });
                        $('ul.files', jq[0]).listview('refresh');

                    }, function (error) {
                        $.mobile.loading('hide');
                        alert("An error has occurred: Code = " + error.code);
                    }, options);
                },
                function (message) { },
                {
                    quality: 50,
                    destinationType: navigator.camera.DestinationType.FILE_URI,
                    sourceType: navigator.camera.PictureSourceType.PHOTOLIBRARY
                }
            );
        }
    }
};


function RetakeVisible(row) {
    if (row.STATUS != undefined) {
        var status = row.STATUS;
        if (status != null && (status == "NR" || status == "A" || status == "Return" || status == "Plus" || status == "退回" || status == "加簽" || status == "退回" || status == "加签")) {
            return false;
        }
    }
    if (row.PLUSROLES != undefined) {
        var plusroles = row.PLUSROLES;
        if (plusroles != null && plusroles.Length > 0) {
            return false;
        }
    }
    if (row.S_USER_ID != undefined) {
        var user = row.S_USER_ID;
        var fLoginUser = "";
        $.ajax({
            type: 'post',
            dataType: 'json',
            url: webSiteUrl + '/handler/SystemHandle.ashx?type=ClientInfo',
            async: false,
            cache: false,
            success: function (clientInfo) {
                fLoginUser = clientInfo.UserID;
            },
            error: function (err) {

            }
        });
        if (user != null && user == fLoginUser) {
            return true;
        }
    }

    return false;
}
