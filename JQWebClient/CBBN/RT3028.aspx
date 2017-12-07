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

         //續約單列印
        function btn1Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.BATCH;
            var CUSTID = row.CUSID;
            var NOTICEID = row.noticeid;
            $("#JQDataGrid2").datagrid('setWhere', "A.NOTICEID='" + NOTICEID + "'"); //續約 
            
            var WhereString = "";
            exportDevReport("#JQDataGrid2", "sRT302.RT3021R", "RT302", "~/CBBN/DevReportForm/RT3021RF.aspx", WhereString);
        }

        //信封列印
        function btn2Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.BATCH;
            var CUSTID = row.CUSID;
            var NOTICEID = row.noticeid;
            $("#JQDataGrid1").datagrid('setWhere', "A.NOTICEID='" + NOTICEID + "'"); //維護單 
            var WhereString = "";
            exportDevReport("#JQDataGrid1", "sRT302.RT302R", "RT302", "~/CBBN/DevReportForm/RT302RF.aspx", WhereString);
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
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶編號G+YYMMDD001(YY西元後二位)" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="80">
                    </JQTools:JQGridColumn>
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

        <div id="plLetter" style="display:none"><!-- style="display:none"-->
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
        <p>
        <div id="plC" style="display:none"> <!-- style="display:none"-->
            <JQTools:JQDataGrid ID="JQDataGrid2" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="RT3021R" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="False" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT302.RT3021R" RowNumbers="True" Title="JQDataGrid" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="上載批次" Editor="text" FieldName="BATCH" Frozen="False" IsNvarChar="False" MaxLength="8" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="16">
                    </JQTools:JQGridColumn>
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
                    <JQTools:JQGridColumn Alignment="left" Caption="addr2" Editor="text" FieldName="addr2" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="addr3" Editor="text" FieldName="addr3" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="P01" Editor="text" FieldName="P01" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="COD11" Editor="text" FieldName="COD11" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="COD12" Editor="text" FieldName="COD12" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="COD13" Editor="text" FieldName="COD13" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="D1" Editor="text" FieldName="D1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="CSNOTICEID01" Editor="text" FieldName="CSNOTICEID01" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="CSCUSID01" Editor="text" FieldName="CSCUSID01" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="Q1" Editor="text" FieldName="Q1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="P02" Editor="text" FieldName="P02" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="COD21" Editor="text" FieldName="COD21" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="COD22" Editor="text" FieldName="COD22" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="COD23" Editor="text" FieldName="COD23" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="D2" Editor="text" FieldName="D2" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="CSNOTICEID02" Editor="text" FieldName="CSNOTICEID02" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="CSCUSID02" Editor="text" FieldName="CSCUSID02" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="Q2" Editor="text" FieldName="Q2" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="P03" Editor="text" FieldName="P03" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="COD31" Editor="text" FieldName="COD31" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="COD32" Editor="text" FieldName="COD32" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="COD33" Editor="text" FieldName="COD33" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="D3" Editor="text" FieldName="D3" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="CSNOTICEID03" Editor="text" FieldName="CSNOTICEID03" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="CSCUSID03" Editor="text" FieldName="CSCUSID03" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="Q3" Editor="text" FieldName="Q3" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="續約通知書單號Cyyyymmddxxxx" Editor="text" FieldName="NOTICEID" Frozen="False" IsNvarChar="False" MaxLength="13" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="26">
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
        </p>
        </div>
    </form>
</body>
</html>
