<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebUControl.aspx.cs" Inherits="InnerPages_WebUControl"
    Theme="ControlSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/MenuUtility.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <infolight:webdatasource id="wdsUser" runat="server" datamember="userInfo" webdatasetid="WUser"
                    autoapply="True">
            </infolight:webdatasource>
            </div>
            <table id="TABLE1" style="width: 100%" cellpadding="0" cellspacing="0">
                <tr height="50%">
                    <td align="left" colspan="1" rowspan="1" style="width: 1288px; vertical-align: middle;" id="NavigatorContent">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <infolight:webnavigator id="wnUser" runat="server" bindingobject="wgvUser" oncommand="wnUser_Command"><NavStates>
<InfoLight:WebNavigatorStateItem StateText="Initial" EnableControls="First;Previous;Next;Last;Add;Update;Delete;Query;Print;Export"></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Browsed" EnableControls="First;Previous;Next;Last;Add;Update;Delete;Query;Print;Export"></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Inserting" EnableControls="OK;Cancel;Apply;Abort"></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Editing" EnableControls="OK;Cancel;Apply;Abort"></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Applying" EnableControls=""></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Changing" EnableControls="First;Previous;Next;Last;Add;Update;Delete;Apply;Abort;Export"></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Querying" EnableControls=""></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Printing" EnableControls=""></InfoLight:WebNavigatorStateItem>
</NavStates>
<NavControls>
<InfoLight:ControlItem ControlName="First" ControlText="first" ControlType="Image" ImageUrl="../image/uipics/first.gif" MouseOverImageUrl="../image/uipics/first2.gif" DisenableImageUrl="../image/uipics/first3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Previous" ControlText="previous" ControlType="Image" ImageUrl="../image/uipics/previous.gif" MouseOverImageUrl="../image/uipics/previous2.gif" DisenableImageUrl="../image/uipics/previous3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Next" ControlText="next" ControlType="Image" ImageUrl="../image/uipics/next.gif" MouseOverImageUrl="../image/uipics/next2.gif" DisenableImageUrl="../image/uipics/next3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Last" ControlText="last" ControlType="Image" ImageUrl="../image/uipics/last.gif" MouseOverImageUrl="../image/uipics/last2.gif" DisenableImageUrl="../image/uipics/last3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Add" ControlText="add" ControlType="Image" ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif" DisenableImageUrl="../image/uipics/add3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Query" ControlText="query" ControlType="Image" ImageUrl="../image/uipics/query.gif" MouseOverImageUrl="../image/uipics/query2.gif" DisenableImageUrl="../image/uipics/query3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Export" ControlText="export" ControlType="Image" ImageUrl="../image/uipics/export.gif" MouseOverImageUrl="../image/uipics/export2.gif" DisenableImageUrl="../image/uipics/export3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
</NavControls>
</infolight:webnavigator>
                                </td>
                                <td>
                                    </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr height="50%">
                    <td align="left" colspan="1" dir="ltr" rowspan="1" style="width: 100%" valign="top">
                        <infolight:webgridview id="wgvUser" runat="server" datasourceid="wdsUser" editurl="../WebManager/WebUControlAdd.aspx"
                            backcolor="White" bordercolor="#CCCCCC" borderwidth="1px"
                             onrowdeleted="wgvUser_RowDeleted" openeditleft="250" openedittop="250"
                            openeditwidth="550" createinnernavigator="False" width="100%" skinid="GridViewManagerSkin1">
<FooterStyle BackColor="White" ForeColor="#000066"></FooterStyle>

<HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"></HeaderStyle>

<PagerSettings Mode="NumericFirstLast"></PagerSettings>

<RowStyle ForeColor="#000066"></RowStyle>
<Columns>
<asp:CommandField CancelImageUrl="~/Image/UIPics/Cancel.gif" DeleteImageUrl="~/Image/UIPics/Delete.gif" EditImageUrl="~/Image/UIPics/Edit.gif" SelectImageUrl="~/Image/UIPics/Select.gif" ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" UpdateImageUrl="~/Image/UIPics/OK.gif" ButtonType="Image">
<HeaderStyle Width="10%"></HeaderStyle>

<ItemStyle Wrap="False"></ItemStyle>
</asp:CommandField>
<asp:BoundField DataField="USERID" HeaderText="USERID" SortExpression="USERID">
<HeaderStyle Wrap="False" Width="12%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="USERNAME" HeaderText="USERNAME" SortExpression="USERNAME">
<HeaderStyle Wrap="False" Width="15%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="AGENT" HeaderText="AGENT" SortExpression="AGENT">
<HeaderStyle Wrap="False" Width="5%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="CREATEDATE" HeaderText="CREATEDATE" SortExpression="CREATEDATE">
<HeaderStyle Wrap="False" Width="10%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" SortExpression="DESCRIPTION">
<HeaderStyle Wrap="False" Width="15%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="EMAIL" HeaderText="EMAIL" SortExpression="EMAIL">
<HeaderStyle Wrap="False" Width="18%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="AUTOLOGIN" HeaderText="AUTOLOGIN" SortExpression="AUTOLOGIN">
<HeaderStyle Wrap="False" Width="5%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="MSAD" HeaderText="MSAD" SortExpression="MSAD">
<HeaderStyle Wrap="False" Width="5%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="PWD" HeaderText="PWD" SortExpression="PWD" Visible="False">
<HeaderStyle Wrap="False" Width="5%"></HeaderStyle>
</asp:BoundField>
</Columns>

<SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"></SelectedRowStyle>

<PagerStyle HorizontalAlign="Left" BackColor="White" ForeColor="#000066"></PagerStyle>
</infolight:webgridview>
                    </td>
                </tr>
                <tr height="50%">
                    <td align="left" colspan="1" dir="ltr" rowspan="1" style="width: 100%;"
                        valign="top">
                        &nbsp;<table style="width: 100%;" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Button ID="btnGetADUser" runat="server" OnClick="btnGetADUser_Click" Text="GetADUser"
                                        CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';" Width="90px" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" Text="OK" Width="70px"
                                        Visible="False" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';"
                                        onmouseover="this.className='btn_mouseover';" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                                        Width="70px" Visible="False" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';"
                                        onmouseover="this.className='btn_mouseover';" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
