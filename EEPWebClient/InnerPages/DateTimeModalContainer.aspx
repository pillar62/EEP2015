<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DateTimeModalContainer.aspx.cs" Inherits="InnerPages_DateTimeModalContainer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>DateTime Selector</title>
        <script language="javascript">
    function frame_onblur() {}
    </script>
</head>
<body style="background-image: url(../Image/query/Query.bmp)">
    <form id="form1" method="post" runat="server">
        <iframe frameborder="no" src='frmDateTime.aspx' id="IFRAME1" width="100%" height="100%"
            language="javascript" onblur="return frame_onblur()"></iframe>
    </form>
</body>
</html>
