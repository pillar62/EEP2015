<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccessGroup.aspx.cs" Inherits="InnerPages_AccessGroup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <div class="easyui-layout" fit="true">
        <div data-options="region:'west'" style="width: 190px; height: 200px">
            <table id="gridGroupsFrom_AccessGroup" title="Group list" style="width: auto" fit="true" pagination="false" rownumbers="false" singleselect="true"></table>
        </div>
        <div data-options="region:'east'" style="width: 190px; height: 200px">
            <table id="gridGroupsTo_AccessGroup" title="Selected groups" style="width: auto" fit="true" pagination="false" rownumbers="false" singleselect="true"></table>
        </div>
        <div data-options="region:'center'" style="padding: 0px; text-align: center;">
            <a id="btnGroupsTo_AccessGroup" href="#" class="easyui-linkbutton" style="margin-top: 100px" onclick="btnGroupsTo_AccessGroupClick()">></a>
            <br />
            <br />
            <br />
            <a id="btnGroupsBack_AccessGroup" href="#" class="easyui-linkbutton" style="margin: auto" onclick="btnGroupsBack_AccessGroup()"><</a>
        </div>
    </div>
</body>
</html>
