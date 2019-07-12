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

        function InsEntryno() {
            var ii = 1;
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1041.cmd', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT1043" + "&parameters=" + CUSID,
                cache: false,
                async: false,
                success: function (data) {                    
                    ii = data;
                }
            });

            return ii;
        }

        function dgOnloadSuccess()
        {
            
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = "CUSID='" + CUSID + "'";
                $("#dataGridView").datagrid('setWhere', sWhere);                
                $("#JQDataGrid1").datagrid('setWhere', "CUSID='" + CUSID + "'"); //過濾用戶資料
            }
            flag = false;            
        }

        function btn3Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var CUSID = row.CUSID;
            var BATCHNO = row.BATCHNO;
            parent.addTab("用戶應收應付帳款查詢", "CBBN/RT10411.aspx?CUSID=" + CUSID + "&BATCHNO=" + BATCHNO);
        }

        //轉應收結案
        function btn1Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var BATCHNO = row.BATCHNO;
            var CUSID = row.CUSID;
            $("#JQDataGrid1").datagrid('setWhere', "CUSID='" + CUSID + "'"); //過濾用戶資料
            var row1 = $('#JQDataGrid1').datagrid('getSelected');
            
            var ENTRYNO = row.ENTRYNO;
            var usr = getClientInfo('_usercode');
            var typ = "MF";
            var rcvmoneydat = row.rcvmoneydat;
            var CANCELDAT = row1.CANCELDAT;
            var DROPDAT = row1.DROPDAT;
            var BATCHNO = row.BATCHNO;
            var SETAMT = row.SETAMT;
            var MOVEAMT = row.MOVEAMT;
            var EQUIPAMT = row.EQUIPAMT;
            var GTAMT = row.GTAMT;

            if (rcvmoneydat != null && rcvmoneydat != "") {
                alert("無收款日不可結案!!!");
                return false;
            }

            if (BATCHNO != null && BATCHNO != "") {
                alert("已轉應收結案!!");
                return false;
            }

            if (CANCELDAT != null && CANCELDAT != "") {
                alert("客戶資料已經作廢!!");
                return false;
            }

            if (DROPDAT != null && DROPDAT != "") {
                alert("客戶已經退租!!");
                return false;
            }

            if (SETAMT == 0 && MOVEAMT == 0 && EQUIPAMT == 0 && GTAMT == 0)
            {
                alert("金額不可為0!");
                return false;
            }

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1041.cmdRT1041', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT1041" + "&parameters=" + CUSID + "," + ENTRYNO + "," + usr + "," + typ + "," + BATCHNO,
                cache: false,
                async: false,
                success: function (data) {
                    alert("處理完成");
                    $('#dataGridView').datagrid('reload');
                }
            });
        }
        //轉應收結案反轉
        function btn2Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var BATCHNO = row.BATCHNO;
            var CUSID = row.CUSID;
            $("#JQDataGrid1").datagrid('setWhere', "CUSID='" + CUSID+"'"); //過濾用戶資料
            var row1 = $('#JQDataGrid1').datagrid('getSelected'); //用戶資料
            var row2 = $('#JQDataGrid2').datagrid('getSelected'); //應收付資料
            var data = $("#JQDataGrid2").datagrid('getData');
            var QQ = data.total;
            var ENTRYNO = row.ENTRYNO;
            var usr = getClientInfo('_usercode');
            var typ = "MR";
            var rcvmoneydat = row.rcvmoneydat;
            var CANCELDAT = row1.CANCELDAT;
            var DROPDAT = row1.DROPDAT;
            var BATCHNO = row.BATCHNO;
            var SETAMT = row.SETAMT;
            var MOVEAMT = row.MOVEAMT;
            var EQUIPAMT = row.EQUIPAMT;
            var GTAMT = row.GTAMT;
            if (QQ == 0)
            {
                var MDAT = "";
                var REALAMT = 0;
            }
            else
            {
                var MDAT = row2.MDAT;
                var REALAMT = row2.REALAMT;
            }

            if (rcvmoneydat != null && rcvmoneydat != "") {
                alert("無收款日不可結案!!!");
                return false;
            }

            if (BATCHNO == null || BATCHNO == "") {
                alert("尚未轉應收結案!!");
                return false;
            }

            if (CANCELDAT != null && CANCELDAT != "") {
                alert("客戶資料已經作廢!!");
                return false;
            }

            if (DROPDAT != null && DROPDAT != "") {
                alert("客戶已經退租!!");
                return false;
            }

            if (SETAMT == 0 && MOVEAMT == 0 && EQUIPAMT == 0 && GTAMT == 0) {
                alert("金額不可為0!");
                return false;
            }

            if (MDAT != null && MDAT != "") {
                alert("應收帳款已沖帳不可結案返轉!!!");
                return false;
            }
            if (REALAMT > 0) {
                alert("應收帳款已沖帳不可結案返轉!!!");
                return false;
            }

            
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1041.cmdRT1041', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT1041" + "&parameters=" + CUSID + "," + ENTRYNO + "," + usr + "," + typ + "," + BATCHNO,
                cache: false,
                async: false,
                success: function (data) {
                    alert("處理完成");
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        //作廢
        function btn4Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var CUSID = row.CUSID;
            var ENTRYNO = row.ENTRYNO;
            var usr = getClientInfo('_usercode');
            var typ = "C";
            var finishDAT = row.finishDAT;
            var BATCHNO = row.BATCHNO;
            var CANCELDAT = row.CANCELDAT;

            if (finishDAT != null && finishDAT != "") {
                alert("已結案不可作廢!!!");
                return false;
            }

            if (BATCHNO != null && BATCHNO != "") {
                alert("已轉應收結案，不可作廢!!");
                return false;
            }

            if (CANCELDAT != null && CANCELDAT != "") {
                alert("資料已經作廢!!");
                return false;
            }

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1041.cmdRT1041', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT1041" + "&parameters=" + CUSID + "," + ENTRYNO + "," + usr + "," + typ + "," + BATCHNO,
                cache: false,
                async: false,
                success: function (data) {
                    alert("處理完成");
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        //作廢反轉
        function btn5Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var CUSID = row.CUSID;
            var ENTRYNO = row.ENTRYNO;
            var usr = getClientInfo('_usercode');
            var typ = "R";
            var finishDAT = row.finishDAT;
            var BATCHNO = row.BATCHNO;
            var CANCELDAT = row.CANCELDAT;

            if (CANCELDAT == null || CANCELDAT == "") {
                alert("資料尚未作廢，不可反轉!!");
                return false;
            }

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1041.cmdRT1041', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT1041" + "&parameters=" + CUSID + "," + ENTRYNO + "," + usr + "," + typ + "," + BATCHNO,
                cache: false,
                async: false,
                success: function (data) {
                    alert("處理完成");
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        function btnReloadClick() {
            $('#dataGridView').datagrid('reload');
        }
        /*
        $(document).ready(function () {
            alert("add now");
            $("#toolbardataGridMaster").css('display', 'block');
            alert("add ok");
        })
        */
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT1041.RTLessorAVSCustRepair" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustRepair" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="用戶維修收款" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" Format="" Visible="true" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派單日期" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Visible="true" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費方式" Editor="inforefval" FieldName="PAYTYPE" Format="" maxlength="2" Width="80" EditorOptions="title:'查詢',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="right" Caption="設定費" Editor="numberbox" FieldName="SETAMT" Format="" Visible="true" Width="70" />
                    <JQTools:JQGridColumn Alignment="right" Caption="移機費" Editor="numberbox" FieldName="MOVEAMT" Format="" Visible="true" Width="70" />
                    <JQTools:JQGridColumn Alignment="right" Caption="設備費" Editor="numberbox" FieldName="EQUIPAMT" Format="" Visible="true" Width="70" />
                    <JQTools:JQGridColumn Alignment="right" Caption="保證金" Editor="numberbox" FieldName="GTAMT" Format="" Visible="true" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款產生日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" Visible="true" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="12" Visible="true" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案日期" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Visible="true" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Visible="true" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" Format="" MaxLength="6" Visible="true" Width="120" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="False"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="轉應收結案" Visible="True" OnClick="btn1Click" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="反轉應收結案" Visible="True" OnClick="btn2Click" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="應收應付" Visible="True" OnClick="btn3Click" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢" Visible="True" OnClick="btn4Click" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢反轉" Visible="True" OnClick="btn5Click" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btnReloadClick" Text="資料更新" Visible="True" Icon="icon-reload" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶維修收款">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCustRepair" HorizontalColumnsCount="2" RemoteName="sRT1041.RTLessorAVSCustRepair" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶序號" Editor="infocombobox" FieldName="CUSID" Format="" maxlength="15" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" Format="" Width="32" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派單日期" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="維修員工" Editor="infocombobox" FieldName="REALENGINEER" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.View_RTEmployee',tableName:'View_RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="維修經銷商" Editor="infocombobox" FieldName="REALCONSIGNEE" Format="" maxlength="10" Width="180" EditorOptions="valueField:'CUSID',textField:'CONT',remoteName:'sRT100.RTConsignee',tableName:'RTConsignee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="收款日期" Editor="datebox" FieldName="RCVMONEYDAT" Format="yyyy/mm/dd" Width="180" maxlength="0" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶購買設備" Editor="inforefval" FieldName="EQUIP" Format="" maxlength="2" Width="180" EditorOptions="title:'查詢',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P2'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="設備費" Editor="numberbox" FieldName="EQUIPAMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="設定費" Editor="numberbox" FieldName="SETAMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="移機費" Editor="numberbox" FieldName="MOVEAMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金" Editor="numberbox" FieldName="GTAMT" maxlength="0" Width="180" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費方式" Editor="inforefval" FieldName="PAYTYPE" Format="" maxlength="2" Width="180" EditorOptions="title:'查詢',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡種類" Editor="inforefval" FieldName="CREDITCARDTYPE" Format="" maxlength="2" Width="180" EditorOptions="title:'查詢',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M6'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發卡銀行" Editor="inforefval" FieldName="CREDITBANK" Format="" maxlength="3" Width="180" EditorOptions="title:'查詢',panelWidth:350,panelHeight:400,remoteName:'sRT100.RTBank',tableName:'RTBank',columns:[],columnMatches:[],whereItems:[{field:'CREDITCARD',value:'Y'}],valueField:'HEADNO',textField:'HEADNC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:true,capsLock:'none',fixTextbox:'false'" PlaceHolder="請用挑選" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡卡號" Editor="text" FieldName="CREDITCARDNO" Format="" maxlength="16" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="持卡人姓名" Editor="text" FieldName="CREDITNAME" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效期限(月)" Editor="text" FieldName="CREDITDUEM" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效期限(年)" Editor="text" FieldName="CREDITDUEY" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案日期" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" maxlength="0" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款產生日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" maxlength="12" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="帳款產生人員" Editor="infocombobox" FieldName="TUSR" Format="" Width="180" ReadOnly="True" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" maxlength="6" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註說明" Editor="textarea" FieldName="MEMO" Format="" Width="360" maxlength="500" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日期" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" maxlength="0" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改人員" Editor="infocombobox" EditorOptions="valueField:'CUSID',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="UUSR" Format="" maxlength="6" ReadOnly="True" Width="180" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQAutoSeq ID="JQAutoSeq1" runat="server" BindingObjectID="dataGridView" FieldName="ENTRYNO" NumDig="1" />
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="CUSID" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" FieldName="ENTRYNO" RemoteMethod="False" DefaultMethod="InsEntryno" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
        <div hidden="hidden">
        <JQTools:JQDataGrid ID="JQDataGrid1" runat="server" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" AutoApply="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RTLessorAVSCUST" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT1041.V_RTLessorAVSCUST" RowNumbers="True" Title="JQDataGrid" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
            <Columns>
                <JQTools:JQGridColumn Alignment="left" Caption="社區序號" Editor="text" FieldName="COMQ1" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="主線序號" Editor="text" FieldName="LINEQ1" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="客戶代號" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="30">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="text" FieldName="CANCELDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="text" FieldName="DROPDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                </JQTools:JQGridColumn>
            </Columns>        
        </JQTools:JQDataGrid>
        <JQTools:JQDataGrid ID="JQDataGrid2" runat="server" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" AutoApply="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RTLessorAVSCUSTAR" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT1041.V_RTLessorAVSCUSTAR" RowNumbers="True" Title="JQDataGrid" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
            <Columns>
                <JQTools:JQGridColumn Alignment="left" Caption="客戶編號G+YYMMDD001(YY西元後二位)" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="30">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="24">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="沖銷日" Editor="text" FieldName="MDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="已沖消金額" Editor="text" FieldName="REALAMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
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
