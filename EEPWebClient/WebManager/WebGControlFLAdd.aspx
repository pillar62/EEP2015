<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebGControlFLAdd.aspx.cs"
    Inherits="InnerPages_WebGControlAdd" Theme="ControlSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Insert/Update/View Group Infomation</title>
    <link href="../css/MenuUtility.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <infolight:webdatasource id="wdsGroup" runat="server" datamember="groupInfo" webdatasetid="WGroup"
                autoapply="True">
            </infolight:webdatasource>
            &nbsp;</div>
        <table style="height: 350px" width="500">
            <tr>
                <td style="vertical-align: top; width: 214px; text-align: left">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="vertical-align: top" rowspan="2">
                    <asp:Button ID="btnAgent" runat="server" OnClick="btnAgent_Click" Text="Agent" CssClass="btn_mouseout"
                        onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';" /><infolight:webformview id="wfvGroup" runat="server" datasourceid="wdsGroup" ondatabound="wfvGroup_DataBound"
                        skinid="FormViewManagerSkin1" width="240px"><EditItemTemplate>
<TABLE class="container_table"><TBODY><TR><TD><asp:Label id="CaptionGROUPID" runat="server" Text="GROUPID:" Width="100px" __designer:wfdid="w198"></asp:Label> </TD><TD><asp:TextBox id="GROUPIDTextBox" runat="server" Text='<%# Bind("GROUPID") %>' __designer:wfdid="w199">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionGROUPNAME" runat="server" Text="GROUPNAME:" Width="100px" __designer:wfdid="w200"></asp:Label> </TD><TD><asp:TextBox id="GROUPNAMETextBox" runat="server" Text='<%# Bind("GROUPNAME") %>' __designer:wfdid="w201">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionDESCRIPTION" runat="server" Text="DESCRIPTION:" Width="100px" __designer:wfdid="w202"></asp:Label> </TD><TD><asp:TextBox id="DESCRIPTIONTextBox" runat="server" Text='<%# Bind("DESCRIPTION") %>' __designer:wfdid="w203">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionMSAD" runat="server" Text="MSAD:" Width="100px" __designer:wfdid="w204"></asp:Label> </TD><TD><asp:TextBox id="MSADTextBox" runat="server" Text='<%# Bind("MSAD") %>' __designer:wfdid="w205">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionISROLE" runat="server" Text="ISROLE:" Width="100px" __designer:wfdid="w206"></asp:Label> </TD><TD><asp:TextBox id="ISROLETextBox" runat="server" Text='<%# Bind("ISROLE") %>' __designer:wfdid="w207">
                                        </asp:TextBox> </TD></TR></TBODY></TABLE>
</EditItemTemplate>
<InsertItemTemplate>
<TABLE class="container_table"><TBODY><TR><TD><asp:Label id="CaptionGROUPID" runat="server" Text="GROUPID:" Width="100px" __designer:wfdid="w208"></asp:Label> </TD><TD><asp:TextBox id="GROUPIDTextBox" runat="server" Text='<%# Bind("GROUPID") %>' __designer:wfdid="w209">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionGROUPNAME" runat="server" Text="GROUPNAME:" Width="100px" __designer:wfdid="w210"></asp:Label> </TD><TD><asp:TextBox id="GROUPNAMETextBox" runat="server" Text='<%# Bind("GROUPNAME") %>' __designer:wfdid="w211">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionDESCRIPTION" runat="server" Text="DESCRIPTION:" Width="100px" __designer:wfdid="w212"></asp:Label> </TD><TD><asp:TextBox id="DESCRIPTIONTextBox" runat="server" Text='<%# Bind("DESCRIPTION") %>' __designer:wfdid="w213">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionMSAD" runat="server" Text="MSAD:" Width="100px" __designer:wfdid="w214"></asp:Label> </TD><TD><asp:TextBox id="MSADTextBox" runat="server" Text='<%# Bind("MSAD") %>' __designer:wfdid="w215">
                                        </asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionISROLE" runat="server" Text="ISROLE:" Width="100px" __designer:wfdid="w216"></asp:Label> </TD><TD><asp:TextBox id="ISROLETextBox" runat="server" Text='<%# Bind("ISROLE") %>' __designer:wfdid="w217">
                                        </asp:TextBox> </TD></TR></TBODY></TABLE>
</InsertItemTemplate>
<ItemTemplate>
<TABLE class="container_table"><TBODY><TR><TD><asp:Label id="CaptionGROUPID" runat="server" Text="GROUPID:" Width="100px" __designer:wfdid="w188"></asp:Label> </TD><TD><asp:Label id="GROUPIDLabel" runat="server" Text='<%# Bind("GROUPID") %>' __designer:wfdid="w189"></asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionGROUPNAME" runat="server" Text="GROUPNAME:" Width="100px" __designer:wfdid="w190"></asp:Label> </TD><TD><asp:Label id="GROUPNAMELabel" runat="server" Text='<%# Bind("GROUPNAME") %>' __designer:wfdid="w191"></asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionDESCRIPTION" runat="server" Text="DESCRIPTION:" Width="100px" __designer:wfdid="w192"></asp:Label> </TD><TD><asp:Label id="DESCRIPTIONLabel" runat="server" Text='<%# Bind("DESCRIPTION") %>' __designer:wfdid="w193">
                                        </asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionMSAD" runat="server" Text="MSAD:" Width="100px" __designer:wfdid="w194"></asp:Label> </TD><TD><asp:Label id="MSADLabel" runat="server" Text='<%# Bind("MSAD") %>' __designer:wfdid="w195"></asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionISROLE" runat="server" Text="ISROLE:" Width="100px" __designer:wfdid="w196"></asp:Label> </TD><TD><asp:Label id="ISROLELabel" runat="server" Text='<%# Bind("ISROLE") %>' __designer:wfdid="w197"></asp:Label> </TD></TR></TBODY>
                                        </TABLE>
</ItemTemplate>
</infolight:webformview>
                    <infolight:webtranslate id="wtGroup" runat="server" bindingobject="wfvGroup" datasourceid="wdsGroup"
                        onokclick="wtGroup_OKClick">
                    </infolight:webtranslate>
                            </td>                            
                            <td style="vertical-align: top" rowspan="2">
                            <asp:CheckBoxList ID="cblUser" runat="server" RepeatColumns="2" Width="250px">
                    </asp:CheckBoxList><asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"
                        Text="Save" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';"
                        onmouseover="this.className='btn_mouseover';" />
                            </td>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; width: 214px; text-align: left">
                    <iframe id="accessFrame" runat="server" frameborder="0" height="350" scrolling="auto" src="WebAccessMenu.aspx"
                        width="500"></iframe>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
