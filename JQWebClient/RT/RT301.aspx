<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT301.aspx.cs" Inherits="Template_JQueryQuery1" %>

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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT105.View_RTLessorAVSCase" runat="server" AutoApply="True"
                DataMember="View_RTLessorAVSCase" Pagination="True" QueryTitle="Query"
                Title="RT301" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="true" AllowAdd="False" ViewCommandVisible="False" ReportFileName="~/RT/RT301R.rdlc">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="方案編號" Editor="numberbox" FieldName="CASEID" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案名稱" Editor="text" FieldName="CASENAME" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始日" Editor="datebox" FieldName="STARTDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結束日" Editor="datebox" FieldName="ENDDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建案人員" Editor="text" FieldName="CRTUSR" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" MaxLength="500" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建檔員" Editor="text" FieldName="EUSR" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="輸入日期" Editor="datebox" FieldName="EDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="修改員" Editor="text" FieldName="UUSR" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-print" ItemType="easyui-linkbutton" OnClick="exportReport" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="toexcel" Text="Excel" Visible="True" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="方案編號" Condition="%" DataType="string" Editor="text" FieldName="CASEID" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="方案名稱" Condition="%" DataType="string" Editor="text" FieldName="CASENAME" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="開始日" Condition="&gt;=" DataType="string" Editor="text" FieldName="STARTDAT" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="開始日" Condition="&lt;=" DataType="string" Editor="text" FieldName="STARTDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
            <JQTools:JQAutoSeq ID="JQAutoSeq1" runat="server" />
        </div>

    </form>
</body>
</html>
