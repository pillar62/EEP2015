<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="InnerPages_ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
    <div id="info" style="padding: 10px">
        <table cellspacing="5">
            <tr>
                <td colspan="2">
                    <img src="img/EEP.png" width="307" height="56" />
                </td>
            </tr>
            <tr>
                <td align="right" style="width:150px">
                    <label id="labelUserID">
                        用戶編號:</label>
                </td>
                <td style="width:50%">
                    <input id="userID"  style="width:120px"/>
                </td>
            </tr>
            <tr>
                <td align="right" style="width:50%">
                    <label id="labelOPassword">
                        舊密碼:</label>
                </td>
                <td style="width:50%"> 
                    <input id="oPassword" type="password" style="width:120px"/>
                </td>
            </tr>
            <tr>
                <td align="right" style="width:50%">
                    <label id="labelNPassword">
                        新密碼:</label>
                </td>
                <td style="width:50%">
                    <input id="nPassword" type="password" style="width:120px"/>
                </td>
            </tr>
            <tr>
                <td align="right" style="width:50%">
                    <label id="labelCPassword">
                        確認密碼:</label>
                </td>
                <td style="width:50%">
                     <input id="cPassword" type="password" style="width:120px"/>
                </td>
            </tr>
             <tr>
                <td align="right" colspan="2">
                    <input id="ok" type="button"  value="确定"/>
                </td>
            </tr>
        </table>
        <%-- <div class="easyui-layout" data-options="fit:true" style="width: 300px; height: 230px;
            padding: 5px;">
            <div data-options="region:'center'" style="height: 150px; padding: 10px;">
               <table>
                <tr>
                    <td>
                    <label id="labelUserID">UserID:</label>
                    </td>
                     <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                     <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                     <td>
                    </td>
                </tr>
               </table>
            </div>
            <div data-options="region:'south',border:false" style="height: 50px; padding: 10px;">
                         <div style="text-align: right;">
                    <a class="easyui-linkbutton" href="javascript:void(0)" style="vertical-align: top;"
                        onclick="feedBack()">OK</a>
                    <a class="easyui-linkbutton" href="javascript:void(0)" style="vertical-align: top;"
                        onclick="feedBack()">Cancel</a>
            </div>
        </div>--%>
    </div>
    </form>
</body>
</html>
