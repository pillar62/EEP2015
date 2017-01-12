<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MobileResetPWD.aspx.cs" Inherits="MobileLogOn" %>

<%@ Register Assembly="JQMobileTools" Namespace="JQMobileTools" TagPrefix="jqm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <JQMobileTools:JQScriptManager runat="server" ID="JQScriptManager" SubFolder="true"
            LocalScript="true" />
        <div data-role="page" data-theme="b" class="indexbg">
            <div data-role="header" data-theme="b">
                <h1>EEP2015</h1>
            </div>
            <div class="info-logon" data-role="content" data-theme="b" style="margin: 0 auto; min-width: 100px; max-width: 400px; min-height: 370px">
                <div data-role="fieldcontain">
                    <label for="user" style="font-weight: bold">
                        UserID</label>
                    <input type="text" name="user" id="user" data-mini="true" style="width: 200px" />
                </div>
                <div data-role="fieldcontain">
                    <label for="email" style="font-weight: bold">
                        Email</label>
                    <input type="text" name="email" id="email" data-mini="true" />
                </div>
                <div id="reseterror" data-role="popup" data-overlay-theme="a" data-theme="d" class="ui-content">
                    <p>
                        test
                    </p>
                </div>
                <fieldset class="ui-grid-b">
                    <div class="ui-block-a">
                        <a class="return" href="../MobileLogOn.aspx" data-mini='true' data-role='button' data-theme='b'>Return</a>
                    </div>
                    <div class="ui-block-b">
                        <a class="reset" data-mini='true' data-role='button' data-theme='b' onclick="javascript:$('.info-logon').logon('resetP');;">Reset</a>
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
