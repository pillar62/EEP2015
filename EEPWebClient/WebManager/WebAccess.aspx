<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebAccess.aspx.cs" Inherits="WebAccessGroup"
    Theme="ControlSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Access Setting</title>
    <link href="../css/MenuUtility.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="MenuCotent" width="280">
                <tr>
                    <td id="MenuContent" rowspan="3">
                        <asp:Label ID="Label1" runat="server" Text="Menu ID:"></asp:Label>
                        <asp:Label ID="labelMenuID" runat="server" Text="Label"></asp:Label>
                        <infolight:webdatasource id="wdsGroup" runat="server" datamember="sqlMGroups" webdatasetid="WGroup">
        </infolight:webdatasource>
                        <asp:CheckBoxList ID="cbGroup" runat="server" DataSourceID="wdsGroup" DataTextField="GROUPNAME"
                            DataValueField="GROUPID" RepeatColumns="2">
                        </asp:CheckBoxList>
                        <infolight:webdatasource id="wdsUser" runat="server" datamember="userInfo" webdatasetid="WUser">
        </infolight:webdatasource>
                        <asp:CheckBoxList ID="cbUser" runat="server" DataSourceID="wdsUser" DataTextField="USERNAME"
                            DataValueField="USERID" OnDataBound="cbUser_DataBound">
                        </asp:CheckBoxList>
                        &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td rowspan="1">
                        <table style="position: relative">
                            <tr>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text="OK" OnClick="btnOK_Click" CssClass="btn_mouseout"
                                        onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';"
                                        Width="60px" /><asp:Button ID="Button2" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                            CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';"
                                            Width="60px" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
