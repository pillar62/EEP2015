<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RefValModalContainer.aspx.cs"
    Inherits="InnerPages_RefValModalContainer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>set value</title>
     <link href="../css/innerpage/refvalmodalcontainer.css" rel="stylesheet" type="text/css" />

    <script language="javascript">
    function frame_onblur() {}
    </script>

</head>
<body class="container">
    <form id="form1" method="post" runat="server">
        <iframe frameborder="no" src='frmRefVal.aspx' id="IFRAME1" width="100%" height="100%"
            language="javascript" onblur="return frame_onblur()"></iframe>
    </form>
    <script language="javascript">
        var a = window.dialogArguments;
        document.title = a[9];
    </script>
</body>
</html>
