<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WMasterDetail3.aspx.cs" Inherits="Template_WMasterDetail3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
        <InfoLight:WebDataSource ID="View" runat="server" WebDataSetID="WView">
        </InfoLight:WebDataSource>
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" BackColor="LightSeaGreen"
            BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px" ContentBackColor=""
            ContentForeColor="White" Font-Bold="True" ForeColor="DarkSlateGray" ShowCompany="False"
            ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
            ShowTitle="True" ShowUserID="True" ShowUserName="True" StatusBackColor="White"
            StatusForeColor="DarkSlateGray" TitleBackColor="DarkSlateGray" TitleForeColor="White"
            Width="100%" />
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BindingObject="wfvMaster"
            OnCommand="WebNavigator1_Command" ShowDataStyle="FormViewStyle" Width="100%" ViewBindingObject="WgView" BackColor="MediumAquamarine" StatusStrip="WebStatusStrip1">
            <NavControls>
                <InfoLight:ControlItem ControlName="First" ControlText="首筆" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/first.gif" MouseOverImageUrl="../image/uipics/first2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Previous" ControlText="上筆" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/previous.gif" MouseOverImageUrl="../image/uipics/previous2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Next" ControlText="下筆" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/next.gif" MouseOverImageUrl="../image/uipics/next2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Last" ControlText="末筆" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/last.gif" MouseOverImageUrl="../image/uipics/last2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Add" ControlText="新增" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Update" ControlText="更改" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/edit.gif" MouseOverImageUrl="../image/uipics/edit2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Delete" ControlText="刪除" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/delete.gif" MouseOverImageUrl="../image/uipics/delete2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="OK" ControlText="確認" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Cancel" ControlText="取消" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Apply" ControlText="存檔" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/apply.gif" MouseOverImageUrl="../image/uipics/apply2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Abort" ControlText="放棄" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/abort.gif" MouseOverImageUrl="../image/uipics/abort2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Query" ControlText="查詢" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/query.gif" MouseOverImageUrl="../image/uipics/query2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Print" ControlText="打印" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/print.gif" MouseOverImageUrl="../image/uipics/print2.gif"
                    Size="25" />
            </NavControls>
        </InfoLight:WebNavigator>
        <InfoLight:WebGridView ID="WgView" runat="server" DataSourceID="View" AddIndentityField="False" AllowPaging="True" AutoGenerateColumns="False" CellPadding="2" CreateInnerNavigator="False" DataHasChanged="False" EditReturn="True" EditURL="" ExpressionFieldCount="0" ForeColor="#333333" GetServerText="True" GridInserting="False" HeaderStyleWrap="False" HorizontalAlign="Left" InnerNavigatorLinkLabel="" InnerNavigatorShowStyle="Image" OnSelectedIndexChanged="WgView_SelectedIndexChanged" OpenEditHeight="400" OpenEditUrlInServerMode="True" OpenEditWidth="500" PageSize="15" TotalActive="False" Width="30%" Height="480px">
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
                <asp:CommandField ButtonType="Image" SelectImageUrl="~/Image/UIPics/Select.gif" ShowSelectButton="True" >
                    <headerstyle wrap="False" />
                </asp:CommandField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" />
        </InfoLight:WebGridView>
        <InfoLight:WebFormView ID="wfvMaster" runat="server" AllowPaging="True" AllValidateSucess="True" BackColor="White" BorderColor="Gray" BorderStyle="Groove" BorderWidth="2px" CellPadding="2" CellSpacing="2" DataHasChanged="False" DataSourceID="Master" GridLines="Both" InsertBack="False" LayOutColNum="2" OnAfterInsertLocate="wfvMaster_AfterInsertLocate" Height="91px" OnCanceled="wfvMaster_Canceled" Width="70%">
            <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
            <EditRowStyle BackColor="Teal" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="MintCream" ForeColor="MidnightBlue" />
            <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
            <InsertRowStyle BackColor="Teal" ForeColor="White" />
        </InfoLight:WebFormView>
        <InfoLight:webgridview id="wgvDetail" runat="server" allowpaging="True" autogeneratecolumns="False" cellpadding="2"
            createinnernavigator="True" datasourceid="Detail" forecolor="#333333"
            width="70%" AbortIconUrl="../Image/UIPics/Abort.gif" AddIconUrl="../Image/UIPics/Add.gif" ApplyIconUrl="../Image/UIPics/Apply.gif" CancelIconUrl="../Image/UIPics/Cancel.gif" InnerNavigatorShowStyle="Image" MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif" MouseOverAddIconUrl="../Image/UIPics/Add2.gif" MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif" MouseOverCancelIconUrl="../Image/UIPics/Cancel2.gif" MouseOverOKIconUrl="../Image/UIPics/OK2.gif" MouseOverQueryIconUrl="../Image/UIPics/Query2.gif" OKIconUrl="../Image/UIPics/OK.gif" QueryIconUrl="../Image/UIPics/Query.gif" HeaderStyleWrap="False">
<FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True"></FooterStyle>

<RowStyle BackColor="#E3EAEB" ForeColor="Black"></RowStyle>

<EditRowStyle BackColor="#7C6F57"></EditRowStyle>

<SelectedRowStyle BackColor="#C5BBAF" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#1C5E55" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="DarkCyan" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White" ForeColor="Black"></AlternatingRowStyle>
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
</InfoLight:webgridview>
    </form>
</body>
</html>
