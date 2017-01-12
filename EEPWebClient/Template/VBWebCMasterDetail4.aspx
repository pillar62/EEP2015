<%@ Page Language="VB" AutoEventWireup="true" CodeFile="VBWebCMasterDetail4.aspx.vb" Inherits="Template_VBWebCMasterDetail4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <InfoLight:WebDataSource ID="Master" runat="server" AutoApply="True" WebDataSetID="WMaster">
            </InfoLight:WebDataSource>
        </div>
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
            OnBeforeCommand="WebNavigator1_BeforeCommand" OnCommand="WebNavigator1_Command"
            ShowDataStyle="FormViewStyle" StatusStrip="WebStatusStrip1" Width="100%">
            <NavControls>
                <InfoLight:ControlItem ControlName="First" ControlText="掸" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/first.gif" MouseOverImageUrl="../image/uipics/first2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Previous" ControlText="掸" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/previous.gif" MouseOverImageUrl="../image/uipics/previous2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Next" ControlText="掸" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/next.gif" MouseOverImageUrl="../image/uipics/next2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Last" ControlText="ソ掸" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/last.gif" MouseOverImageUrl="../image/uipics/last2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Add" ControlText="穝糤" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Update" ControlText="э" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/edit.gif" MouseOverImageUrl="../image/uipics/edit2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Delete" ControlText="埃" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/delete.gif" MouseOverImageUrl="../image/uipics/delete2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="OK" ControlText="絋粄" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Cancel" ControlText="" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Apply" ControlText="郎" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/apply.gif" MouseOverImageUrl="../image/uipics/apply2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Abort" ControlText="斌" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/abort.gif" MouseOverImageUrl="../image/uipics/abort2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Query" ControlText="琩高" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/query.gif" MouseOverImageUrl="../image/uipics/query2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Print" ControlText="ゴ" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/print.gif" MouseOverImageUrl="../image/uipics/print2.gif"
                    Size="25" />
            </NavControls>
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
        <InfoLight:WebMultiViewCaptions ID="WebMultiViewCaptions1" runat="server" MultiViewID="MultiView1"
            TableStyle="Style3" Width="100%">
            <Captions>
                <InfoLight:WebMultiViewCaption Caption="戈聅凝" />
                <InfoLight:WebMultiViewCaption Caption="戈絪胯" />
            </Captions>
        </InfoLight:WebMultiViewCaptions>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server">
                &nbsp;</asp:View>
            <asp:View ID="View2" runat="server">
                <br />
                <br />
                <asp:ImageButton ID="ImageButton10" runat="server" CausesValidation="True" CommandName="Update"
                    ImageUrl="~/Image/UIPics/OK.gif" OnClick="ImageButton10_Click" Text="Update" />
                &nbsp;<asp:ImageButton ID="ImageButton11" runat="server" CausesValidation="False"
                    CommandName="Cancel" ImageUrl="~/Image/UIPics/Cancel.gif" OnClick="ImageButton11_Click"
                    Text="Cancel" />
            </asp:View>
        </asp:MultiView>&nbsp;<InfoLight:WebGridView ID="wgvDetail" runat="server" AbortIconUrl="../Image/UIPics/Abort.gif"
            AddIconUrl="../Image/UIPics/Add.gif" AddIndentityField="False" AllowPaging="True"
            ApplyIconUrl="../Image/UIPics/Apply.gif" AutoEmptyDataText="False" AutoGenerateColumns="False"
            BackColor="White" BorderStyle="Groove" BorderWidth="2px" ButtonTooltip="" CancelIconUrl="../Image/UIPics/Cancel.gif"
            CellPadding="1" CellSpacing="1" CreateInnerNavigator="False" DataHasChanged="False"
            EditReturn="True" EditURL="" ExpressionFieldCount="0" ForeColor="#333333" GetServerText="True"
            GridInserting="False" HeaderStyleWrap="False" InnerNavigatorLinkLabel="" InnerNavigatorShowStyle="Image"
            MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif" MouseOverAddIconUrl="../Image/UIPics/Add2.gif"
            MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif" MouseOverCancelIconUrl="../Image/UIPics/Cancel2.gif"
            MouseOverOKIconUrl="../Image/UIPics/OK2.gif" MouseOverQueryIconUrl="../Image/UIPics/Query2.gif"
            MultiCheckColumn="" MultiCheckColumnIndexToTranslate="-1" NeedExecuteAdd="True"
            OKIconUrl="../Image/UIPics/OK.gif" OnRowEditing="wgvDetail_RowEditing" OpenEditHeight="400"
            OpenEditUrlInServerMode="True" OpenEditWidth="500" QueryIconUrl="../Image/UIPics/Query.gif"
            ShowFooter="True" SkipInsert="False" TotalActive="False" TotalCaption="" ValidateFailed="False"
            Width="100%">
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
<asp:ImageButton id="ImageButton6" runat="server" Text="Update" CausesValidation="True" ImageUrl="~/Image/UIPics/OK.gif" CommandName="Update" __designer:wfdid="w23"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton7" runat="server" Text="Cancel" CausesValidation="False" ImageUrl="~/Image/UIPics/Cancel.gif" CommandName="Cancel" __designer:wfdid="w24"></asp:ImageButton> 
</edititemtemplate>
                    <footertemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" __designer:wfdid="w19" 
                            CausesValidation="False" CommandName="Add" ImageUrl="~/Image/UIPics/Add.gif" 
                            onclick="ImageButton1_Click1" Text="Edit" />
                    </footertemplate>
                    <itemtemplate>
<asp:ImageButton id="ImageButton8" runat="server" Text="Edit" CausesValidation="False" ImageUrl="~/Image/UIPics/Edit.gif" CommandName="Edit" __designer:wfdid="w21"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton9" runat="server" Text="Delete" CausesValidation="False" ImageUrl="~/Image/UIPics/Delete.gif" CommandName="Delete" __designer:wfdid="w22"></asp:ImageButton> 
</itemtemplate>
                    <headerstyle wrap="False" />
                </asp:TemplateField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" />
        </InfoLight:WebGridView>
        <InfoLight:WebFormView ID="wfvDetail" runat="server" AllValidateSucess="True" AutoEmptyDataText="False"
            CellPadding="4" DataHasChanged="False" DataSourceID="Detail" ForeColor="#333333"
            InsertBack="False" KeyValues="" LayOutColNum="2" NeedExecuteAdd="True" OldPageIndex="-1"
            ValidateFailed="False" Width="100%">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="DarkTurquoise" />
            <RowStyle BackColor="#EFF3FB" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <InsertRowStyle BackColor="DarkTurquoise" />
        </InfoLight:WebFormView>
        <InfoLight:WebValidate ID="WebDetailValidate" runat="server"></InfoLight:WebValidate>
    
    </div>
    </form>
</body>
</html>
