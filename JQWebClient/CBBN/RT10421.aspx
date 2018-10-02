<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT10421.aspx.cs" Inherits="Template_JQuerySingle1" %>

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
                
                flag = false;
            }
            flag = false;
        }

        function Selected()
        {
            setTimeout(function () {
                if (sMODE == 'I') {
                    openForm('#JQDialog1', $('#dataGridView').datagrid('getSelected'), "inserted", 'Dialog');
                }
                else
                    if (sMODE == 'E') {
                        openForm('#JQDialog1', $('#dataGridView').datagrid('getSelected'), "updated", 'Dialog');
                    }
                    else
                        if (sMODE == 'V') {
                            openForm('#JQDialog1', $('#dataGridView').datagrid('getSelected'), "viewed", 'Dialog');
                        }
            }, 500);
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
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT1042.RTLessorAVSCustSndwork" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustSndwork" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="用戶裝機派工單資料維護" AllowAdd="False" AlwaysClose="True" OnDeleted="CloseTab" OnUpdated="CloseTab" OnLoadSuccess="dgOnloadSuccess" AllowDelete="True" AllowUpdate="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True" OnSelect="Selected">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" MaxLength="12" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="SENDWORKDAT" Format="yyyy/mm/dd" Visible="true" Width="90" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply"
                        Text="存檔" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel"
                        Text="取消"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶裝機派工單資料維護" EditMode="Dialog">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCustSndwork" HorizontalColumnsCount="2" RemoteName="sRT1042.RTLessorAVSCustSndwork" OnApplied="CloseTab" OnCancel="CloseTab" IsAutoSubmit="False" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" maxlength="15" Width="180" ReadOnly="True" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" maxlength="12" Width="110" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="完工日期" Editor="datebox" FieldName="SENDWORKDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="收款日期" Editor="datebox" FieldName="RCVMONEYDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="列印日期" Editor="datebox" FieldName="PRTDAT" Format="yyyy/mm/dd" Width="180" Visible="False" ReadOnly="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="列印人員" Editor="infocombobox" FieldName="PRTUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="預定施工人員" Editor="infocombobox" FieldName="ASSIGNENGINEER" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="實際施工人員" Editor="infocombobox" FieldName="REALENGINEER" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="預定施工經銷商" Editor="text" FieldName="ASSIGNCONSIGNEE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="實際施工經銷商" Editor="text" FieldName="REALCONSIGNEE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="完工結案日" Editor="datebox" FieldName="CLOSEDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案人員" Editor="infocombobox" FieldName="CLOSEUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" maxlength="0" Width="300" Span="2" ReadOnly="True" Visible="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="DROPUSR" Format="" maxlength="6" Width="180" Visible="True" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔人員" Editor="text" FieldName="EUSR" Format="" Width="180" Visible="True" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日期" Editor="infocombobox" FieldName="EDAT" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" Visible="True" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動人員" Editor="infocombobox" FieldName="UUSR" Format="yyyy/mm/dd" Width="180" Visible="True" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動日期" Editor="text" FieldName="UDAT" Format="yyyy/mm/dd" maxlength="6" Width="180" Visible="True" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註說明" Editor="textarea" FieldName="MEMO" Format="" Width="300" Visible="True" EditorOptions="height:60" MaxLength="300" Span="2" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="CUSID" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="自動編號" FieldName="PRTNO" RemoteMethod="False" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>

        </div>
    </form>
</body>
</html>
