<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1011.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var COMQ1 = Request.getQueryStringByName2("COMQ1");
        var LINEQ1 = Request.getQueryStringByName2("LINEQ1");
        var flag = true;
        if (COMQ1 == "")
        {
            flag = false;
        }

        function InsDefault()
        {
            if (COMQ1 != "")
            {
                return COMQ1;
            }            
        }

        function InsDefaultLineq() {
            if (LINEQ1 != "") {
                return LINEQ1;
            }
        }

        $(document).ready(function () {
            dgOnloadSuccess();
        })

        function dgOnloadSuccess() {
            if (flag) {
                var sWhere = "COMQ1='" + COMQ1 + "'";
                if (LINEQ1 != "")
                {
                    sWhere = sWhere + " AND LINEQ1='"+LINEQ1+"'"
                }
                    
                $("#dataGridView").datagrid('setWhere', sWhere);
            }
            flag = false;
        }

        //異動查詢
        function btn1Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            parent.addTab("物品領用單資料維護", "CBBN/RT10111.aspx?PRTNO=" + PRTNO);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT101.RTLessorAVSCmtyLineHARDWARE" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCmtyLineHARDWARE" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="用戶派工單設備資料維護" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" OnLoadSuccess="dgOnloadSuccess" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Visible="true" Width="120" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.RTLessorAVSCmtyH',tableName:'RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" MaxLength="12" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="序號" Editor="numberbox" FieldName="SEQ" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="設備名稱" Editor="infocombobox" FieldName="PRODNO" Format="" MaxLength="6" Visible="true" Width="120" EditorOptions="valueField:'PRODNO',textField:'PRODNC',remoteName:'sRT100.RTprodh',tableName:'RTprodh',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="設備規格" Editor="inforefval" FieldName="ITEMNO" Format="" MaxLength="3" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,remoteName:'sRT100.RTprodd1',tableName:'RTprodd1',columns:[],columnMatches:[],whereItems:[{field:'PRODNO',value:'row[PRODNO]'}],valueField:'ITEMNO',textField:'SPEC',valueFieldCaption:'細項編號',textFieldCaption:'規格',cacheRelationText:true,checkData:true,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="right" Caption="數量" Editor="numberbox" FieldName="QTY" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="金額" Editor="numberbox" FieldName="AMT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="庫別代號" Editor="infocombobox" FieldName="WAREHOUSE" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="valueField:'WAREHOUSE',textField:'WARENAME',remoteName:'sRT100.HBWAREHOUSE',tableName:'HBWAREHOUSE',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="領用單號" Editor="text" FieldName="RCVPRTNO" Format="" MaxLength="13" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="領用結案日" Editor="datebox" FieldName="RCVFINISHDAT" Format="yyyy/mm/dd" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主機編號" Editor="text" FieldName="HOSTNO" Format="" MaxLength="3" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="上層設備編號" Editor="text" FieldName="PRELEVELHOSTNO" Format="" MaxLength="3" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="上層設備PORT" Editor="text" FieldName="PRELEVELPORTNO" Format="" MaxLength="3" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租(撤銷)原因" Editor="text" FieldName="DROPREASON" Format="" MaxLength="100" Visible="true" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" Visible="False" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply"
                        Text="存檔" Visible="False" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel"
                        Text="取消" Visible="False"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="異動查詢" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶派工單設備資料維護">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCmtyLineHARDWARE" HorizontalColumnsCount="2" RemoteName="sRT101.RTLessorAVSCmtyLineHARDWARE" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Width="180" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.RTLessorAVSCmtyH',tableName:'RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" maxlength="12" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="項次" Editor="numberbox" FieldName="SEQ" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="設備名稱" Editor="infocombobox" FieldName="PRODNO" Format="" maxlength="6" Width="180" EditorOptions="valueField:'PRODNO',textField:'PRODNC',remoteName:'sRT100.RTprodh',tableName:'RTprodh',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="設備規格" Editor="inforefval" FieldName="ITEMNO" Format="" maxlength="3" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,remoteName:'sRT100.RTprodd1',tableName:'RTprodd1',columns:[{field:'ITEMNO',title:'細項編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'SPEC',title:'規格',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}],columnMatches:[],whereItems:[{field:'PRODNO',value:'row[PRODNO]'}],valueField:'ITEMNO',textField:'SPEC',valueFieldCaption:'細項編號',textFieldCaption:'規格',cacheRelationText:true,checkData:true,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="上層設備編號" Editor="text" FieldName="PRELEVELHOSTNO" Format="" maxlength="3" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="上層設備編號PORT" Editor="text" FieldName="PRELEVELPORTNO" Format="" maxlength="3" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主機編號" Editor="text" FieldName="HOSTNO" Format="" maxlength="3" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="數量" Editor="numberbox" FieldName="QTY" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="單位" Editor="text" FieldName="UNIT" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="金額" Editor="numberbox" FieldName="AMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="庫別代號" Editor="infocombobox" FieldName="WAREHOUSE" Format="" maxlength="2" Width="180" EditorOptions="valueField:'WAREHOUSE',textField:'WARENAME',remoteName:'sRT100.HBWAREHOUSE',tableName:'HBWAREHOUSE',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="DROPUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢原因" Editor="textarea" FieldName="DROPREASON" Format="" maxlength="100" Width="360" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" maxlength="12" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="轉應收帳款日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="轉應收帳款人員" Editor="infocombobox" FieldName="TUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔員" Editor="infocombobox" FieldName="EUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="輸入日期" Editor="datebox" FieldName="EDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改員" Editor="infocombobox" FieldName="UUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="textarea" FieldName="MEMO" Format="" maxlength="50" Width="300" Span="2" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="" FieldName="COMQ1" RemoteMethod="False" DefaultMethod="InsDefault" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="" FieldName="LINEQ1" RemoteMethod="False" DefaultMethod="InsDefaultLineq" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>
</html>
