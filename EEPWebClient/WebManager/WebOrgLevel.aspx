<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebOrgLevel.aspx.cs" Inherits="WebManager_WebOrgLevel"
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
            <infolight:webdatasource id="WDSOrgLevel" runat="server" datamember="cmdOrgLevel"
                webdatasetid="wOrgLevel" autoapply="True">
        </infolight:webdatasource>
            <infolight:webdatasource id="WDSOrgKind" runat="server" datamember="cmdOrgKind" webdatasetid="wOrgKind">
        </infolight:webdatasource>
            <infolight:webmultiviewcaptions id="WebMultiViewCaptions1" runat="server" multiviewid="MultiView1">
            <captions>
                <InfoLight:WebMultiViewCaption Caption="OrgLevel" />
                <InfoLight:WebMultiViewCaption Caption="OrgKind" />
            </captions>
        </infolight:webmultiviewcaptions>
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="View1" runat="server">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="NavigatorContent">
                                <infolight:webnavigator id="navOrgLevel" runat="server" anyqueryid="navOrgLevel"
                                    bindingobject="WebGridView1">
                    <navstates>
                        <InfoLight:WebNavigatorStateItem 
                    EnableControls="First;Previous;Next;Last;Add;Update;Delete;Query;Print;Export" 
                    StateText="Initial" />
                        <InfoLight:WebNavigatorStateItem 
                    EnableControls="First;Previous;Next;Last;Add;Update;Delete;Query;Print;Export" 
                    StateText="Browsed" />
                        <InfoLight:WebNavigatorStateItem EnableControls="OK;Cancel;Apply;Abort" 
                    StateText="Inserting" />
                        <InfoLight:WebNavigatorStateItem EnableControls="OK;Cancel;Apply;Abort" 
                    StateText="Editing" />
                        <InfoLight:WebNavigatorStateItem EnableControls="" 
                    StateText="Applying" />
                        <InfoLight:WebNavigatorStateItem 
                    EnableControls="First;Previous;Next;Last;Add;Update;Delete;Apply;Abort;Export" 
                    StateText="Changing" />
                        <InfoLight:WebNavigatorStateItem EnableControls="" 
                    StateText="Querying" />
                        <InfoLight:WebNavigatorStateItem EnableControls="" 
                    StateText="Printing" />
                    </navstates>
                    <navcontrols>
                        <InfoLight:ControlItem ControlName="First" ControlText="first" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/first3.gif" 
                    ImageUrl="../image/uipics/first.gif" 
                    MouseOverImageUrl="../image/uipics/first2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Previous" ControlText="previous" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/previous3.gif" 
                    ImageUrl="../image/uipics/previous.gif" 
                    MouseOverImageUrl="../image/uipics/previous2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Next" ControlText="next" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/next3.gif" 
                    ImageUrl="../image/uipics/next.gif" 
                    MouseOverImageUrl="../image/uipics/next2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Last" ControlText="last" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/last3.gif" 
                    ImageUrl="../image/uipics/last.gif" 
                    MouseOverImageUrl="../image/uipics/last2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Add" ControlText="add" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/add3.gif" ImageUrl="../image/uipics/add.gif" 
                    MouseOverImageUrl="../image/uipics/add2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Query" ControlText="query" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/query3.gif" 
                    ImageUrl="../image/uipics/query.gif" 
                    MouseOverImageUrl="../image/uipics/query2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Export" ControlText="export" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/export3.gif" 
                    ImageUrl="../image/uipics/export.gif" 
                    MouseOverImageUrl="../image/uipics/export2.gif" Size="25" />
                    </navcontrols>
                </infolight:webnavigator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <infolight:webgridview id="WebGridView1" runat="server" cellpadding="3" createinnernavigator="False"
                                    datasourceid="WDSOrgLevel" width="780px" backcolor="White" bordercolor="#CCCCCC"
                                    borderstyle="None" borderwidth="1px" skinid="GridViewManagerSkin1">
<FooterStyle BackColor="White" ForeColor="#000066"></FooterStyle>

<HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"></HeaderStyle>

<PagerSettings Mode="NumericFirstLast"></PagerSettings>

<RowStyle ForeColor="#000066"></RowStyle>
<Columns>
<asp:CommandField CancelImageUrl="~/Image/UIPics/Cancel.gif" DeleteImageUrl="~/Image/UIPics/Delete.gif" EditImageUrl="~/Image/UIPics/Edit.gif" SelectImageUrl="~/Image/UIPics/Select.gif" ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" UpdateImageUrl="~/Image/UIPics/OK.gif" ButtonType="Image">
<HeaderStyle Width="15%"></HeaderStyle>
</asp:CommandField>
<asp:BoundField DataField="LEVEL_NO" HeaderText="LEVEL_NO" SortExpression="LEVEL_NO">
<HeaderStyle Wrap="False" Width="20%"></HeaderStyle>

