<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT304.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        function exportNot()
        {
            //$('#btnIns').show();
            $("#JQDataGrid1").datagrid('setWhere', "b.closedat is null"); //
            exportGrid("#JQDataGrid1");
        }

        function btnReloadClick() {
            $('#dataGridMaster').datagrid('reload');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT304.cmRT304" runat="server" AutoApply="True"
                DataMember="cmRT304" Pagination="True" QueryTitle="查詢條件"
                Title="Seednet每日交易檔及逢8結算檔查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="帳單編號" Editor="text" FieldName="csnoticeid" Format="" MaxLength="10" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶編號" Editor="text" FieldName="cscusid" Format="" MaxLength="15" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="列帳年月" Editor="text" FieldName="accountym" Format="" MaxLength="5" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶名" Editor="text" FieldName="cusnc" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="實繳金額" Editor="text" FieldName="amt" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶方案" Editor="text" FieldName="memo" Format="" MaxLength="50" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費超商" Editor="text" FieldName="csname" Format="" MaxLength="20" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶至超商繳款日" Editor="text" FieldName="cspaydat" Format="" MaxLength="7" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="超商處理日期" Editor="text" FieldName="csseednetdat" Format="" MaxLength="7" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="Seednet收款日" Editor="text" FieldName="rcvdat" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="Seednet沖銷日" Editor="text" FieldName="abatedat" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="Seednet結算日" Editor="text" FieldName="closedat" Format="" MaxLength="0" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="結算檔匯入" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="列印Seednet未結算交易檔" Visible="True" OnClick="exportNot" Icon="icon-excel" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btnReloadClick" Text="資料更新" Visible="True" Icon="icon-reload" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="帳單編號" Condition="%" DataType="string" Editor="text" FieldName="csnoticeid" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="用戶編號" Condition="%" DataType="string" Editor="text" FieldName="cscusid" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="年月" Condition="=" DataType="string" Editor="text" FieldName="accountym" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="用戶名稱" Condition="%" DataType="string" Editor="text" FieldName="cusnc" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>
        <div id="plnot" hidden="hidden" >
        <JQTools:JQDataGrid ID="JQDataGrid1" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="cmRT3041" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT304.cmRT3041" RowNumbers="True" Title="Seednet未結算交易檔" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="True">
            <Columns>
                <JQTools:JQGridColumn Alignment="left" Caption="帳單編號" Editor="text" FieldName="csnoticeid" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="用戶編號" Editor="text" FieldName="cscusid" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="列帳年月" Editor="text" FieldName="accountym" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="用戶名稱" Editor="text" FieldName="cusnc" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="未結金額" Editor="text" FieldName="amt" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="用戶方案" Editor="text" FieldName="memo" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="繳費超商" Editor="text" FieldName="csname" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="用戶至超商繳款日" Editor="text" FieldName="cspaydat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="超商處理日" Editor="text" FieldName="csseednetdat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
            </Columns>
            <TooItems>
                <JQTools:JQToolItem Enabled="True" Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="Insert" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="Update" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="Delete" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="Apply" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="Cancel" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel檔" Visible="True" />
            </TooItems>
        </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
