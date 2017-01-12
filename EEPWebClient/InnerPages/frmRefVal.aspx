<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmRefVal.aspx.cs" Inherits="InnerPages_frmRefVal" Theme="InnerPageSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/innerpage/refval.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table id="main_table" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td class="container">
                <table>
                    <tr>
                        <td class="btn_container">
                            <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';" />
                        </td>
                        <td class="btn_container">
                            <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';" />
                        </td>
                        <td class="btn_container">
                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';" />
                        </td>
                        <td class="btn_container">
                            <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" 
                                CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" 
                                onmouseover="this.className='btn_mouseover';" Visible="False" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="container">
                <InfoLight:WebGridView id="gridView" runat="server" skinid="RefValGrid"
                onselectedindexchanged="gridView_SelectedIndexChanged" 
                onpageindexchanging="gridView_PageIndexChanging" 
                onrowcreated="gridView_RowCreated" CreateInnerNavigator="False">
<PagerSettings Mode="NumericFirstLast"></PagerSettings>
                    <columns>
                        <asp:TemplateField ShowHeader="False">
                            <footertemplate>
                                <asp:ImageButton id="imgBtnOK" onclick="imgBtnOK_Click" runat="server" ImageUrl="~/Image/UIPics/OK.gif"></asp:ImageButton>
                                <asp:ImageButton id="imgBtnCancel" onclick="imgBtnCancel_Click" runat="server" ImageUrl="~/Image/UIPics/Cancel.gif"></asp:ImageButton> 
                            </footertemplate>
                            <itemtemplate>
                                <asp:ImageButton id="ImageButton1" runat="server" CommandName="Select" ImageUrl="~/Image/refval/RefValSelect.gif" CausesValidation="False"></asp:ImageButton>
                            </itemtemplate>
                        </asp:TemplateField>
                    </columns>
                </infolight:WebGridView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
