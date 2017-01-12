<%@ Page Language="C#" AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="InnerPages_User" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../js/themes/default/easyui.css" rel="stylesheet" />
    <link href="../js/themes/icon.css" rel="stylesheet" />
    <script src="../js/jquery-1.9.0.min.js"></script>
    <script src="jquery.easyui.min.1.3.5.js"></script>
    <script src="../js/jquery.json.js" type="text/javascript"></script>
    <script src="../js/jquery.infolight.js"></script>
    <script src="../js/jquery.infolight.security.js"></script>
    <script src="../js/jquery.infolight.extend.js"></script>
    <script src="../js/datagrid-detailview.js"></script>
    <script src="../js/plugins/datagrid-filter.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var div = $("#UserMain");
            var security = $('<div style="width:100%;height:100%"></div>').appendTo(div);
            security.security(
            {
                type: "user",
                onSaveSuccess: function () {
                    //if ($(this).security('options').type == 'solution') {
                    //    $('#comboSolution').combobox('reload');
                    //    $('#comboDatabase').combobox('reload');
                    //}
                }
            });
        });
    </script>
</head>
<body>
    <div id="UserMain">
    </div>
</body>
</html>