<ItemStyle Wrap="False"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="LEVEL_DESC" HeaderText="LEVEL_DESC" SortExpression="LEVEL_DESC">
<HeaderStyle Wrap="False"></HeaderStyle>

<ItemStyle Wrap="False"></ItemStyle>
</asp:BoundField>
</Columns>

<SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"></SelectedRowStyle>

<PagerStyle HorizontalAlign="Left" BackColor="White" ForeColor="#000066"></PagerStyle>
</infolight:webgridview>
                            </td>
                        </tr>
                    </table>
                    &nbsp;
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="NavigatorContent">
                                <infolight:webnavigator id="navOrgKind" runat="server" anyqueryid="navOrgKind" bindingobject="WebGridView2">
                    <navstates>
                        <InfoLight:WebNavigatorStateItem 
                    EnableControls="First;Previous;Next;Last;Add;Update;Delete;Query;Print;Export" 
                    StateText="Initial" />
                        <InfoLight:WebNavigatorStateItem 
                    EnableControls="First;Previous;Next;Last;Add;Update;Delete;Query;Print;Export" 
                    StateText="Browsed" />
                        <InfoLight:WebNavigatorStateItem EnableControls="OK;Cancel;Apply;Abort" 
                    StateText="Inserting" />
                        <InfoLight:WebNavigatorStateItem EnableControls="OK;Cancel;Apply;Abort" 
                    StateText="Editing" />
                        <InfoLight:WebNavigatorStateItem EnableControls="" 
                    StateText="Applying" />
                        <InfoLight:WebNavigatorStateItem 
                    EnableControls="First;Previous;Next;Last;Add;Update;Delete;Apply;Abort;Export" 
                    StateText="Changing" />
                        <InfoLight:WebNavigatorStateItem EnableControls="" 
                    StateText="Querying" />
                        <InfoLight:WebNavigatorStateItem EnableControls="" 
                    StateText="Printing" />
                    </navstates>
                    <navcontrols>
                        <InfoLight:ControlItem ControlName="First" ControlText="first" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/first3.gif" 
                    ImageUrl="../image/uipics/first.gif" 
                    MouseOverImageUrl="../image/uipics/first2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Previous" ControlText="previous" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/previous3.gif" 
                    ImageUrl="../image/uipics/previous.gif" 
                    MouseOverImageUrl="../image/uipics/previous2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Next" ControlText="next" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/next3.gif" 
                    ImageUrl="../image/uipics/next.gif" 
                    MouseOverImageUrl="../image/uipics/next2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Last" ControlText="last" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/last3.gif" 
                    ImageUrl="../image/uipics/last.gif" 
                    MouseOverImageUrl="../image/uipics/last2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Add" ControlText="add" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/add3.gif" ImageUrl="../image/uipics/add.gif" 
                    MouseOverImageUrl="../image/uipics/add2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Query" ControlText="query" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/query3.gif" 
                    ImageUrl="../image/uipics/query.gif" 
                    MouseOverImageUrl="../image/uipics/query2.gif" Size="25" />
                        <InfoLight:ControlItem ControlName="Export" ControlText="export" 
                    ControlType="Image" ControlVisible="True" 
                    DisenableImageUrl="../image/uipics/export3.gif" 
                    ImageUrl="../image/uipics/export.gif" 
                    MouseOverImageUrl="../image/uipics/export2.gif" Size="25" />
                    </navcontrols>
                </infolight:webnavigator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <infolight:webgridview id="WebGridView2" runat="server" backcolor="White" bordercolor="#CCCCCC"
                                    borderstyle="None" borderwidth="1px" cellpadding="3" createinnernavigator="False"
                                    datasourceid="WDSOrgKind" width="780px" skinid="GridViewManagerSkin1">
<FooterStyle BackColor="White" ForeColor="#000066"></FooterStyle>

<HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"></HeaderStyle>

<PagerSettings Mode="NumericFirstLast"></PagerSettings>

<RowStyle ForeColor="#000066"></RowStyle>
<Columns>
<asp:CommandField CancelImageUrl="~/Image/UIPics/Cancel.gif" DeleteImageUrl="~/Image/UIPics/Delete.gif" EditImageUrl="~/Image/UIPics/Edit.gif" SelectImageUrl="~/Image/UIPics/Select.gif" ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" UpdateImageUrl="~/Image/UIPics/OK.gif" ButtonType="Image">
<HeaderStyle Width="15%"></HeaderStyle>
</asp:CommandField>
<asp:BoundField DataField="ORG_KIND" HeaderText="ORG_KIND" SortExpression="ORG_KIND">
<HeaderStyle Width="20%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="KIND_DESC" HeaderText="KIND_DESC" SortExpression="KIND_DESC"></asp:BoundField>
</Columns>

<SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"></SelectedRowStyle>

<PagerStyle HorizontalAlign="Left" BackColor="White" ForeColor="#000066"></PagerStyle>
</infolight:webgridview>
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </div>
    </form>
</body>
</html>
