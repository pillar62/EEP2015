<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WMasterDetail5.aspx.cs" Inherits="Template_WMasterDetail6" Theme="ControlSkin" StylesheetTheme="ControlSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <infolight:webdatasource id="Master" runat="server" autoapply="True" webdatasetid="WMaster" cachedataset="True">
            </infolight:webdatasource>
            </div>
            <infolight:webdatasource id="Detail" runat="server" masterdatasource="Master" webdatasetid="WMaster" cachedataset="True">
        </infolight:webdatasource>
            <AjaxTools:AjaxScriptManager ID="AjaxScriptManager1" runat="server">
            </AjaxTools:AjaxScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <infolight:webstatusstrip id="WebStatusStrip1" runat="server"
                        backcolor="White" bordercolor="LightGray" borderstyle="Groove"
                        borderwidth="2px" contentbackcolor="" contentforecolor="White" font-bold="True"
                        forecolor="MediumBlue" showcompany="False" showdate="True" showeepalias="True"
                        shownavigatorstatus="True" showsolution="False" showtitle="True"
                        showuserid="True" showusername="True" statusbackcolor="White"
                        statusforecolor="MediumBlue" titlebackcolor="MediumBlue" titleforecolor="White"
                        width="100%" skinid="StatusStripSkin1" />
                    <br />
                    <infolight:webnavigator id="WebNavigator1" runat="server"
                        backcolor="White" bindingobject="WgView" bordercolor="#E0E0E0"
                        borderstyle="Groove" borderwidth="2px" oncommand="WebNavigator1_Command"
                        showdatastyle="GridStyle" statusstrip="WebStatusStrip1" width="100%"
                        skinid="WebNavigatorManagerSkin1">
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
                            ControlType="Image" ControlVisible="False" 
                            DisenableImageUrl="../image/uipics/edit3.gif" 
                            ImageUrl="../image/uipics/edit.gif" 
                            MouseOverImageUrl="../image/uipics/edit2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Delete" ControlText="埃" 
                            ControlType="Image" ControlVisible="False" 
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
                </infolight:webnavigator>
                    <br />
                    <br />
                    <br />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="AjaxModalPanel1$ButtonOK" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="AjaxModalPanel1$ButtonClose" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <infolight:webgridview id="WgView" runat="server" datasourceid="Master"
                width="100%" aborticonurl="../Image/UIPics/Abort.gif"
                addiconurl="../Image/UIPics/Add.gif" applyiconurl="../Image/UIPics/Apply.gif"
                canceliconurl="../Image/UIPics/Cancel.gif"
                mouseoveraborticonurl="../Image/UIPics/Abort2.gif"
                mouseoveraddiconurl="../Image/UIPics/Add2.gif"
                mouseoverapplyiconurl="../Image/UIPics/Apply2.gif"
                mouseovercanceliconurl="../Image/UIPics/Cancel2.gif"
                mouseoverokiconurl="../Image/UIPics/OK2.gif"
                mouseoverqueryiconurl="../Image/UIPics/Query2.gif"
                okiconurl="../Image/UIPics/OK.gif" queryiconurl="../Image/UIPics/Query.gif"
                headerstylewrap="False" skinid="GridViewSkin1" editurlpanel="AjaxModalPanel1">
            <navcontrols>
<InfoLight:ControlItem ControlVisible="False" Size="25" 
                    MouseOverImageUrl="../image/uipics/add2.gif" ControlText="add" 
                    ControlName="Add" ControlType="Image" ImageUrl="../image/uipics/add.gif"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlVisible="True" Size="25" MouseOverImageUrl="../image/uipics/ok2.gif" ControlText="Insert" ControlName="OK" ControlType="Image" ImageUrl="../image/uipics/ok.gif"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlVisible="True" Size="25" MouseOverImageUrl="../image/uipics/cancel2.gif" ControlText="cancel" ControlName="Cancel" ControlType="Image" ImageUrl="../image/uipics/cancel.gif"></InfoLight:ControlItem>
</navcontrols>
            <Columns>
<asp:TemplateField ShowHeader="False"><EditItemTemplate>
<asp:ImageButton id="ImageButton5" runat="server" Text="Update" CausesValidation="True" ImageUrl="~/Image/UIPics/OK.gif" CommandName="Update" __designer:wfdid="w3"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton6" runat="server" Text="Cancel" CausesValidation="False" ImageUrl="~/Image/UIPics/Cancel.gif" CommandName="Cancel" __designer:wfdid="w4"></asp:ImageButton>
</EditItemTemplate>
    <headerstyle wrap="False" />
