<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1045.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID");
        var flag = true;
        var usr = getClientInfo('_usercode');

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
                $("#JQDataGrid1").datagrid('setWhere', sWhere); //處理用戶資料
            }
            flag = false;
        }

        function dgOnInsert()
        {
            var row = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.DUEDAT;
            if (ss == "" && ss == null) {
                alert("客戶主檔無到期日，無法建立續約資料。");
                return false;
            }
            var ss = row.DROPDAT;
            alert(ss);
            if (ss != "" && ss != null) {
                alert("客戶已退租，不可建立續約資料，請改用復機作業。");
                return false;
            }
        }

        //轉拆機單
        function btn1Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            var entryno = row.ENTRYNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1045.cmdRT1045A', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT1045A" + "&parameters=" + CUSID + "," + entryno + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }        

        function onapplycheck(val)
        {
            var row = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.DUEDAT;
            var s2 = $("#JQDataForm1STRBILLINGDAT").datebox("getValue");
            alert(s2);
            if (s2 < ss) {
                alert("開始計費日不可小於客戶主檔到期日。");
                return false;
            }

            //判斷如果 付款方式為信用卡 就要控制卡別等欄位不可空白
            if ($("#dataFormMasterPAYTYPE").refval('getValue') == "01") {
                if ($("#dataFormMasterCREDITCARDTYPE").refval('getValue') == "") {
                    alert("請輸入信用卡種類!")
                    $("#dataFormMasterCREDITCARDTYPE").data("inforefval").refval.find("input.refval-text").focus();
                    return false;
                }
                if ($("#dataFormMasterCREDITBANK").refval('getValue') == "") {
                    alert("請輸入信用卡發卡銀行!")
                    $("#dataFormMasterCREDITBANK").data("inforefval").val.find("input.refval-text").focus();
                    return false;
                }
                if ($("#dataFormMasterCREDITCARDNO").val() == "") {
                    alert("請輸入信用卡卡號!")
                    $("#dataFormMasterCREDITCARDNO").focus();
                    return false;
                }
                if ($("#dataFormMasterCREDITNAME").val() == "") {
                    alert("請輸入持卡人姓名!")
                    $("#dataFormMasterCREDITNAME").focus();
                    return false;
                }
                if ($("#dataFormMasterCREDITDUEM").val() == "") {
                    alert("信用卡有效(月)!")
                    $("#dataFormMasterCREDITDUEM").focus();
                    return false;
                }
                if ($("#dataFormMasterCREDITDUEY").val() == "") {
                    alert("信用卡有效(年)!")
                    $("#dataFormMasterCREDITDUEY").focus();
                    return false;
                }
            }

        }

        //派工單查詢
        function btn2Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            var s2 = row.ENTRYNO;
            parent.addTab("用戶退租拆機派工單資料維護", "CBBN/RT10451.aspx?CUSID=" + ss + "&ENTRYNO=" + s2);
        }

        //退租結案
        function btn3Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            //var PRTNO = row.PRTNO;
            var ENTRYNO = row.ENTRYNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1045.cmdRT10456', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10456" + "&parameters=" + CUSID + "," + ENTRYNO + "," +  usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        //結案返轉
        function btn4Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
//            var PRTNO = row.PRTNO;
            var ENTRYNO = row.ENTRYNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1045.cmdRT10457', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10457" + "&parameters=" + CUSID + "," + ENTRYNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        //作廢
        function btn5Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
