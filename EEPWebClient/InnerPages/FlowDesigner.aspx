<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FlowDesigner.aspx.cs" Inherits="InnerPages_FlowDesigner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language="javascript" type="text/javascript">
        function showPos()
        {
            alert("x:" + event.offsetX + ", y:" + event.offsetY + ", width:" + event.srcElement.width + ",height:" + event.srcElement.height);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <%--<img alt="no picture" src="../Image/WorkFlow2.jpg" onclick="showPos()" />--%>
        <asp:ImageMap ID="imageMapDesigner" runat="server">
            <%--<asp:RectangleHotSpot HotSpotMode="Navigate" NavigateUrl="" PostBackValue="Start" Left="330" Top="71" Bottom="111" Right="460"/>--%>
        </asp:ImageMap>
    </form>
</body>
</html>
