<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1043.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID");
        var COMTYPE = Request.getQueryStringByName2("COMTYPE");
        var DUEDAT = Request.getQueryStringByName2("DUEDAT");
        var SSTRDT = Request.getQueryStringByName2("DUEDAT");
        
        var flag = true;
        var usr = getClientInfo('_usercode');

        function InsDefault() {
            if (CUSID != "") {
                
                var myDate = new Date(DUEDAT);
                myDate.setDate(myDate.getDate() + 1);
                var sDT = myDate.getFullYear()+"-";
                
                if (myDate.getMonth() + 1 > 9) {
                    var month = myDate.getMonth() + 1;
                    
                    sDT = sDT + month;
                }
                else {
                    var month = myDate.getMonth() + 1;
                    month = "0" + month;
                    
                    sDT = sDT + month;
                }
                
                if (myDate.getDate() < 10) {
                    sDT = sDT + "-0" + myDate.getDate();
                }
                else {
                    sDT = sDT + "-" + myDate.getDate();
                }
                    
                SSTRDT = sDT;
                //$("#dataFormMasterSTRBILLINGDAT").datebox('setValue', sDT);
                //$("#dataFormMasterADJUSTDAY").val("0");
                $("#dataFormMasterCOMTYPE").val(COMTYPE);
                return CUSID;
            }
        }

        function InsDefaultDUE() {
            
            if (DUEDAT != "") {
                return DUEDAT;
            }
        }

        function InsDefaultSTRDT() {

            if (SSTRDT != "") {
                return SSTRDT;
            }
        }

        function InsDefaultCOMTYPE() {

            if (COMTYPE != "") {
                return COMTYPE;
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

        function dgMasterLoadSuccess()
        {
            $("#dataFormMasterCUSID").focus();
            $("#dataFormMasterCOMTYPE").val(COMTYPE);
        }

        function dgOnInsert()
        {
            var row = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
            
            if (row.length > 0) {
                var ss = row.DUEDAT;
                if (ss == "" && ss == null) {
                    alert("客戶主檔無到期日，無法建立續約資料。");
                    return false;
                }
                ss = row.DROPDAT;

                if (ss != "" && ss != null) {
                    alert("客戶已退租，不可建立續約資料，請改用復機作業。");
                    return false;
                }

                $("#dataFormMasterCUSID").focus();
                $("#dataFormMasterCASEKIND").focus();
            }
        }

        function btn1Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            var s2 = row.ENTRYNO;
            parent.addTab("用戶續約收款派工單資料維護", "CBBN/RT10431.aspx?CUSID=" + ss + "&ENTRYNO=" + s2);
        }        

        function onapplycheck(val)
        {
            var row = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.DUEDAT;
            var s2 = $("#dataFormMasterSTRBILLINGDAT").datebox("getValue");

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

        //轉應收結案
        function btn2Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            try {
                var row1 = $('#JQDataGrid1').datagrid('getSelected');//用戶
                var sWhere = "CUSID='" + CUSID + "' AND ENTRYNO=" + row.ENTRYNO + " and dropdat is null and unclosedat is null and closedat is null ";

                $("#JQDataGrid3").datagrid('setWhere', sWhere);
                var row3 = $('#JQDataGrid3').datagrid('getSelected');//派工
            }
            catch (err)
            { alert(err); }

            var PRTNO = row.PRTNO;
            var entryno = row.ENTRYNO;

            if (row.CANCELDAT != "" && row.CANCELDAT != null) {
                alert("續約資料已作廢時，不可執行轉應收帳款作業");
                return false;
            }

            if (row1.CANCELDAT != "" && row1.CANCELDAT != null) {
                alert("客戶資料已作廢，必須作廢續約資料。");
                return false;
            }

            if (row1.DROPDAT != "" && row1.DROPDAT != null) {
                alert("客戶資料已退租，必須作廢續約資料。");
                return false;
            }

            if (row.AMT == 0) {
                alert("應收金額為0者，不可產生應收帳款");
                return false;
            }

            /*
            if (row.PAYTYPE == "02") {
                alert("繳費方式為現金付款時，必須由收款派工單產生應收帳款");
                return false;
            }
            */

            if (row1.STRBILLINGDAT == "" && row1.STRBILLINGDAT == null) {
                alert("開始計費日空白時不可轉應收結案作業。");
                return false;
            }

            if (row1.BATCHNO != "") {
                alert("己產生應收帳款");
                return false;
            }

            if (row.DROPDAT != "" && row.DROPDAT != null) {
                alert("當已作廢時，不可執行完工結案或未完工結案");
                return false;
            }

            var rows = $('#JQDataGrid3').datagrid('getRows');

            if (rows.length != 0) {
                alert("此續約資料已存在收款派工單，必須由派工單進行結案作業。");
                return false;
            }
            
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1043.cmdRT10436', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10436" + "&parameters=" + CUSID + "," + entryno + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert("用戶續約轉應收帳款成功，請點選重新整理!" + data);
                }
            });
        }

        //返轉應收結案
        function btn3Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            try {
                var row1 = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
            }
            catch (err)
            { alert(err); }

            var PRTNO = row.PRTNO;
            var ENTRYNO = row.ENTRYNO;
            if ((row1.DROPDAT != "" && row1.DROPDAT != null) || (row1.CANCELDAT != "" && row1.CANCELDAT != null)) {
                alert("客戶已退租或作廢");
                return false;
            }

            if ((row.CLOSEDAT != "" && row.CLOSEDAT != null) || (row.UNCLOSEDAT != "" && row.UNCLOSEDAT != null)) {
                alert("此裝機派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案");
                return false;
            }

            if (row.BONUSCLOSEYM != "" || row.STOCKCLOSEYM != "") {
                alert("此裝機派工單已月結，不可異動");
                return false;
            }

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1042.cmdRT10422', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10422" + "&parameters=" + CUSID + "," + ENTRYNO + "," + PRTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert("未完工結案完成，請點選重新整理");
                }
            });
        }

        //應收應付
        function btn4Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var CUSID = row.CUSID;
            var BATCHNO = row.BATCHNO;
            parent.addTab("用戶應收應付帳款查詢", "CBBN/RT10411.aspx?CUSID=" + CUSID + "&BATCHNO=" + BATCHNO);
        }

        //作廢
        function btn5Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            try {
                var row1 = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
            }
            catch (err)
            { alert(err); }

            var PRTNO = row.PRTNO;
            var ENTRYNO = row.ENTRYNO;

            if (row.DROPDAT != "" && row.DROPDAT != null) {
                alert("此派工單已作廢，不可重覆執行作廢作業");
                return false;
            }

            if ((row.BONUSCLOSEYM != "" && row.BONUSCLOSEYM != null) || (row.STOCKCLOSEYM != "" && row.STOCKCLOSEYM != null)) {
                alert("此裝機派工單已月結，不可異動");
                return false;
            }

            if ((row.closedat != "" && row.closedat != null) || (row.unclosedat != "" && row.unclosedat != null)) {
                alert("此派工單已完工結案，不可作廢(欲作廢請先清除裝機完工日)");
                return false;
            }

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1043.cmdRT10438', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10438" + "&parameters=" + CUSID + "," + ENTRYNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert("資料已作廢，請點選重新整理!");
                }
            });
        }

        //作廢返轉
        function btn6Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data

            var PRTNO = row.PRTNO;
            var ENTRYNO = row.ENTRYNO;

            if (row.CANCELDAT == "" || row.CANCELDAT == null) {
                alert("此派工單尚未作廢，不可重覆執行作廢返轉作業");
                return false;
            }

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1043.cmdRT10439', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10439" + "&parameters=" + CUSID + "," + ENTRYNO + "," + PRTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert("資料已作廢返轉，請點選重新整理!");
                }
            });
        }

        function btnReloadClick() {
            //$("#dataGridView").datagrid("setWhere", "");
            $('#dataGridView').datagrid('reload');
        }
        //
        function btn7Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var CUSID = row.CUSID;
            var BATCHNO = row.BATCHNO;
            parent.addTab("用戶應收應付帳款查詢", "CBBN/RT10433.aspx?CUSID=" + CUSID + "&BATCHNO=" + BATCHNO);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT1043.RTLessorAVSCustCont" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustCont" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="用戶續約作業" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" OnLoadSuccess="dgOnloadSuccess" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False" OnInsert="dgOnInsert">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶名稱" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Visible="true" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="續約申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="STRBILLINGDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費週期" Editor="inforefval" FieldName="PAYCYCLE" Format="" Visible="true" Width="90" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" MaxLength="2" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費方式" Editor="inforefval" FieldName="PAYTYPE" Format="" MaxLength="2" Visible="true" Width="90" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="right" Caption="可用期別" Editor="numberbox" FieldName="PERIOD" Format="" Visible="true" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="第二戶" Editor="text" FieldName="SECONDCASE" Format="" MaxLength="1" Visible="true" Width="50" />
                    <JQTools:JQGridColumn Alignment="right" Caption="應收金額" Editor="numberbox" FieldName="AMT" Format="" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="實收金額" Editor="numberbox" FieldName="REALAMT" Format="" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轉應收帳款日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="12" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案日期" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="90" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="False"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="收款派工" Visible="False" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn2Click" Text="轉應收結案" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn3Click" Text="返轉應收結案" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn4Click" Text="應收應付" Visible="True" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn5Click" Text="作　廢" Visible="True" Icon="icon-cut" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn6Click" Text="作廢返轉" Visible="True" Icon="icon-redo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn7Click" Text="歷史異動" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btnReloadClick" Text="資料更新" Visible="True" Icon="icon-reload" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶續約作業" Width="927px">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCustCont" HorizontalColumnsCount="2" RemoteName="sRT1043.RTLessorAVSCustCont" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" OnApply="onapplycheck" OnLoadSuccess="dgMasterLoadSuccess" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶序號" Editor="inforefval" FieldName="CUSID" Format="" maxlength="15" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',columns:[],columnMatches:[],whereItems:[],valueField:'CUSID',textField:'CUSNC',valueFieldCaption:'CUSID',textFieldCaption:'CUSNC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" ReadOnly="False" OnBlur="" />
                        <JQTools:JQFormColumn Alignment="left" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="續約申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第二戶(含)以上" Editor="infocombobox" FieldName="SECONDCASE" Format="" maxlength="1" Width="180" EditorOptions="items:[{value:'Y',text:'是',selected:'false'},{value:'N',text:'否',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="資費" Editor="inforefval" FieldName="CASEKIND" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'},{field:'PARM1',value:'client[InsDefaultCOMTYPE]'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費週期" Editor="inforefval" FieldName="PAYCYCLE" Format="" Visible="true" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,remoteName:'sRT100.cmdRTBillCharge',tableName:'cmdRTBillCharge',columns:[],columnMatches:[{field:'PERIOD',value:'PERIOD'},{field:'AMT',value:'AMT'}],whereItems:[{field:'PARM1',value:'client[InsDefaultCOMTYPE]'},{field:'CASEKIND',value:'row[CASEKIND]'}],valueField:'PAYCYCLE',textField:'MEMO',valueFieldCaption:'代碼',textFieldCaption:'備註',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" MaxLength="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費方式" Editor="inforefval" FieldName="PAYTYPE" Format="" MaxLength="2" Visible="true" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收金額" Editor="numberbox" FieldName="AMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="可使用期數" Editor="text" FieldName="PERIOD" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="收款日期" Editor="datebox" FieldName="RCVMONEYDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡種類" Editor="inforefval" FieldName="CREDITCARDTYPE" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M6'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發卡銀行" Editor="inforefval" FieldName="CREDITBANK" Format="" maxlength="3" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTBank',tableName:'RTBank',columns:[],columnMatches:[],whereItems:[{field:'CREDITCARD',value:'Y'}],valueField:'HEADNO',textField:'HEADNC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:true,capsLock:'none',fixTextbox:'false'" OnBlur="請用挑選指定" />
                        <JQTools:JQFormColumn Alignment="left" Caption="卡號" Editor="text" FieldName="CREDITCARDNO" Format="" maxlength="16" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="持卡人姓名" Editor="text" FieldName="CREDITNAME" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效(月)" Editor="text" FieldName="CREDITDUEM" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效(年)" Editor="text" FieldName="CREDITDUEY" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="COMTYPE" Editor="text" FieldName="COMTYPE" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="False" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="STRBILLINGDAT" Format="yyyy/mm/dd" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="調整日數" Editor="text" FieldName="ADJUSTDAY" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註說明" Editor="textarea" EditorOptions="height:70" FieldName="MEMO" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="2" Visible="True" Width="500" />
                    </Columns>
                </JQTools:JQDataForm>
                <br />
                用戶申請、異動及施工進度狀態<br /> &nbsp;
                <JQTools:JQDataForm ID="JQDataForm1" runat="server" ChainDataFormID="dataFormMaster" DataMember="RTLessorAVSCustCont" RemoteName="sRT1043.RTLessorAVSCustCont" HorizontalColumnsCount="2" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" OnLoadSuccess="dgMasterLoadSuccess">
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="結案日期" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款產生日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" Width="180" MaxLength="12" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="帳款產生人員" Editor="infocombobox" FieldName="TUSR" Format="" maxlength="6" Width="90" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" maxlength="0" Width="90" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="CANCELUSR" Format="" Width="90" MaxLength="6" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日期" Editor="datebox" FieldName="EDAT" Format="yyyy/mm/dd" maxlength="0" Width="90" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔員" Editor="infocombobox" FieldName="EUSR" Format="" Width="180" ReadOnly="True" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" MaxLength="6" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQAutoSeq ID="JQAutoSeq1" runat="server" BindingObjectID="dataFormMaster" FieldName="ENTRYNO" />
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="CUSID" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today" FieldName="APPLYDAT" RemoteMethod="True" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="N" FieldName="SECONDCASE" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="0" FieldName="ADJUSTDAY" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefaultSTRDT" FieldName="STRBILLINGDAT" RemoteMethod="False" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQValidateColumn CheckNull="True" FieldName="CUSID" RemoteMethod="True" ValidateType="None" />
                        <JQTools:JQValidateColumn CheckNull="True" FieldName="CASEKIND" RemoteMethod="False" ValidateType="None" />
                    </Columns>
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
        <div hidden="hidden">
            <JQTools:JQDataGrid ID="JQDataGrid1" runat="server" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="False" AutoApply="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="View_RTLessorAVSCust" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT104.View_RTLessorAVSCust" RowNumbers="True" Title="JQDataGrid" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶代號" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="text" FieldName="DROPDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日" Editor="text" FieldName="DUEDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                </Columns>
            </JQTools:JQDataGrid>
            <JQTools:JQDataGrid ID="JQDataGrid3" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="False" AutoApply="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="View_RTLessorAVSCustContSndwork" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT1043.View_RTLessorAVSCustContSndwork" RowNumbers="True" Title="JQDataGrid" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="CUSID" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="30">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="ENTRYNO" Editor="text" FieldName="ENTRYNO" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="PRTNO" Editor="text" FieldName="PRTNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="24">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="SENDWORKDAT" Editor="text" FieldName="SENDWORKDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="PRTUSR" Editor="text" FieldName="PRTUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="ASSIGNENGINEER" Editor="text" FieldName="ASSIGNENGINEER" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="ASSIGNCONSIGNEE" Editor="text" FieldName="ASSIGNCONSIGNEE" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="20">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="REALENGINEER" Editor="text" FieldName="REALENGINEER" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="REALCONSIGNEE" Editor="text" FieldName="REALCONSIGNEE" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="20">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="DROPDAT" Editor="text" FieldName="DROPDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="DROPDESC" Editor="text" FieldName="DROPDESC" Frozen="False" IsNvarChar="False" MaxLength="200" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="400">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="CLOSEDAT" Editor="text" FieldName="CLOSEDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="BONUSCLOSEYM" Editor="text" FieldName="BONUSCLOSEYM" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="BONUSCLOSEDAT" Editor="text" FieldName="BONUSCLOSEDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="BONUSCLOSEUSR" Editor="text" FieldName="BONUSCLOSEUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="BONUSFINCHK" Editor="text" FieldName="BONUSFINCHK" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="STOCKCLOSEYM" Editor="text" FieldName="STOCKCLOSEYM" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="STOCKCLOSEDAT" Editor="text" FieldName="STOCKCLOSEDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="STOCKCLOSEUSR" Editor="text" FieldName="STOCKCLOSEUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="STOCKFINCHK" Editor="text" FieldName="STOCKFINCHK" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="MDF1" Editor="text" FieldName="MDF1" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="20">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="MDF2" Editor="text" FieldName="MDF2" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="20">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="HOSTNO" Editor="text" FieldName="HOSTNO" Frozen="False" IsNvarChar="False" MaxLength="3" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="6">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="HOSTPORT" Editor="text" FieldName="HOSTPORT" Frozen="False" IsNvarChar="False" MaxLength="3" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="6">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="MEMO" Editor="text" FieldName="MEMO" Frozen="False" IsNvarChar="False" MaxLength="300" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="600">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="PRTDAT" Editor="text" FieldName="PRTDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="UNCLOSEDAT" Editor="text" FieldName="UNCLOSEDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="DROPUSR" Editor="text" FieldName="DROPUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="CLOSEUSR" Editor="text" FieldName="CLOSEUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="EUSR" Editor="text" FieldName="EUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="EDAT" Editor="text" FieldName="EDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="UUSR" Editor="text" FieldName="UUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="UDAT" Editor="text" FieldName="UDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="BATCHNO" Editor="text" FieldName="BATCHNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="24">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="CDAT" Editor="text" FieldName="CDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="MAXENTRYNO" Editor="text" FieldName="MAXENTRYNO" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                </Columns>
            </JQTools:JQDataGrid>
        </div>
    </form>
</body>
<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>
</html>
