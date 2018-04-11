<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT206.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var flag = true;
        function MySelect(rowIndex, rowData) {
            if (flag == false) {
                var ss = rowData.INVNO;
                $("#JQDataGrid1").datagrid('setWhere', "INVNO='" + ss + "'"); //發票明細
            }
        }
        function dgOnloadSuccess() {
            flag = false;

            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.INVNO;
            $("#JQDataGrid1").datagrid('setWhere', "INVNO='" + ss + "'");
        }

        function LinkRT2061(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.INVNO;
            parent.addTab("發票明細", "CBBN/RT2061.aspx?INVNO=" + ss);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT206.RTInvoice" runat="server" AutoApply="True"
                DataMember="RTInvoice" Pagination="True" QueryTitle="查詢條件" EditDialogID="JQDialog1"
                Title="發票主檔維護" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Panel" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False" OnSelect="MySelect">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="發票號碼" Editor="text" FieldName="INVNO" Format="" MaxLength="10" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="發票日期" Editor="datebox" FieldName="INVDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="發票抬頭" Editor="text" FieldName="INVTITLE" Format="" MaxLength="50" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="公司統編" Editor="text" FieldName="UNINO" Format="" MaxLength="10" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="檢查號" Editor="text" FieldName="CHKNO" Format="" MaxLength="10" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="發票聯數" Editor="text" FieldName="INVTYPE" Format="" MaxLength="0" Visible="true" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="課稅別" Editor="inforefval" FieldName="TAXTYPE" Format="" Visible="true" Width="60" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P1'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" MaxLength="1" />
                    <JQTools:JQGridColumn Alignment="right" Caption="銷售額" Editor="numberbox" FieldName="SALESUM" Format="N0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="right" Caption="稅額" Editor="numberbox" FieldName="TAXSUM" Format="N0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="right" Caption="發票總金額" Editor="numberbox" FieldName="TOTALSUM" Format="N0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="right" Caption="列印批次" Editor="numberbox" FieldName="BATCH" Format="" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="發票作廢日期" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="90" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="True"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-view" ItemType="easyui-linkbutton" OnClick="LinkRT2061" Text="發票明細" Visible="True" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="發票號碼" Condition="%" DataType="string" Editor="text" FieldName="INVNO" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="80" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="發票日期起" Condition="&gt;=" DataType="string" Editor="datebox" FieldName="INVDAT" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="90" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="發票日期迄" Condition="&lt;=" DataType="string" Editor="datebox" FieldName="INVDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="90" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="發票抬頭" Condition="%" DataType="string" Editor="text" FieldName="INVTITLE" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="公司統編" Condition="%" DataType="string" Editor="text" FieldName="UNINO" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="90" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="發票聯數" Condition="%" DataType="string" Editor="text" FieldName="INVTYPE" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="80" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="課稅別" Condition="%" DataType="string" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P1'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="TAXTYPE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="80" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="作廢日期" Condition="&gt;=" DataType="string" Editor="datebox" FieldName="CANCELDAT" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="90" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="發票作廢日期" Condition="&lt;=" DataType="string" Editor="datebox" FieldName="CANCELDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="90" />
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="發票主檔維護">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTInvoice" HorizontalColumnsCount="2" RemoteName="sRT206.RTInvoice" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" Width="800px" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="發票號碼" Editor="text" FieldName="INVNO" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="檢查號" Editor="text" FieldName="CHKNO" Format="" maxlength="10" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發票日期" Editor="datebox" FieldName="INVDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司統編" Editor="text" FieldName="UNINO" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發票抬頭" Editor="text" FieldName="INVTITLE" Format="" maxlength="50" Span="2" Width="400" />
                        <JQTools:JQFormColumn Alignment="left" Caption="課稅別" Editor="inforefval" FieldName="TAXTYPE" Format="" maxlength="1" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P1'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發票聯數" Editor="text" FieldName="INVTYPE" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="銷售額" Editor="numberbox" FieldName="SALESUM" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="稅額" Editor="numberbox" FieldName="TAXSUM" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發票總金額" Editor="numberbox" FieldName="TOTALSUM" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="列印批次" Editor="numberbox" FieldName="BATCH" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案別" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'L5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="CASETYPE" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款來源別" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'R1'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="ARSRC" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" maxlength="12" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="國字金額 (KIND=E6)" Editor="text" FieldName="AMTC" Format="" maxlength="8" Visible="False" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發票日期(民國)" Editor="text" FieldName="INVDATC" Format="" maxlength="7" Visible="False" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="最後異動人員" Editor="infocombobox" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="UUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="最後異動日期" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" MaxLength="250" Span="2" Width="400" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
        <JQTools:JQDataGrid ID="JQDataGrid1" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="RTInvoiceSub" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT206.RTInvoiceSub" RowNumbers="True" Title="發票明細" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="True">
            <Columns>
                <JQTools:JQGridColumn Alignment="left" Caption="發票號碼" Editor="text" FieldName="INVNO" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="20">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRY" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="產品名稱" Editor="text" FieldName="PRODNC" Frozen="False" IsNvarChar="False" MaxLength="100" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="200">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="right" Caption="數量" Editor="text" FieldName="QTY" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="right" Caption="單價" Editor="text" FieldName="UNITAMT" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90" Format="N4">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="right" Caption="銷售額" Editor="text" FieldName="SALEAMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="right" Caption="稅額" Editor="text" FieldName="TAXAMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMOSUB" Frozen="False" IsNvarChar="False" MaxLength="250" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="500">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="最後異動人員" Editor="text" FieldName="UUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="最後異動日期" Editor="text" FieldName="UDAT" Format="yyyy/mm/dd" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
            </Columns>
            <TooItems>
                <JQTools:JQToolItem Enabled="True" Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="Insert" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="Update" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="Delete" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="Apply" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="Cancel" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="False" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="Export" Visible="False" />
            </TooItems>
        </JQTools:JQDataGrid>
    </form>
</body>
</html>
