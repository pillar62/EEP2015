function renderMain_Flow() {
    var urlPrdfix = "";
    if (isSubPath) {
        urlPrdfix = "../";
    }
    var mainLoaded = {};
    mainLoaded.inbox = false;
    mainLoaded.outbox = false;
    mainLoaded.notify = false;
    mainLoaded.overtime = false;
    $.sysmsg('getValues', [
        'EEPNetClient/FrmClientMain/ToDoListColumns'
        , 'Web/webClientMainFlow/UIText'
        , 'EEPNetClient/FrmClientMain/SelectOption'
        , 'EEPNetClient/FrmClientMain/OvertimeColumns'
        , 'SLTools/SLMainFlowPage2/GridButton'
    ]);

    var flowUIText = $.sysmsg('getValue', 'EEPNetClient/FrmClientMain/ToDoListColumns');
    var flowUITexts = flowUIText.split(',');
    var titleText = $.sysmsg('getValue', 'Web/webClientMainFlow/UIText');
    var titleTexts = titleText.split(';');
    //$(".tabs-title", "#tabsWorkFlow")[0].innerHTML = titleTexts[2];
    //$(".tabs-title", "#tabsWorkFlow")[1].innerHTML = titleTexts[3];
    //$(".tabs-title", "#tabsWorkFlow")[2].innerHTML = titleTexts[16];
    //$(".tabs-title", "#tabsWorkFlow")[3].innerHTML = titleTexts[4];
    var pleaseSelectText = $.sysmsg('getValue', 'EEPNetClient/FrmClientMain/SelectOption');
    $('#divFlowRunOver').hide();
    $('#tabsWorkFlow').tabs({
        onSelect: function (title, index) {
            if (index == 0 && mainLoaded.inbox == false) {
                mainLoaded.inbox = true;
                $('#dgInbox').datagrid({
                    //url: 'handler/SystemHandle_Flow.ashx?type=ToDoList',
                    url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
                    queryParams: { Type: "ToDoList" },
                    columns: [[
                                { field: 'ck', checkbox: true, width: 80 },
                                {
                                    field: 'FLOWIMPORTANT', title: "", sortable: true, width: 30, formatter: function (value, row, index) {
                                        if (value == 1) {
                                            var imgUrl = urlPrdfix + "Image/WorkflowIcon/Important.png";
                                            return "<img src='" + imgUrl + "'></img>";
                                        }
                                    }
                                }, //
                                {
                                    field: 'FLOWURGENT', title: "", sortable: true, width: 30, formatter: function (value, row, index) {
                                        if (value == 1) {
                                            var imgUrl = urlPrdfix + "Image/WorkflowIcon/urgent.png";
                                            return "<img src='" + imgUrl + "'></img>";
                                        }
                                    }
                                }, //
                                { field: 'FLOW_DESC', title: flowUITexts[0], sortable: true, width: 80 }, //'流程'
                                { field: 'D_STEP_ID', title: flowUITexts[1], sortable: true, width: 80 }, //'作业名称'
                                { field: 'USERNAME', title: flowUITexts[3], sortable: true, width: 80 }, //'寄件者'
                                { field: 'STATUS', title: flowUITexts[6], sortable: true, width: 50 }, //'情况'
                                { field: 'FORM_PRESENT_CT', title: flowUITexts[2], sortable: true, width: 150 }, //'单据号码'
                                {
                                    field: 'REMARK', title: flowUITexts[4], sortable: true, width: 120, formatter: function (value, row, index) {
                                        //return decodeURIComponent(value);
                                        return value;
                                    }
                                }, //'讯息'
                                { field: 'UPDATE_WHOLE_TIME', title: flowUITexts[5], sortable: true, width: 120 }, //'日期'
                                {
                                    field: 'ATTACHMENTS', title: flowUITexts[11], sortable: true, width: 120, formatter: function (value, row, index) {
                                        var link = "";
                                        if (value != null && value != "") {
                                            var lstAttachments = value.split(';');
                                            for (var i = 0; i < lstAttachments.length; i++) {
                                                if (lstAttachments[i] != "" && lstAttachments[i] != "null") {
                                                    var realFileName = lstAttachments[i];
                                                    var fileName = realFileName.replace(/__/g, "&nbsp;");
                                                    var href = "WorkflowFiles/" + realFileName;
                                                    link += "<A id='" + "ATTACHMENTS" + i + "' href='" + href + "' target='_blank' class=" + realFileName + " >" + fileName + "</A>&nbsp&nbsp";
                                                }
                                            }
                                        }
                                        return link;
                                    }
                                }//'相关文件'
                    ]],
                    //toolbar: '#tbInbox',
                    nowrap: false,
                    pagination: true,
                    view: flowCommandView,
                    toolbar: '#tbInbox',
                    singleSelect: false,
                    checkOnSelect: false,
                    type: 'ToDoList',
                    fitColumns: false,
                    title: titleTexts[2],
                    sortName: "UPDATE_WHOLE_TIME",
                    sortOrder: "desc",
                    remoteSort: true,
                    onLoadSuccess: function (data) {
                        var comboDatas = $('#ddlToDoListFilter').combobox("getData");
                        if (comboDatas.length == 0) {
                            var ddlToDoListFilterData = [];
                            ddlToDoListFilterData.push({ id: "-1", text: "<--" + pleaseSelectText + "-->" });
                            comboDatas.push({ id: "-1", text: "<--" + pleaseSelectText + "-->" });
                            $('#ddlToDoListFilter').combobox('loadData', ddlToDoListFilterData);
                            $('#ddlToDoListFilter').combobox('setValue', "<--" + pleaseSelectText + "-->");
                        }
                        if (comboDatas.length == 1) {
                            var ddlToDoListFilterData = comboDatas
                            for (var i = 0; i < data.rows.length; i++) {
                                var value = { id: data.rows[i].FLOW_DESC, text: data.rows[i].FLOW_DESC };
                                var exist = false;
                                for (var j = 0; j < ddlToDoListFilterData.length; j++) {
                                    if (ddlToDoListFilterData[j].id == value.id) {
                                        exist = true;
                                        break;
                                    }
                                }
                                if (!exist) {
                                    ddlToDoListFilterData.push(value);
                                }
                            }
                            $('#ddlToDoListFilter').combobox('loadData', ddlToDoListFilterData);
                            $('#ddlToDoListFilter').combobox('setValue', "<--" + pleaseSelectText + "-->");
                        }
                    },
                    rowStyler: function (index, row) {
                        if (row.IsDelay == "1" || row.ISDELAY == "1")
                            return 'color:red;';
                    }
                });

                $('#ddlToDoListFilter').combobox({
                    onChange: function (newValue, oldValue) {
                        var filter = "";
                        if (oldValue == "") {
                            return;
                        }
                        if (newValue != -1) {
                            filter = "FLOW_DESC='" + newValue + "'";
                        }
                        $('#dgInbox').datagrid('options').queryParams.Filter = encodeURI(filter);
                        $('#dgInbox').datagrid('load');
                    }
                });

                $('#btnRefresh_Inbox').click(function () {
                    if ($('#btnRefresh_Inbox').attr("href") == "#") {
                        RefreshInbox();
                    }
                });

                $('#btnApproveAll_Inbox').click(function () {
                    if ($('#btnApproveAll_Inbox').attr("href") == "#") {
                        var selectedRows = $("#dgInbox").datagrid("getSelections");
                        if (selectedRows.length > 0) {
                            for (var i = 0; i < selectedRows.length; i++) {
                                var row = selectedRows[i];
                                if (row.PLUSROLES) {
                                    var index = $("#dgInbox").datagrid('getRowIndex', row);
                                    $("#dgInbox").datagrid('uncheckRow', index);
                                }
                            }
                            selectedRows = $("#dgInbox").datagrid("getSelections");
                            //doApprove("winApprove", undefined, titleTexts[19], selectedRows[0]);
                            createAndOpenWorkflowDialog("winApprove", titleTexts[19], 550, 400, "InnerPages/FormApproveAll.html", selectedRows[0], formApproveAllLoaded);
                        }
                        else {
                            var warningText = $.sysmsg('getValue', 'FLTools/GloFix/SelectData');
                            alert(warningText);
                        }
                    }
                });

                $('#btnReturnAll_Inbox').click(function () {
                    if ($('#btnReturnAll_Inbox').attr("href") == "#") {
                        var selectedRows = $("#dgInbox").datagrid("getSelections");
                        if (selectedRows.length > 0) {
                            for (var i = 0; i < selectedRows.length; i++) {
                                var row = selectedRows[i];
                                if (row.FLNAVIGATOR_MODE == '5') {
                                    var index = $("#dgInbox").datagrid('getRowIndex', row);
                                    $("#dgInbox").datagrid('uncheckRow', index);
                                }
                            }
                            selectedRows = $("#dgInbox").datagrid("getSelections");
                            //doReturn("winReturn", undefined, titleTexts[20], selectedRows[0]);
                            createAndOpenWorkflowDialog("winReturn", titleTexts[20], 550, 400, "InnerPages/FormReturnAll.html", selectedRows[0], formReturnAllLoaded);
                        }
                        else {
                            var warningText = $.sysmsg('getValue', 'FLTools/GloFix/SelectData');
                            alert(warningText);
                        }
                    }
                });

                $('#btnQuery_Inbox').click(function () {
                    if ($('#btnQuery_Inbox').attr("href") == "#") {
                        var queryFormTitle = $.sysmsg('getValue', 'Srvtools/InfoNavigator/QueryFormTitle');
                        doFlowQuery("winFlowQuery", undefined, queryFormTitle)
                        //createAndOpenWorkflowDialog("winFlowQuery", queryFormTitle, 610, 200, "InnerPages/FormFlowQuery.html", "dgInbox", formFlowQueryLoaded);
                    }
                });

                //RefreshInbox();
            }
            else if (index == 1 && mainLoaded.outbox == false) {
                mainLoaded.outbox = true;
                flowUIText = $.sysmsg('getValue', 'EEPNetClient/FrmClientMain/ToDoListColumns');
                flowUITexts = flowUIText.split(',');
                $("#lSubmitted")[0].innerHTML = flowUITexts[10];
                //flowUIText = $.sysmsg('getValue', 'EEPNetClient/FrmClientMain/OvertimeColumns');
                //flowUITexts = flowUIText.split(',');
                $('#dgOutbox').datagrid({
                    //url: 'handler/SystemHandle_Flow.ashx?type=ToDoHis',
                    url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
                    queryParams: { Type: "ToDoHis" },
                    columns: [[
                                {
                                    field: 'FLOWIMPORTANT', title: "", sortable: true, width: 30, formatter: function (value, row, index) {
                                        if (value == 1) {
                                            var imgUrl = urlPrdfix + "Image/WorkflowIcon/Important.png";
                                            return "<img src='" + imgUrl + "'></img>";
                                        }
                                    }
                                }, //
                                {
                                    field: 'FLOWURGENT', title: "", sortable: true, width: 30, formatter: function (value, row, index) {
                                        if (value == 1) {
                                            var imgUrl = urlPrdfix + "Image/WorkflowIcon/urgent.png";
                                            return "<img src='" + imgUrl + "'></img>";
                                        }
                                    }
                                }, //
                                { field: 'FLOW_DESC', title: flowUITexts[0], sortable: true, width: 80 }, //'流程'
                                { field: 'D_STEP_ID', title: flowUITexts[1], sortable: true, width: 80 }, //'作业名称'
                                { field: 'SENDTO_NAME', title: flowUITexts[7], sortable: true, width: 80 }, //'经办者'
                                { field: 'STATUS', title: flowUITexts[6], sortable: true, width: 50 }, //'情况'
                                { field: 'FORM_PRESENT_CT', title: flowUITexts[2], sortable: true, width: 150 }, //'单据号码'
                                {
                                    field: 'REMARK', title: flowUITexts[4], sortable: true, width: 120, formatter: function (value, row, index) {
                                        //return decodeURIComponent(value);
                                        return value;
                                    }
                                }, //'讯息'
                                { field: 'UPDATE_WHOLE_TIME', title: flowUITexts[5], sortable: true, width: 120 }, //'日期'
                                {
                                    field: 'ATTACHMENTS', title: flowUITexts[11], sortable: true, width: 120, formatter: function (value, row, index) {
                                        var link = "";
                                        if (value != null && value != "") {
                                            var lstAttachments = value.split(';');
                                            for (var i = 0; i < lstAttachments.length; i++) {
                                                if (lstAttachments[i] != "" && lstAttachments[i] != "null") {
                                                    var realFileName = lstAttachments[i];
                                                    var fileName = realFileName.replace(/__/g, "&nbsp;");
                                                    var href = "WorkflowFiles/" + realFileName;
                                                    link += "<A id='" + "ATTACHMENTS" + i + "' href='" + href + "' target='_blank' class=" + realFileName + " >" + fileName + "</A>&nbsp&nbsp";
                                                }
                                            }
                                        }
                                        return link;
                                    }
                                }//'相关文件'
                    ]],
                    pagination: true,
                    nowrap: false,
                    fit: true,
                    view: flowCommandView,
                    type: 'ToDoHis',
                    singleSelect: true,
                    fitColumns: false,
                    title: titleTexts[3],
                    sortName: "UPDATE_WHOLE_TIME",
                    sortOrder: "desc",
                    remoteSort: true,
                    onLoadSuccess: function (data) {
                        var comboDatas = $('#ddlToDoHisFilter').combobox("getData");
                        if (comboDatas.length == 0) {
                            $('#ddlToDoHisFilter').attr("ToDoHis", true);
                            var ddlToDoHisFilterData = [];
                            ddlToDoHisFilterData.push({ id: "-1", text: "<--" + pleaseSelectText + "-->" });
                            $('#ddlToDoHisFilter').combobox('loadData', []);
                            $('#ddlToDoHisFilter').combobox('loadData', ddlToDoHisFilterData);
                            $('#ddlToDoHisFilter').combobox('setValue', "<--" + pleaseSelectText + "-->");
                        }
                        if (comboDatas.length == 1) {
                            var ddlToDoHisFilterData = comboDatas;
                            for (var i = 0; i < data.rows.length; i++) {
                                var value = { id: data.rows[i].FLOW_DESC, text: data.rows[i].FLOW_DESC };
                                var exist = false;
                                for (var j = 0; j < ddlToDoHisFilterData.length; j++) {
                                    if (ddlToDoHisFilterData[j].id == value.id) {
                                        exist = true;
                                        break;
                                    }
                                }
                                if (!exist) {
                                    ddlToDoHisFilterData.push(value);
                                }
                            }
                            $('#ddlToDoHisFilter').combobox('loadData', []);
                            $('#ddlToDoHisFilter').combobox('loadData', ddlToDoHisFilterData);
                            $('#ddlToDoHisFilter').combobox('setValue', "<--" + pleaseSelectText + "-->");
                        }
                    },
                    rowStyler: function (index, row) {
                        if (row.IsDelay == "1" || row.ISDELAY == "1")
                            return 'color:red;';
                    }
                });

                $('#ddlToDoHisFilter').combobox({
                    onChange: function (newValue, oldValue) {
                        var t = "ToDoHis";
                        if ($('#ddlToDoHisFilter').attr("ToDoHis") == "true") {
                            t = "ToDoHis";
                        }
                        else {
                            t = "FlowRunOver";
                        }

                        var filter = "";
                        if (oldValue == "") {
                            return;
                        }
                        if (newValue != -1)
                            filter = "FLOW_DESC='" + newValue + "'";
                        $('#dgOutbox').datagrid('options').queryParams.Filter = encodeURI(filter);
                        $('#dgOutbox').datagrid('load');
                    }
                });

                $('#btnRefresh_Outbox').click(function () {
                    if ($('#btnRefresh_Outbox').attr("href") == "#") {
                        RefreshOutbox();
                    }
                });

                $('#btnQuery_Outbox').click(function () {
                    if ($('#btnQuery_Outbox').attr("href") == "#") {
                        var mode = "dgOutbox";
                        if ($("#divFlowRunOver").is(":hidden") == false)
                            mode = "dgFlowRunOver";
                        var queryFormTitle = $.sysmsg('getValue', 'Srvtools/InfoNavigator/QueryFormTitle');
                        createAndOpenWorkflowDialog("winFlowQuery", queryFormTitle, 610, 200, "InnerPages/FormFlowQuery.html", mode, formFlowQueryLoaded);
                    }
                });

                //RefreshOutbox();

                $('#dgFlowRunOver').datagrid({
                    url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
                    queryParams: { Type: "FlowRunOver" },
                    columns: [[
                                { field: 'FLOW_DESC', title: flowUITexts[0], sortable: true, width: 80 }, //'流程'
                                { field: 'D_STEP_ID', title: flowUITexts[1], sortable: true, width: 80 }, //'作业名称'
                                { field: 'SENDTO_NAME', title: flowUITexts[7], sortable: true, width: 80 }, //'经办者'
                                { field: 'STATUS', title: flowUITexts[6], sortable: true, width: 50 }, //'情况'
                                { field: 'FORM_PRESENT_CT', title: flowUITexts[2], sortable: true, width: 150 }, //'单据号码'
                                {
                                    field: 'REMARK', title: flowUITexts[4], sortable: true, width: 120, formatter: function (value, row, index) {
                                        //return decodeURIComponent(value);
                                        return value;
                                    }
                                }, //'讯息'
                                { field: 'UPDATE_DATE', title: flowUITexts[5], sortable: true, width: 120 }, //'日期'
                    //{ field: 'UPDATE_WHOLE_TIME', title: '日期', sortable: true, width: 120 },
                                {
                                    field: 'ATTACHMENTS', title: flowUITexts[11], sortable: true, width: 120, formatter: function (value, row, index) {
                                        var link = "";
                                        if (value != null && value != "") {
                                            var lstAttachments = value.split(';');
                                            for (var i = 0; i < lstAttachments.length; i++) {
                                                if (lstAttachments[i] != "" && lstAttachments[i] != "null") {
                                                    var href = "WorkflowFiles/" + lstAttachments[i];
                                                    link += "<A id='" + "ATTACHMENTS" + i + "' href='" + href + "' target='_blank ' >" + lstAttachments[i] + "</A>&nbsp&nbsp";
                                                }
                                            }
                                        }
                                        return link;
                                    }
                                }//'相关文件'
                    ]],
                    fit: true,
                    pagination: true,
                    nowrap: false,
                    view: flowCommandView,
                    type: 'FlowRunOver',
                    fitColumns: false,
                    sortName: "UPDATE_DATE",
                    sortOrder: "desc",
                    remoteSort: true,
                    onLoadSuccess: function (data) {
                        var comboDatas = $('#ddlToDoHisFilter').combobox("getData");
                        if (comboDatas.length == 0) {
                            $('#ddlToDoHisFilter').attr("ToDoHis", false);
                            var ddlToDoHisFilterData = [];
                            ddlToDoHisFilterData.push({ id: "-1", text: "<--" + pleaseSelectText + "-->" });
                            $('#ddlToDoHisFilter').combobox('loadData', []);
                            $('#ddlToDoHisFilter').combobox('loadData', ddlToDoHisFilterData);
                            $('#ddlToDoHisFilter').combobox('setValue', "<--" + pleaseSelectText + "-->");
                        }
                        if (comboDatas.length == 1) {
                            var ddlToDoHisFilterData = comboDatas;
                            for (var i = 0; i < data.rows.length; i++) {
                                var value = { id: data.rows[i].FLOW_DESC, text: data.rows[i].FLOW_DESC };
                                var exist = false;
                                for (var j = 0; j < ddlToDoHisFilterData.length; j++) {
                                    if (ddlToDoHisFilterData[j].id == value.id) {
                                        exist = true;
                                        break;
                                    }
                                }
                                if (!exist) {
                                    ddlToDoHisFilterData.push(value);
                                }
                            }
                            $('#ddlToDoHisFilter').combobox('loadData', []);
                            $('#ddlToDoHisFilter').combobox('loadData', ddlToDoHisFilterData);
                            $('#ddlToDoHisFilter').combobox('setValue', "<--" + pleaseSelectText + "-->");
                        }
                    }
                });
            }
            else if (index == 2 && mainLoaded.notify == false) {
                mainLoaded.notify = true;
                flowUIText = $.sysmsg('getValue', 'EEPNetClient/FrmClientMain/ToDoListColumns');
                flowUITexts = flowUIText.split(',');
                $('#dgNotify').datagrid({
                    //url: 'handler/SystemHandle_Flow.ashx?type=Notify',
                    url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
                    queryParams: { Type: "Notify" },
                    columns: [[
                                { field: 'ck', checkbox: true, width: 80 },
                                { field: 'FLOW_DESC', title: flowUITexts[0], sortable: true, width: 80 }, //'流程'
                                { field: 'D_STEP_ID', title: flowUITexts[1], sortable: true, width: 80 }, //'作业名称'
                                { field: 'USERNAME', title: flowUITexts[3], sortable: true, width: 80 }, //'寄件者'
                                { field: 'STATUS', title: flowUITexts[6], sortable: true, width: 50 }, //'情况'
                                { field: 'FORM_PRESENT_CT', title: flowUITexts[2], sortable: true, width: 150 }, //'单据号码'
                                {
                                    field: 'REMARK', title: flowUITexts[4], sortable: true, width: 120, formatter: function (value, row, index) {
                                        //return decodeURIComponent(value);
                                        return value;
                                    }
                                }, //'讯息'
                                { field: 'UPDATE_WHOLE_TIME', title: flowUITexts[5], sortable: true, width: 120 }, //'日期'
                                {
                                    field: 'ATTACHMENTS', title: flowUITexts[11], sortable: true, width: 120, formatter: function (value, row, index) {
                                        var link = "";
                                        if (value != null && value != "") {
                                            var lstAttachments = value.split(';');
                                            for (var i = 0; i < lstAttachments.length; i++) {
                                                if (lstAttachments[i] != "" && lstAttachments[i] != "null") {
                                                    var href = "WorkflowFiles/" + lstAttachments[i];
                                                    link += "<A id='" + "ATTACHMENTS" + i + "' href='" + href + "' target='_blank ' >" + lstAttachments[i] + "</A>&nbsp&nbsp";
                                                }
                                            }
                                        }
                                        return link;
                                    }
                                }//'相关文件'
                    ]],
                    pagination: true,
                    nowrap: false,
                    view: flowCommandView,
                    singleSelect: false,
                    type: 'Notify',
                    title: titleTexts[16],
                    fitColumns: false,
                    sortName: "UPDATE_WHOLE_TIME",
                    sortOrder: "desc",
                    remoteSort: true
                });

                //RefreshNotify();

                $('#btnRefresh_Notify').click(function () {
                    if ($('#btnRefresh_Notify').attr("href") == "#") {
                        RefreshNotify();
                    }
                });

                $('#btnDeleteAll_Notify').click(function () {
                    if ($('#btnDeleteAll_Notify').attr("href") == "#") {
                        if (confirm(flowDeleteText)) {
                            var urlPrdfix = "";
                            if (isSubPath) {
                                urlPrdfix = "../";
                            }

                            var LISTIDs = "";
                            var PROVIDER_NAMEs = "";
                            var FORM_KEYSs = "";
                            var FORM_PRESENTATIONs = "";
                            var FLOWPATHs = "";
                            var STATUSs = "";
                            var SENDTO_IDs = "";
                            var selectedRows = $('#dgNotify').datagrid("getSelections");
                            for (var i = 0; i < selectedRows.length; i++) {
                                LISTIDs += selectedRows[i].LISTID + "!";
                                PROVIDER_NAMEs += selectedRows[i].PROVIDER_NAME + "!";
                                FORM_KEYSs += selectedRows[i].FORM_KEYS + "!";
                                FORM_PRESENTATIONs += selectedRows[i].FORM_PRESENTATION + "!";
                                FLOWPATHs += selectedRows[i].FLOWPATH + "!";
                                STATUSs += selectedRows[i].STATUS + "!";
                                SENDTO_IDs += selectedRows[i].SENDTO_ID + "!";
                            }
                            var urlParam = {};
                            urlParam.Type = "Workflow";
                            urlParam.Active = "FlowDeleteAll";
                            urlParam.LISTID = LISTIDs;
                            urlParam.PROVIDER_NAME = PROVIDER_NAMEs;
                            urlParam.FORM_KEYS = FORM_KEYSs;
                            urlParam.FORM_PRESENTATION = FORM_PRESENTATIONs;
                            urlParam.FLOWPATH = FLOWPATHs;
                            urlParam.STATUS = STATUSs;
                            urlParam.SENDTO_ID = SENDTO_IDs;

                            $.ajax({
                                type: "POST",
                                url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
                                data: urlParam,
                                cache: false,
                                async: true,
                                success: function (message) {
                                    RefreshNotify();
                                },
                                error: function (err) {
                                    return false;
                                }
                            });
                        }
                    }
                });
            }
            else if (index == 3 && mainLoaded.overtime == false) {
                mainLoaded.overtime = true;
                var usertype = getClientInfo("AUTOLOGIN");
                if (usertype != "S") {
                    $('#cbOvertime_ShowAll').hide();
                    $('#lNotOvertime_showALl').hide();
                }
                flowUIText = $.sysmsg('getValue', 'EEPNetClient/FrmClientMain/OvertimeColumns');
                flowUITexts = flowUIText.split(',');
                $("#lNotOvertime_Delay")[0].innerHTML = flowUITexts[11];
                var filter = "&Level=0";
                $('#dgDelay').datagrid({
                    //url: 'handler/SystemHandle_Flow.ashx?type=Delay' + filter,
                    url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
                    queryParams: { Type: "Delay", Level: 0 },
                    columns: [[
                                { field: 'ck', checkbox: true, width: 80 },
                                { field: 'FLOW_DESC', title: flowUITexts[0], sortable: true, width: 80 }, //'流程'
                                { field: 'D_STEP_ID', title: flowUITexts[1], sortable: true, width: 80 }, //'作业名称'
                                { field: 'SENDTO_NAME', title: flowUITexts[3], sortable: true, width: 80 }, //'经办者'
                                { field: 'STATUS', title: flowUITexts[9], sortable: true, width: 50 }, //'情况'
                                { field: 'FORM_PRESENT_CT', title: flowUITexts[2], sortable: true, width: 150 }, //'单据号码'
                                {
                                    field: 'REMARK', title: flowUITexts[4], sortable: true, width: 120, formatter: function (value, row, index) {
                                        //return decodeURIComponent(value);
                                        return value;
                                    }
                                }, //'讯息'
                                { field: 'UPDATE_WHOLE_TIME', title: flowUITexts[5], sortable: true, width: 120 }//'日期'
                    //{ field: 'OVERTIME', title: '逾时时间', sortable: true, width: 50 }
                    ]],
                    nowrap: false,
                    pagination: false,
                    fitColumns: false,
                    singleSelect: false,
                    title: titleTexts[4],
                    view: flowCommandView,
                    type: 'Delay',
                    sortName: "UPDATE_WHOLE_TIME",
                    sortOrder: "desc",
                    remoteSort: true,
                    onLoadSuccess: function (data) {

                    }
                });
                $('#btnRefresh_Delay').click(function () {
                    if ($('#btnRefresh_Delay').attr("href") == "#") {
                        RefreshDelay();
                    }
                });

                $('#sOvertimeColumn_Delay').combobox({
                    onChange: function (newValue, oldValue) {
                        $('#dgDelay').datagrid('options').queryParams.Level = newValue;
                        if ($('#cbNotOvertime_Delay').attr('checked'))
                            $('#dgDelay').datagrid('options').queryParams.Type = "DelayAllData";
                        else
                            $('#dgDelay').datagrid('options').queryParams.Type = "Delay";
                        $('#dgDelay').datagrid('load');
                    }
                });
                var queryText = $.sysmsg('getValue', 'Web/webClientMainFlow/UIText').split(';')[17];
                $('#btOvertime_Query').linkbutton({ text: queryText });
                $('#btOvertime_Query').click(function () {
                    if ($('#btOvertime_Query').attr("href") == "#") {
                        var queryFormTitle = $.sysmsg('getValue', 'Srvtools/InfoNavigator/QueryFormTitle');
                        createAndOpenWorkflowDialog("winFlowQuery", queryFormTitle, 610, 200, "InnerPages/OvertimeQuery.html", "dgDelay", overtimeQueryLoaded);
                    }
                });
                var transferText = $.sysmsg('getValue', 'Web/webClientMainFlow/UIText').split(';')[22];
                $('#btnTransfer_Delay').linkbutton({ text: transferText });
                $('#btnTransfer_Delay').click(function () {
                    transferDalay();
                });
                function transferDalay() {
                    var sendToRole = false;
                    var sendToUser = false;
                    var combogridRole = null;
                    var combogridUser = null;

                    var needTransferRows = $('#dgDelay').datagrid('getChecked');
                    if (needTransferRows.length == 0) {
                        alert("Please select a row first.");
                        return;
                    }
                    for (var i = 0; i < needTransferRows.length; i++) {
                        var selectedRow = needTransferRows[i];
                        if (selectedRow.SENDTO_KIND == 2 || selectedRow.SENDTO_KIND == "2") {
                            sendToUser = true;
                        }
                        else {
                            sendToRole = true;
                        }
                    }

                    var width = 280;
                    if (sendToUser && sendToRole) width = 550;
                    var sendtoSelectDialog = $('<div ></div>').appendTo('body');
                    sendtoSelectDialog.dialog({
                        title: 'Transfer ',
                        width: width,
                        height: 'auto',
                        closed: false,
                        cache: false,
                        modal: true,
                        onClose: function () {
                            $(this).remove();
                        }
                    });
                    var tr = $('<table><tbody><tr></tr></tbody></table>').appendTo(sendtoSelectDialog);

                    if (sendToRole) {
                        $('<td>Send To Role:</td>').appendTo(tr);
                        var td = $('<td></td>').appendTo(tr);
                        combogridRole = $('<input class="easyui-combobox",panelHeight:100" />').appendTo(td);
                        combogridRole.combogrid({
                            idField: 'GROUPID',
                            textField: 'GROUPNAME',
                            mode: 'local',
                            columns: [[
                                { field: 'GROUPID', title: 'GROUPID', width: 60 },
                                { field: 'GROUPNAME', title: 'GROUPNAME', width: 100 }
                            ]],
                            filter: function (q, row) {
                                var opts = $(this).combogrid('options');
                                if (row[opts.idField].indexOf(q) == 0 || row[opts.textField].indexOf(q) == 0)
                                    return true;
                                else
                                    return false;
                            }
                        });
                        $('<br/>').appendTo(sendtoSelectDialog);
                        $.ajax({
                            type: "POST",
                            url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
                            data: { Type: 'Workflow', Active: "FlowGetAllRoleOrUser", SendToKind: "0" },
                            cache: false,
                            async: true,
                            success: function (data) {
                                data = eval(data);
                                combogridRole.combogrid("grid").datagrid("loadData", data);
                            },
                            error: function () {
                                return false;
                            }
                        });
                    }
                    if (sendToUser) {
                        $('<td>Send To User:</td>').appendTo(tr);
                        var td = $('<td></td>').appendTo(tr);
                        combogridUser = $('<input class="easyui-combobox",panelHeight:100" />').appendTo(td);
                        combogridUser.combogrid({
                            idField: 'USERID',
                            textField: 'USERNAME',
                            mode: 'local',
                            columns: [[
                                { field: 'USERID', title: 'USERID', width: 60 },
                                { field: 'USERNAME', title: 'USERNAME', width: 100 }
                            ]],
                            filter: function (q, row) {
                                var opts = $(this).combogrid('options');
                                if (row[opts.idField].indexOf(q) == 0 || row[opts.textField].indexOf(q) == 0)
                                    return true;
                                else
                                    return false;
                            }
                        });
                        $.ajax({
                            type: "POST",
                            url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
                            data: { Type: 'Workflow', Active: "FlowGetAllRoleOrUser", SendToKind: "2" },
                            cache: false,
                            async: true,
                            success: function (data) {
                                data = eval(data);
                                combogridUser.combogrid("grid").datagrid("loadData", data);
                            },
                            error: function () {
                                return false;
                            }
                        });
                    }

                    var okbutton = $('<a href="#" value="OK" ></a>').appendTo(sendtoSelectDialog);
                    okbutton.linkbutton({
                        iconCls: 'icon-ok',
                    });
                    okbutton.unbind().bind('click', function () {
                        for (var i = 0; i < needTransferRows.length; i++) {
                            var selectedRow = needTransferRows[i];

                            var sendtoid = "";
                            var sendtoname = "";
                            if (selectedRow.SENDTO_KIND == 2 || selectedRow.SENDTO_KIND == "2") {
                                sendtoid = combogridUser.combogrid('getValue');
                                sendtoname = combogridUser.combogrid('getText');
                                if (!sendtoid) {
                                    alert("Please select a user to transfer to.");
                                    return;
                                }
                            }
                            else {
                                sendtoid = combogridRole.combogrid('getValue');
                                sendtoname = combogridRole.combogrid('getText') + "/";
                                $.ajax({
                                    type: "POST",
                                    url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
                                    data: { Type: 'Workflow', Active: "FlowUserGroups", GroupID: sendtoid },
                                    cache: false,
                                    async: false,
                                    success: function (data) {
                                        data = eval(data);
                                        //for (var j = 0; j < 1; j++) {
                                        //    sendtoname += data[j].USERNAME + ",";
                                        //}
                                        //sendtoname = sendtoname.substr(0, sendtoname.length - 2);
                                        if (data.length > 0)
                                            sendtoname += data[0].USERNAME;
                                    },
                                    error: function () {
                                        return false;
                                    }
                                });

                                if (!sendtoid) {
                                    alert("Please select a role to transfer to.");
                                    return;
                                }
                            }

                            var urlParam = {};
                            urlParam.Type = "Workflow";
                            urlParam.Active = "FlowTransfer";
                            urlParam.LISTID = selectedRow.LISTID;
                            urlParam.FLOWPATH = selectedRow.FLOWPATH;
                            var srcid = selectedRow.SENDTO_ID;
                            var kind = selectedRow.SENDTO_KIND;

                            if (sendtoid != srcid) {
                                urlParam.SENDTO_ID = sendtoid;
                                urlParam.SENDTO_NAME = sendtoname;
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
                            }
                        }
                        sendtoSelectDialog.dialog('close');
                        $('#dgDelay').datagrid('load');
                    });

                    var canclebutton = $('<a href="#" value="Close" ></a>').appendTo(sendtoSelectDialog);
                    canclebutton.linkbutton({
                        iconCls: 'icon-cancel',
                    });
                    canclebutton.unbind().bind('click', function () {
                        sendtoSelectDialog.dialog('close');
                    });
                }

                var showALlText = $.sysmsg('getValue', 'Web/webClientMainFlow/UIText').split(';')[23];
                $('#lNotOvertime_showALl').text(showALlText);
            }
        }
    });

    RefrshFlowCount(0);
    RefrshFlowCount(1);
    RefrshFlowCount(2);
    RefrshFlowCount(3);
}