//            var PRTNO = row.PRTNO;
            var ENTRYNO = row.ENTRYNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1045.cmdRT10458', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10458" + "&parameters=" + CUSID + "," + ENTRYNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        //作廢返轉
        function btn6Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            var ENTRYNO = row.ENTRYNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1045.cmdRT10459', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10459" + "&parameters=" + CUSID + "," + ENTRYNO + "," + PRTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        //派工單查詢
        function btn7Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            var s2 = row.ENTRYNO;
            parent.addTab("用戶退租拆機派工單異動資料查詢", "CBBN/RT10459.aspx?CUSID=" + ss + "&ENTRYNO=" + s2);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT1045.RTLessorAVSCustDrop" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustDrop" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="用戶退租作業" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" OnLoadSuccess="dgOnloadSuccess" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="90" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="退租項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Visible="true" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租種類" Editor="inforefval" FieldName="DROPKIND" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'N7'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="預定退租日" Editor="datebox" FieldName="ENDDAT" Format="yyyy/mm/dd" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租結案日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案人員" Editor="infocombobox" FieldName="FUSR" Format="" MaxLength="6" Visible="true" Width="80" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="拆機工單" Editor="text" FieldName="SNDPRTNO" Format="" MaxLength="12" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轉拆機單日期" Editor="datebox" FieldName="SNDWORK" Format="yyyy/mm/dd" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="拆機結案日" Editor="datebox" FieldName="SNDWORKCLOSE" Format="yyyy/mm/dd" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轉應付帳款日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="應付帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="12" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" Format="" Visible="true" Width="120" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" MaxLength="6" />
                    <JQTools:JQGridColumn Alignment="left" Caption="MEMO" Editor="text" FieldName="MEMO" Format="" MaxLength="500" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="EUSR" Editor="text" FieldName="EUSR" Format="" Visible="False" Width="120" MaxLength="6" />
                    <JQTools:JQGridColumn Alignment="left" Caption="EDAT" Editor="datebox" FieldName="EDAT" Format="" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="UUSR" Editor="text" FieldName="UUSR" Format="" MaxLength="6" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="UDAT" Editor="datebox" FieldName="UDAT" Format="" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="SNDUSR" Editor="text" FieldName="SNDUSR" Format="" MaxLength="6" Visible="False" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="TUSR" Editor="text" FieldName="TUSR" Format="" MaxLength="6" Visible="False" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="False"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="轉拆機單" Visible="False" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn2Click" Text="派工單查詢" Visible="False" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn3Click" Text="退租結案" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn4Click" Text="結案返轉" Visible="True" Icon="icon-undo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn5Click" Text="作　　廢" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn6Click" Text="作廢返轉" Visible="True" Icon="icon-undo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn7Click" Text="歷史異動" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶退租作業">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCustDrop" HorizontalColumnsCount="2" RemoteName="sRT1045.RTLessorAVSCustDrop" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶編號" Editor="infocombobox" FieldName="CUSID" Format="" maxlength="15" Width="180" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="退租種類" Editor="inforefval" FieldName="DROPKIND" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'N7'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="退租申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="預定退租日" Editor="datebox" FieldName="ENDDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="退租結案日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案人員" Editor="infocombobox" FieldName="FUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="拆機工單" Editor="text" FieldName="SNDPRTNO" Format="" maxlength="12" Width="180" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="拆機結案日" Editor="datebox" FieldName="SNDWORKCLOSE" Format="yyyy/mm/dd" Width="180" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔人員" Editor="infocombobox" FieldName="EUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日期" Editor="datebox" FieldName="EDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" EditorOptions="dateFormat:'datetime',showTimeSpinner:true,showSeconds:true,editable:true" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動人員" Editor="infocombobox" FieldName="UUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動日期" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" EditorOptions="dateFormat:'datetime',showTimeSpinner:true,showSeconds:true,editable:true" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="textarea" FieldName="MEMO" Format="" maxlength="500" Width="400" EditorOptions="height:60" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="SNDWORK" Editor="datebox" FieldName="SNDWORK" Format="" Width="180" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="SNDUSR" Editor="text" FieldName="SNDUSR" Format="" maxlength="6" Width="180" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="TARDAT" Editor="datebox" FieldName="TARDAT" Format="" Width="180" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="BATCHNO" Editor="text" FieldName="BATCHNO" Format="" maxlength="12" Width="180" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="TUSR" Editor="text" FieldName="TUSR" Format="" maxlength="6" Width="180" Visible="False" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQAutoSeq ID="JQAutoSeq1" runat="server" BindingObjectID="dataFormMaster" FieldName="ENTRYNO" />
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="CUSID" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" FieldName="EUSR" RemoteMethod="True" DefaultMethod="" DefaultValue="_usercode" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today2" FieldName="EDAT" RemoteMethod="True" />
                        <JQTools:JQDefaultColumn CarryOn="False" FieldName="UUSR" RemoteMethod="True" DefaultValue="_usercode" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today2" FieldName="UDAT" RemoteMethod="True" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
        <div hidden="hidden">
        </div>
    </form>
</body>
<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>
</html>
