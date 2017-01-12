<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VBWebCMasterDetail_VFG.aspx.vb" Inherits="Template_VBWebCMasterDetail_VFG" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
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
        <InfoLight:WebDataSource ID="View" runat="server" WebDataSetID="WView">
        </InfoLight:WebDataSource>
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" BackColor="LightSeaGreen"
            BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px" ContentBackColor=""
            ContentForeColor="White" Font-Bold="True" ForeColor="DarkSlateGray" ShowCompany="False"
            ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
            ShowTitle="True" ShowUserID="True" ShowUserName="True" StatusBackColor="White"
            StatusForeColor="DarkSlateGray" TitleBackColor="DarkSlateGray" TitleForeColor="White"
            Width="644px" />
        <br />
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BackColor="MediumAquamarine"
            BindingObject="wfvMaster" OnCommand="WebNavigator1_Command" ShowDataStyle="FormViewStyle"
            StatusStrip="WebStatusStrip1" ViewBindingObject="WgView" Width="419px">
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
        &nbsp;&nbsp;
        <InfoLight:WebGridView ID="WgView" runat="server" AddIndentityField="False" AllowPaging="True"
            AutoGenerateColumns="False" CellPadding="2" CreateInnerNavigator="False" DataHasChanged="False"
            DataSourceID="View" EditReturn="True" EditURL="" ExpressionFieldCount="0" ForeColor="#333333"
            GetServerText="True" GridInserting="False" HeaderStyleWrap="False" Height="480px"
            HorizontalAlign="Left" InnerNavigatorLinkLabel="" InnerNavigatorShowStyle="Image"
            OnSelectedIndexChanged="WgView_SelectedIndexChanged" OpenEditHeight="400" OpenEditUrlInServerMode="True"
            OpenEditWidth="500" PageSize="15" TotalActive="False" Width="234px">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="Teal" Font-Bold="True" ForeColor="White" />
            <NavControls>
                <InfoLight:ControlItem ControlName="Add" ControlText="add" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Query" ControlText="query" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/query.gif" MouseOverImageUrl="../image/uipics/query2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Apply" ControlText="apply" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/apply.gif" MouseOverImageUrl="../image/uipics/apply2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Abort" ControlText="abort" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/abort.gif" MouseOverImageUrl="../image/uipics/abort2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="OK" ControlText="Insert" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Cancel" ControlText="cancel" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif"
                    Size="25" />
            </NavControls>
            <EditRowStyle BackColor="#999999" />
            <PagerStyle BackColor="Teal" ForeColor="White" HorizontalAlign="Center" />
            <AlternatingRowStyle BackColor="Honeydew" ForeColor="SteelBlue" />
            <RowStyle BackColor="White" ForeColor="DarkSlateGray" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <Columns>
                <asp:CommandField ButtonType="Image" SelectImageUrl="~/Image/UIPics/Select.gif" ShowSelectButton="True">
                    <headerstyle wrap="False" />
                </asp:CommandField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" />
        </InfoLight:WebGridView>
        <InfoLight:WebFormView ID="wfvMaster" runat="server" AllowPaging="True" AllValidateSucess="True"
            BackColor="White" BorderColor="Gray" BorderStyle="Groove" BorderWidth="2px" CellPadding="2"
            CellSpacing="2" DataHasChanged="False" DataSourceID="Master" GridLines="Both"
            Height="91px" InsertBack="False" LayOutColNum="2" OnAfterInsertLocate="wfvMaster_AfterInsertLocate"
            OnCanceled="wfvMaster_Canceled">
            <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
            <EditRowStyle BackColor="Teal" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="MintCream" ForeColor="MidnightBlue" />
            <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
            <InsertRowStyle BackColor="Teal" ForeColor="White" />
        </InfoLight:WebFormView>
        <InfoLight:WebGridView ID="wgvDetail" runat="server" AbortIconUrl="../Image/UIPics/Abort.gif"
            AddIconUrl="../Image/UIPics/Add.gif" AllowPaging="True" ApplyIconUrl="../Image/UIPics/Apply.gif"
            AutoGenerateColumns="False" CancelIconUrl="../Image/UIPics/Cancel.gif" CellPadding="2"
            CreateInnerNavigator="True" DataSourceID="Detail" ForeColor="#333333" HeaderStyleWrap="False"
            InnerNavigatorShowStyle="Image" MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif"
            MouseOverAddIconUrl="../Image/UIPics/Add2.gif" MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif"
            MouseOverCancelIconUrl="../Image/UIPics/Cancel2.gif" MouseOverOKIconUrl="../Image/UIPics/OK2.gif"
            MouseOverQueryIconUrl="../Image/UIPics/Query2.gif" OKIconUrl="../Image/UIPics/OK.gif"
            QueryIconUrl="../Image/UIPics/Query.gif" Width="573px">
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#E3EAEB" ForeColor="Black" />
            <EditRowStyle BackColor="#7C6F57" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#1C5E55" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="DarkCyan" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="Black" />
            <PagerSettings Mode="NumericFirstLast" />
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <edititemtemplate>
<asp:ImageButton runat="server" Text="Update" CommandName="Update" ImageUrl="~/Image/UIPics/OK.gif" CausesValidation="True" id="ImageButton6"></asp:ImageButton>&nbsp;<asp:ImageButton runat="server" Text="Cancel" CommandName="Cancel" ImageUrl="~/Image/UIPics/Cancel.gif" CausesValidation="False" id="ImageButton7"></asp:ImageButton>
</edititemtemplate>
                    <headerstyle wrap="False" />
                    <itemtemplate>
<asp:ImageButton id="ImageButton8" runat="server" Text="Edit" CausesValidation="False" ImageUrl="~/Image/UIPics/Edit.gif" CommandName="Edit" __designer:wfdid="w185"></asp:ImageButton>&nbsp;<asp:ImageButton id="ImageButton9" runat="server" Text="Delete" CausesValidation="False" ImageUrl="~/Image/UIPics/Delete.gif" CommandName="Delete" __designer:wfdid="w186"></asp:ImageButton> 
</itemtemplate>
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
        </InfoLight:WebGridView>
    
    </div>
    </form>
</body>
</html>
