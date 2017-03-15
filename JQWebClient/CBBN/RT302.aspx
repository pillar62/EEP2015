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
                    //var rows = $.parseJSON(data);//將JSon轉會到Object類型提供給Grid顯示                    
                    alert(data);
                    //window.open('../download/test1.txt', 'file download', config = 'height=500,width=500');
                    //window.location.href = '../download/334.20170310';
                    myCSFunction();
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
            $('#JQFileUpload1').next().remove()
            initInfoFileUpload($('#JQFileUpload1'));
            //openImport("#dataGridMaster", 2, 0);
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
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="0.產生續約單" Visible="True" Icon="icon-view" OnClick="LinkRT3021" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="0.產生續約單(過期)" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="1.匯出續約文字檔" Visible="True" Icon="icon-view" OnClick="WriteToFile" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="2.上傳續約文字檔" Visible="True" OnClick="fileUpLoad" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="3.匯入條碼檔" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="4.列印續約單" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="5.列印信封" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="用戶明細" Visible="True" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="續約通知書批次" Condition="%" DataType="string" Editor="text" FieldName="BATCH" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="上傳檔案" Condition="%" DataType="string" Editor="infofileupload" FieldName="CUSNC3" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
            <JQTools:JQFileUpload ID="JQFileUpload1" runat="server" BorderWidth="600px" FileSizeLimited="50000" ShowButton="True" ShowLocalFile="True" UpLoadFolder="barcode" Width="600px" />
        </div>

    </form>
</body>
</html>
