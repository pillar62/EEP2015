<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT207.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT207.RTInvMonth" runat="server" AutoApply="True"
                DataMember="RTInvMonth" Pagination="True" QueryTitle="查詢條件" EditDialogID="JQDialog1"
                Title="發票字軌維護" QueryMode="Panel" ReportFileName="~/CBBN/RT207R.rdlc" DeleteCommandVisible="False" UpdateCommandVisible="False" ViewCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="年份" Editor="numberbox" FieldName="INVYEAR" Format="" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="月份" Editor="text" FieldName="INVMONTH" Format="" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="發票字軌" Editor="text" FieldName="INVTRACK" Format="" MaxLength="2" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="二聯發票起號" Editor="text" FieldName="INVNOS" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="二聯發票止號" Editor="text" FieldName="INVNOE" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="三聯發票起號" Editor="text" FieldName="INVNOS3" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="三聯發票止號" Editor="text" FieldName="INVNOE3" Format="" MaxLength="10" Visible="true" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="True"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="exportReport" Text="列印" Visible="True" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="年份" Condition="=" DataType="number" Editor="text" FieldName="INVYEAR" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="月份" Condition="=" DataType="number" Editor="text" FieldName="INVMONTH" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="發票字軌" Condition="%%" DataType="string" Editor="text" FieldName="INVTRACK" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="發票字軌維護">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTInvMonth" HorizontalColumnsCount="2" RemoteName="sRT207.RTInvMonth" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="年份" Editor="numberbox" FieldName="INVYEAR" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="月份" Editor="text" FieldName="INVMONTH" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發票字軌" Editor="text" FieldName="INVTRACK" Format="" maxlength="2" Width="180" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="二聯發票起號" Editor="text" FieldName="INVNOS" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="二聯發票止號" Editor="text" FieldName="INVNOE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="三聯發票起號" Editor="text" FieldName="INVNOS3" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="三聯發票止號" Editor="text" FieldName="INVNOE3" Format="" maxlength="10" Width="180" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
</html>
