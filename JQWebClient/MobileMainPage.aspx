<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MobileMainPage.aspx.cs" Inherits="MobileMainPage" %>

<%@ Register Assembly="JQMobileTools" Namespace="JQMobileTools" TagPrefix="jqm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%-- <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="jquery.mobile-1.3.2/jquery.mobile-1.3.2.min.css" rel="stylesheet" />
    <link href="js/themes/infolight.mobile.css" rel="stylesheet" />
    <link href="js/jquery.mobile-modify.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="js/jquery.json.js"></script>
    <script type="text/javascript" src="jquery.mobile-1.3.2/jquery.mobile-1.3.2.min.js"></script>
    <script type="text/javascript" src="js/jquery.infolight.mobile.js"></script>

    <link href="css/jquery.swipeButton.css" rel="stylesheet" />
    <script type="text/javascript" src="js/plugins/jquery.swipeButton.js"></script>
    <script type="text/javascript" src="js/jquery.infolight.mobile-wf.js"></script>
    <script type="text/javascript" src="MobileMainFlowPage.js"></script>--%>
</head>
<body>
    <form id="form2" runat="server">
        <JQMobileTools:JQScriptManager runat="server" ID="JQScriptManager" SubFolder="false" LocalScript="true" />
        <div data-role="page" data-theme="b" class="indexbg">
            <div data-role="header" data-theme="b">
                <h1>EEP2015</h1>
                <a class="refresh" data-mini="true" data-inline="true" data-icon="refresh" data-role="button"
                    data-iconpos="notext" data-theme="b">Refresh</a> <a class="logout" data-mini="true"
                        data-inline="true" data-icon="back" data-role="button" data-iconpos="notext"
                        data-theme="b" onclick="javascript:$('.info-main').main('logout');">Logout</a>
            </div>
            <div class="logo">
                <img src="js/images/logo.png" />
            </div>
            <div id="mainC" class="info-main" data-role="content" style="height: 1000px; overflow-x: hidden">
                <JQMobileTools:JQMetro ID="JQMetro1" runat="server" JQueryVersion="1.8.0" LocalScript="True" SubFolder="False" />
                <ul id="menu" data-role="listview" data-theme="d" data-divider-theme="b" data-filter="true" data-mini="true"
                    data-filter-placeholder="Search menu">
                </ul>
            </div>
            <div data-role="footer" data-theme="b" class="transparent-bg">
                <fieldset id="fieldsetMenu" class="ui-grid-a">
                    <div class="ui-block-a">
                        <a class="menu" data-mini="true" data-role="button" data-theme="b">Menu</a>
                    </div>
                    <div class="ui-block-b">
                        <a class="flow" data-mini="true" data-role="button" data-theme="b" onclick="gotoInbox()">Flow</a>
                    </div>
                </fieldset>
            </div>
        </div>
    </form>
</body>
</html>
