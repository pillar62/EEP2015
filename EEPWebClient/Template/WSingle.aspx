<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WSingle.aspx.cs" Inherits="WSingle_WSingle" Theme="ControlSkin" StylesheetTheme="ControlSkin"%>

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
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" BackColor="White"
            BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px" ContentBackColor=""
            ContentForeColor="White" Font-Bold="True" ForeColor="MediumBlue" ShowCompany="False"
            ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
            ShowTitle="True" ShowUserID="True" ShowUserName="True" Width="100%" 
            StatusForeColor="MediumBlue" TitleBackColor="MediumBlue" TitleForeColor="White" 
            SkinID="StatusStripSkin1" />
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BindingObject="wgvMaster"
            Width="100%" BackColor="White" Height="22px" StatusStrip="WebStatusStrip1" 
            SkinID="WebNavigatorManagerSkin1" BorderColor="Silver" BorderStyle="Groove" BorderWidth="2px">
           <navcontrols>
                <InfoLight:ControlItem ControlName="First" ControlText="首筆" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/first.gif" MouseOverImageUrl="../image/uipics/first2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/first3.gif" />
                <InfoLight:ControlItem ControlName="Previous" ControlText="上筆" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/previous.gif" MouseOverImageUrl="../image/uipics/previous2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/previous3.gif" />
                <InfoLight:ControlItem ControlName="Next" ControlText="下筆" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/next.gif" MouseOverImageUrl="../image/uipics/next2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/next3.gif" />
                <InfoLight:ControlItem ControlName="Last" ControlText="末筆" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/last.gif" MouseOverImageUrl="../image/uipics/last2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/last3.gif" />
                <InfoLight:ControlItem ControlName="Add" ControlText="新增" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/add3.gif" />
                <InfoLight:ControlItem ControlName="Apply" ControlText="保存" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/apply.gif" MouseOverImageUrl="../image/uipics/apply2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/apply3.gif" />
                <InfoLight:ControlItem ControlName="Abort" ControlText="放棄" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/abort.gif" MouseOverImageUrl="../image/uipics/abort2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/abort3.gif" />
                <InfoLight:ControlItem ControlName="Query" ControlText="琩高" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/query.gif" MouseOverImageUrl="../image/uipics/query2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/query3.gif" />
                <InfoLight:ControlItem ControlName="Print" ControlText="列印" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/print.gif" MouseOverImageUrl="../image/uipics/print2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/print3.gif" />
                <InfoLight:ControlItem ControlName="Export" ControlType="Image" ControlVisible="True"
                    DisenableImageUrl="../Image/UIPics/Export3.gif" ImageUrl="../Image/UIPics/Export.gif"
                    MouseOverImageUrl="../Image/UIPics/Export2.gif" Size="25" />
            </navcontrols>
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
    
    </div>
        <InfoLight:webgridview id="wgvMaster" runat="server" datasourceid="Master" 
        width="100%" AbortIconUrl="../Image/UIPics/Abort.gif" 
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
            <navcontrols>
<InfoLight:ControlItem ControlVisible="False" Size="25" 
                    MouseOverImageUrl="../image/uipics/add2.gif" ControlText="add" 
                    ControlName="Add" ControlType="Image" ImageUrl="../image/uipics/add.gif"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlVisible="True" Size="25" MouseOverImageUrl="../image/uipics/ok2.gif" ControlText="Insert" ControlName="OK" ControlType="Image" ImageUrl="../image/uipics/ok.gif"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlVisible="True" Size="25" MouseOverImageUrl="../image/uipics/cancel2.gif" ControlText="cancel" ControlName="Cancel" ControlType="Image" ImageUrl="../image/uipics/cancel.gif"></InfoLight:ControlItem>
</navcontrols>
            <Columns>
<asp:TemplateField ShowHeader="False"><EditItemTemplate>
<asp:ImageButton id="ImageButton1" runat="server" Text="Update" CausesValidation="True" ImageUrl="~/Image/UIPics/OK.gif" CommandName="Update" __designer:wfdid="w3"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton2" runat="server" Text="Cancel" CausesValidation="False" ImageUrl="~/Image/UIPics/Cancel.gif" CommandName="Cancel" __designer:wfdid="w4"></asp:ImageButton>
</EditItemTemplate>
    <headerstyle wrap="False" />
<ItemTemplate>
<asp:ImageButton id="ImageButton3" runat="server" Text="Edit" CausesValidation="False" ImageUrl="~/Image/UIPics/Edit.gif" CommandName="Edit" __designer:wfdid="w1"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton4" runat="server" Text="Delete" CausesValidation="False" ImageUrl="~/Image/UIPics/Delete.gif" CommandName="Delete" __designer:wfdid="w2"></asp:ImageButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>

<AlternatingRowStyle BorderColor="White"></AlternatingRowStyle>
</InfoLight:webgridview>
    </form>
</body>
</html>
