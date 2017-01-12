<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WSingle2.aspx.cs" Inherits="WSingle_WSingle" %>

<%@ Register Assembly="AjaxTools" Namespace="AjaxTools" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>EEP2006</title>
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <InfoLight:WebDataSource ID="Master" runat="server" AutoApply="True" WebDataSetID="WMaster" AllowAdd="True" AllowDelete="True" AllowPrint="True" AllowUpdate="True" AlwaysClose="False" AutoApplyForInsert="False" AutoRecordLock="False" AutoRecordLockMode="NoneReload" CommandName="" CommandText="" Eof="True" KeyValues="" LastIndex="-1" Marker="'" MasterDataSource="" PacketRecords="100" QuotePrefix="[" QuoteSuffix="]" TableName="">
        </InfoLight:WebDataSource>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" BackColor="LightSeaGreen"
                    BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px" ContentBackColor=""
                    ContentForeColor="White" Font-Bold="True" ForeColor="DarkSlateGray" ShowCompany="False"
                    ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
                    ShowTitle="True" ShowUserID="True" ShowUserName="True" StatusBackColor="White"
                    StatusForeColor="DarkSlateGray" TitleBackColor="DarkSlateGray" TitleForeColor="White"
                    Width="100%" />
                <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BindingObject="WebFormView1"
                    ShowDataStyle="FormViewStyle" Width="100%" BackColor="MintCream" BorderColor="Transparent" Height="30px" OnCommand="WebNavigator1_Command" StatusStrip="WebStatusStrip1" ViewBindingObject="WebGridView1">
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
                <InfoLight:WebMultiViewCaptions ID="WebMultiViewCaptions1" runat="server" MultiViewID="MultiView1"
                    TableStyle="Style3" Width="100%">
                    <Captions>
                        <InfoLight:WebMultiViewCaption Caption="資料瀏覽" />
                        <InfoLight:WebMultiViewCaption Caption="資料編輯" />
                    </Captions>
                </InfoLight:WebMultiViewCaptions>
                <br />
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                        &nbsp;</asp:View>
                    <asp:View ID="View2" runat="server">
                        &nbsp;</asp:View>
                </asp:MultiView>
            </ContentTemplate>
        </asp:UpdatePanel>
        &nbsp;
                        <InfoLight:WebGridView ID="WebGridView1" runat="server" AddIndentityField="False" DataSourceID="Master"
                            AutoEmptyDataText="False" AutoGenerateColumns="False" ButtonTooltip="" CellPadding="4"
                            DataHasChanged="False" EditReturn="True" EditURL="" ExpressionFieldCount="0"
                            ForeColor="#333333" GetServerText="True" GridInserting="False" GridLines="None"
                            HeaderStyleWrap="False" InnerNavigatorLinkLabel="" InnerNavigatorShowStyle="Image"
                            MultiCheckColumn="" MultiCheckColumnIndexToTranslate="-1" NeedExecuteAdd="True"
                            OnRowCommand="WebGridView1_RowCommand" OpenEditUrlInServerMode="True" SkipInsert="False"
                            TotalActive="False" TotalCaption="" ValidateFailed="False" Width="100%">
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <PagerSettings Mode="NumericFirstLast" />
                            <NavControls>
                                <InfoLight:ControlItem ControlName="Add" ControlText="add" ControlType="Image" ControlVisible="False"
                                    ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif"
                                    Size="25" />
                                <InfoLight:ControlItem ControlName="Query" ControlText="query" ControlType="Image"
                                    ControlVisible="False" ImageUrl="../image/uipics/query.gif" MouseOverImageUrl="../image/uipics/query2.gif"
                                    Size="25" />
                                <InfoLight:ControlItem ControlName="Apply" ControlText="apply" ControlType="Image"
                                    ControlVisible="False" ImageUrl="../image/uipics/apply.gif" MouseOverImageUrl="../image/uipics/apply2.gif"
                                    Size="25" />
                                <InfoLight:ControlItem ControlName="Abort" ControlText="abort" ControlType="Image"
                                    ControlVisible="False" ImageUrl="../image/uipics/abort.gif" MouseOverImageUrl="../image/uipics/abort2.gif"
                                    Size="25" />
                                <InfoLight:ControlItem ControlName="OK" ControlText="Insert" ControlType="Image"
                                    ControlVisible="True" ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif"
                                    Size="25" />
                                <InfoLight:ControlItem ControlName="Cancel" ControlText="cancel" ControlType="Image"
                                    ControlVisible="True" ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif"
                                    Size="25" />
                            </NavControls>
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <RowStyle BackColor="#EFF3FB" />
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <itemtemplate>
<asp:ImageButton runat="server" Text="Select" CommandName="Select" ImageUrl="~/Image/UIPics/Select.gif" CausesValidation="False" id="ImageButton1"></asp:ImageButton>
</itemtemplate>
                                </asp:TemplateField>
                            </Columns>
                        </InfoLight:WebGridView>
        <InfoLight:WebFormView ID="WebFormView1" runat="server" AllValidateSucess="True" DataSourceID="Master" 
            AutoEmptyDataText="False" CellPadding="4" DataHasChanged="False" ForeColor="#333333"
            InsertBack="False" KeyValues="" LayOutColNum="2" NeedExecuteAdd="True" OldPageIndex="-1"
            ValidateFailed="False" Width="100%">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="DarkTurquoise" />
            <RowStyle BackColor="#EFF3FB" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <InsertRowStyle BackColor="DarkTurquoise" />
        </InfoLight:WebFormView>
    
    </div>
    </form>
</body>
</html>
