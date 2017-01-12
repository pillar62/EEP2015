<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Upload.aspx.cs" Inherits="InnerPages_Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../js/themes/default/easyui.css" rel="stylesheet" />
    <link href="../js/themes/icon.css" rel="stylesheet" />
    <script src="../js/jquery-1.8.0.min.js"></script>
    <script src="../js/jquery.easyui.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding: 10px;"> <input type="file" id="importFileUpload" name="importFileUpload" style="width:250px"/>
        <a class="easyui-linkbutton" href="javascript:void(0)" style="vertical-align: top;"
                        onclick="importGrid()">Upload</a>
    </div>
    </form>
</body>
</html>
