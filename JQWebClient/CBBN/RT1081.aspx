<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1081.aspx.cs" Inherits="Template_JQuerySingle1" %>

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
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT108.RT108" runat="server" AutoApply="True"
                DataMember="RT108" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="社區查詢" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="社區" Editor="text" FieldName="COMQ1" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線" Editor="text" FieldName="LINEQ1" Visible="true" Width="90" MaxLength="10" />
                    <JQTools:JQGridColumn Alignment="left" Caption="個案別" Editor="text" FieldName="COMTYPE" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="COMQ11" Editor="text" FieldName="COMQ11" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="COMN" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區附掛" Editor="text" FieldName="LINETEL" Visible="true" Width="40" MaxLength="20" />
                    <JQTools:JQGridColumn Alignment="left" Caption="規模戶數" Editor="text" FieldName="COMCNT" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶數" Editor="text" FieldName="USERCNT" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案別" Editor="text" FieldName="COMTYPENM" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開通日" Editor="text" FieldName="T1APPLYDAT" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="撤線日" Editor="text" FieldName="RCOMDROP" MaxLength="20" Visible="true" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="text" FieldName="CANCELDAT" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="經銷商" Editor="text" FieldName="GROUPNC" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="工程師" Editor="text" FieldName="LEADER" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建置書" Editor="text" FieldName="COMAGREE" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="Reset電話" Editor="text" FieldName="TEL" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="整線派工" Editor="text" FieldName="YN_01" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="整線完成" Editor="text" FieldName="YN_02" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="審核完成" Editor="text" FieldName="YN_03" MaxLength="0" Visible="true" Width="90" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="Insert" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem"
                        Text="Update" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem"
                        Text="Delete"  />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton"
                        OnClick="apply" Text="Apply" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="Cancel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="Export" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="社區查詢">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RT108" HorizontalColumnsCount="2" RemoteName="sRT108.RT108" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="社區" Editor="text" FieldName="COMQ1" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線" Editor="text" FieldName="LINEQ1" Width="180" MaxLength="10" />
                        <JQTools:JQFormColumn Alignment="left" Caption="個案" Editor="text" FieldName="COMTYPE" maxlength="0" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="COMQ11" Editor="text" FieldName="COMQ11" maxlength="0" Width="80" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="COMN" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="附掛電話" Editor="text" FieldName="LINETEL" Width="180" MaxLength="20" />
                        <JQTools:JQFormColumn Alignment="left" Caption="規模戶數" Editor="text" FieldName="COMCNT" maxlength="0" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶數" Editor="text" FieldName="USERCNT" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案別" Editor="text" FieldName="COMTYPENM" maxlength="0" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="開通日" Editor="text" FieldName="T1APPLYDAT" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="撤線日" Editor="text" FieldName="RCOMDROP" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="text" FieldName="CANCELDAT" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="GROUPNC" Editor="text" FieldName="GROUPNC" maxlength="0" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="工程師" Editor="text" FieldName="LEADER" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建置書" Editor="text" FieldName="COMAGREE" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="Reset電話" Editor="text" FieldName="TEL" maxlength="0" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="整線派工" Editor="text" FieldName="YN_01" maxlength="0" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="整線完成" Editor="text" FieldName="YN_02" maxlength="0" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="審核完成" Editor="text" FieldName="YN_03" maxlength="0" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址" Editor="text" FieldName="addr" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="網路IP" Editor="text" FieldName="IPADDR" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地區" Editor="text" FieldName="AREANC" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="聯絡人" Editor="text" FieldName="CTYContact" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="聯絡人電話" Editor="text" FieldName="CTYcontactTel" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="80" />
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
