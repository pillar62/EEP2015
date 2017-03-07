<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT201.aspx.cs" Inherits="Template_JQuerySingle1" %>

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
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT201.RTLessorAVSCustFaqH" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustFaqH" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="用戶客服單資料維護">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="服務單號" Editor="text" FieldName="FAQNO" Format="" MaxLength="13" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="來電日期" Editor="datebox" FieldName="RCVDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="服務類型" Editor="text" FieldName="SERVICETYPE" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="摘要" Editor="text" FieldName="MEMO" Format="" MaxLength="1024" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="連絡電話" Editor="text" FieldName="CONTACTTEL" Format="" MaxLength="15" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="行動電話" Editor="text" FieldName="MOBILE" Format="" MaxLength="30" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="datebox" FieldName="SNDWORK" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="列印日" Editor="datebox" FieldName="PRTDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工結案日" Editor="datebox" FieldName="SNDCLOSEDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客服回CALL日" Editor="datebox" FieldName="CALLBACKDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客服回CALL人員" Editor="text" FieldName="CALLBACKUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="FUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="CANCELDAT" Format="" Visible="true" Width="120" />
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

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶客服單資料維護">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCustFaqH" HorizontalColumnsCount="2" RemoteName="sRT201.RTLessorAVSCustFaqH" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶編號" Editor="text" FieldName="CUSID" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="報修聯絡人" Editor="text" FieldName="FAQMAN" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="inbound / outbound" Editor="text" FieldName="IOLINE" Format="" maxlength="1" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="服務單號" Editor="text" FieldName="FAQNO" Format="" maxlength="13" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="連絡電話" Editor="text" FieldName="CONTACTTEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="行動電話" Editor="text" FieldName="MOBILE" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="EMAIL" Editor="text" FieldName="EMAIL" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="列印日" Editor="datebox" FieldName="PRTDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="受理人員" Editor="text" FieldName="RCVUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="來電日期" Editor="datebox" FieldName="RCVDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="服務類型" Editor="text" FieldName="SERVICETYPE" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔人員" Editor="text" FieldName="EUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日期" Editor="datebox" FieldName="EDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="UUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動日期" Editor="datebox" FieldName="UDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工人員" Editor="text" FieldName="SNDUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="轉派工單日期" Editor="datebox" FieldName="SNDWORK" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="SNDPRTNO" Format="" maxlength="13" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工結案日" Editor="datebox" FieldName="SNDCLOSEDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客服回CALL日" Editor="datebox" FieldName="CALLBACKDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客服回CALL人員" Editor="text" FieldName="CALLBACKUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案日期" Editor="datebox" FieldName="FINISHDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="FUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="CANCELDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="CANCELUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="摘要" Editor="text" FieldName="MEMO" Format="" maxlength="1024" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="列印人員" Editor="text" FieldName="PRTUSR" Format="" maxlength="6" Width="180" />
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
