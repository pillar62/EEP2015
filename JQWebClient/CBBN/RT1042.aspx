<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1042.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
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
                var sWhere = "RTLessorAVSCustSNDWORK.CUSID='" + CUSID + "'";
                $("#dataGridMaster").datagrid('setWhere', sWhere);
                var row = $("#dataGridView").datagrid("selectRow", 0);
                $("#JQDataGrid1").datagrid('setWhere', "CUSID='" + CUSID + "'"); //過濾用戶資料
                $("#JQDataGrid2").datagrid('setWhere', "CUSID='" + CUSID + "' AND PRTNO='" + row.PRTNO + "'"); //過濾用戶資料
            }
            flag = false;
        }

        function btnIns(val) {
            var sMODE = "I";
            parent.addTab("用戶裝機派工單資料新增", "CBBN/RT10421.aspx?CUSID=" + CUSID + "&PRTNO=自動編號" + "&sMODE=" + sMODE);
        }

        function btnEdit(val) {
            var sMODE = "E";
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            parent.addTab("用戶裝機派工單資料修改", "CBBN/RT10421.aspx?CUSID=" + CUSID + "&PRTNO=" + PRTNO + "&sMODE=" + sMODE);
        }

        function btnView(val) {
            var sMODE = "V";
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            parent.addTab("用戶裝機派工單資料修改", "CBBN/RT10421.aspx?CUSID=" + CUSID + "&PRTNO=" + PRTNO + "&sMODE=" + sMODE);
        }

        //物品領用單
        function btn1Click()
        {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            parent.addTab("物品領用單資料維護", "CBBN/RT10422.aspx?PRTNO=" + PRTNO);
        }

        //列印派工單
        function btn2Click() {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            var WhereString = "";
            exportDevReport("#dataGridMaster", "sRT1042.RT1042", "RT1042", "~/CBBN/DevReportForm/RT1042RF.aspx", WhereString);
        }

        //完工結案
        function btn3Click() {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            try
            {
                var row1 = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
            }
            catch (err)
            { alert(err); }

            var PRTNO = row.PRTNO;
            if ((row1.DROPDAT != "" &&row1.DROPDAT != null) || (row1.CANCELDAT != "" && row1.CANCELDAT != null))
            {
                alert("客戶已退租或作廢");
                return false;
            }

            if (row1.RCVMONEY == 0)
            {
                alert("應收金額=0 (無法轉應收帳款)");
                return false;
            }
            
            if (row1.STRBILLINGDAT == "") {
                alert("完工結案時,開始計費日不可空白");
                return false;
            }

            if (row1.BATCHNO != "") {
                alert("己產生應收帳款");
                return false;
            }

            if (row1.FINISHDAT != "") {
                alert("此客戶已完工結案，不可重複執行");
                return false;
            }

            if (row.DROPDAT != "") {
                alert("當已作廢時，不可執行完工結案或未完工結案");
                return false;
            }
            /*
            if (row.CLOSEDAT != "" || row.UNCLOSEDAT != "") {
                alert("此裝機派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案");
                return false;
            }

            if (row.REALENGINEER == "" && row.REALCONSIGNEE == "") {
                alert("此裝機派工單完工時，必須先輸入實際裝機人員或實際裝機經銷商");
                return false;
            }
            */

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1042.cmdRT10421', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10421" + "&parameters=" + CUSID + "," + PRTNO + "," + usr + "," + row1.PERIOD + "," + row1.RCVMONEY + ","
                    + row1.PAYTYPE + "," + row1.CREDITCARDNO,
                cache: false,
                async: false,
                success: function (data) {
                    alert("已完工結案，請點選重新整理!");
                }
            });
        }

        //未完工結案
        function btn4Click() {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            try {
                var row1 = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
            }
            catch (err)
            { alert(err); }

            var PRTNO = row.PRTNO;
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
                data: "mode=method&method=" + "smRT10422" + "&parameters=" + CUSID + "," + PRTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert("未完工結案完成，請點選重新整理");
                }
            });
        }

        //結案返轉
        function btn5Click() {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            try {
                var row1 = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
            }
            catch (err)
            { alert(err); }

            var PRTNO = row.PRTNO;
            if ((row1.DROPDAT != "" && row1.DROPDAT != null) || (row1.CANCELDAT != "" && row1.CANCELDAT != null)) {
                alert("客戶已退租或作廢");
                return false;
            }

            if ((row.BONUSCLOSEYM != "" && row.BONUSCLOSEYM != null) || (row.STOCKCLOSEYM != "" && row.STOCKCLOSEYM != null)) {
                alert("此裝機派工單已月結，不可異動");
                return false;
            }

            if (row.closedat == "" && row.unclosedat == "" && row.closedat == null && row.unclosedat == null) {
                alert("此裝機派工單尚未結案，不可執行結案返轉作業");
                return false;
            }

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1042.cmdRT10423', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10423" + "&parameters=" + CUSID + "," + PRTNO + "," + usr + "," + row.batchno,
                cache: false,
                async: false,
                success: function (data) {
                    alert("結案返轉完成，請點選重新整理!");
                }
            });
        }

        //作廢
        function btn6Click() {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            try {
                var row1 = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
            }
            catch (err)
            { alert(err); }

            var PRTNO = row.PRTNO;
            if (row.DROPDAT != "" && row.DROPDAT != null)  {
                alert("此派工單已作廢，不可重覆執行作廢作業");
                return false;
            }

            if ((row.BONUSCLOSEYM != "" && row.BONUSCLOSEYM != null) || (row.STOCKCLOSEYM != "" && row.STOCKCLOSEYM != null)) {
                alert("此裝機派工單已月結，不可異動");
                return false;
            }

            if ((row.closedat != "" && row.closedat != null)|| (row.unclosedat != "" && row.unclosedat != null)) {
                alert("此派工單已完工結案，不可作廢(欲作廢請先清除裝機完工日)");
                return false;
            }

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1042.cmdRT10424', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10424" + "&parameters=" + CUSID + "," + PRTNO + "," + usr + "," + row.batchno,
                cache: false,
                async: false,
                success: function (data) {
                    alert("資料已作廢，請點選重新整理!");
                }
            });
        }

        //作廢返轉
        function btn7Click() {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            try {
                var row1 = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
            }
            catch (err)
            { alert(err); }

            var PRTNO = row.PRTNO;
            if (row.DROPDAT == "" || row.DROPDAT == null) {
                alert("此派工單尚未作廢，不可重覆執行作廢返轉作業");
                return false;
            }

            if (row.BONUSCLOSEYM != "" && row.BONUSCLOSEYM != null) {
                alert("當獎金計算年月已存在資料時表示該筆資料完工日期當月之獎金已結算,不可再作廢返轉");
                return false;
            }
            
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1042.cmdRT10425', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10425" + "&parameters=" + CUSID + "," + PRTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert("資料已作廢返轉，請點選重新整理!");
                }
            });
        }

        function btn8Click(val) {
            var sMODE = "E";
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            parent.addTab("用戶裝機派工設備資料維護", "CBBN/RT10423.aspx?CUSID=" + CUSID + "&PRTNO=" + PRTNO + "&sMODE=" + sMODE);
        }

        function btn9Click(val) {
            var sMODE = "E";
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            parent.addTab("用戶裝機派工設備資料維護", "CBBN/RT10424.aspx?CUSID=" + CUSID + "&PRTNO=" + PRTNO + "&sMODE=" + sMODE);
        }

        function mySelect()
        {
            //var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            //var PRTNO = row.PRTNO;
            var sWhere = "CUSID='" + CUSID + "'";
            $("#JQDataGrid1").datagrid('setWhere', sWhere);
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT1042.RT1042" runat="server" AutoApply="True"
                DataMember="RT1042" Pagination="True" QueryTitle="Query"
                Title="用戶裝機派工單資料維護" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Fuzzy" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="False" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" OnLoadSuccess="dgOnloadSuccess" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" EnableViewState="False" OnSelect="mySelect" ReportFileName="/DevReportForm/RT1042RF.aspx">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線" Editor="text" FieldName="comqline" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" MaxLength="12" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日期" Editor="datebox" FieldName="SENDWORKDAT" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="列印人員" Editor="text" FieldName="CUSNC" Format="" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="預定施工員" Editor="text" FieldName="Column1" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="實際施工員" Editor="text" FieldName="Column2" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案日" Editor="datebox" FieldName="closedat" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="未完工結案日" Editor="datebox" FieldName="unclosedat" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金計算月" Editor="text" FieldName="BONUSCLOSEYM" Format="yyyy/mm" Width="70" MaxLength="6" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金會計審核日" Editor="datebox" FieldName="BONUSFINCHK" Format="yyyy/mm/dd" MaxLength="0" Width="80" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="batchno" Format="" Width="120" MaxLength="12" />
                    <JQTools:JQGridColumn Alignment="left" Caption="庫存結算月" Editor="text" FieldName="STOCKCLOSEYM" Format="yyyy/mm" MaxLength="6" Width="70" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="庫存會計審核日" Editor="datebox" FieldName="STOCKFINCHK" Format="yyyy/mm/dd" MaxLength="0" Width="80" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="設備數量" Editor="numberbox" FieldName="Column3" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="轉領用單數量" Editor="numberbox" FieldName="Column4" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="已領數量" Editor="numberbox" FieldName="Column5" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="待領數量" Editor="numberbox" FieldName="Column6" Format="" Width="80" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btnIns" Text="新增" Visible="True" Icon="icon-add" ID="btnIns" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="btnEdit" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-view" ItemType="easyui-linkbutton" OnClick="btnView" Text="檢視" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="物品領用單" Visible="True" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn2Click" Text="列印" Visible="True" Icon="icon-print"/>
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn3Click" Text="完工結案" Visible="True" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn4Click" Text="未完工結案" Visible="True" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn5Click" Text="結案返轉" Visible="True" Icon="icon-undo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn6Click" Text="作廢" Visible="True" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn7Click" Text="作廢返轉" Visible="True" Icon="icon-redo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn8Click" Text="設備明細" Visible="True" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn9Click" Text="歷史異動" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
     
               </div>
        
     <div hidden="hidden">
        <JQTools:JQDataGrid ID="JQDataGrid1" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="View_RTLessorAVSCust" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT104.View_RTLessorAVSCust" RowNumbers="True" Title="JQDataGrid" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
            <Columns>
                <JQTools:JQGridColumn Alignment="left" Caption="客戶代號" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="text" FieldName="DROPDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="text" FieldName="CANCELDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="當期收款金額" Editor="text" FieldName="RCVMONEY" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="text" FieldName="STRBILLINGDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="完工日" Editor="text" FieldName="FINISHDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="可使用期數" Editor="text" FieldName="PERIOD" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="繳費方式" Editor="text" FieldName="PAYTYPE" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="卡號" Editor="text" FieldName="CREDITCARDNO" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                </JQTools:JQGridColumn>
            </Columns>
        </JQTools:JQDataGrid>
        <JQTools:JQDataGrid ID="JQDataGrid2" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="RT10421" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT1042.RT10421" RowNumbers="True" Title="JQDataGrid" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
            <Columns>
                <JQTools:JQGridColumn Alignment="left" Caption="CUSID" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="30">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="PRTNO" Editor="text" FieldName="PRTNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="24">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="CNT" Editor="text" FieldName="CNT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
            </Columns>
        </JQTools:JQDataGrid>
        </div>
    </form>
</body>
</html>
