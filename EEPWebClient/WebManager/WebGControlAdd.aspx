<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebGControlAdd.aspx.cs" Inherits="InnerPages_WebGControlAdd" Theme="ControlSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../css/MenuUtility.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <InfoLight:WebDataSource ID="wdsGroup" runat="server" DataMember="groupInfo" WebDataSetID="WGroup"
                AutoApply="True">
            </InfoLight:WebDataSource>
        </div>
        <table style="height: 350px" width="500">
            <tr>
                <td style="vertical-align: top; width: 214px; text-align: left" valign="top">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="vertical-align: top">
                    <InfoLight:WebFormView ID="wfvGroup" runat="server" DataSourceID="wdsGroup" OnDataBound="wfvGroup_DataBound" width="240px"><EditItemTemplate>
<TABLE class="container_table"><TBODY><TR><TD><asp:Label id="CaptionGROUPID" runat="server" Width="100px" Text="GROUPID:" __designer:wfdid="w226"></asp:Label> </TD><TD><asp:TextBox id="GROUPIDTextBox" runat="server" Text='<%# Bind("GROUPID") %>' __designer:wfdid="w227">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionGROUPNAME" runat="server" Width="100px" Text="GROUPNAME:" __designer:wfdid="w228"></asp:Label> </TD><TD><asp:TextBox id="GROUPNAMETextBox" runat="server" Text='<%# Bind("GROUPNAME") %>' __designer:wfdid="w229">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionDESCRIPTION" runat="server" Width="100px" Text="DESCRIPTION:" __designer:wfdid="w230"></asp:Label> </TD><TD><asp:TextBox id="DESCRIPTIONTextBox" runat="server" Text='<%# Bind("DESCRIPTION") %>' __designer:wfdid="w231">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionMSAD" runat="server" Width="100px" Text="MSAD:" __designer:wfdid="w232"></asp:Label> </TD><TD><asp:TextBox id="MSADTextBox" runat="server" Text='<%# Bind("MSAD") %>' __designer:wfdid="w233">
                                        </asp:TextBox> </TD></TR></TBODY></TABLE>
</EditItemTemplate>
<InsertItemTemplate>
<TABLE class="container_table"><TBODY><TR><TD><asp:Label id="CaptionGROUPID" runat="server" Width="100px" Text="GROUPID:" __designer:wfdid="w234"></asp:Label> </TD><TD><asp:TextBox id="GROUPIDTextBox" runat="server" Text='<%# Bind("GROUPID") %>' __designer:wfdid="w235">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionGROUPNAME" runat="server" Width="100px" Text="GROUPNAME:" __designer:wfdid="w236"></asp:Label> </TD><TD><asp:TextBox id="GROUPNAMETextBox" runat="server" Text='<%# Bind("GROUPNAME") %>' __designer:wfdid="w237">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionDESCRIPTION" runat="server" Width="100px" Text="DESCRIPTION:" __designer:wfdid="w238"></asp:Label> </TD><TD><asp:TextBox id="DESCRIPTIONTextBox" runat="server" Text='<%# Bind("DESCRIPTION") %>' __designer:wfdid="w239">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionMSAD" runat="server" Width="100px" Text="MSAD:" __designer:wfdid="w240"></asp:Label> </TD><TD><asp:TextBox id="MSADTextBox" runat="server" Text='<%# Bind("MSAD") %>' __designer:wfdid="w241">
                                        </asp:TextBox> </TD></TR></TBODY></TABLE>
</InsertItemTemplate>
<ItemTemplate>
<TABLE class="container_table"><TBODY><TR><TD><asp:Label id="CaptionGROUPID" runat="server" Width="100px" Text="GROUPID:" __designer:wfdid="w218"></asp:Label> </TD><TD><asp:Label id="GROUPIDLabel" runat="server" Text='<%# Bind("GROUPID") %>' __designer:wfdid="w219"></asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionGROUPNAME" runat="server" Width="100px" Text="GROUPNAME:" __designer:wfdid="w220"></asp:Label> </TD><TD><asp:Label id="GROUPNAMELabel" runat="server" Text='<%# Bind("GROUPNAME") %>' __designer:wfdid="w221"></asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionDESCRIPTION" runat="server" Width="100px" Text="DESCRIPTION:" __designer:wfdid="w222"></asp:Label> </TD><TD><asp:Label id="DESCRIPTIONLabel" runat="server" Text='<%# Bind("DESCRIPTION") %>' __designer:wfdid="w223">
                                        </asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionMSAD" runat="server" Width="100px" Text="MSAD:" __designer:wfdid="w224"></asp:Label> </TD><TD><asp:Label id="MSADLabel" runat="server" Text='<%# Bind("MSAD") %>' __designer:wfdid="w225"></asp:Label> </TD></TR></TBODY></TABLE>
</ItemTemplate>
</InfoLight:WebFormView>
                    <InfoLight:WebTranslate ID="wtGroup" runat="server" BindingObject="wfvGroup" DataSourceID="wdsGroup"
                        OnOKClick="wtGroup_OKClick">
                    </InfoLight:WebTranslate>
                            </td>                           
                            <td style="vertical-align: top" rowspan="1">
                             <asp:CheckBoxList ID="cblUser" runat="server" RepeatColumns="2" Width="250px">
                    </asp:CheckBoxList><asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"
                        Text="Save" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';"
                        onmouseover="this.className='btn_mouseover';" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; width: 214px; text-align: left" valign="top">
                    <iframe id="accessFrame" runat="server" frameborder="0" height="350" scrolling="auto" src="WebAccessMenu.aspx"
                        width="500"></iframe>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
