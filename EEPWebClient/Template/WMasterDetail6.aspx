<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WMasterDetail6.aspx.cs" Inherits="Template_WMasterDetail6" Theme="ControlSkin" StylesheetTheme="ControlSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <InfoLight:WebDataSource ID="Master" runat="server" AutoApply="True" WebDataSetID="WMaster" CacheDataSet="True">
            </InfoLight:WebDataSource>
            </div>
        <InfoLight:WebDataSource ID="Detail" runat="server" MasterDataSource="Master" WebDataSetID="WMaster" CacheDataSet="True">
        </InfoLight:WebDataSource>
        <ajaxTools:AjaxScriptManager ID="AjaxScriptManager1" runat="server">
        </ajaxTools:AjaxScriptManager>
                <script language="javascript" type="text/javascript">
            var originalBodyClass = '';

            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(
function(sender, e) {
    originalBodyClass = document.body.className;
    document.body.className = 'mainDivUpdating';
});

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(
function(sender, e) {
    document.body.className = originalBodyClass;
});

        </script>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" 
                    BackColor="White" BorderColor="LightGray" BorderStyle="Groove" 
                    BorderWidth="2px" ContentBackColor="" ContentForeColor="White" Font-Bold="True" 
                    ForeColor="MediumBlue" ShowCompany="False" ShowDate="True" ShowEEPAlias="True" 
                    ShowNavigatorStatus="True" ShowSolution="False" ShowTitle="True" 
                    ShowUserID="True" ShowUserName="True" StatusBackColor="White" 
                    StatusForeColor="MediumBlue" TitleBackColor="MediumBlue" TitleForeColor="White" 
                    Width="100%" SkinID="StatusStripSkin1" />
                <br />
                <InfoLight:WebNavigator ID="WebNavigator1" runat="server" 
                    BackColor="White" BindingObject="wfvMaster" BorderColor="#E0E0E0" 
                    BorderStyle="Groove" BorderWidth="2px" OnCommand="WebNavigator1_Command" 
                    ShowDataStyle="FormViewStyle" StatusStrip="WebStatusStrip1" Width="100%" 
                    SkinID="WebNavigatorManagerSkin1">
                    <navcontrols>
                        <InfoLight:ControlItem ControlName="First" ControlText="掸" ControlType="Image" 
                            ControlVisible="True" DisenableImageUrl="../image/uipics/first3.gif" 
                            ImageUrl="../image/uipics/first.gif" 
                            MouseOverImageUrl="../image/uipics/first2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Previous" ControlText="掸" 
                            ControlType="Image" ControlVisible="True" 
                            DisenableImageUrl="../image/uipics/previous3.gif" 
                            ImageUrl="../image/uipics/previous.gif" 
                            MouseOverImageUrl="../image/uipics/previous2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Next" ControlText="掸" ControlType="Image" 
                            ControlVisible="True" DisenableImageUrl="../image/uipics/next3.gif" 
                            ImageUrl="../image/uipics/next.gif" 
                            MouseOverImageUrl="../image/uipics/next2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Last" ControlText="ソ掸" ControlType="Image" 
                            ControlVisible="True" DisenableImageUrl="../image/uipics/next3.gif" 
                            ImageUrl="../image/uipics/last.gif" 
                            MouseOverImageUrl="../image/uipics/last2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Add" ControlText="穝糤" ControlType="Image" 
                            ControlVisible="True" DisenableImageUrl="../image/uipics/add3.gif" 
                            ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif" 
                            Size="25" />
                        <InfoLight:ControlItem ControlName="Update" ControlText="э" 
                            ControlType="Image" ControlVisible="True" 
                            DisenableImageUrl="../image/uipics/edit3.gif" 
                            ImageUrl="../image/uipics/edit.gif" 
                            MouseOverImageUrl="../image/uipics/edit2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Delete" ControlText="埃" 
                            ControlType="Image" ControlVisible="True" 
                            DisenableImageUrl="../image/uipics/delete3.gif" 
                            ImageUrl="../image/uipics/delete.gif" 
                            MouseOverImageUrl="../image/uipics/delete2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="OK" ControlText="絋粄" ControlType="Image" 
                            ControlVisible="True" DisenableImageUrl="../image/uipics/ok3.gif" 
                            ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif" 
                            Size="25" />
                        <InfoLight:ControlItem ControlName="Cancel" ControlText="" 
                            ControlType="Image" ControlVisible="True" 
                            DisenableImageUrl="../image/uipics/cancel3.gif" 
                            ImageUrl="../image/uipics/cancel.gif" 
                            MouseOverImageUrl="../image/uipics/cancel2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Apply" ControlText="郎" ControlType="Image" 
                            ControlVisible="True" DisenableImageUrl="../image/uipics/apply3.gif" 
                            ImageUrl="../image/uipics/apply.gif" 
                            MouseOverImageUrl="../image/uipics/apply2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Abort" ControlText="斌" ControlType="Image" 
                            ControlVisible="True" DisenableImageUrl="../image/uipics/abort3.gif" 
                            ImageUrl="../image/uipics/abort.gif" 
                            MouseOverImageUrl="../image/uipics/abort2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Query" ControlText="琩高" ControlType="Image" 
                            ControlVisible="True" DisenableImageUrl="../image/uipics/query3.gif" 
                            ImageUrl="../image/uipics/query.gif" 
                            MouseOverImageUrl="../image/uipics/query2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Print" ControlText="ゴ" ControlType="Image" 
                            ControlVisible="True" DisenableImageUrl="../image/uipics/print3.gif" 
                            ImageUrl="../image/uipics/print.gif" 
                            MouseOverImageUrl="../image/uipics/print2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Export" ControlType="Image" 
                            ControlVisible="True" DisenableImageUrl="../Image/UIPics/Export3.gif" 
                            ImageUrl="../Image/UIPics/Export.gif" 
                            MouseOverImageUrl="../Image/UIPics/Export2.gif" Size="25" />
                    </navcontrols>
                    <navstates>
                        <InfoLight:WebNavigatorStateItem StateText="Initial" />
                        <InfoLight:WebNavigatorStateItem StateText="Browsed" />
                        <InfoLight:WebNavigatorStateItem StateText="Inserting" />
                        <InfoLight:WebNavigatorStateItem StateText="Editing" />
                        <InfoLight:WebNavigatorStateItem StateText="Applying" />
                        <InfoLight:WebNavigatorStateItem StateText="Changing" />
                        <InfoLight:WebNavigatorStateItem StateText="Querying" />
                        <InfoLight:WebNavigatorStateItem StateText="Printing" />
                    </navstates>
                </InfoLight:WebNavigator>
                <br />
                <br />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="AjaxModalPanel1$ButtonOK" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="AjaxModalPanel1$ButtonClose" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <InfoLight:WebFormView ID="wfvMaster" runat="server" AllowPaging="True" 
            DataSourceID="Master" Height="87px" LayOutColNum="2" OnAfterInsertLocate="wfvMaster_AfterInsertLocate"
            OnCanceled="wfvMaster_Canceled" 
            OnPageIndexChanged="wfvMaster_PageIndexChanged" Width="100%" 
            SkinID="FormViewSkin1">
            <InsertRowStyle BackColor="LightCyan" ForeColor="Blue" />
        </InfoLight:WebFormView>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="AjaxModalPanel1$ButtonOK" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="UpdatePanel1$WebNavigator1" EventName="Command" />
                <asp:AsyncPostBackTrigger ControlID="wfvMaster" EventName="PageIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <InfoLight:WebGridView ID="wgvDetail" runat="server" AbortIconUrl="../Image/UIPics/Abort.gif" DataSourceID="Detail"
            AddIconUrl="../Image/UIPics/Add.gif" ApplyIconUrl="../Image/UIPics/Apply.gif"
            CancelIconUrl="../Image/UIPics/Cancel.gif"
            EditURLPanel="AjaxModalPanel1" HeaderStyleWrap="False" MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif"
            MouseOverAddIconUrl="../Image/UIPics/Add2.gif" MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif"
            MouseOverCancelIconUrl="../Image/UIPics/Cancel2.gif" MouseOverOKIconUrl="../Image/UIPics/OK2.gif"
            MouseOverQueryIconUrl="../Image/UIPics/Query2.gif" OKIconUrl="../Image/UIPics/OK.gif"
            QueryIconUrl="../Image/UIPics/Query.gif" Width="100%" 
            OnRowCommand="wgvDetail_RowCommand" SkinID="GridViewSkin1">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <edititemtemplate>
