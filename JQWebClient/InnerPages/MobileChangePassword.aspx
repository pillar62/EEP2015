<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MobileChangePassword.aspx.cs" Inherits="InnerPages_ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>  

    <script type="text/javascript" >
        $(document).ready(function () {
            var NewPasswordErrorMessage = "兩次輸入的密碼不一致";
            var ChangeSucceed = "更改密碼成功";
            var ChangeError = "更改密碼失敗";
            var OldNewErrorMessage = "新舊密碼不能相同";

            var localstring = $.sysmsg('getValue', 'Srvtools/UPWDControl/LabelName');
            var local = localstring.split(';');
            var useridloacl = local[0];
            var oldpwdlocal = local[2];
            var newpwdlocal = local[3];
            var confimlocal = local[4];
            $('#labelUserID').text(useridloacl);
            $('#labelOPassword').text(oldpwdlocal);
            $('#labelNPassword').text(newpwdlocal);
            $('#labelCPassword').text(confimlocal);

            localstring = $.sysmsg('getValue', 'JQWebClient/dialogbuttontext');
            local = localstring.split(',');
            $('#ok')[0].value = local[2];
            NewPasswordErrorMessage = $.sysmsg('getValue', 'Srvtools/UPWDControl/NewPasswordErrorMessage');
            ChangeSucceed = $.sysmsg('getValue', 'Srvtools/UPWDControl/ChangeSucceed');
            ChangeError = $.sysmsg('getValue', 'Srvtools/UPWDControl/ChangeError');
            OldNewErrorMessage = $.sysmsg('getValue', 'Srvtools/UPWDControl/OldNewErrorMessage');

            $("#ok", this).unbind().bind("click", function () {
                var userID = $("#userID", info).val();
                var oPassword = $("#oPassword", info).val();
                var nPassword = $("#nPassword", info).val();
                var cPassword = $("#cPassword", info).val();
                if (nPassword != cPassword) {
                    alert(NewPasswordErrorMessage);
                }
                else if (oPassword == nPassword) {
                    alert(OldNewErrorMessage);
                }
                else {
                    $.ajax({
                        type: "POST",
                        url: "../handler/SystemHandle.ashx?Type=ChangePassword&UserID=" + userID + "&OPassword=" + oPassword + "&NPassword=" + nPassword,
                        cache: false,
                        async: true,
                        success: function (data) {
                            if (data == 'o') {
                                alert(ChangeSucceed);
                            }
                            else if (data == "e") {
                                alert(ChangeError);
                            }
                            else if (data.length > 0) {
                                alert(data);
                            }
                            else {
                                alert(ChangeError);
                            }
                        },
                        error: function (data) {
                            alert(data);
                        }
                    });
                }
            });
        });



        //set multilanguage
        $.sysmsg = function (methodName, value) {
            if (typeof methodName == "string") {
                var method = $.sysmsg.methods[methodName];
                if (method) {
                    return method(value);
                }
            }
        };

        $.sysmsg.messages = {};

        $.sysmsg.methods = {
            load: function (keys) {
                $.ajax({
                    type: "POST",
                    url: window.currentUrl,
                    dataType: 'json',
                    data: "mode=message&keys=" + $.toJSONString(keys),
                    cache: false,
                    async: false,
                    success: function (data) {
                        for (var key in data) {
                            $.sysmsg.messages[key] = data[key];
                        }
                    }, error: function (data) {


                    }
                });
            },
            getValue: function (key) {
                if ($.sysmsg.messages[key]) { }
                else {
                    var keys = [];
                    keys.push(key);
                    $.sysmsg('load', keys);
                }
                return $.sysmsg.messages[key];
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <JQMobileTools:JQScriptManager ID="JQScriptManager1" runat="server" />
           <div id="info" data-role="page" data-theme="b" class="indexbg ui-page ui-page-theme-b ui-page-active" data-url="MobileMainBackground" tabindex="0" style="min-height: 958px;">
    <a data-mini="true" data-inline="true" data-icon="back" data-role="button" data-iconpos="notext" data-theme="b" data-rel="back">Back</a><div class="logo">
                    <img src="../img/EEP.png"  height="56" />
            </div>
            <form class="ui-filterable">
                <div class="ui-shadow-inset ui-input-has-clear ui-body-d ui-corner-all">
                    <label id="labelUserID">
                        用戶編號:</label>
                    <input id="userID" />
                    <label id="labelOPassword">
                        舊密碼:</label>
                    <input id="oPassword" type="password"/>
                    <label id="labelNPassword">
                        新密碼:</label>
                    <input id="nPassword" type="password"/>
                    <label id="labelCPassword">
                        確認密碼:</label>
                     <input id="cPassword" type="password"/>
                    <input id="ok" type="button"  value="确定"/>
                   </div>
            </form>
 </div>
    </form>
</body>
</html>
