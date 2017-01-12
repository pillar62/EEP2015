<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebUControlAdd.aspx.cs" Inherits="InnerPages_WebUControlAdd"
    Theme="ControlSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Insert/Update/View User Infomation</title>
    <link href="../css/MenuUtility.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <infolight:webdatasource id="wdsUser" runat="server" autoapply="True" datamember="userInfo"
                webdatasetid="WUser">
        </infolight:webdatasource>
            <table cellpadding="0" cellspacing="0">
                <tr style="vertical-align: top;">
                    <td style="vertical-align: top; text-align: left; width: 214px;">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <infolight:webformview id="wfvUser" runat="server" datasourceid="wdsUser" ondatabound="wfvUser_DataBound"
                                        skinid="FormViewManagerSkin1" width="240px"><EditItemTemplate>
<TABLE class="container_table"><TBODY><TR><TD><asp:Label id="CaptionUSERID" runat="server" Width="100px" Text="USERID:" __designer:wfdid="w170"></asp:Label> </TD><TD><asp:TextBox id="USERIDTextBox" runat="server" Width="150px" Text='<%# Bind("USERID") %>' __designer:wfdid="w171"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionUSERNAME" runat="server" Width="100px" Text="USERNAME:" __designer:wfdid="w172"></asp:Label> </TD><TD><asp:TextBox id="USERNAMETextBox" runat="server" Width="150px" Text='<%# Bind("USERNAME") %>' __designer:wfdid="w173"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionAGENT" runat="server" Width="100px" Text="AGENT:" __designer:wfdid="w174"></asp:Label> </TD><TD><asp:TextBox id="AGENTTextBox" runat="server" Width="150px" Text='<%# Bind("AGENT") %>' __designer:wfdid="w175"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionPWD" runat="server" Width="100px" Text="PWD:" __designer:wfdid="w176"></asp:Label> </TD><TD><asp:TextBox id="PWDTextBox" runat="server" Width="150px" Text='<%# Bind("PWD") %>' __designer:wfdid="w177" TextMode="Password"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionCREATEDATE" runat="server" Width="100px" Text="CREATEDATE:" __designer:wfdid="w178"></asp:Label> </TD><TD><asp:TextBox id="CREATEDATETextBox" runat="server" Width="150px" Text='<%# Bind("CREATEDATE") %>' __designer:wfdid="w179"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionDESCRIPTION" runat="server" Width="100px" Text="DESCRIPTION:" __designer:wfdid="w180"></asp:Label> </TD><TD><asp:TextBox id="DESCRIPTIONTextBox" runat="server" Width="150px" Text='<%# Bind("DESCRIPTION") %>' __designer:wfdid="w181"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionEMAIL" runat="server" Width="100px" Text="EMAIL:" __designer:wfdid="w182"></asp:Label> </TD><TD><asp:TextBox id="EMAILTextBox" runat="server" Width="150px" Text='<%# Bind("EMAIL") %>' __designer:wfdid="w183"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionAUTOLOGIN" runat="server" Width="100px" Text="AUTOLOGIN:" __designer:wfdid="w184"></asp:Label> </TD><TD><asp:TextBox id="AUTOLOGINTextBox" runat="server" Width="150px" Text='<%# Bind("AUTOLOGIN") %>' __designer:wfdid="w185"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionMSAD" runat="server" Width="100px" Text="MSAD:" __designer:wfdid="w186"></asp:Label> </TD><TD><asp:TextBox id="MSADTextBox" runat="server" Width="150px" Text='<%# Bind("MSAD") %>' __designer:wfdid="w187"></asp:TextBox> </TD></TR></TBODY><TBODY><TR><TD colSpan=2><STRONG><SPAN style="COLOR: #ff0000">*修改時，若不輸入密碼，表示默認無密碼</SPAN></STRONG></TD></TR></TBODY></TABLE>
</EditItemTemplate>
<InsertItemTemplate>
<TABLE class="container_table"><TBODY><TR><TD><asp:Label id="CaptionUSERID" runat="server" Width="100px" Text="USERID:" __designer:wfdid="w80"></asp:Label> </TD><TD><asp:TextBox id="USERIDTextBox" runat="server" Width="150px" Text='<%# Bind("USERID") %>' __designer:wfdid="w81"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionUSERNAME" runat="server" Width="100px" Text="USERNAME:" __designer:wfdid="w82"></asp:Label> </TD><TD><asp:TextBox id="USERNAMETextBox" runat="server" Width="150px" Text='<%# Bind("USERNAME") %>' __designer:wfdid="w83"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionAGENT" runat="server" Width="100px" Text="AGENT:" __designer:wfdid="w84"></asp:Label> </TD><TD><asp:TextBox id="AGENTTextBox" runat="server" Width="150px" Text='<%# Bind("AGENT") %>' __designer:wfdid="w85"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionPWD" runat="server" Width="100px" Text="PWD:" __designer:wfdid="w86"></asp:Label> </TD><TD><asp:TextBox id="PWDTextBox" runat="server" Width="150px" Text='<%# Bind("PWD") %>' __designer:wfdid="w87" TextMode="Password"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionCREATEDATE" runat="server" Width="100px" Text="CREATEDATE:" __designer:wfdid="w88"></asp:Label> </TD><TD><asp:TextBox id="CREATEDATETextBox" runat="server" Width="150px" Text='<%# Bind("CREATEDATE") %>' __designer:wfdid="w89"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionDESCRIPTION" runat="server" Width="100px" Text="DESCRIPTION:" __designer:wfdid="w90"></asp:Label> </TD><TD><asp:TextBox id="DESCRIPTIONTextBox" runat="server" Text='<%# Bind("DESCRIPTION") %>' __designer:wfdid="w91"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionEMAIL" runat="server" Width="100px" Text="EMAIL:" __designer:wfdid="w92"></asp:Label> </TD><TD><asp:TextBox id="EMAILTextBox" runat="server" Width="150px" Text='<%# Bind("EMAIL") %>' __designer:wfdid="w93"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionAUTOLOGIN" runat="server" Width="100px" Text="AUTOLOGIN:" __designer:wfdid="w94"></asp:Label> </TD><TD><asp:TextBox id="AUTOLOGINTextBox" runat="server" Width="150px" Text='<%# Bind("AUTOLOGIN") %>' __designer:wfdid="w95"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="CaptionMSAD" runat="server" Width="100px" Text="MSAD:" __designer:wfdid="w96"></asp:Label> </TD><TD><asp:TextBox id="MSADTextBox" runat="server" Width="150px" Text='<%# Bind("MSAD") %>' __designer:wfdid="w97"></asp:TextBox> </TD></TR></TBODY></TABLE>
</InsertItemTemplate>
<ItemTemplate>
<TABLE class="container_table"><TBODY><TR><TD><asp:Label id="CaptionUSERID" runat="server" Width="100px" Text="USERID:" __designer:wfdid="w19"></asp:Label> </TD><TD><asp:Label id="USERIDLabel" runat="server" Text='<%# Bind("USERID") %>' __designer:wfdid="w20"></asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionUSERNAME" runat="server" Width="100px" Text="USERNAME:" __designer:wfdid="w21"></asp:Label> </TD><TD><asp:Label id="USERNAMELabel" runat="server" Text='<%# Bind("USERNAME") %>' __designer:wfdid="w22"></asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionAGENT" runat="server" Width="100px" Text="AGENT:" __designer:wfdid="w23"></asp:Label> </TD><TD><asp:Label id="AGENTLabel" runat="server" Text='<%# Bind("AGENT") %>' __designer:wfdid="w24"></asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionCREATEDATE" runat="server" Width="100px" Text="CREATEDATE:" __designer:wfdid="w25"></asp:Label> </TD><TD><asp:Label id="CREATEDATELabel" runat="server" Text='<%# Bind("CREATEDATE") %>' __designer:wfdid="w26"></asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionDESCRIPTION" runat="server" Width="100px" Text="DESCRIPTION:" __designer:wfdid="w27"></asp:Label> </TD><TD><asp:Label id="DESCRIPTIONLabel" runat="server" Text='<%# Bind("DESCRIPTION") %>' __designer:wfdid="w28"></asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionEMAIL" runat="server" Width="100px" Text="EMAIL:" __designer:wfdid="w29"></asp:Label> </TD><TD><asp:Label id="EMAILLabel" runat="server" Text='<%# Bind("EMAIL") %>' __designer:wfdid="w30"></asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionAUTOLOGIN" runat="server" Width="100px" Text="AUTOLOGIN:" __designer:wfdid="w31"></asp:Label> </TD><TD><asp:Label id="AUTOLOGINLabel" runat="server" Text='<%# Bind("AUTOLOGIN") %>' __designer:wfdid="w32"></asp:Label> </TD></TR><TR><TD><asp:Label id="CaptionMSAD" runat="server" Width="100px" Text="MSAD:" __designer:wfdid="w33"></asp:Label> </TD><TD><asp:Label id="MSADLabel" runat="server" Text='<%# Bind("MSAD") %>' __designer:wfdid="w34"></asp:Label> </TD></TR></TBODY></TABLE>
</ItemTemplate>
</infolight:webformview>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <infolight:webtranslate id="wtUser" runat="server" bindingobject="wfvUser" datasourceid="wdsUser"
                                        detaildatasourceid="" onokclick="wtUser_OKClick">
        </infolight:webtranslate>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="vertical-align: top">
                    <td style="vertical-align: top; width: 214px; text-align: left">
                        <iframe id="accessFrame" runat="server" frameborder="0" src="WebAccessMenu.aspx" height="350px"
                            width="500" scrolling=""></iframe>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
