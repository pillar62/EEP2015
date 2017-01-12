<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPage_Flow.aspx.cs" Inherits="MainPage_Flow" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link id="easyuiTheme" href="js/themes/default/easyui.css" rel="stylesheet" />
    <link href="js/themes/icon.css" rel="stylesheet" />
    <script src="js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="js/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="js/jquery.json.js" type="text/javascript"></script>
    <script src="js/jquery.infolight.js" type="text/javascript"></script>
    <script src="MainPage.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            setUserInfo();
            renderMain();


            $(window).bind('beforeunload', function (e) {
                var x = window.event.clientX;
                var y = window.event.clientY;
                //alert(window.outerWidth + ":" + x);
                if (y < 0) {
                    logout();
                }
            });


            if ($('#parameter').val()) {
                var param = $.parseJSON($('#parameter').val());
                var listID = param.listID;
                var flowPath = param.flowPath;
                var mode = param.mode;
                var type = 'ToDoList';
                if (mode == 'Notify') {
                    type = 'Notify';
                }

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: 'handler/SystemHandle_Flow.ashx',
                    data: { Type: type, listID: listID },
                    cache: false,
                    async: true,
                    success: function (data) {
                        for (var i = 0; i < data.rows.length; i++) {

                            if (data.rows[i].FLOWPATH == flowPath || (mode == 'Notify' && data.rows[i].FLOWPATH.split(';')[1] == flowPath.split(';')[1])) {
                                var selectedRow = data.rows[i];
                                var urlParam = "?";
                                var flowtype = "flowtodo";
                                for (var field in selectedRow) {
                                    if (field == 'PARAMETERS') {
                                        if (selectedRow[field]) {
                                            urlParam += '&' + selectedRow[field];
                                        }
                                    }
                                    else if (field != 'REMARK') {
                                        urlParam += "&" + field + "=" + encodeURI(selectedRow[field]);
                                    }
                                }
                                if (selectedRow.WEBFORM_NAME != null) {
                                    var url = '';
                                    if (selectedRow.WEBFORM_NAME.toUpperCase().indexOf('WEB.') == 0) {
                                        var webform = selectedRow.WEBFORM_NAME.split('.');
                                        url = "InnerPages/EEPSingleSignOn.aspx" + urlParam + "&Type=" + flowtype + "&Package=" + webform[1] + "&Form=" + webform[2];

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
                                    else {
                                        localStorage.setItem(selectedRow.FORM_PRESENTATION, urlParam);
                                        $.ajax({
                                            url: "handler/SystemHandle_Flow.ashx",
                                            data: { Type: "Encrypt", param: urlParam },
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
                                return;
                            }
                        }
                        if (mode != 'Notify') {
                            var msg = $.sysmsg('getValue', 'FLRuntime/FLInstance/FLSetpIsApprovedOrReturned');
                            alert(msg);
                        }
                    }
                });
            }

            //var items = [];
            //items.push({ imgUrl: "Image/MenuTree/apple.jpg", text: "apple" });
            //items.push({ imgUrl: "Image/MenuTree/android.jpg", text: "android" });
            //items.push({ imgUrl: "Image/MenuTree/google.jpg", text: "google" });
            //$.metro.changeItemsImage("Menu60", items);
            //$.metro.changeItemsImageText("Menu61", items);
            //$.metro.changeItemsText("Menu62", items, undefined, undefined, undefined, "widepeek");

            //$.ajax({
            //    type: "POST",
            //    url: 'handler/SystemHandle_Flow.ashx?type=ToDoList',
            //    cache: false,
            //    async: false,
            //    success: function (data) {
            //        data = eval('(' + data + ')');
            //        var items = [];
            //        for (var i = 0; i < data.length; i++) {
            //            var content = "<table style=\"width: 300px; height: 300px;\">";
            //            content += "<tr><td>流程:</td><td>" + data[i].FLOW_DESC + "<td></tr>";
            //            content += "<tr><td>作业名称:</td><td>" + data[i].D_STEP_ID + "<td></tr>";
            //            content += "<tr><td>寄件者:</td><td>" + data[i].USERNAME + "<td></tr>";
            //            content += "<tr><td>情况:</td><td>" + data[i].STATUS + "<td></tr>";
            //            content += "<tr><td>作业名称:</td><td>" + data[i].D_STEP_ID + "<td></tr>";
            //            content += "<tr><td>讯息:</td><td>" + $.convertRemark(data[i].REMARK) + "<td></tr>";
            //            content += "<tr><td>日期:</td><td>" + data[i].UPDATE_WHOLE_TIME + "<td></tr>";
            //            content += "</table>";
            //            items.push({ content: "<div>" + content + "</div>" });
            //        }
            //        $.metro.changeItemsCustomize("Menu62", items);
            //    },
            //    error: function (data) {

            //    }
            //});
        });

        function setlanguage(currentLang) {
            $.sysmsg('getValues', [
                'JQWebClient/mainpagelinkbutton'
                , 'Srvtools/AnyQuery/DeleteSure'
                , 'FLClientControls/FLNavigator/NavText'
                , 'FLClientControls/FLNavigator/FlowRejectConfirm'
                , 'FLClientControls/FLNavigator/FlowPauseConfirm'
            ]);

            var localstring = $.sysmsg('getValue', 'JQWebClient/mainpagelinkbutton');
            var local = localstring.split(',');
            var homeloacl = local[0];
            var menuslocal = local[1];
            var aboutlocal = local[2];
            var logoutlocal = local[3];
            var changePasswordlocal = local[4];
            var systemlocal = local[7];
            $('#homeMenuButton').text(homeloacl);
            $('#btn-mmMenus').text(menuslocal);
            $('#systemMenuButton').text(systemlocal);
            $('#aboutMenuButton').text(aboutlocal);
            $('#logOutMenuButton').text(logoutlocal);
            $('#changePasswordMenuButton').text(changePasswordlocal);
            //var oid = $(this).attr("id");
            //var buckle = $(this).children("buckle").text();

            var DeleteSure = $.sysmsg('getValue', 'Srvtools/AnyQuery/DeleteSure');
            var NavText = $.sysmsg('getValue', 'FLClientControls/FLNavigator/NavText');
            var NavTexts = NavText.split(";");
            flowDeleteText = String.format(DeleteSure, NavTexts[20]);
            flowRejectText = $.sysmsg('getValue', 'FLClientControls/FLNavigator/FlowRejectConfirm');
        }

        function setUserInfo() {
            $.ajax({
                type: "POST",
                url: "handler/SystemHandle.ashx?Type=USER",
                cache: false,
                async: false,
                success: function (data) {
                    $("#userInfoMenuButton").html(data);
                }
            });

        }

        function openFormLog(text) {
            //            var title = "Open JQuery Form";
            //            var description = text;
            //            $.ajax({
            //                type: "POST",
            //                url: "handler/SystemHandle.ashx?Type=UserDefineLog&Title=" + title + "&Description=" + description,
            //                cache: false,
            //                async: true
            //            });
        }

        function logout() {
            $.ajax({
                type: "POST",
                url: "handler/jqDataHandle.ashx?logout=true",
                data: 'mode=logout',
                cache: false,
                async: true

            });
            window.location.href = 'LogOn.aspx';
        }
    </script>
    <style>
        .tabs-panels .panel .panel-body {
            overflow: hidden;
        }

        #info {
            width: 317px;
            height: 293px;
            background-image: url(img/select_bg.png);
            background-position: -20px 0px;
            color: #ffffff;
        }

        .buttonStyle {
            text-align: center;
            background-image: url(../img/bt_bg.png);
            background-repeat: no-repeat;
            background-color: #014971;
            width: 117px;
            height: 35px;
            list-style-type: none;
            text-decoration: none;
            display: block;
            font-size: 13px;
            font-family: Verdana, Geneva, sans-serif;
            color: #FFF;
            border-style: none;
        }
    </style>
    <script type="text/javascript" src="assets/js/min/bootstrap.min.js"></script>
    <script type="text/javascript" src="assets/js/bootmetro-panorama.js"></script>
    <script type="text/javascript" src="assets/js/bootmetro-pivot.js"></script>
    <script type="text/javascript" src="assets/js/bootmetro-charms.js"></script>
    <script type="text/javascript" src="assets/js/bootstrap-datepicker.js"></script>

    <script type="text/javascript" src="assets/js/jquery.mousewheel.min.js"></script>
    <script type="text/javascript" src="assets/js/jquery.touchSwipe.min.js"></script>

    <link rel="stylesheet" type="text/css" href="assets/css/bootmetroMobile.css" media="screen and (max-width: 600px)" />
    <link rel="stylesheet" type="text/css" href="assets/css/bootmetro.css" media="screen and (min-width: 600px)" />
    <link rel="stylesheet" type="text/css" href="assets/css/bootmetro-responsive.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/bootmetro-icons.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/bootmetro-ui-light.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/datepicker.css" />

    <link href="MetroJs.Full.0.9.75a/MetroJs.css" rel="stylesheet" />
    <script src="MetroJs.Full.0.9.75a/MetroJs.js"></script>
