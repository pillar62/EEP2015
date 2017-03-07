<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT3021.aspx.cs" Inherits="Template_JQueryQuery1" %>

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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT302.RTLessorAVSCustBillingPrt" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustBillingPrt" Pagination="True" QueryTitle="Query"
                Title="每月續約帳單列印查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="true" AllowAdd="False" ViewCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="續約通知書批次(yyyymmXX)" Editor="text" FieldName="BATCH" Format="" MaxLength="8" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日(起) - 已到期用戶" Editor="datebox" FieldName="DUEDATSB" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日(迄) - 已到期用戶" Editor="datebox" FieldName="DUEDATEB" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日(起) - 未到期用戶" Editor="datebox" FieldName="DUEDATSA" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日(迄) - 未到期用戶" Editor="datebox" FieldName="DUEDATEA" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="續約通知書產生日" Editor="datebox" FieldName="CDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="續約通知書產生員" Editor="text" FieldName="CUSR" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最後列印日" Editor="datebox" FieldName="PRTDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最後列印人員" Editor="text" FieldName="PRTUSR" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="條碼Src匯出日(for seednet 整批上傳)" Editor="datebox" FieldName="BARCODOUTDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="條碼Src匯出人(for seednet 整批上傳)" Editor="text" FieldName="BARCODOUTUSR" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="條碼檔匯入日" Editor="datebox" FieldName="BARCODINDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="條碼檔匯入人" Editor="text" FieldName="BARCODINUSR" Format="" MaxLength="6" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="續約通知書批次" Condition="%" DataType="string" Editor="text" FieldName="BATCH" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
