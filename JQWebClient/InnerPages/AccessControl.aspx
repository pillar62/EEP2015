<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccessControl.aspx.cs" Inherits="InnerPages_AccessControl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <div id="AccessControlMain" class="easyui-layout" data-options="fit:true">
        <div data-options="region:'south',split:false" style="height: 40px; padding: 5px">
            <table style="height: 100%; width: 100%">
                <tr>
                    <td style="text-align: center"><a href="javascript:void(0)" id="btnAdd" class="easyui-linkbutton" data-options="plain:false">Add</a></td>
                    <td style="text-align: center"><a href="javascript:void(0)" id="btnDelete" class="easyui-linkbutton" data-options="plain:false">Delete</a></td>
                    <td style="text-align: center"><a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton" data-options="plain:false">Save</a></td>
                    <td style="text-align: center"><a href="javascript:void(0)" id="btnCancel" class="easyui-linkbutton" data-options="plain:false">Cancel</a></td>
                </tr>
            </table>
        </div>
        <div data-options="region:'center'">
            <table id="dgAccessControl" data-options="fit:true" class="easyui-datagrid" />
        </div>
    </div>
</body>
</html>
