<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VBWebCMasterDetail_FG.aspx.vb" Inherits="Template_VBWebSingle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <InfoLight:WebDataSource ID="Master" runat="server" WebDataSetID="WMaster">
        </InfoLight:WebDataSource>
        <InfoLight:WebDataSource ID="Detail" runat="server" MasterDataSource="Master" WebDataSetID="WMaster">
        </InfoLight:WebDataSource>
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" ShowCompany="True"
            ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="True"
            ShowTitle="True" ShowUserID="True" ShowUserName="True" />
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BindingObject="wfvMaster">
            <NavControls>
                <InfoLight:ControlItem ControlName="First" ControlText="first" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/first.gif" MouseOverImageUrl="../image/uipics/first2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Previous" ControlText="previous" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/previous.gif" MouseOverImageUrl="../image/uipics/previous2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Next" ControlText="next" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/next.gif" MouseOverImageUrl="../image/uipics/next2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Last" ControlText="last" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/last.gif" MouseOverImageUrl="../image/uipics/last2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Add" ControlText="add" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Update" ControlText="update" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/edit.gif" MouseOverImageUrl="../image/uipics/edit2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Delete" ControlText="delete" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/delete.gif" MouseOverImageUrl="../image/uipics/delete2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="OK" ControlText="ok" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Cancel" ControlText="cancel" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Apply" ControlText="apply" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/apply.gif" MouseOverImageUrl="../image/uipics/apply2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Abort" ControlText="abort" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/abort.gif" MouseOverImageUrl="../image/uipics/abort2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Query" ControlText="query" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/query.gif" MouseOverImageUrl="../image/uipics/query2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Print" ControlText="print" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/print.gif" MouseOverImageUrl="../image/uipics/print2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Export" ControlText="export" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/export.gif" MouseOverImageUrl="../image/uipics/export2.gif"
                    Size="25" />
            </NavControls>
        </InfoLight:WebNavigator>
        <InfoLight:WebFormView ID="wfvMaster" runat="server" AllValidateSucess="True" AutoEmptyDataText="False"
            CellPadding="4" DataHasChanged="False" ForeColor="#333333" InsertBack="False"
            KeyValues="" LayOutColNum="1" NeedExecuteAdd="True" OldPageIndex="-1" ValidateFailed="False">
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <RowStyle BackColor="#E3EAEB" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        </InfoLight:WebFormView>
        <InfoLight:WebGridView ID="wgvDetail" runat="server" AddIndentityField="False" AutoEmptyDataText="False"
            AutoGenerateColumns="False" BackColor="White" baseCount="0" BorderColor="#CCCCCC"
            BorderStyle="None" BorderWidth="1px" ButtonTooltip="" CellPadding="3" DataHasChanged="False"
            EditReturn="True" EditURL="" ExpressionFieldCount="0" GetServerText="True" GridInserting="False"
            HeaderStyleWrap="True" InnerNavigatorLinkLabel="" InnerNavigatorShowStyle="Image"
            MultiCheckColumn="" MultiCheckColumnIndexToTranslate="-1" NeedExecuteAdd="True"
            OpenEditUrlInServerMode="True" SkipInsert="False" TotalActive="False" TotalCaption=""
            ValidateFailed="False" WizardDesignMode="False">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <RowStyle ForeColor="#000066" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        </InfoLight:WebGridView>
    
    </div>
    </form>
</body>
</html>
