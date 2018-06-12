<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT310.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        //列印社區主線戶數
        function btnPrintClick() {
            var WhereString = "";
            exportDevReport("#dataGridMaster", "sRT310.RT310", "RT310", "~/CBBN/DevReportForm/RT310RF.aspx", WhereString);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT310.RT310" runat="server" AutoApply="True"
                DataMember="RT310" Pagination="True" QueryTitle="查詢條件"
                Title="社區列表" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="False" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="轄區" Editor="text" FieldName="areanc" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="right" Caption="社區序號" Editor="numberbox" FieldName="comq1" Format="" MaxLength="0" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="comn" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="規模戶數" Editor="numberbox" FieldName="comcnt" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線數" Editor="numberbox" FieldName="linenum" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="right" Caption="用戶數" Editor="numberbox" FieldName="custnum" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="縣市" Editor="text" FieldName="cutnc" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="鄉鎮" Editor="text" FieldName="township" Format="" MaxLength="10" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區地址" Editor="text" FieldName="raddr" Format="" MaxLength="60" Width="220" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-print" ItemType="easyui-linkbutton" OnClick="btnPrintClick" Text="列印" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>
</html>
