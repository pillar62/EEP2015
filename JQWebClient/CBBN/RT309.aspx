<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT309.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        function queryGrid(dg) { //查詢后添加固定條件
            if ($(dg).attr('id') == 'dataGridMaster') {
                var where = $(dg).datagrid('getWhere');

                if (where != "") {
                    where = " 1=1 ";
                    FINISHDAT = $("#FINISHDAT_Query").datebox('getValue'); //日期
                    where = where + " and d.TYY = datepart(yyyy, dateadd(d, -1,'" +FINISHDAT+ "')) " 
                    + " and d.TMM = datepart(m, dateadd(d, -1,'" +FINISHDAT+ "')) "
                    + " and (a.dropdat is null  or a.dropdat >= dateadd(m, -1, '"+FINISHDAT+ "'))";
                }
                $(dg).datagrid('setWhere', where);
            }
        }

        //列印社區主線戶數
        function btnPrintClick() {
            var WhereString = "";
            exportDevReport("#dataGridMaster", "sRT309.RT309", "RT309", "~/CBBN/DevReportForm/RT309RF.aspx", WhereString);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT309.RT309" runat="server" AutoApply="True"
                DataMember="RT309" Pagination="True" QueryTitle="查詢條件"
                Title="自營經銷商拆帳" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="主線" Editor="text" FieldName="comn" Format="" MaxLength="0" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區" Editor="text" FieldName="comq" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶代號" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶名稱" Editor="text" FieldName="cusnc" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳款方式" Editor="text" FieldName="paytype" Format="" MaxLength="2" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳款週期" Editor="text" FieldName="paycycle" Format="" MaxLength="2" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="STRBILLINGDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="續約日" Editor="datebox" FieldName="newBILLINGDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日" Editor="datebox" FieldName="DUEDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="列帳年" Editor="numberbox" FieldName="TYY" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="列帳月" Editor="numberbox" FieldName="TMM" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="認列收入" Editor="numberbox" FieldName="amt" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="經銷商拆帳比率" Editor="numberbox" FieldName="ratio" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轄區所屬" Editor="text" FieldName="belongnc" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="經銷商拆帳金額" Editor="numberbox" FieldName="shareamt" Format="" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-print" ItemType="easyui-linkbutton" OnClick="btnPrintClick" Text="列印" Visible="True" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="截止日期(請選月初)：" Condition="=" DataType="datetime" Editor="datebox" FieldName="FINISHDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
