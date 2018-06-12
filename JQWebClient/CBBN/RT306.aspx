<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT306.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
     <script>
         function dgOnloadSuccess() {
             var row = $('#JQDataGrid1').datagrid('getSelected');//取得當前主檔中選中的那個Data

             if ($("#BATCH_Query").val()=="")
             {
                 $("#BATCH_Query").val(row.maxbatch);
                 $("#INVDAT_Query").val(row.dt_s);
             }             
             $('#dd').hide();
             var sWhere = " A.BATCH = '" + row.maxbatch + "'";
             $('#dataGridMaster').datagrid('setWhere', sWhere);//篩選資料           
         }

         //發票列印
         function btnINVOClick(val) {
             var WhereString = "";
             exportDevReport("#dataGridMaster", "sRT306.cmdRT3061", "cmdRT3061", "~/CBBN/DevReportForm/RT306RF.aspx", WhereString);
         }


         function btnReloadClick() {
             $('#dataGridView').datagrid('reload');
         }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT306.cmdRT3061" runat="server" AutoApply="False"
                DataMember="cmdRT3061" Pagination="True" QueryTitle="發票列印"
                Title="發票列印" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ReportFileName="/DevReportForm/RT306RF.aspx">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="發票號碼" Editor="text" FieldName="INVNO" Width="90" MaxLength="10" />
                    <JQTools:JQGridColumn Alignment="left" Caption="發票抬頭" Editor="text" FieldName="INVTITLE" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="統一編號" Editor="text" FieldName="UNINO" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="檢查碼" Editor="text" FieldName="CHKNO" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="48">
                    </JQTools:JQGridColumn><JQTools:JQGridColumn Alignment="left" Caption="發票日期" Editor="text" FieldName="INVDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn><JQTools:JQGridColumn Alignment="left" Caption="稅別" Editor="text" FieldName="TAXTYPE" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="60">
                    </JQTools:JQGridColumn><JQTools:JQGridColumn Alignment="left" Caption="發票金額" Editor="text" FieldName="SALESUM" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn><JQTools:JQGridColumn Alignment="left" Caption="發票稅額" Editor="text" FieldName="TAXSUM" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn><JQTools:JQGridColumn Alignment="left" Caption="發票總額" Editor="text" FieldName="TOTALSUM" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn><JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRY" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="40">
                    </JQTools:JQGridColumn><JQTools:JQGridColumn Alignment="left" Caption="產品名稱" Editor="text" FieldName="PRODNC" Frozen="False" IsNvarChar="False" MaxLength="100" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="100">
                    </JQTools:JQGridColumn><JQTools:JQGridColumn Alignment="left" Caption="列印批號" Editor="text" FieldName="BATCH" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="64">
                    </JQTools:JQGridColumn><JQTools:JQGridColumn Alignment="left" Caption="數量" Editor="text" FieldName="QTY" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="60">
                    </JQTools:JQGridColumn><JQTools:JQGridColumn Alignment="left" Caption="單價" Editor="text" FieldName="UNITAMT" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="60">
                    </JQTools:JQGridColumn><JQTools:JQGridColumn Alignment="left" Caption="銷售額" Editor="text" FieldName="SALEAMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="60">
                    </JQTools:JQGridColumn><JQTools:JQGridColumn Alignment="left" Caption="稅額" Editor="text" FieldName="TAXAMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="60">
                    </JQTools:JQGridColumn></Columns>
                <TooItems>
                    <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="Export" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-print" ItemType="easyui-linkbutton" OnClick="btnINVOClick" Text="列印發票" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btnReloadClick" Text="資料更新" Visible="True" Icon="icon-reload" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="列印批次 :" Condition="=" DataType="string" Editor="text" FieldName="BATCH" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="發票日期(起) :" Condition="&gt;=" DataType="datetime" Editor="datebox" FieldName="INVDAT" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="發票日期(迄) :" Condition="&lt;=" DataType="datetime" Editor="datebox" FieldName="INVDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
            <div id="dd" aria-hidden="True">
            <JQTools:JQDataGrid ID="JQDataGrid1" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="False" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="cmdRT306" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="False" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT306.cmdRT306" RowNumbers="True" Title="JQDataGrid" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="maxbatch" Editor="text" FieldName="maxbatch" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="dt_s" Editor="text" FieldName="dt_s" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="dt_e" Editor="text" FieldName="dt_e" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Enabled="True" Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="Insert" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="Update" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="Delete" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="Apply" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="Cancel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="Export" Visible="True" />
                </TooItems>
            </JQTools:JQDataGrid>
                </div>
        </div>

    </form>
</body>
<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>
</html>
