<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT10423.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID"); //用戶編號
        var PRTNO = Request.getQueryStringByName2("PRTNO"); //維修單號

        var flag = true;

        function InsDefault() {
            if (PRTNO != "") {
                return PRTNO;
            }
        }

        function InsDefaultCUSID() {
            if (CUSID != "") {
                return CUSID;
            }
        }

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = "PRTNO = '" + PRTNO + "'";
                $("#dataGridView").datagrid('setWhere', sWhere);
                $("#dataGridView").datagrid("selectRow", 0);
            }
            flag = false;
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT10423.RTLessorAVSCustHardware" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustHardware" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="用戶裝機派工設備資料維護" OnLoadSuccess="dgOnloadSuccess" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" MaxLength="12" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Visible="true" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="設備名稱" Editor="infocombobox" FieldName="PRODNO" Format="" MaxLength="6" Visible="true" Width="120" EditorOptions="valueField:'PRODNO',textField:'PRODNC',remoteName:'sRT100.RTprodh',tableName:'RTprodh',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="規格" Editor="inforefval" FieldName="ITEMNO" Format="" MaxLength="3" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTprodd1',tableName:'RTprodd1',columns:[],columnMatches:[],whereItems:[{field:'PRODNO',value:'row[PRODNO]'}],valueField:'ITEMNO',textField:'SPEC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="right" Caption="數量" Editor="numberbox" FieldName="QTY" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="金額" Editor="numberbox" FieldName="AMT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="出庫別" Editor="infocombobox" FieldName="WAREHOUSE" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="valueField:'WAREHOUSE',textField:'WARENAME',remoteName:'sRT100.HBWAREHOUSE',tableName:'HBWAREHOUSE',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢原因" Editor="text" FieldName="DROPREASON" Format="" MaxLength="100" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="DROPUSR" Format="" MaxLength="6" Visible="true" Width="90" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="12" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轉應收帳款日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" Visible="true" Width="90" MaxLength="0" />
                    <JQTools:JQGridColumn Alignment="left" Caption="領用單號" Editor="text" FieldName="RCVPRTNO" Format="" MaxLength="13" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="領用結帳日" Editor="datebox" FieldName="RCVFINISHDAT" Format="yyyy/mm/dd" Visible="true" Width="90" />
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
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="轉領用單" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="領用單返轉" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="設備作廢" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢返轉" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="異動查詢" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶裝機派工設備資料維護">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCustHardware" HorizontalColumnsCount="2" RemoteName="sRT10423.RTLessorAVSCustHardware" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="180" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" maxlength="12" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="90" Span="2" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="設備名稱" Editor="infocombobox" FieldName="PRODNO" Format="" MaxLength="6" Visible="true" Width="180" EditorOptions="valueField:'PRODNO',textField:'PRODNC',remoteName:'sRT100.RTprodh',tableName:'RTprodh',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="規格" Editor="inforefval" FieldName="ITEMNO" Format="" MaxLength="3" Visible="true" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTprodd1',tableName:'RTprodd1',columns:[],columnMatches:[],whereItems:[{field:'PRODNO',value:'row[PRODNO]'}],valueField:'ITEMNO',textField:'SPEC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="數量" Editor="numberbox" FieldName="QTY" Format="" Width="90" />
                        <JQTools:JQFormColumn Alignment="left" Caption="單位" Editor="inforefval" FieldName="UNIT" Format="" maxlength="2" Width="90" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'B5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="金額" Editor="numberbox" FieldName="AMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="出庫別" Editor="infocombobox" FieldName="WAREHOUSE" Format="" maxlength="2" Width="180" EditorOptions="valueField:'WAREHOUSE',textField:'WARENAME',remoteName:'sRT100.HBWAREHOUSE',tableName:'HBWAREHOUSE',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="DROPDAT" Format="" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢原因" Editor="textarea" FieldName="DROPREASON" Format="" maxlength="100" Width="300" EditorOptions="height:30" ReadOnly="True" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="DROPUSR" Format="" maxlength="6" Width="180" ReadOnly="True" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" maxlength="12" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="轉應收帳款日" Editor="datebox" FieldName="TARDAT" Format="" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="轉應收帳款人員" Editor="text" FieldName="TUSR" Format="" maxlength="6" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="領用單號" Editor="text" FieldName="RCVPRTNO" Format="" maxlength="13" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="領用單日期" Editor="datebox" FieldName="RCVDAT" Format="" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="領用結帳日" Editor="datebox" FieldName="RCVFINISHDAT" Format="" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔人" Editor="text" FieldName="EUSR" Format="" maxlength="6" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日" Editor="datebox" FieldName="EDAT" Format="" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動人" Editor="text" FieldName="UUSR" Format="" maxlength="6" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動日" Editor="datebox" FieldName="UDAT" Format="" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="textarea" FieldName="MEMO" Format="" maxlength="50" Width="360" EditorOptions="height:40" Span="2" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQAutoSeq ID="JQAutoSeq1" runat="server" BindingObjectID="dataFormMaster" FieldName="ENTRYNO" />
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" FieldName="CUSID" RemoteMethod="False" DefaultMethod="InsDefaultCUSID" />
                        <JQTools:JQDefaultColumn CarryOn="False" FieldName="PRTNO" RemoteMethod="False" DefaultMethod="InsDefault" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today" FieldName="EDAT" RemoteMethod="True" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today" FieldName="UDAT" RemoteMethod="True" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_usercode" FieldName="EUSR" RemoteMethod="True" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_usercode" FieldName="UUSR" RemoteMethod="True" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
</html>
