<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1048.aspx.cs" Inherits="Template_JQueryQuery1" %>

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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT1048.RTLessorAVSCustHardware" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustHardware" Pagination="True" QueryTitle="Query"
                Title="設備保管收據列印" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Window" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" ReportFileName="~/CBBN/RT1048R.rdlc">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="CUSID" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="PRTNO" Editor="text" FieldName="PRTNO" Format="" MaxLength="12" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="ENTRYNO" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="PRODNO" Editor="text" FieldName="PRODNO" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="ITEMNO" Editor="text" FieldName="ITEMNO" Format="" MaxLength="3" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="QTY" Editor="numberbox" FieldName="QTY" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="DROPDAT" Editor="datebox" FieldName="DROPDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="DROPREASON" Editor="text" FieldName="DROPREASON" Format="" MaxLength="100" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="WAREHOUSE" Editor="text" FieldName="WAREHOUSE" Format="" MaxLength="2" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="ASSETNO" Editor="text" FieldName="ASSETNO" Format="" MaxLength="20" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="DROPUSR" Editor="text" FieldName="DROPUSR" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="UNIT" Editor="text" FieldName="UNIT" Format="" MaxLength="2" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="AMT" Editor="numberbox" FieldName="AMT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="EUSR" Editor="text" FieldName="EUSR" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="EDAT" Editor="datebox" FieldName="EDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="UUSR" Editor="text" FieldName="UUSR" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="UDAT" Editor="datebox" FieldName="UDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="MEMO" Editor="text" FieldName="MEMO" Format="" MaxLength="50" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="BATCHNO" Editor="text" FieldName="BATCHNO" Format="" MaxLength="12" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="TARDAT" Editor="datebox" FieldName="TARDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="TUSR" Editor="text" FieldName="TUSR" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="RCVPRTNO" Editor="text" FieldName="RCVPRTNO" Format="" MaxLength="13" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="RCVDAT" Editor="datebox" FieldName="RCVDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="RCVFINISHDAT" Editor="datebox" FieldName="RCVFINISHDAT" Format="" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-print" ItemType="easyui-linkbutton" OnClick="exportReport" Text="印表" Visible="True" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="CUSID" Condition="%" DataType="string" Editor="text" FieldName="CUSID" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
