<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WSingle3.aspx.cs" Inherits="Template_Wsingle3" Theme="ControlSkin" StylesheetTheme="ControlSkin"%>

<%@ Register Assembly="AjaxTools" Namespace="AjaxTools" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <InfoLight:WebDataSource ID="Master" runat="server" AllowAdd="True" AllowDelete="True"
            AllowPrint="True" AllowUpdate="True" AlwaysClose="False" AutoApply="True" AutoApplyForInsert="False"
            AutoRecordLock="False" AutoRecordLockMode="NoneReload" CommandName="" CommandText=""
            Eof="True" KeyValues="" LastIndex="-1" Marker="'" MasterDataSource="" PacketRecords="100"
            QuotePrefix="[" QuoteSuffix="]" TableName="" WebDataSetID="WMaster">
        </InfoLight:WebDataSource>
        <cc1:AjaxScriptManager ID="AjaxScriptManager1" runat="server">
        </cc1:AjaxScriptManager>
    
    </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" BackColor="White"
                    BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px" ContentBackColor=""
                    ContentForeColor="White" Font-Bold="True" ForeColor="DarkSlateGray" ShowCompany="False"
                    ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
                    ShowTitle="True" ShowUserID="True" ShowUserName="True" StatusBackColor="White"
                    StatusForeColor="DarkSlateGray" TitleBackColor="DarkSlateGray" TitleForeColor="White"
                    Width="100%" SkinID="StatusStripSkin1" />
                <br />
                <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BackColor="White" BindingObject="WebGridView1"
                    BorderColor="Transparent" Height="30px" ShowDataStyle="GridStyle"
                    StatusStrip="WebStatusStrip1" Width="100%" 
                    SkinID="WebNavigatorManagerSkin1">
                    <NavControls>
                        <InfoLight:ControlItem ControlName="First" ControlText="first" ControlType="Image"
                            ControlVisible="True" DisenableImageUrl="../image/uipics/first3.gif" ImageUrl="../image/uipics/first.gif"
                            MouseOverImageUrl="../image/uipics/first2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Previous" ControlText="previous" ControlType="Image"
                            ControlVisible="True" DisenableImageUrl="../image/uipics/previous3.gif" ImageUrl="../image/uipics/previous.gif"
                            MouseOverImageUrl="../image/uipics/previous2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Next" ControlText="next" ControlType="Image"
                            ControlVisible="True" DisenableImageUrl="../image/uipics/next3.gif" ImageUrl="../image/uipics/next.gif"
                            MouseOverImageUrl="../image/uipics/next2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Last" ControlText="last" ControlType="Image"
                            ControlVisible="True" DisenableImageUrl="../image/uipics/last3.gif" ImageUrl="../image/uipics/last.gif"
                            MouseOverImageUrl="../image/uipics/last2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Add" ControlText="add" ControlType="Image" ControlVisible="True"
                            DisenableImageUrl="../image/uipics/add3.gif" ImageUrl="../image/uipics/add.gif"
                            MouseOverImageUrl="../image/uipics/add2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Update" ControlText="update" ControlType="Image"
                            ControlVisible="False" DisenableImageUrl="../image/uipics/edit3.gif" ImageUrl="../image/uipics/edit.gif"
                            MouseOverImageUrl="../image/uipics/edit2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Delete" ControlText="delete" ControlType="Image"
                            ControlVisible="False" DisenableImageUrl="../image/uipics/delete3.gif" ImageUrl="../image/uipics/delete.gif"
                            MouseOverImageUrl="../image/uipics/delete2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="OK" ControlText="ok" ControlType="Image" ControlVisible="True"
                            DisenableImageUrl="../image/uipics/ok3.gif" ImageUrl="../image/uipics/ok.gif"
                            MouseOverImageUrl="../image/uipics/ok2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Cancel" ControlText="cancel" ControlType="Image"
                            ControlVisible="True" DisenableImageUrl="../image/uipics/cancel3.gif" ImageUrl="../image/uipics/cancel.gif"
                            MouseOverImageUrl="../image/uipics/cancel2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Apply" ControlText="apply" ControlType="Image"
                            ControlVisible="True" DisenableImageUrl="../image/uipics/apply3.gif" ImageUrl="../image/uipics/apply.gif"
                            MouseOverImageUrl="../image/uipics/apply2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Abort" ControlText="abort" ControlType="Image"
                            ControlVisible="True" DisenableImageUrl="../image/uipics/abort3.gif" ImageUrl="../image/uipics/abort.gif"
                            MouseOverImageUrl="../image/uipics/abort2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Query" ControlText="query" ControlType="Image"
                            ControlVisible="True" DisenableImageUrl="../image/uipics/query3.gif" ImageUrl="../image/uipics/query.gif"
                            MouseOverImageUrl="../image/uipics/query2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Print" ControlText="print" ControlType="Image"
                            ControlVisible="True" DisenableImageUrl="../image/uipics/print3.gif" ImageUrl="../image/uipics/print.gif"
                            MouseOverImageUrl="../image/uipics/print2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Export" ControlText="export" ControlType="Image"
                            ControlVisible="True" DisenableImageUrl="../image/uipics/export3.gif" ImageUrl="../image/uipics/export.gif"
                            MouseOverImageUrl="../image/uipics/export2.gif" Size="25" />
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
                <br />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="AjaxModalPanel1$__btnClose" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="AjaxModalPanel1$__btnClose" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <cc1:AjaxModalPanel ID="AjaxModalPanel1" runat="server" DataContainer="WebFormView1" TriggerUpdatePanel="UpdatePanel1" Width="500px">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <br />
                    <br />
                    <asp:Button ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" Text="OK" />&nbsp;<asp:Button
                        ID="buttonClose" runat="server" OnClick="buttonClose_Click" Text="Close" /><br />
                </ContentTemplate>
            </asp:UpdatePanel>
        </cc1:AjaxModalPanel>
        <InfoLight:WebGridView ID="WebGridView1" runat="server" AbortIconUrl="../Image/UIPics/Abort.gif"
            AddIconUrl="../Image/UIPics/Add.gif" 
        ApplyIconUrl="../Image/UIPics/Apply.gif" 
        CancelIconUrl="../Image/UIPics/Cancel.gif" DataSourceID="Master" 
        HeaderStyleWrap="False" MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif"
            MouseOverAddIconUrl="../Image/UIPics/Add2.gif" MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif"
            MouseOverCancelIconUrl="../Image/UIPics/Cancel2.gif" MouseOverOKIconUrl="../Image/UIPics/OK2.gif"
            MouseOverQueryIconUrl="../Image/UIPics/Query2.gif" OKIconUrl="../Image/UIPics/OK.gif"
            QueryIconUrl="../Image/UIPics/Query.gif" Width="100%" 
        EditURLPanel="AjaxModalPanel1" SkinID="GridViewSkin1">
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
<asp:ImageButton id="ImageButton1" runat="server" CommandName="Update" Text="Update" ImageUrl="~/Image/UIPics/OK.gif" CausesValidation="True" __designer:wfdid="w48"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton2" runat="server" CommandName="Cancel" Text="Cancel" ImageUrl="~/Image/UIPics/Cancel.gif" CausesValidation="False" __designer:wfdid="w49"></asp:ImageButton> 
</edititemtemplate>
                    <itemtemplate>
<asp:ImageButton id="ImageButton3" runat="server" CommandName="AjaxEdit" Text="Edit" ImageUrl="~/Image/UIPics/Edit.gif" CausesValidation="False" __designer:wfdid="w46"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton4" runat="server" CommandName="Delete" Text="Delete" ImageUrl="~/Image/UIPics/Delete.gif" CausesValidation="False" __designer:wfdid="w47"></asp:ImageButton> 
</itemtemplate>
                    <headerstyle wrap="False" />
                </asp:TemplateField>
            </Columns>
            <AlternatingRowStyle BorderColor="White" />
        </InfoLight:WebGridView>
        <InfoLight:WebFormView ID="WebFormView1" runat="server" 
        DataSourceID="Master" LayOutColNum="2" Width="100%" SkinID="FormViewSkin1">
            <InsertRowStyle BackColor="DarkTurquoise" />
        </InfoLight:WebFormView>
    </form>
</body>
</html>
