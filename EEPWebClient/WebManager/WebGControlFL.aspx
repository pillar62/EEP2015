<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebGControlFL.aspx.cs" Inherits="InnerPages_WebGControl" Theme="ControlSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/MenuUtility.css" rel="stylesheet" type="text/css" />
</head>
<body style="width: 100%; height: 500px">
    <form id="form1" runat="server">
        <div>
            <InfoLight:WebDataSource ID="wdsGroup" runat="server" DataMember="groupInfo" WebDataSetID="WGroup"
                AutoApply="True">
            </InfoLight:WebDataSource>
        </div>
        <table style="width: 100%;" id="TABLE1" cellpadding="0" cellspacing="0">
            <tr height="50%">
                <td align="left" colspan="1" rowspan="1" style="width: 1288px" id="NavigatorContent">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <InfoLight:WebNavigator ID="wnGroup" runat="server" BindingObject="wgvGroup" OnCommand="wnGroup_Command">
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
                                        <InfoLight:ControlItem ControlName="Query" ControlText="query" ControlType="Image"
                                            ControlVisible="True" DisenableImageUrl="../image/uipics/query3.gif" ImageUrl="../image/uipics/query.gif"
                                            MouseOverImageUrl="../image/uipics/query2.gif" Size="25" />
                                        <InfoLight:ControlItem ControlName="Export" ControlText="export" ControlType="Image"
                                            ControlVisible="True" DisenableImageUrl="../image/uipics/export3.gif" ImageUrl="../image/uipics/export.gif"
                                            MouseOverImageUrl="../image/uipics/export2.gif" Size="25" />
                                    </NavControls>
                                </InfoLight:WebNavigator>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr height="50%">
                <td align="left" colspan="1" rowspan="1" dir="ltr" valign="top" style="width: 1288px">
                    <InfoLight:WebGridView ID="wgvGroup" runat="server" BackColor="White" BorderColor="#CCCCCC"
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="wdsGroup"
                        EditURL="../WebManager/WebGControlFLAdd.aspx" Width="100%"
                        OnSelectedIndexChanged="wgvGroup_SelectedIndexChanged" OnRowDeleted="wgvGroup_RowDeleted"
                        OpenEditLeft="250" OpenEditTop="250" OpenEditWidth="550" CreateInnerNavigator="False" skinid="GridViewManagerSkin1">
<FooterStyle BackColor="White" ForeColor="#000066"></FooterStyle>

<HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"></HeaderStyle>

<PagerSettings Mode="NumericFirstLast"></PagerSettings>

<RowStyle ForeColor="#000066"></RowStyle>
<Columns>
<asp:CommandField CancelImageUrl="~/Image/UIPics/Cancel.gif" DeleteImageUrl="~/Image/UIPics/Delete.gif" EditImageUrl="~/Image/UIPics/Edit.gif" SelectImageUrl="~/Image/UIPics/Select.gif" ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" UpdateImageUrl="~/Image/UIPics/OK.gif" ButtonType="Image">
<HeaderStyle Width="16%"></HeaderStyle>

<ItemStyle Wrap="False"></ItemStyle>
</asp:CommandField>
<asp:BoundField DataField="GROUPID" HeaderText="GROUPID" SortExpression="GROUPID">
<HeaderStyle Wrap="False" Width="16%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="GROUPNAME" HeaderText="GROUPNAME" SortExpression="GROUPNAME">
<HeaderStyle Wrap="False" Width="20%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" SortExpression="DESCRIPTION">
<HeaderStyle Wrap="False" Width="28%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="MSAD" HeaderText="MSAD" SortExpression="MSAD">
<HeaderStyle Wrap="False" Width="10%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="ISROLE" HeaderText="ISROLE" SortExpression="ISROLE">
<HeaderStyle Wrap="False" Width="10%"></HeaderStyle>
</asp:BoundField>
</Columns>

<SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"></SelectedRowStyle>

<PagerStyle HorizontalAlign="Left" BackColor="White" ForeColor="#000066"></PagerStyle>
</InfoLight:WebGridView>
                </td>
            </tr>
            <tr height="50%">
                <td align="left" colspan="1" dir="ltr" rowspan="1" style="width: 1288px" valign="top">
                                <asp:Button ID="btnGetADGroup" runat="server" OnClick="btnGetADGroup_Click" Text="GetADGroup" CssClass="btn_mouseout"
                        onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';" Width="90px"/></td>
            </tr>
            <tr height="50%">
                <td align="left" colspan="1" dir="ltr" rowspan="1" style="width: 1288px" valign="top">
                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" Text="OK" Width="70px"
                                    Visible="False" CssClass="btn_mouseout"
                        onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';"/>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                                    Width="70px" Visible="False" CssClass="btn_mouseout"
                        onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
