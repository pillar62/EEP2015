<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT303.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        function LinkRT3031(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var TYY = row.TYY;
            var TMM = row.TMM;
            parent.addTab("用戶應收應付帳款明細查詢-全部用戶", "CBBN/RT3031.aspx?TYY=" + TYY + "&TMM=" + TMM);
        }
        function LinkRT3032(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var TYY = row.TYY;
            var TMM = row.TMM;
            parent.addTab("用戶應收應付帳款明細查詢-已沖用戶", "CBBN/RT3032.aspx?TYY=" + TYY + "&TMM=" + TMM);
        }
        function LinkRT3033(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var TYY = row.TYY;
            var TMM = row.TMM;
            parent.addTab("用戶應收應付帳款明細查詢-未沖用戶", "CBBN/RT3033.aspx?TYY=" + TYY + "&TMM=" + TMM);
        }
        function MySelectY(rowIndex, rowData) {
            if (flag == false) {
                $('#JQDataGrid1').datagrid('getPanel').panel('setTitle', '用戶Excel表(年)');
                var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
                var TYY = row.TYY;
                var TMM = row.TMM;
                $("#JQDataGrid1").datagrid('setWhere', "a.TYY='" + TYY + "'"); //維護單 
                exportGrid("#JQDataGrid1");
            }
        }
        function MySelectM(rowIndex, rowData) {
            if (flag == false) {
                $('#JQDataGrid1').datagrid('getPanel').panel('setTitle', '用戶Excel表(年月)');
                var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
                var TYY = row.TYY;
                var TMM = row.TMM;
                $("#JQDataGrid1").datagrid('setWhere', "a.TYY='" + TYY + "' and a.TMM=" + TMM); //維護單 
                exportGrid("#JQDataGrid1");
            }
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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT303.cmRT303" runat="server" AutoApply="True"
                DataMember="cmRT303" Pagination="True" QueryTitle="查詢條件"
                Title="每月應收應付帳款查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="帳款年度" Editor="numberbox" FieldName="TYY" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="帳款月份" Editor="numberbox" FieldName="TMM" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="總金額" Editor="numberbox" FieldName="Expr1" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="已沖金額" Editor="numberbox" FieldName="Expr2" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="未沖金額" Editor="numberbox" FieldName="Expr4" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="總筆數" Editor="numberbox" FieldName="Expr3" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="已沖筆數" Editor="numberbox" FieldName="Expr5" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="未沖筆數" Editor="numberbox" FieldName="Expr6" Format="" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="用戶Excel表(年)" Visible="True" OnClick="MySelectY" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="用戶Excel表(月)" Visible="True" OnClick="MySelectM" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="全部用戶" Visible="True" Icon="icon-view" OnClick="LinkRT3031" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="已沖用戶" Visible="True" Icon="icon-view" OnClick="LinkRT3032" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="未沖用戶" Visible="True" Icon="icon-view" OnClick="LinkRT3033" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btnReloadClick" Text="資料更新" Visible="True" Icon="icon-reload" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="帳款年度" Condition="=" DataType="string" Editor="text" FieldName="TYY" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="帳款月份" Condition="=" DataType="string" Editor="infocombobox" EditorOptions="items:[{value:'',text:' ',selected:'true'},{value:'01',text:'01',selected:'false'},{value:'02',text:'02',selected:'false'},{value:'03',text:'03',selected:'false'},{value:'04',text:'04',selected:'false'},{value:'05',text:'05',selected:'false'},{value:'06',text:'06',selected:'false'},{value:'07',text:'07',selected:'false'},{value:'08',text:'08',selected:'false'},{value:'09',text:'09',selected:'false'},{value:'10',text:'10',selected:'false'},{value:'11',text:'11',selected:'false'},{value:'12',text:'12',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="TMM" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

        <div hidden="hidden">
        <JQTools:JQDataGrid ID="JQDataGrid1" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="cmRT30301" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT303.cmRT30301" RowNumbers="True" Title="用戶資料" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="True">
            <Columns>
                <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="comn" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="客戶" Editor="text" FieldName="cusnc" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="會計科目" Editor="text" FieldName="ACNAMEC" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="立帳年月" Editor="text" FieldName="yymm" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="項目名稱" Editor="text" FieldName="ITEMNC" Frozen="False" IsNvarChar="False" MaxLength="50" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="100">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="正/負" Editor="text" FieldName="PORM" Frozen="False" IsNvarChar="False" MaxLength="1" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="40">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="right" Caption="應收(付)金額" Editor="text" FieldName="AMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="right" Caption="已沖金額" Editor="text" FieldName="REALAMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="未沖金額" Editor="text" FieldName="surplus" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="產生日" Editor="text" FieldName="CDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80" Format="yyyy/mm/dd">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="沖帳日" Editor="text" FieldName="MDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80" Format="yyyy/mm/dd">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="text" FieldName="CANCELDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80" Format="yyyy/mm/dd">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="作廢原因" Editor="text" FieldName="CANCELMEMO" Frozen="False" IsNvarChar="False" MaxLength="100" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="200">
                </JQTools:JQGridColumn>
            </Columns>
            <TooItems>
                <JQTools:JQToolItem Enabled="True" Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="Insert" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="Update" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="Delete" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="Apply" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="Cancel" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="轉Excel" Visible="True" />
            </TooItems>
        </JQTools:JQDataGrid>
        </div>
    </form>
</body>
</html>