function chkSubmittedChanged(e) {
    if (e.checked) {

        $('#divFlowRunOver').panel({
            fit: true
        });
        $('#divFlowRunOver').show();
        $('#divOutbox').hide();
        $('#dgFlowRunOver').datagrid('resize');

    }
    else {
        $('#divOutbox').panel({
            fit: true
        });
        $('#divFlowRunOver').hide();
        $('#divOutbox').show();
        $('#dgOutbox').datagrid('resize');
        //$('#dgFlowRunOver').hide();
        //$('#dgOutbox').show();
    }
}

function cbNotOvertimeChanged(e) {
    if (e.checked) {
        RefreshDelayAllData();
    }
    else {
        RefreshDelay();
    }
}

function cbOvertime_ShowAllChanged(e) {
    if (e.checked) {
        $('#dgDelay').datagrid('options').queryParams.SpecialUse = 2147483646;
        $('#dgDelay').datagrid('load');
    }
    else {
        $('#dgDelay').datagrid('options').queryParams.SpecialUse = "";
        RefreshDelay();
    }
}


function RefreshInbox() {
    $('#dgInbox').datagrid('load');
    RefrshFlowCount(0);
}

window.top["FlowRefreshInbox"] = function () {
    RefreshInbox();
};

function RefreshOutbox() {
    if ($('#dgOutbox').data('datagrid')) {
        $('#dgOutbox').datagrid('load');
    }
    RefrshFlowCount(1);
}

