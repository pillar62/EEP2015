<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT3028.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
     <script>
         var BATCH = Request.getQueryStringByName2("BATCH");
        var flag = true;

        function InsDefault() {
            if (BATCH != "") {
                return BATCH;
            }
        }

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = "A.BATCH='" + BATCH + "'";                
                $("#dataGridMaster").datagrid('setWhere', sWhere);
            }
            flag = false;
        }        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT302.RT3028" runat="server" AutoApply="True"
                DataMember="RT3028" Pagination="True" QueryTitle="Query"
                Title="每月續約帳單客戶明細查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" OnLoadSuccess="dgOnloadSuccess" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="續約單號" Editor="text" FieldName="noticeid" Format="" MaxLength="13" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="上載批次" Editor="text" FieldName="batch" Format="" MaxLength="8" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="直經銷" Editor="text" FieldName="consignee" Format="" MaxLength="0" Width="50" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轄區" Editor="text" FieldName="shortnc" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線" Editor="text" FieldName="comlineq1" Format="" MaxLength="0" Width="50" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="comn" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶名稱" Editor="text" FieldName="cusnc" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日" Editor="datebox" FieldName="duedat" Format="yyyy/mm/dd" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="連絡電話" Editor="text" FieldName="tel" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳單地址" Editor="text" FieldName="raddr" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="dropdat" Format="yyyy/mm/dd" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="strbillingdat" Format="yyyy/mm/dd" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="續約計費日" Editor="datebox" FieldName="newbillingdat" Format="yyyy/mm/dd" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案" Editor="text" FieldName="codenc" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="週期" Editor="text" FieldName="codenc1" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="已收" Editor="text" FieldName="codenc2" Format="" MaxLength="0" Width="80" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-print" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="列印續約單" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-print" ItemType="easyui-linkbutton" OnClick="btn2Click" Text="列印信封" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出至Excel" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
