<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EEPSingleSignOn.aspx.cs"
    Inherits="InnerPages_EEPSingleSignOn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../js/themes/default/easyui.css" rel="stylesheet" />
    <link href="../js/themes/icon.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="../js/jquery.easyui.min.js"></script>
    <script language="javascript" type="text/javascript">
        function singleSignOn(serviceUrl, redirectUrl, userId, password, dataBase, solution) {
            jQuery.support.cors = true; //jQuery Call to WebService returns “No Transport” error
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: serviceUrl + "/WebService.asmx/SingleSignOn",
                data: "{userId:'" + userId + "',password:'" + password + "',dataBase:'" + dataBase + "',solution:'" + solution + "'}",
                dataType: 'json',
                success: function (result) {
                    if (result.d.length > 0) {
                        window.location.href = serviceUrl + "/SingleSignOn.aspx?PublicKey=" + result.d + "&RedirectUrl=" + redirectUrl;
                    }
                    else {
                        $.messager.alert('Error', 'UserID or password is not correct.', 'error');
                    }
                },
                error: function (data) {
                    $.messager.alert('Error', 'Can not connect to eepwebclient.', 'error');
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    </form>
</body>
</html>
