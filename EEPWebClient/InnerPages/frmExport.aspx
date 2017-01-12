<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmExport.aspx.cs" Inherits="InnerPages_frmExport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DownLoad</title>
    <link href="../css/innerpage/export.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id="main_table" cellpadding="0" cellspacing="0">
            <tr>
                <td id="td1">
                    Excel File
                </td>
            </tr>
            <tr>
                <td id="td2">
                    <asp:HyperLink ID="HyperLinkDownload" runat="server">Click to Download</asp:HyperLink>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
