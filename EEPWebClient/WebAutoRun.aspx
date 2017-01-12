<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebAutoRun.aspx.cs" Inherits="WebAutoRun" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 100%; height: 100%">
            <tr height="50%">
                <td align="center" colspan="1" rowspan="1" style="width: 50%; height: 19%">
                </td>
                <td align="center" colspan="1" rowspan="1" style="height: 19%" valign="middle">
                </td>
                <td align="center" colspan="3" rowspan="1" style="width: 50%; height: 19%">
                </td>
            </tr>
            <tr>
                <td align="center" colspan="1" rowspan="1" style="width: 50%; height: 219px">
                </td>
                <td align="center" colspan="1" rowspan="1" style="height: 219px" valign="middle">
                    <table align="center" border="0" cellpadding="0" cellspacing="0" width="341">
                        <tr>
                            <td style="width: 423px; height: 9px">
                                <table align="center" border="0" cellpadding="0" cellspacing="0" style="height: 1px"
                                    width="341">
                                    <tr>
                                        <td style="height: 9px">
                                            <img height="8" src="Image/login/33.jpg" width="420" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td background="Image/login/2.jpg" style="width: 423px">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" style="width: 9%; height: 187px">
                                        </td>
                                        <td style="width: 91%; height: 187px">
                                            <table align="center" border="0" cellpadding="0" cellspacing="0" width="83%">
                                                <tr>
                                                    <td align="left" class="unnamed1" style="height: 23px; width: 110px;">
                                                        <font color="#006699" face="Arial, Helvetica, sans-serif"><strong>
        <asp:Label ID="labelChooseXML" runat="server" Text="Choose XML: " Font-Bold="True" Font-Size="X-Small" ForeColor="SlateBlue" Height="9px" Width="130px"></asp:Label></strong></font></td>
                                                    <td align="left" colspan="2" style="height: 23px">
        <asp:TextBox ID="txtChooseXML" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="unnamed1" style="height: 23px; width: 110px;">
                                                        <font color="#006699" face="Arial, Helvetica, sans-serif"><strong>
        <asp:Label ID="lblUsers" runat="server" Text="Users: " Font-Bold="True" Font-Size="X-Small" ForeColor="SlateBlue"></asp:Label></strong></font></td>
                                                    <td align="left" colspan="2" style="height: 23px">
        <asp:TextBox ID="txtUser" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="unnamed1" style="width: 110px">
        <asp:Label ID="lblModules" runat="server" Text="Packages: " Font-Bold="True" Font-Size="X-Small" ForeColor="SlateBlue"></asp:Label></td>
                                                    <td align="left" colspan="2">
        <asp:TextBox ID="txtPackage" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="unnamed1" style="height: 23px; width: 110px;">
        <asp:Label ID="lblInterval" runat="server" Text="Interval:" Font-Bold="True" Font-Size="X-Small" ForeColor="SlateBlue"></asp:Label></td>
                                                    <td align="left" colspan="2" style="font-family: Times New Roman; height: 23px">
        <asp:TextBox ID="txtInterval" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr style="font-family: Times New Roman">
                                                    <td align="left" class="unnamed1" style="width: 110px; height: 21px">
        <asp:Label ID="lblLog" runat="server" Text="Log: " Font-Bold="True" Font-Size="X-Small" ForeColor="SlateBlue"></asp:Label></td>
                                                    <td align="left" colspan="2" style="height: 21px">
        <asp:TextBox ID="txtLog" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td class="unnamed1" rowspan="2" style="width: 110px">
                                                        &nbsp;</td>
                                                    <td colspan="2">
                                                        <div align="left" class="unnamed1">
                                                            <font color="#ffffff">1</font>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 22px" width="56%">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 21px; height: 28px">
                                                                </td>
                                                                <td align="center" style="width: 60px; height: 28px">
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Go" Font-Bold="False" Width="60px" /></td>
                                                                <td align="center" style="width: 60px; height: 28px">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="height: 22px" width="21%">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table align="center" border="0" cellpadding="0" cellspacing="0" width="341">
                                    <tr>
                                        <td style="height: 9px">
                                            <img height="9" src="Image/login/3.jpg" width="420" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="center" colspan="3" rowspan="1" style="width: 50%; height: 219px">
                </td>
            </tr>
            <tr height="50%">
                <td align="center" colspan="1" rowspan="1" style="width: 50%; height: 18%">
                </td>
                <td align="center" colspan="1" rowspan="1" style="height: 18%" valign="middle">
                </td>
                <td align="center" colspan="3" rowspan="1" style="width: 50%; height: 18%">
                </td>
            </tr>
        </table>
        &nbsp; &nbsp;&nbsp;<br />
        &nbsp;
        <br />
        </div>
    </form>
</body>
</html>
