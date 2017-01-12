<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JQuerySingle.aspx.cs" Inherits="MyPage_Customers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../js/themes/default/easyui.css" rel="stylesheet" />
    <link href="../js/themes/icon.css" rel="stylesheet" />
    <script src="../js/jquery-1.8.0.min.js"></script>
    <script src="../js/jquery.easyui.min.js"></script>
    <script src="../js/jquery.json.js"></script>
    <script src="../js/datagrid-detailview.js"></script>
    <script src="../js/jquery.infolight.js"></script>
    <script src="../js/locale/easyui-lang-zh_TW.js"></script>
    <script type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <table id="dg" class="info-datagrid" title="" data-options="toolbar: '#tb',pagination:true,view:commandview"
            infolight-options="autoApply:true,remoteName:'',tableName:'',queryDialog:'#query'">
            <thead>
                <tr>
                   
                </tr>
            </thead>
        </table>
         <div id="Div1" style="height: auto">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true"
                onclick="insertItem('#dg')">新增</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true"
                onclick="apply('#dg')">確定</a> 
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-cancel',plain:true"
                 onclick="cancel('#dg')">取消</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true"
                onclick="openQuery('#dg')">查詢</a>
        </div>

        <div id="query" class="easyui-window" title="查詢" style="padding: 10px; width: 360px; height: 100;"
            iconcls="icon-search" closed="true" maximizable="false" minimizable="false"
            collapsible="false">
            <div>
                <table>
                    <tr>                       
                        <td>
                            <a class="easyui-linkbutton" iconcls="icon-search" href="javascript:void(0);" onclick="query('#dg')">Query</a>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
         <div id="dlg" class="easyui-dialog" style="width: 600px; padding: 10px 20px; display:none" closed="true"
            buttons="#dlg-buttons" infolight-options="containForm:'#ff'">
            <div id="ff" method="post" infolight-options="remoteName:'',tableName:''">
                <table>                   
                </table>
            </div>
            <div style="text-align: center; padding: 5px">
                <a href="javascript:void(0)" class="easyui-linkbutton" onclick="submitForm('#dlg')">確定</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" onclick="closeForm('#dlg')">取消</a>
            </div>
        </div>
    </form>
</body>
</html>
