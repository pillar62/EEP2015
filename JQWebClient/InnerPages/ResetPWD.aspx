<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetPWD.aspx.cs" Inherits="Account" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../js/themes/default/easyui.css" rel="stylesheet" />
    <link href="../js/themes/icon.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.0.min.js"></script>
    <script src="../js/jquery.easyui.min.js"></script>
    <script src="../js/json2.js"></script>
    <script>
        $(function () {
            $('tr.register').hide();
            $('title').html('忘記密碼');
            //$('#buttonOK').html('重置');
            var db = getQueryStringByName('db');
        });
        function getErrorMessage(html) {
            if (html == '' || html == null) {
                return '';
            }
            var startIndex = html.indexOf('<title');
            var endIndex = html.indexOf('</title');
            if (startIndex > 0 && endIndex > startIndex + 7) {
                return html.substring(startIndex + 7, endIndex);
            }
            else {
                return html;
            }
        }

        function getQueryStringByName(name) {
            var result = location.search.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
            return decodeURI(result[1]);
        }
        function onOKClick() {
            $('div.posttext p').html('');
            var db = getQueryStringByName('db');
            if ($('#user').val() == '') {
                $('div.posttext p').html('用戶名不能為空');
                return;
            }
            if ($('#email').val() == '') {
                $('div.posttext p').html('Email不能為空');
                return;
            }

            var match = $('#user').val().match(/[\w\d]+/g);
            if (match && match.length > 0 && match[0].length == $('#user').val().length) {

            }
            else {
                $('div.posttext p').html('用戶名只能允許英文字母或數字');
                return;
            }
            var data = { user: $('#user').val(), email: $('#email').val(), database: db };
            $('#buttonOK').hide();
            $.ajax({
                type: 'post',
                dataType: 'text',
                url: '../handler/SystemHandle.ashx?type=resetUserP',
                data: data,
                async: true,
                cache: false,
                success: function () {
                    alert('帳號密碼已經重置，請登入Email取得新的密碼');
                    window.location.href = '../LogOn.aspx';
                },
                error: function (data) {
                    //var excection = $.parseJSON(data.responseText);
                    var exception = {};
                    exception.message = getErrorMessage(data.responseText);
                    //                        if (exception.message == 'MaxUserExceed') {
                    //                            window.alert('本服務器已經滿載..為你跳轉到備用服務器!');
                    //                            window.location.href = $('#server').val() + '/Account.aspx?p=register';
                    //                        }
                    if (exception.message == 'EmailError') {
                        $('div.posttext p').html('輸入的Email地址不正確');
                    }
                    else if (exception.message == 'UserNotFound') {
                        $('div.posttext p').html('該用戶不存在');
                    }
                    else {
                        $('div.posttext p').html(exception.message);
                    }
                    data.responseText = '';
                },
                complete: function () {
                    $('#buttonOK').show();
                }
            });
        }
        function nochinesekeyup(target) {
            //            var value = target.value.replace(/[^\w\.\/]/ig, '');
            //            $(target).val(value);
            //            if (target.value.length > 10){
            //                alert('The password can not be more than 10');
            //                $(target).val(value.slice(0, 10));
            //            }
        }
    </script>
    <style>
        .modify_logon_body .logo {
            top: 5%;
        }

        .modify_logon_body .posttext {
            top: 20%;
        }

            .modify_logon_body .info table input {
                height: 25px;
            }

            .modify_logon_body .info table input {
            }

        .modify_logon_body span {
            vertical-align: middle;
        }

        body.modify_logon_body span.combo {
            background-color: #FFFFFF;
        }

        body.modify_logon_body span.combo-arrow {
            background: url('../js/themes/default/images/combo_arrow.png') no-repeat center center;
            background-color: #E0ECFF;
            opacity: 0.600000023841858;
        }

        .modify_logon_body .posttext table input.combo-text {
            padding: 0px 2px;
        }


        .modify_logon_body .container .posttext table tr {
            height: 22px;
        }

            .modify_logon_body .container .posttext table tr td.caption {
                padding-top: 10px;
            }

        .modify_logon_body .posttext table tr + tr + tr + tr + tr + tr + tr td {
            padding-top: 0px;
        }

        .modify_logon_body table {
            border-collapse: collapse;
            border-spacing: 0;
        }

        .modify_logon_body .logo {
            position: absolute;
            top: 12%;
            left: 50%;
            margin-left: -182px;
        }


        .validate {
            color: white;
        }

        .caption {
            color: white;
        }
        .buttonStyle2 {
    text-align: center;
    background-image: url(../img/bt_bg.png);
    background-repeat: no-repeat;
    background-color: #014971;
    width: 117px;
    height: 35px;
    list-style-type: none;
    text-decoration: none;
    font-size: 13px;
    font-family: Verdana, Geneva, sans-serif;
    color: #FFF;
    border-style: none;
}
        #bt2 {
    position: relative;
    height: 35px;
    width: 293px;
    text-align: right;
    left: 284px;
    top: -62px;
}

    #bt2 li {
        list-style-type: none;
        width: 117px;
        height: 35px;
        float: left;
        margin: 2px;
        vertical-align: top;
    }

        #bt2 li a {
            text-align: center;
            background-image: url(../img/bt_bg.png);
            background-repeat: no-repeat;
            width: 117px;
            height: 35px;
            padding-top: 8px;
            list-style-type: none;
            text-decoration: none;
            display: block;
            font-size: 13px;
            font-family: Verdana, Geneva, sans-serif;
            color: #FFF;
        }

            #bt2 li a:hover {
                display: block;
                font-size: 14px;
                color: #000;
                font-family: Verdana, Geneva, sans-serif;
            }
    </style>
</head>
<body class="modify_logon_body">
    <form id="form1" runat="server">
        <div id="container">
            <div id="header"></div>
            <div id="info" class="posttext">
                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                    <tr>
                        <td width="40%" rowspan="14" align="center" valign="middle">
                            <img src="../img/logo.png" width="167" height="54" /></td>
                        <td width="14%">&nbsp;</td>
                        <td width="46%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <img src="../img/EEP.png" width="307" height="56" /></td>
                    </tr>
                    <tr>
                        <td class="whtext">
                            <asp:Label ID="UserIDLabel" runat="server" Text="用戶:"></asp:Label>
                        </td>
                        <td>
                            <input type="text" id="user" onkeyup='nochinesekeyup(this);' placeholder="請輸入帳號名稱">
                        </td>
                    </tr>
                    <tr>
                        <td class="whtext">
                            <asp:Label ID="emailLabel" runat="server" Text="Email:"></asp:Label>
                        </td>
                        <td>
                            <input type="text" id="email" placeholder="請正確填寫（需認證）" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 50px">
                            <p class="validate">
                            </p>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="bt2">
                <ul>
                    <li>
                        <a href="../LogOn.aspx" id="returnA">
                            <asp:Label ID="BackLabel" runat="server" CssClass="buttonStyle2" Text="返回:"></asp:Label></a>
                    </li>
                    <li>
                        <a href="#" id="buttonOK" onclick="javascript:onOKClick();" style="color: #ffffff; margin-right: 10px">
                            <asp:Label ID="OKLabel" runat="server" CssClass="buttonStyle2" Text="OK:"></asp:Label></a>
                        </li>
                </ul>
            </div>
        </div>
        <asp:HiddenField ID="server" runat="server" />
    </form>
</body>
</html>
