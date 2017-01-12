<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WQuery1.aspx.cs" Inherits="Template_WSingle" Theme="ControlSkin" StylesheetTheme="ControlSkin"%>

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
        &nbsp;<cc1:AjaxScriptManager ID="AjaxScriptManager1" runat="server">
        </cc1:AjaxScriptManager>
        <asp:Timer ID="Timer1" runat="server" Interval="5000" OnTick="Timer1_Tick">
        </asp:Timer>
        <InfoLight:WebDataSource ID="Master" runat="server" AutoApply="True" WebDataSetID="WMaster" DataMember="">
        </InfoLight:WebDataSource>
        <InfoLight:WebClientQuery ID="WebClientQuery1" runat="server" DataSourceID="Master">
        </InfoLight:WebClientQuery>
        <cc1:ajaxcollapsiblepanel id="AjaxCollapsiblePanel1" runat="server" collapsedtext="琩高兵ン块"
            expandedtext="琩高兵ン块(闽超)" height="84px" width="537px"><asp:Panel id="Panel1" runat="server" Width="648px" Height="86px">
        </asp:Panel> <asp:Button id="Button1" onclick="Button1_Click" runat="server" 
                Width="94px" Text="Query"></asp:Button> <asp:Button id="Button2" onclick="Button2_Click" runat="server" Width="89px" Text="Clear"></asp:Button></cc1:ajaxcollapsiblepanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
            </Triggers>
        </asp:UpdatePanel>
    
    </div>
        <InfoLight:WebGridView ID="WebGridView1" runat="server" 
        CreateInnerNavigator="False" DataSourceID="Master" Width="646px" 
        SkinID="GridViewSkin1">
        </InfoLight:WebGridView>
    </form>
</body>
</html>
