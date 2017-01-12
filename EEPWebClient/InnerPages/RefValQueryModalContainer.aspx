<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RefValQueryModalContainer.aspx.cs" Inherits="InnerPages_RefValQueryModalContainer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>refval query</title>
        <script id="clientEventHandlersJS" language="javascript">
            <!--
            function IFRAME1_onblur() {
            }
            //-->
		</script>
</head>
<body style="background-image: url(../Image/query/Query.bmp)">
    <form id="form1"  method="post" runat="server">
			<iframe frameborder="no" src='frmRefValQuery.aspx' id="IFRAME1" width="100%" height = "100%"
				language="javascript" onblur="return IFRAME1_onblur()"></iframe>
    </form>
</body>
</html>
