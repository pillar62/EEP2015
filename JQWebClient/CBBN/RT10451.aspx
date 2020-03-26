<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT10451.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID");
        var ENTRYNO = Request.getQueryStringByName2("ENTRYNO");
        var flag = true;
        var usr = getClientInfo('_usercode');

        function InsDefault() {
            if (CUSID != "") {
                return CUSID;
            }
        }

        $(document).ready(function () {
            dgOnloadSuccess();
        })

        function dgOnloadSuccess()
        {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = "A.CUSID='" + CUSID + "'";
                $("#dataGridMaster").datagrid('setWhere', sWhere);
                var row = $("#dataGridMaster").datagrid("selectRow", 0);
            }
            flag = false;
        }

        function btnIns(val) {
            var sMODE = "I";
            parent.addTab("用戶退租拆機派工單資料維護新增", "CBBN/RT104511.aspx?CUSID=" + CUSID + "&PRTNO=自動編號" + "&sMODE=" + sMODE);
        }

        function btnEdit(val) {
            var sMODE = "E";
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            parent.addTab("用戶退租拆機派工單資料維護修改", "CBBN/RT104511.aspx?CUSID=" + CUSID + "&ENTRYNO = " + ENTRYNO + "&PRTNO=" + PRTNO + "&sMODE=" + sMODE);
        }

        //物品領用單
        function btn1Click()
        {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            parent.addTab("物品領用單資料維護", "CBBN/RT104512.aspx?PRTNO=" + PRTNO);
        }

        //完工結案
        function btn3Click() {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            var ENTRYNO = row.ENTRYNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1045.cmdRT10451', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10451" + "&parameters=" + CUSID  + "," + ENTRYNO + "," + PRTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridMaster').datagrid('reload');
                }
            });
        }

        //未完工結案
        function btn4Click() {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            var ENTRYNO = row.ENTRYNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1045.cmdRT10452', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10452" + "&parameters=" + CUSID + "," + ENTRYNO + "," + PRTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridMaster').datagrid('reload');
                }
            });
        }

        //結案返轉
        function btn5Click() {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            var ENTRYNO = row.ENTRYNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1045.cmdRT10453', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10453" + "&parameters=" + CUSID + "," + ENTRYNO + "," + PRTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridMaster').datagrid('reload');
                }
            });
        }

        //作廢
        function btn6Click() {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            var ENTRYNO = row.ENTRYNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1045.cmdRT10454', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10454" + "&parameters=" + CUSID + "," + ENTRYNO + "," + PRTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridMaster').datagrid('reload');
                }
            });
        }

        //作廢返轉
        function btn7Click() {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            var ENTRYNO = row.ENTRYNO;
            
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1045.cmdRT10455', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10455" + "&parameters=" + CUSID + "," + ENTRYNO + "," + PRTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridMaster').datagrid('reload');
                }
            });
        }

        function btn8Click(val) {
            var sMODE = "E";
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            parent.addTab("用戶裝機派工設備資料維護", "CBBN/RT10453.aspx?CUSID=" + CUSID + "&PRTNO=" + PRTNO + "&sMODE=" + sMODE);
        }

        function btn9Click(val) {
            var sMODE = "E";
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var PRTNO = row.PRTNO;
            parent.addTab("用戶退租拆機派工單異動資料查詢", "CBBN/RT10459.aspx?CUSID=" + CUSID + "&PRTNO=" + PRTNO + "&sMODE=" + sMODE);
        }

        function mySelect()
        {
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT1045.RT10451" runat="server" AutoApply="True"
                DataMember="RT10451" Pagination="True" QueryTitle="Query"
                Title="用戶退租拆機派工單資料維護" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Fuzzy" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="False" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" OnLoadSuccess="dgOnloadSuccess" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" EnableViewState="False" OnSelect="mySelect">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線" Editor="text" FieldName="comqline" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" MaxLength="12" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日期" Editor="datebox" FieldName="SENDWORKDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="列印人員" Editor="text" FieldName="CUSNC" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="預定拆機員" Editor="text" FieldName="Column1" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="實際拆機員" Editor="text" FieldName="Column2" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案日" Editor="datebox" FieldName="closedat" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="未完工結案日" Editor="datebox" FieldName="unclosedat" Format="yyyy/mm/dd" Width="80" MaxLength="0" Visible="True" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金計算月" Editor="text" FieldName="BONUSCLOSEYM" Format="yyyy/mm" MaxLength="6" Width="70" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金會計審核日" Editor="datebox" FieldName="BONUSFINCHK" Format="yyyy/mm/dd" Width="80" MaxLength="0" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="batchno" Format="" MaxLength="12" Width="120" Visible="True" />
                    <JQTools:JQGridColumn Alignment="left" Caption="庫存結算月" Editor="text" FieldName="STOCKCLOSEYM" Format="yyyy/mm" MaxLength="6" Width="70" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="庫存會計審核日" Editor="datebox" FieldName="STOCKFINCHK" Format="yyyy/mm/dd" Width="80" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="80" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btnIns" Text="新增" Visible="True" Icon="icon-add" ID="btnIns" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="btnEdit" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="物品領用單" Visible="False" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn2Click" Text="列印" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn3Click" Text="完工結案" Visible="True" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn4Click" Text="未完工結案" Visible="True" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn5Click" Text="結案返轉" Visible="True" Icon="icon-undo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn6Click" Text="作廢" Visible="True" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn7Click" Text="作廢返轉" Visible="True" Icon="icon-redo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn8Click" Text="設備明細" Visible="False" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn9Click" Text="歷史異動" Visible="True" Icon="icon-view" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
     
               </div>
        
    </form>
</body>
</html>
