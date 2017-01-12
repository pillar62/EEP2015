<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VBWebSingle.aspx.vb" Inherits="Template_VBWebSingle" %>

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
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" ShowCompany="True"
            ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="True"
            ShowTitle="True" ShowUserID="True" ShowUserName="True" />
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BindingObject="wgvMaster">
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
        <InfoLight:WebGridView ID="wgvMaster" runat="server" AddIndentityField="False" AutoEmptyDataText="False"
            AutoGenerateColumns="False" baseCount="0" ButtonTooltip="" CellPadding="4" DataHasChanged="False"
            EditReturn="True" EditURL="" ExpressionFieldCount="0" ForeColor="#333333" GetServerText="True"
            GridInserting="False" GridLines="None" HeaderStyleWrap="True" InnerNavigatorLinkLabel=""
            InnerNavigatorShowStyle="Image" MultiCheckColumn="" MultiCheckColumnIndexToTranslate="-1"
            NeedExecuteAdd="True" OpenEditUrlInServerMode="True" SkipInsert="False" TotalActive="False"
            TotalCaption="" ValidateFailed="False" WizardDesignMode="False">
            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
            <AlternatingRowStyle BackColor="White" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        </InfoLight:WebGridView>
    
    </div>
    </form>
</body>
</html>