</head>
<body id="mainBodyLayout" class="easyui-layout">
    <form id="form1" runat="server">
        <div style="width: 1280px; height: 66px; background-image: url(img/head.jpg);" title="" data-options="region:'north',split:false" border="false">
            <div id="menu" style="padding: 2px; border: 0px solid #ddd">
                <a id='homeMenuButton' href="MainPage_Flow.aspx" class="easyui-linkbutton l-btn l-btn-plain" data-options="plain:true"><span class="l-btn-left"><span class="l-btn-text">Home</span></span></a>
                <a id='systemMenuButton' href="#" class="easyui-menubutton" data-options="menu:'#mm3'">System</a>
                <JQTools:JQMenuButton ID="JQMenuButton2" runat="server" MenuId="2mmMenus" TitleMode="false" />

                <div id="mm3">
                    <div onclick="changePassword();">
                        <a id='changePasswordMenuButton' class="easyui-linkbutton l-btn l-btn-plain"
                            data-options="plain:true"><span class="l-btn-left"><span class="l-btn-text">Change Password</span></span></a>
                    </div>
                    <div class="menu-sep"></div>
                    <div>
                        <span id='aboutMenuButton'>About</span>
                        <div class="menu-content">
                            <div style="background: #f0f0f0; padding: 10px; text-align: left;">
                                <img src="Image/Logon2012/Title.png">
                                <p style="font-size: 14px; color: #444;">©Infolight</p>
                            </div>
                        </div>
                    </div>
                    <div class="menu-sep"></div>
                    <div onclick="logout()">
                        <a id='logOutMenuButton' class="easyui-linkbutton l-btn l-btn-plain" data-options="plain:true"><span class="l-btn-left"><span class="l-btn-text">Log out</span></span></a>
                    </div>
                </div>

                <a id='userInfoMenuButton' class="easyui-linkbutton l-btn l-btn-plain" data-options="plain:true"></a>
            </div>
        </div>
        <div title="Menu" data-options="region:'west',title:'Menu',split:true" style="width: 193px; height: 790px; background-image: url(img/left.jpg);">
            <input id="ddlSolution" class="easyui-combobox" style="width: 185px"
                data-options="valueField:'ID',textField:'Name'" />
            <JQTools:JQTree ID="JQTree1" runat="server" CollapseAll="False" FetchAll="true" />
        </div>
        <div data-options="region:'center'">
            <div id="tabsMain" class="easyui-tabs" fit="true">
                <div title="首頁" style="background-image: url(img/main.jpg);">
                    <JQTools:JQMetro ID="JQMetro1" runat="server" DBAlias="ERPS" RootValue="" SubFolder="False" />
                    <asp:Image ID="MainImg" runat="server" class="maincenter" Width="557" Height="371" src="img/img.png" />
                    <%--<img id="MainImg" class="maincenter" src="img/img.png" width="557" height="371">--%>
                </div>
                <%--                <div title="個人事項">
                    <div id="tabsWorkFlow" class="easyui-tabs" data-options="fit:true,plain:true">
                        <div title="111">
                            <div class="easyui-layout" data-options="fit:true">
                                <div data-options="region:'north',split:false,border:false" style="height: 35px">
                                    <table style="height: 100%; width: 100%">
                                        <tr>
                                            <td style="width: 180px">
                                                <select id="ddlToDoListFilter" class="easyui-combobox" data-options="valueField:'id',textField:'text'" style="width: 150px;" />
                                            </td>
                                            <td style="width: 80px">
                                                <a id="btnRefresh_Inbox" href="#" class="easyui-linkbutton">Refresh</a>
                                            </td>
                                            <td style="width: 80px">
                                                <a id="btnQuery_Inbox" href="#" class="easyui-linkbutton">Query</a>
                                            </td>
                                            <td style="width: 120px">
                                                <a id="btnApproveAll_Inbox" href="#" class="easyui-linkbutton">Approve All</a>
                                            </td>
                                            <td style="width: 120px">
                                                <a id="btnReturnAll_Inbox" href="#" class="easyui-linkbutton">Return All</a>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                                <div data-options="region:'center',border:false">
                                    <table id="dgInbox" class="easyui-datagrid" title="收件代办" style="width: auto" fit="true" pagination="false" rownumbers="true" singleselect="false"></table>
                                </div>
                            </div>
                        </div>
                        <div title="222">
                            <div class="easyui-layout" data-options="fit:true">
                                <div data-options="region:'north',split:false,border:false" style="height: 35px">
                                    <table style="height: 100%; width: 100%">
                                        <tr>
                                            <td style="width: 180px">
                                                <select id="ddlToDoHisFilter" class="easyui-combobox" data-options="valueField:'id',textField:'text'" style="width: 150px;" />
                                            </td>
                                            <td style="width: 80px">
                                                <input id="chkSubmitted" type="checkbox" onclick="chkSubmittedChanged(this)" /><label id="lSubmitted">Submitted</label>
                                            </td>
                                            <td style="width: 80px">
                                                <a id="btnRefresh_Outbox" href="#" class="easyui-linkbutton">Refresh</a>
                                            </td>
                                            <td style="width: 80px">
                                                <a id="btnQuery_Outbox" href="#" class="easyui-linkbutton">Query</a>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                                <div data-options="region:'center',border:false">
                                    <div id="divOutbox" class="easyui-panel" data-options="fit:true">
                                        <table id="dgOutbox" class="easyui-datagrid" title="送件经办" fit="true" pagination="false" rownumbers="true" singleselect="true"></table>
                                    </div>
                                    <div id="divFlowRunOver" style="width: 100%; height: 100%;">
                                        <table id="dgFlowRunOver" class="easyui-datagrid" title="已结案" fit="true" pagination="false" rownumbers="true" singleselect="true"></table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div title="333">
                            <div class="easyui-layout" data-options="fit:true">
                                <div data-options="region:'north',split:false,border:false" style="height: 35px">
                                    <table style="height: 100%; width: 100%">
                                        <tr>
                                            <td style="width: 80px">
                                                <a id="btnRefresh_Notify" href="#" class="easyui-linkbutton">Refresh</a>
                                            </td>
                                            <td style="width: 120px">
                                                <a id="btnDeleteAll_Notify" href="#" class="easyui-linkbutton">Delete All</a>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                                <div data-options="region:'center',border:false">
                                    <table id="dgNotify" class="easyui-datagrid" title="通知事项" style="width: auto" fit="true" pagination="false" rownumbers="true" singleselect="true"></table>
                                </div>
                            </div>
                        </div>
                        <div title="444">
                            <div class="easyui-layout" data-options="fit:true">
                                <div data-options="region:'north',split:false,border:false" style="height: 35px">
                                    <table style="height: 100%; width: 100%">
                                        <tr>
                                            <td style="width: 100px">
                                                <label id="lOvertimeColumn_Delay"></label>
                                            </td>
                                            <td style="width: 80px">
                                                <select id="sOvertimeColumn_Delay" class="easyui-combobox" style="width: 80px;">
                                                    <option value="0">0</option>
                                                    <option value="1">1</option>
                                                    <option value="2">2</option>
                                                    <option value="3">3</option>
                                                    <option value="4">4</option>
                                                    <option value="5">5</option>
                                                    <option value="6">6</option>
                                                    <option value="7">7</option>
                                                    <option value="8">8</option>
                                                    <option value="9">9</option>
                                                    <option value="10">10</option>
                                                </select>
                                            </td>
                                            <td style="width: 80px">
                                                <a id="btnRefresh_Delay" href="#" class="easyui-linkbutton">Refresh</a>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                                <div data-options="region:'center',border:false">
                                    <table id="dgDelay" class="easyui-datagrid" title="逾时事项" style="width: auto" fit="true" pagination="false" rownumbers="true" singleselect="true"></table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>--%>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="parameter" />
    </form>
</body>
</html>
