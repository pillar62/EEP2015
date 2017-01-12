<%@ Page Language="VB" AutoEventWireup="true" CodeFile="VBWebCMasterDetail_FG.aspx.vb" Inherits="Template_VBWebSingle" %>

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
        <InfoLight:WebDataSource ID="Detail" runat="server" MasterDataSource="Master" WebDataSetID="WMaster">
        </InfoLight:WebDataSource>
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" BackColor="PowderBlue"
            BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px" ContentBackColor=""
            ContentForeColor="White" Font-Bold="True" ForeColor="MediumBlue" ShowCompany="False"
            ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
            ShowTitle="True" ShowUserID="True" ShowUserName="True" StatusBackColor="White"
            StatusForeColor="MediumBlue" TitleBackColor="MediumBlue" TitleForeColor="White"
            Width="100%" />
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BackColor="PowderBlue"
            BindingObject="wfvMaster" BorderColor="#E0E0E0" BorderStyle="Groove" BorderWidth="2px"
            OnCommand="WebNavigator1_Command" ShowDataStyle="FormViewStyle" StatusStrip="WebStatusStrip1"
            Width="100%">
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
                <InfoLight:ControlItem ControlName="Update" ControlText="э" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/edit.gif" MouseOverImageUrl="../image/uipics/edit2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/edit3.gif" />
                <InfoLight:ControlItem ControlName="Delete" ControlText="埃" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/delete.gif" MouseOverImageUrl="../image/uipics/delete2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/delete3.gif" />
                <InfoLight:ControlItem ControlName="OK" ControlText="絋粄" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/ok3.gif" />
                <InfoLight:ControlItem ControlName="Cancel" ControlText="" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/cancel3.gif" />
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
        <InfoLight:WebFormView ID="wfvMaster" runat="server" AllowPaging="True" AllValidateSucess="True"
            AutoEmptyDataText="False" BackColor="Silver" BorderColor="White" BorderStyle="Groove"
            BorderWidth="2px" CellPadding="4" DataHasChanged="False" DataSourceID="Master"
            ForeColor="#333333" Height="87px" InsertBack="False" KeyValues="" LayOutColNum="2"
            NeedExecuteAdd="True" OldPageIndex="-1" OnAfterInsertLocate="wfvMaster_AfterInsertLocate"
            OnCanceled="wfvMaster_Canceled" OnPageIndexChanged="wfvMaster_PageIndexChanged"
            ValidateFailed="False" Width="100%">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="LightCyan" ForeColor="Blue" />
            <RowStyle BackColor="#EFF3FB" ForeColor="Navy" />
            <PagerStyle BackColor="PowderBlue" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <InsertRowStyle BackColor="LightCyan" ForeColor="Blue" />
        </InfoLight:WebFormView>
        <InfoLight:WebGridView ID="wgvDetail" runat="server" AbortIconUrl="../Image/UIPics/Abort.gif"
            AddIconUrl="../Image/UIPics/Add.gif" AddIndentityField="False" AllowPaging="True"
            ApplyIconUrl="../Image/UIPics/Apply.gif" AutoEmptyDataText="False" AutoGenerateColumns="False"
            BackColor="White" BorderStyle="Groove" BorderWidth="2px" ButtonTooltip="" CancelIconUrl="../Image/UIPics/Cancel.gif"
            CellPadding="1" CellSpacing="1" DataHasChanged="False" DataSourceID="Detail"
            EditReturn="True" EditURL="" ExpressionFieldCount="0" ForeColor="#333333" GetServerText="True"
            GridInserting="False" HeaderStyleWrap="False" InnerNavigatorLinkLabel="" InnerNavigatorShowStyle="Image"
            MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif" MouseOverAddIconUrl="../Image/UIPics/Add2.gif"
            MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif" MouseOverCancelIconUrl="../Image/UIPics/Cancel2.gif"
            MouseOverOKIconUrl="../Image/UIPics/OK2.gif" MouseOverQueryIconUrl="../Image/UIPics/Query2.gif"
            MultiCheckColumn="" MultiCheckColumnIndexToTranslate="-1" NeedExecuteAdd="True"
            OKIconUrl="../Image/UIPics/OK.gif" OpenEditHeight="400" OpenEditUrlInServerMode="True"
            OpenEditWidth="500" QueryIconUrl="../Image/UIPics/Query.gif" SkipInsert="False"
            TotalActive="False" TotalCaption="" ValidateFailed="False" Width="100%">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <EditRowStyle BackColor="#2461BF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="PowderBlue" ForeColor="Blue" HorizontalAlign="Center" />
            <HeaderStyle BackColor="DarkTurquoise" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <edititemtemplate>
<asp:ImageButton runat="server" Text="Update" CommandName="Update" ImageUrl="~/Image/UIPics/OK.gif" CausesValidation="True" id="ImageButton6"></asp:ImageButton>&nbsp;<asp:ImageButton runat="server" Text="Cancel" CommandName="Cancel" ImageUrl="~/Image/UIPics/Cancel.gif" CausesValidation="False" id="ImageButton7"></asp:ImageButton>
</edititemtemplate>
                    <headerstyle wrap="False" />
                    <itemtemplate>
<asp:ImageButton runat="server" Text="Edit" CommandName="Edit" ImageUrl="~/Image/UIPics/Edit.gif" CausesValidation="False" id="ImageButton8"></asp:ImageButton>&nbsp;<asp:ImageButton runat="server" Text="Delete" CommandName="Delete" ImageUrl="~/Image/UIPics/Delete.gif" CausesValidation="False" id="ImageButton9"></asp:ImageButton>
</itemtemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" />
        </InfoLight:WebGridView>
        &nbsp;
    
    </div>
    </form>
</body>
</html>