function RefreshFlowRunOver() {
    if ($('#dgFlowRunOver').data('datagrid')) {
        $('#dgFlowRunOver').datagrid('load');
    }
}

function RefreshNotify() {
    if ($('#dgNotify').data('datagrid')) {
        $('#dgNotify').datagrid('load');
    }
    RefrshFlowCount(2);
}

window.top["FlowRefreshNotify"] = function () {
    RefreshNotify();
};

function RefreshDelay() {
    if ($('#dgDelay').data('datagrid')) {
        if ($('#cbOvertime_ShowAll').attr('checked'))
            $('#dgDelay').datagrid('options').queryParams.Type = "DelayAllData";
        else
            $('#dgDelay').datagrid('options').queryParams.Type = "Delay";
        $('#dgDelay').datagrid('load');
    }
    RefrshFlowCount(3);
}

function RefreshDelayAllData() {
    $('#dgDelay').datagrid('options').queryParams.Type = "DelayAllData";
    $('#dgDelay').datagrid('load');
}

window.top["FlowRefreshOutbox"] = function () {
    RefreshOutbox();
}

var flowCommandView = $.extend({}, $.fn.datagrid.defaults.view, {
    addCommandColumn: function (target, index) {
        var opts = $.data(target, 'datagrid').options;
        if (index >= 0) {
            _add(index);
        } else {
            var length = $(target).datagrid('getRows').length;
            for (var i = 0; i < length; i++) {
                _add(i);
            }

            var commandCount = 0;
            if (opts.type == "ToDoList") {
                commandCount = 3;
            }
            else if (opts.type == "ToDoHis") {
                commandCount = 3;
            }
            else if (opts.type == "FlowRunOver") {
                commandCount = 1;
            }
            else if (opts.type == "Notify") {
                commandCount = 2;
            }
            else if (opts.type == "Delay") {
                commandCount = 2;
            }
            opts.finder.getTr(target, 0, 'allfooter', 1).each(function () {

                var s = '<td><div style="width:' + commandCount * 20 + 'px;text-align:right;"></div></td>';
                var tr = $(this);
                if (tr.is(':empty')) {
                    tr.html(s);
                } else if (tr.children('td.datagrid-td-rownumber').length) {
                    $(s).insertAfter(tr.children('td.datagrid-td-rownumber'));
                } else {
                    $(s).insertBefore(tr.children('td:first'));
                }
            });
        }

        function _add(rowIndex) {
            var urlPrdfix = "";
            if (isSubPath) {
                urlPrdfix = "../";
            }

            var selectedRow = $(target).datagrid('getRows')[rowIndex];

            var tr = opts.finder.getTr(target, rowIndex, 'body', 1);
            if (tr.find('div.datagrid-row-command').length) { return; } // the expander is already exists

            var commandCount = 0;
            var cc = [];
            cc.push('<td>');
            if (opts.type == "ToDoList") {
                commandCount = 3;
                cc.push('<div class="datagrid-row-command" style="text-align:center;width:' + commandCount * 20 + 'px;height:16px;">');
                cc.push('<span class="icon-flow-Select" title="Select" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
                if (selectedRow.PLUSROLES == null || selectedRow.PLUSROLES == "") {
                    cc.push('<span class="icon-flow-Approve" title="Approve" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
                    if (selectedRow.FLNAVIGATOR_MODE != "0" && selectedRow.FLNAVIGATOR_MODE != "5")
                        cc.push('<span class="icon-flow-Return" title="Return" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
                }
            }
            else if (opts.type == "ToDoHis") {
                commandCount = 3;
                cc.push('<div class="datagrid-row-command" style="text-align:center;width:' + commandCount * 20 + 'px;height:16px;">');
                cc.push('<span class="icon-flow-Select FlowRunOver" title="Select" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
                if (RetakeVisible(selectedRow))
                    cc.push('<span class="icon-flow-Retake" title="Retake" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
                cc.push('<span class="icon-flow-Hasten" title="Hasten" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
                //cc.push('<span class="icon-flow-Notify" title="Notify" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
            }
            else if (opts.type == "FlowRunOver") {
                commandCount = 1;
                cc.push('<div class="datagrid-row-command" style="text-align:center;width:' + commandCount * 20 + 'px;height:16px;">');
                cc.push('<span class="icon-flow-Select FlowRunOver" title="Select" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
            }
            else if (opts.type == "Notify") {
                commandCount = 2;
                cc.push('<div class="datagrid-row-command" style="text-align:center;width:' + commandCount * 20 + 'px;height:16px;">');
                cc.push('<span class="icon-flow-Select" title="Select" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
                cc.push('<span class="icon-flow-FlowDelete" title="FlowDelete" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
            }
            else if (opts.type == "Delay") {
                commandCount = 2;
                cc.push('<div class="datagrid-row-command" style="text-align:center;width:' + commandCount * 20 + 'px;height:16px;">');
                cc.push('<span class="icon-flow-Select FlowRunOver" title="Select" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
                //cc.push('<span class="icon-flow-Transfer" title="Transfer" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');//andy说先放着去找图标，然一直没有给我图标，到交货才想起来
            }
            cc.push('</div>');
            cc.push('</td>');
            if (tr.is(':empty')) {
                tr.html(cc.join(''));
            } else if (tr.children('td.datagrid-td-rownumber').length) {
                $(cc.join('')).insertAfter(tr.children('td.datagrid-td-rownumber'));
            } else {
                $(cc.join('')).insertBefore(tr.children('td:first'));
            }


            tr.find('span.icon-flow-Select').unbind('.datagrid').bind('click.datagrid', function (e) {
                var urlParam = "?";
                var flowtype = "flowtodo";
                if ($(this).hasClass("FlowRunOver")) {
                    selectedRow.FLNAVMODE = 6;
                    selectedRow.NAVMODE = 0;
                    selectedRow.NAVIGATOR_MODE = 0;
                    flowtype = "flowtohis";
                }
                for (var field in selectedRow) {
                    if (field == 'PARAMETERS') {
                        if (selectedRow[field]) {
                            urlParam += '&' + selectedRow[field];
                        }
                    }
                    else if (field == 'SENDTO_NAME') {


                    }
                    else if (field != 'REMARK') {
                        if (selectedRow[field]) {
                            if (selectedRow[field] != undefined && selectedRow[field] != null && selectedRow[field].replace != undefined)
                                urlParam += "&" + field + "=" + encodeURI(selectedRow[field].replace(/&/g, "markand"));
                            else
                                urlParam += "&" + field + "=" + encodeURI(selectedRow[field]);
                        }
                        else {
                            urlParam += "&" + field + "=";
                        }
                    }
                }
                if (selectedRow.WEBFORM_NAME != null) {
                    var url = '';
                    if (selectedRow.WEBFORM_NAME.toUpperCase().indexOf('WEB.') == 0) {
                        var webform = selectedRow.WEBFORM_NAME.split('.');
                        url = "InnerPages/EEPSingleSignOn.aspx" + urlParam + "&Type=" + flowtype + "&Package=" + webform[1] + "&Form=" + webform[2];
                        //selectedRow.WEBFORM_NAME
                        $.ajax({
                            url: urlPrdfix + "handler/SystemHandle.ashx",
                            data: { Type: "MENUTABLECAPTION", MenuName: selectedRow.WEBFORM_NAME },
                            type: 'GET',
                            async: false,
                            timeout: 3000,
                            error: function (xml) {
                                alert("加载XML文件出错！");
                            },
                            success: function (formName) {
                                addTab(formName, url);
                            }
                        });
                    }
                    else {
                        localStorage.setItem(selectedRow.FORM_PRESENTATION, urlParam);
                        $.ajax({
                            url: "handler/SystemHandle_Flow.ashx?FORM_PRESENTATION=" + selectedRow.FORM_PRESENTATION,
                            data: { param: urlParam, Type: "Encrypt" },
                            type: 'post',
                            async: true,
                            success: function (param) {
                                url = selectedRow.WEBFORM_NAME.replace(".", "/") + ".aspx?" + param;
                                //selectedRow.WEBFORM_NAME
                                $.ajax({
                                    url: "handler/SystemHandle.ashx?Type=MENUTABLECAPTION&MenuName=" + selectedRow.WEBFORM_NAME,
                                    type: 'GET',
                                    async: false,
                                    timeout: 3000,
                                    error: function (xml) {
                                        alert("加载XML文件出错！");
                                    },
                                    success: function (formName) {
                                        addTab(formName, url);
                                    }
                                });
                            }
                        });

                    }

                }
            });

            var winTitle = $.sysmsg('getValue', 'SLTools/SLMainFlowPage2/GridButton');
            var winTitles = winTitle.split(';');;
            var approveText = winTitles[1];
            //if (selectedRow.FLNAVIGATOR_MODE == "0" && (selectedRow.STATUS == "NF" || selectedRow.STATUS == "取回"
            //    || selectedRow.STATUS == "Retake" || selectedRow.STATUS == "NR" || selectedRow.STATUS == "退回" || selectedRow.STATUS == "Return")) {
            //    var winTitle2 = $.sysmsg('getValue', 'FLClientControls/FLNavigator/NavText');
            //    var winTitles2 = winTitle2.split(';');;
            //    approveText = winTitles2[16];
            //}
            tr.find('span.icon-flow-Approve').unbind('.datagrid').bind('click.datagrid', function (e) {
                doApprove("winApprove", undefined, approveText, selectedRow);
                //createAndOpenWorkflowDialog("winApprove", winTitles[1], 550, 400, "InnerPages/FormApprove.html", selectedRow, formApproveLoaded);
            });

            tr.find('span.icon-flow-Return').unbind('.datagrid').bind('click.datagrid', function (e) {
                doReturn("winReturn", undefined, winTitles[2], selectedRow);
                //createAndOpenWorkflowDialog("winReturn", winTitles[2], 550, 400, "InnerPages/FormReturn.html", selectedRow, formReturnLoaded);
            });

            tr.find('span.icon-flow-Retake').unbind('.datagrid').bind('click.datagrid', function (e) {
                var urlParam = {};
                urlParam.Type = "Workflow";
                urlParam.Active = "Retake";
                urlParam.LISTID = selectedRow.LISTID;
                urlParam.D_STEP_ID = selectedRow.D_STEP_ID;
                urlParam.FLOWIMPORTANT = selectedRow.FLOWIMPORTANT;
                urlParam.FLOWURGENT = selectedRow.FLOWURGENT;
                urlParam.ATTACHMENTS = selectedRow.ATTACHMENTS;

                $.ajax({
                    type: "POST",
                    url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
                    data: urlParam,
                    cache: false,
                    async: true,
                    success: function (message) {
                        alert(message);

                        RefreshInbox();
                        RefreshOutbox();
                    },
                    error: function () {
                        return false;
                    }
                });
            });

            tr.find('span.icon-flow-Notify').unbind('.datagrid').bind('click.datagrid', function (e) {
                doNotify("winNotify", undefined, winTitles[6], selectedRow);
                //createAndOpenWorkflowDialog("winNotify", winTitles[6], 475, 455, "InnerPages/FormNotify.html", selectedRow, formNotifyLoaded);
            });

            tr.find('span.icon-flow-Hasten').unbind('.datagrid').bind('click.datagrid', function (e) {
                doHasten("winHasten", undefined, winTitles[4], selectedRow);
                //createAndOpenWorkflowDialog("winHasten", winTitles[4], 475, 200, "InnerPages/FormHasten.html", selectedRow, formHastenLoaded);
            });

            tr.find('span.icon-flow-FlowDelete').unbind('.datagrid').bind('click.datagrid', function (e) {
                if (confirm(flowDeleteText)) {
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
                            //alert(message);
                            //$(target).datagrid({
                            //    url: urlPrdfix + 'handler/SystemHandle_Flow.ashx',
                            //    data: { Type: "Notify" }
                            //});
                            RefreshNotify();
                        },
                        error: function () {
                            return false;
                        }
                    });
                }
            });
        }
    },
    deleteRow: function (target, index) {
        var opts = $.data(target, "datagrid").options;
        var data = $.data(target, "datagrid").data;
        opts.finder.getTr(target, index).remove();
        $.fn.datagrid.defaults.view.deleteRow.call(this, target, index);
    },
    updateRow: function (target, rowIndex, row) {
        var dc = $.data(target, 'datagrid').dc;
        var opts = $.data(target, 'datagrid').options;
        $.fn.datagrid.defaults.view.updateRow.call(this, target, rowIndex, row);
        this.addCommandColumn.call(this, target, rowIndex);
    },
    onBeforeRender: function (target) {
        var opts = $.data(target, 'datagrid').options;
        var dc = $.data(target, 'datagrid').dc;
        var panel = $(target).datagrid('getPanel');

        var t = dc.view1.children('div.datagrid-header').find('table');
        if (t.find('div.datagrid-header-expander').length) {
            return;
        }

        var commandCount = 0;
        if (opts.type == "ToDoList") {
            commandCount = 3;
        }
        else if (opts.type == "ToDoHis") {
            commandCount = 3;
        }
        else if (opts.type == "FlowRunOver") {
            commandCount = 1;
        }
        else if (opts.type == "Notify") {
            commandCount = 2;
        }
        else if (opts.type == "Delay") {
            commandCount = 2;
        }
        var td = $('<td rowspan="' + opts.frozenColumns.length + '"><div class="datagrid-header-expander" style="width:' + commandCount * 20 + 'px;"></div></td>');
        if ($('tr', t).length == 0) {
            td.wrap('<tr></tr>').parent().appendTo($('tbody', t));
        } else if (opts.rownumbers) {
            td.insertAfter(t.find('td:has(div.datagrid-header-rownumber)'));
        } else {
            td.prependTo(t.find('tr:first'));
        }
    },
    onAfterRender: function (target) {
        this.addCommandColumn(target);


        try {
            var flowUIText = $.sysmsg('getValue', 'Web/webClientMainFlow/UIText');
            var flowUITexts = flowUIText.split(';');

            $('.icon-flow-Select').each(function () {
                this.title = flowUITexts[9];
            });

            $('.icon-flow-Approve').each(function () {
                this.title = flowUITexts[6];
            });

            $('.icon-flow-Return').each(function () {
                this.title = flowUITexts[8];
            });

            $('.icon-flow-Retake').each(function () {
                this.title = flowUITexts[14];
            });

            $('.icon-flow-Notify').each(function () {
                this.title = flowUITexts[16];
            });

            $('.icon-flow-Hasten').each(function () {
                this.title = flowUITexts[18];
            });

            $('.icon-flow-FlowDelete').each(function () {
                this.title = flowUITexts[15];
            });
            $('.icon-flow-Transfer').each(function () {
                this.title = flowUITexts[22];
            });

            $('#dgInbox').datagrid("options").title = flowUITexts[2];
            $('#btnRefresh_Inbox')[0].firstChild.firstChild.innerHTML = flowUITexts[1];
            $('#btnQuery_Inbox')[0].firstChild.firstChild.innerHTML = flowUITexts[17];
            $('#btnApproveAll_Inbox')[0].firstChild.firstChild.innerHTML = flowUITexts[19];
            $('#btnReturnAll_Inbox')[0].firstChild.firstChild.innerHTML = flowUITexts[20];
            $('#btnDeleteAll_Notify')[0].firstChild.firstChild.innerHTML = flowUITexts[21];

            $('#dgInbox').datagrid("options").title = flowUITexts[3];
            $('#btnRefresh_Outbox')[0].firstChild.firstChild.innerHTML = flowUITexts[1];
            $('#btnQuery_Outbox')[0].firstChild.firstChild.innerHTML = flowUITexts[17];

            $('#btnRefresh_Notify')[0].firstChild.firstChild.innerHTML = flowUITexts[1];

            $('#btnRefresh_Delay')[0].firstChild.firstChild.innerHTML = flowUITexts[1];

            var overtimeColumn = $.sysmsg('getValue', 'EEPNetClient/FrmClientMain/OvertimeColumns');
            var overtimeColumns = overtimeColumn.split(',');
            $('#lOvertimeColumn_Delay')[0].innerHTML = overtimeColumns[7];

            //$('#dgInbox').datagrid("getPanel").panel("setTitle", flowUITexts[16]);
            //$('#dgInbox').datagrid("getPanel").panel("setTitle", flowUITexts[4]);
        }
        catch (err) {
            //alert("System.XML Version Too Old");
        }
    }
});

function RetakeVisible(row) {
    var urlPrdfix = "";
    if (isSubPath) {
        urlPrdfix = "../";
    }

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
            url: urlPrdfix + 'handler/SystemHandle.ashx?type=ClientInfo',
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
    //return (this.wizToDoHis.SqlMode != FLTools.ESqlMode.FlowRunOver);
}

function RefrshFlowCount(index) {
    var urlPrdfix = "";
    if (isSubPath) {
        urlPrdfix = "../";
    }

    var url = "";
    var type = "";
    var captionIndex = 0;
    switch (index) {
        case 0:
            url = urlPrdfix + 'handler/SystemHandle_Flow.ashx';
            type = { Type: "ToDoList" };
            captionIndex = 2;
            break;
        case 1:
            url = urlPrdfix + 'handler/SystemHandle_Flow.ashx';
            type = { Type: "ToDoHis" };
            captionIndex = 3;
            break;
        case 2:
            url = urlPrdfix + 'handler/SystemHandle_Flow.ashx';
            type = { Type: "Notify" };
            captionIndex = 16;
            break;
        case 3:
            var filter = "&Level=0";
            url = urlPrdfix + 'handler/SystemHandle_Flow.ashx';
            type = { Type: "Delay", Level: "0" };
            captionIndex = 4;
            break;
    }
    $.ajax({
        type: "POST",
        url: url,
        data: type,
        cache: false,
        async: true,
        success: function (data) {
            data = eval('(' + data + ')');
            var titleText = $.sysmsg('getValue', 'Web/webClientMainFlow/UIText');
            var titleTexts = titleText.split(';');
            var tabs = $('#tabsWorkFlow').tabs('tabs');
            var tab = tabs[index];
            $('#tabsWorkFlow').tabs('update', {
                tab: tab,
                options: {
                    title: '<span class="tabs-title">' + titleTexts[captionIndex] + '<span class="bubble">' + data.total + '</span></span>'
                }
            });
        },
        error: function (data) {
            //data.responseText = '';
            //obj = "[{\"" + textField + "\":\"\"}]";
        }
    });
    var titleText = $.sysmsg('getValue', 'Web/webClientMainFlow/UIText');
    var titleTexts = titleText.split(';');
    var tabs = $('#tabsWorkFlow').tabs('tabs');
    var tab = tabs[index];

    $('#tabsWorkFlow').tabs('update', {
        tab: tab,
        options: {
            title: '<span class="tabs-title">' + titleTexts[captionIndex] + '</span>'
        }
    });
}