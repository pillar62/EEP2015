<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WMasterDetail1.aspx.cs" Inherits="Template_WMasterDetail1" Theme="ControlSkin" StylesheetTheme="ControlSkin" %>

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
        SkinID="WebNavigatorManagerSkin1">
            <NavControls>
                <InfoLight:ControlItem ControlName="First" ControlText="首筆" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/first.gif" MouseOverImageUrl="../image/uipics/first2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/first3.gif" />
                <InfoLight:ControlItem ControlName="Previous" ControlText="上筆" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/previous.gif" MouseOverImageUrl="../image/uipics/previous2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/previous3.gif" />
                <InfoLight:ControlItem ControlName="Next" ControlText="下筆" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/next.gif" MouseOverImageUrl="../image/uipics/next2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/next3.gif" />
                <InfoLight:ControlItem ControlName="Last" ControlText="末筆" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/last.gif" MouseOverImageUrl="../image/uipics/last2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/next3.gif" />
                <InfoLight:ControlItem ControlName="Add" ControlText="新增" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/add3.gif" />
                <InfoLight:ControlItem ControlName="Update" ControlText="更改" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/edit.gif" MouseOverImageUrl="../image/uipics/edit2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/edit3.gif" />
                <InfoLight:ControlItem ControlName="Delete" ControlText="刪除" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/delete.gif" MouseOverImageUrl="../image/uipics/delete2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/delete3.gif" />
                <InfoLight:ControlItem ControlName="OK" ControlText="確認" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/ok3.gif" />
                <InfoLight:ControlItem ControlName="Cancel" ControlText="取消" ControlType="Image"
                    ControlVisible="True" ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/cancel3.gif" />
                <InfoLight:ControlItem ControlName="Apply" ControlText="存檔" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/apply.gif" MouseOverImageUrl="../image/uipics/apply2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/apply3.gif" />
                <InfoLight:ControlItem ControlName="Abort" ControlText="放棄" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/abort.gif" MouseOverImageUrl="../image/uipics/abort2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/abort3.gif" />
                <InfoLight:ControlItem ControlName="Query" ControlText="查詢" ControlType="Image" ControlVisible="True"
                    ImageUrl="../image/uipics/query.gif" MouseOverImageUrl="../image/uipics/query2.gif"
                    Size="25" DisenableImageUrl="../image/uipics/query3.gif" />
                <InfoLight:ControlItem ControlName="Print" ControlText="打印" ControlType="Image" ControlVisible="True"
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
        <InfoLight:WebFormView ID="wfvMaster" runat="server" 
        OnPageIndexChanged="wfvMaster_PageIndexChanged" AllowPaging="True" 
        DataSourceID="Master" LayOutColNum="2" 
        OnAfterInsertLocate="wfvMaster_AfterInsertLocate" 
        OnCanceled="wfvMaster_Canceled" Height="87px" Width="100%" 
        SkinID="FormViewSkin1">
            <InsertRowStyle BackColor="LightCyan" ForeColor="Blue" />
        </InfoLight:WebFormView>
        <InfoLight:webgridview id="wgvDetail" runat="server" datasourceid="Detail"
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
</InfoLight:webgridview>
    </form>
</body>
</html>
