<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT2044.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT204.RT2044" runat="server" AutoApply="True"
                DataMember="RT2044" Pagination="True" QueryTitle="Query"
                Title="超商未沖帳列表" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="False" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="COMN" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶" Editor="text" FieldName="CUSNC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="應收應付" Editor="text" FieldName="ARAC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="期數" Editor="numberbox" FieldName="PERIOD" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="應沖金額" Editor="numberbox" FieldName="AMT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="已沖金額" Editor="numberbox" FieldName="REALAMT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="未沖金額" Editor="numberbox" FieldName="DIFFAMT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖帳日" Editor="datebox" FieldName="MDAT" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖帳員" Editor="text" FieldName="MUSR" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立項一" Editor="text" FieldName="COD1" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立項二" Editor="text" FieldName="COD2" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="產生日" Editor="datebox" FieldName="CDAT" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="Dropdat" Format="yyyy/mm/dd" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="轉Excel" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
