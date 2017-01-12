<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FlowComment.aspx.cs" Inherits="InnerPages_FlowComment"
    Theme="InnerPageSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Flow Comment</title>
    <link href="../css/innerpage/flowcomment.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <%--<object id="WebBrowser" classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2" height="0" width="0"></object>--%>
    <div id="main_content">
        <asp:FormView ID="fvHis" runat="server" Width="100%">
            <ItemTemplate>
                <table class="formview">
                    <tr>
                        <td class="left_align">
                            <b>
                                <%= this.getIndexTitle(0) %>
                                :</b>
                            <%# Eval("FLOW_DESC") %>
                        </td>
                        <td class="left_align">
                            <b>
                                <%= this.getIndexTitle(4) %>
                                :</b>
                            <%# Eval("FORM_PRESENT_CT")%>
                        </td>
                        <td class="left_align">
                            <b>
                                <%= this.getIndexTitle(1) %>
                                :</b>
                            <%# Eval("UPDATE_DATE")%>
                            <%#Eval("UPDATE_TIME")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="left_align">
                            <b>
                                <%= this.getIndexTitle(2) %>
                                :</b>
                            <%# Eval("USER_ID")%>(<%# Eval("USERNAME") %>)
                        </td>
                        <td class="left_align">
                            <b>
                                <%= this.getIndexTitle(3) %>
                                :</b>
                            <%# Eval("S_ROLE_ID")%>(<%# Eval("GROUPNAME") %>)
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:FormView>
        <asp:GridView ID="gdvHis" SkinID="FlowCommentGrid" runat="server" OnPageIndexChanging="gdvHis_PageIndexChanging"
            OnRowDataBound="gdvHis_RowDataBound" Width="100%">
            <Columns>
                <asp:BoundField DataField="S_STEP_ID" HeaderText="S_STEP_ID">
                    <ItemStyle Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="USER_ID" HeaderText="USER_ID">
                    <ItemStyle Width="55px" />
                </asp:BoundField>
                <asp:BoundField DataField="USERNAME" HeaderText="USERNAME">
                    <ItemStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="STATUS" HeaderText="STATUS">
                    <ItemStyle Width="50px" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="REMARK">
                    <ItemTemplate>
                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" BackColor="Transparent" 
                            Text='<%# Eval("REMARK") %>' BorderStyle="None" ReadOnly="True" Width="100%"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ATTACHMENTS">
                    <ItemStyle Width="100px" />
                </asp:TemplateField>
                <asp:BoundField DataField="UPDATE_DATE" HeaderText="UPDATE_DATE">
                    <ItemStyle Width="75px" />
                </asp:BoundField>
                <asp:BoundField DataField="UPDATE_TIME" HeaderText="UPDATE_TIME">
                    <ItemStyle Width="50px" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:Panel ID="panDownload" runat="server">
        </asp:Panel>
        <img src="../Image/UIPics/Print.gif" alt="print" onclick="window.print();" />
    </div>
    </form>
</body>
</html>
