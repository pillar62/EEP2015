<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VBJQuerySingle1.aspx.vb" Inherits="Template_VBJQuerySingle1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="" runat="server" AutoApply="True"
                DataMember="" Pagination="True" QueryTitle="Query"
                Title="JQDataGrid">
                <Columns>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" />
                    <JQTools:jqtoolitem icon="icon-edit" itemtype="easyui-linkbutton"
                        onclick="updateItem" text="更改" />
                    <JQTools:jqtoolitem icon="icon-remove" itemtype="easyui-linkbutton"
                        onclick="deleteItem" text="刪除" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply"
                        Text="確定" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel"
                        Text="取消" />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>
    </form>
</body>
</html>

