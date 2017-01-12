<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WMasterDetail2.aspx.cs" Inherits="Template_WMasterDetail2" Theme="ControlSkin" StylesheetTheme="ControlSkin"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
   </head>
<body>
    <form id="form1" runat="server">
        <InfoLight:webdatasource id="Master" runat="server" webdatasetid="WMaster" AutoApply="True"></InfoLight:webdatasource>
        <InfoLight:webdatasource id="Detail" runat="server" masterdatasource="Master" webdatasetid="WMaster"></InfoLight:webdatasource>
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" BackColor="White"
            BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px" ContentBackColor=""
            ContentForeColor="White" Font-Bold="True" ForeColor="MediumBlue" ShowCompany="False"
            ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
            ShowTitle="True" ShowUserID="True" ShowUserName="True" Width="100%" 
            Height="27px" StatusBackColor="White" StatusForeColor="IndianRed" 
            TitleBackColor="IndianRed" TitleForeColor="White" SkinID="StatusStripSkin1" />
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" 
            BindingObject="wdvMaster" ShowDataStyle="DetailStyle"
            Width="100%" OnCommand="WebNavigator1_Command" BackColor="White" 
            StatusStrip="WebStatusStrip1" BorderColor="LightGray" BorderStyle="Groove" 
            BorderWidth="2px" SkinID="WebNavigatorManagerSkin1">
            <NavControls>
                <InfoLight:ControlItem ControlName="First" ControlText="掸" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/first.gif" MouseOverImageUrl="../image/uipics/first2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/first3.gif" />
                <InfoLight:ControlItem ControlName="Previous" ControlText="掸" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/previous.gif" MouseOverImageUrl="../image/uipics/previous2.gif"
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
                <InfoLight:ControlItem ControlName="Update" ControlText="э" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/edit.gif" MouseOverImageUrl="../image/uipics/edit2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/edit3.gif" />
                <InfoLight:ControlItem ControlName="Delete" ControlText="埃" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/delete.gif" MouseOverImageUrl="../image/uipics/delete2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/delete3.gif" />
                <InfoLight:ControlItem ControlName="OK" ControlText="絋粄" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/ok3.gif" />
                <InfoLight:ControlItem ControlName="Cancel" ControlText="" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif"
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
        <InfoLight:WebDetailsView ID="wdvMaster" runat="server" AllowPaging="True" AutoGenerateRows="False"
            CreateInnerNavigator="False" DataSourceID="Master" 
            OnPageIndexChanged="wdvMaster_PageIndexChanged" Width="100%" 
            AbortIconUrl="../Image/UIPics/Abort.gif" 
            ApplyIconUrl="../Image/UIPics/Apply.gif" 
            MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif" 
            MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif" 
            MouseOverQueryIconUrl="../Image/UIPics/Query2.gif" OnAdding="wdvMaster_Adding" 
            QueryIconUrl="../Image/UIPics/Query.gif" 
            OnAfterInsertLocate="wdvMaster_AfterInsertLocate" 
            OnItemDeleted="wdvMaster_ItemDeleted" OnCanceled="wdvMaster_Canceled" 
            KeyValues="" SkinID="DetailsViewSkin1">
            <editrowstyle BorderColor="#C00000" />
            <FieldHeaderStyle Width="120px" />
            <InsertRowStyle ForeColor="Black" />
            <EmptyDataRowStyle ForeColor="Black" />
        </InfoLight:WebDetailsView>
        <InfoLight:webgridview id="wgvDetail" runat="server" datasourceid="Detail" 
            Width="100%" AbortIconUrl="../Image/UIPics/Abort.gif" 
            AddIconUrl="../Image/UIPics/Add.gif" ApplyIconUrl="../Image/UIPics/Apply.gif" 
            CancelIconUrl="../Image/UIPics/Cancel.gif" 
            MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif" 
            MouseOverAddIconUrl="../Image/UIPics/Add2.gif" 
            MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif" 
            MouseOverCancelIconUrl="../Image/UIPics/Cancel2.gif" 
            MouseOverOKIconUrl="../Image/UIPics/OK2.gif" 
            MouseOverQueryIconUrl="../Image/UIPics/Query2.gif" 
            OKIconUrl="../Image/UIPics/OK.gif" QueryIconUrl="../Image/UIPics/Query.gif" 
            HeaderStyleWrap="False" SkinID="GridViewSkin1">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <edititemtemplate>
<asp:ImageButton runat="server" Text="Update" CommandName="Update" ImageUrl="~/Image/UIPics/OK.gif" CausesValidation="True" id="ImageButton1"></asp:ImageButton>&nbsp;<asp:ImageButton runat="server" Text="Cancel" CommandName="Cancel" ImageUrl="~/Image/UIPics/Cancel.gif" CausesValidation="False" id="ImageButton2"></asp:ImageButton>
</edititemtemplate>
                    <headerstyle wrap="False" />
                    <itemtemplate>
<asp:ImageButton id="ImageButton3" runat="server" Text="Edit" CausesValidation="False" ImageUrl="~/Image/UIPics/Edit.gif" CommandName="Edit" __designer:wfdid="w10"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton4" runat="server" Text="Delete" CausesValidation="False" ImageUrl="~/Image/UIPics/Delete.gif" CommandName="Delete" __designer:wfdid="w11"></asp:ImageButton>
</itemtemplate>
                </asp:TemplateField>
            </Columns>
            <NavControls>
                <InfoLight:ControlItem ControlName="Add" ControlText="add" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif"
                    Size="25">
                </InfoLight:ControlItem>
                <InfoLight:ControlItem ControlName="OK" ControlText="Insert" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif"
                    Size="25">
                </InfoLight:ControlItem>
                <InfoLight:ControlItem ControlName="Cancel" ControlText="cancel" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif"
                    Size="25">
                </InfoLight:ControlItem>
            </NavControls>
</InfoLight:webgridview>
    </form>
</body>
</html>
