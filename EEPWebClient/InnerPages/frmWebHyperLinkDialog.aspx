<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmWebHyperLinkDialog.aspx.cs" Inherits="InnerPages_frmWebHyperLinkDialog" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
    
<body>
    <form id="form1" runat="server">
    <div>
      <iframe id=page name=page height="100%" width="100%") ></iframe>

<script type="text/jscript">
document.all.page.src=pagepath;
</script>

    </div>
    </form>
</body>
</html>
