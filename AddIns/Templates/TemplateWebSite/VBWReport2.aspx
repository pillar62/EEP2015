<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VBWReport2.aspx.vb" Inherits="VBWReport2" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
        </CR:CrystalReportSource>
        <InfoLight:WebDataSource ID="WebDataSource1" runat="server" WebDataSetID="WData">
        </InfoLight:WebDataSource>
        <table style="width: 519px">
            <tr>
                <td style="width: 163px; height: 28px">
                    <span>&nbsp;日期: </span><span style="background-color: #ff0066"></span>
                </td>
                <td style="width: 169px; height: 28px">
                    <InfoLight:WebDateTimePicker ID="WebDateTimePicker1" runat="server"></InfoLight:WebDateTimePicker></td>
                <td style="width: 163px; height: 28px">
                    <span style="background-color: #ffffff">&nbsp;日期: </span><span style="background-color: #ff0066">
                    </span>
                </td>
                <td style="width: 163px; height: 28px">
                    <InfoLight:WebDateTimePicker ID="WebDateTimePicker2" runat="server"></InfoLight:WebDateTimePicker></td>
                <td style="width: 326px; height: 28px">
                    <asp:Button ID="Button1" runat="server" Height="26px" OnClick="Button1_Click" Text="OK"
                        Width="78px" /></td>
            </tr>
        </table>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
            DisplayGroupTree="False" DisplayPage="False" />
    
    </div>
    </form>
</body>
</html>
