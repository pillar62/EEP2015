<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT108.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>

        function btnRT107Click()
        {
            
            var sMODE = "I";
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var COMQ1 = row.COMQ1;
            var LINEQ1 = row.LINEQ1;
            var COMTYPE = row.COMTYPE;
            parent.addTab("用戶維護", "CBBN/RT107.aspx?COMQ1=" + COMQ1 + "&sMODE=" + sMODE + "&LINEQ1=" + LINEQ1 + "&COMTYPE=" + COMTYPE);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT108.RT108" runat="server" AutoApply="True"
                DataMember="RT108" Pagination="True" QueryTitle="查詢條件" EditDialogID="JQDialog1"
                Title="社區資料查詢" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Panel" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="社區" Editor="numberbox" FieldName="COMQ1" Format="" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線" Editor="numberbox" FieldName="LINEQ1" Format="" Visible="True" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="個案別" Editor="text" FieldName="COMTYPE" Format="" MaxLength="30" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="COMQ11" Editor="numberbox" FieldName="COMQ11" Format="" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="COMN" Format="" MaxLength="100" Visible="true" Width="160" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線IP" Editor="text" FieldName="IPADDR" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區附掛" Editor="text" FieldName="LINETEL" Format="" MaxLength="20" Visible="False" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="規模戶數" Editor="numberbox" FieldName="COMCNT" Format="" Visible="true" Width="64" />
                    <JQTools:JQGridColumn Alignment="right" Caption="用戶數" Editor="numberbox" FieldName="USERCNT" Format="" Visible="False" Width="50" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案別" Editor="text" FieldName="COMTYPENM" Format="" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開通日" Editor="datebox" FieldName="T1APPLYDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="撤線日" Editor="text" FieldName="RCOMDROP" Format="yyyy/mm/dd" Visible="true" Width="80" MaxLength="20" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="經銷商" Editor="text" FieldName="GROUPNC" Format="" MaxLength="30" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="工程師" Editor="text" FieldName="LEADER" Format="" MaxLength="30" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建置書" Editor="text" FieldName="COMAGREE" Format="" MaxLength="30" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="Reset電話" Editor="text" FieldName="TEL" Format="" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="整線派工" Editor="text" FieldName="YN_01" Format="" MaxLength="0" Visible="true" Width="64" />
                    <JQTools:JQGridColumn Alignment="left" Caption="整線完成" Editor="text" FieldName="YN_02" Format="" MaxLength="0" Visible="true" Width="64" />
                    <JQTools:JQGridColumn Alignment="left" Caption="審核完成" Editor="text" FieldName="YN_03" Format="" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="64">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply"
                        Text="存檔" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel"
                        Text="取消"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btnRT107Click" Text="客戶" Visible="True" Icon="icon-view" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區名稱" Condition="%" DataType="string" Editor="text" FieldName="COMN" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="個案別" Condition="=" DataType="string" Editor="inforefval" EditorOptions="title:'方案別',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="COMTYPE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="150" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="個案名稱" Condition="%" DataType="string" Editor="text" FieldName="COMTYPENM" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="工程師" Condition="%" DataType="string" Editor="text" FieldName="LEADER" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="經銷商" Condition="%" DataType="string" Editor="text" FieldName="GROUPNC" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="開通日起" Condition="&gt;=" DataType="datetime" Editor="datebox" FieldName="T1APPLYDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="150" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="開通日迄" Condition="&lt;=" DataType="datetime" Editor="datebox" FieldName="T1APPLYDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="150" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="同意書" Condition="%" DataType="string" Editor="text" FieldName="COMAGREE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="寬頻使用戶" Condition="%" DataType="string" Editor="text" FieldName="USERCNT" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區總戶數︰" Condition="%" DataType="string" Editor="text" FieldName="COMCNT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區主線IP含︰" Condition="%" DataType="string" Editor="text" FieldName="IPADDR" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區主線附掛含︰" Condition="%" DataType="string" Editor="text" FieldName="LINETEL" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="社區資料查詢">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RT108" HorizontalColumnsCount="2" RemoteName="sRT108.RT108" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="社區" Editor="numberbox" FieldName="COMQ1" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線" Editor="numberbox" FieldName="LINEQ1" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="個案別" Editor="text" FieldName="COMTYPE" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="COMQ11" Editor="numberbox" FieldName="COMQ11" Format="" Width="180" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="COMN" Format="" maxlength="100" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區附掛" Editor="text" FieldName="LINETEL" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="規模戶數" Editor="numberbox" FieldName="COMCNT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶數" Editor="numberbox" FieldName="USERCNT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案別" Editor="text" FieldName="COMTYPENM" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="開通日" Editor="datebox" FieldName="T1APPLYDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="撤線日" Editor="text" FieldName="RCOMDROP" Format="yyyy/mm/dd" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="經銷商" Editor="text" FieldName="GROUPNC" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="工程師" Editor="text" FieldName="LEADER" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建置書" Editor="text" FieldName="COMAGREE" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="Reset電話" Editor="text" FieldName="TEL" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址" Editor="text" FieldName="addr" Format="" maxlength="200" Width="400" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區IP" Editor="text" FieldName="IPADDR" Format="" maxlength="50" Width="400" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="聯絡人" Editor="text" FieldName="CTYContact" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="聯絡人電話" Editor="text" FieldName="CTYcontactTel" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地區" Editor="text" FieldName="AREANC" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="整線派工" Editor="text" FieldName="YN_01" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="整線完成" Editor="text" FieldName="YN_02" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="審核完成" Editor="text" FieldName="YN_03" Format="" maxlength="0" Width="180" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
</html>
