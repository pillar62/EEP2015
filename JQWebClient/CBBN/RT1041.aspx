<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1041.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID");
        var flag = true;

        function InsDefault() {
            if (CUSID != "") {
                return CUSID;
            }
        }

        function dgOnloadSuccess()
        {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = "CUSID='" + CUSID + "'";
                $("#dataGridView").datagrid('setWhere', sWhere);
            }
            flag = false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT1041.RTLessorAVSCustRepair" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustRepair" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="用戶維修收款" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.RTLessorAVSCust',tableName:'RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派單日期" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Visible="true" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費方式" Editor="infocombobox" FieldName="PAYTYPE" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="right" Caption="設定費" Editor="numberbox" FieldName="SETAMT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="移機費" Editor="numberbox" FieldName="MOVEAMT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="設備費" Editor="numberbox" FieldName="EQUIPAMT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款產生日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="12" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案日期" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" Format="" MaxLength="6" Visible="true" Width="120" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
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
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="轉應收結案" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="反轉應收結案" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="應收應付" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢反轉" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶維修收款">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCustRepair" HorizontalColumnsCount="2" RemoteName="sRT1041.RTLessorAVSCustRepair" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶序號" Editor="infocombobox" FieldName="CUSID" Format="" maxlength="15" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.RTLessorAVSCust',tableName:'RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" Format="" Width="32" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派單日期" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Width="70" />
                        <JQTools:JQFormColumn Alignment="left" Caption="設備代碼(KIND='P2')" Editor="infocombobox" FieldName="EQUIP" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P2'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="設備費" Editor="numberbox" FieldName="EQUIPAMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="設定費" Editor="numberbox" FieldName="SETAMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="移機費" Editor="numberbox" FieldName="MOVEAMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費方式" Editor="infocombobox" FieldName="PAYTYPE" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡種類" Editor="infocombobox" FieldName="CREDITCARDTYPE" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M6'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發卡銀行" Editor="infocombobox" FieldName="CREDITBANK" Format="" maxlength="3" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTBank',tableName:'RTBank',columns:[],columnMatches:[],whereItems:[{field:'CREDITCARD',value:'Y'}],valueField:'HEADNO',textField:'HEADNC',valueFieldCaption:'HEADNO',textFieldCaption:'HEADNC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡卡號" Editor="text" FieldName="CREDITCARDNO" Format="" maxlength="16" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="持卡人姓名" Editor="text" FieldName="CREDITNAME" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效期限(月)" Editor="text" FieldName="CREDITDUEM" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效期限(年)" Editor="text" FieldName="CREDITDUEY" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款產生日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" Width="180" MaxLength="12" />
                        <JQTools:JQFormColumn Alignment="left" Caption="帳款產生人員" Editor="infocombobox" FieldName="TUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案日期" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" Format="" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" MaxLength="6" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註說明" Editor="textarea" FieldName="MEMO" Format="" maxlength="500" Width="360" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="維修員工" Editor="infocombobox" FieldName="REALENGINEER" Format="" maxlength="6" Width="180" EditorOptions="valueField:'CUSID',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="維修經銷商" Editor="text" FieldName="REALCONSIGNEE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="最後異動日期" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" Width="180" maxlength="0" />
                        <JQTools:JQFormColumn Alignment="left" Caption="最後異動人員" Editor="infocombobox" FieldName="UUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'CUSID',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="收款日期" Editor="datebox" FieldName="RCVMONEYDAT" Format="yyyy/mm/dd" Width="180" maxlength="0" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="CUSID" RemoteMethod="False" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
</html>
