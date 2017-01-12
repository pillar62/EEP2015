<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InfoLogin.aspx.cs" Inherits="InfoLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>EEP 2012</title>
    <link href="css/Logon2012.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        function onLoginLoaded() {
            if (isPostBack == "False") {
                GetLastUser();
                GetLastDBSolution();
            }
        }

        var isPostBack = "<%= IsPostBack %>";
        function getCookieVal(offset) {
            var endstr = document.cookie.indexOf(";", offset);
            if (endstr == -1)
                endstr = document.cookie.length;
            return unescape(document.cookie.substring(offset, endstr));
        }

        function GetCookie(name) {
            var arg = name + "=";
            var alen = arg.length;
            var clen = document.cookie.length;
            var i = 0;
            while (i < clen) {
                var j = i + alen;
                if (document.cookie.substring(i, j) == arg)
                    return getCookieVal(j);
                i = document.cookie.indexOf(" ", i) + 1;
                if (i == 0) break;
            }
            return null;
        }

        function SetCookie(name, value, expires) {
            var argv = SetCookie.arguments;
            var argc = SetCookie.arguments.length;

            var expires = (argc > 2) ? argv[2] : null;
            var path = (argc > 3) ? argv[3] : null;
            var domain = (argc > 4) ? argv[4] : null;
            var secure = (argc > 5) ? argv[5] : false;
            document.cookie = name + "=" + escape(value) +
            ((expires == null) ? "" : ("; expires=" + expires.toGMTString())) +     //
            ((path == null) ? "" : ("; path=" + path)) +
            ((domain == null) ? "" : ("; domain=" + domain)) +
            ((secure == true) ? "; secure" : "");
        }

        function ResetCookie() {
            var usr = document.getElementById('txtUserName').value;
            var expdate = new Date()

            SetCookie(usr, null, expdate);
        }

        function GetPwdAndChk() {
            var usr = document.getElementById('txtUserName').value;
            var pwd = GetCookie(usr);

            if (pwd != null) {
                document.getElementById('chkRememberPwd').checked = true;
                document.getElementById('txtPassword').value = pwd;
            }
            else {
                document.getElementById('chkRememberPwd').checked = false;
                document.getElementById('txtPassword').value = "";
            }
        }

        function SetPwdAndChk() {
            var usr = document.getElementById('txtUserName').value;
            SetLastUser(usr);

            if (document.getElementById('chkRememberPwd').checked == true) {
                var pwd = document.getElementById('txtPassword').value;

                var expdate = new Date()
                expdate.setTime(expdate.getTime() + 14 * (24 * 60 * 60 * 1000))

                SetCookie(usr, pwd, expdate);
            }
            else {
                ResetCookie();
            }

            var db = document.getElementById('ddlDataBase').value;
            var sol = document.getElementById('ddlSolution').value;
            SetLastDBSolution(db, sol)
        }

        function GetLastUser() {
            var id = "49BAC005-7D5B-4231-8CEA-16939BEACD67";
            var usr = GetCookie(id);
            if (usr != null) {
                document.getElementById('txtUserName').value = usr;
            }
            else {
                document.getElementById('txtUserName').value = "001";
            }

            GetPwdAndChk();
        }

        function SetLastUser(usr) {
            var id = "49BAC005-7D5B-4231-8CEA-16939BEACD67";

            var expdate = new Date()
            expdate.setTime(expdate.getTime() + 14 * (24 * 60 * 60 * 1000))

            SetCookie(id, usr, expdate);
        }

        function GetLastDBSolution() {
            var did = "2BCD80435-67EA-B52D-9E10-234EB74D1A165DCA";
            var db = GetCookie(did);

            if (db != null) {
                document.getElementById('ddlDataBase').value = db;
            }
            else {

            }
            var sid = "B2380ACE1-3B7A-E1D0-79AC-4512BAC397DB486D";
            var sol = GetCookie(sid);
            if (sol != null) {
                document.getElementById('ddlSolution').value = sol;
            }
            else {

            }
        }

        function SetLastDBSolution(db, sol) {
            var did = "2BCD80435-67EA-B52D-9E10-234EB74D1A165DCA";
            var sid = "B2380ACE1-3B7A-E1D0-79AC-4512BAC397DB486D";
            var expdate = new Date()
            expdate.setTime(expdate.getTime() + 14 * (24 * 60 * 60 * 1000))

            SetCookie(did, db, expdate);
            SetCookie(sid, sol, expdate);
        }

        function btnMouseOver(buttonname) {
            var btn = document.getElementById('btn' + buttonname + 'Container');
            btn.style.backgroundImage = "url('Image/login/btn" + buttonname + "_over.gif')";
        }

        function btnMouseOut(buttonname) {
            var btn = document.getElementById('btn' + buttonname + 'Container');
            btn.style.backgroundImage = "url('Image/login/btn" + buttonname + ".gif')";
        }
    </script>

