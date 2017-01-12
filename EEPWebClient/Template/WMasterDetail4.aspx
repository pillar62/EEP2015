<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WMasterDetail4.aspx.cs" Inherits="Template_WMasterDetail4"  Theme="ControlSkin" StylesheetTheme="ControlSkin"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <InfoLight:webdatasource id="Master" runat="server" autoapply="True" WebDataSetID="WMaster"></InfoLight:webdatasource>
    
    </div>
        <InfoLight:webdatasource id="Detail" runat="server" MasterDataSource="Master" WebDataSetID="WMaster"></InfoLight:webdatasource>
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" BackColor="White"
            BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px" ContentBackColor=""
            ContentForeColor="White" Font-Bold="True" ForeColor="MediumBlue" ShowCompany="False"
            ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
            ShowTitle="True" ShowUserID="True" ShowUserName="True" Width="100%" 
        StatusBackColor="White" StatusForeColor="MediumBlue" 
        TitleBackColor="MediumBlue" TitleForeColor="White" SkinID="StatusStripSkin1" />
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BindingObject="wfvMaster"
            OnCommand="WebNavigator1_Command" ShowDataStyle="FormViewStyle" 
        Width="100%" BackColor="White" BorderColor="#E0E0E0" BorderStyle="Groove" 
        BorderWidth="2px" StatusStrip="WebStatusStrip1" 
        OnBeforeCommand="WebNavigator1_BeforeCommand" SkinID="WebNavigatorManagerSkin1">
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
        <InfoLight:WebFormView ID="wfvMaster" runat="server" 
        OnPageIndexChanged="wfvMaster_PageIndexChanged" AllowPaging="True" 
        DataSourceID="Master" LayOutColNum="2" 
        OnAfterInsertLocate="wfvMaster_AfterInsertLocate" 
        OnCanceled="wfvMaster_Canceled" Height="87px" Width="100%" 
        SkinID="FormViewSkin1">
            <InsertRowStyle BackColor="LightCyan" ForeColor="Blue" />
        </InfoLight:WebFormView>
        <InfoLight:WebMultiViewCaptions ID="WebMultiViewCaptions1" runat="server" MultiViewID="MultiView1"
            Width="100%">
            <Captions>
                <InfoLight:WebMultiViewCaption Caption="流覽資料" />
                <InfoLight:WebMultiViewCaption Caption="編輯資料" />
            </Captions>
        </InfoLight:WebMultiViewCaptions>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server">
                <br />
                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Add"
                    ImageUrl="~/Image/UIPics/Add.gif" OnClick="ImageButton1_Click1" Text="Edit" /></asp:View>
            <asp:View ID="View2" runat="server">
                <br />
                <br />
                <asp:ImageButton ID="ImageButton10" runat="server" 
                    CausesValidation="True" CommandName="Update" ImageUrl="~/Image/UIPics/OK.gif" 
                    onclick="ImageButton10_Click" Text="Update" />
                &nbsp;<asp:ImageButton ID="ImageButton11" runat="server" 
                    CausesValidation="False" CommandName="Cancel" 
                    ImageUrl="~/Image/UIPics/Cancel.gif" onclick="ImageButton11_Click" 
                    Text="Cancel" />
            </asp:View>
        </asp:MultiView>&nbsp;<InfoLight:WebGridView ID="wgvDetail" runat="server" AbortIconUrl="../Image/UIPics/Abort.gif"
            AddIconUrl="../Image/UIPics/Add.gif"
            ApplyIconUrl="../Image/UIPics/Apply.gif" 
        CancelIconUrl="../Image/UIPics/Cancel.gif" DataSourceID="Detail" HeaderStyleWrap="False"
            MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif" MouseOverAddIconUrl="../Image/UIPics/Add2.gif"
            MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif" MouseOverCancelIconUrl="../Image/UIPics/Cancel2.gif"
            MouseOverOKIconUrl="../Image/UIPics/OK2.gif" MouseOverQueryIconUrl="../Image/UIPics/Query2.gif"
            OKIconUrl="../Image/UIPics/OK.gif" QueryIconUrl="../Image/UIPics/Query.gif" 
        Width="100%" OnRowEditing="wgvDetail_RowEditing" 
        CreateInnerNavigator="False" SkinID="GridViewSkin1">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <edititemtemplate>
<asp:ImageButton id="ImageButton6" runat="server" Text="Update" CausesValidation="True" ImageUrl="~/Image/UIPics/OK.gif" CommandName="Update" __designer:wfdid="w23"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton7" runat="server" Text="Cancel" CausesValidation="False" ImageUrl="~/Image/UIPics/Cancel.gif" CommandName="Cancel" __designer:wfdid="w24"></asp:ImageButton> 
</edititemtemplate>
                    <headerstyle wrap="False" />
                    <itemtemplate>
<asp:ImageButton id="ImageButton8" runat="server" Text="Edit" ImageUrl="~/Image/UIPics/Edit.gif" CommandName="Edit" CausesValidation="False" __designer:wfdid="w1"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton9" runat="server" Text="Delete" ImageUrl="~/Image/UIPics/Delete.gif" CommandName="Delete" CausesValidation="False" __designer:wfdid="w2"></asp:ImageButton> 
</itemtemplate>
                </asp:TemplateField>
            </Columns>
        </InfoLight:WebGridView>
        <InfoLight:WebFormView ID="wfvDetail" runat="server" DataSourceID="Detail" 
        LayOutColNum="2" Width="100%" SkinID="FormViewSkin1">
            <InsertRowStyle BackColor="DarkTurquoise" />
        </InfoLight:WebFormView>
    </form>
</body>
</html>
