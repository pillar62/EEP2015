<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccessUser.aspx.cs" Inherits="InnerPages_AccessUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <div class="easyui-layout" fit="true">
        <div data-options="region:'west'" style="width: 190px; height: 200px">
            <table id="gridUsersFrom_AccessUser" title="User list" style="width: auto" fit="true" pagination="false" rownumbers="false" singleselect="true"></table>
        </div>
        <div data-options="region:'east'" style="width: 190px; height: 200px">
            <table id="gridUsersTo_AccessUser" title="Selected users" style="width: auto" fit="true" pagination="false" rownumbers="false" singleselect="true"></table>
        </div>
        <div data-options="region:'center'" style="padding: 0px; text-align: center;">
            <a id="btnUsersTo_AccessUser" href="#" class="easyui-linkbutton" style="margin-top: 100px" onclick="btnUsersTo_AccessUserClick()">></a>
            <br />
            <br />
            <br />
            <a id="btnUsersBack_AccessUser" href="#" class="easyui-linkbutton" style="margin: auto" onclick="btnUsersBack_AccessUser()"><</a>
        </div>
    </div>
</body>
</html>
