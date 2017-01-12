<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MobileLogOn.aspx.cs" Inherits="MobileLogOn" %>

<%@ Register Assembly="JQMobileTools" Namespace="JQMobileTools" TagPrefix="jqm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%-- <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="jquery.mobile-1.3.2/jquery.mobile-1.3.2.min.css" rel="stylesheet" />
    <link href="js/themes/infolight.mobile.css" rel="stylesheet" />
    <link href="js/jquery.mobile-modify.css" rel="stylesheet" />
    <link href="css/jquery.swipeButton.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="js/jquery.json.js"></script>
    <script type="text/javascript" src="jquery.mobile-1.3.2/jquery.mobile-1.3.2.min.js"></script>
    <script type="text/javascript" src="js/jquery.infolight.mobile.js"></script>

    <script type="text/javascript" src="js/plugins/jquery.swipeButton.js"></script>
    <script type="text/javascript" src="js/jquery.infolight.mobile-wf.js"></script>
    <script type="text/javascript" src="MobileMainFlowPage.js"></script>--%>
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
                    <label for="userID" style="font-weight: bold">
                        UserID</label>
                    <input type="text" name="userID" id="userID" data-mini="true" style="width: 200px" />
                </div>
                <div data-role="fieldcontain">
                    <label for="password" style="font-weight: bold">
                        Password</label>
                    <input type="password" name="password" id="password" data-mini="true" />
                </div>
                <div data-role="fieldcontain">
                    <label for="database" style="font-weight: bold">
                        Database</label>
                    <select name="database" id="database" data-mini="true" data-native-menu="false" data-theme="c">
                    </select>
                </div>
                <div data-role="fieldcontain">
                    <label for="solution" style="font-weight: bold">
                        Solution</label>
                    <select name="solution" id="solution" data-mini="true" data-native-menu="false" data-theme="c">
                    </select>
                </div>
                <div id="error" data-role="popup" data-overlay-theme="a" data-theme="d" class="ui-content">
                    <p>
                        test
                    </p>
                </div>
                <fieldset class="ui-grid-b">
                    <div class="ui-block-a">
                        <a class="logon" data-mini='true' data-role='button' data-theme='b' onclick="javascript:$('.info-logon').logon('logon')">Logon</a>
                    </div>
                    <div class="ui-block-b">
                        <a class="cancel" data-mini='true' data-role='button' data-theme='b' onclick="javascript:$('.info-logon').logon('forget');">Cancel</a>
                    </div>
                    <div class="ui-block-c">
                        <label for="remember">
                            Remember</label>
                        <input type="checkbox" data-mini="true" name="remember" id="remember" />
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
