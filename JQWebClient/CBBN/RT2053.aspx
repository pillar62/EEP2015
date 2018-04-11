<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT2053.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var caseno = Request.getQueryStringByName2("caseno");
        var flag = true;
        var usr = getClientInfo('_usercode');

        function InsDefault() {
            if (caseno != "") {
                return caseno;
            }
        }

        function dgOnloadSuccess()
        {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = "caseno='" + caseno + "'";
                $("#dataGridView").datagrid('setWhere', sWhere);                
            }
            flag = false;
        }

        function btnEditClick(val) {
            openForm('#JQDialog1', $('#dataGridView').datagrid('getSelected'), "updated", 'Dialog');
        }

        function btnDeleteClick(val) {
        }

        function btnViewClick(val) {
            openForm('#JQDialog1', $('#dataGridView').datagrid('getSelected'), "viewed", 'Dialog');
        }

        function btn1Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var caseno = row.CASENO;
            var ENTRYNO = row.ENTRYNO;

            if (confirm("是否作廢？")) {
                $.ajax({
                    type: "POST",
                    url: '../handler/jqDataHandle.ashx?RemoteName=sRT205.cmd', //連接的Server端，command
                    //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                    data: "mode=method&method=" + "smRT20531" + "&parameters=" + caseno + "," + ENTRYNO + "," +  usr,
                    cache: false,
                    async: false,
                    success: function (data) {
                        alert(data);
                        $('#dataGridView').datagrid('reload');
                    }
                });
            }
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT205.RTFaqAdd" runat="server" AutoApply="True"
                DataMember="RTFaqAdd" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="客訴追件維護" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客訴單號+聯絡人" Editor="inforefval" FieldName="CASENO" Format="" MaxLength="10" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT205.View_RTFaqM',tableName:'View_RTFaqM',columns:[{field:'CASENO',title:'客訴單號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'FAQMAN',title:'報修聯絡人',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}],columnMatches:[],whereItems:[],valueField:'CASENO',textField:'FAQMAN',valueFieldCaption:'CASENO',textFieldCaption:'單號+聯絡人',cacheRelationText:false,checkData:false,showValueAndText:true,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" Format="" Visible="False" Width="30" />
                    <JQTools:JQGridColumn Alignment="left" Caption="進出線" Editor="inforefval" FieldName="IOBOUND" Format="" MaxLength="1" Visible="true" Width="50" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" MaxLength="1600" Visible="true" Width="200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="追件人員" Editor="infocombobox" FieldName="ADDUSR" Format="" MaxLength="6" Visible="true" Width="80" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="追件時間" Editor="datebox" FieldName="ADDDAT" Format="yyyy/mm/dd HH:MM:SS" Visible="true" Width="90" EditorOptions="dateFormat:'datetime',showTimeSpinner:true,showSeconds:true,editable:true" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動人員" Editor="infocombobox" FieldName="UUSR" Format="" MaxLength="6" Visible="true" Width="80" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動時間" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd HH:MM:SS" Visible="true" Width="90" EditorOptions="dateFormat:'datetime',showTimeSpinner:true,showSeconds:true,editable:true" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" Format="" MaxLength="6" Visible="true" Width="80" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢時間" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd HH:MM:SS" Visible="true" Width="90" EditorOptions="dateFormat:'datetime',showTimeSpinner:true,showSeconds:true,editable:true" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="True"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-cut" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="作廢" Visible="True" />
                </TooItems>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="客訴追件維護">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTFaqAdd" HorizontalColumnsCount="2" RemoteName="sRT205.RTFaqAdd" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="客訴單號" Editor="text" FieldName="CASENO" Format="" maxlength="10" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="180" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="進出線" Editor="inforefval" FieldName="IOBOUND" Format="" maxlength="1" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="textarea" FieldName="MEMO" Format="" maxlength="1600" Width="400" EditorOptions="height:60" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="追件人員" Editor="infocombobox" FieldName="ADDUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="追件時間" Editor="datebox" FieldName="ADDDAT" Format="yyyy/mm/dd HH:MM:SS" Width="180" ReadOnly="True" EditorOptions="dateFormat:'datetime',showTimeSpinner:true,showSeconds:true,editable:true" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動人員" Editor="infocombobox" FieldName="UUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動時間" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd HH:MM:SS" Width="180" ReadOnly="True" EditorOptions="dateFormat:'datetime',showTimeSpinner:true,showSeconds:true,editable:true" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢時間" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd HH:MM:SS" Width="180" ReadOnly="True" EditorOptions="dateFormat:'datetime',showTimeSpinner:true,showSeconds:true,editable:true" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="CASENO" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_usercode" FieldName="ADDUSR" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today2" FieldName="ADDDAT" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_usercode" FieldName="UUSR" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today2" FieldName="UDAT" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="9" FieldName="ENTRYNO" RemoteMethod="True" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
        <p>
            &nbsp;</p>
    </form>
</body>
<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>
</html>
