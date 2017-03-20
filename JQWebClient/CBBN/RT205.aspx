<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT205.aspx.cs" Inherits="Template_JQuerySingle1" %>

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
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT205.cmRT205" runat="server" AutoApply="True"
                DataMember="cmRT205" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="客訴資料維護">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客訴單號(yyymmddxxx)" Editor="text" FieldName="caseno" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案別 (P5)" Editor="text" FieldName="comtype" Format="" MaxLength="1" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="consignee" Editor="text" FieldName="consignee" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="leader" Editor="text" FieldName="leader" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區序號" Editor="text" FieldName="comq1" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="comn" Editor="text" FieldName="comn" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="dropdat" Editor="datebox" FieldName="dropdat" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報修聯絡人" Editor="text" FieldName="faqman" Format="" MaxLength="50" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="受理時間" Editor="text" FieldName="rcvdat" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案時間" Editor="datebox" FieldName="closedat" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="c_caseno" Editor="numberbox" FieldName="c_caseno" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="finishnum" Editor="text" FieldName="finishnum" Format="" MaxLength="0" Visible="true" Width="120" />
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

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="客訴資料維護">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="cmRT205" HorizontalColumnsCount="2" RemoteName="sRT205.cmRT205" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="客訴單號(yyymmddxxx)" Editor="text" FieldName="caseno" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案別 (P5)" Editor="text" FieldName="comtype" Format="" maxlength="1" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="consignee" Editor="text" FieldName="consignee" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="leader" Editor="text" FieldName="leader" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="text" FieldName="comq1" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="comn" Editor="text" FieldName="comn" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="dropdat" Editor="datebox" FieldName="dropdat" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="報修聯絡人" Editor="text" FieldName="faqman" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="受理時間" Editor="text" FieldName="rcvdat" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案時間" Editor="datebox" FieldName="closedat" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="c_caseno" Editor="numberbox" FieldName="c_caseno" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="finishnum" Editor="text" FieldName="finishnum" Format="" maxlength="0" Width="180" />
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
