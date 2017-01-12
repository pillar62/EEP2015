<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPage.aspx.cs" Inherits="MainPage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="js/themes/default/easyui.css" rel="stylesheet" />
    <link href="js/themes/icon.css" rel="stylesheet" />
    <script src="js/jquery-1.8.0.min.js"></script>
    <script src="js/jquery.easyui.min.js"></script>
    <script src="js/jquery.json.js" type="text/javascript"></script>
    <script src="js/jquery.infolight.js" type="text/javascript"></script>
    <script src="MainPage.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            setUserInfo();
            renderMain();

            $(window).bind('beforeunload', function (e) {
                var x = window.event.clientX;
                var y = window.event.clientY;
                if (y < 0) {
                    logout();
                }
            });
        });

        function setlanguage(currentLang) {
            var localstring = $.sysmsg('getValue', 'JQWebClient/mainpagelinkbutton');
            var local = localstring.split(',');
            var homeloacl = local[0];
            var menuslocal = local[1];
            var aboutlocal = local[2];
            var logoutlocal = local[3];
            var changePasswordlocal = local[4];
            var mainpage = local[5];
            $('#homeMenuButton').text(homeloacl);
            $('#btn-mmMenus').text(menuslocal);
            $('#aboutMenuButton').text(aboutlocal);
            $('#aboutMenuButton').text(aboutlocal);
            $('#logOutMenuButton').text(logoutlocal);
            $('#changePasswordMenuButton').text(changePasswordlocal);
            var hometab = $('#tabsMain').tabs('getTab', 0)
            $('#tabsMain').tabs('update', {
                tab: hometab,
                options: {
                    title: mainpage
                }
            });
            //var oid = $(this).attr("id");
            //var buckle = $(this).children("buckle").text();
        }

        function setUserInfo() {
            $.ajax({
                type: "POST",
                url: "handler/SystemHandle.ashx?Type=USER",
                cache: false,
                async: true,
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
</head>
<body class="easyui-layout">
    <form id="form1" runat="server">
        <div style="width: 1280px; height: 66px; background-image: url(img/head.jpg);" title=""
            data-options="region:'north',split:false" border="false">
            <div style="padding: 2px; border: 0px solid #ddd">
                <a id='homeMenuButton' href="MainPage.aspx" class="easyui-linkbutton l-btn l-btn-plain"
                    data-options="plain:true"><span class="l-btn-left"><span class="l-btn-text">Home</span></span></a>
                <JQTools:JQMenuButton ID="JQMenuButton2" runat="server" MenuId="mmMenus" />
                <a id='logOutMenuButton' href="LogOn.aspx" onclick="logout()" class="easyui-linkbutton l-btn l-btn-plain"
                    data-options="plain:true"><span class="l-btn-left"><span class="l-btn-text">Log out</span></span></a>
                <a id='changePasswordMenuButton' onclick="changePassword();" class="easyui-linkbutton l-btn l-btn-plain"
                    data-options="plain:true"><span class="l-btn-left"><span class="l-btn-text">Change Password</span></span></a>
                <a id='aboutMenuButton' href="#" class="easyui-menubutton" data-options="menu:'#mm3'">About</a>
                <div id="mm3" class="menu-content" style="background: #f0f0f0; padding: 10px; text-align: left">
                    <img src="Image/Logon2012/Title.png">
                    <p style="font-size: 14px; color: #444;">
                        ©Infolight
                    </p>
                </div>
                <a id='userInfoMenuButton' class="easyui-linkbutton l-btn l-btn-plain" data-options="plain:true"></a>
            </div>
        </div>
        <div title="Menu" data-options="region:'west',title:'Menu',split:true" style="width: 193px; height: 790px; background-image: url(img/left.jpg);">
            <%--<ul id="treeMain" class="easyui-tree">
            </ul>--%>

            <input id="ddlSolution" class="easyui-combobox" style="width: 185px" data-options="valueField:'ID',textField:'Name'" />
            <JQTools:JQTree ID="JQTree1" runat="server" />
        </div>
        <div data-options="region:'center'">
            <div id="tabsMain" class="easyui-tabs" fit="true">
                <div title="首頁" style="background-image: url(img/main.jpg);">
                    <img class="maincenter" src="img/img.png" width="557" height="371">
                </div>
            </div>
        </div>
    </form>
</body>
</html>
