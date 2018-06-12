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
                    <JQTools:JQGridColumn Alignment="left" Caption="收款方式" Editor="text" FieldName="CODENC" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="組別(工程師)" Editor="text" FieldName="belongnc" Format="" MaxLength="0" Width="80" Visible="True" />
                    <JQTools:JQGridColumn Alignment="right" Caption="收款金額" Editor="text" FieldName="amt" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="數量" Editor="numberbox" FieldName="qty" Format="" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="統編" Editor="text" FieldName="unino" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="發票抬頭" Editor="text" FieldName="invtitle" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="comn" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶名稱" Editor="text" FieldName="cusnc" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳單地址" Editor="text" FieldName="raddr" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="電話" Editor="text" FieldName="contacttel" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報竣日" Editor="datebox" FieldName="docketdat" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="收款日" Editor="datebox" FieldName="rcvmoneydat" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="起始日" Editor="datebox" FieldName="strbillingdat" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="終止日" Editor="datebox" FieldName="duedat" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="memo" Format="" MaxLength="0" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Enabled="True" Icon="icon-print" ItemType="easyui-linkbutton" OnClick="btnPrintClick" Text="印表" Visible="True" />
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