</head>
<body onload="onLoginLoaded()">
    <form id="frmLogin" runat="server">
        <div align="center">
            <div align="center" style="margin-top: 119px; margin-left: 50px;">
                <table class="maintable" cellpadding="0" dir="ltr" frame="border">
                    <tr>
                        <td class="login_top" height="106">
                            <asp:Image ID="login_top" runat="server" ImageUrl="~/Image/Logon2012/Title.png" CssClass="login_top_image" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 0px; margin: 0px" height="176">
                            <table cellpadding="0" class="style1">
                                <tr>
                                    <td align="right" style="padding: 20px 20px 4px 20px;">
                                        <asp:Label ID="lbUser" runat="server" Text="User" CssClass="lab"></asp:Label>
                                    </td>
                                    <td align="left" style="padding: 20px 20px 4px 20px;">
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="control" onblur="GetPwdAndChk()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="padding: 4px 20px 4px 20px;">
                                        <asp:Label ID="lbPassword" runat="server" Text="Password" CssClass="lab"></asp:Label>
                                    </td>
                                    <td align="left" style="padding: 4px 20px 4px 20px;">
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="padding: 4px 20px 4px 20px;">
                                        <asp:Label ID="lbDataBase" runat="server" Text="DataBase" CssClass="lab"></asp:Label>
                                    </td>
                                    <td align="left" style="padding: 4px 20px 4px 20px;">
                                        <asp:DropDownList ID="ddlDataBase" runat="server" CssClass="control">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="padding: 4px 20px 4px 20px;">
                                        <asp:Label ID="lbSolution" runat="server" Text="Solution" CssClass="lab"></asp:Label>
                                    </td>
                                    <td align="left" style="padding: 4px 20px 4px 20px;">
                                        <asp:DropDownList ID="ddlSolution" runat="server" CssClass="control" OnSelectedIndexChanged="ddlSolution_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding: 4px 20px 20px 20px; width: 100%">
                                        <asp:CheckBox ID="chkRememberPwd" runat="server" Text="Remember password" CssClass="cbx" Height="12px"></asp:CheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Label ID="FailureText" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="login_bot" valign="middle" height="71">
                            <table>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="OKButton" runat="server" Text="login" ImageUrl="~/Image/Logon2012/Login_TW.png"
                                            OnClientClick="SetPwdAndChk()" OnClick="OKButton_Click" onmouseover="this.src='Image/Login/Login_over_tw.png'"
                                             /> <%--onmouseout="this.src='Image/Login/Login_tw.png'"--%>
                                    </td>

                                    <td width="50">
                                        &nbsp;</td>
                                    <td>
                                        <asp:ImageButton ID="CancelButton" runat="server" Text="Cancel" ImageUrl="~/Image/Logon2012/Cancel.png"
                                            OnClientClick="window.close();" onmouseover="this.src='Image/Login/cancel_over_tw.png'"
                                             /> <%--onmouseout="this.src='Image/Login/cancel_tw.png'"--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
