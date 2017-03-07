<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1042.aspx.cs" Inherits="Template_JQuerySingle1" %>

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
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT1042.RTLessorAVSCustSndwork" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustSndwork" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="用戶裝機派工單資料維護">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="主線" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" MaxLength="12" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工日/完工日" Editor="datebox" FieldName="SENDWORKDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="列印人員" Editor="text" FieldName="PRTUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="預定施工員" Editor="text" FieldName="ASSIGNENGINEER" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="預定人員" Editor="text" FieldName="ASSIGNCONSIGNEE" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="實際施工員" Editor="text" FieldName="REALENGINEER" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="實際人員" Editor="text" FieldName="REALCONSIGNEE" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工結案日" Editor="datebox" FieldName="DROPDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="未完工員因" Editor="text" FieldName="DROPDESC" Format="" MaxLength="200" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金關帳日" Editor="datebox" FieldName="CLOSEDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金計算月" Editor="text" FieldName="BONUSCLOSEYM" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金計算日" Editor="datebox" FieldName="BONUSCLOSEDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金結算員" Editor="text" FieldName="BONUSCLOSEUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金會計審核日" Editor="datebox" FieldName="BONUSFINCHK" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="庫存結算月" Editor="text" FieldName="STOCKCLOSEYM" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="庫存結算日" Editor="datebox" FieldName="STOCKCLOSEDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="庫存結算員" Editor="text" FieldName="STOCKCLOSEUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="庫存會計審核日" Editor="datebox" FieldName="STOCKFINCHK" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="MDF位置" Editor="text" FieldName="MDF1" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="端子版位置" Editor="text" FieldName="MDF2" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="SWITCH(PNA)編號" Editor="text" FieldName="HOSTNO" Format="" MaxLength="3" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="PORT號" Editor="text" FieldName="HOSTPORT" Format="" MaxLength="3" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註說明" Editor="text" FieldName="MEMO" Format="" MaxLength="300" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="列印日期" Editor="datebox" FieldName="PRTDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="未完工結案日" Editor="datebox" FieldName="UNCLOSEDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="DROPUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="CLOSEUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建檔人員" Editor="text" FieldName="EUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建檔日期" Editor="datebox" FieldName="EDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="UUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動日期" Editor="datebox" FieldName="UDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="BATCHNO" Editor="text" FieldName="BATCHNO" Format="" MaxLength="12" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="CDAT" Editor="datebox" FieldName="CDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="MAXENTRYNO" Editor="numberbox" FieldName="MAXENTRYNO" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶網卡MAC" Editor="text" FieldName="MAC" Format="" MaxLength="20" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="收款日(裝機)" Editor="datebox" FieldName="RCVMONEYDAT" Format="" Visible="true" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply"
                        Text="存檔" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel"
                        Text="取消"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶裝機派工單資料維護">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCustSndwork" HorizontalColumnsCount="2" RemoteName="sRT1042.RTLessorAVSCustSndwork" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="主線" Editor="text" FieldName="CUSID" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" maxlength="12" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工日/完工日" Editor="datebox" FieldName="SENDWORKDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="列印人員" Editor="text" FieldName="PRTUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="預定施工員" Editor="text" FieldName="ASSIGNENGINEER" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="預定人員" Editor="text" FieldName="ASSIGNCONSIGNEE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="實際施工員" Editor="text" FieldName="REALENGINEER" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="實際人員" Editor="text" FieldName="REALCONSIGNEE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="完工結案日" Editor="datebox" FieldName="DROPDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="未完工員因" Editor="text" FieldName="DROPDESC" Format="" maxlength="200" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="獎金關帳日" Editor="datebox" FieldName="CLOSEDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="獎金計算月" Editor="text" FieldName="BONUSCLOSEYM" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="獎金計算日" Editor="datebox" FieldName="BONUSCLOSEDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="獎金結算員" Editor="text" FieldName="BONUSCLOSEUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="獎金會計審核日" Editor="datebox" FieldName="BONUSFINCHK" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="庫存結算月" Editor="text" FieldName="STOCKCLOSEYM" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="庫存結算日" Editor="datebox" FieldName="STOCKCLOSEDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="庫存結算員" Editor="text" FieldName="STOCKCLOSEUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="庫存會計審核日" Editor="datebox" FieldName="STOCKFINCHK" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="MDF位置" Editor="text" FieldName="MDF1" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="端子版位置" Editor="text" FieldName="MDF2" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="SWITCH(PNA)編號" Editor="text" FieldName="HOSTNO" Format="" maxlength="3" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="PORT號" Editor="text" FieldName="HOSTPORT" Format="" maxlength="3" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註說明" Editor="text" FieldName="MEMO" Format="" maxlength="300" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="列印日期" Editor="datebox" FieldName="PRTDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="未完工結案日" Editor="datebox" FieldName="UNCLOSEDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="DROPUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="CLOSEUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔人員" Editor="text" FieldName="EUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日期" Editor="datebox" FieldName="EDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="UUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動日期" Editor="datebox" FieldName="UDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="BATCHNO" Editor="text" FieldName="BATCHNO" Format="" maxlength="12" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="CDAT" Editor="datebox" FieldName="CDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="MAXENTRYNO" Editor="numberbox" FieldName="MAXENTRYNO" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶網卡MAC" Editor="text" FieldName="MAC" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="收款日(裝機)" Editor="datebox" FieldName="RCVMONEYDAT" Format="" Width="180" />
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
