<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WMasterDetail2.aspx.cs" Inherits="Template_WMasterDetail2" %>

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
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" BackColor="Bisque"
            BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px" ContentBackColor=""
            ContentForeColor="White" Font-Bold="True" ForeColor="IndianRed" ShowCompany="False"
            ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
            ShowTitle="True" ShowUserID="True" ShowUserName="True" Width="100%" Height="27px" StatusBackColor="White" StatusForeColor="IndianRed" TitleBackColor="IndianRed" TitleForeColor="White" />
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BindingObject="wdvMaster" ShowDataStyle="DetailStyle"
            Width="100%" OnCommand="WebNavigator1_Command" BackColor="Bisque" StatusStrip="WebStatusStrip1" BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px">
            <NavControls>
                <InfoLight:ControlItem ControlName="First" ControlText="首筆" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/first.gif" MouseOverImageUrl="../image/uipics/first2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Previous" ControlText="上筆" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/previous.gif" MouseOverImageUrl="../image/uipics/previous2.gif"
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
                <InfoLight:ControlItem ControlName="Update" ControlText="更改" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/edit.gif" MouseOverImageUrl="../image/uipics/edit2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Delete" ControlText="刪除" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/delete.gif" MouseOverImageUrl="../image/uipics/delete2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="OK" ControlText="確認" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif"
                    Size="25" />
                <InfoLight:ControlItem ControlName="Cancel" ControlText="取消" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif"
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
        <InfoLight:WebDetailsView ID="wdvMaster" runat="server" AllowPaging="True" AutoGenerateRows="False"
            CreateInnerNavigator="False" DataSourceID="Master" OnPageIndexChanged="wdvMaster_PageIndexChanged" BackColor="White" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="2" CellSpacing="1" Width="100%" AbortIconUrl="../Image/UIPics/Abort.gif" ApplyIconUrl="../Image/UIPics/Apply.gif" InnerNavigatorShowStyle="Image" MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif" MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif" MouseOverQueryIconUrl="../Image/UIPics/Query2.gif" OnAdding="wdvMaster_Adding" QueryIconUrl="../Image/UIPics/Query.gif" GetServerText="True" OnAfterInsertLocate="wdvMaster_AfterInsertLocate" OnItemDeleted="wdvMaster_ItemDeleted" OnCanceled="wdvMaster_Canceled">
            <footerstyle backcolor="#F7DFB5" forecolor="#8C4510" />
            <editrowstyle backcolor="AntiqueWhite" font-bold="False" forecolor="Black" BorderColor="#C00000" />
            <rowstyle backcolor="#FFF7E7" forecolor="#8C4510" />
            <pagerstyle forecolor="#8C4510" horizontalalign="Center" BackColor="Bisque" />
            <headerstyle backcolor="SaddleBrown" font-bold="True" forecolor="White" />
            <PagerSettings Mode="NumericFirstLast" />
            <FieldHeaderStyle Width="120px" />
            <InsertRowStyle ForeColor="Black" />
            <EmptyDataRowStyle ForeColor="Black" />
        </InfoLight:WebDetailsView>
        <InfoLight:webgridview id="wgvDetail" runat="server" allowpaging="True" autogeneratecolumns="False" datasourceid="Detail" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" Width="100%" AbortIconUrl="../Image/UIPics/Abort.gif" AddIconUrl="../Image/UIPics/Add.gif" ApplyIconUrl="../Image/UIPics/Apply.gif" CancelIconUrl="../Image/UIPics/Cancel.gif" InnerNavigatorShowStyle="Image" MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif" MouseOverAddIconUrl="../Image/UIPics/Add2.gif" MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif" MouseOverCancelIconUrl="../Image/UIPics/Cancel2.gif" MouseOverOKIconUrl="../Image/UIPics/OK2.gif" MouseOverQueryIconUrl="../Image/UIPics/Query2.gif" OKIconUrl="../Image/UIPics/OK.gif" QueryIconUrl="../Image/UIPics/Query.gif" HeaderStyleWrap="False" AddIndentityField="False" AutoEmptyDataText="False" ButtonTooltip="" DataHasChanged="False" EditReturn="True" EditURL="" ExpressionFieldCount="0" GetServerText="True" GridInserting="False" GridLines="None" InnerNavigatorLinkLabel="" MultiCheckColumn="" MultiCheckColumnIndexToTranslate="-1" NeedExecuteAdd="True" OpenEditHeight="400" OpenEditUrlInServerMode="True" OpenEditWidth="500" SkipInsert="False" TotalActive="False" TotalCaption="" ValidateFailed="False">
            <footerstyle backcolor="Tan" />
            <selectedrowstyle backcolor="DarkSlateBlue" forecolor="GhostWhite" />
            <pagerstyle backcolor="BurlyWood" forecolor="DarkSlateBlue" horizontalalign="Center" />
            <headerstyle backcolor="BurlyWood" font-bold="True" />
            <alternatingrowstyle backcolor="PaleGoldenrod" />
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
            <PagerSettings Mode="NumericFirstLast" />
</InfoLight:webgridview>
    </form>
</body>
</html>
