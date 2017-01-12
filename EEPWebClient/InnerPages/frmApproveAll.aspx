<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmApproveAll.aspx.cs" Inherits="InnerPages_frmApproveAll"
    Theme="InnerPageSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SAConfirm</title>
    <link href="../css/innerpage/flowsubmitconfirm.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function upload_click() {
            var important = document.getElementById('chkImportant').checked;
            var urgent = document.getElementById('chkUrgent').checked;
            var suggest = encodeURI(document.getElementById('txtSuggest').value);
            var role = -1, org = -1, rstep = -1;
            if (document.getElementById('ddlRoles'))
                role = document.getElementById('ddlRoles').selectedIndex;
            if (document.getElementById('ddlOrg'))
                org = document.getElementById('ddlOrg').selectedIndex;
            if (document.getElementById('ddlReturnStep'))
                rstep = document.getElementById('ddlReturnStep').selectedIndex;
            window.open('FlowUploadFiles.aspx?uploadParam1=' + important + '&uploadParam2=' + urgent + '&uploadParam3=' + suggest + '&uploadParam4=' + role + '&uploadParam5=' + org + '&uploadParam6=' + rstep + '&ATTACHMENTS=' + request('ATTACHMENTS') + '&VDSNAME=' + request('VDSNAME'), '', 'width=600px,height=500px,top=200,left=200,toolbar=no,status=yes,scrollbars,resizable');
        }

        function request(paras) {
            var url = window.location.href;
            var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
            var paraObj = {}
            for (i = 0; j = paraString[i]; i++) {
                paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
            }
            var returnValue = paraObj[paras.toLowerCase()];
            if (typeof (returnValue) == "undefined") {
                return "";
            } else {
                return returnValue;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="main_container">
        <table cellpadding="0px" border="0px" cellspacing="0px" width="100%">
            <tr>
                <td class="left-top">
                </td>
                <td class="top">
                </td>
                <td class="right-top">
                </td>
            </tr>
            <tr>
                <td class="left">
                </td>
                <td class="center">
                    <fieldset class="leftalign_fieldset">
                        <legend>
                            <%= this.getHtmlText(0) %>
                        </legend>
                        <asp:CheckBox ID="chkImportant" runat="server" Visible="False" />
                        <br />
                        <asp:CheckBox ID="chkUrgent" runat="server" Visible="False" />
                        <div>
                            <infolight:webmultiviewcaptions id="mvCaption" runat="server" tablestyle="Style5"
                                multiviewid="MultiView" font-size="12pt" ontabchanging="mvCaption_TabChanging">
                                <captions>
                        <InfoLight:WebMultiViewCaption />
                    </captions>
                            </infolight:webmultiviewcaptions>
                            <asp:MultiView ID="MultiView" runat="server">
                                <asp:View ID="suggestionView" runat="server">
                                    <br />
                                    <table class="full_width">
                                        <tr>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtSuggest" runat="server" TextMode="MultiLine" Width="100%" Height="70px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="part_container">
                                                <asp:Label ID="lblRole" runat="server" />
                                                <asp:DropDownList ID="ddlRoles" runat="server" />
                                            </td>
                                            <td class="part_container">
                                                <asp:Label ID="lblOrg" runat="server" />
                                                <asp:DropDownList ID="ddlOrg" runat="server" />
                                            </td>
                                            <td class="part_container">
                                                <asp:Label ID="lblReturnStep" runat="server" />
                                                <asp:DropDownList ID="ddlReturnStep" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="panDownload" runat="server">
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="historyView" runat="server">
                                    <asp:GridView ID="gdvHis" runat="server" SkinID="FlowSubmitConfirmGrid" OnPageIndexChanging="gdvHis_PageIndexChanging"
                                        OnRowDataBound="gdvHis_RowDataBound" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="S_STEP_ID">
                                                <ItemStyle Width="50px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="USER_ID">
                                                <ItemStyle Width="55px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="USERNAME">
                                                <ItemStyle Width="60px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="STATUS">
                                                <ItemStyle Width="50px" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" BackColor="Transparent"
                                                        Text='<%# Eval("REMARK") %>' BorderStyle="None" ReadOnly="True" Width="100px"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="UPDATE_DATE">
                                                <ItemStyle Width="75px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UPDATE_TIME">
                                                <ItemStyle Width="50px" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:View>
                                <asp:View ID="PreviewView" runat="server">
                                    <asp:GridView ID="gdvPreview" runat="server" SkinID="FlowSubmitConfirmGrid">
                                    </asp:GridView>
                                </asp:View>
                            </asp:MultiView>
                        </div>
                        <div class="button_container">
                            &nbsp;<asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" CssClass="btn_rect_mouseout"
                                onmouseout="this.className='btn_rect_mouseout';" onmouseover="this.className='btn_rect_mouseover';" />&nbsp;</div>
                        <div class="button_container">
                            &nbsp;<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn_rect_mouseout"
                                onmouseout="this.className='btn_rect_mouseout';" onmouseover="this.className='btn_rect_mouseover';" /></div>
                        <div class="button_container">
                            &nbsp;<asp:Button ID="btnPreview" runat="server" OnClick="btnPreview_Click" CssClass="btn_rect_mouseout"
                                onmouseout="this.className='btn_rect_mouseout';" onmouseover="this.className='btn_rect_mouseover';"
                                Visible="False" /></div>
                        <div class="button_container">
                            <asp:Button ID="upload" runat="server" Text="upload files" CssClass="btn_upload_rect_mouseout"
                                onmouseout="this.className='btn_upload_rect_mouseout';" onmouseover="this.className='btn_upload_rect_mouseover';"
                                OnClientClick="upload_click()" Visible="False" />
                            <%--<input id="upload" type="button" value="upload files" class="btn_upload_rect_mouseout"
                    onmouseout="this.className='btn_upload_rect_mouseout';" onmouseover="this.className='btn_upload_rect_mouseover';"
                    onclick="window.open('FlowUploadFiles.aspx', '', 'width=500px,height=200px,toolbar=no,status=yes,scrollbars,resizable');" />--%>
                        </div>
                    </fieldset>
                    <asp:Panel ID="panResult" runat="server" CssClass="full_width" Visible="false">
                        <fieldset id="resultContainer" class="rightalign_fieldset">
                            <legend>
                                <%= this.getHtmlText(8) %>
                            </legend>
                            <table class="full_width">
                                <tr>
                                    <td id="td1">
                                        <asp:Label ID="send" runat="server" SkinID="FullWidthMessageLabel" CssClass="left_align"></asp:Label>
                                    </td>
                                    <td id="td2">
                                        <asp:Label ID="result" runat="server" SkinID="FullWidthMessageLabel" CssClass="left_align"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn_rect_mouseout"
                                            onmouseout="this.className='btn_rect_mouseout';" onmouseover="this.className='btn_rect_mouseover';" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                </td>
                <td class="right">
                </td>
            </tr>
            <tr>
                <td class="left-bottom">
                </td>
                <td class="bottom">
                </td>
                <td class="right-bottom">
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
