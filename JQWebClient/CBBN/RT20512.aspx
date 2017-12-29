<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT20512.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" UseFlow="False" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT2051.RTFaqM" runat="server" AutoApply="True"
                DataMember="RTFaqM" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="客訴資料維護" AlwaysClose="True" DeleteCommandVisible="False" UpdateCommandVisible="False" ViewCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客訴單號(yyymmddxxx)" Editor="text" FieldName="CASENO" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案別 (P5)" Editor="text" FieldName="COMTYPE" Format="" MaxLength="1" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="社區序號" Editor="numberbox" FieldName="COMQ1" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線序號" Editor="text" FieldName="LINEQ1" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶編號" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶項次" Editor="text" FieldName="ENTRYNO" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報修聯絡人" Editor="text" FieldName="FAQMAN" Format="" MaxLength="50" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡電話" Editor="text" FieldName="TEL" Format="" MaxLength="50" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="手機" Editor="text" FieldName="MOBILE" Format="" MaxLength="50" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報修原因(P7)" Editor="text" FieldName="FAQREASON" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="進出線(I:用戶來電, O:客服Call Out)" Editor="text" FieldName="IOBOUND" Format="" MaxLength="1" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" MaxLength="1500" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="受理人員" Editor="text" FieldName="RCVUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="受理時間" Editor="datebox" FieldName="RCVDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="CLOSEUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案時間" Editor="datebox" FieldName="CLOSEDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="UUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動時間" Editor="datebox" FieldName="UDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="CANCELUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢時間" Editor="datebox" FieldName="CANCELDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶來電詢問方案(Q2)" Editor="text" FieldName="ASKCASE" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶來源別 (Q3)" Editor="text" FieldName="CUSTSRC" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="comtype1" Editor="text" FieldName="comtype1" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="COMQ11" Editor="numberbox" FieldName="COMQ11" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="LINEQ11" Editor="numberbox" FieldName="LINEQ11" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="CUSID1" Editor="text" FieldName="CUSID1" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="comtypenc" Editor="text" FieldName="comtypenc" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="belongnc" Editor="text" FieldName="belongnc" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="salesnc" Editor="text" FieldName="salesnc" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="comq" Editor="text" FieldName="comq" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="comn" Editor="text" FieldName="comn" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="LINETEL" Editor="text" FieldName="LINETEL" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="gateway" Editor="text" FieldName="gateway" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="CMTYIP" Editor="text" FieldName="CMTYIP" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="LINERATE" Editor="text" FieldName="LINERATE" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="ARRIVEDAT" Editor="datebox" FieldName="ARRIVEDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="rcomdrop" Editor="datebox" FieldName="rcomdrop" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="idslamip" Editor="text" FieldName="idslamip" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="cusnc" Editor="text" FieldName="cusnc" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="contacttel" Editor="text" FieldName="contacttel" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="companytel" Editor="text" FieldName="companytel" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="raddr" Editor="text" FieldName="raddr" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="CUSTIP" Editor="text" FieldName="CUSTIP" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="CASEKIND" Editor="text" FieldName="CASEKIND" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="paycycle" Editor="text" FieldName="paycycle" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="paytype" Editor="text" FieldName="paytype" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="overdue" Editor="text" FieldName="overdue" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="freecode" Editor="text" FieldName="freecode" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="docketdat" Editor="datebox" FieldName="docketdat" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="strbillingdat" Editor="datebox" FieldName="strbillingdat" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="newbillingdat" Editor="datebox" FieldName="newbillingdat" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="duedat" Editor="datebox" FieldName="duedat" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="dropdat" Editor="datebox" FieldName="dropdat" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="canceldat1" Editor="datebox" FieldName="canceldat1" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="secondcase" Editor="text" FieldName="secondcase" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="nciccusno" Editor="text" FieldName="nciccusno" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="Sp499cons" Editor="text" FieldName="Sp499cons" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="WtlApplyDat" Editor="datebox" FieldName="WtlApplyDat" Format="" Visible="true" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="True"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="客訴資料維護">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTFaqM" HorizontalColumnsCount="2" RemoteName="sRT2051.RTFaqM" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="客訴單號(yyymmddxxx)" Editor="text" FieldName="CASENO" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案別 (P5)" Editor="text" FieldName="COMTYPE" Format="" maxlength="1" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="numberbox" FieldName="COMQ1" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線序號" Editor="text" FieldName="LINEQ1" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶編號" Editor="text" FieldName="CUSID" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶項次" Editor="text" FieldName="ENTRYNO" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="報修聯絡人" Editor="text" FieldName="FAQMAN" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="聯絡電話" Editor="text" FieldName="TEL" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="手機" Editor="text" FieldName="MOBILE" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="報修原因(P7)" Editor="text" FieldName="FAQREASON" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="進出線(I:用戶來電, O:客服Call Out)" Editor="text" FieldName="IOBOUND" Format="" maxlength="1" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" maxlength="1500" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="受理人員" Editor="text" FieldName="RCVUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="受理時間" Editor="datebox" FieldName="RCVDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="CLOSEUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案時間" Editor="datebox" FieldName="CLOSEDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="UUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動時間" Editor="datebox" FieldName="UDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="CANCELUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢時間" Editor="datebox" FieldName="CANCELDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶來電詢問方案(Q2)" Editor="text" FieldName="ASKCASE" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶來源別 (Q3)" Editor="text" FieldName="CUSTSRC" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="comtype1" Editor="text" FieldName="comtype1" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="COMQ11" Editor="numberbox" FieldName="COMQ11" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="LINEQ11" Editor="numberbox" FieldName="LINEQ11" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="CUSID1" Editor="text" FieldName="CUSID1" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="comtypenc" Editor="text" FieldName="comtypenc" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="belongnc" Editor="text" FieldName="belongnc" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="salesnc" Editor="text" FieldName="salesnc" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="comq" Editor="text" FieldName="comq" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="comn" Editor="text" FieldName="comn" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="LINETEL" Editor="text" FieldName="LINETEL" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="gateway" Editor="text" FieldName="gateway" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="CMTYIP" Editor="text" FieldName="CMTYIP" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="LINERATE" Editor="text" FieldName="LINERATE" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="ARRIVEDAT" Editor="datebox" FieldName="ARRIVEDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="rcomdrop" Editor="datebox" FieldName="rcomdrop" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="idslamip" Editor="text" FieldName="idslamip" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="cusnc" Editor="text" FieldName="cusnc" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="contacttel" Editor="text" FieldName="contacttel" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="companytel" Editor="text" FieldName="companytel" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="raddr" Editor="text" FieldName="raddr" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="CUSTIP" Editor="text" FieldName="CUSTIP" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="CASEKIND" Editor="text" FieldName="CASEKIND" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="paycycle" Editor="text" FieldName="paycycle" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="paytype" Editor="text" FieldName="paytype" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="overdue" Editor="text" FieldName="overdue" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="freecode" Editor="text" FieldName="freecode" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="docketdat" Editor="datebox" FieldName="docketdat" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="strbillingdat" Editor="datebox" FieldName="strbillingdat" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="newbillingdat" Editor="datebox" FieldName="newbillingdat" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="duedat" Editor="datebox" FieldName="duedat" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="dropdat" Editor="datebox" FieldName="dropdat" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="canceldat1" Editor="datebox" FieldName="canceldat1" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="secondcase" Editor="text" FieldName="secondcase" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="nciccusno" Editor="text" FieldName="nciccusno" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="Sp499cons" Editor="text" FieldName="Sp499cons" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="WtlApplyDat" Editor="datebox" FieldName="WtlApplyDat" Format="" Width="180" />
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
