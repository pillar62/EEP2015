<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="InnerPages_Error" %>

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
    <div>
        <div class="easyui-layout" data-options="fit:true" style="width: 500px; height: 320px;
            padding: 5px;">
            <div data-options="region:'center'" style="height: 80px; padding: 10px;">
                <div style="font-weight: bold">
                    Message:
                </div>
                <div class="errormessage">
                </div>
                <div style="text-align: right; vertical-align: bottom">
                    <a class="easyui-linkbutton errorstack-button" href="javascript:void(0)" onclick="showStack();" style="vertical-align: bottom">
                        ShowStack</a>
                </div>
                <div class="errorstack" style="display:none">
                    <div style="font-weight: bold">
                        Call Stack:
                    </div>
                    <div class="errorstackmessage" style="white-space: nowrap;">
                    </div>
                </div>
            </div>
            <%--       <div data-options="region:'center'" style=" padding:10px;white-space:nowrap;" class="errorStack" >
            
            <div style="font-weight: bold">
                Call Stack
            </div>
            <div>
                <textarea class="errorStackMessage" style="height: 120px; width: 450px;" />
            </div>
        </div>--%>
            <div data-options="region:'south',border:false" style="height: 100px; padding: 10px;">
                <div style="font-weight: bold">
                    Description:
                </div>
                <div style="text-align: right;">
                    <textarea class="errordescription" style="height: 50px; width: 375px"></textarea>
                    <a class="easyui-linkbutton" href="javascript:void(0)" style="vertical-align: top;"
                        onclick="feedBack()">Feedback</a>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
