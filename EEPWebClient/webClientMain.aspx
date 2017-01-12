<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webClientMain.aspx.cs" Inherits="webClientMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>EEP 2012</title>
    <link href="css/ClientMain2012.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="methods2.js"></script>

    <script language="javascript" type="text/javascript" src="prototype-1.4.0.js"></script>

    <script language="javascript" type="text/javascript">
        function openFaverMenu() {
            window.open("WebFavorMenu.aspx", "_blank", "height=330,width=530,top=200,left=200,toolbar=no,status=no,resizable=yes,scrollable=yes,location=no");
        }

        function btnMouseOver() {
            var btn = document.getElementById('ibGo');
            btn.style.backgroundImage = "url('Image/Main2012/go_over.png')";
            btn.style.backgroundRepeat = "no-repeat";
        }

        function btnMouseOut() {
            var btn = document.getElementById('ibGo');
            btn.style.backgroundImage = "url('Image/Main2012/go_normal.png')";
            btn.style.backgroundRepeat = "no-repeat";
        }
    </script>

</head>
<body onload="mainPageload()" onbeforeunload="RunOnBeforeUnload()">
    <form id="form1" runat="server">
        <div>
            <table border="0" cellpadding="0" cellspacing="0" class="fullWidthTable">
                <tr>
                    <td id="headerLogo_bg">
                        <div id="headerLogo">
                            <div id="headerLogo_right">
                            </div>
                        </div>
                    </td>
                </tr>
                <%--<tr>
                <td style="height: 45px;">
                    <asp:Silverlight ID="Xaml1" runat="server" Source="~/ClientBin/SilverlightTools.xap"
                        CssClass="slPlugin" MinimumVersion="2.0.30523" Width="100%" Windowless="true"
                        PluginBackground="Transparent" />
                </td>
            </tr>--%>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="fullWidthTable">
                <tr>
                    <td id="leftframeContainer">
                        <div id="leftframe" class="leftPart">
                            <table id="refTab" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="3" id="topContent"></td>
                                </tr>
                                <tr>
                                    <td id="middleContent">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="marginContent"></td>
                                                <td>
                                                    <asp:ImageButton ID="ibMyFavor" runat="server" ImageUrl="~/Image/Main2012/favorite_normal.png"
                                                        CssClass="buttonImg" Width="20px" Height="20px"></asp:ImageButton>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCaption" runat="server" CssClass="leftControl"></asp:TextBox>
                                                </td>
                                                <td id="goContainer">
                                                    <asp:LinkButton ID="ibGO" runat="server" OnClick="ibGO_Click" onmouseover="btnMouseOver()"
                                                        onmouseout="btnMouseOut()" Width="100%" Height="27px" />
                                                </td>
                                                <td class="marginContent"></td>
                                            </tr>
                                            <tr>
                                                <td class="marginContent"></td>
                                                <td>
                                                    <img id="imgSolution" src="Image/Main2012/solution.png" alt="solution"
                                                        class="buttonImg" />
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="ddlSolution" runat="server" AutoPostBack="true" CssClass="leftControl"
                                                        OnSelectedIndexChanged="ddlSolution_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="marginContent"></td>
                                            </tr>
                                            <tr>
                                                <td class="marginContent"></td>
                                                <td colspan="3">
                                                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="204px">
                                                        <asp:TreeView ID="tView" runat="server" ImageSet="Simple" NodeIndent="10" ShowLines="True"
                                                            AutoGenerateDataBindings="False" ExpandDepth="1" OnTreeNodePopulate="tView_TreeNodePopulate" Font-Bold="False" ForeColor="#FFFFFF">
                                                            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="#FFFFFF" HorizontalPadding="0px"
                                                                NodeSpacing="0px" VerticalPadding="0px" />
                                                            <SelectedNodeStyle Font-Underline="True" ForeColor="#030303" HorizontalPadding="0px"
                                                                VerticalPadding="0px" />
                                                            <HoverNodeStyle Font-Underline="True" ForeColor="#030303" />
                                                            <ParentNodeStyle Font-Bold="False" />
                                                        </asp:TreeView>
                                                    </asp:Panel>
                                                </td>
                                                <td class="marginContent"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" id="footContent">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td id="marginContent1"></td>
                                                <td id="changePasswordContainer">
                                                    <asp:Button ID="btnChangepassword" runat="server" Text="change password" OnClick="btnChangepassword_Click"
                                                        CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';" />
                                                </td>
                                                <td id="logOutContainer">
                                                    <asp:Button ID="btnLogOut" runat="server" Text="log out" OnClick="btnLogOut_Click"
                                                        CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';" />
                                                </td>
                                                <td id="marginContent2"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td id="split">
                        <img src="Image/Main2012/close.png" alt="hide tree" id="IMG1" onclick="IMG1_onclick(this)" />
                    </td>
                    <td id="mainframe">
                        <iframe id="main" name="main" runat="server" src="DefaultPage.aspx" frameborder="0"
                            marginheight="0" marginwidth="2" width="100%" align="right" height="628"></iframe>
                    </td>
                </tr>
            </table>
            <%--<table border="0" cellpadding="0" cellspacing="0" class="fullWidthTable">
                <tr>
                    <td id="footerLeft"></td>
                    <td id="footerMiddle"></td>
                    <td id="footerRight"></td>
                </tr>
            </table>--%>
        </div>
    </form>
</body>
</html>
