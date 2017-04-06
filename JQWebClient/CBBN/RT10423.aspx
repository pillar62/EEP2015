<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT10423.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID"); //用戶編號
        var PRTNO = Request.getQueryStringByName2("PRTNO"); //維修單號
        var usr = getClientInfo('_usercode');

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

        //轉領用單
        function btn1Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            try {
                var row1 = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
                var row2 = $('#JQDataGrid2').datagrid('getSelected');//取得當前主檔中選中的那個Data
            }
            catch (err)
            { alert(err); }

            if ((row1.CLOSEDAT != "" && row1.CLOSEDAT != null) || (row1.UNCLOSEDAT != "" && row1.UNCLOSEDAT != null)) {
                alert("派工單已完工結案或未完工結案，不可轉物品領用單");
                return false;
            }

            if (row1.DROPDAT != "" && row1.DROPDAT != null) {
                alert("派工單已作廢，不可轉物品領用單");
                return false;
            }

            if (row1.CDAT != "" && row1.CDAT != null) {
                alert("派工單已產生應收帳款，不返可轉物品領用單");
                return false;
            }

            var rows = $('#JQDataGrid2').datagrid('getRows')
            if (rows.length == 0 || row2.CNT == 0 || row2.CNT == null) {
                alert("派工單已無其它設備可轉物品領用單");
                return false;
            }
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT10423.cmdRT104231', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT104231" + "&parameters=" + CUSID + "," + PRTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert("已轉領用單，請點選重新整理!");
                }
            });
        }

        //轉領用單
        function btn2Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            try {
                var row1 = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
                var row2 = $('#JQDataGrid2').datagrid('getSelected');//取得當前主檔中選中的那個Data
            }
            catch (err)
            { alert(err); }

            if ((row1.CLOSEDAT != "" && row1.CLOSEDAT != null) || (row1.UNCLOSEDAT != "" && row1.UNCLOSEDAT != null)) {
                alert("派工單已完工結案或未完工結案，不可轉物品領用單");
                return false;
            }

            if (row1.DROPDAT != "" && row1.DROPDAT != null) {
                alert("派工單已作廢，不可轉物品領用單");
                return false;
            }

            if (row1.CDAT != "" && row1.CDAT != null) {
                alert("派工單已產生應收帳款，不返可轉物品領用單");
                return false;
            }

            if (row.DROPDAT != "" && row.DROPDAT != null) {
                alert("此設備已作廢，不可返轉物品領用單");
                return false;
            }

            if (row.RCVPRTNO == "" || row.RCVPRTNO == null) {
                alert("此設備尚未轉物品領用單，不可返轉");
                return false;
            }

            if (row.RCVFINISHDAT != "" && row.RCVFINISHDAT != null) {
                alert("此設備之物品領用單已經結案，不可返轉(欲返轉請先取消物品領用單結案作業)");
                return false;
            }

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT10423.cmdRT104232', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT104232" + "&parameters=" + row.RCVPRTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert("用戶派工設備返轉物品領用單作業成功，請點選重新整理!");
                }
            });
        }

        //作廢
        function btn3Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            try {
                var row1 = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
                var row2 = $('#JQDataGrid2').datagrid('getSelected');//取得當前主檔中選中的那個Data
            }
            catch (err)
            { alert(err); }

            if (row.DROPDAT != "" && row.DROPDAT != null) {
                alert("此筆設備已作廢，不可重覆作廢");
                return false;
            }

            if (row.BATCHNO != "" && row.BATCHNO != null) {
                alert("已轉應收帳款，不可作廢(欲作廢必須直接作廢派工單)");
                return false;
            }

            if ((row1.CLOSEDAT != "" && row1.CLOSEDAT != null) || (row1.UNCLOSEDAT != "" && row1.UNCLOSEDAT != null)) {
                alert("所屬派工單已結案，不可作廢設備明細");
                return false;
            }

            if (row1.DROPDAT != "" && row1.DROPDAT != null) {
                alert("派工單已作廢，不可轉物品領用單");
                return false;
            }

            if (row1.CDAT != "" && row1.CDAT != null) {
                alert("派工單已產生應收帳款，不返可轉物品領用單");
                return false;
            }


            if (row.RCVPRTNO != "" && row.RCVPRTNO != null) {
                alert("已轉物品領用單，不可作廢(欲作廢請先返轉物品領用單)");
                return false;
            }

            var ENTRYNO = row.ENTRYNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT10423.cmdRT104233', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT104233" + "&parameters=" + CUSID + "," + PRTNO + "," + ENTRYNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert("設備安裝資料作廢成功，請點選重新整理!");
                }
            });
        }

        //作廢返轉
        function btn4Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            try {
                var row1 = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data
                var row2 = $('#JQDataGrid2').datagrid('getSelected');//取得當前主檔中選中的那個Data
            }
            catch (err)
            { alert(err); }

            if (row.DROPDAT == "" || row.DROPDAT == null) {
                alert("此筆設備尚未作廢，不可作廢返轉");
                return false;
            }

            if ((row1.CLOSEDAT != "" && row1.CLOSEDAT != null) || (row1.UNCLOSEDAT != "" && row1.UNCLOSEDAT != null)) {
                alert("所屬派工單已結案，不可執行作廢返轉");
                return false;
            }

            var ENTRYNO = row.ENTRYNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT10423.cmdRT104234', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT104234" + "&parameters=" + CUSID + "," + PRTNO + "," + ENTRYNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert("設備安裝資料作廢返轉成功，請點選重新整理!");
                }
            });
        }

        function btn5Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            var ENTRYNO = row.ENTRYNO;
            parent.addTab("用戶派工單安裝設備異動資料查詢", "CBBN/RT104235.aspx?CUSID=" + CUSID + "&PRTNO=" + PRTNO + "&ENTRYNO=" + ENTRYNO);
        }

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = "PRTNO = '" + PRTNO + "' AND CUSID='"+CUSID+"'";
                $("#dataGridView").datagrid('setWhere', sWhere);
                $("#dataGridView").datagrid("selectRow", 0);
                $("#JQDataGrid1").datagrid('setWhere', sWhere);
                $("#JQDataGrid1").datagrid("selectRow", 0);                
                $("#JQDataGrid2").datagrid('setWhere', sWhere);
                $("#JQDataGrid2").datagrid("selectRow", 0);
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
                Title="用戶裝機派工設備資料維護" OnLoadSuccess="dgOnloadSuccess" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
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
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply"
                        Text="存檔" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel"
                        Text="取消"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="轉領用單" Visible="True" OnClick="btn1Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="領用單返轉" Visible="True" OnClick="btn2Click" Icon="icon-undo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="設備作廢" Visible="True" OnClick="btn3Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢返轉" Visible="True" OnClick="btn4Click" Icon="icon-undo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="異動查詢" Visible="True" OnClick="btn5Click" Icon="icon-view" />
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
        <div hidden="hidden">
        <JQTools:JQDataGrid ID="JQDataGrid1" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="View_RTLessorAVSCustSndwork" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT1042.View_RTLessorAVSCustSndwork" RowNumbers="True" Title="JQDataGrid" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
            <Columns>
                <JQTools:JQGridColumn Alignment="left" Caption="主線" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="30">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="24">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="派工日/完工日" Editor="text" FieldName="SENDWORKDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="列印人員" Editor="text" FieldName="PRTUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="預定施工員" Editor="text" FieldName="ASSIGNENGINEER" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="預定人員" Editor="text" FieldName="ASSIGNCONSIGNEE" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="20">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="實際施工員" Editor="text" FieldName="REALENGINEER" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="實際人員" Editor="text" FieldName="REALCONSIGNEE" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="20">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="完工結案日" Editor="text" FieldName="DROPDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="未完工員因" Editor="text" FieldName="DROPDESC" Frozen="False" IsNvarChar="False" MaxLength="200" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="400">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="獎金關帳日" Editor="text" FieldName="CLOSEDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="獎金計算月" Editor="text" FieldName="BONUSCLOSEYM" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="獎金計算日" Editor="text" FieldName="BONUSCLOSEDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="獎金結算員" Editor="text" FieldName="BONUSCLOSEUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="獎金會計審核日" Editor="text" FieldName="BONUSFINCHK" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="庫存結算月" Editor="text" FieldName="STOCKCLOSEYM" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="庫存結算日" Editor="text" FieldName="STOCKCLOSEDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="庫存結算員" Editor="text" FieldName="STOCKCLOSEUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="庫存會計審核日" Editor="text" FieldName="STOCKFINCHK" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="MDF位置" Editor="text" FieldName="MDF1" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="20">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="端子版位置" Editor="text" FieldName="MDF2" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="20">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="SWITCH(PNA)編號" Editor="text" FieldName="HOSTNO" Frozen="False" IsNvarChar="False" MaxLength="3" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="6">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="PORT號" Editor="text" FieldName="HOSTPORT" Frozen="False" IsNvarChar="False" MaxLength="3" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="6">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="備註說明" Editor="text" FieldName="MEMO" Frozen="False" IsNvarChar="False" MaxLength="300" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="600">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="列印日期" Editor="text" FieldName="PRTDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="未完工結案日" Editor="text" FieldName="UNCLOSEDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="DROPUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="CLOSEUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="建檔人員" Editor="text" FieldName="EUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="建檔日期" Editor="text" FieldName="EDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="UUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="12">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="異動日期" Editor="text" FieldName="UDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="BATCHNO" Editor="text" FieldName="BATCHNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="24">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="CDAT" Editor="text" FieldName="CDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="MAXENTRYNO" Editor="text" FieldName="MAXENTRYNO" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="用戶網卡MAC" Editor="text" FieldName="MAC" Frozen="False" IsNvarChar="False" MaxLength="20" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="40">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="收款日(裝機)" Editor="text" FieldName="RCVMONEYDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
            </Columns>
        </JQTools:JQDataGrid>
        <JQTools:JQDataGrid ID="JQDataGrid2" runat="server" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" AutoApply="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RT104231" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT10423.V_RT104231" RowNumbers="True" Title="JQDataGrid" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
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
