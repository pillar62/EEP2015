<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT104511.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID"); //用戶編號
        var sMODE = Request.getQueryStringByName2("sMODE");
        var PRTNO = Request.getQueryStringByName2("PRTNO"); //維修單號

        var flag = true;

        function InsDefault() {
            if (CUSID != "") {
                return CUSID;
            }
        }

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = "CUSID='" + CUSID + "' AND PRTNO = '" + PRTNO + "'";
                $("#dataGridView").datagrid('setWhere', sWhere);
                $("#dataGridView").datagrid("selectRow", 0);
                setTimeout(function () {
                    if (sMODE == 'I') {
                        openForm('#JQDialog1', $('#dataGridView').datagrid('getSelected'), "inserted", 'Dialog');
                    }
                    else
                    if (sMODE == 'E') {
                        openForm('#JQDialog1', $('#dataGridView').datagrid('getSelected'), "updated", 'Dialog');
                    }
                }, 500);
                flag = false;
            }
            flag = false;
        }
       
        
        function CloseTab()
        {
            window.parent.closeCurrentTab();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>

            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT1045.RTLessorAVSCustDropSndwork" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustDropSndwork" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="用戶復機裝機派工單資料維護" AllowAdd="False" AlwaysClose="True" OnDeleted="CloseTab" OnUpdated="CloseTab" OnLoadSuccess="dgOnloadSuccess" AllowDelete="True" AllowUpdate="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租序號" Editor="text" FieldName="ENTRYNO" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" Visible="true" Width="120" MaxLength="12" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="SENDWORKDAT" Format="yyyy/mm/dd" Visible="True" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="收款日" Editor="datebox" FieldName="RCVMONEYDAT" Format="" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="列印日期" Editor="datebox" FieldName="PRTDAT" Format="" MaxLength="0" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="列印人員" Editor="text" FieldName="PRTUSR" Format="" MaxLength="6" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="預定施工員" Editor="text" FieldName="ASSIGNENGINEER" Format="" MaxLength="6" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="實際施工員" Editor="text" FieldName="REALENGINEER" Format="" MaxLength="6" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="預定施工經銷商" Editor="text" FieldName="ASSIGNCONSIGNEE" Format="" MaxLength="10" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="實際施工經銷商" Editor="text" FieldName="REALCONSIGNEE" Format="" Visible="False" Width="120" MaxLength="10" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工結案日" Editor="datebox" FieldName="CLOSEDAT" Format="" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="未完工結案日" Editor="datebox" FieldName="UNCLOSEDAT" Format="" MaxLength="0" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="CLOSEUSR" Format="" MaxLength="6" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="未完工/作廢原因" Editor="text" FieldName="DROPDESC" Format="" MaxLength="200" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金計算月" Editor="text" FieldName="BONUSCLOSEYM" Format="" Visible="False" Width="120" MaxLength="6" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金計算日" Editor="datebox" FieldName="BONUSCLOSEDAT" Format="" MaxLength="0" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金結算員" Editor="text" FieldName="BONUSCLOSEUSR" Format="" Visible="False" Width="120" MaxLength="6" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金會計審核日" Editor="datebox" FieldName="BONUSFINCHK" Format="" MaxLength="0" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="庫存計算月" Editor="text" FieldName="STOCKCLOSEYM" Format="" Visible="False" Width="120" MaxLength="6" />
                    <JQTools:JQGridColumn Alignment="left" Caption="庫存計算日" Editor="datebox" FieldName="STOCKCLOSEDAT" Format="" MaxLength="0" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="庫存計算員" Editor="text" FieldName="STOCKCLOSEUSR" Format="" Visible="False" Width="120" MaxLength="6" />
                    <JQTools:JQGridColumn Alignment="left" Caption="庫存會計審核日" Editor="datebox" FieldName="STOCKFINCHK" Format="" MaxLength="0" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建檔人員" Editor="text" FieldName="EUSR" Format="" Visible="False" Width="120" MaxLength="6" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建檔日期" Editor="datebox" FieldName="EDAT" Format="" MaxLength="0" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="UUSR" Format="" Visible="False" Width="120" MaxLength="6" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動日期" Editor="datebox" FieldName="UDAT" Format="" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="DROPDAT" Format="" MaxLength="0" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="DROPUSR" Format="" MaxLength="6" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註說明" Editor="text" FieldName="MEMO" Format="" Frozen="False" IsNvarChar="False" MaxLength="300" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="120">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="True"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶復機裝機派工單資料維護" EditMode="Dialog">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCustDropSndwork" HorizontalColumnsCount="2" RemoteName="sRT1045.RTLessorAVSCustDropSndwork" OnApplied="CloseTab" OnCancel="CloseTab" IsAutoSubmit="False" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" maxlength="15" Width="180" ReadOnly="True" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="退租序號" Editor="numberbox" FieldName="ENTRYNO" maxlength="0" Width="80" ReadOnly="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" Width="110" MaxLength="12" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="完工日期" Editor="datebox" FieldName="SENDWORKDAT" Format="yyyy/mm/dd" Width="90" />
                        <JQTools:JQFormColumn Alignment="left" Caption="列印日期" Editor="datebox" FieldName="PRTDAT" Format="yyyy/mm/dd" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="列印人員" Editor="infocombobox" FieldName="PRTUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="預定拆機人員" Editor="infocombobox" FieldName="ASSIGNENGINEER" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="實際拆機人員" Editor="infocombobox" FieldName="REALENGINEER" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="預定拆機經銷商" Editor="text" FieldName="ASSIGNCONSIGNEE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="實際拆機經銷商" Editor="text" FieldName="REALCONSIGNEE" Format="" Width="180" MaxLength="10" />
                        <JQTools:JQFormColumn Alignment="left" Caption="完工結案日" Editor="datebox" FieldName="CLOSEDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="未完工結案日" Editor="datebox" FieldName="UNCLOSEDAT" Format="yyyy/mm/dd" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案人員" Editor="infocombobox" FieldName="CLOSEUSR" Format="" maxlength="6" Width="180" Span="1" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="未完工作廢原因" Editor="textarea" FieldName="DROPDESC" Format="" maxlength="200" Width="300" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="MDF位置" Editor="text" FieldName="MDF1" Width="180" MaxLength="0" />
                        <JQTools:JQFormColumn Alignment="left" Caption="端子版位置" Editor="text" FieldName="MDF2" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="SWITCH(PNA)編號" Editor="text" FieldName="HOSTNO" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="PORT號" Editor="text" FieldName="HOSTPORT" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="獎金計算月" Editor="text" FieldName="BONUSCLOSEYM" Format="" Width="180" MaxLength="6" />
                        <JQTools:JQFormColumn Alignment="left" Caption="獎金計算日" Editor="datebox" FieldName="BONUSCLOSEDAT" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="獎金結算員" Editor="infocombobox" FieldName="BONUSCLOSEUSR" Format="" Width="180" MaxLength="6" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="獎金會計審核日" Editor="datebox" FieldName="BONUSFINCHK" Format="yyyy/mm/dd" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="庫存計算月" Editor="text" FieldName="STOCKCLOSEYM" Format="" Width="180" MaxLength="6" />
                        <JQTools:JQFormColumn Alignment="left" Caption="庫存計算日" Editor="datebox" FieldName="STOCKCLOSEDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="庫存計算員" Editor="infocombobox" FieldName="STOCKCLOSEUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="庫存會計審核日" Editor="datebox" FieldName="STOCKFINCHK" Format="yyyy/mm/dd" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="180" MaxLength="0" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="DROPUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔人員" Editor="infocombobox" FieldName="EUSR" Format="" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" MaxLength="6" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日期" Editor="datebox" FieldName="EDAT" Format="yyyy/mm/dd" maxlength="0" Width="180" Span="1" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動人員" Editor="infocombobox" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="UUSR" Format="" MaxLength="6" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動日期" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註說明" Editor="textarea" EditorOptions="height:60" FieldName="MEMO" Format="" MaxLength="400" NewRow="False" ReadOnly="False" RowSpan="1" Span="2" Visible="True" Width="300" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQAutoSeq ID="JQAutoSeq1" runat="server" BindingObjectID="dataFormMaster" FieldName="ENTRYNO" />
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="CUSID" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="自動編號" FieldName="PRTNO" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="1" FieldName="ENTRYNO" RemoteMethod="True" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>

        </div>
    </form>
</body>
</html>