<ItemTemplate>
    <asp:ImageButton id="ImageButton9" runat="server" Text="View" CausesValidation="False" ImageUrl="~/Image/UIPics/OK.gif" CommandName="AjaxView" __designer:wfdid="w1"></asp:ImageButton>
    <asp:ImageButton id="ImageButton7" runat="server" Text="Edit" CausesValidation="False" ImageUrl="~/Image/UIPics/Edit.gif" CommandName="AjaxEdit" __designer:wfdid="w1"></asp:ImageButton>
    <asp:ImageButton id="ImageButton8" runat="server" Text="Delete" CausesValidation="False" ImageUrl="~/Image/UIPics/Delete.gif" CommandName="Delete" __designer:wfdid="w2"></asp:ImageButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>

<AlternatingRowStyle BorderColor="White"></AlternatingRowStyle>

<PagerSettings Mode="NumericFirstLast"></PagerSettings>
</infolight:webgridview>
            <AjaxTools:AjaxModalPanel ID="AjaxModalPanel1" runat="server" Width="500px" DataContainer="wfvMaster;wgvDetail" TriggerUpdatePanel="UpdatePanel3" Height="400px">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <br />
                        <br />
                        <br />
                        <br />
                        <asp:Button ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" Text="OK" />&nbsp;<asp:Button
                            ID="ButtonClose" runat="server" OnClick="buttonClose_Click" Text="Close" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="AjaxModalPanel1$ButtonOK" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="UpdatePanel1$WebNavigator1" EventName="Command" />
                        <asp:AsyncPostBackTrigger ControlID="wfvMaster" EventName="PageIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="AjaxModalPanel2$__btnClose" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </AjaxTools:AjaxModalPanel>
            <infolight:webformview id="wfvMaster" runat="server" allowpaging="True"
                datasourceid="Master" height="87px" layoutcolnum="2" onafterinsertlocate="wfvMaster_AfterInsertLocate"
                oncanceled="wfvMaster_Canceled"
                onpageindexchanged="wfvMaster_PageIndexChanged" width="100%"
                skinid="FormViewSkin1">
            <InsertRowStyle BackColor="LightCyan" ForeColor="Blue" />
        </infolight:webformview>
            <infolight:webgridview id="wgvDetail" runat="server" aborticonurl="../Image/UIPics/Abort.gif" DataSourceID="Detail"
                addiconurl="../Image/UIPics/Add.gif" applyiconurl="../Image/UIPics/Apply.gif"
                canceliconurl="../Image/UIPics/Cancel.gif"
                editurlpanel="AjaxModalPanel2" headerstylewrap="False" mouseoveraborticonurl="../Image/UIPics/Abort2.gif"
                mouseoveraddiconurl="../Image/UIPics/Add2.gif" mouseoverapplyiconurl="../Image/UIPics/Apply2.gif"
                mouseovercanceliconurl="../Image/UIPics/Cancel2.gif" mouseoverokiconurl="../Image/UIPics/OK2.gif"
                mouseoverqueryiconurl="../Image/UIPics/Query2.gif" okiconurl="../Image/UIPics/OK.gif"
                queryiconurl="../Image/UIPics/Query.gif" width="100%"
                onrowcommand="wgvDetail_RowCommand" skinid="GridViewSkin1">
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
        </infolight:webgridview>
            <br />
            <AjaxTools:AjaxModalPanel ID="AjaxModalPanel2" runat="server" Width="500px" DataContainer="wfvDetail" TriggerUpdatePanel="UpdatePanel3">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <br />
                        <asp:Button ID="ButtonOK1" runat="server" OnClick="ButtonOK1_Click" Text="OK" />&nbsp;<asp:Button
                            ID="ButtonClose1" runat="server" OnClick="buttonClose1_Click" Text="Close" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </AjaxTools:AjaxModalPanel>
            <infolight:webformview id="wfvDetail" runat="server" datasourceid="Detail"
                layoutcolnum="2" width="100%" skinid="FormViewSkin1">
            <InsertRowStyle BackColor="DarkTurquoise" />
        </infolight:webformview>
        </div>
    </form>
</body>
</html>
