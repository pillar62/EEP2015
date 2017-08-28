<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT302.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        function LinkRT3021(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            var s1 = row.LINEQ1;
            parent.addTab("每月續約帳單轉檔作業", "CBBN/RT3021.aspx?COMQ1=" + ss + "&LINEQ1=" + s1);
        }

        function LinkRT3022(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            var s1 = row.LINEQ1;
            parent.addTab("每月續約帳單轉檔作業(過期專用)", "CBBN/RT3022.aspx?COMQ1=" + ss + "&LINEQ1=" + s1);
        }

        //信封列印
        function btn7Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.BATCH;
            $("#JQDataGrid1").datagrid('setWhere', "BATCH='" + ss + "'"); //維護單 
            var WhereString = "";
            exportDevReport("#JQDataGrid1", "sRT302.RT302R", "RT302", "~/CBBN/DevReportForm/RT302RF.aspx", WhereString);
        }

        function btn8Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.BATCH;
            parent.addTab("每月續約帳單客戶明細查詢", "CBBN/RT3028.aspx?BATCH=" + ss);
        }

        function WriteToFile(text) {
            //轉出檔案 預設呼叫後端產生檔案之後再開啟網頁下載功能
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var sbatch = row.BATCH; //批號
            alert(sbatch);
            
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT302.cmdRT3022', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT3022" + "&parameters=" + sbatch,
                cache: false,
                async: false,
                success: function (data) {
                    Date.prototype.yyyymmdd = function () {
                        var mm = this.getMonth() + 1; // getMonth() is zero-based
                        var dd = this.getDate();

                        return [this.getFullYear(),
                                (mm > 9 ? '' : '0') + mm,
                                (dd > 9 ? '' : '0') + dd].join('');
                    };

                    var date = new Date();
                    var val = '334' + date.yyyymmdd();

                    //function downloadScript(val, rowData) {
                        //return '<a href="../handler/JqFileHandler2.ashx?File=' + val + '">' + val + '</a>';
                    //}
                    //var rows = $.parseJSON(data);//將JSon轉會到Object類型提供給Grid顯示                    
                    //alert(data);
                    //window.open('../download/test1.txt', 'file download', config = 'height=500,width=500');
                    window.location.href = "../handler/JqFileHandler2.ashx?File=" + val;
                    //myCSFunction();
                }
            });
        }

        function myCSFunction() {
            $.ajax({

                type: "POST",

                data: "mode=getFile",  //Test隨意定義

                cache: false,

                async: true,

                success: function (data1) {
                    if (data1!=""){
                        alert(data1);  //可以顯示出"CS Function test！"字樣
                    }
                },
                error: function (data) {
                }
            });
        }

        function fileUpLoad() {
            //$('#JQFileUpload1').next().remove()
            //initInfoFileUpload($('#JQFileUpload1'));
            //openImport("#dataGridMaster", 2, 0);
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.BATCH;
            parent.addTab("上傳續約文字檔", "https://service.seed.net.tw/proxy_portal.htm");
        }

        function fileuploadsuccess(value) {
            //這個value值就是文件名。
            alert(value);
            $('#JQFileUpload1').next().remove()
            initInfoFileUpload($('#JQFileUpload1'));
        }

        function getFileUploadValue() {
            //寫在onBeforeUpload事件
            var infofileUpload = $('#JQFileUpload1');
            var infofileUploadvalue = $.data(this, "infofileupload").value;
            var infofileUploadfile = $.data(this, "infofileupload").file;
            infofileUploadvalue.val()//取得文件名稱
            infofileUploadfile.val()//上傳路徑

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT302.RT302" runat="server" AutoApply="True"
                DataMember="RT302" Pagination="True" QueryTitle="查詢條件"
                Title="每月續約帳單列印查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="續約通知書批次" Editor="text" FieldName="BATCH" Format="" MaxLength="8" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日B(起)" Editor="datebox" FieldName="DUEDATSB" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日B(迄)" Editor="datebox" FieldName="DUEDATEB" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日A(起)" Editor="datebox" FieldName="DUEDATSA" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日A(迄)" Editor="datebox" FieldName="DUEDATEA" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="產生日" Editor="datebox" FieldName="CDAT" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="產生員" Editor="text" FieldName="CUSNC" Format="yyyy/mm/dd" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="匯出日" Editor="datebox" FieldName="BARCODOUTDAT" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="匯出員" Editor="text" FieldName="cusnc1" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="條碼匯入日" Editor="datebox" FieldName="BARCODINDAT" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="條碼匯入員" Editor="text" FieldName="cusnc2" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最後列印日" Editor="datebox" FieldName="PRTDAT" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="列印員" Editor="text" FieldName="CUSNC3" Format="" MaxLength="0" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="0.產生續約單" Visible="True" Icon="icon-view" OnClick="LinkRT3021" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="0.產生續約單(過期)" Visible="True" Icon="icon-view" OnClick="LinkRT3022" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="1.匯出續約文字檔" Visible="True" Icon="icon-view" OnClick="WriteToFile" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="2.上傳續約文字檔" Visible="True" OnClick="fileUpLoad" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="3.匯入條碼檔" Visible="True" OnClick="btn5Click" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="4.列印續約單" Visible="True" OnClick="btn6Click" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="5.列印信封" Visible="True" OnClick="btn7Click" Icon="icon-print" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="用戶明細" Visible="True" OnClick="btn8Click" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="續約通知書批次" Condition="%" DataType="string" Editor="text" FieldName="BATCH" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="上傳檔案" Condition="%" DataType="string" Editor="infofileupload" FieldName="CUSNC3" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
            <JQTools:JQFileUpload ID="JQFileUpload1" runat="server" BorderWidth="600px" FileSizeLimited="50000" ShowButton="True" ShowLocalFile="True" UpLoadFolder="barcode" Width="600px" onSuccess="fileuploadsuccess" />
        </div>

        <div id="plLetter">
            <JQTools:JQDataGrid ID="JQDataGrid1" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="RT302R" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT302.RT302R" RowNumbers="True" Title="JQDataGrid" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="cusnc" Editor="text" FieldName="cusnc" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="mydear" Editor="text" FieldName="mydear" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="duedat" Editor="text" FieldName="duedat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="comn" Editor="text" FieldName="comn" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="applydat" Editor="text" FieldName="applydat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="birthday" Editor="text" FieldName="birthday" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="contacttel" Editor="text" FieldName="contacttel" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="mobile" Editor="text" FieldName="mobile" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="socialid" Editor="text" FieldName="socialid" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="email" Editor="text" FieldName="email" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="codenc" Editor="text" FieldName="codenc" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="rzone3" Editor="text" FieldName="rzone3" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="newBillingDat" Editor="text" FieldName="newBillingDat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="payselect" Editor="text" FieldName="payselect" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="casepay" Editor="text" FieldName="casepay" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="addr2" Editor="text" FieldName="addr2" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="addr3" Editor="text" FieldName="addr3" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                </Columns>
            </JQTools:JQDataGrid>
        </div>
    </form>
</body>
</html>
