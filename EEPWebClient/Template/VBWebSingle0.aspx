<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VBWebSingle0.aspx.vb" Inherits="Template_VBWebSingle0" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
        <InfoLight:WebNavigator ID="WebNavigator2" runat="server" BackColor="White"
            BindingObject="WebFormView1" BorderColor="Silver" BorderStyle="Groove" BorderWidth="2px"
            Height="22px" StatusStrip="WebStatusStrip1" Width="100%" 
            ShowDataStyle="FormViewStyle" SkinID="WebNavigatorManagerSkin1">
            <NavStates>
                <InfoLight:WebNavigatorStateItem EnableControls="First;Previous;Next;Last;Add;Update;Delete;Query;Print;Export"
                    StateText="Initial" />
                <InfoLight:WebNavigatorStateItem EnableControls="First;Previous;Next;Last;Add;Update;Delete;Query;Print;Export"
                    StateText="Browsed" />
                <InfoLight:WebNavigatorStateItem EnableControls="OK;Cancel;Apply;Abort" StateText="Inserting" />
                <InfoLight:WebNavigatorStateItem EnableControls="OK;Cancel;Apply;Abort" StateText="Editing" />
                <InfoLight:WebNavigatorStateItem EnableControls="" StateText="Applying" />
                <InfoLight:WebNavigatorStateItem EnableControls="First;Previous;Next;Last;Add;Update;Delete;Apply;Abort;Export"
                    StateText="Changing" />
                <InfoLight:WebNavigatorStateItem EnableControls="" StateText="Querying" />
                <InfoLight:WebNavigatorStateItem EnableControls="" StateText="Printing" />
            </NavStates>
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
                    ControlVisible="True" DisenableImageUrl="../image/uipics/edit3.gif" ImageUrl="../image/uipics/edit.gif"
                    MouseOverImageUrl="../image/uipics/edit2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Delete" ControlText="delete" ControlType="Image"
                    ControlVisible="True" DisenableImageUrl="../image/uipics/delete3.gif" ImageUrl="../image/uipics/delete.gif"
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
        </InfoLight:WebNavigator>
    
    </div>
        <InfoLight:WebFormView ID="WebFormView1" runat="server" 
        DataSourceID="Master" LayOutColNum="2" 
        AllowPaging="True" SkinID="FormViewSkin1">
        </InfoLight:WebFormView>
    </form>
</body>
</html>
