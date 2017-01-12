<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VBWebCMasterDetail_DG.aspx.vb" Inherits="Template_VBWebSingle" %>

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
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" ContentBackColor=""
            ContentForeColor="" Orientation="Horizontal" ShowCompany="True" ShowDate="True"
            ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="True" ShowTitle="True"
            ShowUserID="True" ShowUserName="True" StatusBackColor="" TitleBackColor="" />
        <br />
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BindingObject="wdvMaster">
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
        <InfoLight:WebDetailsView ID="wdvMaster" runat="server" AllowPaging="True" AllValidateSucess="True"
            AutoEmptyDataText="False" AutoGenerateRows="False" CellPadding="4" CreateInnerNavigator="True"
            DataHasChanged="False" ExpressionFieldCount="0" ForeColor="#333333" GetServerText="True"
            GridLines="None" InnerNavigatorLinkLabel="" InnerNavigatorShowStyle="Image" InsertBack="False"
            KeyValues="" NeedExecuteAdd="True" OldPageIndex="-1" ValidateFailed="False">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
            <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        </InfoLight:WebDetailsView>
        <InfoLight:WebGridView ID="wgvDetail" runat="server" AddIndentityField="False" AutoEmptyDataText="False"
            AutoGenerateColumns="False" BackColor="White" baseCount="0" BorderColor="#3366CC"
            BorderStyle="None" BorderWidth="1px" ButtonTooltip="" CellPadding="4" DataHasChanged="False"
            EditReturn="True" EditURL="" ExpressionFieldCount="0" GetServerText="True" GridInserting="False"
            HeaderStyleWrap="True" InnerNavigatorLinkLabel="" InnerNavigatorShowStyle="Image"
            MultiCheckColumn="" MultiCheckColumnIndexToTranslate="-1" NeedExecuteAdd="True"
            OpenEditUrlInServerMode="True" SkipInsert="False" TotalActive="False" TotalCaption=""
            ValidateFailed="False" WizardDesignMode="False">
            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
            <RowStyle BackColor="White" ForeColor="#003399" />
            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
        </InfoLight:WebGridView>
    
    </div>
    </form>
</body>
</html>