<asp:ImageButton id="ImageButton1" runat="server" Text="Update" __designer:wfdid="w7" CausesValidation="True" ImageUrl="~/Image/UIPics/OK.gif" CommandName="Update"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton2" runat="server" Text="Cancel" __designer:wfdid="w8" CausesValidation="False" ImageUrl="~/Image/UIPics/Cancel.gif" CommandName="Cancel"></asp:ImageButton> 
</edititemtemplate>
                    <itemtemplate>
<asp:ImageButton id="ImageButton3" runat="server" Text="Edit" __designer:wfdid="w5" CausesValidation="False" ImageUrl="~/Image/UIPics/Edit.gif" CommandName="AjaxEdit"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton4" runat="server" Text="Delete" __designer:wfdid="w6" CausesValidation="False" ImageUrl="~/Image/UIPics/Delete.gif" CommandName="Delete"></asp:ImageButton> 
</itemtemplate>
                    <headerstyle wrap="False" />
                </asp:TemplateField>
            </Columns>
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
            <AlternatingRowStyle BorderColor="White" />
        </InfoLight:WebGridView>
        <ajaxTools:AjaxModalPanel ID="AjaxModalPanel1" runat="server" Width="500px" DataContainer="wfvDetail" TriggerUpdatePanel="UpdatePanel3">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <br />
                <asp:Button ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" Text="OK" />&nbsp;<asp:Button
                    ID="ButtonClose" runat="server" OnClick="buttonClose_Click" Text="Close" />
            </ContentTemplate>
        </asp:UpdatePanel>
        </ajaxTools:AjaxModalPanel>
        <InfoLight:WebFormView ID="wfvDetail" runat="server" DataSourceID="Detail" 
            LayOutColNum="2" Width="100%" SkinID="FormViewSkin1">
            <InsertRowStyle BackColor="DarkTurquoise" />
        </InfoLight:WebFormView>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
            <ProgressTemplate>
                <ajaxTools:UpdateProgressPanel ID="UpdateProgressPanel1" runat="server" Height="200px"
                    ImageUrl="~/Image/Ajax/wait.gif" Width="200px" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        &nbsp;</div>
    </form>
</body>
</html>
