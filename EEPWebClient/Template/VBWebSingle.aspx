<%@ Page Language="VB" AutoEventWireup="true" CodeFile="VBWebSingle.aspx.vb" Inherits="Template_VBWebSingle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <InfoLight:WebDataSource ID="Master" runat="server" AutoApply="True" WebDataSetID="WMaster">
        </InfoLight:WebDataSource>
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" BackColor="PowderBlue"
            BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px" ContentBackColor=""
            ContentForeColor="White" Font-Bold="True" ForeColor="MediumBlue" ShowCompany="False"
            ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
            ShowTitle="True" ShowUserID="True" ShowUserName="True" StatusForeColor="MediumBlue"
            TitleBackColor="MediumBlue" TitleForeColor="White" Width="100%" />
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BackColor="PowderBlue"
            BindingObject="wgvMaster" BorderColor="Silver" BorderStyle="Groove" BorderWidth="2px"
            Height="22px" StatusStrip="WebStatusStrip1" Width="100%">
            <NavControls>
                <InfoLight:ControlItem ControlName="First" ControlText="掸" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/first.gif" MouseOverImageUrl="../image/uipics/first2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/first3.gif" />
                <InfoLight:ControlItem ControlName="Previous" ControlText="掸" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/previous.gif" MouseOverImageUrl="../image/uipics/previous2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/previous3.gif" />
                <InfoLight:ControlItem ControlName="Next" ControlText="掸" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/next.gif" MouseOverImageUrl="../image/uipics/next2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/next3.gif" />
                <InfoLight:ControlItem ControlName="Last" ControlText="ソ掸" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/last.gif" MouseOverImageUrl="../image/uipics/last2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/last3.gif" />
                <InfoLight:ControlItem ControlName="Add" ControlText="穝糤" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/add3.gif" />
                <InfoLight:ControlItem ControlName="Apply" ControlText="郎" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/apply.gif" MouseOverImageUrl="../image/uipics/apply2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/apply3.gif" />
                <InfoLight:ControlItem ControlName="Abort" ControlText="斌" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/abort.gif" MouseOverImageUrl="../image/uipics/abort2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/abort3.gif" />
                <InfoLight:ControlItem ControlName="Query" ControlText="琩高" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/query.gif" MouseOverImageUrl="../image/uipics/query2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/query3.gif" />
                <InfoLight:ControlItem ControlName="Print" ControlText="ゴ" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/print.gif" MouseOverImageUrl="../image/uipics/print2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/print3.gif" />
                <InfoLight:ControlItem ControlName="Export" ControlType="Image" ControlVisible="True"
                    DisenableImageUrl="../Image/UIPics/Export3.gif" ImageUrl="../Image/UIPics/Export.gif"
                    MouseOverImageUrl="../Image/UIPics/Export2.gif" Size="25" />
            </NavControls>
            <NavStates>
                <InfoLight:WebNavigatorStateItem StateText="Initial" />
                <InfoLight:WebNavigatorStateItem StateText="Browsed" />
                <InfoLight:WebNavigatorStateItem StateText="Inserting" />
                <InfoLight:WebNavigatorStateItem StateText="Editing" />
                <InfoLight:WebNavigatorStateItem StateText="Applying" />
                <InfoLight:WebNavigatorStateItem StateText="Changing" />
                <InfoLight:WebNavigatorStateItem StateText="Querying" />
                <InfoLight:WebNavigatorStateItem StateText="Printing" />
            </NavStates>
        </InfoLight:WebNavigator>
        <InfoLight:WebGridView ID="wgvMaster" runat="server" AbortIconUrl="../Image/UIPics/Abort.gif"
            AddIconUrl="../Image/UIPics/Add.gif" AllowPaging="True" ApplyIconUrl="../Image/UIPics/Apply.gif"
            AutoGenerateColumns="False" BackColor="White" BorderColor="White" BorderStyle="Groove"
            BorderWidth="2px" CancelIconUrl="../Image/UIPics/Cancel.gif" CellPadding="1"
            CreateInnerNavigator="True" DataSourceID="Master" ForeColor="#333333" HeaderStyleWrap="False"
            InnerNavigatorShowStyle="Image" MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif"
            MouseOverAddIconUrl="../Image/UIPics/Add2.gif" MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif"
            MouseOverCancelIconUrl="../Image/UIPics/Cancel2.gif" MouseOverOKIconUrl="../Image/UIPics/OK2.gif"
            MouseOverQueryIconUrl="../Image/UIPics/Query2.gif" OKIconUrl="../Image/UIPics/OK.gif"
            QueryIconUrl="../Image/UIPics/Query.gif" Width="100%">
            <PagerSettings Mode="NumericFirstLast" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <NavControls>
                <InfoLight:ControlItem ControlName="Add" ControlText="add" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="OK" ControlText="Insert" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Cancel" ControlText="cancel" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif"
                    Size="25" />
            </NavControls>
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <edititemtemplate>
<asp:ImageButton id="ImageButton1" runat="server" Text="Update" CausesValidation="True" ImageUrl="~/Image/UIPics/OK.gif" CommandName="Update" __designer:wfdid="w3"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton2" runat="server" Text="Cancel" CausesValidation="False" ImageUrl="~/Image/UIPics/Cancel.gif" CommandName="Cancel" __designer:wfdid="w4"></asp:ImageButton>
</edititemtemplate>
                    <headerstyle wrap="False" />
                    <itemtemplate>
<asp:ImageButton id="ImageButton3" runat="server" Text="Edit" CausesValidation="False" ImageUrl="~/Image/UIPics/Edit.gif" CommandName="Edit" __designer:wfdid="w1"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton4" runat="server" Text="Delete" CausesValidation="False" ImageUrl="~/Image/UIPics/Delete.gif" CommandName="Delete" __designer:wfdid="w2"></asp:ImageButton>
</itemtemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle BackColor="#EFF3FB" />
            <EditRowStyle BackColor="#2461BF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="LightBlue" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="DeepSkyBlue" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" BorderColor="White" />
        </InfoLight:WebGridView>
        &nbsp;</div>
    </form>
</body>
</html>
