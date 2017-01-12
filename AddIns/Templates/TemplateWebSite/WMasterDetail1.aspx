<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WMasterDetail1.aspx.cs" Inherits="Template_WMasterDetail1" %>

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
        <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" BackColor="PowderBlue"
            BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px" ContentBackColor=""
            ContentForeColor="White" Font-Bold="True" ForeColor="MediumBlue" ShowCompany="False"
            ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
            ShowTitle="True" ShowUserID="True" ShowUserName="True" Width="100%" StatusBackColor="White" StatusForeColor="MediumBlue" TitleBackColor="MediumBlue" TitleForeColor="White" />
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BindingObject="wfvMaster"
            OnCommand="WebNavigator1_Command" ShowDataStyle="FormViewStyle" Width="100%" BackColor="PowderBlue" BorderColor="#E0E0E0" BorderStyle="Groove" BorderWidth="2px" StatusStrip="WebStatusStrip1">
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
        <InfoLight:WebFormView ID="wfvMaster" runat="server" OnPageIndexChanged="wfvMaster_PageIndexChanged" AllowPaging="True" AllValidateSucess="True" CellPadding="4" DataHasChanged="False" DataSourceID="Master" InsertBack="False" LayOutColNum="2" OnAfterInsertLocate="wfvMaster_AfterInsertLocate" OnCanceled="wfvMaster_Canceled" AutoEmptyDataText="False" ForeColor="#333333" Height="87px" KeyValues="" NeedExecuteAdd="True" OldPageIndex="-1" ValidateFailed="False" Width="100%" BackColor="Silver" BorderColor="White" BorderStyle="Groove" BorderWidth="2px">
            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
            <EditRowStyle BackColor="LightCyan" ForeColor="Blue" />
            <RowStyle BackColor="#EFF3FB" ForeColor="Navy" />
            <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="PowderBlue" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <InsertRowStyle BackColor="LightCyan" ForeColor="Blue" />
        </InfoLight:WebFormView>
        <InfoLight:webgridview id="wgvDetail" runat="server" allowpaging="True" autogeneratecolumns="False" cellpadding="1" datasourceid="Detail" forecolor="#333333"
            width="100%" AbortIconUrl="../Image/UIPics/Abort.gif" AddIconUrl="../Image/UIPics/Add.gif" ApplyIconUrl="../Image/UIPics/Apply.gif" CancelIconUrl="../Image/UIPics/Cancel.gif" InnerNavigatorShowStyle="Image" MouseOverAbortIconUrl="../Image/UIPics/Abort2.gif" MouseOverAddIconUrl="../Image/UIPics/Add2.gif" MouseOverApplyIconUrl="../Image/UIPics/Apply2.gif" MouseOverCancelIconUrl="../Image/UIPics/Cancel2.gif" MouseOverOKIconUrl="../Image/UIPics/OK2.gif" MouseOverQueryIconUrl="../Image/UIPics/Query2.gif" OKIconUrl="../Image/UIPics/OK.gif" QueryIconUrl="../Image/UIPics/Query.gif" HeaderStyleWrap="False" AddIndentityField="False" AutoEmptyDataText="False" BackColor="White" BorderStyle="Groove" BorderWidth="2px" ButtonTooltip="" CellSpacing="1" DataHasChanged="False" EditReturn="True" EditURL="" ExpressionFieldCount="0" GetServerText="True" GridInserting="False" InnerNavigatorLinkLabel="" MultiCheckColumn="" MultiCheckColumnIndexToTranslate="-1" NeedExecuteAdd="True" OpenEditHeight="400" OpenEditUrlInServerMode="True" OpenEditWidth="500" SkipInsert="False" TotalActive="False" TotalCaption="" ValidateFailed="False">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="PowderBlue" ForeColor="Blue" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="DarkTurquoise" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <edititemtemplate>
<asp:ImageButton runat="server" Text="Update" CommandName="Update" ImageUrl="~/Image/UIPics/OK.gif" CausesValidation="True" id="ImageButton6"></asp:ImageButton>&nbsp;<asp:ImageButton runat="server" Text="Cancel" CommandName="Cancel" ImageUrl="~/Image/UIPics/Cancel.gif" CausesValidation="False" id="ImageButton7"></asp:ImageButton>
</edititemtemplate>
                    <headerstyle wrap="False" />
                    <itemtemplate>
<asp:ImageButton runat="server" Text="Edit" CommandName="Edit" ImageUrl="~/Image/UIPics/Edit.gif" CausesValidation="False" id="ImageButton8"></asp:ImageButton>&nbsp;<asp:ImageButton runat="server" Text="Delete" CommandName="Delete" ImageUrl="~/Image/UIPics/Delete.gif" CausesValidation="False" id="ImageButton9"></asp:ImageButton>
</itemtemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" />
</InfoLight:webgridview>
    </form>
</body>
</html>
