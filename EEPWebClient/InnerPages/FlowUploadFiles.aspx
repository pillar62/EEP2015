<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FlowUploadFiles.aspx.cs"
    Inherits="InnerPages_FlowUploadFiles" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>flow files upload</title>
    <link href="../css/innerpage/flowuploadfiles.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        //        function addFiles(files) {
        //            var s = "";
        //            if (files && files != "") {
        //                s = encodeURI(";" + files);
        //            }
        //            
        //            var opener_url = window.opener.document.location.href;
        //            var i = opener_url.indexOf('&uploadParam1=');
        //            if (i != -1) {
        //                opener_url = opener_url.substring(0, i);
        //            }
        //            var this_url = window.document.location.href;
        //            var j = this_url.indexOf('&ATTACHMENTS');
        //            var k = this_url.indexOf('?');
        //            var backParam = '';
        //            if (j != -1) {
        //                backParam = '&' + this_url.substring(k + 1, j);
        //            }
        //            else {
        //                backParam = '&' + this_url.substring(k + 1);
        //            }
        //            
        //            alert(backParam);

        //            if (opener_url.indexOf('&ATTACHMENTS=') != -1) {
        //                var attachIndex = opener_url.indexOf('ATTACHMENTS=');
        //                var part1 = opener_url.substring(0, attachIndex);
        //                var part2 = opener_url.substring(attachIndex);
        //                if (part2.indexOf('&') != -1) {
        //                    var replaceAttachments = part2.substring(0, part2.indexOf('&'));
        //                    var part3 = part2.substring(part2.indexOf('&'));

        //                    window.opener.location.href = part1 + replaceAttachments + s + part3 + backParam;
        //                }
        //                else {
        //                    window.opener.location.href = opener_url + s + backParam;
        //                }
        //            }
        //            else {
        //                window.opener.location.href = opener_url + '&ATTACHMENTS=' + s + backParam;
        //            }
        //            window.close();
        //        }

        //        function filterfiles(files) {
        //            var opener_url = window.opener.document.location.href;
        //            var oriFiles = request(opener_url, 'ATTACHMENTS');
        //            if (oriFiles && oriFiles != "") {
        //                opener_url = opener_url.replace(oriFiles, files);
        //                window.opener.document.location.href = opener_url;
        //            }
        //            window.close();
        //        }


        function setfiles(files) {
            var this_url = window.document.location.href;
            var paramIndex = this_url.indexOf('&uploadParam1=');
            var param = this_url.substring(paramIndex);

            var opener_url = window.opener.document.location.href;
            if (opener_url.indexOf('&ATTACHMENTS=') != -1) {
                var attachIndex = opener_url.indexOf('ATTACHMENTS=');
                var part1 = opener_url.substring(0, attachIndex);
                var part2 = opener_url.substring(attachIndex);
                if (part2.indexOf('&') != -1) {
                    //var replaceAttachments = part2.substring(0, part2.indexOf('&'));
                    var part3 = part2.substring(part2.indexOf('&'));
                    if (part3.indexOf('&uploadParam1=') == 0) {
                        part3 = '';
                    }
                    else if (part3.indexOf('&uploadParam1=') > 0) {
                        part3 = part3.substring(0, part3.indexOf('&uploadParam1='));
                    }

                    window.opener.location.href = part1 + '&ATTACHMENTS=' + files + part3 + param;
                }
                else {
                    window.opener.location.href = part1 + '&ATTACHMENTS=' + files + param;
                }
            }
            else {
                window.opener.location.href = opener_url + '&ATTACHMENTS=' + files + param;
            }
            window.close();
        }

        function request(url, paras) {
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

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

    <fieldset>
        <legend class="color_legend">add files </legend>
        <table class="tabcontainer">
            <tr>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="FileUpload2" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="FileUpload3" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="FileUpload4" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="FileUpload5" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnUpload" runat="server" Text="upload" OnClick="btnUpload_Click"
                        CssClass="btn_rect_mouseout" onmouseout="this.className='btn_rect_mouseout';"
                        onmouseover="this.className='btn_rect_mouseover';" />
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend class="color_legend">delete files </legend>
        <div class="tabcontainer">
            <asp:UpdatePanel ID="delContainer" runat="server" UpdateMode="Conditional" 
                onload="delContainer_Load">
            </asp:UpdatePanel>
            <asp:Button ID="btnDelete" runat="server" Text="delete" CssClass="btn_rect_mouseout"
                onmouseout="this.className='btn_rect_mouseout';" onmouseover="this.className='btn_rect_mouseover';"
                OnClick="btnDelete_Click" />
        </div>
    </fieldset>

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

    </form>
</body>
</html>
