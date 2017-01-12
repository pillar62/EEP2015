<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MobileTimeout.aspx.cs" Inherits="MobileLogOn" %>

<%@ Register Assembly="JQMobileTools" Namespace="JQMobileTools" TagPrefix="jqm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <JQMobileTools:JQScriptManager runat="server" ID="JQScriptManager" SubFolder="false"
            LocalScript="true" />
        <div data-role="page" data-theme="b" class="indexbg">
            <div data-role="header" data-theme="b">
                <h1>EEP2015</h1>
            </div>
            <div class="info-logon" data-role="content" data-theme="b" style="margin: 0 auto; min-width: 100px; max-width: 400px; min-height: 370px">
                <div data-role="fieldcontain">
                    <br />
                    <br />
                    <p>
                        對不起，您的登錄逾時！請重新登錄!</p>
                    <p>
                        Sorry! Your Login already timeout! Please Relogin!</p>
                </div>
                <fieldset class="ui-grid-b">
                    <div class="ui-block-b">
                        <a class="relogin" data-mini='true' data-role='button' data-theme='b' onclick="javascript: window.parent.location.href = 'MobileLogOn.aspx';">Relogin</a>
                    </div>
                </fieldset>
            </div>
            <div data-role="footer" data-theme="b" class="transparent-bg">
                <h1>Copyright by Infolight</h1>
            </div>
        </div>
    </form>
</body>
</html>
