<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT10424.aspx.cs" Inherits="Template_JQueryQuery1" %>

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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT1042.RT10424" runat="server" AutoApply="True"
                DataMember="RT10424" Pagination="True" QueryTitle="Query"
                Title="用戶收款派工單異動資料查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="true" AllowAdd="False" ViewCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="CUSID" Editor="text" FieldName="CUSID" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="ENTRYNO" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="PRTNO" Editor="text" FieldName="PRTNO" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="SEQ" Editor="numberbox" FieldName="SEQ" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="COMQLINE" Editor="text" FieldName="COMQLINE" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="PRTNO1" Editor="text" FieldName="PRTNO1" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="ENTRYNO1" Editor="numberbox" FieldName="ENTRYNO1" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="CHGDAT" Editor="datebox" FieldName="CHGDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="代碼名稱 
" Editor="text" FieldName="CODENC" Format="" MaxLength="500" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="CUSNC" Editor="text" FieldName="CUSNC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="SENDWORKDAT" Editor="datebox" FieldName="SENDWORKDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="DROPDAT" Editor="datebox" FieldName="DROPDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="DROPDESC" Editor="text" FieldName="DROPDESC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="CLOSEDAT" Editor="datebox" FieldName="CLOSEDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="UNCLOSEDAT" Editor="datebox" FieldName="UNCLOSEDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="BONUSCLOSEYM" Editor="text" FieldName="BONUSCLOSEYM" Format="" MaxLength="0" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
