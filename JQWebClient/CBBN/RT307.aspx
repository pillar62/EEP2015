<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT307.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        //列印收款明細表
        function btnPrintClick() {
            var WhereString = "";
            exportDevReport("#dataGridMaster", "sRT307.RT307", "RT307", "~/CBBN/DevReportForm/RT307RF.aspx", WhereString);
        }

        function DownLoadTxt() {
            var s1 = $('#dataGridMaster').datagrid('getWhere');//取得當前主檔中選中的那個Data
            if (s1 == "")
            {
                alert("請先查詢資料!!");
                return false;
            }

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT307.cmd', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT3071" + "&parameters=" + s1,
                cache: false,
                async: false,
                success: function (data) {
                    if (data == "N") {
                        alert("查無資料可供轉出!!");
                        return false;
                    }
                    else
                    {
                        alert("下載檔案名稱：["+data+"] 請儲存到指定位置!!");
                        window.location.href = "../handler/JqFileHandler2.ashx?File=" + data;
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT307.RT307" runat="server" AutoApply="True"
                DataMember="RT307" Pagination="True" QueryTitle="查詢條件"
                Title="收款明細表" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" ReportFileName="/DevReportForm/RT307RF.aspx" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="組別(工程師)" Editor="text" FieldName="belongnc" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="收款方式" Editor="text" FieldName="CODENC" MaxLength="0" Width="60" Visible="True" Format="" />
                    <JQTools:JQGridColumn Alignment="left" Caption="品名" Editor="text" FieldName="amtnc" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="數量" Editor="numberbox" FieldName="qty" Format="" Width="40" />
                    <JQTools:JQGridColumn Alignment="right" Caption="收款金額" Editor="text" FieldName="amt" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="統編" Editor="text" FieldName="unino" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="發票抬頭" Editor="text" FieldName="invtitle" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="comn" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶名稱" Editor="text" FieldName="cusnc" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳單地址" Editor="text" FieldName="raddr" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="電話" Editor="text" FieldName="contacttel" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報竣日" Editor="datebox" FieldName="docketdat" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="收款日" Editor="datebox" FieldName="rcvmoneydat" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="起始日" Editor="datebox" FieldName="strbillingdat" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="終止日" Editor="datebox" FieldName="duedat" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="memo" Format="" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="120">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Enabled="True" Icon="icon-print" ItemType="easyui-linkbutton" OnClick="btnPrintClick" Text="印表" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-ok" ItemType="easyui-linkbutton" Text="轉出電子發票檔" Visible="True" OnClick="DownLoadTxt" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel檔" Visible="True" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="收款日期起" Condition="&gt;=" DataType="string" DefaultValue="_today" Editor="datebox" FieldName="rcvmoneydat" Format="yyyy/mm/dd" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="收款日期迄" Condition="&lt;=" DataType="string" DefaultValue="_today" Editor="datebox" FieldName="rcvmoneydat" Format="yyyy/mm/dd" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>
</html>
