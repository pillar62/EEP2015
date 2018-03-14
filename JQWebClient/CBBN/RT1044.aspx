<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1044.aspx.cs" Inherits="Template_JQuerySingle1" %>

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

        function btn1Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            var s2 = row.ENTRYNO;
            parent.addTab("用戶復機收款派工單資料維護", "CBBN/RT10441.aspx?CUSID=" + ss + "&ENTRYNO=" + s2);
        }

        function onapplycheck(val) {
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

        //轉應收結案
        function btn2Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            try {
                var row1 = $('#JQDataGrid1').datagrid('getSelected');//用戶
                var sWhere = "CUSID='" + CUSID + "' AND ENTRYNO=" + row.ENTRYNO + " and dropdat is null and unclosedat is null and closedat is null ";
                alert(sWhere);
                $("#JQDataGrid3").datagrid('setWhere', sWhere);
                var row3 = $('#JQDataGrid3').datagrid('getSelected');//派工
            }
            catch (err)
            { alert(err); }

            var PRTNO = row.PRTNO;
            var entryno = row.ENTRYNO;

            if (row.CANCELDAT != "" && row.CANCELDAT != null) {
                alert("復機資料已作廢時，不可執行轉應收帳款作業");
                return false;
            }

            if (row1.CANCELDAT != "" && row1.CANCELDAT != null) {
                alert("客戶資料已作廢，必須作廢復機資料。");
                return false;
            }

            if (row1.DROPDAT != "" && row1.DROPDAT != null) {
                alert("客戶資料已退租，必須作廢復機資料。");
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
            }*/

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
                alert("此復機資料已存在收款派工單，必須由派工單進行結案作業。");
                return false;
            }

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1044.cmdRT10446', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10446" + "&parameters=" + CUSID + "," + entryno + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert("用戶復機轉應收帳款成功，請點選重新整理!" + data);
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
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1044.cmdRT10442', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10442" + "&parameters=" + CUSID + "," + ENTRYNO + "," + PRTNO + "," + usr,
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
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1044.cmdRT10448', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10448" + "&parameters=" + CUSID + "," + ENTRYNO + "," + usr,
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
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1044.cmdRT10449', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10449" + "&parameters=" + CUSID + "," + ENTRYNO + "," + PRTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert("資料已作廢返轉，請點選重新整理!");
                }
            });
        }

        //歷史異動
        function btn7Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var CUSID = row.CUSID;
            var BATCHNO = row.BATCHNO;
            parent.addTab("用戶復機異動資料查詢", "CBBN/RT10447.aspx?CUSID=" + CUSID + "&BATCHNO=" + BATCHNO);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT1044.RTLessorAVSCustReturn" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustReturn" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="用戶復機作業" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="復機申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="STRBILLINGDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費週期" Editor="inforefval" FieldName="PAYCYCLE" Format="" Visible="true" Width="120" MaxLength="2" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費方式" Editor="inforefval" FieldName="PAYTYPE" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="right" Caption="可用期別" Editor="numberbox" FieldName="PERIOD" Format="" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="right" Caption="應收金額" Editor="numberbox" FieldName="AMT" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="實收金額" Editor="numberbox" FieldName="REALAMT" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轉應收帳款日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="12" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="90" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="True"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="收款派工" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn2Click" Text="轉應收結案" Visible="True" Icon="icon-save" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn3Click" Text="返轉應收結案" Visible="True" Icon="icon-undo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn4Click" Text="應收應付" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn5Click" Text="作　廢" Visible="True" Icon="icon-cut" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn6Click" Text="作廢返轉" Visible="True" Icon="icon-redo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn7Click" Text="歷史異動" Visible="True" Icon="icon-view" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶復機作業" Width="786px">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCustReturn" HorizontalColumnsCount="2" RemoteName="sRT1044.RTLessorAVSCustReturn" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="180" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="復機申請日" Editor="datebox" FieldName="APPLYDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收金額" Editor="numberbox" FieldName="AMT" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案類型" Editor="inforefval" FieldName="CASEKIND" Format="" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" MaxLength="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="復機費" Editor="numberbox" FieldName="RETURNMONEY" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費週期" Editor="inforefval" FieldName="PAYCYCLE" Format="" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" MaxLength="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費方式" Editor="inforefval" FieldName="PAYTYPE" Format="" Visible="true" Width="180" MaxLength="2" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡種類" Editor="inforefval" FieldName="CREDITCARDTYPE" Format="" MaxLength="2" Visible="true" Width="180" EditorOptions="title:'查詢',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M6'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發卡銀行" Editor="inforefval" FieldName="CREDITBANK" Format="" maxlength="3" Width="180" EditorOptions="title:'查詢',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTBank',tableName:'RTBank',columns:[],columnMatches:[],whereItems:[{field:'CREDITCARD',value:'Y'}],valueField:'HEADNO',textField:'HEADNC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡卡號" Editor="text" FieldName="CREDITCARDNO" Format="" maxlength="16" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="持卡人姓名" Editor="text" FieldName="CREDITNAME" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效期限(月)" Editor="text" FieldName="CREDITDUEM" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效期限(年)" Editor="text" FieldName="CREDITDUEY" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="收款日期(復機)" Editor="datebox" FieldName="RCVMONEYDAT" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日期" Editor="datebox" FieldName="EDAT" Format="" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔人員" Editor="text" FieldName="EUSR" Format="" Width="180" ReadOnly="True" MaxLength="6" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動日期" Editor="datebox" FieldName="UDAT" Format="" maxlength="0" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="UUSR" Format="" Width="180" ReadOnly="True" MaxLength="6" />
                        <JQTools:JQFormColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="STRBILLINGDAT" Format="" maxlength="0" Width="180" ReadOnly="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案日期" Editor="datebox" FieldName="FINISHDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="可用期數" Editor="numberbox" FieldName="PERIOD" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="調整天數" Editor="numberbox" FieldName="ADJUSTDAY" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款產生日" Editor="datebox" FieldName="TARDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" Width="180" MaxLength="12" />
                        <JQTools:JQFormColumn Alignment="left" Caption="帳款產生人員" Editor="text" FieldName="TUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="CANCELDAT" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="CANCELUSR" Format="" Width="180" MaxLength="6" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註說明" Editor="textarea" FieldName="MEMO" Format="" maxlength="500" Width="400" EditorOptions="height:80" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="REALAMT" Editor="numberbox" FieldName="REALAMT" Format="" maxlength="0" Width="180" Span="1" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="MAXENTRYNO" Editor="numberbox" FieldName="MAXENTRYNO" Format="" Width="180" Visible="False" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQAutoSeq ID="JQAutoSeq1" runat="server" BindingObjectID="dataFormMaster" FieldName="ENTRYNO" />
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="CUSID" RemoteMethod="False" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
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
        </div>
    </form>
</body>
</html>
